using SDSetupCommon.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SDSetupCommon.Communications {
    public class GeneralEndpoints {
        private static string ProbeEndpoint = "/api/v2/probe";
        public static async Task<ServerInformation> Probe() {
            return await AbstractCommunications.GetAsync<ServerInformation>(EndpointSettings.serverInformation.Hostname + ProbeEndpoint);
        }
    }
}
