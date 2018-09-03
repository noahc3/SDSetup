namespace SDSetupManifestGenerator {
    partial class FormEditPlatform {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEditPlatform));
            this.txtId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.picIcon = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMenu = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtClasses = new System.Windows.Forms.TextBox();
            this.cbxVisible = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtImg = new System.Windows.Forms.TextBox();
            this.btnTestImg = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // txtId
            // 
            this.txtId.Location = new System.Drawing.Point(12, 29);
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(505, 20);
            this.txtId.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "ID";
            // 
            // picIcon
            // 
            this.picIcon.Location = new System.Drawing.Point(523, 12);
            this.picIcon.Name = "picIcon";
            this.picIcon.Size = new System.Drawing.Size(270, 270);
            this.picIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picIcon.TabIndex = 2;
            this.picIcon.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Full Name";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(12, 68);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(505, 20);
            this.txtName.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Menu Name";
            // 
            // txtMenu
            // 
            this.txtMenu.Location = new System.Drawing.Point(12, 107);
            this.txtMenu.Name = "txtMenu";
            this.txtMenu.Size = new System.Drawing.Size(505, 20);
            this.txtMenu.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 130);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(152, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "CSS Color (ie. red, blue, green)";
            // 
            // txtClasses
            // 
            this.txtClasses.Location = new System.Drawing.Point(12, 146);
            this.txtClasses.Name = "txtClasses";
            this.txtClasses.Size = new System.Drawing.Size(505, 20);
            this.txtClasses.TabIndex = 3;
            // 
            // cbxVisible
            // 
            this.cbxVisible.AutoSize = true;
            this.cbxVisible.Location = new System.Drawing.Point(12, 172);
            this.cbxVisible.Name = "cbxVisible";
            this.cbxVisible.Size = new System.Drawing.Size(67, 17);
            this.cbxVisible.TabIndex = 4;
            this.cbxVisible.Text = "Is Visible";
            this.cbxVisible.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 217);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Picture URL";
            // 
            // txtImg
            // 
            this.txtImg.Location = new System.Drawing.Point(12, 233);
            this.txtImg.Name = "txtImg";
            this.txtImg.Size = new System.Drawing.Size(505, 20);
            this.txtImg.TabIndex = 5;
            // 
            // btnTestImg
            // 
            this.btnTestImg.Location = new System.Drawing.Point(12, 259);
            this.btnTestImg.Name = "btnTestImg";
            this.btnTestImg.Size = new System.Drawing.Size(505, 23);
            this.btnTestImg.TabIndex = 6;
            this.btnTestImg.Text = "Test Image";
            this.btnTestImg.UseVisualStyleBackColor = true;
            this.btnTestImg.Click += new System.EventHandler(this.btnTestImg_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(661, 306);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(132, 23);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(523, 306);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(132, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FormEditPlatform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 342);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnTestImg);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtImg);
            this.Controls.Add(this.cbxVisible);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtClasses);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtMenu);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.picIcon);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtId);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormEditPlatform";
            this.Text = "Edit Platform";
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox picIcon;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMenu;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtClasses;
        private System.Windows.Forms.CheckBox cbxVisible;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtImg;
        private System.Windows.Forms.Button btnTestImg;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}