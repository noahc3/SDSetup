namespace SDSetupBackendControlPanel {
    partial class GetMasterPassword {
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
            this.lblPassword = new System.Windows.Forms.Label();
            this.btnSetPassword = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPasswordValid = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(9, 9);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(116, 13);
            this.lblPassword.TabIndex = 11;
            this.lblPassword.Text = "Enter Master Password";
            // 
            // btnSetPassword
            // 
            this.btnSetPassword.Location = new System.Drawing.Point(12, 102);
            this.btnSetPassword.Name = "btnSetPassword";
            this.btnSetPassword.Size = new System.Drawing.Size(227, 23);
            this.btnSetPassword.TabIndex = 10;
            this.btnSetPassword.Text = "Continue";
            this.btnSetPassword.UseVisualStyleBackColor = true;
            this.btnSetPassword.Click += new System.EventHandler(this.btnSetPassword_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(12, 28);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(227, 20);
            this.txtPassword.TabIndex = 8;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // lblPasswordValid
            // 
            this.lblPasswordValid.AutoSize = true;
            this.lblPasswordValid.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPasswordValid.ForeColor = System.Drawing.Color.Red;
            this.lblPasswordValid.Location = new System.Drawing.Point(9, 51);
            this.lblPasswordValid.Name = "lblPasswordValid";
            this.lblPasswordValid.Size = new System.Drawing.Size(133, 13);
            this.lblPasswordValid.TabIndex = 12;
            this.lblPasswordValid.Text = "Master Password Incorrect";
            this.lblPasswordValid.Visible = false;
            // 
            // GetMasterPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(252, 136);
            this.Controls.Add(this.lblPasswordValid);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.btnSetPassword);
            this.Controls.Add(this.txtPassword);
            this.Name = "GetMasterPassword";
            this.Text = "GetMasterPassword";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Button btnSetPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPasswordValid;
    }
}