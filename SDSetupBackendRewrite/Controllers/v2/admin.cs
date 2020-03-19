using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SDSetupCommon.Data;
using SDSetupBackendRewrite.Providers;
using System.Runtime.InteropServices;

namespace SDSetupBackendRewrite.Controllers {
    [ApiController]
    [Route("api/v2/admin")]
    public class admin : ControllerBase {

        private readonly ILogger<general> _logger;

        public admin(ILogger<general> logger) {
            _logger = logger;
        }

        [HttpPost("populateuuidstatus")]
        public ServerInformation PopulateUUIDStatus([FromBody] ServerInformation info) {
            Console.WriteLine(info.UUID + " / " + Program.ActiveRuntime.privilegedUuid);
            info.PrivilegedUUID = Security.IsUUIDPrivileged(info.UUID);
            return info;
        }
    }
}
