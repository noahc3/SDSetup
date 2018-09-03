namespace SDSetupManifestGenerator {
    partial class FormViewSections {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormViewSections));
            this.btnClose = new System.Windows.Forms.Button();
            this.lvwSections = new System.Windows.Forms.ListView();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.ddlPlatforms = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(9, 424);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(189, 23);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lvwSections
            // 
            this.lvwSections.Location = new System.Drawing.Point(9, 52);
            this.lvwSections.Name = "lvwSections";
            this.lvwSections.Size = new System.Drawing.Size(189, 264);
            this.lvwSections.TabIndex = 5;
            this.lvwSections.UseCompatibleStateImageBehavior = false;
            this.lvwSections.View = System.Windows.Forms.View.List;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(9, 380);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(189, 23);
            this.btnDelete.TabIndex = 8;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(9, 351);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(189, 23);
            this.btnEdit.TabIndex = 7;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(9, 322);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(189, 23);
            this.btnNew.TabIndex = 6;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // ddlPlatforms
            // 
            this.ddlPlatforms.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlPlatforms.FormattingEnabled = true;
            this.ddlPlatforms.Location = new System.Drawing.Point(9, 25);
            this.ddlPlatforms.Name = "ddlPlatforms";
            this.ddlPlatforms.Size = new System.Drawing.Size(189, 21);
            this.ddlPlatforms.TabIndex = 10;
            this.ddlPlatforms.SelectedIndexChanged += new System.EventHandler(this.ddlPlatforms_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Platform";
            // 
            // FormViewSections
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(210, 458);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ddlPlatforms);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lvwSections);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnNew);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormViewSections";
            this.Text = "View Sections";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ListView lvwSections;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.ComboBox ddlPlatforms;
        private System.Windows.Forms.Label label1;
    }
}