using SDSetupCommon.Communications;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SDSetupCommon.Data.UpdaterModels {
    public class WebhookTrigger : UpdaterTrigger {

        public string Name { get; set; } = "";

        private string _webhookId { get; set; }

        public string WebhookId {
            get {
                if (_webhookId.NullOrWhiteSpace()) _webhookId = Utilities.CreateCryptographicallySecureGuid().ToCleanString();
                return _webhookId;
            }

            set {
                //Once set it is considered readonly.
                if (_webhookId.NullOrWhiteSpace()) _webhookId = value;
            }
        }
    }
}
