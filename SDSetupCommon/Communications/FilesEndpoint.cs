using SDSetupCommon.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SDSetupCommon.Communications {
    public class FilesEndpoint {
        private static string LatestPackagesetEndpoint = "/api/v2/files/latestpackageset";
        private static string ManifestEndpoint = "/api/v2/files/manifest/{0}"; //0: packageset name
        public static async Task<string> GetLatestPackageset() {
            return await CommsUtilities.GetStringAsync(CommsUtilities.FullApiEndpoint(LatestPackagesetEndpoint));
        }

        public static async Task<Manifest> GetManifest(string packageset) {
            return await CommsUtilities.GetJsonAsync<Manifest>(CommsUtilities.FullApiEndpoint(String.Format(ManifestEndpoint, packageset)));
        }

        public static async Task<Manifest> GetLatestManifest() {
            string latestPackageset = await GetLatestPackageset();
            return await GetManifest(latestPackageset);
        }
    }
}
