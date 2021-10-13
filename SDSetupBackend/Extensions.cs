using SDSetupBackend.Data;
using SDSetupCommon.Data.PackageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using SDSetupCommon;
using SDSetupCommon.Data.UpdaterModels;

namespace SDSetupBackend {
    public static class Extensions {

        public static string GetLocalPath(this Package package, string packageset) {
            return ($"{Program.ActiveConfig.FilesPath}/{packageset}/{package.ID}/").AsPath();
        }
        public static string GetMetaPath(this Package package, string packageset) {
            return ($"{package.GetLocalPath(packageset)}/info.json").AsPath();
        }
        public static string GetVersionPath(this Package package, string packageset, string version) {
            return ($"{package.GetLocalPath(packageset)}/{version}").AsPath();
        }

        public static string GetVersion(this GitReleaseTagVersionSource src) {
            return "";
        }
    }
}
