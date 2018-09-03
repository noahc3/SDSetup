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
    public partial class FormViewCategories : Form {
        public FormViewCategories() {
            InitializeComponent();
            RefreshPlatforms();
            RefreshSections();
            RefreshCategories();
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
            lvwCategories.Clear();
            if ((PackageSection)ddlSections.SelectedItem == null) return;
            foreach (PackageCategory k in ((PackageSection)ddlSections.SelectedItem).Categories) {
                ListViewItem i = new ListViewItem(k.Name);
                i.Tag = k;
                lvwCategories.Items.Add(i);
            }
        }

        private void ddlPlatforms_SelectedValueChanged(object sender, EventArgs e) {
            RefreshSections();
            RefreshCategories();
        }

        private void ddlSections_SelectedValueChanged(object sender, EventArgs e) {
            RefreshCategories();
        }

        private void btnClose_Click(object sender, EventArgs e) {
            Close();
        }

        private void btnDelete_Click(object sender, EventArgs e) {
            if (lvwCategories.SelectedItems.Count < 1) return;
            ((PackageSection)ddlSections.SelectedItem).Categories.Remove((PackageCategory)lvwCategories.SelectedItems[0].Tag);
            RefreshCategories();
        }

        private void btnNew_Click(object sender, EventArgs e) {
            new FormEditCategory((Platform)ddlPlatforms.SelectedItem, (PackageSection)ddlSections.SelectedItem).ShowDialog();
            RefreshCategories();
        }

        private void btnEdit_Click(object sender, EventArgs e) {
            if (lvwCategories.SelectedItems.Count < 1) return;
            new FormEditCategory((Platform)ddlPlatforms.SelectedItem, (PackageSection)ddlSections.SelectedItem, (PackageCategory)lvwCategories.SelectedItems[0].Tag).ShowDialog();
            RefreshCategories();
        }
    }
}
