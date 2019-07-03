/* Copyright (c) 2019 noahc3
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */
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