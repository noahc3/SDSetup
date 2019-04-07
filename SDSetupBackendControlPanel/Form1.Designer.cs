namespace SDSetupBackendControlPanel {
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnBuild = new System.Windows.Forms.Button();
            this.btnPushPrivateTesting = new System.Windows.Forms.Button();
            this.btnPushPublicTest = new System.Windows.Forms.Button();
            this.btnPushPublic = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btnBrowsePrivateKey = new System.Windows.Forms.Button();
            this.lblGuideDir = new System.Windows.Forms.Label();
            this.txtGuideDir = new System.Windows.Forms.TextBox();
            this.lblMasterPassword = new System.Windows.Forms.Label();
            this.btnConfigureMasterPassword = new System.Windows.Forms.Button();
            this.btnServerEdit = new System.Windows.Forms.Button();
            this.btnServerDelete = new System.Windows.Forms.Button();
            this.btnServerAdd = new System.Windows.Forms.Button();
            this.listboxServers = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDebug = new System.Windows.Forms.TextBox();
            this.btnBuildBackend = new System.Windows.Forms.Button();
            this.btnPushBackendPrivate = new System.Windows.Forms.Button();
            this.btnPushBackendPublic = new System.Windows.Forms.Button();
            this.btnBrowseBackendDir = new System.Windows.Forms.Button();
            this.lblBackendDir = new System.Windows.Forms.Label();
            this.txtBackendDir = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(804, 352);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnBuildBackend);
            this.tabPage1.Controls.Add(this.btnPushBackendPrivate);
            this.tabPage1.Controls.Add(this.btnPushBackendPublic);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(796, 326);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Backend";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnBuild);
            this.tabPage2.Controls.Add(this.btnPushPrivateTesting);
            this.tabPage2.Controls.Add(this.btnPushPublicTest);
            this.tabPage2.Controls.Add(this.btnPushPublic);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(796, 326);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Guide";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnBuild
            // 
            this.btnBuild.Location = new System.Drawing.Point(8, 169);
            this.btnBuild.Name = "btnBuild";
            this.btnBuild.Size = new System.Drawing.Size(776, 23);
            this.btnBuild.TabIndex = 3;
            this.btnBuild.Text = "Build Guide";
            this.btnBuild.UseVisualStyleBackColor = true;
            this.btnBuild.Click += new System.EventHandler(this.btnBuild_Click);
            // 
            // btnPushPrivateTesting
            // 
            this.btnPushPrivateTesting.Location = new System.Drawing.Point(8, 293);
            this.btnPushPrivateTesting.Name = "btnPushPrivateTesting";
            this.btnPushPrivateTesting.Size = new System.Drawing.Size(776, 23);
            this.btnPushPrivateTesting.TabIndex = 2;
            this.btnPushPrivateTesting.Text = "Push Build to Private Testing";
            this.btnPushPrivateTesting.UseVisualStyleBackColor = true;
            this.btnPushPrivateTesting.Click += new System.EventHandler(this.btnPushPrivateTesting_Click);
            // 
            // btnPushPublicTest
            // 
            this.btnPushPublicTest.Location = new System.Drawing.Point(8, 264);
            this.btnPushPublicTest.Name = "btnPushPublicTest";
            this.btnPushPublicTest.Size = new System.Drawing.Size(776, 23);
            this.btnPushPublicTest.TabIndex = 1;
            this.btnPushPublicTest.Text = "Push Build to Public Testing";
            this.btnPushPublicTest.UseVisualStyleBackColor = true;
            this.btnPushPublicTest.Click += new System.EventHandler(this.btnPushPublicTest_Click);
            // 
            // btnPushPublic
            // 
            this.btnPushPublic.Location = new System.Drawing.Point(8, 235);
            this.btnPushPublic.Name = "btnPushPublic";
            this.btnPushPublic.Size = new System.Drawing.Size(776, 23);
            this.btnPushPublic.TabIndex = 0;
            this.btnPushPublic.Text = "Push Build to Public";
            this.btnPushPublic.UseVisualStyleBackColor = true;
            this.btnPushPublic.Click += new System.EventHandler(this.btnPushPublic_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btnBrowseBackendDir);
            this.tabPage3.Controls.Add(this.lblBackendDir);
            this.tabPage3.Controls.Add(this.txtBackendDir);
            this.tabPage3.Controls.Add(this.btnBrowsePrivateKey);
            this.tabPage3.Controls.Add(this.lblGuideDir);
            this.tabPage3.Controls.Add(this.txtGuideDir);
            this.tabPage3.Controls.Add(this.lblMasterPassword);
            this.tabPage3.Controls.Add(this.btnConfigureMasterPassword);
            this.tabPage3.Controls.Add(this.btnServerEdit);
            this.tabPage3.Controls.Add(this.btnServerDelete);
            this.tabPage3.Controls.Add(this.btnServerAdd);
            this.tabPage3.Controls.Add(this.listboxServers);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(796, 326);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Config";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btnBrowsePrivateKey
            // 
            this.btnBrowsePrivateKey.Location = new System.Drawing.Point(709, 126);
            this.btnBrowsePrivateKey.Name = "btnBrowsePrivateKey";
            this.btnBrowsePrivateKey.Size = new System.Drawing.Size(75, 23);
            this.btnBrowsePrivateKey.TabIndex = 9;
            this.btnBrowsePrivateKey.Text = "Browse";
            this.btnBrowsePrivateKey.UseVisualStyleBackColor = true;
            this.btnBrowsePrivateKey.Click += new System.EventHandler(this.btnBrowsePrivateKey_Click);
            // 
            // lblGuideDir
            // 
            this.lblGuideDir.AutoSize = true;
            this.lblGuideDir.Location = new System.Drawing.Point(206, 84);
            this.lblGuideDir.Name = "lblGuideDir";
            this.lblGuideDir.Size = new System.Drawing.Size(146, 13);
            this.lblGuideDir.TabIndex = 8;
            this.lblGuideDir.Text = "Local Guide Source Directory";
            // 
            // txtGuideDir
            // 
            this.txtGuideDir.Location = new System.Drawing.Point(209, 100);
            this.txtGuideDir.Name = "txtGuideDir";
            this.txtGuideDir.Size = new System.Drawing.Size(575, 20);
            this.txtGuideDir.TabIndex = 7;
            this.txtGuideDir.TextChanged += new System.EventHandler(this.txtGuideDir_TextChanged);
            // 
            // lblMasterPassword
            // 
            this.lblMasterPassword.AutoSize = true;
            this.lblMasterPassword.Location = new System.Drawing.Point(8, 167);
            this.lblMasterPassword.Name = "lblMasterPassword";
            this.lblMasterPassword.Size = new System.Drawing.Size(162, 13);
            this.lblMasterPassword.TabIndex = 6;
            this.lblMasterPassword.Text = "Master Password Not Configured";
            // 
            // btnConfigureMasterPassword
            // 
            this.btnConfigureMasterPassword.Location = new System.Drawing.Point(11, 183);
            this.btnConfigureMasterPassword.Name = "btnConfigureMasterPassword";
            this.btnConfigureMasterPassword.Size = new System.Drawing.Size(192, 23);
            this.btnConfigureMasterPassword.TabIndex = 5;
            this.btnConfigureMasterPassword.Text = "Configure Master Password";
            this.btnConfigureMasterPassword.UseVisualStyleBackColor = true;
            this.btnConfigureMasterPassword.Click += new System.EventHandler(this.btnConfigureMasterPassword_Click);
            // 
            // btnServerEdit
            // 
            this.btnServerEdit.Location = new System.Drawing.Point(77, 126);
            this.btnServerEdit.Name = "btnServerEdit";
            this.btnServerEdit.Size = new System.Drawing.Size(60, 23);
            this.btnServerEdit.TabIndex = 4;
            this.btnServerEdit.Text = "Edit";
            this.btnServerEdit.UseVisualStyleBackColor = true;
            this.btnServerEdit.Click += new System.EventHandler(this.btnServerEdit_Click);
            // 
            // btnServerDelete
            // 
            this.btnServerDelete.Location = new System.Drawing.Point(143, 126);
            this.btnServerDelete.Name = "btnServerDelete";
            this.btnServerDelete.Size = new System.Drawing.Size(60, 23);
            this.btnServerDelete.TabIndex = 3;
            this.btnServerDelete.Text = "Delete";
            this.btnServerDelete.UseVisualStyleBackColor = true;
            this.btnServerDelete.Click += new System.EventHandler(this.btnServerDelete_Click);
            // 
            // btnServerAdd
            // 
            this.btnServerAdd.Location = new System.Drawing.Point(11, 126);
            this.btnServerAdd.Name = "btnServerAdd";
            this.btnServerAdd.Size = new System.Drawing.Size(60, 23);
            this.btnServerAdd.TabIndex = 2;
            this.btnServerAdd.Text = "Add";
            this.btnServerAdd.UseVisualStyleBackColor = true;
            this.btnServerAdd.Click += new System.EventHandler(this.btnServerAdd_Click);
            // 
            // listboxServers
            // 
            this.listboxServers.FormattingEnabled = true;
            this.listboxServers.Location = new System.Drawing.Point(11, 25);
            this.listboxServers.Name = "listboxServers";
            this.listboxServers.Size = new System.Drawing.Size(192, 95);
            this.listboxServers.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server Config";
            // 
            // txtDebug
            // 
            this.txtDebug.Location = new System.Drawing.Point(4, 354);
            this.txtDebug.Multiline = true;
            this.txtDebug.Name = "txtDebug";
            this.txtDebug.ReadOnly = true;
            this.txtDebug.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDebug.Size = new System.Drawing.Size(796, 276);
            this.txtDebug.TabIndex = 1;
            // 
            // btnBuildBackend
            // 
            this.btnBuildBackend.Location = new System.Drawing.Point(8, 297);
            this.btnBuildBackend.Name = "btnBuildBackend";
            this.btnBuildBackend.Size = new System.Drawing.Size(256, 23);
            this.btnBuildBackend.TabIndex = 6;
            this.btnBuildBackend.Text = "Build Backend";
            this.btnBuildBackend.UseVisualStyleBackColor = true;
            this.btnBuildBackend.Click += new System.EventHandler(this.btnBuildBackend_Click);
            // 
            // btnPushBackendPrivate
            // 
            this.btnPushBackendPrivate.Location = new System.Drawing.Point(532, 297);
            this.btnPushBackendPrivate.Name = "btnPushBackendPrivate";
            this.btnPushBackendPrivate.Size = new System.Drawing.Size(256, 23);
            this.btnPushBackendPrivate.TabIndex = 5;
            this.btnPushBackendPrivate.Text = "Push Build to Testing";
            this.btnPushBackendPrivate.UseVisualStyleBackColor = true;
            this.btnPushBackendPrivate.Click += new System.EventHandler(this.btnPushBackendPrivate_Click);
            // 
            // btnPushBackendPublic
            // 
            this.btnPushBackendPublic.Location = new System.Drawing.Point(270, 297);
            this.btnPushBackendPublic.Name = "btnPushBackendPublic";
            this.btnPushBackendPublic.Size = new System.Drawing.Size(256, 23);
            this.btnPushBackendPublic.TabIndex = 4;
            this.btnPushBackendPublic.Text = "Push Build to Public";
            this.btnPushBackendPublic.UseVisualStyleBackColor = true;
            this.btnPushBackendPublic.Click += new System.EventHandler(this.btnPushBackendPublic_Click);
            // 
            // btnBrowseBackendDir
            // 
            this.btnBrowseBackendDir.Location = new System.Drawing.Point(709, 51);
            this.btnBrowseBackendDir.Name = "btnBrowseBackendDir";
            this.btnBrowseBackendDir.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseBackendDir.TabIndex = 12;
            this.btnBrowseBackendDir.Text = "Browse";
            this.btnBrowseBackendDir.UseVisualStyleBackColor = true;
            this.btnBrowseBackendDir.Click += new System.EventHandler(this.btnBrowseBackendDir_Click);
            // 
            // lblBackendDir
            // 
            this.lblBackendDir.AutoSize = true;
            this.lblBackendDir.Location = new System.Drawing.Point(206, 9);
            this.lblBackendDir.Name = "lblBackendDir";
            this.lblBackendDir.Size = new System.Drawing.Size(161, 13);
            this.lblBackendDir.TabIndex = 11;
            this.lblBackendDir.Text = "Local Backend Source Directory";
            // 
            // txtBackendDir
            // 
            this.txtBackendDir.Location = new System.Drawing.Point(209, 25);
            this.txtBackendDir.Name = "txtBackendDir";
            this.txtBackendDir.Size = new System.Drawing.Size(575, 20);
            this.txtBackendDir.TabIndex = 10;
            this.txtBackendDir.TextChanged += new System.EventHandler(this.txtBackendDir_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 632);
            this.Controls.Add(this.txtDebug);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnServerEdit;
        private System.Windows.Forms.Button btnServerDelete;
        private System.Windows.Forms.Button btnServerAdd;
        private System.Windows.Forms.ListBox listboxServers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDebug;
        private System.Windows.Forms.Label lblMasterPassword;
        private System.Windows.Forms.Button btnConfigureMasterPassword;
        private System.Windows.Forms.Label lblGuideDir;
        private System.Windows.Forms.TextBox txtGuideDir;
        private System.Windows.Forms.Button btnBrowsePrivateKey;
        private System.Windows.Forms.Button btnBuild;
        private System.Windows.Forms.Button btnPushPrivateTesting;
        private System.Windows.Forms.Button btnPushPublicTest;
        private System.Windows.Forms.Button btnPushPublic;
        private System.Windows.Forms.Button btnBuildBackend;
        private System.Windows.Forms.Button btnPushBackendPrivate;
        private System.Windows.Forms.Button btnPushBackendPublic;
        private System.Windows.Forms.Button btnBrowseBackendDir;
        private System.Windows.Forms.Label lblBackendDir;
        private System.Windows.Forms.TextBox txtBackendDir;
    }
}

