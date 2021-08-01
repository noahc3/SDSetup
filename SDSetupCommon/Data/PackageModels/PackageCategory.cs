﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SDSetupCommon.Data.PackageModels {
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
}