using System.IO;
using Microsoft.Extensions.Logging;
using SDSetupCommon.Data.PackageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SDSetupCommon.Data.UpdaterModels;
using SDSetupCommon;
using System.IO.Enumeration;
using MongoDB.Driver.Core.WireProtocol.Messages.Encoders.JsonEncoders;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Security.Permissions;
using ICSharpCode.SharpZipLib.Zip;
using SDSetupCommon.Data.BundlerModels;
using System.Runtime.ExceptionServices;
using System.Net;

namespace SDSetupBackend.Data {
    //Runtime contains information about currently active variables. During a hot-reload, the active runtime object will be left in place and will
    //be replaced by a new runtime object once the reload is verified successful to prevent problems if the reload fails.
    public class Runtime {
        private string _privilegedUuid;
        public string privilegedUuid { 
            get {
                return _privilegedUuid;
            }
            set {
                Program.logger.LogWarning("A new privileged UUID was set: " + value);
                _privilegedUuid = value;
            }
        }

        public string userDatabasePath = "";
        public string latestPackageSet = "";
        public string latestAppVersion = "";

        public Dictionary<string, Manifest> Manifests = new Dictionary<string, Manifest>();
        private Dictionary<string, WebhookTriggerRegistration> Webhooks = new Dictionary<string, WebhookTriggerRegistration>();
        private Dictionary<string, List<string>> TimedUpdateTriggers = new Dictionary<string, List<string>>();
        private Task ScheduledTimedTask;

        private Dictionary<string, BundlerProgress> BundlerProgresses = new Dictionary<string, BundlerProgress>();
        private Dictionary<string, string> FinishedBundles = new Dictionary<string, string>();

        public bool UpdatePackageMeta(string packageset, Package changedPackage) {
            Package package = Manifests[packageset]?.FindPackageById(changedPackage.ID);
            bool result = Manifests[packageset].UpdatePackage(changedPackage);
            
            if (result) {
                
                //re-register update triggers
                foreach(UpdaterTrigger k in package.AutoUpdateTriggers) {
                    if (k is WebhookTrigger) {
                        DeregisterWebhook((k as WebhookTrigger).WebhookId);
                    } else if (k is TimedScanTrigger) {
                        DeregisterTimedUpdate(packageset, package.ID);
                    }
                }

                package = Manifests[packageset]?.FindPackageById(package.ID);

                foreach (UpdaterTrigger k in package.AutoUpdateTriggers) {
                    if (k is WebhookTrigger) {
                        RegisterWebhook((k as WebhookTrigger).WebhookId, packageset, package.ID);
                    } else if (k is TimedScanTrigger) {
                        RegisterTimedUpdate(packageset, package.ID);
                    }
                }

                File.WriteAllText(changedPackage.GetMetaPath(packageset), JsonConvert.SerializeObject(changedPackage, typeof(Package), new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto }));
            }

            return result;
        }

        public void PurgeStaleBundles() {
            TimeSpan retentionTime = TimeSpan.Parse(Program.ActiveConfig.ZipRetentionTime);
            List<string> deleteKeys = new List<string>();
            Program.logger.LogDebug("Purging stale bundles from temp directory.");
            foreach (string k in BundlerProgresses.Keys) {
                if (BundlerProgresses[k].IsComplete && 
                    DateTime.UtcNow - BundlerProgresses[k].CompletionTime > retentionTime) {
                    try {
                        Program.logger.LogDebug($"Purging bundle {FinishedBundles[k]}.");
                        File.Delete(FinishedBundles[k]);
                        deleteKeys.Add(k);
                    } catch (Exception e) {
                        Program.logger.LogWarning($"Failed to delete zip at {k}, a handle may still be open on the file.");
                    }

                }
            }

            foreach (string k in deleteKeys) {
                BundlerProgresses.Remove(k);
                FinishedBundles.Remove(k);
            }
        }

        public string GetBundlePath(string uuid) {
            string path = null;
            if (FinishedBundles.ContainsKey(uuid)) {
                path = FinishedBundles[uuid];
            }

            return path;
        }

        public BundlerProgress GetBundlerProgress(string uuid) {
            BundlerProgress result = null;
            if (BundlerProgresses.ContainsKey(uuid)) {
                result = BundlerProgresses[uuid].Copy();
            }

            return result;
        }

        //TODO: move to Package::validate
        public void AssertValidPackage(string packageset, string packageID) {
            Package package;
            DirectoryInfo packageDir;

            if (!Manifests.ContainsKey(packageset)) throw new DirectoryNotFoundException($"GetPackageFiles: Packageset {packageset} not found.");

            package = Manifests[packageset].FindPackageById(packageID);
            if (package == null) throw new DirectoryNotFoundException($"GetPackageFiles: Package {packageset}/{packageID} not found.");

            packageDir = new DirectoryInfo($"{Program.ActiveConfig.FilesPath}/{packageset}/{packageID}/{package.VersionInfo.Version}".AsPath());
            if (!packageDir.Exists && package.VersionInfo.Size != 0) throw new DirectoryNotFoundException($"GetPackageFiles: Version desync, {packageset}/{packageID}/{package.VersionInfo.Version} not found.");
        }

        public void BuildBundle(string uuid, string packageset, string[] packages) {
            string zipPath = Program.ActiveConfig.GetTempFilePath();
            List<string> fileList = new List<string>();
            FileStream outputStream = null;
            ZipOutputStream zipStream;
            BundlerProgress progress;
            try {
                progress = new BundlerProgress() {
                    Progress = 0,
                    Total = packages.Length,
                    IsComplete = false,
                    Success = false,
                    CurrentTask = "Getting ready..."
                };
                BundlerProgresses[uuid] = progress;

                outputStream = new FileStream(zipPath, FileMode.Create);
                zipStream = new ZipOutputStream(outputStream);
                zipStream.SetLevel(Program.ActiveConfig.ZipCompressionLevel);

                //TODO: dependency resolution

                foreach(string packageID in packages) {
                    AssertValidPackage(packageset, packageID);
                    string name = Manifests[packageset].FindPackageById(packageID).Name;
                    progress.CurrentTask = $"Adding '{name}' to your bundle...";
                    PutPackageZipEntries(packageset, packageID, zipStream, ref fileList);
                    progress.Progress++;
                }

                zipStream.Close();

                FinishedBundles[uuid] = zipPath;

                Program.logger.LogDebug("Done! " + zipPath);

                progress.CompletionTime = DateTime.UtcNow;
                progress.Success = true;
                progress.IsComplete = true;

            } catch (Exception e) {
                outputStream?.Close();
                BundlerProgresses[uuid] = new BundlerProgress() {
                    Progress = 0,
                    Total = 1,
                    IsComplete = true,
                    Success = false,
                    CurrentTask = "Failed.",
                    CompletionTime = DateTime.UtcNow
                };

                if (!zipPath.NullOrWhiteSpace() && File.Exists(zipPath)) {
                    File.Delete(zipPath);
                }

                //rethrow preserving trace
                ExceptionDispatchInfo.Capture(e).Throw();
                throw; //nop
            }
        }

        public void PutPackageZipEntries(string packageset, string packageID, ZipOutputStream zipStream, ref List<string> fileList) {
            IEnumerable<string> files;
            IEnumerable<string> directories;
            List<ZipEntry> entries = new List<ZipEntry>();
            DirectoryInfo packageDir;
            Package package;

            package = Manifests[packageset].FindPackageById(packageID);
            if (package.VersionInfo.Size == 0) return;
            packageDir = new DirectoryInfo($"{Program.ActiveConfig.FilesPath}/{packageset}/{packageID}/{package.VersionInfo.Version}".AsPath());

            files = Utilities.EnumerateFiles(packageDir.FullName);
            directories = Utilities.EnumerateEmptyDirectories(packageDir.FullName);

            foreach(string k in files) {
                string pathInZip = k.AsPath().Replace(packageDir.FullName.AsPath(), "");
                if (!fileList.Contains(pathInZip)) {
                    ZipEntry newEntry = new ZipEntry(pathInZip);
                    fileList.Add(pathInZip);
                    zipStream.PutNextEntry(newEntry);
                    //TODO: memory caching
                    FileStream fs = new FileStream(k, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    fs.CopyTo(zipStream, 81920);
                    fs.Close();
                    zipStream.CloseEntry();
                }
            }

            foreach(string k in directories) {
                string pathInZip = k.AsPath().Replace(packageDir.FullName.AsPath(), "");
                if (pathInZip.Last() != '/') pathInZip += '/';
                if (!fileList.Contains(pathInZip)) {
                    ZipEntry newEntry = new ZipEntry(pathInZip);
                    fileList.Add(pathInZip);
                    zipStream.PutNextEntry(newEntry);
                    zipStream.CloseEntry();
                }
            }

        }

        public bool RegisterWebhook(string webhookId, string packageSet, string packageId) {
            if (Webhooks.Keys.Contains(webhookId)) return false;

            Webhooks[webhookId] = new WebhookTriggerRegistration(webhookId, packageSet, packageId);
            return true;
        }

        public bool DeregisterWebhook(string webhookId) {
            if (Webhooks.ContainsKey(webhookId)) {
                Webhooks.Remove(webhookId);
                return true;
            } else {
                return false;
            }
        }

        public bool RegisterTimedUpdate(string packageset, string packageId) {
            if (!TimedUpdateTriggers.ContainsKey(packageset)) {
                TimedUpdateTriggers[packageset] = new List<string>();
            }

            if (!TimedUpdateTriggers[packageset].Contains(packageId)) {
                TimedUpdateTriggers[packageset].Add(packageId);
            } else {
                return false;
            }

            return true;
        }

        public bool DeregisterTimedUpdate(string packageset, string packageId) {
            if (!TimedUpdateTriggers.ContainsKey(packageset)) return false;
            if (!TimedUpdateTriggers[packageset].Contains(packageId)) return false;
            
            TimedUpdateTriggers[packageset].Remove(packageId);
            return true;
        }

        public string GetWebhookPackage(string webhookId) {
            if (Webhooks.ContainsKey(webhookId)) {
                return Webhooks[webhookId]?.PackageID;
            }
            return null;
        }

        public void ScheduleTimedTasks() {
            //TODO: implement task cancellation if a currently scheduled task is not complete
            TimeSpan interval = TimeSpan.Parse(Program.ActiveConfig.TimedTasksInterval); ;
            ScheduledTimedTask = Task.Delay(interval).ContinueWith(async o => { await ExecuteTimedTasks(); });
        }

        public async Task ExecuteTimedTasks() {
            Program.logger.LogDebug("Executing timed tasks.");

            await ExecuteTimedAutoUpdates();
            PurgeStaleBundles();

            ScheduleTimedTasks();
        }

        public async Task ExecuteTimedAutoUpdates() {
            Program.logger.LogDebug("Executing timed auto updates.");
            foreach (string packageset in TimedUpdateTriggers.Keys) {
                foreach(string package in TimedUpdateTriggers[packageset]) {
                    Program.logger.LogDebug($"Executing timed auto update for package {packageset}/{package}.");
                    if (await ExecuteAutoUpdate(packageset, package)) {
                        Program.logger.LogDebug($"Timed auto update for package {packageset}/{package} completed successfully.");
                    }
                }
            }
        }

        public async Task<bool> ExecuteAutoUpdate(string packageset, string packageid) {
            Package package;
            bool conditionsPassed = true;
            DirectoryInfo tmpDir;
            string newVersion;
            string targetDirectory;

            if (!Manifests.ContainsKey(packageset)) return false;

            package = Manifests[packageset]?.FindPackageById(packageid);
            if (package == null) 
                return false;
            
            try {
                package = package.Copy();
            } catch (Exception e) {
                Program.logger.LogDebug(e.Message);
            }
           
            foreach (UpdaterCondition k in package.AutoUpdateConditions) {
                if (!await k.Assert(package)) {
                    conditionsPassed = false;
                    break;
                }
            }

            if (!conditionsPassed) return false;

            tmpDir = Program.ActiveConfig.GetTempDirectory();
            Program.logger.LogDebug("Update path: " + tmpDir.FullName);
            try {
                foreach (UpdaterTask k in package.AutoUpdateTasks) {
                    await k.Apply(tmpDir.FullName);
                }
            } catch {
                return false;
            }

            newVersion = await package.AutoUpdateVersionSource.GetVersion();

            package.VersionInfo.Version = newVersion;
            package.VersionInfo.Size = tmpDir.SizeRecursive();

            targetDirectory = $"{Program.ActiveConfig.FilesPath}/{packageset}/{package.ID}/{newVersion}/".AsPath();
            Program.logger.LogDebug("Update path in packageset: " + targetDirectory);

            tmpDir.MoveTo(targetDirectory);

            this.UpdatePackageMeta(packageset, package);

            return true;

        }


    }
}
