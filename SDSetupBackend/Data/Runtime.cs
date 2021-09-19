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
using System.Reflection.Metadata;
using System.CodeDom.Compiler;

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
        private ConcurrentDictionary<string, string> PermaBundles = new ConcurrentDictionary<string, string>();

        private bool WebhookUpdateInProgress = false;
        private bool TimedUpdateInProgress = false;
        private bool TimedUpdateQueued = false;
        private ConcurrentQueue<string> QueuedWebhookUpdates = new ConcurrentQueue<string>();
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
            if (manifest == null || changedPackage == null) return false;
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
                if (!BundlerProgresses[k].Permanent &&
                    BundlerProgresses[k].IsComplete && 
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
        public void AssertValidPackage(Manifest manifest, string packageID) {
            Package package;
            DirectoryInfo packageDir;

            package = manifest.FindPackageById(packageID);
            if (package == null) throw new DirectoryNotFoundException($"GetPackageFiles: Package {manifest.Packageset}/{packageID} not found.");

            packageDir = new DirectoryInfo($"{Program.ActiveConfig.FilesPath}/{manifest.Packageset}/{packageID}/{package.VersionInfo.Version}".AsPath());
            if (!packageDir.Exists && package.VersionInfo.Size != 0) throw new DirectoryNotFoundException($"GetPackageFiles: Version desync, {manifest.Packageset}/{packageID}/{package.VersionInfo.Version} not found.");
        }

        public string GetUuidForPermalinkBundle(string packageset, string name) {
            string identifier = $"{packageset}/{name}";
            if (PermaBundles.ContainsKey(identifier)) {
                return PermaBundles[identifier];
            } else return null;
        }

        public async Task BuildPermalinkBundle(Manifest manifest, Bundle bundle) {
            string packageset = manifest.Packageset;
            string identifier = $"{packageset}/{bundle.Name}";
            string uuid = Utilities.CreateCryptographicallySecureGuid().ToCleanString();
            string oldUuid;
            Program.logger.LogInformation($"Generating pre-configured bundle '{identifier}'");

            if (PermaBundles.ContainsKey(identifier)) {
                Program.logger.LogInformation($"Checking if old pre-configured bundle for '{identifier}' is outdated.");
                oldUuid = PermaBundles[identifier];
                if (BundlerProgresses[oldUuid].CompletionTime > manifest.LastUpdated) {
                    //if the manifest is older than this bundle, no need to generate a new bundle.
                    Program.logger.LogInformation($"Pre-configured bundle for '{identifier}' is already up-to-date, will not regenerate.");
                    return;
                }
            }

            Program.logger.LogInformation($"Building pre-configured bundle for '{identifier}' .");
            await BuildBundle(uuid, manifest, bundle.Packages);

            if (FinishedBundles.ContainsKey(uuid)) {
                BundlerProgresses[uuid].Permanent = true;
                if (PermaBundles.ContainsKey(identifier)) {
                    Program.logger.LogInformation($"Marking old pre-configured bundle '{identifier}' for deletion.");
                    oldUuid = PermaBundles[identifier];
                    BundlerProgresses[oldUuid].Permanent = false;
                }
                PermaBundles[identifier] = uuid;
                bundle.PermalinkAvailable = true;
                Program.logger.LogInformation($"Pre-configured bundle '{identifier}' built and live.");
            } else {
                Program.logger.LogError($"Failed to generate pre-configured bundle for {packageset}/{bundle.Name}");
            }

        }

        public bool BuildBundle(string uuid, string packageset, string[] packages) {
            if (Manifests.ContainsKey(packageset)) {
                Task.Run(async () => { await BuildBundle(uuid, Manifests[packageset], packages); });
                return true;
            }

            return false;
        }

        public async Task BuildBundle(string uuid, Manifest manifest, string[] packages) {
            string zipPath = Program.ActiveConfig.GetTempFilePath();
            List<string> fileList = new List<string>();
            FileStream outputStream = null;
            ZipOutputStream zipStream;
            BundlerProgress progress;
            List<Package> resolvedPackages;

            try {
                progress = new BundlerProgress() {
                    Progress = 0,
                    Total = packages.Length,
                    IsComplete = false,
                    Success = false,
                    CurrentTask = "Getting ready..."
                };
                BundlerProgresses[uuid] = progress;

                if (manifest == null) throw new Exception("Packageset not found.");
                manifest = manifest.Copy();

                foreach (string packageID in packages) {
                    AssertValidPackage(manifest, packageID);
                }

                resolvedPackages = new List<Package>();

                foreach(string packageID in packages.ToArray()) {
                    Package p = manifest.FindPackageById(packageID);
                    Package dp;

                    if (!resolvedPackages.Contains(p)) resolvedPackages.Add(p);

                    foreach (string dep in p.Dependencies) {
                        dp = manifest.FindPackageById(dep);
                        if (!resolvedPackages.Contains(dp)) resolvedPackages.Add(dp);
                    }
                }

                resolvedPackages.Sort((a, b) => {
                    return a.Priority.CompareTo(b.Priority);
                });

                outputStream = new FileStream(zipPath, FileMode.Create);
                zipStream = new ZipOutputStream(outputStream);
                zipStream.SetLevel(Program.ActiveConfig.ZipCompressionLevel);                

                foreach(Package p in resolvedPackages) {
                    progress.CurrentTask = $"Adding '{p.Name}' to your bundle...";
                    PutPackageZipEntries(p, manifest.Packageset, zipStream, ref fileList);
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

        public void PutPackageZipEntries(Package package, string packageset, ZipOutputStream zipStream, ref List<string> fileList) {
            IEnumerable<string> files;
            IEnumerable<string> directories;
            List<ZipEntry> entries = new List<ZipEntry>();
            DirectoryInfo packageDir;

            if (package.VersionInfo.Size == 0) return;
            packageDir = new DirectoryInfo($"{Program.ActiveConfig.FilesPath}/{packageset}/{package.ID}/{package.VersionInfo.Version}".AsPath());

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

        public WebhookTriggerRegistration GetWebhookTriggerInfo(string webhookId) {
            if (Webhooks.ContainsKey(webhookId)) {
                return Webhooks[webhookId];
            }
            return null;
        }

        public bool ScheduleWebhookUpdate(string webhookId) {
            if (GetWebhookTriggerInfo(webhookId) == null) return false;

            if (!TimedUpdateInProgress && !WebhookUpdateInProgress) {
                _ = ProcessWebhookUpdate(webhookId); //start this async
            } else {
                QueuedWebhookUpdates.Enqueue(webhookId);
            }

            return true;
        }

        private async Task ProcessQueuedWebhookUpdates() {
            if (!QueuedWebhookUpdates.IsEmpty) {
                Program.logger.LogDebug("Processing webhook update queue.");
                WebhookUpdateInProgress = true;
                string id;
                if (QueuedWebhookUpdates.TryDequeue(out id)) {
                    await ProcessWebhookUpdate(id, false);
                }
                WebhookUpdateInProgress = false;
            }
        }

        private async Task ProcessWebhookUpdate(string webhookId, bool updateState = true) {
            if (updateState) WebhookUpdateInProgress = true;

            WebhookTriggerRegistration w = GetWebhookTriggerInfo(webhookId);

            if (w != null) {
                Package p = Manifests[w.Packageset].FindPackageById(w.PackageID);

                Program.logger.LogDebug($"Processing webhook update for package {w.Packageset}/{w.PackageID}");

                p = await ExecuteAutoUpdate(p, w.Packageset);
                if (p == null) Program.logger.LogDebug($"Webhook update for package {w.Packageset}/{w.PackageID} failed.");
                else {
                    UpdatePackageInfo(Manifests[w.Packageset], p);
                    Program.logger.LogDebug($"Webhook update for package {w.Packageset}/{w.PackageID} complete.");
                }
            }

            if (!QueuedWebhookUpdates.IsEmpty) {
                string id;
                if (QueuedWebhookUpdates.TryDequeue(out id)) {
                    await ProcessWebhookUpdate(id, false);
                }
            }

            if (TimedUpdateQueued) {
                await ExecuteTimedAutoUpdates(true);
            }

            if (updateState) WebhookUpdateInProgress = false;
        }

        public void ScheduleTimedTasks() {
            //TODO: implement task cancellation if a currently scheduled task is not complete
            TimeSpan interval = TimeSpan.Parse(Program.ActiveConfig.TimedTasksInterval); ;
            ScheduledTimedTask = Task.Delay(interval).ContinueWith(async o => { await ExecuteTimedTasks(); });
        }

        public async Task BuildAllPermalinkBundles() {
            List<Task> tasks = new List<Task>();
            foreach(Manifest manifest in Manifests.Values) {
                foreach (Platform p in manifest.Platforms.Values) {
                    foreach (Bundle b in p.Bundles) {
                        tasks.Add(BuildPermalinkBundle(manifest, b));
                    }
                }
            }

            await Task.WhenAll(tasks);
        }

        public async Task ExecuteTimedTasks() {
            Program.logger.LogDebug("Executing timed tasks.");

            await ExecuteTimedAutoUpdates();
            await BuildAllPermalinkBundles();
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

        public async Task ExecuteTimedAutoUpdates(bool ignoreQueue = false) {
            Dictionary<string, List<Package>> updatedPackages = new Dictionary<string, List<Package>>();
            Manifest manifest;
            Package package;

            if (!ignoreQueue && WebhookUpdateInProgress) {
                TimedUpdateQueued = true;
                Program.logger.LogInformation("Timed auto update will be delayed until the current webhook updates complete.");
                return;
                
            } else {
                TimedUpdateQueued = false;
            }

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

                    await ProcessQueuedWebhookUpdates();

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
