using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDSetupBackend
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
    }
}
