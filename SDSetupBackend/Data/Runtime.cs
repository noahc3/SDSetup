using System.IO;
using Microsoft.Extensions.Logging;
using SDSetupCommon.Data.PackageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SDSetupCommon.Data.UpdaterModels;
using MongoDB.Driver;

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

        public bool UpdatePackage(string packageset, Package changedPackage) {
            bool result = Manifests[packageset].UpdatePackage(changedPackage);
            
            if (result) {
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

        public string GetWebhookPackage(string webhookId) {
            if (Webhooks.ContainsKey(webhookId)) {
                return Webhooks[webhookId]?.PackageID;
            }
            return null;
        }


    }
}
