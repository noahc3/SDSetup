using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Newtonsoft.Json;
using System.Net;
using System.Timers;

using SDSetupCommon;

namespace SDSetupBackend {
    public class Program {

        public static Dictionary<string, string> Manifests;

        private static string ip;
        private static int httpPort;

        public static string Temp;
        public static string Files;
        public static string Config;

        public static string[] validChannels;
        public static List<string> uuidLocks = new List<string>();

        public static Dictionary<string, DeletingFileStream> generatedZips = new Dictionary<string, DeletingFileStream>();

        public static string latestPackageset = "default";
        public static string latestAppVersion = "NO VERSION";

        public static DownloadStats dlStats;
        private static bool dlStatsInitialized = false;
        private static Timer dlStatsSaveTimer;

        private static string _privelegedUUID;
        private static string privelegedUUID {
            get {
                return _privelegedUUID;
            }

            set {
                Console.WriteLine("[WARN] New priveleged UUID: " + value);
                _privelegedUUID = value;
            }
        }

        public static void Main(string[] args) {

            Console.WriteLine("Working Directory: " + AppContext.BaseDirectory);

            ReloadEverything();

            Console.WriteLine(Config);

            string[] hostConf = File.ReadAllLines((Config + "/host.txt").AsPath());
            ip = hostConf[0];
            httpPort = Convert.ToInt32(hostConf[1]);

            string[] certInfo = File.ReadAllLines((Config + "/https.txt").AsPath());

            privelegedUUID = Guid.NewGuid().ToString().Replace("-", "").ToLower();

            IWebHost host = CreateWebHostBuilder(args).Build();
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(options => {
                    options.Listen(IPAddress.Parse(ip), httpPort);
                    //options.Listen(IPAddress.Parse(ip), httpsPort, listenOptions => {
                    //    listenOptions.UseHttps(httpsCertLocation, httpsCertKey);
                    //});
                })
                .UseStartup<Startup>();

        public static bool IsUuidPriveleged(string uuid) {
            if (uuid == privelegedUUID) return true;
            return false;
        }

        public static bool SetPrivelegedUUID(string oldUuid, string newUuid) {
            if (oldUuid != privelegedUUID) return false;
            privelegedUUID = newUuid;
            return true;
        }

        public static string ReloadEverything() {
#if (!DEBUG)
            try {
#endif
                //if dlstats was initialized, write them to disk before reloading.
                if (dlStatsInitialized) {
                    dlStats.VerifyStatisticsIntegrity(U.GetPackageListInLatestPackageset());
                    File.WriteAllText((Config + "/dlstats.bin").AsPath(), dlStats.ToDataBinary(U.GetPackageList(latestPackageset)));
                }

                //use temporary variables so if anything goes wrong, values wont be out of sync.
                Dictionary<string, string> _Manifests = new Dictionary<string, string>();

                string _Temp = (AppContext.BaseDirectory + "/temp").AsPath();
                string _Files = (AppContext.BaseDirectory + "/files").AsPath();
                string _Config = (AppContext.BaseDirectory + "/config").AsPath();

                if (!Directory.Exists(_Temp)) Directory.CreateDirectory(_Temp);
                if (!Directory.Exists(_Files)) Directory.CreateDirectory(_Files);
                if (!Directory.Exists(_Config)) Directory.CreateDirectory(_Config);
                if (!File.Exists((_Config + "/latestpackageset.txt").AsPath())) File.WriteAllText((_Config + "/latestpackageset.txt").AsPath(), "default");
                if (!File.Exists((_Config + "/latestappversion.txt").AsPath())) File.WriteAllText((_Config + "/latestappversion.txt").AsPath(), "NO VERSION");
                if (!File.Exists((_Config + "/validchannels.txt").AsPath())) File.WriteAllLines((_Config + "/validchannels.txt").AsPath(), new string[] { "latest", "nightly" });

                foreach(string n in Directory.EnumerateDirectories(_Files)) {
                    string k = n.Split(Path.DirectorySeparatorChar).Last();
                    if (!File.Exists((_Files + "/" + k + "/manifest6.json").AsPath())) File.WriteAllText(_Files + "/" + k + "/manifest6.json", "{}");
                }

                string _latestPackageset = File.ReadAllText((_Config + "/latestpackageset.txt").AsPath());
                string _latestAppVersion = File.ReadAllText((_Config + "/latestappversion.txt").AsPath());
                string[] _validChannels = File.ReadAllLines((_Config + "/validchannels.txt").AsPath());

                //look away
                foreach (string n in Directory.EnumerateDirectories(_Files)) {
                    string k = n.Split(Path.DirectorySeparatorChar).Last();
                    Manifest m = JsonConvert.DeserializeObject<Manifest>(File.ReadAllText((_Files + "/" + k + "/manifest6.json").AsPath()));
                    foreach (string c in Directory.EnumerateDirectories((_Files + "/" + k).AsPath())) {
                        string f = c.Split(Path.DirectorySeparatorChar).Last();
                        Package p = JsonConvert.DeserializeObject<Package>(File.ReadAllText((_Files + "/" + k + "/" + f + "/info.json").AsPath()));
                        m.Platforms[p.Platform].PackageSections[p.Section].Categories[p.Category].Subcategories[p.Subcategory].Packages[p.ID] = p;
                    }
                    _Manifests[k] = JsonConvert.SerializeObject(m, Formatting.Indented);
                }

                //this must be set before GetPackageListInLatestPackageset() is called
                Files = _Files;

                DownloadStats _dlStats;

                if (File.Exists((_Config + "/dlstats.bin").AsPath())) {
                    _dlStats = DownloadStats.FromDataBinary(File.ReadAllText((_Config + "/dlstats.bin").AsPath()));
                } else {
                    _dlStats = new DownloadStats();
                }

                Manifest latestManifest = JsonConvert.DeserializeObject<Manifest>(_Manifests[_latestPackageset]);
                _dlStats.VerifyStatisticsIntegrity(U.GetPackageList(_latestPackageset), latestManifest);
                _Manifests[_latestPackageset] = JsonConvert.SerializeObject(latestManifest);

                if (dlStatsSaveTimer != null) dlStatsSaveTimer.Stop();
                dlStatsSaveTimer = new Timer();
#if (DEBUG)
                dlStatsSaveTimer.Interval = 10000; //10 seconds
#else
                dlStatsSaveTimer.Interval = 600000; //10 minutes
#endif
                dlStatsSaveTimer.AutoReset = true;
                dlStatsSaveTimer.Elapsed += (sender, e) => {
                    dlStats.VerifyStatisticsIntegrity(U.GetPackageListInLatestPackageset());
                    File.WriteAllText((Config + "/dlstats.bin").AsPath(), dlStats.ToDataBinary(U.GetPackageList(latestPackageset)));
                    Console.WriteLine("[ SAVE ] Wrote download stats to file (" + DateTime.Now.ToShortDateString() + " | " + DateTime.Now.ToShortTimeString() + ").");
                };
                dlStatsSaveTimer.Start();

                //update the real variables
                Temp = _Temp;
                Config = _Config;
                latestPackageset = _latestPackageset;
                latestAppVersion = _latestAppVersion;
                validChannels = _validChannels;
                Manifests = _Manifests;
                dlStats = _dlStats;

                dlStatsInitialized = true;
#if (!DEBUG)
            } catch (Exception e) {
                return "[ERROR] Something went wrong while reloading: \n\n\nMessage:\n   " + e.Message + "\n\nStack Trace:\n" + e.StackTrace + "\n\n\nThe server will continue running and no changes will be saved";
            }
#endif
            return "Success";
        }

        public static bool OverridePrivelegedUuid() {
            if (File.Exists((Config + "/uuidoverride.txt").AsPath())) {
                privelegedUUID = File.ReadAllText((Config + "/uuidoverride.txt").AsPath());
                File.Delete((Config + "/uuidoverride.txt").AsPath());
                return true;
            }
            return false;
        }

        
    }
}
