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
    public partial class FormViewSubcategories : Form {
        public FormViewSubcategories() {
            InitializeComponent();
            RefreshPlatforms();
            RefreshSections();
            RefreshCategories();
            RefreshSubcategories();
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
            lvwSubcategories.Clear();
            if ((PackageCategory)ddlCategories.SelectedItem == null) return;
            foreach (PackageSubcategory k in ((PackageCategory)ddlCategories.SelectedItem).Subcategories) {
                ListViewItem i = new ListViewItem(k.Name);
                i.Tag = k;
                lvwSubcategories.Items.Add(i);
            }
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

        private void btnClose_Click(object sender, EventArgs e) {
            Close();
        }

        private void btnDelete_Click(object sender, EventArgs e) {
            if (lvwSubcategories.SelectedItems.Count < 1) return;
            ((PackageCategory)ddlCategories.SelectedItem).Subcategories.Remove((PackageSubcategory)lvwSubcategories.SelectedItems[0].Tag);
            RefreshSubcategories();
        }

        private void btnNew_Click(object sender, EventArgs e) {
            if (ddlCategories.SelectedItem == null) {
                MessageBox.Show("Select a category!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            new FormEditSubcategory((Platform)ddlPlatforms.SelectedItem, (PackageSection)ddlSections.SelectedItem, (PackageCategory) ddlCategories.SelectedItem).ShowDialog();
            RefreshSubcategories();
        }

        private void btnEdit_Click(object sender, EventArgs e) {
            if (lvwSubcategories.SelectedItems.Count < 1) return;
            new FormEditSubcategory((Platform)ddlPlatforms.SelectedItem, (PackageSection)ddlSections.SelectedItem, (PackageCategory)ddlCategories.SelectedItem, (PackageSubcategory)lvwSubcategories.SelectedItems[0].Tag).ShowDialog();
            RefreshSubcategories();
        }
    }
}
