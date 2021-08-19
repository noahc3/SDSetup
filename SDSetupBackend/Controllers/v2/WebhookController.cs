using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDSetupBackend.Controllers.v2 {

    [ApiController]
    [Route("api/v2/webhook")]
    public class WebhookController : ControllerBase {
        [HttpGet("{webhookId}")]
        public async Task<IActionResult> TriggerWebhook([FromRoute] string webhookId) {
            bool result = Program.ActiveRuntime.ScheduleWebhookUpdate(webhookId);
            if (result) return StatusCode(202); //Accepted
            else return StatusCode(404);
        }
    }
}
