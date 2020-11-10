using System;
using System.Collections.Generic;
using System.Text;
using SDSetupCommon.Data.FrontendModels;

namespace SDSetupCommon.Data.PackageModels {
    public class Manifest {
        public string Packageset { get; set; } = ""; //note: any value stored in the file will always be overridden by the server.
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

        public Package FindPackageByPath(string platform, string section, string category, string subcategory, string packageId) {
            PackageSubcategory s = FindSubcategoryByPath(platform, section, category, subcategory);
            Package p = null;
            if (s != null && s.Packages != null && s.Packages.ContainsKey(packageId)) p = s.Packages[packageId];

            return p;
        }

        public bool UpdatePackage(Package changedPackage) {
            PackageSubcategory sub = FindSubcategoryByPath(changedPackage.Platform, changedPackage.Section, changedPackage.Category, changedPackage.Subcategory);

            if (sub == null) return false;
            if (!sub.Packages.ContainsKey(changedPackage.ID)) return false;

            sub.Packages[changedPackage.ID] = changedPackage;

            return true;
        }
    }
}
