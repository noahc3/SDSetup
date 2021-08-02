using SDSetupCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SDSetupBackend.Data {
    //Config is the structure for the config.json file used to configure the operation of the server. Any properties that also exist in Runtime
    //will overwrite Runtime when the server restarts or the config is manually reloaded, and any changes to the active Runtime will be written to the config.
    public class Config {
        public string BindIp = "127.0.0.1";
        public int BindPort = 5000;
        public bool UseUpdater = true;
        public string TimedTasksInterval = "12:00:00"; // 12 hours
        public string ZipRetentionTime = "01:00:00"; //1 hour
        public int ZipCompressionLevel = 3; //0-9, 0=store
        public string TempPath = (Globals.RootDirectory + "/temp").AsPath();
        public string FilesPath = (Globals.RootDirectory + "/files").AsPath();
        public string DataPath = (Globals.RootDirectory + "/data/").AsPath();

        public bool AppSupport = true;
        public string LatestPackageset = "default";
        public string LatestAppVersion = "v1.0";
        public string LatestAppPath = (Globals.RootDirectory + "/config/sdsetup.nro").AsPath();

        public string ManagerFrontendUrl = "http://manager.sdsetup.com";
        public string GithubClientId = "";
        public string GithubClientSecret = "";
        public string GitlabClientId = "";
        public string GitlabClientSecret = "";

        public string GithubUsername = "";
        public string GithubPassword = "";

        

        public DirectoryInfo GetTempDirectory() {
            DirectoryInfo tmp = new DirectoryInfo(Path.Join(TempPath, Utilities.CreateGuid().ToCleanString()));
            tmp.Create();
            return tmp;
        }

        public string GetTempFilePath() {
            string tmp = Path.Join(TempPath, Utilities.CreateGuid().ToCleanString());
            return tmp;
        }

    }
}
