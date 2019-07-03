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
using SDSetupBackendControlPanel.Types;

namespace SDSetupBackendControlPanel {
    public partial class ServerEditor : Form {

        private ServerConfig serverConfig;

        public ServerEditor() {
            InitializeComponent();

            ConfigureBindings();

            serverConfig = new ServerConfig();
            SetData(serverConfig);
        }

        public ServerEditor(ServerConfig conf) {
            InitializeComponent();

            ConfigureBindings();

            serverConfig = new ServerConfig(conf);
            SetData(serverConfig);
        }

        private void ConfigureBindings() {
            Binding bindAskPassword = new Binding("Enabled", cbxAskPassword, "Checked");
            bindAskPassword.Parse += Common.Events.SwitchBool;
            bindAskPassword.Format += Common.Events.SwitchBool;
            txtPassword.DataBindings.Add(bindAskPassword);

            panelPrivateKey.DataBindings.Add("Enabled", cbxKeyBasedAuth, "Checked");

            Binding bindAskPassphrase = new Binding("Enabled", cbxAskPassphrase, "Checked");
            bindAskPassphrase.Parse += Common.Events.SwitchBool;
            bindAskPassphrase.Format += Common.Events.SwitchBool;
            txtPassphrase.DataBindings.Add(bindAskPassphrase);
        }

        private void SetData(ServerConfig conf) {
            txtHostname.Text = conf.Hostname;
            txtUsername.Text = conf.Username;
            txtPassword.Text = conf.Password;

            cbxKeyBasedAuth.Checked = conf.UsesKeyBasedAuthentication;
            cbxAskPassword.Checked = conf.AskForPasswordEachRun;

            txtKeyPath.Text = conf.PrivateKeyPath;
            txtPassphrase.Text = conf.PrivateKeyPassphrase;
            cbxAskPassphrase.Checked = conf.AskForPrivateKeyPassphraseEachRun;

            txtBackendDir.Text = conf.BackendDirectory;
            txtBackendTestingDir.Text = conf.BackendTestingDirectory;
            txtGuideDir.Text = conf.GuideDirectory;
            txtPubTestingDir.Text = conf.PublicTestingGuideDirectory;
            txtPrivTestingDir.Text = conf.PrivateTestingGuideDirectory;

            lblUUID.Text = $"{conf.UUID}";
        }

        private void SaveData(ServerConfig conf) {
            conf.Hostname = txtHostname.Text;
            conf.Username = txtUsername.Text;
            conf.Password = txtPassword.Text;

            conf.UsesKeyBasedAuthentication = cbxKeyBasedAuth.Checked;
            conf.AskForPasswordEachRun = cbxAskPassword.Checked;

            conf.PrivateKeyPath = txtKeyPath.Text;
            conf.PrivateKeyPassphrase = txtPassphrase.Text;
            conf.AskForPrivateKeyPassphraseEachRun = cbxAskPassphrase.Checked;

            conf.BackendDirectory = txtBackendDir.Text;
            conf.BackendTestingDirectory = txtBackendTestingDir.Text;
            conf.GuideDirectory = txtGuideDir.Text;
            conf.PublicTestingGuideDirectory = txtPubTestingDir.Text;
            conf.PrivateTestingGuideDirectory = txtPrivTestingDir.Text;

            conf.UUID = lblUUID.Text;
        }

        private void btnSave_Click(object sender, EventArgs e) {
            SaveData(serverConfig);
            ConnectionTest connTest = new ConnectionTest(serverConfig);
            connTest.ShowDialog();

            if (connTest.DialogResult == DialogResult.OK) {
                Program.config.Servers[serverConfig.UUID] = serverConfig;
                Program.SaveConfig();
                this.DialogResult = DialogResult.OK;
                Close();
            } else {
                MessageBox.Show($"The connection test failed!\n\n{connTest.LastMessage}");
            }
        }

        private void btnBrowsePrivateKey_Click(object sender, EventArgs e) {
            OpenFileDialog d = new OpenFileDialog();
            string path;

            if (d.ShowDialog() == DialogResult.OK) {
                path = d.FileName;
            } else {
                return;
            }

            txtKeyPath.Text = path;
        }
    }
}
