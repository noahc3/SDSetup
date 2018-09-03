namespace SDSetupManifestGenerator {
    partial class FormViewSubcategories {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormViewSubcategories));
            this.label2 = new System.Windows.Forms.Label();
            this.ddlSections = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ddlPlatforms = new System.Windows.Forms.ComboBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.lvwSubcategories = new System.Windows.Forms.ListView();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.ddlCategories = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 29;
            this.label2.Text = "Section";
            // 
            // ddlSections
            // 
            this.ddlSections.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlSections.FormattingEnabled = true;
            this.ddlSections.Location = new System.Drawing.Point(12, 65);
            this.ddlSections.Name = "ddlSections";
            this.ddlSections.Size = new System.Drawing.Size(189, 21);
            this.ddlSections.TabIndex = 22;
            this.ddlSections.SelectedValueChanged += new System.EventHandler(this.ddlSections_SelectedValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 28;
            this.label1.Text = "Platform";
            // 
            // ddlPlatforms
            // 
            this.ddlPlatforms.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlPlatforms.FormattingEnabled = true;
            this.ddlPlatforms.Location = new System.Drawing.Point(12, 25);
            this.ddlPlatforms.Name = "ddlPlatforms";
            this.ddlPlatforms.Size = new System.Drawing.Size(189, 21);
            this.ddlPlatforms.TabIndex = 21;
            this.ddlPlatforms.SelectedValueChanged += new System.EventHandler(this.ddlPlatforms_SelectedValueChanged);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(12, 504);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(189, 23);
            this.btnClose.TabIndex = 27;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lvwSubcategories
            // 
            this.lvwSubcategories.Location = new System.Drawing.Point(12, 132);
            this.lvwSubcategories.Name = "lvwSubcategories";
            this.lvwSubcategories.Size = new System.Drawing.Size(189, 264);
            this.lvwSubcategories.TabIndex = 23;
            this.lvwSubcategories.UseCompatibleStateImageBehavior = false;
            this.lvwSubcategories.View = System.Windows.Forms.View.List;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(12, 460);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(189, 23);
            this.btnDelete.TabIndex = 26;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(12, 431);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(189, 23);
            this.btnEdit.TabIndex = 25;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(12, 402);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(189, 23);
            this.btnNew.TabIndex = 24;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 31;
            this.label3.Text = "Category";
            // 
            // ddlCategories
            // 
            this.ddlCategories.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlCategories.FormattingEnabled = true;
            this.ddlCategories.Location = new System.Drawing.Point(12, 105);
            this.ddlCategories.Name = "ddlCategories";
            this.ddlCategories.Size = new System.Drawing.Size(189, 21);
            this.ddlCategories.TabIndex = 30;
            this.ddlCategories.SelectedValueChanged += new System.EventHandler(this.ddlCategories_SelectedValueChanged);
            // 
            // FormViewSubcategories
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(215, 538);
            this.ControlBox = false;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ddlCategories);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ddlSections);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ddlPlatforms);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lvwSubcategories);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnNew);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormViewSubcategories";
            this.Text = "View Subcategories";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ddlSections;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ddlPlatforms;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ListView lvwSubcategories;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ddlCategories;
    }
}