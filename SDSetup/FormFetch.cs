/* Copyright (c) 2018 noahc3
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
using System.IO;
using System.IO.Compression;
using Microsoft.VisualBasic.FileIO;

namespace SDSetupManifestGenerator {
    public partial class FormFetch : Form {

        private string DefaultURL, Version, ID;
        public List<Artifact> OUTPUT;

        public FormFetch(string DefaultURL, string Version, string ID) {
            InitializeComponent();

            this.DefaultURL = DefaultURL;
            this.Version = Version;
            this.ID = ID;

            //tvwItems.Nodes.Add(new TreeNode("sd", new TreeNode[] { new TreeNode("switch"), new TreeNode("retroarch", new TreeNode[] { new TreeNode("cores", new TreeNode[] { new TreeNode("switch") })})}));
            tvwItems.Nodes.Add(new TreeNode("pc", new TreeNode[] { new TreeNode("payloads") }));
            tvwItems.Nodes.Add(new TreeNode("sd", new TreeNode[] { new TreeNode("switch") }));
            //tvwItems.Nodes.Add(new TreeNode("sd", new TreeNode[] { new TreeNode("3ds"), new TreeNode("cias") }));
            tvwItems.ExpandAll();

            txtUrl.Text = DefaultURL;

            txtUploadUrl.Text = "https://cdn.rawgit.com/noahc3/SDSetupFiles/master/";
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

        private void tvwItems_DragOver(object sender, DragEventArgs e) {
            Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
            TreeNode DestinationNode = ((TreeView)sender).GetNodeAt(pt);
            if (DestinationNode != null && DestinationNode.Tag != null) DestinationNode = DestinationNode.Parent;
            ((TreeView)sender).SelectedNode = DestinationNode;
            e.Effect = DragDropEffects.Move;
        }

        private void tvwItems_ItemDrag(object sender, ItemDragEventArgs e) {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void tvwItems_MouseUp(object sender, MouseEventArgs e) {
            Point pt = ((TreeView)sender).PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y));
            TreeNode DestinationNode = ((TreeView)sender).GetNodeAt(pt);
            if (DestinationNode == null) tvwItems.SelectedNode = DestinationNode;
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

        private void btnFetchUrl_Click(object sender, EventArgs e) {
            string id = ID;
            string version = Version;
            string rawUrl = txtUrl.Text;

            string[] rawArtifacts;
            List<Artifact> artifacts = new List<Artifact>();
            string user = "";

            if (rawUrl.ToLower().Contains("github.com/")) {

                user = rawUrl.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[2];

                rawArtifacts = Git.GetLatestReleaseAssets(rawUrl);
            } else {
                rawArtifacts = new string[] { rawUrl };
            }

            if (Directory.Exists(Path.Combine(R.wd, id + "\\"))) Directory.Delete(Path.Combine(R.wd, id + "\\"), true);
            Directory.CreateDirectory(Path.Combine(R.wd, id + "\\", ".temp\\"));

            foreach (string url in rawArtifacts) {
                //TODO: needs cleanup oh god please
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
            //tvwItems.Nodes.Clear();
            foreach (Artifact k in artifacts) {
                string[] nodes = k.Directory.Replace('\\', '/').Split('/');
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


            tvwItems.Enabled = true;
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

        private void btnSave_Click(object sender, EventArgs e) {
            string id = ID;
            string version = Version.Replace(" ", "");

            TreeNode[] nodes = GetAllNodes();

            List<Artifact> artifacts = new List<Artifact>();

            if (Directory.Exists(Environment.CurrentDirectory + "\\OUTPUTDIR\\" + id + "\\" + version + "\\")) Directory.Delete(Environment.CurrentDirectory + "\\OUTPUTDIR\\" + id + "\\" + version + "\\", true);
            foreach (TreeNode k in nodes) {
                if (k.Tag != null) {
                    Artifact artifact = (Artifact)k.Tag;
                    artifact.Directory = k.FullPath;
                    if (String.IsNullOrEmpty(txtUploadUrl.Text) || txtUploadUrl.Text.Last() != '/') txtUploadUrl.Text += '/';
                    artifact.URL = txtUploadUrl.Text + id + "/" + version + "/" + artifact.Directory;

                    FileInfo fi = new FileInfo(Environment.CurrentDirectory + "\\OUTPUTDIR\\" + id + "\\" + version + "\\" + artifact.Directory);
                    Directory.CreateDirectory(fi.DirectoryName);
                    File.Move(artifact.DiskLocation, fi.FullName);
                    artifact.FileName = fi.Name;
                    artifact.DiskLocation = null;


                    artifacts.Add(artifact);
                }
            }

            this.OUTPUT = artifacts;

            Close();

        }

        private void btnFetchFromFile_Click(object sender, EventArgs e) {
            List<Artifact> artifacts = new List<Artifact>();

            OpenFileDialog d = new OpenFileDialog();
            string path;

            if (d.ShowDialog() == DialogResult.OK) {
                path = d.FileName;
            } else {
                return;
            }

            string id = ID;
            string version = Version;
            string url = path.Replace("\\", "/");

            if (Directory.Exists(Path.Combine(R.wd, id + "\\"))) FileSystem.DeleteDirectory(Path.Combine(R.wd, id + "\\"), DeleteDirectoryOption.DeleteAllContents);
            Directory.CreateDirectory(Path.Combine(R.wd, id + "\\", ".temp\\"));

            FileSystem.CopyFile(path, R.wd + id + "\\.temp\\" + url.Split('/').Last(), true);

            //TODO: needs cleanup oh god please
            string[] files = new string[1] { R.wd + id + "\\.temp\\" + url.Split('/').Last() };
            foreach (string n in files) {
                artifacts.Add(new Artifact("", "/" + n.Replace("/", "\\").Replace(Path.Combine(R.wd, id + "\\", ".temp\\"), "").Replace("\\", "/"), n.Replace("\\", "/").Split('/').Last(), n));
            }

            //tvwItems.Nodes.Clear();
            foreach (Artifact k in artifacts) {
                string[] nodes = k.Directory.Replace('\\', '/').Split('/');
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


            tvwItems.Enabled = true;
        }

        private void btnFetchFolder_Click(object sender, EventArgs e) {

            List<Artifact> artifacts = new List<Artifact>();

            FolderBrowserDialog d = new FolderBrowserDialog();
            string path;

            if (d.ShowDialog() == DialogResult.OK) {
                path = d.SelectedPath;
            } else {
                return;
            }

            string id = ID;
            string version = Version;
            string url = path.Replace("\\", "/");

            if (Directory.Exists(Path.Combine(R.wd, id + "\\"))) Directory.Delete(Path.Combine(R.wd, id + "\\"), true);
            Directory.CreateDirectory(Path.Combine(R.wd, id + "\\", ".temp\\"));

            FileSystem.CopyDirectory(path, Path.Combine(R.wd, id + "\\", ".temp\\." + url.Split('/').Last() + "\\"), true);

            //TODO: needs cleanup oh god please
            string[] files = G.GetAllFilesInDir(Path.Combine(R.wd, id + "\\", ".temp\\." + url.Split('/').Last() + "\\"));
            foreach (string n in files) {
                artifacts.Add(new Artifact("", "/" + n.Replace(Path.Combine(R.wd, id + "\\", ".temp\\." + url.Split('/').Last() + "\\"), "").Replace("\\", "/"), n.Replace("\\", "/").Split('/').Last(), n));
            }

            //tvwItems.Nodes.Clear();
            foreach (Artifact k in artifacts) {
                string[] nodes = k.Directory.Replace('\\', '/').Split('/');
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


            tvwItems.Enabled = true;
        }
    }
}
