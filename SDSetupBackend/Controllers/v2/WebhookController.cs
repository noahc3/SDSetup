using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDSetupBackend.Controllers.v2 {

    [ApiController]
    [Route("api/v2/webhook")]
    public class WebhookController {
        [HttpGet("{webhookId}")]
        public async Task<IActionResult> TriggerWebhook([FromRoute] string webhookId) {
            string package = Program.ActiveRuntime.GetWebhookPackage(webhookId);
            if (String.IsNullOrWhiteSpace(package)) {
                return new StatusCodeResult(404);
            } else {
                return new StatusCodeResult(200);
                //STUB: trigger auto update
            }
        }
    }
}
