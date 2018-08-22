/* Copyright (c) 2018 noahc3
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace SDSetupManifestGenerator {
    public static class G {
        public static FormAuthoringTool tool;

        private static WebClient web = new WebClient();

        public static void DownloadFile(string url, string path) {
            web.DownloadFile(url, path);
        }

        public static string[] GetAllFilesInDir(string dir) {
            List<string> files = new List<string>();
            foreach(string k in Directory.EnumerateDirectories(dir)) {
                files.AddRange(GetAllFilesInDir(k));
            }
            files.AddRange(Directory.EnumerateFiles(dir));

            return files.ToArray();
        }
    }
}
