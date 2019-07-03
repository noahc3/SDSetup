/* Copyright (c) 2019 noahc3
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace SDSetupCommon
{
    public class DownloadStats {
        public string StatisticsTrackingInitDate = DateTime.UtcNow.ToLongDateString() + " " + DateTime.UtcNow.ToLongTimeString();
        public bool initialized = false;
        private DateTime CurrentDateTime;
        private string CurrentDateTimeString;
        public long AllTimeBundles = 0;
        public Dictionary<string, Dictionary<string, int>> GranularStats; //<[DateTime.tostring], <[pkg name], downloads>>, last 30 days of data
        public Dictionary<string, long> AllTimeStats; //<[pkg name], alltime downloads>

        public const int _hoursToStore = 720;

        public void IncrementPackageDownloadCount(string package) {
            if (!AllTimeStats.ContainsKey(package)) return;

            AllTimeStats[package]++;
            GranularStats[CurrentDateTimeString][package]++;
        }

        public void VerifyStatisticsIntegrity(List<string> packages, Manifest manifestToUpdate = null) {
            CurrentDateTime = GetSanitizedDateTime();
            CurrentDateTimeString = CurrentDateTime.ToString();

            if (GranularStats == null) GranularStats = new Dictionary<string, Dictionary<string, int>>();
            if (AllTimeStats == null) AllTimeStats = new Dictionary<string, long>(); 

            foreach(string k in packages) {
                if (!AllTimeStats.ContainsKey(k)) {
                    AllTimeStats[k] = 0;
                }
            }

            for (int i = -1 * _hoursToStore + 1; i <= 0; i++) {
                DateTime point = CurrentDateTime.AddHours(i);
                string sPoint = point.ToString();
                if (!GranularStats.ContainsKey(sPoint)) {
                    GranularStats[sPoint] = new Dictionary<string, int>();
                }
                foreach (string k in packages) {
                    if (!GranularStats[sPoint].ContainsKey(k)) {
                        GranularStats[sPoint][k] = 0;
                    }
                }
            }

            while (GranularStats.Keys.Count > _hoursToStore) {
                GranularStats.Remove(GranularStats.ElementAt(0).Key);
            }

            if (manifestToUpdate != null) {
                foreach(Platform plat in manifestToUpdate.Platforms.Values) {
                    foreach(PackageSection sec in plat.PackageSections.Values) {
                        foreach(PackageCategory cat in sec.Categories.Values) {
                            foreach(PackageSubcategory sub in cat.Subcategories.Values) {
                                foreach(Package p in sub.Packages.Values) {
                                    if (AllTimeStats.ContainsKey(p.ID)) {
                                        p.Downloads = AllTimeStats[p.ID];
                                    } else {
                                        p.Downloads = 0;
                                    }
                                    
                                }
                            }
                        }
                    }
                }
            }
        }

        private DateTime GetSanitizedDateTime() {
            DateTime now = DateTime.UtcNow;
            DateTime point = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0, 0);
            return point;
        }

        public static DownloadStats FromDataBinary(string raw) {

            string[] binary = raw.Split('\n');

            DownloadStats stats = new DownloadStats();
            stats.CurrentDateTime = stats.GetSanitizedDateTime();
            stats.CurrentDateTimeString = stats.CurrentDateTime.ToString();

            stats.StatisticsTrackingInitDate = binary[0].Split('|')[0];
            stats.AllTimeBundles = Convert.ToInt64(binary[0].Split('|').Last());

            stats.GranularStats = new Dictionary<string, Dictionary<string, int>>();
            stats.AllTimeStats = new Dictionary<string, long>();

            foreach (string k in binary[1].Split('.')) {
                stats.GranularStats[k] = new Dictionary<string, int>();
            }
            
            for (int n = 2; n < binary.Length - 1; n++) {
                string k = binary[n];
                string[] splits = k.Split('|');
                stats.AllTimeStats[splits[0]] = Convert.ToInt64(splits[1]);

                string[] values = splits[2].Split('.');

                int i = 0;

                foreach (string time in binary[1].Split('.')) {
                    stats.GranularStats[time][splits[0]] = Convert.ToInt32(values[i]);
                    i++;
                }
            }

            return stats;
        }

        public string ToDataBinary(List<string> packages) {

            VerifyStatisticsIntegrity(packages);

            string binary = "";
            
            binary = binary.NewLine(StatisticsTrackingInitDate + "|" + AllTimeBundles);

            string info = "";
            foreach (string k in GranularStats.Keys) {
                info += k + ".";
            }
            binary = binary.NewLine(info.Remove(info.Length - 1));

            Dictionary<string, string> infos = new Dictionary<string, string>();

            foreach(string k in packages) {
                infos[k] = "";
            }

            for (int i = 0; i > -1 * _hoursToStore; i--) {
                DateTime point = CurrentDateTime.AddHours(i);
                string sPoint = point.ToString();
                foreach (string k in packages) {
                    infos[k] += GranularStats[sPoint][k] + ".";
                }
            }

            foreach (string k in packages) {
                infos[k] = (k + "|" + AllTimeStats[k] + "|" + infos[k]);
                infos[k] = infos[k].Remove(infos[k].Length - 1); //remove trailing '.'
                binary = binary.NewLine(infos[k]);
            }

            return binary;
        }
    }
}
