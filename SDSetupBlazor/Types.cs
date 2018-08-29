using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor;

namespace SDSetupBlazor {
    public class Manifest {
        public string Version = "";
        public string Copyright = "";
        public Dictionary<string, Platform> Platforms = new Dictionary<string, Platform>();
        public List<FAQSection> FAQSections = new List<FAQSection>();

        public Manifest(string version, string copyright, Dictionary<string, Platform> platforms, List<FAQSection> fAQSections) {
            Version = version;
            Copyright = copyright;
            Platforms = platforms;
            FAQSections = fAQSections;
        }

        public Manifest() {

        }
    }

    public class Platform {
        public string Name = "";
        public string MenuName = "";
        public string HomeIcon = "";
        public string ID = "";
        public string Color = "";
        public List<PackageSection> PackageSections = new List<PackageSection>();

        public Platform(string name, string menuName, string homeIcon, string iD, List<PackageSection> packageSections, string color) {
            Name = name;
            MenuName = menuName;
            HomeIcon = homeIcon;
            ID = iD;
            PackageSections = packageSections;
            Color = color;
        }

        public Platform() {

        }
    }

    public class PackageSection {
        public string ID = "";
        public string Name = "";
        public string DisplayName = "";
        public int ListingMode = 0;
        public bool Visible = true;
        public List<string> When = new List<string>();
        public int WhenMode = 0; //0: all | 1: any
        public string Footer;
        public List<PackageCategory> Categories = new List<PackageCategory>();

        

        public PackageSection() {

        }

        public PackageSection(string iD, string name, string displayName, int listingMode, bool visible, List<string> when, int whenMode, List<PackageCategory> categories, string footer) {
            ID = iD;
            Name = name;
            DisplayName = displayName;
            ListingMode = listingMode;
            Visible = visible;
            When = when;
            WhenMode = whenMode;
            Categories = categories;
            Footer = footer;
        }
    }

    public class PackageCategory {
        public string ID = "";
        public string Name = "";
        public string DisplayName = "";
        public bool Visible = true;
        public List<string> When = new List<string>();
        public int WhenMode = 0; //0: all | 1: any
        public List<PackageSubcategory> Subcategories = new List<PackageSubcategory>();



        public PackageCategory() {

        }

        public PackageCategory(string iD, string name, string displayName, bool visible, List<string> when, int whenMode, List<PackageSubcategory> subcategories) {
            ID = iD;
            Name = name;
            DisplayName = displayName;
            Visible = visible;
            When = when;
            WhenMode = whenMode;
            Subcategories = subcategories;
        }
    }

    public class PackageSubcategory {
        public string ID = "";
        public string Name = "";
        public string DisplayName = "";
        public bool Visible = true;
        public List<string> When = new List<string>();
        public int WhenMode = 0; //0: all | 1: any
        public List<Package> Packages = new List<Package>();


        public PackageSubcategory() {

        }

        public PackageSubcategory(string iD, string name, string displayName, bool visible, List<string> when, int whenMode, List<Package> packages) {
            ID = iD;
            Name = name;
            DisplayName = displayName;
            Visible = visible;
            When = when;
            WhenMode = whenMode;
            Packages = packages;
        }
    }

    public class Package {
        public string ID = "";
        public string Name = "";
        public string DisplayName = "";
        public string Authors = "";
        public string Version = "";
        public string Source = "";
        public string DLSource = "";
        public bool EnabledByDefault = false;
        public bool Visible = true;
        public string Description = "";
        public List<string> When = new List<string>();
        public int WhenMode = 0; //0: all | 1: any
        public List<string> Dependencies = new List<string>();
        public List<Artifact> Artifacts = new List<Artifact>();



        public Package() {

        }

        public Package(string iD, string name, string displayName, string authors, string version, string source, string dLSource, bool enabledByDefault, bool visible, string description, List<string> when, int whenMode, List<string> dependencies, List<Artifact> artifacts) {
            ID = iD;
            Name = name;
            DisplayName = displayName;
            Authors = authors;
            Version = version;
            Source = source;
            DLSource = dLSource;
            EnabledByDefault = enabledByDefault;
            Visible = visible;
            Description = description;
            When = when;
            WhenMode = whenMode;
            Dependencies = dependencies;
            Artifacts = artifacts;
        }
    }

    public class Artifact {
        public string URL;
        public string Directory;
        public string FileName;
        public string DiskLocation;

        public Artifact(string uRL, string directory, string fileName, string diskLocation) {
            URL = uRL;
            Directory = directory;
            FileName = fileName;
            DiskLocation = diskLocation;
        }

        public Artifact() {

        }
    }

    public class FAQSection {
        public string Name;
        public List<FAQ> FAQs;

        public FAQSection(string name, List<FAQ> fAQs) {
            Name = name;
            FAQs = fAQs;
        }

        public FAQSection() {

        }
    }

    public class FAQ {
        public string question;
        public string answer;

        public FAQ(string question, string answer) {
            this.question = question;
            this.answer = answer;
        }

        public FAQ() {

        }
    }
}
