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
    public partial class GetMasterPassword : Form {
        private string MasterPasswordHash;
        public string MasterPassword;
        public GetMasterPassword(string MasterPasswordHash) {
            InitializeComponent();

            this.MasterPasswordHash = MasterPasswordHash;
        }

        private void btnSetPassword_Click(object sender, EventArgs e) {
            if (Security.SHA256Sum(txtPassword.Text) == MasterPasswordHash) {
                MasterPassword = txtPassword.Text;
                DialogResult = DialogResult.OK;
                Close();
            } else {
                txtPassword.Text = "";
                lblPasswordValid.Visible = true;
            }
        }
    }
}
