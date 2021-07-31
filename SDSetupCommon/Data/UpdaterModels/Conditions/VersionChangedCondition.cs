using SDSetupCommon.Communications;
using SDSetupCommon.Data.PackageModels;
using SDSetupCommon.Data.ServiceModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SDSetupCommon.Data.UpdaterModels {
    public class VersionChangedCondition : UpdaterCondition {

        public async Task<bool> Assert(Package package) {
            string version = await package.AutoUpdateVersionSource.GetVersion();
            return version != package.VersionInfo.Version;
        }
    }
}
