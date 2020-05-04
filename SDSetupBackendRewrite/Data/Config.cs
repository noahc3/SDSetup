using SDSetupCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDSetupBackendRewrite.Data {
    //Config is the structure for the config.json file used to configure the operation of the server. Any properties that also exist in Runtime
    //will overwrite Runtime when the server restarts or the config is manually reloaded, and any changes to the active Runtime will be written to the config.
    public class Config {
        public string BindIp = "127.0.0.1";
        public int BindPort = 5000;
        public bool UseUpdater = true;
        public string UpdaterInterval = "12:00:00"; // 12 hours
        public string TempPath = (Globals.RootDirectory + "/temp").AsPath();
        public string FilesPath = (Globals.RootDirectory + "/files").AsPath();
        public string UpdaterPath = (Globals.RootDirectory + "/updater/SDSetupUpdater").AsPath();

        public string[] ValidChannels = new string[] { "latest" };
        public bool AppSupport = true;
        public string LatestPackageset = "default";
        public string LatestAppVersion = "v1.0";
        public string LatestAppPath = (Globals.RootDirectory + "/config/sdsetup.nro").AsPath();

        public string ManagerFrontendUrl = "http://manager.sdsetup.com";
        public string GithubClientId = "";
        public string GithubClientSecret = "";
        public string GitlabClientId = "";
        public string GitlabClientSecret = "";

        public bool UseMongoDB = false;
        public string MongoDBHostname = "";
        public string MongoDBUsername = "";
        public string MongoDBPassword = "";
        public string MongoDBDatabase = "";

    }
}
