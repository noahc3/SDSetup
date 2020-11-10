using System;
using System.Collections.Generic;
using System.Text;

namespace SDSetupCommon.Data.PackageModels {
    public class Bundle {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Packages { get; set; }

        public Bundle() {

        }
    }
}
