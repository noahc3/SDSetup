using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SDSetupCommon;
using SDSetupCommon.Data;
using SDSetupCommon.Data.BundlerModels;

namespace SDSetupBackend.Controllers.v2 {
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
        public async Task<IActionResult> Manifest(string packageset) {
            if (Program.ActiveRuntime.Manifests.ContainsKey(packageset)) return new JsonResult(Program.ActiveRuntime.Manifests[packageset]);
            else return new StatusCodeResult(404);
        }

        [HttpPost("requestbundle")]
        public async Task<IActionResult> RequestBundle([FromBody] BundleRequestModel model) {
            string clientId = model.clientId;
            string packageset = model.packageset;
            string[] packages = model.packages;
            string uuid = Utilities.CreateCryptographicallySecureGuid().ToCleanString();

            //TODO: implement queue
            if (Program.ActiveRuntime.BuildBundle(uuid, clientId, packageset, packages)) {
                return StatusCode(202, uuid); //accepted
            } else {
                return StatusCode(400, "Invalid packageset."); //bad request
            }

            
        }

        [HttpGet("bundleprogress/{uuid}")]
        public async Task<IActionResult> BundleProgress(string uuid) {
            BundlerProgress progress = Program.ActiveRuntime.GetBundlerProgress(uuid);
            if (progress != null) {
                return new ObjectResult(progress);
            } else {
                return StatusCode(404); //not found
            }
        }

        [HttpGet("downloadbundle/{uuid}")]
        public async Task<IActionResult> DownloadBundle(string uuid) {
            string path = Program.ActiveRuntime.GetBundlePath(uuid);
            FileStream stream;
            if (path == null) return StatusCode(404); //not found

            stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            return File(stream, "application/octet-stream", $"sdsetup-{DateTime.UtcNow.ToShortDateString()}.zip");
        }

        [HttpGet("ddl/{packageset}/{name}")]
        public async Task<IActionResult> DirectDownloadBundle(string packageset, string name) {
            string uuid = Program.ActiveRuntime.GetUuidForPermalinkBundle(packageset, name);

            if (String.IsNullOrWhiteSpace(uuid)) return StatusCode(404);
            else return RedirectToAction("DownloadBundle", "Files", new { uuid = uuid });
        }
    }
}
