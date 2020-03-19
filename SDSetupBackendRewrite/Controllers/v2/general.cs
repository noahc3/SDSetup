using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SDSetupCommon.Data;

namespace SDSetupBackendRewrite.Controllers {
    [ApiController]
    [Route("api/v2")]
    public class general : ControllerBase {

        private readonly ILogger<general> _logger;

        public general(ILogger<general> logger) {
            _logger = logger;
        }

        [HttpGet("probe")]
        public ServerInformation Probe() {
            return ServerInformation.getPopulatedServerInformation();
        }
    }
}
