using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDSetupBackendRewrite.Providers {
    public class Security {
        public static bool IsUUIDPrivileged(string uuid) {
            return Program.ActiveRuntime.privilegedUuid.Contains(uuid);
        }
    }
}
