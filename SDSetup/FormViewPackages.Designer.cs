namespace SDSetupManifestGenerator {
    partial class FormViewPackages {
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
            this.label3 = new System.Windows.Forms.Label();
            this.ddlCategories = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ddlSections = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ddlPlatforms = new System.Windows.Forms.ComboBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.lvwPackages = new System.Windows.Forms.ListView();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.ddlSubcategories = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 42;
            this.label3.Text = "Category";
            // 
            // ddlCategories
            // 
            this.ddlCategories.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlCategories.FormattingEnabled = true;
            this.ddlCategories.Location = new System.Drawing.Point(12, 105);
            this.ddlCategories.Name = "ddlCategories";
            this.ddlCategories.Size = new System.Drawing.Size(189, 21);
            this.ddlCategories.TabIndex = 41;
            this.ddlCategories.SelectedValueChanged += new System.EventHandler(this.ddlCategories_SelectedValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 40;
            this.label2.Text = "Section";
            // 
            // ddlSections
            // 
            this.ddlSections.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlSections.FormattingEnabled = true;
            this.ddlSections.Location = new System.Drawing.Point(12, 65);
            this.ddlSections.Name = "ddlSections";
            this.ddlSections.Size = new System.Drawing.Size(189, 21);
            this.ddlSections.TabIndex = 33;
            this.ddlSections.SelectedValueChanged += new System.EventHandler(this.ddlSections_SelectedValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 39;
            this.label1.Text = "Platform";
            // 
            // ddlPlatforms
            // 
            this.ddlPlatforms.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlPlatforms.FormattingEnabled = true;
            this.ddlPlatforms.Location = new System.Drawing.Point(12, 25);
            this.ddlPlatforms.Name = "ddlPlatforms";
            this.ddlPlatforms.Size = new System.Drawing.Size(189, 21);
            this.ddlPlatforms.TabIndex = 32;
            this.ddlPlatforms.SelectedValueChanged += new System.EventHandler(this.ddlPlatforms_SelectedValueChanged);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(12, 544);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(189, 23);
            this.btnClose.TabIndex = 38;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lvwPackages
            // 
            this.lvwPackages.Location = new System.Drawing.Point(12, 172);
            this.lvwPackages.Name = "lvwPackages";
            this.lvwPackages.Size = new System.Drawing.Size(189, 264);
            this.lvwPackages.TabIndex = 34;
            this.lvwPackages.UseCompatibleStateImageBehavior = false;
            this.lvwPackages.View = System.Windows.Forms.View.List;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(12, 500);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(189, 23);
            this.btnDelete.TabIndex = 37;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(12, 471);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(189, 23);
            this.btnEdit.TabIndex = 36;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(12, 442);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(189, 23);
            this.btnNew.TabIndex = 35;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 129);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 44;
            this.label4.Text = "Subcategory";
            // 
            // ddlSubcategories
            // 
            this.ddlSubcategories.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlSubcategories.FormattingEnabled = true;
            this.ddlSubcategories.Location = new System.Drawing.Point(12, 145);
            this.ddlSubcategories.Name = "ddlSubcategories";
            this.ddlSubcategories.Size = new System.Drawing.Size(189, 21);
            this.ddlSubcategories.TabIndex = 43;
            this.ddlSubcategories.SelectedValueChanged += new System.EventHandler(this.ddlSubcategories_SelectedValueChanged);
            // 
            // FormViewPackages
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(213, 577);
            this.ControlBox = false;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ddlSubcategories);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ddlCategories);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ddlSections);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ddlPlatforms);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lvwPackages);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnNew);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormViewPackages";
            this.Text = "FormViewPackages";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ddlCategories;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ddlSections;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ddlPlatforms;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ListView lvwPackages;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox ddlSubcategories;
    }
}