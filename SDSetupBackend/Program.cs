using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Newtonsoft.Json;
using Octokit;
using SDSetupBackend.Data;
using SDSetupBackend.Data.Accounts;
using SDSetupCommon;
using SDSetupCommon.Data.PackageModels;
using SDSetupCommon.Data.PackageModels.Legacy;
using SDSetupCommon.Data.UpdaterModels;

namespace SDSetupBackend {
    public class Program {

        public static ILogger logger;

        public static Config ActiveConfig { get; private set; }
        public static Runtime ActiveRuntime { get; private set; }

        public static GitHubClient GithubClient { get; private set; }

        public static IUserDatabase Users;

        public static void Main(string[] args) {
            bool err = false;
            var host = CreateHostBuilder(args).Build();
            logger = host.Services.GetRequiredService<ILogger<Program>>();

            logger.LogInformation("The backend server has begun initialization.");

            //attempt to load the configuration file from disk.
            if (!LoadConfigFromDisk()) {
                logger.LogError("Errors were detected while loading the configuration and initialization cannot continue. View the log for more details.");
                Environment.Exit(1);
            } else {
                logger.LogInformation("The configuration file loaded without errors.");
            }

            if (!SDSetupCommon.Communications.Overrides.UseDirectGitHub(
                ActiveConfig.GithubClientId, ActiveConfig.GithubClientSecret)) err = true;

            if (err) {
                logger.LogError("Errors were detected with the provided GitHub Client ID/Secret and initialization cannot continue. Please validate the configured ID and secret token.");
                Environment.Exit(1);
            } else {
                logger.LogInformation("Direct service overrides have been enabled in Common.");
            }

            logger.LogInformation("Loading runtime information");

            if (!LoadRuntime()) {
                logger.LogError("Errors were detected while configuring runtime information and initialization cannot continue. View the log for more details.");
                Environment.Exit(1);
            } else {
                logger.LogInformation("The runtime was configured without errors.");
            }

            if (!LoadPackageSets()) {
                logger.LogError("Errors were detected while loading packagesets and initialization cannot continue. View the log for more details.");
            } else {
                logger.LogInformation("Packagesets were loaded without errors.");
            }

            ActiveRuntime.ScheduleTimedTasks();
            logger.LogInformation("Timed tasks have been scheduled with an interval of " + TimeSpan.Parse(ActiveConfig.TimedTasksInterval).ToString());

            host.Run();
        }

        public static bool LoadRuntime() {
            Runtime runtime = new Runtime();
            runtime.privilegedUuid = Utilities.CreateCryptographicallySecureGuid().ToCleanString();
            ActiveRuntime = runtime;
            Users = new JsonUserDatabase(ActiveConfig.DataPath);
            return true;
        }

        public static bool LoadConfigFromDisk() {

            logger.LogInformation("Loading configuration file from disk.");

            bool err = false;

            string configPath = (Globals.RootDirectory + "/config/config.json").AsPath();

            if (!File.Exists(configPath)) {
                logger.LogInformation("No configuration file found at " + configPath + ", creating a new one and saving.");
                if (!Directory.Exists(configPath.Replace("config.json", ""))) Directory.CreateDirectory(configPath.Replace("config.json", ""));
                Config newConfig = new Config();
                File.WriteAllText(configPath, JsonConvert.SerializeObject(newConfig, Formatting.Indented));
            }

            Config proposedConfig = JsonConvert.DeserializeObject<Config>(File.ReadAllText(configPath));

            if (!Directory.Exists(proposedConfig.FilesPath)) Directory.CreateDirectory(proposedConfig.FilesPath);

            if (!Directory.Exists(proposedConfig.TempPath)) Directory.CreateDirectory(proposedConfig.TempPath);

            if (!Directory.Exists(proposedConfig.DataPath)) Directory.CreateDirectory(proposedConfig.DataPath);

            if (proposedConfig.UseUpdater) {
                if (String.IsNullOrWhiteSpace(proposedConfig.GithubUsername)) {
                    err = true;
                    logger.LogError("The auto-updater requires a valid GitHub login. Please specify a username in the config file.");
                }
                if (String.IsNullOrWhiteSpace(proposedConfig.GithubPassword)) {
                    err = true;
                    logger.LogError("The auto-updater requires a valid GitHub login. Please specify a passord in the config file.");
                }
                if (!TimeSpan.TryParse(proposedConfig.TimedTasksInterval, out _)) {
                    err = true;
                    logger.LogError("Failed to parse updater interval. Please ensure the syntax is correct. See https://docs.microsoft.com/en-us/dotnet/api/system.timespan.tryparse for examples.");
                }
            }
            if (String.IsNullOrWhiteSpace(proposedConfig.LatestPackageset)) {
                err = true;
                logger.LogError("You need to specify a packageset to use.");
            }
            if (proposedConfig.AppSupport) {
                logger.LogWarning("App support is not complete in the current version of SDSetupBackend and will not correctly support the SDSetup app. Please use the old version of the backend.");
                if (!File.Exists(proposedConfig.LatestAppPath)) {
                    err = true;
                    logger.LogError("SDSetup app path could not be found. Please ensure the path is correct or disable app support by setting AppSupport to false.");
                }
                if (String.IsNullOrWhiteSpace(proposedConfig.LatestAppVersion)) {
                    err = true;
                    logger.LogError("SDSetup app version was not specified. Please specify an app version or disable app support by setting AppSupport to false.");
                }
            }

            try {
                TimeSpan.Parse(proposedConfig.TimedTasksInterval);
            } catch (Exception e) {
                err = true;
                logger.LogDebug("Could not parse TimedTasksInterval from config. Please ensure the format is valid for TimeSpan.Parse().");
            }

            try {
                TimeSpan.Parse(proposedConfig.ZipRetentionTime);
            } catch (Exception e) {
                err = true;
                logger.LogDebug("Could not parse ZipRetentionTime from config. Please ensure the format is valid for TimeSpan.Parse().");
            }

            if (!err) {
                ActiveConfig = proposedConfig;
                File.WriteAllText(configPath, JsonConvert.SerializeObject(ActiveConfig, Formatting.Indented));
            }

            return !err;

        }

        public static bool LoadPackageSets() {
            Dictionary<string, Manifest> manifests = new Dictionary<string, Manifest>();
            foreach (DirectoryInfo packagesetDirectory in new DirectoryInfo(ActiveConfig.FilesPath).GetDirectories()) {
                logger.LogDebug("Processing packageset at " + packagesetDirectory.FullName);
                FileInfo manifestFile = new FileInfo((packagesetDirectory.FullName + "/manifest6.json").AsPath());
                Manifest manifest;
                string packagesetName = packagesetDirectory.Name;
                List<Package> packages = new List<Package>();
                try {
                    manifest = JsonConvert.DeserializeObject<Manifest>(File.ReadAllText(manifestFile.FullName));
                } catch (Exception e) {
                    logger.LogError("Failed to parse the manifest found at '" + manifestFile.FullName + "', the following exception was thrown:\n" + e.Message + "\n" + e.StackTrace);
                    return false;
                }

                foreach (DirectoryInfo packageDirectory in packagesetDirectory.GetDirectories()) {
                    bool dirty = false;
                    FileInfo packageFile = new FileInfo((packageDirectory.FullName + "/info.json").AsPath());
                    Package package;

                    try {
                        package = JsonConvert.DeserializeObject<Package>(File.ReadAllText(packageFile.FullName), new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });
                        logger.LogDebug("Processing package '" + package.ID + "'");
                        if (package.VersionInfo == null) throw new JsonSerializationException("Package format is outdated.");
                    } catch (JsonSerializationException e) {
                        //Something is wrong with the package information, it's likely either using the legacy format
                        //or has no defined channels. Find out which.

#pragma warning disable CS0618 // Type or member is obsolete
                        LegacyPackageV1 oldFormat = JsonConvert.DeserializeObject<LegacyPackageV1>(File.ReadAllText(packageFile.FullName));
#pragma warning restore CS0618 // Type or member is obsolete

                        if (oldFormat.Versions.Count == 0) {
                            logger.LogError("The package '" + oldFormat.ID + "' has no channels defined. Please define at least the 'latest' channel.");
                            return false;
                        } else {
                            // DATAFIXER:
                            // Old package information files did not use the VersionInfo object. Convert
                            // the old format to the new format using VersionInfo.
                            logger.LogInformation("Found outdated package format for package '" + oldFormat.ID + "', this package will be updated.");
                            package = oldFormat.UpgradeFormat();
                            dirty = true;
                        }
                    }

                    DirectoryInfo versionDirectory = new DirectoryInfo((packageDirectory.FullName + "/" + package.VersionInfo.Version).AsPath());

                    // DATAFIXER:
                    // Old package directories will have the version directories named with the channel
                    // they are for. The new format is for the directories to be named with the version.
                    if (!versionDirectory.Exists) {
                        DirectoryInfo oldVersionDirectory = new DirectoryInfo((packageDirectory.FullName + "/latest").AsPath());
                        if (oldVersionDirectory.Exists) {
                            logger.LogInformation("Found outdated version format for package '" + package.ID + "', this package will be updated.");
                            oldVersionDirectory.MoveTo(versionDirectory.FullName);
                        }
                    }

                    // DATAFIXER:
                    // IF the size data for the version is missing or invalid, calculate the size of
                    // the package.
                    if (package.VersionInfo.Size <= 0) {
                        logger.LogInformation("Found missing or invalid size data for package '" + package.ID + "' version '" + package.VersionInfo.Version + "', this information will be updated.");
                        package.VersionInfo.Size = versionDirectory.SizeRecursive();
                        dirty = true;
                    }

                    if (dirty) File.WriteAllText(packageFile.FullName, JsonConvert.SerializeObject(package, Formatting.Indented));

                    packages.Add(package);
                }

                foreach (Package p in packages) {

                    logger.LogDebug($"Registering package '{p.ID}' with packageset '{packagesetName}'");

                    //TODO: Add timed trigger support
                    foreach (UpdaterTrigger k in p.AutoUpdateTriggers) {
                        if (k is WebhookTrigger) {
                            logger.LogDebug($"Found webhook update trigger for package {p.ID}, registering");
                            ActiveRuntime.RegisterWebhook(((WebhookTrigger)k).WebhookId, packagesetName, p.ID);
                        } else if (k is TimedScanTrigger) {
                            logger.LogDebug($"Found timed scan update trigger for package {p.ID}, registering");
                            ActiveRuntime.RegisterTimedUpdate(packagesetName, p.ID);
                        }
                    }

                    manifest
                        .Platforms[p.Platform]
                        .PackageSections[p.Section]
                        .Categories[p.Category]
                        .Subcategories[p.Subcategory]
                        .Packages
                        .Add(p.ID);

                    manifest
                        .Packages
                        .Add(p.ID, p);
                }

                manifest.Packageset = packagesetName;

                manifests.Add(packagesetName, manifest);

            }

            ActiveRuntime.Manifests = manifests;

            return true;
        }

        public static IHostBuilder CreateHostBuilder(string[] args) {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
