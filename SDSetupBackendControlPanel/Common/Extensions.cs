using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDSetupBackendControlPanel.Common {
    static class Extensions {
        public static string ToCleanUuidString(this Guid g) {
            return g.ToString().ToLower().Replace("-", "");
        }
    }
}
