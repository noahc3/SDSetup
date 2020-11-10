using System;
using System.Collections.Generic;
using System.Text;

namespace SDSetupCommon.Data.PackageModels {
    public class VersionInfo {
        public string Version { get; set; } = "v0";
        public long Size { get; set; } = 0;

        public VersionInfo() {
        }
    }
}
