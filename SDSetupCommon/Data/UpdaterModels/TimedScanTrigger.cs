using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SDSetupCommon.Data.UpdaterModels {
    public class TimedScanTrigger : UpdaterTrigger {
        public int Hours = 12;

        public async Task Register() {
            //STUB: Implement timed scan registry
        }
    }
}
