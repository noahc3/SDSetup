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
    public partial class FormEditSubcategory : Form {
        private Platform platform;
        private PackageSection section;
        private PackageCategory category;
        private PackageSubcategory subcategory;

        public FormEditSubcategory(Platform platform, PackageSection section, PackageCategory category) {
            InitializeComponent();

            this.platform = platform;
            this.section = section;
            this.category = category;

            RefreshPlatforms();
            ddlPlatforms.SelectedItem = platform;
            RefreshSections();
            ddlSections.SelectedItem = section;
            RefreshCategories();
            ddlCategories.SelectedItem = category;
        }

        public FormEditSubcategory(Platform platform, PackageSection section, PackageCategory category, PackageSubcategory subcategory) {
            InitializeComponent();

            this.platform = platform;
            this.section = section;
            this.category = category;
            this.subcategory = subcategory;

            txtId.Text = subcategory.ID;
            txtName.Text = subcategory.Name;
            txtDisplayName.Text = subcategory.DisplayName;
            cbxVisible.Checked = subcategory.Visible;

            RefreshPlatforms();
            ddlPlatforms.SelectedItem = platform;
            RefreshSections();
            ddlSections.SelectedItem = section;
            RefreshCategories();
            ddlCategories.SelectedItem = category;

            foreach (string k in subcategory.When) lvwWhenDependencies.Items.Add(k);
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

        private void ddlPlatforms_SelectedValueChanged(object sender, EventArgs e) {
            RefreshSections();
            RefreshCategories();
        }

        private void ddlSections_SelectedValueChanged(object sender, EventArgs e) {
            RefreshCategories();
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

        private void btnDeleteWhen_Click(object sender, EventArgs e) {
            if (lvwWhenDependencies.SelectedItems.Count < 1) return;
            lvwWhenDependencies.Items.Remove(lvwWhenDependencies.SelectedItems[0]);
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e) {
            if ((PackageCategory)ddlCategories.SelectedItem == null) {
                MessageBox.Show("Please select a category!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (subcategory == null) {
                bool exists = false;
                foreach (PackageSubcategory k in ((PackageCategory)ddlCategories.SelectedItem).Subcategories) {
                    if (k.ID == txtId.Text) {
                        exists = true;
                        break;
                    }
                }

                if (exists) {
                    MessageBox.Show("A subcategory with that ID already exists in that category!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                List<string> whens = new List<string>();
                foreach (ListViewItem k in lvwWhenDependencies.Items) whens.Add(k.Text);

                ((PackageCategory)ddlCategories.SelectedItem).Subcategories.Add(new PackageSubcategory(txtId.Text, txtName.Text, txtDisplayName.Text, cbxVisible.Checked, whens, (int)nudWhenMode.Value, new List<Package>()));
            } else {
                bool exists = false;
                foreach (PackageSubcategory k in ((PackageCategory)ddlCategories.SelectedItem).Subcategories) {
                    if (k.ID == txtId.Text) {
                        exists = true;
                        break;
                    }
                }

                if (exists) {
                    if (txtId.Text != subcategory.ID) {
                        MessageBox.Show("A subcategory with that ID already exists in that category!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (txtId.Text == subcategory.ID && (PackageCategory)ddlCategories.SelectedItem != category) {
                        MessageBox.Show("A subcategory with that ID already exists in that category!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                if ((PackageCategory)ddlCategories.SelectedItem != category) {
                    category.Subcategories.Remove(subcategory);
                    ((PackageCategory)ddlCategories.SelectedItem).Subcategories.Add(subcategory);
                }

                subcategory.ID = txtId.Text;
                subcategory.Name = txtName.Text;
                subcategory.DisplayName = txtDisplayName.Text;
                subcategory.Visible = cbxVisible.Checked;

                List<string> whens = new List<string>();
                foreach (ListViewItem k in lvwWhenDependencies.Items) whens.Add(k.Text);

                subcategory.When = whens;
            }
            G.NeedsSaving = true;
            Close();
        }
    }
}
