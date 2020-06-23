using System;
using System.Collections.Generic;
using System.Text;

namespace SDSetupCommon.Communications {
    public class WebhookEndpoints {
        public static string WebhookEndpoint { get { return CommsUtilities.FullApiEndpoint("/api/v2/webhook/{0}"); } } // {0}: Webhook UUID
    }
}
