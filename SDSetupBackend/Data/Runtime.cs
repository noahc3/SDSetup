using System.IO;
using Microsoft.Extensions.Logging;
using SDSetupCommon.Data.PackageModels;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
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
using SDSetupBackend.Data.Integrations;
using SDSetupCommon.Data.IntegrationModels;

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

        private ConcurrentDictionary<string, BundlerProgress> BundlerProgresses = new ConcurrentDictionary<string, BundlerProgress>();
        private ConcurrentDictionary<string, string> FinishedBundles = new ConcurrentDictionary<string, string>();

        private bool TimedUpdateInProgress = false;
        private ConcurrentQueue<(string, Package)> QueuedPackageInfoUpdates = new ConcurrentQueue<(string, Package)>();

        public bool UpdatePackageInfo(string packageset, Package changedPackage, bool bypassQueue = false) {
            if (!Manifests.ContainsKey(packageset) || !Manifests[packageset].Packages.ContainsKey(changedPackage.ID)) {
                return false;
            } 
            
            if (!bypassQueue && TimedUpdateInProgress) { //if a timed update is in progress, put changes into the queue for later.
                QueuedPackageInfoUpdates.Enqueue((packageset, changedPackage));
                return true;
            } else { //hold ptr to old package info, update pkg info, deregister all of the old update triggers, then register all the new ones with a ptr to the new pkg.
                return UpdatePackageInfo(Manifests[packageset], changedPackage);
            }
        }

        public bool UpdatePackageInfo(Manifest manifest, Package changedPackage) {
            Package package = manifest.FindPackageById(changedPackage.ID);
            bool result = manifest.UpdatePackage(changedPackage);

            if (result) {
                foreach (UpdaterTrigger k in package.AutoUpdateTriggers) {
                    if (k is WebhookTrigger) {
                        DeregisterWebhook((k as WebhookTrigger).WebhookId);
                    } else if (k is TimedScanTrigger) {
                        DeregisterTimedUpdate(manifest.Packageset, package.ID);
                    }
                }

                package = manifest.FindPackageById(changedPackage.ID);

                foreach (UpdaterTrigger k in package.AutoUpdateTriggers) {
                    if (k is WebhookTrigger) {
                        RegisterWebhook((k as WebhookTrigger).WebhookId, manifest.Packageset, package.ID);
                    } else if (k is TimedScanTrigger) {
                        RegisterTimedUpdate(manifest.Packageset, package.ID);
                    }
                }

                File.WriteAllText(changedPackage.GetMetaPath(manifest.Packageset), JsonConvert.SerializeObject(changedPackage, typeof(Package), new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto }));
            }

            return result;
        }
        
        public bool UpdateManifest(Manifest manifest) {
            string packageset = manifest.Packageset;

            if (Manifests.ContainsKey(packageset)) {
                Webhooks.Clear();
                TimedUpdateTriggers.Clear();

                foreach (Package package in manifest.Packages.Values) {
                    foreach (UpdaterTrigger k in package.AutoUpdateTriggers) {
                        if (k is WebhookTrigger) {
                            RegisterWebhook((k as WebhookTrigger).WebhookId, packageset, package.ID);
                        } else if (k is TimedScanTrigger) {
                            RegisterTimedUpdate(packageset, package.ID);
                        }
                    }

                    File.WriteAllText(package.GetMetaPath(packageset), JsonConvert.SerializeObject(package, typeof(Package), new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto }));
                }

                Manifests[packageset] = manifest;

                return true;
            }

            return false;
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
                BundlerProgresses.TryRemove(k, out _);
                FinishedBundles.TryRemove(k, out _);
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
            UpdateDonationInfo();

            ScheduleTimedTasks();
        }

        public void UpdateDonationInfo() {
            PatreonIntegration patreon = null;
            DonationModel donation = new DonationModel();

            Program.logger.LogDebug("Updating donation information.");

            if (!String.IsNullOrWhiteSpace(Program.ActiveConfig.PatreonAccessToken)) {
                patreon = PatreonIntegration.GetPatreonData(Program.ActiveConfig.PatreonAccessToken, Program.ActiveConfig.PatreonCampaignId);
                if (patreon == null) {
                    Program.logger.LogError("Failed to update Patreon donation info during scheduled cycle.");
                    return;
                }
            }

            donation = new DonationModel();
            donation.KofiUrl = Program.ActiveConfig.KofiUrl;
            if (patreon != null) {
                donation.PatreonUrl = patreon.Url;
                donation.PatreonFundingCurrent = patreon.FundingCurrent;
                donation.PatreonFundingGoal = patreon.FundingGoal;
            }

            foreach (Manifest m in Manifests.Values) {
                m.DonationInfo = donation;
            }
        }

        public async Task ExecuteTimedAutoUpdates() {
            Dictionary<string, List<Package>> updatedPackages = new Dictionary<string, List<Package>>();
            Manifest manifest;
            Package package;
            if (!TimedUpdateInProgress) {
                TimedUpdateInProgress = true;
                try {
                    Program.logger.LogDebug("Executing timed auto updates.");

                    foreach (string packageset in TimedUpdateTriggers.Keys) {
                        updatedPackages[packageset] = new List<Package>();
                        manifest = Manifests[packageset];

                        foreach (string packageId in TimedUpdateTriggers[packageset]) {
                            Program.logger.LogDebug($"Executing timed auto update for package {packageset}/{packageId}.");

                            package = manifest.FindPackageById(packageId);
                            package = await ExecuteAutoUpdate(package, packageset);
                            if (package != null) {
                                Program.logger.LogDebug($"Timed auto update for package {packageset}/{package} completed successfully.");
                                updatedPackages[packageset].Add(package);
                            } else {
                                Program.logger.LogDebug($"No updates detected for package {packageset}/{package}.");
                            }
                        }
                    }

                    if (updatedPackages.Count > 0) {
                        Program.logger.LogDebug($"Writing package meta changes to disk.");
                        foreach (string packageset in updatedPackages.Keys) {
                            List<Package> changed = updatedPackages[packageset];
                            if (changed.Count > 0) {
                                Program.logger.LogDebug($"Processing packageset {packageset}");
                                manifest = Manifests[packageset].Copy(); //copy the manifest so we arent making changes on the live manifest.
                                foreach (Package p in changed) {
                                    Program.logger.LogDebug($"Updating package {packageset}/{p.ID}");
                                    UpdatePackageInfo(manifest, p); //reregister triggers and write all changes to disk.
                                }
                                Program.logger.LogDebug($"Pushing updated package information for packageset {packageset} public.");
                                Manifests[packageset] = manifest; //once changes are fully written to disk, push the new manifest live.
                            }
                        }
                    } else {
                        Program.logger.LogDebug("No packages were updated.");
                    }

                } finally {
                    if (!QueuedPackageInfoUpdates.IsEmpty) {
                        Program.logger.LogDebug("Processing queued package info updates.");
                        while (!QueuedPackageInfoUpdates.IsEmpty) {
                            (string, Package) t;

                            if (QueuedPackageInfoUpdates.TryDequeue(out t) && Manifests.ContainsKey(t.Item1)) {
                                string packageset = t.Item1;
                                Package p = t.Item2;
                                Package og = Manifests[packageset].FindPackageById(p.ID);

                                Program.logger.LogDebug($"Updating package info for {packageset}/{p.ID}");

                                if (og != null) {
                                    p.VersionInfo = og.VersionInfo; //maintain new version data.
                                    UpdatePackageInfo(Manifests[packageset], p);
                                }
                            }
                        }
                    }

                    TimedUpdateInProgress = false;

                    Program.logger.LogDebug("Timed auto update process complete.");
                }
            }
        }

        public async Task<Package> ExecuteAutoUpdate(Package package, string packageset) {
            bool conditionsPassed = true;
            DirectoryInfo tmpDir;
            string newVersion;
            string targetDirectory;
                        
            try {
                package = package.Copy();
            } catch (Exception e) {
                Program.logger.LogDebug(e.Message);
                return null;
            }
           
            foreach (UpdaterCondition k in package.AutoUpdateConditions) {
                if (!await k.Assert(package)) {
                    conditionsPassed = false;
                    break;
                }
            }

            if (!conditionsPassed) return null;

            tmpDir = Program.ActiveConfig.GetTempDirectory();
            Program.logger.LogDebug("Update path: " + tmpDir.FullName);
            try {
                foreach (UpdaterTask k in package.AutoUpdateTasks) {
                    await k.Apply(tmpDir.FullName);
                }
            } catch {
                return null;
            }

            newVersion = await package.AutoUpdateVersionSource.GetVersion();

            package.VersionInfo.Version = newVersion;
            package.VersionInfo.Size = tmpDir.SizeRecursive();

            targetDirectory = $"{Program.ActiveConfig.FilesPath}/{packageset}/{package.ID}/{newVersion}/".AsPath();
            Program.logger.LogDebug("Update path in packageset: " + targetDirectory);

            tmpDir.MoveTo(targetDirectory);

            return package;

        }


    }
}
