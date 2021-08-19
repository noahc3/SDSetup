using System;
using System.Collections.Generic;
using System.Text;

namespace SDSetupCommon.Data.PackageModels {
    public class Bundle {
        public string Name { get; set; }
        public string Description { get; set; }
        public string[] Packages { get; set; }
        public bool PermalinkAvailable { get; set; } = false;

        public Bundle() {

        }
    }
}
