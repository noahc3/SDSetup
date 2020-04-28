/* Copyright (c) 2019 noahc3
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDSetupCommon {

    public class Message {
        public string Color { get; set; } = "info";
        public string InnerHTML { get; set; } = "Welcome to Homebrew SD Setup!";

        public Message() { }
    }

    public class Bundle {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Packages { get; set; }

        public Bundle() {

        }
    }

    public class Warning {
        public string Title { get; set; }
        public string Content { get; set; }
        public string PackageID { get; set; }

        public Warning() {

        }
    }

    public class Manifest {
        public string Version { get; set; } = "";
        public string Copyright { get; set; } = "";
        public Message Message { get; set; } = new Message();
        public Dictionary<string, Platform> Platforms { get; set; } = new Dictionary<string, Platform>();

        public Manifest() {

        }
    }

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

    public class PackageSection {
        public string ID { get; set; } = "";
        public string Name { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public int ListingMode { get; set; } = 0;
        public bool Visible { get; set; } = true;
        public List<string> When { get; set; } = new List<string>();
        public int WhenMode { get; set; } = 0; //0: all | 1: any
        public string Footer { get; set; } = "";
        public Dictionary<string, PackageCategory> Categories { get; set; } = new Dictionary<string, PackageCategory>();



        public PackageSection() {

        }
    }

    public class PackageCategory {
        public string ID { get; set; } = "";
        public string Name { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public bool Visible { get; set; } = true;
        public List<string> When { get; set; } = new List<string>();
        public int WhenMode { get; set; } = 0; //0: all | 1: any
        public Dictionary<string, PackageSubcategory> Subcategories { get; set; } = new Dictionary<string, PackageSubcategory>();

        public PackageCategory() {

        }
    }

    public class PackageSubcategory {
        public string ID { get; set; } = "";
        public string Name { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public bool Visible { get; set; } = true;
        public List<string> When { get; set; } = new List<string>();
        public int WhenMode { get; set; } = 0; //0: all | 1: any
        public Dictionary<string, Package> Packages { get; set; } = new Dictionary<string, Package>();


        public PackageSubcategory() {

        }
    }

    public class Package {
        public string ID { get; set; } = "";
        public string Name { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string Platform { get; set; } = "";
        public string Section { get; set; } = "";
        public string Category { get; set; } = "";
        public string Subcategory { get; set; } = "";
        public string Authors { get; set; } = "";
        public long Downloads { get; set; } = 0;
        public Dictionary<string, VersionInfo> Channels { get; set; } = new Dictionary<string, VersionInfo>();
        public string Source { get; set; } = "";
        public string DLSource { get; set; } = "";
        public int Priority { get; set; } = 0;
        public bool EnabledByDefault { get; set; } = false;
        public bool Visible { get; set; } = true;
        public bool ShowsInCredits { get; set; } = true;
        public string Description { get; set; } = "";
        public List<string> When { get; set; } = new List<string>();
        public int WhenMode { get; set; } = 0; //0: all | 1: any
        public Warning Warning { get; set; }
        public List<string> Dependencies { get; set; } = new List<string>();
        public List<string> DeleteOnUpdate { get; set; } = new List<string>();

        public AutoUpdateType AutoUpdateType { get; set; } = AutoUpdateType.None;
        public string AutoUpdateHint { get; set; } = "";
        public string AutoUpdatePathOverride { get; set; } = "";

        public Package() {

        }
    }

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
                Source = Source,
                DLSource = DLSource,
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

            foreach(KeyValuePair<string, string> v in Versions) {
                p.Channels[v.Key] = new VersionInfo() {
                    Version = v.Value,
                    Size = 0
                };
            }

            return p;
        }
    }

    public class VersionInfo {
        public string Version { get; set; } = "v0";
        public long Size { get; set; } = 0;

        public VersionInfo() {
        }
    }

    public enum AutoUpdateType {
        None = 0, LibGet = 1, Github = 2, Kosmos = 3, Custom = 4
    }
}