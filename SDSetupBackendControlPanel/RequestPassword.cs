using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SDSetupBackendControlPanel {
    public partial class RequestPassword : Form {

        public string password;

        public RequestPassword(int mode, string hostname, string username) {
            InitializeComponent();

            switch (mode) {
                case 1:
                    lblInfo.Text = $"Please enter your password\nHostname: {hostname}\nUser: {username}";
                    break;
                case 2:
                    lblInfo.Text = $"Please enter your private key passphrase\nHostname: {hostname}\nUser: {username}";
                    break;
            }


        }

        private void btnContinue_Click(object sender, EventArgs e) {
            password = txtInput.Text;
        }
    }
}
