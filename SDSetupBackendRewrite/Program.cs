using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
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
using SDSetupBackendRewrite.Data;
using SDSetupBackendRewrite.Data.Accounts;
using SDSetupCommon;

namespace SDSetupBackendRewrite {
    public class Program {

        public static ILogger logger;

        public static Config ActiveConfig { get; private set; }
        public static Runtime ActiveRuntime { get; private set; }

        public static IUserDatabase Users = new JsonUserDatabase();

        public static void Main(string[] args) {
            logger = LoggerFactory.Create(o => {
                o.ClearProviders();
                o.AddConsole();
            }).CreateLogger("SDSetupBackend Application Output");
            logger.LogInformation("The backend server has begun initialization.");

            //attempt to load the configuration file from disk.
            if (!LoadConfigFromDisk()) {
                logger.LogError("Errors were detected while loading the configuration and initialization cannot continue. View the log for more details.");
                Environment.Exit(1);
            } else {
                logger.LogInformation("The configuration file loaded without errors.");
            }

            logger.LogInformation("Loading runtime information");

            if (!LoadRuntime()) {
                logger.LogError("Errors were detected while configuring runtime information and initialization cannot continue. View the log for more details.");
                Environment.Exit(1);
            } else {
                logger.LogInformation("The runtime was configured without errors.");
            }


            var host = CreateHostBuilder(args).Build();
            host.Run();
        }

        public static bool LoadRuntime() {
            Runtime runtime = new Runtime();
            runtime.privilegedUuid = Utilities.CreateCryptographicallySecureGuid().ToCleanString();
            ActiveRuntime = runtime;
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
            if (proposedConfig.UseUpdater) {
                if (!TimeSpan.TryParse(proposedConfig.UpdaterInterval, out _)) {
                    err = true;
                    logger.LogError("Failed to parse updater interval. Please ensure the syntax is correct. See https://docs.microsoft.com/en-us/dotnet/api/system.timespan.tryparse for examples.");
                }
                if (!File.Exists(proposedConfig.UpdaterPath)) {
                    err = true;
                    logger.LogError("Updater executable could not be found. Make sure to specify a valid path to the updater or disable autoupdates by setting UseUpdater to false.");
                }
            }
            if (proposedConfig.ValidChannels.Count() == 0) {
                err = true;
                logger.LogError("You need at least one valid package channel!");
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

            if (!err) ActiveConfig = proposedConfig;

            return !err;

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging => {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
