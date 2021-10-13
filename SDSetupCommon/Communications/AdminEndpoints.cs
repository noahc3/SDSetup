using SDSetupCommon.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SDSetupCommon.Communications {
    public class AdminEndpoints {
        private static string PopulateUUIDPrivilegedEndpoint = "/api/v2/admin/populateuuidstatus";
        private static string SystemLogsEndpoint = "/api/v2/admin/systemlogs";
        private static string BundlerLogsEndpoint = "/api/v2/admin/bundlerlogs/{0}";
        public static async Task<ServerInformation> PopulateUUIDStatus() {
            return await CommsUtilities.PostJsonAsync<ServerInformation, ServerInformation>(EndpointSettings.serverInformation.Hostname + PopulateUUIDPrivilegedEndpoint, EndpointSettings.serverInformation);
        }

        public static async Task<List<TaskLogger>> GetSystemLogs() {
            return await CommsUtilities.GetJsonAsync<List<TaskLogger>>(CommsUtilities.FullApiEndpoint(SystemLogsEndpoint));
        }

        public static async Task<List<TaskLogger>> GetBundlerLogs(string search) {
            return await CommsUtilities.GetJsonAsync<List<TaskLogger>>(CommsUtilities.FullApiEndpoint(String.Format(BundlerLogsEndpoint, search)));
        }

    }
}
