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
using System.IO.Compression;

namespace SDSetupManifestGenerator {
    public partial class Form1 : Form {

        int pI = 0;
        string[] inputs;
        Dictionary<string, Package> packages = new Dictionary<string, Package>();

        public Form1() {
            InitializeComponent();
            G.SetForm1(this);

            ToggleAuth(false);
            int result = Git.AuthCachedUnsafe();
            if (result == -1) {
                G.log("GITHUB (WARN): UNSAFE CACHED AUTHENTICATION ENABLED! YOUR USERNAME AND PASSWORD ARE STORED IN PLAIN TEXT ON DISK!");
                lblGit.Text = "Auth Fail! Check your username and password.";
                lblGit.ForeColor = Color.Red;
                G.log("GITHUB (WARN): UNSAFE CACHED AUTHENTICATION ENABLED! YOUR USERNAME AND PASSWORD ARE STORED IN PLAIN TEXT ON DISK!");
                G.log("GITHUB: Auth Fail! Username or password incorrect.");
                ToggleAuth(true);
            } else if (result == 0) {
                lblGit.Text = "Auth Success but Out of Requests!";
                lblGit.ForeColor = Color.Orange;
                G.log("GITHUB (WARN): UNSAFE CACHED AUTHENTICATION ENABLED! YOUR USERNAME AND PASSWORD ARE STORED IN PLAIN TEXT ON DISK!");
                G.log("GITHUB: No requests available! Please wait an hour.");
                ToggleAuth(true);
            } else if (result == -2) {
                G.log("GITHUB (WARN): UNSAFE CACHED AUTHENTICATION ENABLED! YOUR USERNAME AND PASSWORD ARE STORED IN PLAIN TEXT ON DISK!");
                G.log("GITHUB: No unsafe auth info stored.");
                ToggleAuth(true);
            } else if (result == -3) {
                G.log("GITHUB: Unsafe auth disabled.");
                ToggleAuth(true);
            } else {
                lblGit.Text = "Auth Success! Remaining Requests: " + result;
                lblGit.ForeColor = Color.Green;
                G.log("GITHUB (WARN): UNSAFE CACHED AUTHENTICATION ENABLED! YOUR USERNAME AND PASSWORD ARE STORED IN PLAIN TEXT ON DISK!");
                G.log("GITHUB: Auth Success! Remaining Requests: " + result);
                ToggleStart(true);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e) {
            ToggleAuth(false);
            G.log("GITHUB: Attempting authentication...");
            int result = Git.Auth(txtUsername.Text, txtPassword.Text);
            if (result == -1) {
                lblGit.Text = "Auth Fail! Check your username and password.";
                lblGit.ForeColor = Color.Red;
                G.log("GITHUB: Auth Fail! Username or password incorrect");
                ToggleAuth(true);
            } else if (result == 0) {
                lblGit.Text = "Auth Success but Out of Requests!";
                lblGit.ForeColor = Color.Orange;
                G.log("GITHUB: No requests available! Please wait an hour.");
                ToggleAuth(true);
            } else {
                lblGit.Text = "Auth Success! Remaining Requests: " + result;
                lblGit.ForeColor = Color.Green;
                G.log("GITHUB: Auth Success! Remaining Requests: " + result);
                ToggleStart(true);
            }
        }

        private void btnStart_Click(object sender, EventArgs e) {
            ToggleStart(false);
            G.log("Let the games begin");
            if(!File.Exists(Path.Combine(R.wd, "input.txt"))) {
                G.log("Failed! No input.txt in data directory!");
                G.log(Path.Combine(R.wd, "input.txt"));
                ToggleStart(true);
                return;
            }
            inputs = File.ReadAllLines(Path.Combine(R.wd, "input.txt"));
            G.log("Found input.txt! Entries found: " + inputs.Length);

            packages = new Dictionary<string, Package>();

            NextPackage();
        }

        private void NextPackage() {
            lblProgress.Text = "Progress: Package " + (pI + 1) + " of " + inputs.Length;
            string k = inputs[pI];

            string id = k.Split('=')[0];
            string rawUrl = k.Substring(id.Length + 1);

            if (rawUrl.ToLower().Contains("github.com/")) {
                G.log("Found Github package with ID: " + id);

                string user = rawUrl.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[2];

                string[] rawArtifacts = Git.GetLatestReleaseAssets(rawUrl);
                List<Artifact> artifacts = new List<Artifact>();

                foreach (string url in rawArtifacts) {
                    Directory.CreateDirectory(Path.Combine(R.wd, id + "\\", ".temp\\"));
                    G.DownloadFile(url, Path.Combine(R.wd, id + "\\", ".temp\\", url.Split('/').Last()));
                    if (url.Replace("/", "").EndsWith(".zip")) {
                        ZipFile.ExtractToDirectory(Path.Combine(R.wd, id + "\\", ".temp\\", url.Split('/').Last()), Path.Combine(R.wd, id + "\\", ".temp\\." + url.Split('/').Last() + "\\"));
                        string[] files = G.GetAllFilesInDir(Path.Combine(R.wd, id + "\\", ".temp\\." + url.Split('/').Last() + "\\"));
                        foreach(string n in files) {
                            artifacts.Add(new Artifact("", "/" + n.Replace(Path.Combine(R.wd, id + "\\", ".temp\\." + url.Split('/').Last() + "\\"), "").Replace("\\", "/"), n.Replace("\\", "/").Split('/').Last()));
                        }
                        continue;
                    }
                    artifacts.Add(new Artifact("", "/" + url.Split('/').Last(), url.Split('/').Last()));
                }

                Package p = new Package(id, "", user, "", "", false, artifacts.ToArray());

                PopulateFields(p);

                ToggleMeta(true);
            }
        }

        private void PopulateFields(Package package) {
            txtId.Text = package.id;
            txtName.Text = package.name;
            txtCat.Text = package.category;
            txtSubcat.Text = package.subcategory;
            cbxEnabled.Checked = package.enabledByDefault;
            txtAuthors.Text = package.authors;

            foreach(Artifact k in package.artifacts) {
                cbxlItems.Items.Add(k.filename);
                lbxDirs.Items.Add(k.dir);
            }
        }

        private void ToggleAuth(bool state) {
            txtUsername.Enabled = state;
            txtPassword.Enabled = state;
            btnLogin.Enabled = state;
        }

        private void ToggleStart(bool state) {
            btnStart.Enabled = state;
        }

        private void ToggleMeta(bool state) {
            txtId.Enabled = state;
            txtName.Enabled = state;
            txtCat.Enabled = state;
            txtSubcat.Enabled = state;
            txtAuthors.Enabled = state;
            cbxEnabled.Enabled = state;
            cbxlItems.Enabled = state;
            lbxDirs.Enabled = state;
            btnNext.Enabled = state;
        }
    }
}
