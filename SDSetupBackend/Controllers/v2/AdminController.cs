using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SDSetupCommon.Data;
using SDSetupBackend.Providers;
using System.Runtime.InteropServices;

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


    }
}
