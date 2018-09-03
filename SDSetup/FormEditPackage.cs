using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SDSetupManifestGenerator {
    public partial class FormEditPackage : Form {
        private Platform platform;
        private PackageSection section;
        private PackageCategory category;
        private PackageSubcategory subcategory;
        private Package package;

        List<Artifact> artifacts = new List<Artifact>();

        public FormEditPackage(Platform platform, PackageSection section, PackageCategory category, PackageSubcategory subcategory) {
            InitializeComponent();

            this.platform = platform;
            this.section = section;
            this.category = category;
            this.subcategory = subcategory;

            RefreshPlatforms();
            ddlPlatforms.SelectedItem = platform;
            RefreshSections();
            ddlSections.SelectedItem = section;
            RefreshCategories();
            ddlCategories.SelectedItem = category;
            RefreshSubcategories();
            ddlSubcategories.SelectedItem = subcategory;
        }

        public FormEditPackage(Platform platform, PackageSection section, PackageCategory category, PackageSubcategory subcategory, Package package) {
            InitializeComponent();

            this.platform = platform;
            this.section = section;
            this.category = category;
            this.subcategory = subcategory;
            this.package = package;

            txtId.Text = package.ID;
            txtName.Text = package.Name;
            txtDisplayName.Text = package.DisplayName;
            txtAuthors.Text = package.Authors;
            txtVersion.Text = package.Version;
            txtSource.Text = package.Source;
            txtDlSource.Text = package.DLSource;
            txtDescription.Text = package.Description;
            cbxVisible.Checked = package.Visible;
            cbxDefault.Checked = package.EnabledByDefault;
            nudWhenMode.Value = package.WhenMode;

            foreach (string k in package.When) lvwWhenDependencies.Items.Add(k);
            foreach (string k in package.Dependencies) lvwDependencies.Items.Add(k);
            foreach (Artifact k in package.Artifacts) artifacts.Add(new Artifact(k.URL, k.Directory, k.FileName, k.DiskLocation));

            RefreshPlatforms();
            ddlPlatforms.SelectedItem = platform;
            RefreshSections();
            ddlSections.SelectedItem = section;
            RefreshCategories();
            ddlCategories.SelectedItem = category;
            RefreshSubcategories();
            ddlSubcategories.SelectedItem = subcategory;
        }

        private void RefreshPlatforms() {
            ddlPlatforms.Items.Clear();
            foreach (Platform k in G.manifest.Platforms.Values) ddlPlatforms.Items.Add(k);
        }

        private void RefreshSections() {
            ddlSections.Items.Clear();
            if (((Platform)ddlPlatforms.SelectedItem) == null) return;
            foreach (PackageSection k in ((Platform)ddlPlatforms.SelectedItem).PackageSections) ddlSections.Items.Add(k);
        }

        private void RefreshCategories() {
            ddlCategories.Items.Clear();
            if (((PackageSection)ddlSections.SelectedItem) == null) return;
            foreach (PackageCategory k in ((PackageSection)ddlSections.SelectedItem).Categories) ddlCategories.Items.Add(k);
        }

        private void RefreshSubcategories() {
            ddlSubcategories.Items.Clear();
            if (((PackageCategory)ddlCategories.SelectedItem) == null) return;
            foreach (PackageSubcategory k in ((PackageCategory)ddlCategories.SelectedItem).Subcategories) ddlSubcategories.Items.Add(k);
        }

        private void btnAddWhen_Click(object sender, EventArgs e) {
            if (String.IsNullOrEmpty(txtWhen.Text) || String.IsNullOrWhiteSpace(txtWhen.Text)) {
                MessageBox.Show("Please enter a package name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool exists = false;
            foreach (ListViewItem k in lvwWhenDependencies.Items) {
                if (k.Text == txtWhen.Text) {
                    exists = true;
                    break;
                }
            }

            if (exists) {
                MessageBox.Show("That package is already a when dependency!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lvwWhenDependencies.Items.Add(new ListViewItem(txtWhen.Text));
            txtWhen.Text = "";
        }

        private void btnAddDependency_Click(object sender, EventArgs e) {
            if (String.IsNullOrEmpty(txtDependency.Text) || String.IsNullOrWhiteSpace(txtDependency.Text)) {
                MessageBox.Show("Please enter a package name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool exists = false;
            foreach (ListViewItem k in lvwDependencies.Items) {
                if (k.Text == txtDependency.Text) {
                    exists = true;
                    break;
                }
            }

            if (exists) {
                MessageBox.Show("That package is already a dependency!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lvwDependencies.Items.Add(txtDependency.Text);
            txtDependency.Text = "";
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e) {
            if ((PackageSubcategory)ddlSubcategories.SelectedItem == null) {
                MessageBox.Show("Please select a subcategory!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (package == null) {
                string location = "";
                bool exists = false;
                foreach(Platform plat in G.manifest.Platforms.Values) {
                    location = plat.ID;
                    foreach (PackageSection sec in plat.PackageSections) {
                        location += "/" + sec.ID;
                        foreach(PackageCategory cat in sec.Categories) {
                            location += "/" + cat.ID;
                            foreach(PackageSubcategory sub in cat.Subcategories) {
                                location += "/" + sub.ID;
                                foreach (Package p in sub.Packages) {
                                    location += "/" + p.ID;
                                    if (p.ID == txtId.Text) {
                                        exists = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                if (exists) {
                    MessageBox.Show("A package with that ID already exists! (" + location + ")", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                List<string> whens = new List<string>();
                foreach (ListViewItem k in lvwWhenDependencies.Items) whens.Add(k.Text);

                List<string> depends = new List<string>();
                foreach (ListViewItem k in lvwDependencies.Items) depends.Add(k.Text);

                ((PackageSubcategory)ddlSubcategories.SelectedItem).Packages.Add(new Package(txtId.Text, txtName.Text, txtDisplayName.Text, txtAuthors.Text, txtVersion.Text, txtSource.Text, txtDlSource.Text, cbxDefault.Checked, cbxVisible.Checked, txtDescription.Text, whens, (int)nudWhenMode.Value, depends, artifacts));
            } else {
                string location = "";
                bool exists = false;
                foreach (Platform plat in G.manifest.Platforms.Values) {
                    foreach (PackageSection sec in plat.PackageSections) {
                        foreach (PackageCategory cat in sec.Categories) {
                            foreach (PackageSubcategory sub in cat.Subcategories) {
                                foreach (Package p in sub.Packages) {
                                    if (p.ID == txtId.Text) {
                                        exists = true;
                                        location = plat.ID + "/" + sec.ID + "/" + cat.ID + "/" + sub.ID + "/" + p.ID;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                if (exists) {
                    if (location != platform.ID + "/" + section.ID + "/" + category.ID + "/" + subcategory.ID + "/" + package.ID) {
                        if (txtId.Text != package.ID) {
                            MessageBox.Show("A package with that ID already exists! (" + location + ")", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        if (txtId.Text == package.ID && (PackageSubcategory)ddlSubcategories.SelectedItem != subcategory) {
                            MessageBox.Show("A package with that ID already exists! (" + location + ")", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                if ((PackageSubcategory)ddlSubcategories.SelectedItem != subcategory) {
                    subcategory.Packages.Remove(package);
                    ((PackageSubcategory)ddlSubcategories.SelectedItem).Packages.Add(package);
                }

                package.ID = txtId.Text;
                package.Name = txtName.Text;
                package.DisplayName = txtDisplayName.Text;
                package.Authors = txtAuthors.Text;
                package.Version = txtVersion.Text;
                package.Source = txtSource.Text;
                package.DLSource = txtDlSource.Text;
                package.Description = txtDescription.Text;
                package.Visible = cbxVisible.Checked;
                package.EnabledByDefault = cbxDefault.Checked;
                package.WhenMode = (int) nudWhenMode.Value;
                package.Artifacts = artifacts;

                List<string> whens = new List<string>();
                foreach (ListViewItem k in lvwWhenDependencies.Items) whens.Add(k.Text);
                List<string> depends = new List<string>();
                foreach (ListViewItem k in lvwDependencies.Items) depends.Add(k.Text);

                package.When = whens;
                package.Dependencies = depends;
            }

            Close();
        }

        private void ddlPlatforms_SelectedValueChanged(object sender, EventArgs e) {
            RefreshSections();
            RefreshCategories();
            RefreshSubcategories();
        }

        private void ddlSections_SelectedValueChanged(object sender, EventArgs e) {
            RefreshCategories();
            RefreshSubcategories();
        }

        private void ddlCategories_SelectedValueChanged(object sender, EventArgs e) {
            RefreshSubcategories();
        }

        private void btnDeleteWhen_Click(object sender, EventArgs e) {
            if (lvwWhenDependencies.SelectedItems.Count < 1) return;
            lvwWhenDependencies.Items.Remove(lvwWhenDependencies.SelectedItems[0]);
        }

        private void btnDeleteDependency_Click(object sender, EventArgs e) {
            if (lvwDependencies.SelectedItems.Count < 1) return;
            lvwDependencies.Items.Remove(lvwDependencies.SelectedItems[0]);
        }

        private void btnFetch_Click(object sender, EventArgs e) {
            FormFetch frm = new FormFetch(txtDlSource.Text, txtVersion.Text, txtId.Text);
            frm.ShowDialog();
            artifacts = frm.OUTPUT;
        }
    }
}
