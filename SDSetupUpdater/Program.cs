using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using Newtonsoft.Json;
using LibGetDownloader;
using SDSetupCommon;
using Octokit;

/*
 * Forewarning:
 * This tool is designed specifically for SDSetup. While you could feasibly
 * use the frontend and backend for any sort of package management where files
 * need to be joined into a single ZIP, this updater tool is specially designed
 * to update Nintendo Switch homebrew packages on SDSetup and nothing else.
 * 
 * Also the code sucks and is not dynamic in any way.
 * 
 */

namespace SDSetupUpdater {
    class Program {

        static GitHubClient ghClient;

        static List<string> FinalOutput = new List<string>();
        static Config config;
        static List<string> OutdatedPackagesLibGet = new List<string>();
        static Dictionary<string, string> OutdatedPackagesKosmos = new Dictionary<string, string>();
        static bool KosmosOutdated = false;
        static Dictionary<string, SDSetupCommon.Package> SDPackages = new Dictionary<string, SDSetupCommon.Package>();


        static void Main(string[] args) {
            //start arg and config validation
            Options o = new Options();
            Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(opts => o = opts);

            if (!File.Exists("config.json")) {
                Log("File config.json not found. Make sure it exists!");
                ExitProcedure(1);
            }

            if (!Directory.Exists(o.Directory)) {
                Log($"Directory {new DirectoryInfo(o.Directory).FullName} doesn't exist!");
                ExitProcedure(1);
            }

            string oPackagesetDirectory = new DirectoryInfo(Path.Join(o.Directory, o.Packageset)).FullName;

            if (!Directory.Exists(oPackagesetDirectory)) {
                Log($"Packageset directory {new DirectoryInfo(oPackagesetDirectory).FullName} doesn't exist!");
                ExitProcedure(1);
            }
            //end arg and config validation

            DateTime startTime = DateTime.UtcNow;
            config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
            Log("SDSetup Updater Started - " + startTime.ToLongTimeString() + " " + startTime.ToLongDateString());
            if (o.DryRun) Log("NOTE: Dry run is enabled!");

            Log("============================================================");
            Log($"Using libget repo '{config.LibGetRepo}'");
            Log($"Using SDSetup packageset '{o.Packageset}'");
            try {
                ghClient = new GitHubClient(new Octokit.ProductHeaderValue("sdsetup"));
                Credentials credentials = new Credentials(config.GithubUsername, config.GithubAuthToken);
            } catch (Exception e) {
                Log($"An exception occurred: {e.GetType().ToString()}\n{e.Message}\nStack Trace:\n{e.StackTrace}");
                ExitProcedure(1);
                return;
            }
            Log($"Github authenticated successfully ({ghClient.Miscellaneous.GetRateLimits().Result.Resources.Core.Remaining})");
            Log("============================================================");


            Repo repo;

            try {
                repo = Repo.GetRepo(config.LibGetRepo);
            } catch (Exception e) {
                Log($"An exception occurred: {e.GetType().ToString()}\n{e.Message}\nStack Trace:\n{e.StackTrace}");
                ExitProcedure(1);
                return;
            }

            Log("");
            Log("-------------------------  LibGet  -------------------------");

            foreach (string _k in Directory.EnumerateDirectories(oPackagesetDirectory)) {
                string k = new DirectoryInfo(_k).Name;
                string packageDirectory = new DirectoryInfo(Path.Join(oPackagesetDirectory, k)).FullName;
                SDPackages[k] = JsonConvert.DeserializeObject<SDSetupCommon.Package>(File.ReadAllText(Path.Join(packageDirectory, "info.json")));
            }

            foreach(SDSetupCommon.Package sdPackage in SDPackages.Values) {
                if (sdPackage.AutoUpdateType == AutoUpdateType.LibGet) {
                    if (!repo.PackageExists(sdPackage.AutoUpdateHint)) {
                        Log($"**[Warning]** libget package named '{sdPackage.AutoUpdateHint}' for SDSetup package {sdPackage.ID} doesn't exist. Skipping...");
                        continue;
                    }

                    LibGetDownloader.Package lgPackage = repo.GetPackage(sdPackage.AutoUpdateHint);

                    if (sdPackage.Versions["latest"] != lgPackage.Version && sdPackage.Versions["latest"] != "v" + lgPackage.Version) {
                        Log($"**[Update Detected]** '{sdPackage.ID}' {sdPackage.Versions["latest"]} -> {lgPackage.Version}");
                        OutdatedPackagesLibGet.Add(sdPackage.ID);
                    }
                }
            }

            Log("");
            Log("-------------------------  Kosmos  -------------------------");

            string latestKosmos = ghClient.Repository.Release.GetLatest(config.KosmosRepositoryId).Result.TagName;

            if (latestKosmos == SDPackages["atmos_musthave"].Versions["latest"]) {
                Log($"Kosmos is already up-to-date ({latestKosmos})!");
            } else {
                KosmosOutdated = true;
                Log($"Kosmos is outdated! Running auto script...");
                Dictionary<string, string> kosmos = RunKosmosAutoScript();

                foreach (SDSetupCommon.Package sdPackage in SDPackages.Values) {
                    if (sdPackage.AutoUpdateType == AutoUpdateType.Kosmos) {
                        //musthave needs special handling because im too lazy to fix my setup for how Atmosphere is packaged by SDSetup.
                        if (sdPackage.ID == "atmos_musthave") {
                            Log($"**[Update Detected]** '{sdPackage.ID}' {sdPackage.Versions["latest"]} -> {latestKosmos}");
                            OutdatedPackagesKosmos[sdPackage.ID] = kosmos[sdPackage.AutoUpdateHint];
                            continue;
                        }

                        if (!kosmos.ContainsKey(sdPackage.AutoUpdateHint)) {
                            Log($"**[Warning]** Kosmos auto package output named '{sdPackage.AutoUpdateHint}' for SDSetup package {sdPackage.ID} doesn't exist. Skipping...");
                            continue;
                        }
                        if (kosmos[sdPackage.AutoUpdateHint] != sdPackage.Versions["latest"]) {
                            Log($"**[Update Detected]** '{sdPackage.ID}' {sdPackage.Versions["latest"]} -> {kosmos[sdPackage.AutoUpdateHint]}");
                            OutdatedPackagesKosmos[sdPackage.ID] = kosmos[sdPackage.AutoUpdateHint];
                        }
                    }
                }
            }

            Log("");
            Log("============================================================");
            Log("");

            if (o.DryRun) {
                Log("Dry run completed.");
                ExitProcedure(0);
                return;
            }

            string newPackageset = "auto" + (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            string nPackagesetDirectory = new DirectoryInfo(Path.Join(o.Directory, newPackageset)).FullName;

            if (!(OutdatedPackagesLibGet.Count > 0 || KosmosOutdated)) {
                Log("No outdated packages detected. Process complete.");
                ExitProcedure(0);
                return;
            }

            Log($"Detected outdated packages. Cloning packageset '{o.Packageset}' to '{newPackageset}'");
            U.DirectoryCopy(oPackagesetDirectory, nPackagesetDirectory, true);

            if (OutdatedPackagesLibGet.Count > 0) {

                Log("");
                Log("============================================================");
                Log("");
                Log($"Detected {OutdatedPackagesLibGet.Count} outdated libget packages. Updating...");

                foreach(string k in OutdatedPackagesLibGet) {
                    string packageFilesDirectory = Path.Join(nPackagesetDirectory, k, "latest", "sd");
                    Directory.Delete(packageFilesDirectory, true);
                    Directory.CreateDirectory(packageFilesDirectory);
                    repo.DownloadPackageToDisk(SDPackages[k].AutoUpdateHint, packageFilesDirectory, true);
                    if (File.Exists(Path.Join(packageFilesDirectory, "info.json"))) File.Delete(Path.Join(packageFilesDirectory, "info.json"));
                    if (File.Exists(Path.Join(packageFilesDirectory, "manifest.install"))) File.Delete(Path.Join(packageFilesDirectory, "manifest.install"));
                    string oVersion = SDPackages[k].Versions["latest"];
                    string nVersion;
                    string lgVersion = repo.GetPackage(SDPackages[k].AutoUpdateHint).Version;
                    if (Char.IsNumber(lgVersion[0])) nVersion = "v" + lgVersion;
                    else nVersion = lgVersion;
                    SDPackages[k].Versions["latest"] = nVersion;
                    File.WriteAllText(Path.Join(nPackagesetDirectory, k, "info.json"), JsonConvert.SerializeObject(SDPackages[k], Formatting.Indented));
                    Log($"**[Package Updated]** '{k}' {oVersion} -> {nVersion}");
                }

            }

            if (KosmosOutdated) {

                Log("");
                Log("============================================================");
                Log("");
                Log($"Detected {OutdatedPackagesKosmos.Count} outdated Kosmos packages. Updating...");

                foreach (string k in OutdatedPackagesKosmos.Keys) {
                    string kosmosAutoPackageDirectory = Path.Join(new FileInfo(config.KosmosUpdaterScriptPath).Directory.FullName, "out", SDPackages[k].AutoUpdateHint);
                    string packageFilesDirectory = Path.Join(nPackagesetDirectory, k, "latest", "sd");

                    Directory.Delete(packageFilesDirectory, true);
                    Directory.Move(kosmosAutoPackageDirectory, packageFilesDirectory);
                    string oVersion = SDPackages[k].Versions["latest"];
                    string nVersion;

                    if (SDPackages[k].ID == "atmos_musthave") {
                        nVersion = latestKosmos;
                    } else {
                        nVersion = OutdatedPackagesKosmos[k];
                    }

                    SDPackages[k].Versions["latest"] = nVersion;
                    File.WriteAllText(Path.Join(nPackagesetDirectory, k, "info.json"), JsonConvert.SerializeObject(SDPackages[k], Formatting.Indented));
                    Log($"**[Package Updated]** '{k}' {oVersion} -> {nVersion}");
                }
                //update public facing "atmosphere" package with new atmos/kosmos version.
                SDPackages["atmosphere"].Versions["latest"] = $"{OutdatedPackagesKosmos["atmos_musthave"]}/{latestKosmos}";
                File.WriteAllText(Path.Join(nPackagesetDirectory, "atmosphere", "info.json"), JsonConvert.SerializeObject(SDPackages["atmosphere"], Formatting.Indented));
            }

            if (!String.IsNullOrWhiteSpace(o.PrivilegedUUID)) {
                Log("");
                Log("============================================================");
                Log("");

                Log($"Reloading backend {config.BackendHostname}");

                UpdateBackend(o.PrivilegedUUID, newPackageset);
            }

            Log("");
            Log("============================================================");
            Log("");
            Log($"Done! in {DateTime.UtcNow.Subtract(startTime).TotalSeconds}s");

            ExitProcedure(0);
            return;

        }

        static void Log(string message) {
            Console.WriteLine(message);
            FinalOutput.Add(message);
        }

        static void ExitProcedure(int code) {
            if (!String.IsNullOrEmpty(config.WebhookEndpoint)) {
                using (HttpClient client = new HttpClient()) {
                    for (int i = 0; i < FinalOutput.Count; i+= 50) {
                        List<string> finalOutputRange = new List<string>();
                        foreach(string k in FinalOutput.GetRange(i, Math.Min(50, FinalOutput.Count - i))) {
                            finalOutputRange.Add(k.Replace("\\", "/"));
                        }
                        string contentString = String.Join("\\n", finalOutputRange);
                        HttpContent content = new StringContent("{\"username\": \"SDSetup Updater\", \"content\": \"" + contentString +"\"}");
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        client.PostAsync(config.WebhookEndpoint, content).Wait();
                    }
                }
            }
            Environment.Exit(code);
        }

        static bool UpdateBackend(string uuid, string packageSet) {
            using (HttpClient client = new HttpClient()) {
                HttpResponseMessage resp = client.GetAsync($"{config.BackendHostname}/api/v1/admin/reloadall/{uuid}").Result;
                if (!resp.IsSuccessStatusCode) {
                    Log($"**[Error]** Failed to reload backend manifests! ({resp.StatusCode})");
                    Log($"{resp.Content.ReadAsStringAsync().Result}");
                    return false;
                }

                resp = client.GetAsync($"{config.BackendHostname}/api/v1/set/latestpackageset/{uuid}/{packageSet}").Result;
                if (!resp.IsSuccessStatusCode) {
                    Log($"**[Error]** Failed to set latest package set! ({resp.StatusCode})");
                    Log($"{resp.Content.ReadAsStringAsync().Result}");
                    return false;
                }

                return true;
            }
        }

        static Dictionary<string, string> RunKosmosAutoScript() {
            string scriptDirectory = new FileInfo(config.KosmosUpdaterScriptPath).Directory.FullName;
            string scriptPath = new FileInfo(config.KosmosUpdaterScriptPath).FullName;
            Process process = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = scriptPath,
                    Arguments = $"{Path.Join(scriptDirectory, "out")} {config.GithubUsername} {config.GithubAuthToken} auto",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = scriptDirectory
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            Dictionary<string, string> versions = new Dictionary<string, string>();

            foreach (string k in result.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)) {
                string version = k.Split(':')[1];
                if (Char.IsNumber(version[0])) version = "v" + version;
                versions[k.Split(':')[0]] = version;
            }
            return versions;

        }
    }

    class Options {
        [Option('d', "directory", Required = true, HelpText = "SDSetup \"files\" directory containing packagesets.")]
        public string Directory { get; set; }

        [Option('p', "packageset", Required = true, HelpText = "Packageset that needs updating.")]
        public string Packageset { get; set; }

        [Option('u', "uuid", HelpText = "Privileged UUID used to reload the backend. If not specified, backend is not reloaded.")]
        public string PrivilegedUUID { get; set; }

        [Option("dryrun", HelpText = "Perform update procedures without changing any files. For testing purposes.", Default = false)]
        public bool DryRun { get; set; }
    }

}
