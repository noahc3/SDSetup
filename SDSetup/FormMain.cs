using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using Octokit;
using Newtonsoft.Json;

namespace SDSetupManifestGenerator {
    public partial class FormMain : Form {

        public FormMain() {
            InitializeComponent();

            int result = Git.AuthCachedUnsafe();
            if (result == -1) {
                lblGit.Text = "Auth Fail! Check your username and password.";
                lblGit.ForeColor = Color.Red;
            } else if (result == 0) {
                lblGit.Text = "Auth Success but Out of Requests!";
                lblGit.ForeColor = Color.Orange;
            } else if (result == -2) {
            } else if (result == -3) {
            } else {
                lblGit.Text = "Auth Success! Remaining Requests: " + result;
                lblGit.ForeColor = Color.Green;
            }

            if (File.Exists(R.m)) G.manifest = JsonConvert.DeserializeObject<Manifest>(File.ReadAllText(R.m));
            else G.manifest = new Manifest();
        }

        private void btnLogin_Click(object sender, EventArgs e) {
            int result = Git.Auth(txtUsername.Text, txtPassword.Text);
            if (result == -1) {
                lblGit.Text = "Auth Fail! Check your username and password.";
                lblGit.ForeColor = Color.Red;
            } else if (result == 0) {
                lblGit.Text = "Auth Success but Out of Requests!";
                lblGit.ForeColor = Color.Orange;
            } else {
                lblGit.Text = "Auth Success! Remaining Requests: " + result;
                lblGit.ForeColor = Color.Green;
            }
        }

        private void btnWriteManifest_Click(object sender, EventArgs e) {
            if (File.Exists(R.m)) File.Delete(R.m);
            File.WriteAllText(R.m, JsonConvert.SerializeObject(G.manifest, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
        }

        private void btnManifestInfo_Click(object sender, EventArgs e) {
            new FormManifest().ShowDialog();
        }

        private void btnPlatforms_Click(object sender, EventArgs e) {
            new FormViewPlatforms().ShowDialog();
        }

        private void btnSections_Click(object sender, EventArgs e) {
            new FormViewSections().ShowDialog();
        }

        private void btnCategories_Click(object sender, EventArgs e) {
            new FormViewCategories().ShowDialog();
        }
    }
}
