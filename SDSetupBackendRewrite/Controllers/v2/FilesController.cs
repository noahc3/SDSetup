using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SDSetupCommon;
using SDSetupCommon.Data;

namespace SDSetupBackendRewrite.Controllers.v2 {
    [ApiController]
    [Route("api/v2/files")]
    public class FilesController : ControllerBase {

        private readonly ILogger<GeneralController> _logger;

        public FilesController(ILogger<GeneralController> logger) {
            _logger = logger;
        }

        [HttpGet("latestpackageset")]
        public async Task<IActionResult> LatestPackageSet() {
            return new ObjectResult(Program.ActiveConfig.LatestPackageset);
        }

        [HttpGet("manifest/{packageset}")]
        public async Task<IActionResult> LatestPackageSet(string packageset) {
            if (Program.ActiveRuntime.Manifests.ContainsKey(packageset)) return new JsonResult(Program.ActiveRuntime.Manifests[packageset]);
            else return new StatusCodeResult(404);
        }
    }
}
