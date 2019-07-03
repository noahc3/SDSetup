/* Copyright (c) 2019 noahc3
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDSetupBackendControlPanel.Types;
using WinSCP;

namespace SDSetupBackendControlPanel.Common {
    public class ConnectionUtils {

        //process

        public static SessionOptions GetSessionOptions(ServerConfig conf) {

            conf = new ServerConfig(conf);

            if (conf.AskForPasswordEachRun) {
                conf.Password = RequestPassword(conf);
            }

            if (conf.AskForPrivateKeyPassphraseEachRun) {
                conf.PrivateKeyPassphrase = RequestPassphrase(conf);
            }

            SessionOptions opts = new SessionOptions {
                Protocol = Protocol.Sftp,
                HostName = conf.Hostname,
                UserName = conf.Username,
                Password = conf.Password,
                SshPrivateKeyPath = conf.PrivateKeyPath,
                PrivateKeyPassphrase = conf.PrivateKeyPassphrase,
                SshHostKeyFingerprint = null,
                GiveUpSecurityAndAcceptAnySshHostKey = true
            };

            return opts;
        }

        public static Session GetSession(SessionOptions opts) {
            Session session = new Session();
            session.Open(opts);

            return session;
        }

        //helpers

        public static string RequestPassword(ServerConfig conf) {
            RequestPassword req = new RequestPassword(1, conf.Hostname, conf.Username);
            req.ShowDialog();

            string password = req.password;

            req.Close();

            return password;
        }

        public static string RequestPassphrase(ServerConfig conf) {
            RequestPassword req = new RequestPassword(2, conf.Hostname, conf.Username);
            req.ShowDialog();

            string password = req.password;

            req.Close();

            return password;
        }
    }
}
