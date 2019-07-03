/* Copyright (c) 2019 noahc3
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDSetupBackendControlPanel.Types {
    class Config {
        public string MasterPasswordHash;
        public string CryptoSanityCheck = "f1a028bbc85240e0aafab336070861a5";
        public const string _CryptoSanityCheck = "f1a028bbc85240e0aafab336070861a5";
        public Dictionary<string, ServerConfig> Servers;

        public string GuideSourceDirectory = "";
        public string BackendSourceDirectory = "";

        public Config() {
            MasterPasswordHash = "";
            Servers = new Dictionary<string, ServerConfig>();
            GuideSourceDirectory = "";
            BackendSourceDirectory = "";
        }

        public Config(Config template) {
            MasterPasswordHash = template.MasterPasswordHash;
            CryptoSanityCheck = template.CryptoSanityCheck;
            Servers = new Dictionary<string, ServerConfig>();

            foreach (ServerConfig k in template.Servers.Values) {
                Servers[k.UUID] = new ServerConfig(k);
            }

            GuideSourceDirectory = template.GuideSourceDirectory;
            BackendSourceDirectory = template.BackendSourceDirectory;
        }
    }
}
