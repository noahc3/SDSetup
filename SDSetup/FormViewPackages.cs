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
    public partial class FormViewPackages : Form {
        public FormViewPackages() {
            InitializeComponent();
            RefreshPlatforms();
            RefreshSections();
            RefreshCategories();
            RefreshSubcategories();
            RefreshPackages();
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

        private void RefreshPackages() {
            lvwPackages.Clear();
            if ((PackageSubcategory)ddlSubcategories.SelectedItem == null) return;
            foreach (Package k in ((PackageSubcategory)ddlSubcategories.SelectedItem).Packages) {
                ListViewItem i = new ListViewItem(k.Name);
                i.Tag = k;
                lvwPackages.Items.Add(i);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e) {
            if (lvwPackages.SelectedItems.Count < 1) return;
            ((PackageSubcategory)ddlSubcategories.SelectedItem).Packages.Remove((Package)lvwPackages.SelectedItems[0].Tag);
            RefreshPackages();
        }

        private void btnClose_Click(object sender, EventArgs e) {
            Close();
        }

        private void ddlPlatforms_SelectedValueChanged(object sender, EventArgs e) {
            RefreshSections();
            RefreshCategories();
            RefreshSubcategories();
            RefreshPackages();
        }

        private void ddlSections_SelectedValueChanged(object sender, EventArgs e) {
            RefreshCategories();
            RefreshSubcategories();
            RefreshPackages();
        }

        private void ddlCategories_SelectedValueChanged(object sender, EventArgs e) {
            RefreshSubcategories();
            RefreshPackages();
        }

        private void ddlSubcategories_SelectedValueChanged(object sender, EventArgs e) {
            RefreshPackages();
        }

        private void btnNew_Click(object sender, EventArgs e) {
            if (ddlSubcategories.SelectedItem == null) {
                MessageBox.Show("Select a subcategory!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            new FormEditPackage((Platform)ddlPlatforms.SelectedItem, (PackageSection)ddlSections.SelectedItem, (PackageCategory)ddlCategories.SelectedItem, (PackageSubcategory)ddlSubcategories.SelectedItem).ShowDialog();
            RefreshPackages();
        }

        private void btnEdit_Click(object sender, EventArgs e) {
            if (lvwPackages.SelectedItems.Count < 1) return;
            new FormEditPackage((Platform)ddlPlatforms.SelectedItem, (PackageSection)ddlSections.SelectedItem, (PackageCategory)ddlCategories.SelectedItem, (PackageSubcategory)ddlSubcategories.SelectedItem, (Package) lvwPackages.SelectedItems[0].Tag).ShowDialog();
            RefreshPackages();
        }
    }
}
