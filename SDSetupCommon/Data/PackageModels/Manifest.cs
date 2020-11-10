using System;
using System.Collections.Generic;
using System.Text;
using SDSetupCommon.Data.FrontendModels;

namespace SDSetupCommon.Data.PackageModels {
    public class Manifest {
        public string Version { get; set; } = "";
        public string Copyright { get; set; } = "";
        public Message Message { get; set; } = new Message();
        public Dictionary<string, Platform> Platforms { get; set; } = new Dictionary<string, Platform>();

        public Manifest() {

        }

        public Package FindPackageById(string id) {
            foreach (Platform p in Platforms.Values) {
                foreach (PackageSection s in p.PackageSections.Values) {
                    foreach (PackageCategory c in s.Categories.Values) {
                        foreach (PackageSubcategory sc in c.Subcategories.Values) {
                            if (sc.Packages.ContainsKey(id)) return sc.Packages[id];
                        }
                    }
                }
            }
            return null;
        }
    }
}
