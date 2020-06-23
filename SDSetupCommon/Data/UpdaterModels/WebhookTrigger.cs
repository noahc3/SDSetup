using SDSetupCommon.Communications;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SDSetupCommon.Data.UpdaterModels {
    public class WebhookTrigger : UpdaterTrigger {

        public string Name { get; set; } = "";

        private string _webhookEndpoint { get; set; }

        public string WebhookEndpoint {
            get {
                if (_webhookEndpoint.NullOrWhiteSpace()) _webhookEndpoint = String.Format(WebhookEndpoints.WebhookEndpoint, Utilities.CreateCryptographicallySecureGuid().ToCleanString());
                return _webhookEndpoint;
            }

            set {
                //Once set it is considered readonly.
                if (_webhookEndpoint.NullOrWhiteSpace()) _webhookEndpoint = value;
            }
        }

        public async Task Register() {
            //STUB: Implement webhook registry
        }
    }
}
