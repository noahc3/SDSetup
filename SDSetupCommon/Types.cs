/* Copyright (c) 2018 noahc3
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
        public string Color = "info";
        public string InnerHTML = "Welcome to Homebrew SD Setup!";

        public Message(string color, string innerHTML) {
            Color = color;
            InnerHTML = innerHTML;
        }

        public Message() { }
    }

    public class Bundle {
        public string Name;
        public string Description;
        public List<string> Packages;

        public Bundle(string name, string description, List<string> packages) {
            Name = name;
            Description = description;
            this.Packages = packages;
        }

        public Bundle() {

        }
    }

    public class Warning {
        public string Title;
        public string Content;
        public string PackageID;

        public Warning(string title, string content, string packageId) {
            this.Title = title;
            this.Content = content;
            this.PackageID = packageId;
        }

        public Warning() {

        }
    }

    public class Manifest {
        public string Version = "";
        public string Copyright = "";
        public Message Message = new Message();
        public Dictionary<string, Platform> Platforms = new Dictionary<string, Platform>();

        public Manifest(string version, string copyright, Dictionary<string, Platform> platforms, Message message) {
            Version = version;
            Copyright = copyright;
            Platforms = platforms;
            Message = message;
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
        public bool Visible = true;
        public Dictionary<string, PackageSection> PackageSections = new Dictionary<string, PackageSection>();
        public List<Bundle> Bundles = new List<Bundle>();



        public Platform() {

        }

        public Platform(string name, string menuName, string homeIcon, string iD, string color, bool visible, Dictionary<string, PackageSection> packageSections, List<Bundle> bundles) {
            Name = name;
            MenuName = menuName;
            HomeIcon = homeIcon;
            ID = iD;
            Color = color;
            Visible = visible;
            PackageSections = packageSections;
            Bundles = bundles;
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
        public Dictionary<string, PackageCategory> Categories = new Dictionary<string, PackageCategory>();



        public PackageSection() {

        }

        public PackageSection(string iD, string name, string displayName, int listingMode, bool visible, List<string> when, int whenMode, Dictionary<string, PackageCategory> categories, string footer) {
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
        public Dictionary<string, PackageSubcategory> Subcategories = new Dictionary<string, PackageSubcategory>();



        public PackageCategory() {

        }

        public PackageCategory(string iD, string name, string displayName, bool visible, List<string> when, int whenMode, Dictionary<string, PackageSubcategory> subcategories) {
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
        public Dictionary<string, Package> Packages = new Dictionary<string, Package>();


        public PackageSubcategory() {

        }

        public PackageSubcategory(string iD, string name, string displayName, bool visible, List<string> when, int whenMode, Dictionary<string, Package> packages) {
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



        public Package() {

        }

        public Package(string iD, string name, string displayName, string platform, string section, string category, string subcategory, string authors, long downloads, Dictionary<string, string> versions, string source, string dLSource, int size, int priority, bool enabledByDefault, bool visible, bool showsInCredits, string description, List<string> when, int whenMode, Warning warning, List<string> dependencies, List<string> deleteOnUpdate) {
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
    }
}