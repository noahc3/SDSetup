using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDSetupManifestGenerator {
    public class Manifest {
        public string Version;
        public string Copyright;
        public Platform[] Platforms;
        public FAQSection[] FAQSections;
    }

    public class Platform {
        public string Name;
        public string MenuName;
        public string HomeIcon;
        public string ID;
        public CFW[] CFWs;
        public PackageSection[] PackageSections;
    }

    public class CFW {
        public string ID;
        public string Name;
        public string DisplayName;
        public Package[] Packages;
    }

    public class PackageSection {
        public string ID;
        public string Name;
        public string DisplayName;
        public int ListingMode;
        public bool Visible;
        public PackageCategory[] Categories;
    }

    public class PackageCategory {
        public string ID;
        public string Name;
        public string DisplayName;
        public bool Visible;
        public PackageSubcategory[] Subcategories;
    }

    public class PackageSubcategory {
        public string ID;
        public string Name;
        public string DisplayName;
        public bool Visible;
        public Package[] Packages;
    }

    public class Package {
        public string ID;
        public string Name;
        public string DisplayName;
        public string Authors;
        public string Version;
        public string Source;
        public string DLSource;
        public bool EnabledByDefault;
        public bool Visible;
        public string Description;
        public string[] Dependencies;
        public Artifact[] Artifacts;
    }

    public class Artifact {
        public string URL;
        public string Directory;
        public string FileName;
    }

    public class FAQSection {
        public string Name;
        public FAQ[] FAQs;
    }

    public class FAQ {
        public string question;
        public string answer;
    }
}
