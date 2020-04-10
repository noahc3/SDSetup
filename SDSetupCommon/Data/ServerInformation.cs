using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDSetupCommon.Data {
    public class ServerInformation {
        public string Hostname { get; set; } = "http://files.sdsetup.com";
        public string UUID { get; set; } = "";
        public string Name { get; set; } = "";
        public string Version { get; set; } = "";
        public string Commit { get; set; } = "";
        public string Status { get; set; } = "";
        public bool PrivilegedUUID { get; set; } = false;

        public static ServerInformation getPopulatedServerInformation() {
            return new ServerInformation {
                Name = "SDSetup Backend Server",
                Version = "v2.0 dev",
                Commit = "unknown",
                Status = "Operational"
            };
        }
    }
}
