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
