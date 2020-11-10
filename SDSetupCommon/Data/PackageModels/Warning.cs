using System;
using System.Collections.Generic;
using System.Text;

namespace SDSetupCommon.Data.PackageModels {
    public class Warning {
        public string Title { get; set; }
        public string Content { get; set; }
        public string PackageID { get; set; }

        public Warning() {

        }
    }
}
