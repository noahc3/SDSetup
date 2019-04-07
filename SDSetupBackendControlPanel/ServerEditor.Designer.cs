namespace SDSetupBackendControlPanel {
    partial class ServerEditor {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.txtHostname = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblHostname = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPrivateKey = new System.Windows.Forms.Label();
            this.txtKeyPath = new System.Windows.Forms.TextBox();
            this.cbxKeyBasedAuth = new System.Windows.Forms.CheckBox();
            this.btnBrowsePrivateKey = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblUUID = new System.Windows.Forms.Label();
            this.cbxAskPassword = new System.Windows.Forms.CheckBox();
            this.panelPrivateKey = new System.Windows.Forms.Panel();
            this.cbxAskPassphrase = new System.Windows.Forms.CheckBox();
            this.lblPassphrase = new System.Windows.Forms.Label();
            this.txtPassphrase = new System.Windows.Forms.TextBox();
            this.lblBackendDir = new System.Windows.Forms.Label();
            this.txtBackendDir = new System.Windows.Forms.TextBox();
            this.lblGuideDir = new System.Windows.Forms.Label();
            this.txtGuideDir = new System.Windows.Forms.TextBox();
            this.lblPubTestingGuideDir = new System.Windows.Forms.Label();
            this.txtPubTestingDir = new System.Windows.Forms.TextBox();
            this.lblPrivTestingGuideDir = new System.Windows.Forms.Label();
            this.txtPrivTestingDir = new System.Windows.Forms.TextBox();
            this.lblBackendTestingDir = new System.Windows.Forms.Label();
            this.txtBackendTestingDir = new System.Windows.Forms.TextBox();
            this.panelPrivateKey.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtHostname
            // 
            this.txtHostname.Location = new System.Drawing.Point(12, 25);
            this.txtHostname.Name = "txtHostname";
            this.txtHostname.Size = new System.Drawing.Size(353, 20);
            this.txtHostname.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(290, 584);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblHostname
            // 
            this.lblHostname.AutoSize = true;
            this.lblHostname.Location = new System.Drawing.Point(9, 9);
            this.lblHostname.Name = "lblHostname";
            this.lblHostname.Size = new System.Drawing.Size(55, 13);
            this.lblHostname.TabIndex = 2;
            this.lblHostname.Text = "Hostname";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(9, 48);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(55, 13);
            this.lblUsername.TabIndex = 4;
            this.lblUsername.Text = "Username";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(12, 64);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(353, 20);
            this.txtUsername.TabIndex = 1;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(9, 87);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 6;
            this.lblPassword.Text = "Password";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(12, 103);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(353, 20);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // lblPrivateKey
            // 
            this.lblPrivateKey.AutoSize = true;
            this.lblPrivateKey.Location = new System.Drawing.Point(0, 8);
            this.lblPrivateKey.Name = "lblPrivateKey";
            this.lblPrivateKey.Size = new System.Drawing.Size(80, 13);
            this.lblPrivateKey.TabIndex = 8;
            this.lblPrivateKey.Text = "Private Key File";
            // 
            // txtKeyPath
            // 
            this.txtKeyPath.Location = new System.Drawing.Point(3, 24);
            this.txtKeyPath.Name = "txtKeyPath";
            this.txtKeyPath.Size = new System.Drawing.Size(347, 20);
            this.txtKeyPath.TabIndex = 1;
            // 
            // cbxKeyBasedAuth
            // 
            this.cbxKeyBasedAuth.AutoSize = true;
            this.cbxKeyBasedAuth.Location = new System.Drawing.Point(12, 162);
            this.cbxKeyBasedAuth.Name = "cbxKeyBasedAuth";
            this.cbxKeyBasedAuth.Size = new System.Drawing.Size(175, 17);
            this.cbxKeyBasedAuth.TabIndex = 4;
            this.cbxKeyBasedAuth.Text = "Uses Key Based Authentication";
            this.cbxKeyBasedAuth.UseVisualStyleBackColor = true;
            // 
            // btnBrowsePrivateKey
            // 
            this.btnBrowsePrivateKey.Location = new System.Drawing.Point(275, 50);
            this.btnBrowsePrivateKey.Name = "btnBrowsePrivateKey";
            this.btnBrowsePrivateKey.Size = new System.Drawing.Size(75, 23);
            this.btnBrowsePrivateKey.TabIndex = 2;
            this.btnBrowsePrivateKey.Text = "Browse";
            this.btnBrowsePrivateKey.UseVisualStyleBackColor = true;
            this.btnBrowsePrivateKey.Click += new System.EventHandler(this.btnBrowsePrivateKey_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(209, 584);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblUUID
            // 
            this.lblUUID.AutoSize = true;
            this.lblUUID.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUUID.Location = new System.Drawing.Point(7, 594);
            this.lblUUID.Name = "lblUUID";
            this.lblUUID.Size = new System.Drawing.Size(37, 13);
            this.lblUUID.TabIndex = 12;
            this.lblUUID.Text = "UUID:";
            // 
            // cbxAskPassword
            // 
            this.cbxAskPassword.AutoSize = true;
            this.cbxAskPassword.Location = new System.Drawing.Point(12, 129);
            this.cbxAskPassword.Name = "cbxAskPassword";
            this.cbxAskPassword.Size = new System.Drawing.Size(162, 17);
            this.cbxAskPassword.TabIndex = 3;
            this.cbxAskPassword.Text = "Ask For Password Each Run";
            this.cbxAskPassword.UseVisualStyleBackColor = true;
            // 
            // panelPrivateKey
            // 
            this.panelPrivateKey.Controls.Add(this.cbxAskPassphrase);
            this.panelPrivateKey.Controls.Add(this.lblPrivateKey);
            this.panelPrivateKey.Controls.Add(this.lblPassphrase);
            this.panelPrivateKey.Controls.Add(this.txtKeyPath);
            this.panelPrivateKey.Controls.Add(this.txtPassphrase);
            this.panelPrivateKey.Controls.Add(this.btnBrowsePrivateKey);
            this.panelPrivateKey.Location = new System.Drawing.Point(12, 185);
            this.panelPrivateKey.Name = "panelPrivateKey";
            this.panelPrivateKey.Size = new System.Drawing.Size(353, 155);
            this.panelPrivateKey.TabIndex = 5;
            // 
            // cbxAskPassphrase
            // 
            this.cbxAskPassphrase.AutoSize = true;
            this.cbxAskPassphrase.Location = new System.Drawing.Point(3, 121);
            this.cbxAskPassphrase.Name = "cbxAskPassphrase";
            this.cbxAskPassphrase.Size = new System.Drawing.Size(171, 17);
            this.cbxAskPassphrase.TabIndex = 4;
            this.cbxAskPassphrase.Text = "Ask For Passphrase Each Run";
            this.cbxAskPassphrase.UseVisualStyleBackColor = true;
            // 
            // lblPassphrase
            // 
            this.lblPassphrase.AutoSize = true;
            this.lblPassphrase.Location = new System.Drawing.Point(0, 79);
            this.lblPassphrase.Name = "lblPassphrase";
            this.lblPassphrase.Size = new System.Drawing.Size(119, 13);
            this.lblPassphrase.TabIndex = 16;
            this.lblPassphrase.Text = "Private Key Passphrase";
            // 
            // txtPassphrase
            // 
            this.txtPassphrase.Location = new System.Drawing.Point(3, 95);
            this.txtPassphrase.Name = "txtPassphrase";
            this.txtPassphrase.Size = new System.Drawing.Size(347, 20);
            this.txtPassphrase.TabIndex = 3;
            this.txtPassphrase.UseSystemPasswordChar = true;
            // 
            // lblBackendDir
            // 
            this.lblBackendDir.AutoSize = true;
            this.lblBackendDir.Location = new System.Drawing.Point(9, 368);
            this.lblBackendDir.Name = "lblBackendDir";
            this.lblBackendDir.Size = new System.Drawing.Size(141, 13);
            this.lblBackendDir.TabIndex = 14;
            this.lblBackendDir.Text = "SDSetup Backend Directory";
            // 
            // txtBackendDir
            // 
            this.txtBackendDir.Location = new System.Drawing.Point(12, 384);
            this.txtBackendDir.Name = "txtBackendDir";
            this.txtBackendDir.Size = new System.Drawing.Size(353, 20);
            this.txtBackendDir.TabIndex = 13;
            // 
            // lblGuideDir
            // 
            this.lblGuideDir.AutoSize = true;
            this.lblGuideDir.Location = new System.Drawing.Point(9, 446);
            this.lblGuideDir.Name = "lblGuideDir";
            this.lblGuideDir.Size = new System.Drawing.Size(80, 13);
            this.lblGuideDir.TabIndex = 16;
            this.lblGuideDir.Text = "Guide Directory";
            // 
            // txtGuideDir
            // 
            this.txtGuideDir.Location = new System.Drawing.Point(12, 462);
            this.txtGuideDir.Name = "txtGuideDir";
            this.txtGuideDir.Size = new System.Drawing.Size(353, 20);
            this.txtGuideDir.TabIndex = 15;
            // 
            // lblPubTestingGuideDir
            // 
            this.lblPubTestingGuideDir.AutoSize = true;
            this.lblPubTestingGuideDir.Location = new System.Drawing.Point(9, 485);
            this.lblPubTestingGuideDir.Name = "lblPubTestingGuideDir";
            this.lblPubTestingGuideDir.Size = new System.Drawing.Size(150, 13);
            this.lblPubTestingGuideDir.TabIndex = 18;
            this.lblPubTestingGuideDir.Text = "Public Testing Guide Directory";
            // 
            // txtPubTestingDir
            // 
            this.txtPubTestingDir.Location = new System.Drawing.Point(12, 501);
            this.txtPubTestingDir.Name = "txtPubTestingDir";
            this.txtPubTestingDir.Size = new System.Drawing.Size(353, 20);
            this.txtPubTestingDir.TabIndex = 17;
            // 
            // lblPrivTestingGuideDir
            // 
            this.lblPrivTestingGuideDir.AutoSize = true;
            this.lblPrivTestingGuideDir.Location = new System.Drawing.Point(9, 524);
            this.lblPrivTestingGuideDir.Name = "lblPrivTestingGuideDir";
            this.lblPrivTestingGuideDir.Size = new System.Drawing.Size(154, 13);
            this.lblPrivTestingGuideDir.TabIndex = 20;
            this.lblPrivTestingGuideDir.Text = "Private Testing Guide Directory";
            // 
            // txtPrivTestingDir
            // 
            this.txtPrivTestingDir.Location = new System.Drawing.Point(12, 540);
            this.txtPrivTestingDir.Name = "txtPrivTestingDir";
            this.txtPrivTestingDir.Size = new System.Drawing.Size(353, 20);
            this.txtPrivTestingDir.TabIndex = 19;
            // 
            // lblBackendTestingDir
            // 
            this.lblBackendTestingDir.AutoSize = true;
            this.lblBackendTestingDir.Location = new System.Drawing.Point(9, 407);
            this.lblBackendTestingDir.Name = "lblBackendTestingDir";
            this.lblBackendTestingDir.Size = new System.Drawing.Size(179, 13);
            this.lblBackendTestingDir.TabIndex = 22;
            this.lblBackendTestingDir.Text = "SDSetup Backend Testing Directory";
            // 
            // txtBackendTestingDir
            // 
            this.txtBackendTestingDir.Location = new System.Drawing.Point(12, 423);
            this.txtBackendTestingDir.Name = "txtBackendTestingDir";
            this.txtBackendTestingDir.Size = new System.Drawing.Size(353, 20);
            this.txtBackendTestingDir.TabIndex = 21;
            // 
            // ServerEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(377, 619);
            this.Controls.Add(this.lblBackendTestingDir);
            this.Controls.Add(this.txtBackendTestingDir);
            this.Controls.Add(this.lblPrivTestingGuideDir);
            this.Controls.Add(this.txtPrivTestingDir);
            this.Controls.Add(this.lblPubTestingGuideDir);
            this.Controls.Add(this.txtPubTestingDir);
            this.Controls.Add(this.lblGuideDir);
            this.Controls.Add(this.txtGuideDir);
            this.Controls.Add(this.lblBackendDir);
            this.Controls.Add(this.txtBackendDir);
            this.Controls.Add(this.panelPrivateKey);
            this.Controls.Add(this.cbxAskPassword);
            this.Controls.Add(this.lblUUID);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.cbxKeyBasedAuth);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblHostname);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtHostname);
            this.Name = "ServerEditor";
            this.Text = "Server Editor";
            this.panelPrivateKey.ResumeLayout(false);
            this.panelPrivateKey.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtHostname;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblHostname;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPrivateKey;
        private System.Windows.Forms.TextBox txtKeyPath;
        private System.Windows.Forms.CheckBox cbxKeyBasedAuth;
        private System.Windows.Forms.Button btnBrowsePrivateKey;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblUUID;
        private System.Windows.Forms.CheckBox cbxAskPassword;
        private System.Windows.Forms.Panel panelPrivateKey;
        private System.Windows.Forms.CheckBox cbxAskPassphrase;
        private System.Windows.Forms.Label lblPassphrase;
        private System.Windows.Forms.TextBox txtPassphrase;
        private System.Windows.Forms.Label lblBackendDir;
        private System.Windows.Forms.TextBox txtBackendDir;
        private System.Windows.Forms.Label lblGuideDir;
        private System.Windows.Forms.TextBox txtGuideDir;
        private System.Windows.Forms.Label lblPubTestingGuideDir;
        private System.Windows.Forms.TextBox txtPubTestingDir;
        private System.Windows.Forms.Label lblPrivTestingGuideDir;
        private System.Windows.Forms.TextBox txtPrivTestingDir;
        private System.Windows.Forms.Label lblBackendTestingDir;
        private System.Windows.Forms.TextBox txtBackendTestingDir;
    }
}