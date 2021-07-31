using System;
using System.Collections.Generic;
using System.Text;

namespace SDSetupCommon.Data.UpdaterModels {
    public class WebhookTriggerRegistration {
        public string Packageset;
        public string PackageID;
        public string WebhookID;

        public WebhookTriggerRegistration() {

        }

        public WebhookTriggerRegistration(string webhookID, string packageset, string packageID) {
            Packageset = packageset;
            PackageID = packageID;
            WebhookID = webhookID;
        }
    }
}
