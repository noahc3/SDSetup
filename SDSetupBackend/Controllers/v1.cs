/* Copyright (c) 2019 noahc3
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */
using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Http = Microsoft.AspNetCore.Http;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


using SDSetupCommon;


namespace SDSetupBackend.Controllers {
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
                Response.StatusCode = Http.StatusCodes.Status423Locked;
                return new ObjectResult("UUID " + uuid + " locked");
            } else if (!Program.validChannels.Contains(channel)) {
                Response.StatusCode = Http.StatusCodes.Status401Unauthorized;
                return new ObjectResult("Invalid channel");
            } else if (!Directory.Exists((Program.FilesPath + "/" + packageset).AsPath())) {
                Response.StatusCode = Http.StatusCodes.Status400BadRequest;
                return new ObjectResult("Invalid packageset");
            } else if (System.IO.File.Exists((Program.FilesPath + "/" + packageset + "/.PRIVILEGED.FLAG").AsPath()) && !Program.IsUuidPriveleged(uuid)) {
                Response.StatusCode = Http.StatusCodes.Status401Unauthorized;
                return new ObjectResult("You do not have access to that packageset");
            } else {
                string tempdir = (Program.TempPath + "/" + uuid).AsPath();
                try {
                    Program.uuidLocks.Add(uuid);

                    string[] requestedPackages = packages.Split(';');
                    //List<KeyValuePair<string, string>> files = new List<KeyValuePair<string, string>>();
                    OrderedDictionary files = new OrderedDictionary();
                    foreach (string k in requestedPackages) {
                        //sanitize input
                        if (k.Contains("/") || k.Contains("\\") || k.Contains("..") || k.Contains("~") || k.Contains("%")) {
                            Program.uuidLocks.Remove(uuid);
                            Response.StatusCode = Http.StatusCodes.Status401Unauthorized;
                            return new ObjectResult("hackerman");
                        }

                        if (Directory.Exists((Program.FilesPath + "/" + packageset + "/" + k + "/" + channel).AsPath())) {
                            foreach (string f in EnumerateAllFiles((Program.FilesPath + "/" + packageset + "/" + k + "/" + channel).AsPath())) {
                                if (client == "hbswitch") {
                                    if (f.StartsWith((Program.FilesPath + "/" + packageset + "/" + k + "/" + channel + "/sd").AsPath())) {
                                        files[f.Replace((Program.FilesPath + "/" + packageset + "/" + k + "/" + channel + "/sd").AsPath(), "")] = f;
                                    }
                                } else {
                                    files[f.Replace((Program.FilesPath + "/" + packageset + "/" + k + "/" + channel).AsPath(), "")] = f;
                                }
                            }

                            
                        }
                    }

                    ResultFile file = ZipFromFilestreams(files, uuid);

                    Program.generatedZips[uuid] = file;

                    Program.uuidLocks.Remove(uuid);
                    return new ObjectResult("READY");
                } catch (Exception e) {
                    Program.uuidLocks.Remove(uuid);
                    Console.WriteLine(e.Message);
                    Response.StatusCode = Http.StatusCodes.Status500InternalServerError;
                    return new ObjectResult("Internal server error occurred");
                }
            }
            
        }

        [HttpGet("fetch/generatedzip/{uuid}")]
        public ActionResult FetchGeneratedZip(string uuid) {
            try {
                if (Program.generatedZips.ContainsKey(uuid)) {
                    FileStream stream = Program.generatedZips[uuid].GetHandle();
                    if (stream == null) {
                        Program.generatedZips.Remove(uuid);
                        Response.StatusCode = Http.StatusCodes.Status404NotFound;
                        return new ObjectResult("Expired");
                    }
                    string zipname = ("SDSetup(" + DateTime.Now.ToShortDateString() + ").zip").Replace("-", ".").Replace("_", ".").Replace(" ", ".");
                    Response.Headers["Content-Disposition"] = "filename=" + zipname;
                    return new FileStreamResult(stream, "application/zip");
                } else {
                    Response.StatusCode = Http.StatusCodes.Status404NotFound;
                    return new ObjectResult("Expired");
                }
            } catch (Exception e) {
                Program.generatedZips[uuid] = null;
                Response.StatusCode = Http.StatusCodes.Status404NotFound;
                return new ObjectResult("Something went wrong (the zip may have expired): \n\n" + e.Message);
            }
            
        }

        [HttpGet("fetch/manifest/{uuid}/{packageset}")]
        public ActionResult FetchManifest(string uuid, string packageset) {
            if (!Directory.Exists((Program.FilesPath + "/" + packageset).AsPath())) {
                return new ObjectResult(packageset);
            } else if (System.IO.File.Exists((Program.FilesPath + "/" + packageset + "/.PRIVILEGED.FLAG").AsPath()) && !Program.IsUuidPriveleged(uuid)) {
                Response.StatusCode = Http.StatusCodes.Status401Unauthorized;
                return new ObjectResult("You do not have access to that packageset");
            }

            return new ObjectResult(Program.Manifests[packageset]);
        }

        [HttpGet("fetch/dlstats")]
        public ActionResult GetDownloadStats() {
            //TODO: dont do this
            //return new ObjectResult(Program.dlStats.ToDataBinary(U.GetPackageListInLatestPackageset()));
            return null;
        }

        [HttpGet("fetch/dlstatsdebug")]
        public ActionResult GetDownloadStatsDebug() {
            //TODO: dont do this
            //return new ObjectResult(JsonConvert.SerializeObject(DownloadStats.FromDataBinary(Program.dlStats.ToDataBinary(U.GetPackageListInLatestPackageset())), Formatting.Indented));
            return null;
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
            return new FileStreamResult(new FileStream((Program.ConfigPath + "/sdsetup-switch.nro").AsPath(), FileMode.Open, FileAccess.Read, FileShare.ReadWrite), "application/octet-stream");
        }

        [HttpGet("set/latestpackageset/{uuid}/{packageset}")]
        public ActionResult SetLatestPackageset(string uuid, string packageset) {
            if (!Program.IsUuidPriveleged(uuid)) {
                Response.StatusCode = Http.StatusCodes.Status401Unauthorized;
                return new ObjectResult("UUID not priveleged");
            }
            Program.latestPackageset = packageset;
            System.IO.File.WriteAllText(Program.ConfigPath + "/latestpackageset.txt", packageset);
            return new ObjectResult("Success");
        }

        [HttpGet("admin/reloadall/{uuid}")]
        public ActionResult ReloadEverything(string uuid) {
            if (!Program.IsUuidPriveleged(uuid)) {
                Response.StatusCode = Http.StatusCodes.Status401Unauthorized;
                return new ObjectResult("UUID not priveleged");
            }
            return new ObjectResult(Program.ReloadEverything());
        }

        [HttpGet("admin/overrideprivilegeduuid/")]
        public ActionResult OverridePrivilegedUuid(string uuid) {
            if (Program.OverridePrivilegedUuid()) return new ObjectResult("Success");
            Response.StatusCode = Http.StatusCodes.Status500InternalServerError;
            return new ObjectResult("Failed");
        }

        [HttpGet("admin/checkuuidstatus/{uuid}")]
        public ActionResult CheckUuidStatus(string uuid) {
            if (Program.IsUuidPriveleged(uuid)) return new ObjectResult("UUID is priveleged");
            Response.StatusCode = Http.StatusCodes.Status401Unauthorized;
            return new ObjectResult("UUID not priveleged");
        }

        [HttpGet("admin/setprivelegeduuid/{oldUuid}/{newUuid}")]
        public ActionResult SetPrivelegedUuid(string oldUuid, string newUuid) {

            if (Program.SetPrivelegedUUID(oldUuid, newUuid)) return new ObjectResult("Success");
            Response.StatusCode = Http.StatusCodes.Status400BadRequest;
            return new ObjectResult("Old UUID invalid");

        }

        [HttpGet("admin/toggleautoupdates/{uuid}")]
        public ActionResult ToggleAutoUpdates(string uuid) {
            if (!Program.IsUuidPriveleged(uuid)) {
                Response.StatusCode = Http.StatusCodes.Status401Unauthorized;
                return new ObjectResult("UUID not priveleged");
            }
            return new ObjectResult("Success: " + Program.ToggleAutoUpdates());
        }


        public static ResultFile ZipFromFilestreams(OrderedDictionary files, string uuid) {

            FileStream outputMemStream = new FileStream((Program.TempPath + "/" + Guid.NewGuid().ToString().Replace("-", "").ToLower()).AsPath(), FileMode.Create);
            ZipOutputStream zipStream = new ZipOutputStream(outputMemStream);

            zipStream.SetLevel(3); //0-9, 9 being the highest level of compression
            foreach(DictionaryEntry f in files) {
                ZipEntry newEntry = new ZipEntry((string) f.Key);
                newEntry.DateTime = DateTime.Now;
                zipStream.PutNextEntry(newEntry);
                FileStream fs = new FileStream((string) f.Value, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                fs.CopyTo(zipStream, 4096);
                //StreamUtils.Copy(fs, zipStream, new byte[4096]);
                fs.Close();
                zipStream.CloseEntry();
            }
            

            zipStream.IsStreamOwner = false;    // False stops the Close also Closing the underlying stream.
            zipStream.Close();          // Must finish the ZipOutputStream before using outputMemStream.

            outputMemStream.Position = 0;
            ResultFile file = new ResultFile(outputMemStream.Name, uuid, 180000);
            outputMemStream.Close();
            return file;
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
