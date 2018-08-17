using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;
using System.IO.Compression;

namespace SDSetupManifestGenerator {
    public partial class FormMain : Form {

        string oldId = "";
        Dictionary<string, Package> packages = new Dictionary<string, Package>();

        public FormMain() {
            InitializeComponent();

            G.SetFormMain(this);

            if (File.Exists(Environment.CurrentDirectory + "\\manifest.json")) {
                packages = JsonConvert.DeserializeObject<Dictionary<string, Package>>(File.ReadAllText(Environment.CurrentDirectory + "\\manifest.json"));
                PopulatePackages();
            }

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

        private void ToggleAuth(bool state) {
            txtUsername.Enabled = state;
            txtPassword.Enabled = state;
            btnLogin.Enabled = state;
        }

        private void ToggleMeta(bool state) {
            txtId.Enabled = state;
            txtName.Enabled = state;
            txtCat.Enabled = state;
            txtSubcat.Enabled = state;
            txtAuthors.Enabled = state;
            cbxEnabled.Enabled = state;
            //tvwItems.Enabled = state;
            btnNewPath.Enabled = state;
            btnEditPath.Enabled = state;
            btnDeletePath.Enabled = state;
            btnFetch.Enabled = state;
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

        private void PopulatePackages() {
            lbxPackages.Items.Clear();
            foreach (KeyValuePair<string, Package> k in packages) {
                lbxPackages.Items.Add(k.Value.id);
            }
        }

        private void PopulateInfo(Package package) {
            

            txtId.Text = package.id;
            txtName.Text = package.name;
            txtCat.Text = package.category;
            txtSubcat.Text = package.subcategory;
            cbxEnabled.Checked = package.enabledByDefault;
            txtAuthors.Text = package.authors;
            txtVersion.Text = package.version;
            txtSource.Text = package.source;
            txtDlUrl.Text = package.dlSource;

            tvwItems.Enabled = false;
            tvwItems.Nodes.Clear();

            //tvwItems.Nodes.Add(new TreeNode("sd", new TreeNode[] { new TreeNode("switch") }));
            //tvwItems.Nodes.Add(new TreeNode("pc", new TreeNode[] { new TreeNode("payloads") }));

            foreach (Artifact k in package.artifacts) {
                string[] nodes = k.dir.Replace('\\', '/').Split('/');
                TreeNode node;
                if (tvwItems.Nodes.ContainsKey(nodes[0])) node = tvwItems.Nodes[nodes[0]];
                else {
                    tvwItems.Nodes.Add(nodes[0], nodes[0]);
                    node = tvwItems.Nodes[nodes[0]];
                }
                for (int i = 1; i < nodes.Length; i++) {
                    if (node.Nodes.ContainsKey(nodes[i])) node = node.Nodes[nodes[i]];
                    else {
                        node.Nodes.Add(nodes[i], nodes[i]);
                        node = node.Nodes[nodes[i]];
                    }
                }

                node.Tag = k;
            }

            tvwItems.ExpandAll();
        }

        private void ToggleStart(bool state) {
            btnWrite.Enabled = state;
            btnNewPackage.Enabled = state;
            btnDeletePackage.Enabled = state;
            btnOpenPackage.Enabled = state;
            lbxPackages.Enabled = state;
        }

        private void btnOpenPackage_Click(object sender, EventArgs e) {
            if (lbxPackages.SelectedItems.Count > 0) {
                oldId = (string)lbxPackages.SelectedItems[0];
                PopulateInfo(packages[(string)lbxPackages.SelectedItems[0]]);
                ToggleMeta(true);
            }
            
        }

        private void btnNewPackage_Click(object sender, EventArgs e) {
            string guid = Guid.NewGuid().ToString().ToLower().Replace("-", "");
            packages[guid] = new Package(guid, "", "", "", "", "", "", false, null);
            PopulatePackages();
        }

        private void btnDeletePackage_Click(object sender, EventArgs e) {
            if (lbxPackages.SelectedItems.Count > 0) {
                packages.Remove((string)lbxPackages.SelectedItems[0]);
                PopulatePackages();
            }
        }

        private void btnFetch_Click(object sender, EventArgs e) {
            ToggleMeta(false);

            string id = txtId.Text;
            string rawUrl = txtDlUrl.Text;

            string[] rawArtifacts;
            List<Artifact> artifacts = new List<Artifact>();
            string user = "";

            if (rawUrl.ToLower().Contains("github.com/")) {
                G.log("Found Github package with ID: " + id);

                user = rawUrl.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[2];

                rawArtifacts = Git.GetLatestReleaseAssets(rawUrl);
            } else {
                G.log("Found direct package with ID: " + id);
                rawArtifacts = new string[] { rawUrl };
            }

            foreach (string url in rawArtifacts) {
                //TODO: needs cleanup oh god please
                Directory.CreateDirectory(Path.Combine(R.wd, id + "\\", ".temp\\"));
                G.DownloadFile(url, Path.Combine(R.wd, id + "\\", ".temp\\", url.Split('/').Last()));
                if (url.Replace("/", "").EndsWith(".zip")) {
                    ZipFile.ExtractToDirectory(Path.Combine(R.wd, id + "\\", ".temp\\", url.Split('/').Last()), Path.Combine(R.wd, id + "\\", ".temp\\." + url.Split('/').Last() + "\\"));
                    string[] files = G.GetAllFilesInDir(Path.Combine(R.wd, id + "\\", ".temp\\." + url.Split('/').Last() + "\\"));
                    foreach (string n in files) {
                        artifacts.Add(new Artifact("", "/" + n.Replace(Path.Combine(R.wd, id + "\\", ".temp\\." + url.Split('/').Last() + "\\"), "").Replace("\\", "/"), n.Replace("\\", "/").Split('/').Last(), n));
                    }
                    continue;
                }
                artifacts.Add(new Artifact("", "/" + url.Split('/').Last(), url.Split('/').Last(), Path.Combine(R.wd, id + "\\", ".temp\\", url.Split('/').Last())));
            }

            tvwItems.Nodes.Clear();

            tvwItems.Nodes.Add(new TreeNode("sd", new TreeNode[] { new TreeNode("switch") }));
            tvwItems.Nodes.Add(new TreeNode("pc", new TreeNode[] { new TreeNode("payloads") }));

            foreach (Artifact k in artifacts) {
                string[] nodes = k.dir.Replace('\\', '/').Split('/');
                TreeNode node;
                if (tvwItems.Nodes.ContainsKey(nodes[1])) node = tvwItems.Nodes[nodes[1]];
                else {
                    tvwItems.Nodes.Add(nodes[1], nodes[1]);
                    node = tvwItems.Nodes[nodes[1]];
                }
                for (int i = 2; i < nodes.Length; i++) {
                    if (node.Nodes.ContainsKey(nodes[i])) node = node.Nodes[nodes[i]];
                    else {
                        node.Nodes.Add(nodes[i], nodes[i]);
                        node = node.Nodes[nodes[i]];
                    }
                }

                node.Tag = k;
            }

            ToggleMeta(true);
        }

        private TreeNode[] GetAllNodes() {
            List<TreeNode> nodes = new List<TreeNode>();
            foreach (TreeNode k in tvwItems.Nodes) {
                nodes.AddRange(GetNodesInNode(k));
            }

            return nodes.ToArray();
        }

        private TreeNode[] GetNodesInNode(TreeNode node) {
            List<TreeNode> nodes = new List<TreeNode>();
            foreach (TreeNode k in node.Nodes) {
                nodes.AddRange(GetNodesInNode(k));
            }

            nodes.Add(node);

            return nodes.ToArray();
        }

        private void tvwItems_MouseUp(object sender, MouseEventArgs e) {
            Point pt = ((TreeView)sender).PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y));
            TreeNode DestinationNode = ((TreeView)sender).GetNodeAt(pt);
            if (DestinationNode == null) tvwItems.SelectedNode = DestinationNode;
        }

        private void tvwItems_DragDrop(object sender, DragEventArgs e) {
            TreeNode NewNode;

            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false)) {
                Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
                TreeNode DestinationNode = ((TreeView)sender).GetNodeAt(pt);
                NewNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
                if (DestinationNode != NewNode.Parent) {
                    if (DestinationNode != null && DestinationNode.Tag != null) DestinationNode = DestinationNode.Parent;

                    if (DestinationNode != null) {
                        DestinationNode.Nodes.Add((TreeNode)NewNode.Clone());
                        DestinationNode.Expand();
                    } else {
                        ((TreeView)sender).Nodes.Add((TreeNode)NewNode.Clone());
                    }

                    NewNode.Remove();
                }
            }
        }

        private void tvwItems_ItemDrag(object sender, ItemDragEventArgs e) {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void tvwItems_DragOver(object sender, DragEventArgs e) {
            Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
            TreeNode DestinationNode = ((TreeView)sender).GetNodeAt(pt);
            if (DestinationNode != null && DestinationNode.Tag != null) DestinationNode = DestinationNode.Parent;
            ((TreeView)sender).SelectedNode = DestinationNode;
            e.Effect = DragDropEffects.Move;
        }

        private void btnNewPath_Click(object sender, EventArgs e) {
            if (tvwItems.SelectedNode != null) {
                tvwItems.SelectedNode.Nodes.Add("New Path");
                tvwItems.SelectedNode = tvwItems.SelectedNode.LastNode;
                tvwItems.SelectedNode.BeginEdit();
            } else {
                tvwItems.Nodes.Add("New Path");
                tvwItems.SelectedNode = tvwItems.Nodes[tvwItems.Nodes.Count - 1];
                tvwItems.Nodes[tvwItems.Nodes.Count - 1].BeginEdit();
            }
        }

        private void btnEditPath_Click(object sender, EventArgs e) {
            if (tvwItems.SelectedNode != null) tvwItems.SelectedNode.BeginEdit();
        }

        private void btnDeletePath_Click(object sender, EventArgs e) {
            if (tvwItems.SelectedNode != null) tvwItems.SelectedNode.Remove();
        }

        private void btnSave_Click(object sender, EventArgs e) {
            bool updateArtifacts = tvwItems.Enabled;

            ToggleMeta(false);

            TreeNode[] nodes = GetAllNodes();

            List<Artifact> artifacts = new List<Artifact>();

            if (updateArtifacts) {
                foreach (TreeNode k in nodes) {
                    if (k.Tag != null) {
                        Artifact artifact = (Artifact)k.Tag;
                        artifact.dir = k.FullPath;
                        if (String.IsNullOrEmpty(txtURL.Text) || txtURL.Text.Last() != '/') txtURL.Text += '/';
                        artifact.url = txtURL.Text + txtId.Text + "/" + artifact.dir;

                        FileInfo fi = new FileInfo(Environment.CurrentDirectory + "\\OUTPUTDIR\\" + txtId.Text + "\\" + artifact.dir);
                        Directory.CreateDirectory(fi.DirectoryName);
                        File.Move(artifact.diskLocation, fi.FullName);
                        artifact.filename = fi.Name;
                        artifact.diskLocation = null;

                        G.log("Artifact from package " + txtId.Text + " moved to " + artifact.dir);

                        artifacts.Add(artifact);
                    }
                }
            } else artifacts = packages[oldId].artifacts.ToList();



            Package package = new Package(txtId.Text, txtName.Text, txtAuthors.Text, txtCat.Text, txtSubcat.Text, txtVersion.Text, txtSource.Text, txtDlUrl.Text, cbxEnabled.Checked, artifacts.ToArray());

            packages.Remove(oldId);
            packages[txtId.Text] = package;
            

            PopulatePackages();
        }

        private void btnWrite_Click(object sender, EventArgs e) {
            G.log("Writing manifest to manifest_out.json");
            File.WriteAllText(Environment.CurrentDirectory + "\\manifest.json", JsonConvert.SerializeObject(packages, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            G.log("Done writing manifest to manifest_out.json");
        }
    }
}
