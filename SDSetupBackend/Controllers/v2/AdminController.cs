using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SDSetupCommon.Data;
using SDSetupBackend.Providers;
using System.Runtime.InteropServices;
using SDSetupBackend.Data;
using SDSetupCommon.Data.Account;

namespace SDSetupBackend.Controllers {
    [ApiController]
    [Route("api/v2/admin")]
    public class AdminController : ControllerBase {

        private readonly ILogger<GeneralController> _logger;

        public AdminController(ILogger<GeneralController> logger) {
            _logger = logger;
        }

        [HttpPost("populateuuidstatus")]
        public ServerInformation PopulateUUIDStatus([FromBody] ServerInformation info) {
            Console.WriteLine(info.UUID + " / " + Program.ActiveRuntime.privilegedUuid);
            info.PrivilegedUUID = Security.IsUUIDPrivileged(info.UUID);
            return info;
        }

        [HttpGet("systemlogs")]
        public async Task<IActionResult> GetSystemLogs() {
            SDSetupUser user = await AuthorizationUtilities.CheckRequestMinAuthorization(Request, SDSetupRole.Administrator);
            if (user == null) return new StatusCodeResult(401); //unauthorized

            return new ObjectResult(Program.ActiveRuntime.SystemLogs.Values);
        }

        [HttpGet("bundlerlogs/{search}")]
        public async Task<IActionResult> GetBundlerLogs([FromRoute] string search) {
            SDSetupUser user = await AuthorizationUtilities.CheckRequestMinAuthorization(Request, SDSetupRole.Administrator);
            if (user == null) return new StatusCodeResult(401); //unauthorized

            var result = Program.ActiveRuntime.UserBundleLogs.Values.ToList().Where(
                x => x.BundlerUuid.ToLower().Contains(search.ToLower()) || x.ClientUuid.ToLower().Contains(search.ToLower())
            );

            return new ObjectResult(result);
        }


    }
}
