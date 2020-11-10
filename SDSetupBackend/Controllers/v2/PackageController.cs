using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SDSetupBackend.Data;
using SDSetupCommon;
using SDSetupCommon.Data;
using SDSetupCommon.Data.Account;
using SDSetupCommon.Data.PackageModels;

namespace SDSetupBackend.Controllers.v2 {
    [ApiController]
    [Route("api/v2/package")]
    public class PackageController : ControllerBase {

        private readonly ILogger<GeneralController> _logger;

        public PackageController(ILogger<GeneralController> logger) {
            _logger = logger;
        }

        [HttpPost("updatenow")]
        public async Task<IActionResult> UpdateNow([FromBody] UpdatePackageModel model) {
            Console.WriteLine("YES!");
            SDSetupUser user = await AuthorizationUtilities.CheckRequestMinAuthorization(Request, SDSetupRole.Administrator);
            if (user == null) return new StatusCodeResult(401); //unauthorized

            string m = model.packageset;
            Package p = model.changedPackage;

            //try {
                bool result = Program.ActiveRuntime.UpdatePackage(m, p);

                if (result) {
                    return new StatusCodeResult(202); //accepted
                } else {
                    return new StatusCodeResult(500); //bad request
                }
            //} catch (Exception) {
            //    return new StatusCodeResult(500); //internal server error
            //}
        }
    }
}
