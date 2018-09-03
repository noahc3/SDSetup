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
    public partial class FormEditPlatform : Form {

        Platform p;

        public FormEditPlatform() {
            InitializeComponent();
        }

        public FormEditPlatform(Platform p) {
            InitializeComponent();
            this.p = p;

            txtId.Text = p.ID;
            txtName.Text = p.Name;
            txtMenu.Text = p.MenuName;
            txtClasses.Text = p.Color;
            cbxVisible.Checked = p.Visible;
            txtImg.Text = p.HomeIcon;

            picIcon.ImageLocation = txtImg.Text;
        }

        private void btnTestImg_Click(object sender, EventArgs e) {
            picIcon.ImageLocation = txtImg.Text;
        }

        private void btnSave_Click(object sender, EventArgs e) {
            if (p == null) {
                if (G.manifest.Platforms.ContainsKey(txtId.Text)) {
                    MessageBox.Show("A platform with that ID already exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                G.manifest.Platforms.Add(txtId.Text, new Platform(txtName.Text, txtMenu.Text, txtImg.Text, txtId.Text, new List<PackageSection>(), txtClasses.Text, cbxVisible.Checked));
            } else {

                if (txtId.Text != p.ID && G.manifest.Platforms.ContainsKey(txtId.Text)) {
                    MessageBox.Show("A platform with that ID already exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                p.ID = txtId.Text;
                p.Name = txtName.Text;
                p.MenuName = txtMenu.Text;
                p.Color = txtClasses.Text;
                p.Visible = cbxVisible.Checked;
                p.HomeIcon = txtImg.Text;
            }
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            Close();
        }
    }
}
