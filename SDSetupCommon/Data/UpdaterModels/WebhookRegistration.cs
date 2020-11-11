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

        public WebhookTriggerRegistration(string packageset, string packageID, string webhookID) {
            Packageset = packageset;
            PackageID = packageID;
            WebhookID = webhookID;
        }
    }
}
