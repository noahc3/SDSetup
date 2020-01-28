/* Copyright (c) 2019 noahc3
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */
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
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;

using SDSetupCommon;
using System.Security.Cryptography;

namespace SDSetupBackend {
    public class Program {

        public static Dictionary<string, string> Manifests;

        private static string ip;
        private static int httpPort;

        public static string TempPath;
        public static string FilesPath;
        public static string ConfigPath;
        public static string UpdaterPath;

        public static Timer UpdaterTimer;
        public static bool UpdaterEnabled = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        public static string[] validChannels;
        public static List<string> uuidLocks = new List<string>();

        public static Dictionary<string, DeletingFileStream> generatedZips = new Dictionary<string, DeletingFileStream>();

        public static string latestPackageset = "default";
        public static string latestAppVersion = "NO VERSION";

        //public static DownloadStats dlStats;
        //private static bool dlStatsInitialized = false;
        //private static Timer dlStatsSaveTimer;

        private static string _privilegedUUID;
        private static string privilegedUUID {
            get {
                return _privilegedUUID;
            }

            set {
                Console.WriteLine("[WARN] New priveleged UUID: " + value);
                _privilegedUUID = value;
            }
        }

        public static void Main(string[] args) {

            Console.WriteLine("Working Directory: " + AppContext.BaseDirectory);

            Console.WriteLine(ReloadEverything());

            Console.WriteLine(ConfigPath);

            string[] hostConf = File.ReadAllLines((ConfigPath + "/host.txt").AsPath());
            ip = hostConf[0];
            httpPort = Convert.ToInt32(hostConf[1]);

            privilegedUUID = createCryptographicallySecureGuid().ToString().Replace("-", "").ToLower();

            UpdaterTimer = new Timer((e) => {
                if (UpdaterEnabled) {
                    if (File.Exists(($"{UpdaterPath}/SDSetupUpdater").AsPath())) {
                        Process process = new Process {
                            StartInfo = new ProcessStartInfo {
                                FileName = ($"{UpdaterPath}/SDSetupUpdater").AsPath(),
                                Arguments = $"-d {FilesPath} -p {latestPackageset} -u {privilegedUUID}",
                                UseShellExecute = false,
                                CreateNoWindow = true,
                                WorkingDirectory = UpdaterPath
                            }
                        };
                        process.Start();
                    }
                }
            }, null, TimeSpan.Zero, TimeSpan.FromMinutes(720));

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                Console.WriteLine("[WARN] Updater has been disabled as non-Linux platforms are not supported!");
            }

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
            if (uuid == privilegedUUID) return true;
            return false;
        }

        public static bool SetPrivelegedUUID(string oldUuid, string newUuid) {
            if (oldUuid != privilegedUUID) return false;
            privilegedUUID = newUuid;
            return true;
        }

        public static bool ToggleAutoUpdates() {
            UpdaterEnabled = !UpdaterEnabled;
            return UpdaterEnabled;
        }

        public static string ReloadEverything() {
#if (!DEBUG)
            try {
#endif
            //if dlstats was initialized, write them to disk before reloading.
            //if (dlStatsInitialized) {
            //    dlStats.VerifyStatisticsIntegrity(U.GetPackageListInLatestPackageset());
            //    File.WriteAllText((Config + "/dlstats.bin").AsPath(), dlStats.ToDataBinary(U.GetPackageList(latestPackageset)));
            //}

            //use temporary variables so if anything goes wrong, values wont be out of sync.
            Dictionary<string, string> _Manifests = new Dictionary<string, string>();

            string _TempPath = (AppContext.BaseDirectory + "/temp").AsPath();
            string _FilesPath = (AppContext.BaseDirectory + "/files").AsPath();
            string _ConfigPath = (AppContext.BaseDirectory + "/config").AsPath();
            string _UpdaterPath = (AppContext.BaseDirectory + "/updater").AsPath();

            if (!Directory.Exists(_TempPath)) Directory.CreateDirectory(_TempPath);
            if (!Directory.Exists(_FilesPath)) Directory.CreateDirectory(_FilesPath);
            if (!Directory.Exists(_ConfigPath)) Directory.CreateDirectory(_ConfigPath);
            if (!File.Exists((_ConfigPath + "/latestpackageset.txt").AsPath())) File.WriteAllText((_ConfigPath + "/latestpackageset.txt").AsPath(), "default");
            if (!File.Exists((_ConfigPath + "/latestappversion.txt").AsPath())) File.WriteAllText((_ConfigPath + "/latestappversion.txt").AsPath(), "NO VERSION");
            if (!File.Exists((_ConfigPath + "/validchannels.txt").AsPath())) File.WriteAllLines((_ConfigPath + "/validchannels.txt").AsPath(), new string[] { "latest", "nightly" });

            foreach(string n in Directory.EnumerateDirectories(_FilesPath)) {
                string k = n.Split(Path.DirectorySeparatorChar).Last();
                if (!File.Exists((_FilesPath + "/" + k + "/manifest6.json").AsPath())) File.WriteAllText(_FilesPath + "/" + k + "/manifest6.json", "{}");
            }

            string _latestPackageset = File.ReadAllText((_ConfigPath + "/latestpackageset.txt").AsPath());
            string _latestAppVersion = File.ReadAllText((_ConfigPath + "/latestappversion.txt").AsPath());
            string[] _validChannels = File.ReadAllLines((_ConfigPath + "/validchannels.txt").AsPath());

            //look away
            foreach (string n in Directory.EnumerateDirectories(_FilesPath)) {
                string k = n.Split(Path.DirectorySeparatorChar).Last();
                Manifest m = JsonConvert.DeserializeObject<Manifest>(File.ReadAllText((_FilesPath + "/" + k + "/manifest6.json").AsPath()));
                foreach (string c in Directory.EnumerateDirectories((_FilesPath + "/" + k).AsPath()).OrderBy(filename => filename)) {
                    string f = c.Split(Path.DirectorySeparatorChar).Last();
                    Package p = JsonConvert.DeserializeObject<Package>(File.ReadAllText((_FilesPath + "/" + k + "/" + f + "/info.json").AsPath()));
                    m.Platforms[p.Platform].PackageSections[p.Section].Categories[p.Category].Subcategories[p.Subcategory].Packages[p.ID] = p;
                }
                _Manifests[k] = JsonConvert.SerializeObject(m, Formatting.Indented);
            }

            //this must be set before GetPackageListInLatestPackageset() is called
            FilesPath = _FilesPath;

            //DownloadStats _dlStats;

            //if (File.Exists((_Config + "/dlstats.bin").AsPath())) {
            //    _dlStats = DownloadStats.FromDataBinary(File.ReadAllText((_Config + "/dlstats.bin").AsPath()));
            //} else {
            //    _dlStats = new DownloadStats();
            //}

            Manifest latestManifest = JsonConvert.DeserializeObject<Manifest>(_Manifests[_latestPackageset]);
            //_dlStats.VerifyStatisticsIntegrity(U.GetPackageList(_latestPackageset), latestManifest);
            _Manifests[_latestPackageset] = JsonConvert.SerializeObject(latestManifest);

            //if (dlStatsSaveTimer != null) dlStatsSaveTimer.Stop();
            //dlStatsSaveTimer = new Timer();
#if (DEBUG)     //
            //dlStatsSaveTimer.Interval = 10000; //10 seconds
#else           //
            //dlStatsSaveTimer.Interval = 600000; //10 minutes
#endif          //
            //dlStatsSaveTimer.AutoReset = true;
            //dlStatsSaveTimer.Elapsed += (sender, e) => {
            //    dlStats.VerifyStatisticsIntegrity(U.GetPackageListInLatestPackageset());
            //    File.WriteAllText((Config + "/dlstats.bin").AsPath(), dlStats.ToDataBinary(U.GetPackageList(latestPackageset)));
            //    Console.WriteLine("[ SAVE ] Wrote download stats to file (" + DateTime.Now.ToShortDateString() + " | " + DateTime.Now.ToShortTimeString() + ").");
            //};
            //dlStatsSaveTimer.Start();

            //update the real variables
            TempPath = _TempPath;
            ConfigPath = _ConfigPath;
            UpdaterPath = _UpdaterPath;
            latestPackageset = _latestPackageset;
            latestAppVersion = _latestAppVersion;
            validChannels = _validChannels;
            Manifests = _Manifests;
            //dlStats = _dlStats;

            //dlStatsInitialized = true;
#if (!DEBUG)
            } catch (Exception e) {
                return "[ERROR] Something went wrong while reloading: \n\n\nMessage:\n   " + e.Message + "\n\nStack Trace:\n" + e.StackTrace + "\n\n\nThe server will continue running and no changes will be saved";
            }
#endif
            return "Success";
        }

        public static bool OverridePrivilegedUuid() {
            if (File.Exists((ConfigPath + "/uuidoverride.txt").AsPath())) {
                privilegedUUID = File.ReadAllText((ConfigPath + "/uuidoverride.txt").AsPath());
                File.Delete((ConfigPath + "/uuidoverride.txt").AsPath());
                return true;
            }
            return false;
        }

        public static Guid createCryptographicallySecureGuid() {
            using (var provider = RandomNumberGenerator.Create()) {
                var bytes = new byte[16];
                provider.GetBytes(bytes);

                return new Guid(bytes);
            }
        }


    }
}
