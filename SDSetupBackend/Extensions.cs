using SDSetupBackend.Data;
using SDSetupCommon.Data.PackageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using SDSetupCommon;

namespace SDSetupBackend {
    public static class Extensions {

        public static string GetLocalPath(this Package package, string packageset) {
            return ($"{Program.ActiveConfig.FilesPath}/{packageset}/{package.ID}/").AsPath();
        }
        public static string GetMetaPath(this Package package, string packageset) {
            return ($"{package.GetLocalPath(packageset)}/info.json").AsPath();
        }
    }
}
