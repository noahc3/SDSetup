using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace SDSetupManifestGenerator {
    public static class G {
        private static Form1 frm;
        private static FormMain frmm;
        private static WebClient web = new WebClient();

        public static void SetForm1(Form1 frm) {
            G.frm = frm;
        }

        public static void SetFormMain(FormMain frm) {
            G.frmm = frm;
        }

        public static void log(string message) {
            Action action = new Action(() => {
                frmm.txtLog.AppendText("[ INFO ] " + message + "\r\n");
            });
            if (frmm.InvokeRequired) frmm.Invoke(action);
            else action();
        }

        public static void DownloadFile(string url, string path) {
            G.log("Downloading File " + url.Split('/').Last());
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
