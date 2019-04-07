using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace sdsetup_backend.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class v1 : ControllerBase {
        
        [HttpGet("fetch/zip")]
        public ActionResult FetchZip() {
            //wtf microsoft use a string array
            Microsoft.Extensions.Primitives.StringValues _uuid;
            Microsoft.Extensions.Primitives.StringValues _packageset;
            Microsoft.Extensions.Primitives.StringValues _channel;
            Microsoft.Extensions.Primitives.StringValues _packages;
            Microsoft.Extensions.Primitives.StringValues _client;

            if (!Request.Headers.TryGetValue("SDSETUP-UUID", out _uuid)) return new StatusCodeResult(400);
            if (!Request.Headers.TryGetValue("SDSETUP-PACKAGESET", out _packageset)) return new StatusCodeResult(400);
            if (!Request.Headers.TryGetValue("SDSETUP-CHANNEL", out _channel)) return new StatusCodeResult(400);
            if (!Request.Headers.TryGetValue("SDSETUP-PACKAGES", out _packages)) return new StatusCodeResult(400);
            Request.Headers.TryGetValue("SDSETUP-CLIENT", out _client); // optional, currently only used by homebrew app to specify it only wants SD folder


            string uuid = _uuid[0];
            string packageset = _packageset[0];
            string channel = _channel[0];
            string packages = _packages[0];
            string client = null;
            if (_client.Count > 0 && !String.IsNullOrWhiteSpace(_client[0])) {
                client = _client[0];
            }


            if (Program.uuidLocks.Contains(uuid)) {
                return new ObjectResult("UUID " + uuid + " locked");
            } else if (!Program.validChannels.Contains(channel)) {
                return new ObjectResult("Invalid channel");
            } else if (!Directory.Exists(Program.Files + "/" + packageset)) {
                return new ObjectResult("Invalid packageset");
            } else if (System.IO.File.Exists(Program.Files + "/" + packageset + "/.PRIVILEGED.FLAG") && !Program.IsUuidPriveleged(uuid)) {
                return new ObjectResult("You do not have access to that packageset");
            } else {
                string tempdir = Program.Temp + "/" + uuid;
                try {
                    Program.uuidLocks.Add(uuid);

                    string[] requestedPackages = packages.Split(';');
                    List<KeyValuePair<string, string>> files = new List<KeyValuePair<string, string>>();
                    foreach (string k in requestedPackages) {
                        //sanitize input
                        if (k.Contains("/") || k.Contains("/") || k.Contains("..") || k.Contains("~") || k.Contains("%")) {
                            Program.uuidLocks.Remove(uuid);
                            return new ObjectResult("hackerman");
                        }

                        if (Directory.Exists(Program.Files + "/" + packageset + "/" + k + "/" + channel)) {
                            foreach (string f in EnumerateAllFiles(Program.Files + "/" + packageset + "/" + k + "/" + channel)) {
                                if (client == "hbswitch") {
                                    if (f.StartsWith(Program.Files + "/" + packageset + "/" + k + "/" + channel + "/sd")) {
                                        files.Add(new KeyValuePair<string, string>(f.Replace(Program.Files + "/" + packageset + "/" + k + "/" + channel + "/sd", ""), f));
                                    }
                                } else {
                                    files.Add(new KeyValuePair<string, string>(f.Replace(Program.Files + "/" + packageset + "/" + k + "/" + channel, ""), f));
                                }
                            }
                        }
                    }

                    DeletingFileStream stream = (DeletingFileStream) ZipFromFilestreams(files.ToArray(), uuid);

                    Program.generatedZips[uuid] = stream;
                    stream.Timeout(30000);

                    Program.uuidLocks.Remove(uuid);
                    return new ObjectResult("READY");
                } catch (Exception) {
                    Program.uuidLocks.Remove(uuid);
                    return new ObjectResult("Internal server error occurred");
                }
            }
            
        }

        [HttpGet("fetch/generatedzip/{uuid}")]
        public ActionResult FetchGeneratedZip(string uuid) {
            try {
                if (Program.generatedZips.ContainsKey(uuid)) {
                    Program.generatedZips[uuid].StopTimeout();
                    DeletingFileStream stream = Program.generatedZips[uuid];
                    Program.generatedZips[uuid] = null;
                    Program.generatedZips.Remove(uuid);
                    if (stream == null) {
                        return new ObjectResult("Expired");
                    }
                    string zipname = ("SDSetup(" + DateTime.Now.ToShortDateString() + ").zip").Replace("-", ".").Replace("_", ".");
                    Response.Headers["Content-Disposition"] = "filename=" + zipname;
                    return new FileStreamResult(stream, "application/zip");
                } else {
                    return new ObjectResult("Expired");
                }
            } catch (Exception) {
                Program.generatedZips[uuid] = null;
                return new ObjectResult("Expired");
            }
            
        }

        [HttpGet("fetch/manifest/{uuid}/{packageset}")]
        public ActionResult FetchManifest(string uuid, string packageset) {
            if (!Directory.Exists(Program.Files + "/" + packageset)) {
                return new ObjectResult(packageset);
            } else if (System.IO.File.Exists(Program.Files + "/" + packageset + "/.PRIVILEGED.FLAG") && !Program.IsUuidPriveleged(uuid)) {
                return new ObjectResult("You do not have access to that packageset");
            }

            return new ObjectResult(Program.Manifests[packageset]);
        }

        [HttpGet("fetch/dlstats")]
        public ActionResult GetDownloadStats() {
            //TODO: dont do this
            return new ObjectResult(Program.dlstats.ToDataBinary());
        }

        [HttpGet("fetch/dlstatsdebug")]
        public ActionResult GetDownloadStatsDebug() {
            //TODO: dont do this
            return new ObjectResult(JsonConvert.SerializeObject(DownloadStats.FromDataBinary(Program.dlstats.ToDataBinary()), Formatting.Indented));
        }

        [HttpGet("get/latestpackageset")]
        public ActionResult GetLatestPackageset() {
            return new ObjectResult(Program.latestPackageset);
        }

        [HttpGet("get/latestappversion/switch")]
        public ActionResult GetLatestAppVersion() {
            return new ObjectResult(Program.latestAppVersion);
        }

        [HttpGet("get/latestappdownload/switch")]
        public ActionResult GetLatestAppDownload() {
            string zipname = "sdsetup-switch.nro";
            Response.Headers["Content-Disposition"] = "filename=" + zipname;
            return new FileStreamResult(new FileStream(Program.Config + "/sdsetup-switch.nro", FileMode.Open, FileAccess.Read, FileShare.ReadWrite), "application/octet-stream");
        }

        [HttpGet("set/latestpackageset/{uuid}/{packageset}")]
        public ActionResult SetLatestPackageset(string uuid, string packageset) {
            if (!Program.IsUuidPriveleged(uuid)) return new ObjectResult("UUID not priveleged");
            Program.latestPackageset = packageset;
            return new ObjectResult("Success");
        }

        [HttpGet("admin/reloadall/{uuid}")]
        public ActionResult ReloadEverything(string uuid) {
            if (!Program.IsUuidPriveleged(uuid)) return new ObjectResult("UUID not priveleged");
            return new ObjectResult(Program.ReloadEverything());
        }

        [HttpGet("admin/overrideprivelegeduuid/")]
        public ActionResult OverridePrivelegedUuid(string uuid) {
            if (Program.OverridePrivelegedUuid()) return new ObjectResult("Success");
            return new ObjectResult("Failed");
        }

        [HttpGet("admin/checkuuidstatus/{uuid}")]
        public ActionResult CheckUuidStatus(string uuid) {
            if (Program.IsUuidPriveleged(uuid)) return new ObjectResult("UUID is priveleged");
            return new ObjectResult("UUID not priveleged");
        }

        [HttpGet("admin/setprivelegeduuid/{oldUuid}/{newUuid}")]
        public ActionResult SetPrivelegedUuid(string oldUuid, string newUuid) {

            if (Program.SetPrivelegedUUID(oldUuid, newUuid)) return new ObjectResult("Success");
            else return new ObjectResult("Old UUID invalid");

        }


        public static Stream ZipFromFilestreams(KeyValuePair<string, string>[] files, string uuid) {

            DeletingFileStream outputMemStream = new DeletingFileStream(Program.Temp + "/" + Guid.NewGuid().ToString().Replace("-", "").ToLower(), FileMode.Create, uuid);
            ZipOutputStream zipStream = new ZipOutputStream(outputMemStream);

            zipStream.SetLevel(3); //0-9, 9 being the highest level of compression

            foreach(KeyValuePair<string, string> f in files) {
                ZipEntry newEntry = new ZipEntry(f.Key);
                newEntry.DateTime = DateTime.Now;
                zipStream.PutNextEntry(newEntry);
                FileStream fs = new FileStream(f.Value, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                fs.CopyTo(zipStream, 4096);
                //StreamUtils.Copy(fs, zipStream, new byte[4096]);
                fs.Close();
                zipStream.CloseEntry();
            }
            

            zipStream.IsStreamOwner = false;    // False stops the Close also Closing the underlying stream.
            zipStream.Close();          // Must finish the ZipOutputStream before using outputMemStream.

            outputMemStream.Position = 0;
            
            return outputMemStream;
        }

        private static string[] EnumerateAllFiles(string dir) {
            return Directory.EnumerateFiles(dir, "*", SearchOption.AllDirectories).ToArray();
        }


        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, bool overwriteFiles) {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists) {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName)) {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files) {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, overwriteFiles);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs) {
                foreach (DirectoryInfo subdir in dirs) {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs, overwriteFiles);
                }
            }
        }
    }
}
