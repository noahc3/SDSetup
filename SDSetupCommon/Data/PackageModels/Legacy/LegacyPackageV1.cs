using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDSetupCommon.Data.PackageModels.Legacy {
    [Obsolete("Start using the new Package class. This class is only provided for purposes of conerting the old format to the new format.")]
    public class LegacyPackageV1 {
        public string ID = "";
        public string Name = "";
        public string DisplayName = "";
        public string Platform = "";
        public string Section = "";
        public string Category = "";
        public string Subcategory = "";
        public string Authors = "";
        public long Downloads = 0;
        public Dictionary<string, string> Versions = new Dictionary<string, string>();
        public string Source = "";
        public string DLSource = "";
        public int Size = 0;
        public int Priority = 0;
        public bool EnabledByDefault = false;
        public bool Visible = true;
        public bool ShowsInCredits = true;
        public string Description = "";
        public List<string> When = new List<string>();
        public int WhenMode = 0; //0: all | 1: any
        public Warning Warning;
        public List<string> Dependencies = new List<string>();
        public List<string> DeleteOnUpdate = new List<string>();

        public AutoUpdateType AutoUpdateType = AutoUpdateType.None;
        public string AutoUpdateHint = "";
        public string AutoUpdatePathOverride = "";

        public LegacyPackageV1() {

        }

        public LegacyPackageV1(string iD, string name, string displayName, string platform, string section, string category, string subcategory, string authors, long downloads, Dictionary<string, string> versions, string source, string dLSource, int size, int priority, bool enabledByDefault, bool visible, bool showsInCredits, string description, List<string> when, int whenMode, Warning warning, List<string> dependencies, List<string> deleteOnUpdate) {
            ID = iD;
            Name = name;
            DisplayName = displayName;
            Platform = platform;
            Section = section;
            Category = category;
            Subcategory = subcategory;
            Authors = authors;
            Downloads = downloads;
            Versions = versions;
            Source = source;
            DLSource = dLSource;
            Size = size;
            Priority = priority;
            EnabledByDefault = enabledByDefault;
            Visible = visible;
            ShowsInCredits = showsInCredits;
            Description = description;
            When = when;
            WhenMode = whenMode;
            Warning = warning;
            Dependencies = dependencies;
            DeleteOnUpdate = deleteOnUpdate;
        }

        public Package UpgradeFormat() {
            Package p = new Package() {
                ID = ID,
                Name = Name,
                DisplayName = DisplayName,
                Platform = Platform,
                Section = Section,
                Category = Category,
                Subcategory = Subcategory,
                Authors = Authors,
                Downloads = Downloads,
                VersionInfo = new VersionInfo() { Version = Versions.First().Value.Replace("/", "."), Size = 0 },
                Source = Source,
                Priority = Priority,
                EnabledByDefault = EnabledByDefault,
                Visible = Visible,
                ShowsInCredits = ShowsInCredits,
                Description = Description,
                When = When,
                WhenMode = WhenMode,
                Warning = Warning,
                Dependencies = Dependencies,
                DeleteOnUpdate = DeleteOnUpdate
            };

            return p;
        }
    }
}
