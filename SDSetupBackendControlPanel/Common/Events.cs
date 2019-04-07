using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SDSetupBackendControlPanel.Common {
    static class Events {
        public static void SwitchBool(object sender, ConvertEventArgs e) {
            e.Value = !((bool)e.Value);
        }

        public static void ListboxIndexExists(object sender, ConvertEventArgs e) {
            e.Value = (int) e.Value >= 0;
        }
    }
}