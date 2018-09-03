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
    public partial class FormEditSection : Form {

        Platform platform;
        PackageSection section;

        public FormEditSection(Platform platform) {
            InitializeComponent();

            this.platform = platform;

            foreach (Platform k in G.manifest.Platforms.Values) ddlPlatform.Items.Add(k);
            ddlPlatform.SelectedItem = platform;
        }

        public FormEditSection(Platform platform, PackageSection section) {
            InitializeComponent();

            this.platform = platform;
            this.section = section;

            txtId.Text = section.ID;
            txtName.Text = section.Name;
            txtDisplayName.Text = section.DisplayName;
            txtFooter.Text = section.Footer;
            nudListingMode.Value = section.ListingMode;
            nudWhenMode.Value = section.WhenMode;
            cbxVisible.Checked = section.Visible;
            foreach (string k in section.When) lvwWhenDependencies.Items.Add(k);

            foreach (Platform k in G.manifest.Platforms.Values) ddlPlatform.Items.Add(k);
            ddlPlatform.SelectedItem = platform;
        }

        private void btnDeleteWhen_Click(object sender, EventArgs e) {
            if (lvwWhenDependencies.SelectedItems.Count < 1) return;
            lvwWhenDependencies.Items.Remove(lvwWhenDependencies.SelectedItems[0]);
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

        private void btnSave_Click(object sender, EventArgs e) {
            if (section == null) {
                bool exists = false;
                foreach (PackageSection k in ((Platform)ddlPlatform.SelectedItem).PackageSections) {
                    if (k.ID == txtId.Text) {
                        exists = true;
                        break;
                    }
                }

                if (exists) {
                    MessageBox.Show("A section with that ID already exists in this platform!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                List<string> whens = new List<string>();
                foreach (ListViewItem k in lvwWhenDependencies.Items) whens.Add(k.Text);

                ((Platform)ddlPlatform.SelectedItem).PackageSections.Add(new PackageSection(txtId.Text, txtName.Text, txtDisplayName.Text, (int) nudListingMode.Value, cbxVisible.Checked, whens, (int) nudWhenMode.Value, new List<PackageCategory>(), txtFooter.Text));
            } else {
                bool exists = false;
                foreach (PackageSection k in ((Platform)ddlPlatform.SelectedItem).PackageSections) {
                    if (k.ID == txtId.Text) {
                        exists = true;
                        break;
                    }
                }

                if (exists && !((Platform)ddlPlatform.SelectedItem).Equals(platform)) {
                    MessageBox.Show("A section with that ID already exists in this platform!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (exists && ((Platform)ddlPlatform.SelectedItem).Equals(platform) && txtId.Text != section.ID) {
                    MessageBox.Show("A section with that ID already exists in this platform!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                section.ID = txtId.Text;
                section.Name = txtName.Text;
                section.DisplayName = txtDisplayName.Text;
                section.Footer = txtFooter.Text;
                section.ListingMode = (int)nudListingMode.Value;
                section.WhenMode = (int)nudWhenMode.Value;
                section.Visible = cbxVisible.Checked;

                List<string> whens = new List<string>();
                foreach (ListViewItem k in lvwWhenDependencies.Items) whens.Add(k.Text);

                section.When = whens;

                if (!((Platform)ddlPlatform.SelectedItem).Equals(platform)) {
                    platform.PackageSections.Remove(section);
                    ((Platform)ddlPlatform.SelectedItem).PackageSections.Add(section);
                }
            }

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            Close();
        }
    }
}
