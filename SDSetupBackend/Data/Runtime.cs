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
            ScheduleTimedTasks();
        }

        public async Task ExecuteTimedAutoUpdates() {
            Program.logger.LogDebug("Executing timed auto updates.");
            foreach (string packageset in TimedUpdateTriggers.Keys) {
                foreach(string package in TimedUpdateTriggers[packageset]) {
                    Program.logger.LogDebug($"Executing timed auto update for package {packageset}/{package}.");
                    if (await ExecuteAutoUpdate(packageset, package)) {
                        Program.logger.LogDebug($"Timed auto update for package {packageset}/{package} completed successfully.");
                    } else {
                        Program.logger.LogError($"Timed auto update for package {packageset}/{package} failed.");
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

            tmpDir = Utilities.GetTempDirectory();
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
