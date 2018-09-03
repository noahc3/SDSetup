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
    public partial class FormViewPlatforms : Form {
        public FormViewPlatforms() {
            InitializeComponent();
            RefreshListing();
        }

        private void RefreshListing() {
            lvwPlatforms.Clear();
            foreach(Platform p in G.manifest.Platforms.Values) {
                ListViewItem i = new ListViewItem(p.Name);
                i.Tag = p;
                lvwPlatforms.Items.Add(i);
            }
        }

        private void btnNew_Click(object sender, EventArgs e) {
            new FormEditPlatform().ShowDialog();
            RefreshListing();
        }

        private void btnEdit_Click(object sender, EventArgs e) {
            if (lvwPlatforms.SelectedItems.Count < 1) return;
            new FormEditPlatform((Platform)lvwPlatforms.SelectedItems[0].Tag).ShowDialog();
            RefreshListing();
        }

        private void btnDelete_Click(object sender, EventArgs e) {
            if (lvwPlatforms.SelectedItems.Count < 1) return;
            G.manifest.Platforms.Remove(((Platform)lvwPlatforms.SelectedItems[0].Tag).ID);
            RefreshListing();
        }

        private void btnClose_Click(object sender, EventArgs e) {
            Close();
        }
    }
}
