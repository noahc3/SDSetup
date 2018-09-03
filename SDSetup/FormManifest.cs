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
    public partial class FormManifest : Form {
        public FormManifest() {
            InitializeComponent();

            txtManifestVersion.Text = G.manifest.Version;
            txtCopyright.Text = G.manifest.Copyright;
            txtMessage.Text = G.manifest.Message.InnerHTML;
            txtClasses.Text = G.manifest.Message.Color;

        }

        private void btnSave_Click(object sender, EventArgs e) {
            G.manifest.Version = txtManifestVersion.Text;
            G.manifest.Copyright = txtCopyright.Text;
            G.manifest.Message.InnerHTML = txtMessage.Text;
            G.manifest.Message.Color = txtClasses.Text;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            Close();
        }
    }
}
