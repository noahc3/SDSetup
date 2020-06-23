using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SDSetupBackendRewrite.Data;
using SDSetupCommon;
using SDSetupCommon.Data;
using SDSetupCommon.Data.Account;
using SDSetupCommon.Data.PackageModels;

namespace SDSetupBackendRewrite.Controllers.v2 {
    [ApiController]
    [Route("api/v2/package")]
    public class PackageController : ControllerBase {

        private readonly ILogger<GeneralController> _logger;

        public PackageController(ILogger<GeneralController> logger) {
            _logger = logger;
        }

        [HttpPost("updatenow")]
        public async Task<IActionResult> LatestPackageSet([FromBody] UpdatePackageModel model) {
            SDSetupUser user = await AuthorizationUtilities.CheckRequestMinAuthorization(Request, SDSetupRole.Administrator);
            if (user == null) return new StatusCodeResult(401); //unauthorized

            string m = model.packageset;
            Package p = model.newPackage;

            try {
                //TODO: garbage
                if (Program.ActiveRuntime.Manifests[m]
                    .Platforms[p.Platform]
                    .PackageSections[p.Section]
                    .Categories[p.Category]
                    .Subcategories[p.Subcategory]
                    .Packages.ContainsKey(p.ID)) {

                    //TODO: my god its so bad
                    Program.ActiveRuntime.Manifests[m]
                    .Platforms[p.Platform]
                    .PackageSections[p.Section]
                    .Categories[p.Category]
                    .Subcategories[p.Subcategory]
                    .Packages[p.ID] = p;

                    //TODO: standardize this in one location (UpdatePackage method or something)
                    System.IO.File.WriteAllText((Program.ActiveConfig.FilesPath + "/" + m + "/" + p.ID + "/info.json").AsPath(), JsonConvert.SerializeObject(p, Formatting.Indented));

                    return new StatusCodeResult(202); //accepted
                } else {
                    return new StatusCodeResult(400); //bad request
                }
            } catch (Exception) {
                return new StatusCodeResult(400); //bad request
            }
        }
    }
}
