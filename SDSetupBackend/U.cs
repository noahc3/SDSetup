using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace SDSetupBackend {
    public class U {
        public static List<string> GetPackageListInLatestPackageset() {
            return GetPackageList(Program.latestPackageset);
        }

        public static List<string> GetPackageList(string packageset) {
            List<string> packages = new List<string>();

            foreach (string k in Directory.EnumerateDirectories((Program.Files + "/" + packageset).AsPath())) {
                packages.Add(k.Split(Path.DirectorySeparatorChar).Last());
            }

            return packages;
        }
    }
}
