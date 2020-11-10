using Newtonsoft.Json;
using SDSetupCommon.Data.PackageModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SDSetupCommon.Communications {
    public class PackageEndpoint {
        private static string UpdateNowEndpoint = "/api/v2/package/updatenow";

        public static async Task<HttpStatusCode> UpdatePackageNow(string packageset, Package changedPackage) {
            UpdatePackageModel model = new UpdatePackageModel { packageset = packageset, changedPackage = changedPackage };
            return await CommsUtilities.PostJsonAsync<UpdatePackageModel>(EndpointSettings.serverInformation.Hostname + UpdateNowEndpoint, model);
        }
    }
}
