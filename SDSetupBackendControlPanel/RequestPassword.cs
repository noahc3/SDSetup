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

namespace SDSetupBackendControlPanel {
    public partial class RequestPassword : Form {

        public string password;

        public RequestPassword(int mode, string hostname, string username) {
            InitializeComponent();

            switch (mode) {
                case 1:
                    lblInfo.Text = $"Please enter your password\nHostname: {hostname}\nUser: {username}";
                    break;
                case 2:
                    lblInfo.Text = $"Please enter your private key passphrase\nHostname: {hostname}\nUser: {username}";
                    break;
            }


        }

        private void btnContinue_Click(object sender, EventArgs e) {
            password = txtInput.Text;
        }
    }
}
