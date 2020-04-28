/* Copyright (c) 2019 noahc3
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SDSetupCommon
{
    public static class Extensions {
        public static string NewLine(this string str, object line) {
            str = str + line.ToString() + "\n";
            return str;
        }

        //converts any path to the correct version for that platform, provided it does not start with a drive letter.
        public static string AsPath(this string path) {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
                return path.Replace("/", "\\").Replace("\\\\", "\\");
            } else {
                return path.Replace("\\", "/").Replace("//", "/");
            }
        }

        public static bool IsNullOrWhiteSpace(this string str) {
            return String.IsNullOrWhiteSpace(str);
        }

        public static string ToCleanString(this Guid guid) {
            return guid.ToString().Replace("-", "").ToLower();
        }

        //src: https://stackoverflow.com/a/468131, originally MSDN.
        public static long SizeRecursive(this DirectoryInfo d) {
            try {
                long size = 0;
                // Add file sizes.
                FileInfo[] fis = d.GetFiles();
                foreach (FileInfo fi in fis) {
                    size += fi.Length;
                }
                // Add subdirectory sizes.
                DirectoryInfo[] dis = d.GetDirectories();
                foreach (DirectoryInfo di in dis) {
                    size += di.SizeRecursive();
                }
                return size;
            } catch {
                return 0;
            }
        }
    }
}
