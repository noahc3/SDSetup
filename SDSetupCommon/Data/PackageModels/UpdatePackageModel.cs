using System;
using System.Collections.Generic;
using System.Text;

namespace SDSetupCommon.Data.PackageModels {
    public class UpdatePackageModel {
        public string packageset { get; set; }
        public Package changedPackage { get; set; }
    }
}
