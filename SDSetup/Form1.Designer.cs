namespace SDSetupManifestGenerator {
    partial class Form1 {
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
            this.btnLogin = new System.Windows.Forms.Button();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblID = new System.Windows.Forms.Label();
            this.txtId = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.txtCat = new System.Windows.Forms.TextBox();
            this.lblCategory = new System.Windows.Forms.Label();
            this.txtSubcat = new System.Windows.Forms.TextBox();
            this.lblSubCategory = new System.Windows.Forms.Label();
            this.lblGit = new System.Windows.Forms.Label();
            this.cbxEnabled = new System.Windows.Forms.CheckBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.lblProgress = new System.Windows.Forms.Label();
            this.txtAuthors = new System.Windows.Forms.TextBox();
            this.lblAuthors = new System.Windows.Forms.Label();
            this.lblURL = new System.Windows.Forms.Label();
            this.txtURL = new System.Windows.Forms.TextBox();
            this.tvwItems = new System.Windows.Forms.TreeView();
            this.btnNewPath = new System.Windows.Forms.Button();
            this.btnEditPath = new System.Windows.Forms.Button();
            this.btnDeletePath = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(12, 95);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(548, 23);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "Login to GitHub";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(12, 28);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(548, 20);
            this.txtUsername.TabIndex = 0;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(12, 69);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(548, 20);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(9, 12);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(55, 13);
            this.lblUsername.TabIndex = 5;
            this.lblUsername.Text = "Username";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(9, 53);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 6;
            this.lblPassword.Text = "Password";
            // 
            // lblID
            // 
            this.lblID.AutoSize = true;
            this.lblID.Location = new System.Drawing.Point(563, 12);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(64, 13);
            this.lblID.TabIndex = 7;
            this.lblID.Text = "Package ID";
            // 
            // txtId
            // 
            this.txtId.Enabled = false;
            this.txtId.Location = new System.Drawing.Point(566, 28);
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(223, 20);
            this.txtId.TabIndex = 4;
            // 
            // txtName
            // 
            this.txtName.Enabled = false;
            this.txtName.Location = new System.Drawing.Point(566, 69);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(223, 20);
            this.txtName.TabIndex = 5;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(563, 53);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(81, 13);
            this.lblName.TabIndex = 9;
            this.lblName.Text = "Package Name";
            // 
            // txtCat
            // 
            this.txtCat.Enabled = false;
            this.txtCat.Location = new System.Drawing.Point(566, 108);
            this.txtCat.Name = "txtCat";
            this.txtCat.Size = new System.Drawing.Size(223, 20);
            this.txtCat.TabIndex = 6;
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(563, 92);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(49, 13);
            this.lblCategory.TabIndex = 11;
            this.lblCategory.Text = "Category";
            // 
            // txtSubcat
            // 
            this.txtSubcat.Enabled = false;
            this.txtSubcat.Location = new System.Drawing.Point(566, 147);
            this.txtSubcat.Name = "txtSubcat";
            this.txtSubcat.Size = new System.Drawing.Size(223, 20);
            this.txtSubcat.TabIndex = 7;
            // 
            // lblSubCategory
            // 
            this.lblSubCategory.AutoSize = true;
            this.lblSubCategory.Location = new System.Drawing.Point(563, 131);
            this.lblSubCategory.Name = "lblSubCategory";
            this.lblSubCategory.Size = new System.Drawing.Size(70, 13);
            this.lblSubCategory.TabIndex = 13;
            this.lblSubCategory.Text = "Sub-category";
            // 
            // lblGit
            // 
            this.lblGit.AutoSize = true;
            this.lblGit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGit.ForeColor = System.Drawing.Color.Red;
            this.lblGit.Location = new System.Drawing.Point(9, 131);
            this.lblGit.Name = "lblGit";
            this.lblGit.Size = new System.Drawing.Size(129, 13);
            this.lblGit.TabIndex = 15;
            this.lblGit.Text = "Status: Not Authenticated";
            // 
            // cbxEnabled
            // 
            this.cbxEnabled.AutoSize = true;
            this.cbxEnabled.Enabled = false;
            this.cbxEnabled.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbxEnabled.Location = new System.Drawing.Point(566, 212);
            this.cbxEnabled.Name = "cbxEnabled";
            this.cbxEnabled.Size = new System.Drawing.Size(116, 17);
            this.cbxEnabled.TabIndex = 8;
            this.cbxEnabled.Text = "Enabled by Default";
            this.cbxEnabled.UseVisualStyleBackColor = true;
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.Location = new System.Drawing.Point(12, 233);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.Size = new System.Drawing.Size(548, 350);
            this.txtLog.TabIndex = 17;
            this.txtLog.TabStop = false;
            // 
            // btnStart
            // 
            this.btnStart.Enabled = false;
            this.btnStart.Location = new System.Drawing.Point(12, 204);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(545, 23);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnNext
            // 
            this.btnNext.Enabled = false;
            this.btnNext.Location = new System.Drawing.Point(566, 257);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(223, 23);
            this.btnNext.TabIndex = 14;
            this.btnNext.Text = "Next Package";
            this.btnNext.UseVisualStyleBackColor = true;
            // 
            // lblProgress
            // 
            this.lblProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProgress.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblProgress.Location = new System.Drawing.Point(12, 586);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(1170, 23);
            this.lblProgress.TabIndex = 19;
            this.lblProgress.Text = "Progress: Not yet started";
            this.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAuthors
            // 
            this.txtAuthors.Enabled = false;
            this.txtAuthors.Location = new System.Drawing.Point(566, 186);
            this.txtAuthors.Name = "txtAuthors";
            this.txtAuthors.Size = new System.Drawing.Size(223, 20);
            this.txtAuthors.TabIndex = 9;
            // 
            // lblAuthors
            // 
            this.lblAuthors.AutoSize = true;
            this.lblAuthors.Location = new System.Drawing.Point(563, 170);
            this.lblAuthors.Name = "lblAuthors";
            this.lblAuthors.Size = new System.Drawing.Size(43, 13);
            this.lblAuthors.TabIndex = 21;
            this.lblAuthors.Text = "Authors";
            // 
            // lblURL
            // 
            this.lblURL.AutoSize = true;
            this.lblURL.Location = new System.Drawing.Point(9, 162);
            this.lblURL.Name = "lblURL";
            this.lblURL.Size = new System.Drawing.Size(58, 13);
            this.lblURL.TabIndex = 23;
            this.lblURL.Text = "URL Prefix";
            // 
            // txtURL
            // 
            this.txtURL.Location = new System.Drawing.Point(12, 178);
            this.txtURL.Name = "txtURL";
            this.txtURL.Size = new System.Drawing.Size(545, 20);
            this.txtURL.TabIndex = 22;
            this.txtURL.TabStop = false;
            this.txtURL.UseSystemPasswordChar = true;
            // 
            // tvwItems
            // 
            this.tvwItems.AllowDrop = true;
            this.tvwItems.FullRowSelect = true;
            this.tvwItems.LabelEdit = true;
            this.tvwItems.Location = new System.Drawing.Point(795, 12);
            this.tvwItems.Name = "tvwItems";
            this.tvwItems.PathSeparator = "/";
            this.tvwItems.Size = new System.Drawing.Size(387, 542);
            this.tvwItems.TabIndex = 10;
            this.tvwItems.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tvwItems_ItemDrag);
            this.tvwItems.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvwItems_DragDrop);
            this.tvwItems.DragEnter += new System.Windows.Forms.DragEventHandler(this.tvwItems_DragEnter);
            this.tvwItems.DragOver += new System.Windows.Forms.DragEventHandler(this.tvwItems_DragOver);
            this.tvwItems.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tvwItems_Click);
            // 
            // btnNewPath
            // 
            this.btnNewPath.Enabled = false;
            this.btnNewPath.Location = new System.Drawing.Point(795, 560);
            this.btnNewPath.Name = "btnNewPath";
            this.btnNewPath.Size = new System.Drawing.Size(125, 23);
            this.btnNewPath.TabIndex = 11;
            this.btnNewPath.Text = "New Path";
            this.btnNewPath.UseVisualStyleBackColor = true;
            this.btnNewPath.Click += new System.EventHandler(this.btnNewPath_Click);
            // 
            // btnEditPath
            // 
            this.btnEditPath.Enabled = false;
            this.btnEditPath.Location = new System.Drawing.Point(926, 560);
            this.btnEditPath.Name = "btnEditPath";
            this.btnEditPath.Size = new System.Drawing.Size(125, 23);
            this.btnEditPath.TabIndex = 12;
            this.btnEditPath.Text = "Edit Path";
            this.btnEditPath.UseVisualStyleBackColor = true;
            this.btnEditPath.Click += new System.EventHandler(this.btnEditPath_Click);
            // 
            // btnDeletePath
            // 
            this.btnDeletePath.Enabled = false;
            this.btnDeletePath.Location = new System.Drawing.Point(1057, 560);
            this.btnDeletePath.Name = "btnDeletePath";
            this.btnDeletePath.Size = new System.Drawing.Size(125, 23);
            this.btnDeletePath.TabIndex = 13;
            this.btnDeletePath.Text = "Delete Path";
            this.btnDeletePath.UseVisualStyleBackColor = true;
            this.btnDeletePath.Click += new System.EventHandler(this.btnDeletePath_Click);
            // 
            // btnReset
            // 
            this.btnReset.Enabled = false;
            this.btnReset.Location = new System.Drawing.Point(566, 286);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(223, 23);
            this.btnReset.TabIndex = 24;
            this.btnReset.Text = "Reset This Package";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1194, 614);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnDeletePath);
            this.Controls.Add(this.btnEditPath);
            this.Controls.Add(this.btnNewPath);
            this.Controls.Add(this.tvwItems);
            this.Controls.Add(this.lblURL);
            this.Controls.Add(this.txtURL);
            this.Controls.Add(this.txtAuthors);
            this.Controls.Add(this.lblAuthors);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnStart);
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
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtCat;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.TextBox txtSubcat;
        private System.Windows.Forms.Label lblSubCategory;
        private System.Windows.Forms.Label lblGit;
        private System.Windows.Forms.CheckBox cbxEnabled;
        private System.Windows.Forms.Button btnStart;
        public System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.TextBox txtAuthors;
        private System.Windows.Forms.Label lblAuthors;
        private System.Windows.Forms.Label lblURL;
        private System.Windows.Forms.TextBox txtURL;
        private System.Windows.Forms.TreeView tvwItems;
        private System.Windows.Forms.Button btnNewPath;
        private System.Windows.Forms.Button btnEditPath;
        private System.Windows.Forms.Button btnDeletePath;
        private System.Windows.Forms.Button btnReset;
    }
}

