/* Copyright (c) 2019 noahc3
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using SDSetupBackendControlPanel.Types;
using SDSetupBackendControlPanel.Common;
using Newtonsoft.Json;

namespace SDSetupBackendControlPanel {
    static class Program {

        private const string CONFIG_LOCATION = "cpconf.json";

        public static Config config;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (File.Exists(CONFIG_LOCATION)) config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(CONFIG_LOCATION));
            else {
                config = new Config();
                File.WriteAllText(CONFIG_LOCATION, JsonConvert.SerializeObject(config, Formatting.Indented));
            }

            if (!String.IsNullOrWhiteSpace(config.MasterPasswordHash)) config = Security.DecryptSensitiveData(config);

            Application.Run(new Form1());
        }

        public static void SaveConfig() {
            if (String.IsNullOrWhiteSpace(config.MasterPasswordHash)) File.WriteAllText(CONFIG_LOCATION, JsonConvert.SerializeObject(config, Formatting.Indented));
            else File.WriteAllText(CONFIG_LOCATION, JsonConvert.SerializeObject(Security.EncryptSensitiveData(config), Formatting.Indented));
        }
    }
}
