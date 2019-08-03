/* Copyright (c) 2019 noahc3
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;

using SDSetupCommon;

namespace SDSetupBlazor
{
    public static class G {
#if (DEBUG)
        //public static string hostname = "http://localhost:5000";
        public static string hostname = "https://files.sdsetup.com";
#else
        public static string hostname = "https://files.sdsetup.com";
#endif

        public static string packageset = "default24";
        public static string channel = "latest";
        
        public static string consoleId = "switch";

        public static bool initialized = false;
        public static bool isMobile = false;

        public static Dictionary<string, Dictionary<string, List<string>>> WhenDependants = new Dictionary<string, Dictionary<string, List<string>>>();
        public static Dictionary<string, Action> SectionRefreshes = new Dictionary<string, Action>();
        public static Dictionary<string, Action> SelectionRefreshes = new Dictionary<string, Action>();

        public static string[] autofillBlacklist = new string[] { "cfw", "cfwoptions", "payloads", "pctools", "cfwaddons" };
        public static string[] autodlBlacklist = new string[] { "cfw", "cfwoptions", "cfwaddons" };
        public static Dictionary<string, Dictionary<string, bool>> selectedPackages = new Dictionary<string, Dictionary<string, bool>>();
        public static Dictionary<string, Dictionary<string, Package>> packages = new Dictionary<string, Dictionary<string, Package>>();
        public static Manifest manifest;
        public static DownloadStats downloadStats;

        public static bool showWarning = false;
        public static int scrollPosStore;
        private static Warning _currentWarning;

        public static bool donotcontinue = false;

        private static List<string> oldPreSelects;

        public static Warning GetCurrentWarning() {
            return _currentWarning;
        }

        public static async void SetCurrentWarning(IJSRuntime jsRuntime, Warning warning) {
            _currentWarning = warning;
            if (warning != null) {
                showWarning = true;
                scrollPosStore = await ZipHelpers.GetScroll(jsRuntime);
                Shared.MainLayout.ForceUiRefresh();
            } else {
                await ZipHelpers.SetScroll(jsRuntime, scrollPosStore); 
            }
        }

        [JSInvokable]
        public static void allowContinue() {
            donotcontinue = false;
        }

        public static void Init(string url) {
            foreach (Platform k in manifest.Platforms.Values) {
                packages[k.ID] = new Dictionary<string, Package>();
                selectedPackages[k.ID] = new Dictionary<string, bool>();
                WhenDependants[k.ID] = new Dictionary<string, List<string>>();
                foreach (PackageSection sec in k.PackageSections.Values) {
                    foreach (PackageCategory c in sec.Categories.Values) {
                        foreach (PackageSubcategory s in c.Subcategories.Values) {
                            foreach (Package p in s.Packages.Values) {
                                selectedPackages[k.ID][p.ID] = p.EnabledByDefault;
                                packages[k.ID][p.ID] = p;
                                foreach (string w in p.When) {
                                    if (!WhenDependants[k.ID].ContainsKey(w)) WhenDependants[k.ID][w] = new List<string>();
                                    if (!WhenDependants[k.ID][w].Contains(sec.ID)) WhenDependants[k.ID][w].Add(sec.ID);
                                }
                            }
                        }
                    }
                }
            }
            if (url.Contains("/console?")) {
                string a = url.Split('?')[1];
                if (a.Contains('#')) a = a.Split('#')[0];
                G.consoleId = a;
            }
            SelectByUrl(url);
        }

        public static void SelectByUrl(string url) {
            if (!G.manifest.Platforms.ContainsKey(G.consoleId)) return;
            if (url.Split('#').Count() < 2 || url.Split('#')[1].Length == 0) {
                return;
            }
            List<string> preselects = url.Split('#')[1].Split(';').ToList();

            if (oldPreSelects != null && preselects.SequenceEqual(oldPreSelects)) return;
            oldPreSelects = preselects;
            foreach (KeyValuePair<string, bool> k in selectedPackages[consoleId].ToList()) {
                if (preselects.Contains(k.Key)) selectedPackages[consoleId][k.Key] = true;
                else selectedPackages[consoleId][k.Key] = false;
                if (SelectionRefreshes.ContainsKey(k.Key)) SelectionRefreshes[k.Key]();
            }

            return;
        }

        public static void SelectByList(List<string> packages) {
            foreach (KeyValuePair<string, bool> k in selectedPackages[consoleId].ToList()) {
                bool newValue = false;
                if (packages.Contains(k.Key)) newValue = true;
                if (newValue != k.Value) {
                    selectedPackages[consoleId][k.Key] = newValue;
                    if (SelectionRefreshes.ContainsKey(k.Key)) SelectionRefreshes[k.Key]();
                }
            }

            foreach (Action a in SectionRefreshes.Values) a();
        }

        public static bool CanShow(bool visible, List<string> When, int WhenMode) {
            if (!visible) return false;
            if (When.Count == 0) return true;
            if (WhenMode == 0) {
                foreach (string k in When) {
                    if (!selectedPackages[consoleId].ContainsKey(k) || !selectedPackages[consoleId][k]) return false;
                }
                return true;
            } else if (WhenMode == 1) {
                foreach (string k in When) {
                    if (selectedPackages[consoleId].ContainsKey(k) && selectedPackages[consoleId][k]) return true;
                }
                return false;
            }
            return false;
        }

        public static bool CanDownload(List<string> When, int WhenMode) {
            if (When.Count == 0) return true;
            if (WhenMode == 0) {
                foreach (string k in When) {
                    if (!selectedPackages[consoleId].ContainsKey(k) || !selectedPackages[consoleId][k]) return false;
                }
                return true;
            } else if (WhenMode == 1) {
                foreach (string k in When) {
                    if (selectedPackages[consoleId].ContainsKey(k) && selectedPackages[consoleId][k]) return true;
                }
                return false;
            }
            return false;
        }
    }
}
