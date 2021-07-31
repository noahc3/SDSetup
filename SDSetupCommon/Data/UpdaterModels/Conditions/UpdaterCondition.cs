using SDSetupCommon.Data.PackageModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SDSetupCommon.Data.UpdaterModels {
    public interface UpdaterCondition {
        Task<bool> Assert(Package package);
    }
}
