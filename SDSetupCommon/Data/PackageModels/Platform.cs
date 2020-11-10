using System;
using System.Collections.Generic;
using System.Text;

namespace SDSetupCommon.Data.PackageModels {
    public class Platform {
        public string Name { get; set; } = "";
        public string MenuName { get; set; } = "";
        public string HomeIcon { get; set; } = "";
        public string ID { get; set; } = "";
        public string Color { get; set; } = "";
        public bool Visible { get; set; } = true;
        public Dictionary<string, PackageSection> PackageSections { get; set; } = new Dictionary<string, PackageSection>();
        public List<Bundle> Bundles { get; set; } = new List<Bundle>();



        public Platform() {

        }
    }
}
