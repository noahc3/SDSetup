/* Copyright (c) 2018 noahc3
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Blazor.Services;

namespace SDSetupBlazor
{
    public static class G {
        public static bool initialized = false;
        public static bool isMobile = false;

        public static string[] autofillBlacklist = new string[] { "cfw", "cfwoptions", "payloads", "pctools", "cfwaddons" };
        public static string[] autodlBlacklist = new string[] { "cfw", "cfwoptions", "cfwaddons" };
        public static Dictionary<string, bool> selectedPackages = new Dictionary<string, bool>();
        public static Dictionary<string, Dictionary<string, Package>> packages = new Dictionary<string, Dictionary<string, Package>>();
        public static Manifest manifest;

        public static bool donotcontinue = false;

        private static List<string> oldPreSelects;

        [JSInvokable]
        public static void allowContinue() {
            donotcontinue = false;
        }

        public static void Init(string url) {
            foreach (Platform k in manifest.Platforms.Values) {
                packages[k.ID] = new Dictionary<string, Package>();
                foreach (PackageSection sec in k.PackageSections) {
                    foreach (PackageCategory c in sec.Categories) {
                        foreach (PackageSubcategory s in c.Subcategories) {
                            foreach (Package p in s.Packages) {
                                selectedPackages[p.ID] = p.EnabledByDefault;
                                packages[k.ID][p.ID] = p;
                            }
                        }
                    }
                }
            }

            SelectByUrl(url);
        }

        public static void SelectByUrl(string url) {
            if (url.Split('#').Count() < 2 || url.Split('#')[1].Length == 0) {
                return;
            }
            List<string> preselects = url.Split('#')[1].Split(';').ToList();

            if (oldPreSelects != null && preselects.SequenceEqual(oldPreSelects)) return;
            oldPreSelects = preselects;
            foreach (KeyValuePair<string, bool> k in selectedPackages.ToList()) {
                if (preselects.Contains(k.Key)) selectedPackages[k.Key] = true;
                else selectedPackages[k.Key] = false;
            }

            return;
        }
    }
}
