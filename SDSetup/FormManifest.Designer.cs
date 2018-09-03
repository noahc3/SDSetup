namespace SDSetupManifestGenerator {
    partial class FormManifest {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormManifest));
            this.txtCopyright = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.gbxIndexMessage = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtClasses = new System.Windows.Forms.TextBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.txtManifestVersion = new System.Windows.Forms.TextBox();
            this.gbxIndexMessage.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtCopyright
            // 
            this.txtCopyright.Location = new System.Drawing.Point(12, 93);
            this.txtCopyright.Multiline = true;
            this.txtCopyright.Name = "txtCopyright";
            this.txtCopyright.Size = new System.Drawing.Size(776, 111);
            this.txtCopyright.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(644, 335);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(144, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(494, 335);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(144, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Copyright Footer (HTML)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(225, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Manifest Version (whydidimakethisastring.jpeg)";
            // 
            // gbxIndexMessage
            // 
            this.gbxIndexMessage.Controls.Add(this.label4);
            this.gbxIndexMessage.Controls.Add(this.label3);
            this.gbxIndexMessage.Controls.Add(this.txtClasses);
            this.gbxIndexMessage.Controls.Add(this.txtMessage);
            this.gbxIndexMessage.Location = new System.Drawing.Point(12, 210);
            this.gbxIndexMessage.Name = "gbxIndexMessage";
            this.gbxIndexMessage.Size = new System.Drawing.Size(776, 107);
            this.gbxIndexMessage.TabIndex = 2;
            this.gbxIndexMessage.TabStop = false;
            this.gbxIndexMessage.Text = "Front Page Notification";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(185, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "CSS Color (ie. info, warning, negative)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(192, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Message (HTML) (Leave blank to hide)";
            // 
            // txtClasses
            // 
            this.txtClasses.Location = new System.Drawing.Point(13, 81);
            this.txtClasses.Name = "txtClasses";
            this.txtClasses.Size = new System.Drawing.Size(757, 20);
            this.txtClasses.TabIndex = 4;
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(13, 34);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(757, 20);
            this.txtMessage.TabIndex = 3;
            // 
            // txtManifestVersion
            // 
            this.txtManifestVersion.Location = new System.Drawing.Point(12, 37);
            this.txtManifestVersion.Name = "txtManifestVersion";
            this.txtManifestVersion.Size = new System.Drawing.Size(776, 20);
            this.txtManifestVersion.TabIndex = 0;
            // 
            // FormManifest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 376);
            this.ControlBox = false;
            this.Controls.Add(this.txtManifestVersion);
            this.Controls.Add(this.gbxIndexMessage);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtCopyright);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormManifest";
            this.Text = "Edit Global Manifest Info";
            this.gbxIndexMessage.ResumeLayout(false);
            this.gbxIndexMessage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtCopyright;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox gbxIndexMessage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtClasses;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.TextBox txtManifestVersion;
    }
}