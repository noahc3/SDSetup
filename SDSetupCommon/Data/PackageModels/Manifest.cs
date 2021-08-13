using System;
using System.Collections.Generic;
using System.Text;
using SDSetupCommon.Data.FrontendModels;
using SDSetupCommon.Data.IntegrationModels;

namespace SDSetupCommon.Data.PackageModels {
    public class Manifest {
        public string Packageset { get; set; } = ""; //note: any value stored in the file will always be overridden by the server.
        public string Version { get; set; } = "";
        public string Copyright { get; set; } = "";
        public Message Message { get; set; } = new Message();
        public Dictionary<string, Platform> Platforms { get; set; } = new Dictionary<string, Platform>();
        public Dictionary<string, Package> Packages { get; set; } = new Dictionary<string, Package>();
        public DonationModel DonationInfo { get; set; } = new DonationModel();

        public Manifest() {

        }

        public Package FindPackageById(string id) {
            if (Packages.ContainsKey(id)) return Packages[id];
            else return null;
        }

        public Platform FindPlatformByPath(string platform) {
            Platform p = null;
            if (Platforms != null && Platforms.ContainsKey(platform)) p = Platforms[platform];

            return p;
        }

        public PackageSection FindSectionByPath(string platform, string section) {
            Platform p = FindPlatformByPath(platform);
            PackageSection s = null;
            if (p != null && p.PackageSections != null && p.PackageSections.ContainsKey(section)) s = p.PackageSections[section];

            return s;
        }

        public PackageCategory FindCategoryByPath(string platform, string section, string category) {
            PackageSection s = FindSectionByPath(platform, section);
            PackageCategory c = null;
            if (s != null && s.Categories != null && s.Categories.ContainsKey(category)) c = s.Categories[category];

            return c;
        }

        public PackageSubcategory FindSubcategoryByPath(string platform, string section, string category, string subcategory) {
            PackageCategory c = FindCategoryByPath(platform, section, category);
            PackageSubcategory s = null;
            if (c != null && c.Subcategories != null && c.Subcategories.ContainsKey(subcategory)) s = c.Subcategories[subcategory];

            return s;
        }

        public bool UpdatePackage(Package changedPackage) {

            if (!Packages.ContainsKey(changedPackage.ID)) return false;
            Packages[changedPackage.ID] = changedPackage;

            return true;
        }
    }
}
