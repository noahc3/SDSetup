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
