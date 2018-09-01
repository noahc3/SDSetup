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
using Newtonsoft.Json;

namespace SDSetupManifestGenerator {
    public partial class FormAuthoringTool : Form {
        
        private Platform SelectedPlatform;
        private PackageSection SelectedSection;
        private PackageCategory SelectedCategory;
        private PackageSubcategory SelectedSubcategory;
        public static Package SelectedPackage;

        public Manifest manifest;

        public FormAuthoringTool() {
            InitializeComponent();

            G.tool = this;

            int result = Git.AuthCachedUnsafe();
            if (result == -1) {
                lblGit.Text = "Auth Fail! Check your username and password.";
                lblGit.ForeColor = Color.Red;
            } else if (result == 0) {
                lblGit.Text = "Auth Success but Out of Requests!";
                lblGit.ForeColor = Color.Orange;
            } else if (result == -2) {
            } else if (result == -3) {
            } else {
                lblGit.Text = "Auth Success! Remaining Requests: " + result;
                lblGit.ForeColor = Color.Green;
            }

            if (File.Exists(R.m)) manifest = JsonConvert.DeserializeObject<Manifest>(File.ReadAllText(R.m));
            else manifest = new Manifest();

            RefreshDisplay();
        }

        private void btnLogin_Click(object sender, EventArgs e) {
            int result = Git.Auth(txtUsername.Text, txtPassword.Text);
            if (result == -1) {
                lblGit.Text = "Auth Fail! Check your username and password.";
                lblGit.ForeColor = Color.Red;
            } else if (result == 0) {
                lblGit.Text = "Auth Success but Out of Requests!";
                lblGit.ForeColor = Color.Orange;
            } else {
                lblGit.Text = "Auth Success! Remaining Requests: " + result;
                lblGit.ForeColor = Color.Green;
            }
        }

        private void RefreshDisplay() {
            lvwPlatforms.Items.Clear();
            foreach (Platform k in manifest.Platforms.Values) {
                ListViewItem item = new ListViewItem(k.ID);
                item.Tag = k;
                lvwPlatforms.Items.Add(item);
            }

            if (SelectedPlatform != null) {
                txtPlatformID.Text = SelectedPlatform.ID;
                txtPlatformName.Text = SelectedPlatform.Name;
                txtPlatformMenu.Text = SelectedPlatform.MenuName;
                txtPlatformIcon.Text = SelectedPlatform.HomeIcon;

                lvwSections.Items.Clear();
                foreach (PackageSection k in SelectedPlatform.PackageSections) {
                    ListViewItem item = new ListViewItem(k.ID);
                    item.Tag = k;
                    lvwSections.Items.Add(item);
                }

                if (SelectedSection != null) {
                    txtSectionName.Text = SelectedSection.Name;
                    txtSectionDisplay.Text = SelectedSection.DisplayName;
                    txtSectionID.Text = SelectedSection.ID;
                    nudDisplayMode.Value = SelectedSection.ListingMode;
                    cbxSectionVisible.Checked = SelectedSection.Visible;

                    lvwCategories.Items.Clear();
                    foreach(PackageCategory k in SelectedSection.Categories) {
                        ListViewItem item = new ListViewItem(k.ID);
                        item.Tag = k;
                        lvwCategories.Items.Add(item);
                    }

                    if (SelectedCategory != null) {
                        txtCategoryName.Text = SelectedCategory.Name;
                        txtCategoryDisplay.Text = SelectedCategory.DisplayName;
                        txtCategoryID.Text = SelectedCategory.ID;
                        cbxCategoryVisible.Checked = SelectedCategory.Visible;

                        lvwSubcategories.Items.Clear();
                        foreach (PackageSubcategory k in SelectedCategory.Subcategories) {
                            ListViewItem item = new ListViewItem(k.ID);
                            item.Tag = k;
                            lvwSubcategories.Items.Add(item);
                        }

                        if (SelectedSubcategory != null) {
                            txtSubcategoryName.Text = SelectedSubcategory.Name;
                            txtSubcategoryDisplay.Text = SelectedSubcategory.DisplayName;
                            txtSubcategoryID.Text = SelectedSubcategory.ID;
                            cbxSubcategoryVisible.Checked = SelectedSubcategory.Visible;

                            lvwPackages.Items.Clear();
                            foreach(Package k in SelectedSubcategory.Packages) {
                                ListViewItem item = new ListViewItem(k.ID);
                                item.Tag = k;
                                lvwPackages.Items.Add(item);
                            }

                            if (SelectedPackage != null) {
                                txtPackageId.Text = SelectedPackage.ID;
                                txtPackageName.Text = SelectedPackage.Name;
                                txtPackageDisplay.Text = SelectedPackage.DisplayName;
                                cbxPackageVisible.Checked = SelectedPackage.Visible;
                                txtPackageAuthors.Text = SelectedPackage.Authors;
                                txtDescription.Text = SelectedPackage.Description;
                                txtPackageDlSource.Text = SelectedPackage.DLSource;
                                txtPackageSource.Text = SelectedPackage.Source;
                                txtPackageVersion.Text = SelectedPackage.Version;
                                cbxPackageDefault.Checked = SelectedPackage.EnabledByDefault;
                                lvwPackageDependencies.Items.Clear();
                                foreach (string k in SelectedPackage.Dependencies) {
                                    lvwPackageDependencies.Items.Add(k);
                                }
                            }
                        } else {
                            SelectedPackage = null;
                        }

                    } else {
                        SelectedPackage = null;
                        SelectedSubcategory = null;
                    }

                } else {
                    SelectedPackage = null;
                    SelectedCategory = null;
                    SelectedSubcategory = null;
                }

            } else {
                SelectedPackage = null;
                SelectedSection = null;
                SelectedCategory = null;
                SelectedSubcategory = null;
            }
        }

        private void btnPlatformSave_Click(object sender, EventArgs e) {
            if (SelectedPlatform == null) {
                manifest.Platforms[txtPlatformID.Text] = new Platform(txtPlatformName.Text, txtPlatformMenu.Text, txtPlatformIcon.Text, txtPlatformID.Text, new List<PackageSection>(), "", true);
            } else {
                SelectedPlatform.Name = txtPlatformName.Text;
                SelectedPlatform.MenuName = txtPlatformMenu.Text;
                SelectedPlatform.HomeIcon = txtPlatformIcon.Text;
                SelectedPlatform.ID = txtPlatformID.Text;
            }

            SelectedPlatform = null;

            RefreshDisplay();
        }

        private void btnPlatformOpen_Click(object sender, EventArgs e) {
            if (lvwPlatforms.SelectedItems.Count > 0) {
                SelectedPlatform = ((Platform)lvwPlatforms.SelectedItems[0].Tag);
                RefreshDisplay();
            }
        }

        private void btnWriteManifest_Click(object sender, EventArgs e) {
            if (File.Exists(R.m)) File.Delete(R.m);
            File.WriteAllText(R.m, JsonConvert.SerializeObject(manifest, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
        }

        private void btnSectionSave_Click(object sender, EventArgs e) {
            if (SelectedSection == null) {
                SelectedPlatform.PackageSections.Add(new PackageSection(txtSectionID.Text, txtSectionName.Text, txtSectionDisplay.Text, (int) nudDisplayMode.Value, cbxSectionVisible.Checked, new List<string>(), 0, new List<PackageCategory>(), ""));
            } else {
                SelectedSection.ID = txtSectionID.Text;
                SelectedSection.Name = txtSectionName.Text;
                SelectedSection.DisplayName = txtSectionDisplay.Text;
                SelectedSection.ListingMode = (int) nudDisplayMode.Value;
                SelectedSection.Visible = cbxSectionVisible.Checked;
            }

            SelectedSection = null;

            RefreshDisplay();
        }

        private void btnSectionOpen_Click(object sender, EventArgs e) {
            if (lvwSections.SelectedItems.Count > 0) {
                SelectedSection = ((PackageSection)lvwSections.SelectedItems[0].Tag);
                RefreshDisplay();
            }
        }

        private void btnCategorySave_Click(object sender, EventArgs e) {
            if (SelectedCategory == null) {
                SelectedSection.Categories.Add(new PackageCategory(txtCategoryID.Text, txtCategoryName.Text, txtCategoryDisplay.Text, cbxCategoryVisible.Checked, new List<string>(), 0, new List<PackageSubcategory>()));
            } else {
                SelectedCategory.ID = txtCategoryID.Text;
                SelectedCategory.Name = txtCategoryName.Text;
                SelectedCategory.DisplayName = txtCategoryDisplay.Text;
                SelectedCategory.Visible = cbxCategoryVisible.Checked;
            }

            SelectedCategory = null;

            RefreshDisplay();
        }

        private void btnCategoryOpen_Click(object sender, EventArgs e) {
            if (lvwCategories.SelectedItems.Count > 0) {
                SelectedCategory = ((PackageCategory)lvwCategories.SelectedItems[0].Tag);
                RefreshDisplay();
            }
        }

        private void btnSubcategorySave_Click(object sender, EventArgs e) {
            if (SelectedSubcategory == null) {
                SelectedCategory.Subcategories.Add(new PackageSubcategory(txtSubcategoryID.Text, txtSubcategoryName.Text, txtSubcategoryDisplay.Text, cbxSubcategoryVisible.Checked, new List<string>(), 0, new List<Package>()));
            } else {
                SelectedSubcategory.ID = txtSubcategoryID.Text;
                SelectedSubcategory.Name = txtSubcategoryName.Text;
                SelectedSubcategory.DisplayName = txtSubcategoryDisplay.Text;
                SelectedSubcategory.Visible = cbxSubcategoryVisible.Checked;
            }

            SelectedSubcategory = null;

            RefreshDisplay();
        }

        private void btnSubcategoryOpen_Click(object sender, EventArgs e) {
            if (lvwSubcategories.SelectedItems.Count > 0) {
                SelectedSubcategory = ((PackageSubcategory)lvwSubcategories.SelectedItems[0].Tag);
                RefreshDisplay();
            }
        }

        private void btnPackageSave_Click(object sender, EventArgs e) {
            if (SelectedPackage == null) {
                SelectedSubcategory.Packages.Add(new Package(txtPackageId.Text, txtPackageName.Text, txtPackageDisplay.Text, txtPackageAuthors.Text, txtPackageVersion.Text, txtPackageSource.Text, txtPackageDlSource.Text, cbxPackageDefault.Checked, cbxPackageVisible.Checked, txtDescription.Text, new List<string>(), 0, new List<string>(), new List<Artifact>()));
            } else {
                SelectedPackage.ID = txtPackageId.Text;
                SelectedPackage.Name = txtPackageName.Text;
                SelectedPackage.DisplayName = txtPackageDisplay.Text;
                SelectedPackage.Visible = cbxPackageVisible.Checked;
                SelectedPackage.Authors = txtPackageAuthors.Text;
                SelectedPackage.Description = txtDescription.Text;
                SelectedPackage.DLSource = txtPackageDlSource.Text;
                SelectedPackage.Source = txtPackageSource.Text;
                SelectedPackage.Version = txtPackageVersion.Text;
                SelectedPackage.EnabledByDefault = cbxPackageDefault.Checked;
                SelectedPackage.Dependencies.Clear();
                foreach(ListViewItem k in lvwPackageDependencies.Items) {
                    SelectedPackage.Dependencies.Add(k.Text);
                }
            }

            SelectedPackage = null;

            RefreshDisplay();
        }

        private void btnOpenPackage_Click(object sender, EventArgs e) {
            if (lvwPackages.SelectedItems.Count > 0) {
                SelectedPackage = ((Package)lvwPackages.SelectedItems[0].Tag);
                RefreshDisplay();
            }
        }

        private void btnFetchFiles_Click(object sender, EventArgs e) {
            new FetchWindow().Show();
        }

        private void btnPackageAddDependency_Click(object sender, EventArgs e) {
            if (SelectedPackage != null) {
                SelectedPackage.Dependencies.Add(txtPackageDependency.Text);
                RefreshDisplay();
            }
        }
    }
}
