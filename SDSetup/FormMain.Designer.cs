namespace SDSetupManifestGenerator {
    partial class FormMain {
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
            this.btnWrite = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtDlUrl = new System.Windows.Forms.TextBox();
            this.lblDlUrl = new System.Windows.Forms.Label();
            this.btnDeletePackage = new System.Windows.Forms.Button();
            this.btnNewPackage = new System.Windows.Forms.Button();
            this.btnOpenPackage = new System.Windows.Forms.Button();
            this.lbxPackages = new System.Windows.Forms.ListBox();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.lblSource = new System.Windows.Forms.Label();
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.lblVersion = new System.Windows.Forms.Label();
            this.btnFetch = new System.Windows.Forms.Button();
            this.btnDeletePath = new System.Windows.Forms.Button();
            this.btnEditPath = new System.Windows.Forms.Button();
            this.btnNewPath = new System.Windows.Forms.Button();
            this.tvwItems = new System.Windows.Forms.TreeView();
            this.lblURL = new System.Windows.Forms.Label();
            this.txtURL = new System.Windows.Forms.TextBox();
            this.txtAuthors = new System.Windows.Forms.TextBox();
            this.lblAuthors = new System.Windows.Forms.Label();
            this.lblProgress = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.cbxEnabled = new System.Windows.Forms.CheckBox();
            this.lblGit = new System.Windows.Forms.Label();
            this.txtSubcat = new System.Windows.Forms.TextBox();
            this.lblSubCategory = new System.Windows.Forms.Label();
            this.txtCat = new System.Windows.Forms.TextBox();
            this.lblCategory = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.txtId = new System.Windows.Forms.TextBox();
            this.lblID = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnWrite
            // 
            this.btnWrite.Enabled = false;
            this.btnWrite.Location = new System.Drawing.Point(12, 384);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(275, 23);
            this.btnWrite.TabIndex = 75;
            this.btnWrite.Text = "Write Manifest";
            this.btnWrite.UseVisualStyleBackColor = true;
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(566, 384);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(223, 23);
            this.btnSave.TabIndex = 74;
            this.btnSave.Text = "Save Package";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtDlUrl
            // 
            this.txtDlUrl.Location = new System.Drawing.Point(566, 329);
            this.txtDlUrl.Name = "txtDlUrl";
            this.txtDlUrl.Size = new System.Drawing.Size(223, 20);
            this.txtDlUrl.TabIndex = 72;
            // 
            // lblDlUrl
            // 
            this.lblDlUrl.AutoSize = true;
            this.lblDlUrl.Location = new System.Drawing.Point(563, 313);
            this.lblDlUrl.Name = "lblDlUrl";
            this.lblDlUrl.Size = new System.Drawing.Size(80, 13);
            this.lblDlUrl.TabIndex = 73;
            this.lblDlUrl.Text = "Download URL";
            // 
            // btnDeletePackage
            // 
            this.btnDeletePackage.Enabled = false;
            this.btnDeletePackage.Location = new System.Drawing.Point(476, 384);
            this.btnDeletePackage.Name = "btnDeletePackage";
            this.btnDeletePackage.Size = new System.Drawing.Size(85, 23);
            this.btnDeletePackage.TabIndex = 71;
            this.btnDeletePackage.Text = "Delete";
            this.btnDeletePackage.UseVisualStyleBackColor = true;
            this.btnDeletePackage.Click += new System.EventHandler(this.btnDeletePackage_Click);
            // 
            // btnNewPackage
            // 
            this.btnNewPackage.Enabled = false;
            this.btnNewPackage.Location = new System.Drawing.Point(385, 384);
            this.btnNewPackage.Name = "btnNewPackage";
            this.btnNewPackage.Size = new System.Drawing.Size(85, 23);
            this.btnNewPackage.TabIndex = 70;
            this.btnNewPackage.Text = "New";
            this.btnNewPackage.UseVisualStyleBackColor = true;
            this.btnNewPackage.Click += new System.EventHandler(this.btnNewPackage_Click);
            // 
            // btnOpenPackage
            // 
            this.btnOpenPackage.Enabled = false;
            this.btnOpenPackage.Location = new System.Drawing.Point(294, 384);
            this.btnOpenPackage.Name = "btnOpenPackage";
            this.btnOpenPackage.Size = new System.Drawing.Size(85, 23);
            this.btnOpenPackage.TabIndex = 69;
            this.btnOpenPackage.Text = "Open";
            this.btnOpenPackage.UseVisualStyleBackColor = true;
            this.btnOpenPackage.Click += new System.EventHandler(this.btnOpenPackage_Click);
            // 
            // lbxPackages
            // 
            this.lbxPackages.Enabled = false;
            this.lbxPackages.FormattingEnabled = true;
            this.lbxPackages.IntegralHeight = false;
            this.lbxPackages.Location = new System.Drawing.Point(293, 12);
            this.lbxPackages.Name = "lbxPackages";
            this.lbxPackages.Size = new System.Drawing.Size(267, 366);
            this.lbxPackages.TabIndex = 68;
            // 
            // txtSource
            // 
            this.txtSource.Location = new System.Drawing.Point(566, 264);
            this.txtSource.Name = "txtSource";
            this.txtSource.Size = new System.Drawing.Size(223, 20);
            this.txtSource.TabIndex = 66;
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.Location = new System.Drawing.Point(563, 248);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(41, 13);
            this.lblSource.TabIndex = 67;
            this.lblSource.Text = "Source";
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(566, 225);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(223, 20);
            this.txtVersion.TabIndex = 64;
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(563, 209);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(42, 13);
            this.lblVersion.TabIndex = 65;
            this.lblVersion.Text = "Version";
            // 
            // btnFetch
            // 
            this.btnFetch.Location = new System.Drawing.Point(566, 355);
            this.btnFetch.Name = "btnFetch";
            this.btnFetch.Size = new System.Drawing.Size(223, 23);
            this.btnFetch.TabIndex = 63;
            this.btnFetch.Text = "Fetch Latest";
            this.btnFetch.UseVisualStyleBackColor = true;
            this.btnFetch.Click += new System.EventHandler(this.btnFetch_Click);
            // 
            // btnDeletePath
            // 
            this.btnDeletePath.Location = new System.Drawing.Point(1057, 758);
            this.btnDeletePath.Name = "btnDeletePath";
            this.btnDeletePath.Size = new System.Drawing.Size(125, 23);
            this.btnDeletePath.TabIndex = 55;
            this.btnDeletePath.Text = "Delete Path";
            this.btnDeletePath.UseVisualStyleBackColor = true;
            this.btnDeletePath.Click += new System.EventHandler(this.btnDeletePath_Click);
            // 
            // btnEditPath
            // 
            this.btnEditPath.Location = new System.Drawing.Point(926, 758);
            this.btnEditPath.Name = "btnEditPath";
            this.btnEditPath.Size = new System.Drawing.Size(125, 23);
            this.btnEditPath.TabIndex = 54;
            this.btnEditPath.Text = "Edit Path";
            this.btnEditPath.UseVisualStyleBackColor = true;
            this.btnEditPath.Click += new System.EventHandler(this.btnEditPath_Click);
            // 
            // btnNewPath
            // 
            this.btnNewPath.Location = new System.Drawing.Point(795, 758);
            this.btnNewPath.Name = "btnNewPath";
            this.btnNewPath.Size = new System.Drawing.Size(125, 23);
            this.btnNewPath.TabIndex = 52;
            this.btnNewPath.Text = "New Path";
            this.btnNewPath.UseVisualStyleBackColor = true;
            this.btnNewPath.Click += new System.EventHandler(this.btnNewPath_Click);
            // 
            // tvwItems
            // 
            this.tvwItems.AllowDrop = true;
            this.tvwItems.FullRowSelect = true;
            this.tvwItems.LabelEdit = true;
            this.tvwItems.Location = new System.Drawing.Point(795, 12);
            this.tvwItems.Name = "tvwItems";
            this.tvwItems.PathSeparator = "/";
            this.tvwItems.Size = new System.Drawing.Size(387, 740);
            this.tvwItems.TabIndex = 51;
            this.tvwItems.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tvwItems_ItemDrag);
            this.tvwItems.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvwItems_DragDrop);
            this.tvwItems.DragOver += new System.Windows.Forms.DragEventHandler(this.tvwItems_DragOver);
            this.tvwItems.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tvwItems_MouseUp);
            // 
            // lblURL
            // 
            this.lblURL.AutoSize = true;
            this.lblURL.Location = new System.Drawing.Point(9, 162);
            this.lblURL.Name = "lblURL";
            this.lblURL.Size = new System.Drawing.Size(58, 13);
            this.lblURL.TabIndex = 62;
            this.lblURL.Text = "URL Prefix";
            // 
            // txtURL
            // 
            this.txtURL.Location = new System.Drawing.Point(12, 178);
            this.txtURL.Name = "txtURL";
            this.txtURL.Size = new System.Drawing.Size(275, 20);
            this.txtURL.TabIndex = 61;
            this.txtURL.TabStop = false;
            // 
            // txtAuthors
            // 
            this.txtAuthors.Location = new System.Drawing.Point(566, 186);
            this.txtAuthors.Name = "txtAuthors";
            this.txtAuthors.Size = new System.Drawing.Size(223, 20);
            this.txtAuthors.TabIndex = 50;
            // 
            // lblAuthors
            // 
            this.lblAuthors.AutoSize = true;
            this.lblAuthors.Location = new System.Drawing.Point(563, 170);
            this.lblAuthors.Name = "lblAuthors";
            this.lblAuthors.Size = new System.Drawing.Size(43, 13);
            this.lblAuthors.TabIndex = 60;
            this.lblAuthors.Text = "Authors";
            // 
            // lblProgress
            // 
            this.lblProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProgress.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblProgress.Location = new System.Drawing.Point(12, 784);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(1170, 23);
            this.lblProgress.TabIndex = 59;
            this.lblProgress.Text = "Manifest Authoring Tool v3.1";
            this.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.Location = new System.Drawing.Point(12, 413);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.Size = new System.Drawing.Size(777, 368);
            this.txtLog.TabIndex = 58;
            this.txtLog.TabStop = false;
            // 
            // cbxEnabled
            // 
            this.cbxEnabled.AutoSize = true;
            this.cbxEnabled.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbxEnabled.Location = new System.Drawing.Point(566, 290);
            this.cbxEnabled.Name = "cbxEnabled";
            this.cbxEnabled.Size = new System.Drawing.Size(116, 17);
            this.cbxEnabled.TabIndex = 48;
            this.cbxEnabled.Text = "Enabled by Default";
            this.cbxEnabled.UseVisualStyleBackColor = true;
            // 
            // lblGit
            // 
            this.lblGit.AutoSize = true;
            this.lblGit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGit.ForeColor = System.Drawing.Color.Red;
            this.lblGit.Location = new System.Drawing.Point(9, 131);
            this.lblGit.Name = "lblGit";
            this.lblGit.Size = new System.Drawing.Size(129, 13);
            this.lblGit.TabIndex = 57;
            this.lblGit.Text = "Status: Not Authenticated";
            // 
            // txtSubcat
            // 
            this.txtSubcat.Location = new System.Drawing.Point(566, 147);
            this.txtSubcat.Name = "txtSubcat";
            this.txtSubcat.Size = new System.Drawing.Size(223, 20);
            this.txtSubcat.TabIndex = 47;
            // 
            // lblSubCategory
            // 
            this.lblSubCategory.AutoSize = true;
            this.lblSubCategory.Location = new System.Drawing.Point(563, 131);
            this.lblSubCategory.Name = "lblSubCategory";
            this.lblSubCategory.Size = new System.Drawing.Size(70, 13);
            this.lblSubCategory.TabIndex = 56;
            this.lblSubCategory.Text = "Sub-category";
            // 
            // txtCat
            // 
            this.txtCat.Location = new System.Drawing.Point(566, 108);
            this.txtCat.Name = "txtCat";
            this.txtCat.Size = new System.Drawing.Size(223, 20);
            this.txtCat.TabIndex = 45;
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(563, 92);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(49, 13);
            this.lblCategory.TabIndex = 53;
            this.lblCategory.Text = "Category";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(566, 69);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(223, 20);
            this.txtName.TabIndex = 43;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(563, 53);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(81, 13);
            this.lblName.TabIndex = 49;
            this.lblName.Text = "Package Name";
            // 
            // txtId
            // 
            this.txtId.Location = new System.Drawing.Point(566, 28);
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(223, 20);
            this.txtId.TabIndex = 41;
            // 
            // lblID
            // 
            this.lblID.AutoSize = true;
            this.lblID.Location = new System.Drawing.Point(563, 12);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(64, 13);
            this.lblID.TabIndex = 46;
            this.lblID.Text = "Package ID";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(9, 53);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 44;
            this.lblPassword.Text = "Password";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(9, 12);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(55, 13);
            this.lblUsername.TabIndex = 42;
            this.lblUsername.Text = "Username";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(12, 69);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(275, 20);
            this.txtPassword.TabIndex = 38;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(12, 28);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(275, 20);
            this.txtUsername.TabIndex = 37;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(12, 95);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(275, 23);
            this.btnLogin.TabIndex = 39;
            this.btnLogin.Text = "Login to GitHub";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1194, 812);
            this.Controls.Add(this.btnWrite);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtDlUrl);
            this.Controls.Add(this.lblDlUrl);
            this.Controls.Add(this.btnDeletePackage);
            this.Controls.Add(this.btnNewPackage);
            this.Controls.Add(this.btnOpenPackage);
            this.Controls.Add(this.lbxPackages);
            this.Controls.Add(this.txtSource);
            this.Controls.Add(this.lblSource);
            this.Controls.Add(this.txtVersion);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.btnFetch);
            this.Controls.Add(this.btnDeletePath);
            this.Controls.Add(this.btnEditPath);
            this.Controls.Add(this.btnNewPath);
            this.Controls.Add(this.tvwItems);
            this.Controls.Add(this.lblURL);
            this.Controls.Add(this.txtURL);
            this.Controls.Add(this.txtAuthors);
            this.Controls.Add(this.lblAuthors);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.cbxEnabled);
            this.Controls.Add(this.lblGit);
            this.Controls.Add(this.txtSubcat);
            this.Controls.Add(this.lblSubCategory);
            this.Controls.Add(this.txtCat);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtId);
            this.Controls.Add(this.lblID);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.btnLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.Text = "Manifest Authoring Tool";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnWrite;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtDlUrl;
        private System.Windows.Forms.Label lblDlUrl;
        private System.Windows.Forms.Button btnDeletePackage;
        private System.Windows.Forms.Button btnNewPackage;
        private System.Windows.Forms.Button btnOpenPackage;
        private System.Windows.Forms.ListBox lbxPackages;
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.TextBox txtVersion;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Button btnFetch;
        private System.Windows.Forms.Button btnDeletePath;
        private System.Windows.Forms.Button btnEditPath;
        private System.Windows.Forms.Button btnNewPath;
        private System.Windows.Forms.TreeView tvwItems;
        private System.Windows.Forms.Label lblURL;
        private System.Windows.Forms.TextBox txtURL;
        private System.Windows.Forms.TextBox txtAuthors;
        private System.Windows.Forms.Label lblAuthors;
        private System.Windows.Forms.Label lblProgress;
        public System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.CheckBox cbxEnabled;
        private System.Windows.Forms.Label lblGit;
        private System.Windows.Forms.TextBox txtSubcat;
        private System.Windows.Forms.Label lblSubCategory;
        private System.Windows.Forms.TextBox txtCat;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Button btnLogin;
    }
}