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
    public partial class FormViewSections : Form {
        public FormViewSections() {
            InitializeComponent();
            foreach (Platform k in G.manifest.Platforms.Values) ddlPlatforms.Items.Add(k);
            RefreshListing();
        }

        private void RefreshListing() {
            lvwSections.Clear();
            if (ddlPlatforms.SelectedItem == null) return;
            foreach(PackageSection k in ((Platform)ddlPlatforms.SelectedItem).PackageSections) {
                ListViewItem i = new ListViewItem(k.Name);
                i.Tag = k;
                lvwSections.Items.Add(i);
            }
        }

        private void btnNew_Click(object sender, EventArgs e) {
            if (ddlPlatforms.SelectedItem == null) {
                MessageBox.Show("Select a platform for the section", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            new FormEditSection((Platform)ddlPlatforms.SelectedItem).ShowDialog();
            RefreshListing();
        }

        private void btnEdit_Click(object sender, EventArgs e) {
            if (lvwSections.SelectedItems.Count < 1) return;
            new FormEditSection((Platform) ddlPlatforms.SelectedItem, (PackageSection) lvwSections.SelectedItems[0].Tag).ShowDialog();
            RefreshListing();
        }

        private void btnDelete_Click(object sender, EventArgs e) {
            if (lvwSections.SelectedItems.Count < 1) return;
            ((Platform)ddlPlatforms.SelectedItem).PackageSections.Remove((PackageSection)lvwSections.SelectedItems[0].Tag);
            RefreshListing();
        }

        private void btnClose_Click(object sender, EventArgs e) {
            Close();
        }

        private void ddlPlatforms_SelectedIndexChanged(object sender, EventArgs e) {
            RefreshListing();
        }
    }
}
