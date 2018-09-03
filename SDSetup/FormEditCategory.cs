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
    public partial class FormEditCategory : Form {

        private Platform platform;
        private PackageSection section;
        private PackageCategory category;

        public FormEditCategory(Platform platform, PackageSection section) {
            InitializeComponent();

            this.platform = platform;
            this.section = section;

            RefreshPlatforms();
            ddlPlatforms.SelectedItem = platform;
            RefreshSections();
            ddlSections.SelectedItem = section;
        }

        public FormEditCategory(Platform platform, PackageSection section, PackageCategory category) {
            InitializeComponent();

            this.platform = platform;
            this.section = section;
            this.category = category;

            txtId.Text = category.ID;
            txtName.Text = category.Name;
            txtDisplayName.Text = category.DisplayName;
            cbxVisible.Checked = category.Visible;

            RefreshPlatforms();
            ddlPlatforms.SelectedItem = platform;
            RefreshSections();
            ddlSections.SelectedItem = section;

            foreach (string k in category.When) lvwWhenDependencies.Items.Add(k);
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

        private void btnCancel_Click(object sender, EventArgs e) {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e) {
            if ((PackageSection)ddlSections.SelectedItem == null) {
                MessageBox.Show("Please select a section!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (category == null) {
                bool exists = false;
                foreach (PackageCategory k in ((PackageSection)ddlSections.SelectedItem).Categories) {
                    if (k.ID == txtId.Text) {
                        exists = true;
                        break;
                    }
                }

                if (exists) {
                    MessageBox.Show("A category with that ID already exists in that section!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                List<string> whens = new List<string>();
                foreach (ListViewItem k in lvwWhenDependencies.Items) whens.Add(k.Text);

                ((PackageSection)ddlSections.SelectedItem).Categories.Add(new PackageCategory(txtId.Text, txtName.Text, txtDisplayName.Text, cbxVisible.Checked, whens, (int) nudWhenMode.Value, new List<PackageSubcategory>()));
            } else {
                bool exists = false;
                foreach (PackageCategory k in ((PackageSection)ddlSections.SelectedItem).Categories) {
                    if (k.ID == txtId.Text) {
                        exists = true;
                        break;
                    }
                }

                if (exists) {
                    if (txtId.Text != category.ID) {
                        MessageBox.Show("A category with that ID already exists in that section!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (txtId.Text == category.ID && (PackageSection)ddlSections.SelectedItem != section) {
                        MessageBox.Show("A category with that ID already exists in that section!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                if ((PackageSection)ddlSections.SelectedItem != section) {
                    section.Categories.Remove(category);
                    ((PackageSection)ddlSections.SelectedItem).Categories.Add(category);
                }

                category.ID = txtId.Text;
                category.Name = txtName.Text;
                category.DisplayName = txtDisplayName.Text;
                category.Visible = cbxVisible.Checked;

                List<string> whens = new List<string>();
                foreach (ListViewItem k in lvwWhenDependencies.Items) whens.Add(k.Text);

                category.When = whens;
            }

            Close();
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

        private void ddlPlatforms_SelectedValueChanged(object sender, EventArgs e) {
            RefreshSections();
        }
    }
}
