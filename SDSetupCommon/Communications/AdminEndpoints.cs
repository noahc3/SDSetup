using SDSetupCommon.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SDSetupCommon.Communications {
    public class AdminEndpoints {
        private static string PopulateUUIDPrivilegedEndpoint = "/api/v2/admin/populateuuidstatus";
        public static async Task<ServerInformation> PopulateUUIDStatus() {
            return await AbstractCommunications.PostJsonAsync<ServerInformation, ServerInformation>(EndpointSettings.serverInformation.Hostname + PopulateUUIDPrivilegedEndpoint, EndpointSettings.serverInformation);
        }
    }
}
