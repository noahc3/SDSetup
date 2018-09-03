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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.lblGit = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnPlatforms = new System.Windows.Forms.Button();
            this.btnCategories = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.btnSections = new System.Windows.Forms.Button();
            this.btnWriteManifest = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnManifestInfo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblGit
            // 
            this.lblGit.AutoSize = true;
            this.lblGit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGit.ForeColor = System.Drawing.Color.Red;
            this.lblGit.Location = new System.Drawing.Point(12, 188);
            this.lblGit.Name = "lblGit";
            this.lblGit.Size = new System.Drawing.Size(129, 13);
            this.lblGit.TabIndex = 69;
            this.lblGit.Text = "Status: Not Authenticated";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(12, 50);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 68;
            this.lblPassword.Text = "Password";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(12, 9);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(55, 13);
            this.lblUsername.TabIndex = 67;
            this.lblUsername.Text = "Username";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(15, 66);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(218, 20);
            this.txtPassword.TabIndex = 65;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(15, 25);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(218, 20);
            this.txtUsername.TabIndex = 64;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(15, 162);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(218, 23);
            this.btnLogin.TabIndex = 66;
            this.btnLogin.Text = "Login to GitHub";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(221, 70);
            this.label1.TabIndex = 70;
            this.label1.Text = "Please note that your username and password are stored in plaintext on your disk," +
    " because implementing OAuth flow requires a server that I don\'t feel like paying" +
    " for, setting up, or maintaining.";
            // 
            // btnPlatforms
            // 
            this.btnPlatforms.Location = new System.Drawing.Point(15, 279);
            this.btnPlatforms.Name = "btnPlatforms";
            this.btnPlatforms.Size = new System.Drawing.Size(218, 23);
            this.btnPlatforms.TabIndex = 71;
            this.btnPlatforms.Text = "Edit Platforms";
            this.btnPlatforms.UseVisualStyleBackColor = true;
            this.btnPlatforms.Click += new System.EventHandler(this.btnPlatforms_Click);
            // 
            // btnCategories
            // 
            this.btnCategories.Location = new System.Drawing.Point(15, 337);
            this.btnCategories.Name = "btnCategories";
            this.btnCategories.Size = new System.Drawing.Size(218, 23);
            this.btnCategories.TabIndex = 72;
            this.btnCategories.Text = "Edit Categories";
            this.btnCategories.UseVisualStyleBackColor = true;
            this.btnCategories.Click += new System.EventHandler(this.btnCategories_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(15, 366);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(218, 23);
            this.button2.TabIndex = 73;
            this.button2.Text = "Edit Subcategories";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(15, 395);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(218, 23);
            this.button3.TabIndex = 74;
            this.button3.Text = "Edit Packages";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnSections
            // 
            this.btnSections.Location = new System.Drawing.Point(15, 308);
            this.btnSections.Name = "btnSections";
            this.btnSections.Size = new System.Drawing.Size(218, 23);
            this.btnSections.TabIndex = 75;
            this.btnSections.Text = "Edit Sections";
            this.btnSections.UseVisualStyleBackColor = true;
            this.btnSections.Click += new System.EventHandler(this.btnSections_Click);
            // 
            // btnWriteManifest
            // 
            this.btnWriteManifest.BackColor = System.Drawing.Color.Green;
            this.btnWriteManifest.ForeColor = System.Drawing.Color.White;
            this.btnWriteManifest.Location = new System.Drawing.Point(15, 445);
            this.btnWriteManifest.Name = "btnWriteManifest";
            this.btnWriteManifest.Size = new System.Drawing.Size(218, 23);
            this.btnWriteManifest.TabIndex = 76;
            this.btnWriteManifest.Text = "Write Manifest";
            this.btnWriteManifest.UseVisualStyleBackColor = false;
            this.btnWriteManifest.Click += new System.EventHandler(this.btnWriteManifest_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(73, 471);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(178, 23);
            this.label2.TabIndex = 77;
            this.label2.Text = "Manifest Authoring Tool 5.0";
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // btnManifestInfo
            // 
            this.btnManifestInfo.Location = new System.Drawing.Point(15, 250);
            this.btnManifestInfo.Name = "btnManifestInfo";
            this.btnManifestInfo.Size = new System.Drawing.Size(218, 23);
            this.btnManifestInfo.TabIndex = 78;
            this.btnManifestInfo.Text = "Edit Global Manifest Info";
            this.btnManifestInfo.UseVisualStyleBackColor = true;
            this.btnManifestInfo.Click += new System.EventHandler(this.btnManifestInfo_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(252, 503);
            this.Controls.Add(this.btnManifestInfo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnWriteManifest);
            this.Controls.Add(this.btnSections);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnCategories);
            this.Controls.Add(this.btnPlatforms);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblGit);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.btnLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.Text = "Manifest Tool 5";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblGit;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnPlatforms;
        private System.Windows.Forms.Button btnCategories;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btnSections;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnManifestInfo;
        public System.Windows.Forms.Button btnWriteManifest;
    }
}