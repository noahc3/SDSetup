/* Copyright (c) 2019 noahc3
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SDSetupBackendControlPanel.Common;

namespace SDSetupBackendControlPanel {
    public partial class SetMasterPassword : Form {
        public SetMasterPassword() {
            InitializeComponent();
        }

        private void btnSetPassword_Click(object sender, EventArgs e) {
            if (txtPassword.Text != txtConfirmPassword.Text) {
                MessageBox.Show("Password inputs do not match!", "Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text)) {
                MessageBox.Show("Entered password cannot be blank.", "Blank", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string sha256sum = Security.SHA256Sum(txtPassword.Text);

            Program.config.MasterPasswordHash = sha256sum;
            Program.SaveConfig();

            Close();
        }
    }
}
