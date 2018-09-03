namespace SDSetupManifestGenerator {
    partial class FormEditPackage {
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
            this.label5 = new System.Windows.Forms.Label();
            this.ddlCategories = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ddlSections = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.ddlPlatforms = new System.Windows.Forms.ComboBox();
            this.cbxVisible = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDisplayName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtId = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.ddlSubcategories = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtAuthors = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtDlSource = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.cbxDefault = new System.Windows.Forms.CheckBox();
            this.lvwWhenDependencies = new System.Windows.Forms.ListView();
            this.txtWhen = new System.Windows.Forms.TextBox();
            this.btnAddWhen = new System.Windows.Forms.Button();
            this.btnDeleteWhen = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.nudWhenMode = new System.Windows.Forms.NumericUpDown();
            this.lvwDependencies = new System.Windows.Forms.ListView();
            this.txtDependency = new System.Windows.Forms.TextBox();
            this.btnAddDependency = new System.Windows.Forms.Button();
            this.btnDeleteDependency = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.btnFetch = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudWhenMode)).BeginInit();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 87);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 71;
            this.label5.Text = "Category";
            // 
            // ddlCategories
            // 
            this.ddlCategories.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlCategories.FormattingEnabled = true;
            this.ddlCategories.Location = new System.Drawing.Point(12, 102);
            this.ddlCategories.Name = "ddlCategories";
            this.ddlCategories.Size = new System.Drawing.Size(171, 21);
            this.ddlCategories.TabIndex = 2;
            this.ddlCategories.SelectedValueChanged += new System.EventHandler(this.ddlCategories_SelectedValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 69;
            this.label4.Text = "Section";
            // 
            // ddlSections
            // 
            this.ddlSections.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlSections.FormattingEnabled = true;
            this.ddlSections.Location = new System.Drawing.Point(12, 63);
            this.ddlSections.Name = "ddlSections";
            this.ddlSections.Size = new System.Drawing.Size(171, 21);
            this.ddlSections.TabIndex = 1;
            this.ddlSections.SelectedValueChanged += new System.EventHandler(this.ddlSections_SelectedValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 13);
            this.label8.TabIndex = 67;
            this.label8.Text = "Platform";
            // 
            // ddlPlatforms
            // 
            this.ddlPlatforms.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlPlatforms.FormattingEnabled = true;
            this.ddlPlatforms.Location = new System.Drawing.Point(12, 24);
            this.ddlPlatforms.Name = "ddlPlatforms";
            this.ddlPlatforms.Size = new System.Drawing.Size(171, 21);
            this.ddlPlatforms.TabIndex = 0;
            this.ddlPlatforms.SelectedValueChanged += new System.EventHandler(this.ddlPlatforms_SelectedValueChanged);
            // 
            // cbxVisible
            // 
            this.cbxVisible.AutoSize = true;
            this.cbxVisible.Location = new System.Drawing.Point(12, 560);
            this.cbxVisible.Name = "cbxVisible";
            this.cbxVisible.Size = new System.Drawing.Size(56, 17);
            this.cbxVisible.TabIndex = 12;
            this.cbxVisible.Text = "Visible";
            this.cbxVisible.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 243);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 64;
            this.label3.Text = "Display Name";
            // 
            // txtDisplayName
            // 
            this.txtDisplayName.Location = new System.Drawing.Point(12, 259);
            this.txtDisplayName.Name = "txtDisplayName";
            this.txtDisplayName.Size = new System.Drawing.Size(505, 20);
            this.txtDisplayName.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 204);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 62;
            this.label2.Text = "Name";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(12, 220);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(505, 20);
            this.txtName.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 165);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 13);
            this.label1.TabIndex = 60;
            this.label1.Text = "ID";
            // 
            // txtId
            // 
            this.txtId.Location = new System.Drawing.Point(12, 181);
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(505, 20);
            this.txtId.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 126);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 73;
            this.label6.Text = "Subcategory";
            // 
            // ddlSubcategories
            // 
            this.ddlSubcategories.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlSubcategories.FormattingEnabled = true;
            this.ddlSubcategories.Location = new System.Drawing.Point(12, 141);
            this.ddlSubcategories.Name = "ddlSubcategories";
            this.ddlSubcategories.Size = new System.Drawing.Size(171, 21);
            this.ddlSubcategories.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 282);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 13);
            this.label7.TabIndex = 75;
            this.label7.Text = "Authors";
            // 
            // txtAuthors
            // 
            this.txtAuthors.Location = new System.Drawing.Point(12, 298);
            this.txtAuthors.Name = "txtAuthors";
            this.txtAuthors.Size = new System.Drawing.Size(505, 20);
            this.txtAuthors.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 321);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(42, 13);
            this.label9.TabIndex = 77;
            this.label9.Text = "Version";
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(12, 337);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(505, 20);
            this.txtVersion.TabIndex = 8;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 360);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 13);
            this.label10.TabIndex = 79;
            this.label10.Text = "Source";
            // 
            // txtSource
            // 
            this.txtSource.Location = new System.Drawing.Point(12, 376);
            this.txtSource.Name = "txtSource";
            this.txtSource.Size = new System.Drawing.Size(505, 20);
            this.txtSource.TabIndex = 9;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 399);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(92, 13);
            this.label11.TabIndex = 81;
            this.label11.Text = "Download Source";
            // 
            // txtDlSource
            // 
            this.txtDlSource.Location = new System.Drawing.Point(12, 415);
            this.txtDlSource.Name = "txtDlSource";
            this.txtDlSource.Size = new System.Drawing.Size(505, 20);
            this.txtDlSource.TabIndex = 10;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(9, 438);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(99, 13);
            this.label12.TabIndex = 83;
            this.label12.Text = "Description (HTML)";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(12, 454);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(505, 100);
            this.txtDescription.TabIndex = 11;
            // 
            // cbxDefault
            // 
            this.cbxDefault.AutoSize = true;
            this.cbxDefault.Location = new System.Drawing.Point(74, 560);
            this.cbxDefault.Name = "cbxDefault";
            this.cbxDefault.Size = new System.Drawing.Size(117, 17);
            this.cbxDefault.TabIndex = 13;
            this.cbxDefault.Text = "Enabled By Default";
            this.cbxDefault.UseVisualStyleBackColor = true;
            // 
            // lvwWhenDependencies
            // 
            this.lvwWhenDependencies.Location = new System.Drawing.Point(535, 65);
            this.lvwWhenDependencies.Name = "lvwWhenDependencies";
            this.lvwWhenDependencies.Size = new System.Drawing.Size(168, 94);
            this.lvwWhenDependencies.TabIndex = 15;
            this.lvwWhenDependencies.UseCompatibleStateImageBehavior = false;
            this.lvwWhenDependencies.View = System.Windows.Forms.View.List;
            // 
            // txtWhen
            // 
            this.txtWhen.Location = new System.Drawing.Point(535, 205);
            this.txtWhen.Name = "txtWhen";
            this.txtWhen.Size = new System.Drawing.Size(168, 20);
            this.txtWhen.TabIndex = 17;
            // 
            // btnAddWhen
            // 
            this.btnAddWhen.Location = new System.Drawing.Point(535, 231);
            this.btnAddWhen.Name = "btnAddWhen";
            this.btnAddWhen.Size = new System.Drawing.Size(168, 23);
            this.btnAddWhen.TabIndex = 18;
            this.btnAddWhen.Text = "Add";
            this.btnAddWhen.UseVisualStyleBackColor = true;
            this.btnAddWhen.Click += new System.EventHandler(this.btnAddWhen_Click);
            // 
            // btnDeleteWhen
            // 
            this.btnDeleteWhen.Location = new System.Drawing.Point(535, 165);
            this.btnDeleteWhen.Name = "btnDeleteWhen";
            this.btnDeleteWhen.Size = new System.Drawing.Size(168, 23);
            this.btnDeleteWhen.TabIndex = 16;
            this.btnDeleteWhen.Text = "Delete";
            this.btnDeleteWhen.UseVisualStyleBackColor = true;
            this.btnDeleteWhen.Click += new System.EventHandler(this.btnDeleteWhen_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(532, 48);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(108, 13);
            this.label13.TabIndex = 90;
            this.label13.Text = "When Dependencies";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(532, 9);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(132, 13);
            this.label14.TabIndex = 89;
            this.label14.Text = "When Mode (0: all, 1: any)";
            // 
            // nudWhenMode
            // 
            this.nudWhenMode.Location = new System.Drawing.Point(535, 25);
            this.nudWhenMode.Name = "nudWhenMode";
            this.nudWhenMode.Size = new System.Drawing.Size(168, 20);
            this.nudWhenMode.TabIndex = 14;
            // 
            // lvwDependencies
            // 
            this.lvwDependencies.Location = new System.Drawing.Point(535, 298);
            this.lvwDependencies.Name = "lvwDependencies";
            this.lvwDependencies.Size = new System.Drawing.Size(168, 94);
            this.lvwDependencies.TabIndex = 19;
            this.lvwDependencies.UseCompatibleStateImageBehavior = false;
            this.lvwDependencies.View = System.Windows.Forms.View.List;
            // 
            // txtDependency
            // 
            this.txtDependency.Location = new System.Drawing.Point(535, 438);
            this.txtDependency.Name = "txtDependency";
            this.txtDependency.Size = new System.Drawing.Size(168, 20);
            this.txtDependency.TabIndex = 21;
            // 
            // btnAddDependency
            // 
            this.btnAddDependency.Location = new System.Drawing.Point(535, 464);
            this.btnAddDependency.Name = "btnAddDependency";
            this.btnAddDependency.Size = new System.Drawing.Size(168, 23);
            this.btnAddDependency.TabIndex = 22;
            this.btnAddDependency.Text = "Add";
            this.btnAddDependency.UseVisualStyleBackColor = true;
            this.btnAddDependency.Click += new System.EventHandler(this.btnAddDependency_Click);
            // 
            // btnDeleteDependency
            // 
            this.btnDeleteDependency.Location = new System.Drawing.Point(535, 398);
            this.btnDeleteDependency.Name = "btnDeleteDependency";
            this.btnDeleteDependency.Size = new System.Drawing.Size(168, 23);
            this.btnDeleteDependency.TabIndex = 20;
            this.btnDeleteDependency.Text = "Delete";
            this.btnDeleteDependency.UseVisualStyleBackColor = true;
            this.btnDeleteDependency.Click += new System.EventHandler(this.btnDeleteDependency_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(532, 281);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(76, 13);
            this.label15.TabIndex = 95;
            this.label15.Text = "Dependencies";
            // 
            // btnFetch
            // 
            this.btnFetch.Location = new System.Drawing.Point(535, 531);
            this.btnFetch.Name = "btnFetch";
            this.btnFetch.Size = new System.Drawing.Size(168, 23);
            this.btnFetch.TabIndex = 23;
            this.btnFetch.Text = "Fetch Files";
            this.btnFetch.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(433, 578);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(132, 23);
            this.btnCancel.TabIndex = 25;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(571, 578);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(132, 23);
            this.btnSave.TabIndex = 24;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // FormEditPackage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(725, 613);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnFetch);
            this.Controls.Add(this.lvwDependencies);
            this.Controls.Add(this.txtDependency);
            this.Controls.Add(this.btnAddDependency);
            this.Controls.Add(this.btnDeleteDependency);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.lvwWhenDependencies);
            this.Controls.Add(this.txtWhen);
            this.Controls.Add(this.btnAddWhen);
            this.Controls.Add(this.btnDeleteWhen);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.nudWhenMode);
            this.Controls.Add(this.cbxDefault);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtDlSource);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtSource);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtVersion);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtAuthors);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.ddlSubcategories);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ddlCategories);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ddlSections);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.ddlPlatforms);
            this.Controls.Add(this.cbxVisible);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtDisplayName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtId);
            this.Name = "FormEditPackage";
            this.Text = "FormEditPackage";
            ((System.ComponentModel.ISupportInitialize)(this.nudWhenMode)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox ddlCategories;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox ddlSections;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox ddlPlatforms;
        private System.Windows.Forms.CheckBox cbxVisible;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDisplayName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox ddlSubcategories;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtAuthors;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtVersion;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtDlSource;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.CheckBox cbxDefault;
        private System.Windows.Forms.ListView lvwWhenDependencies;
        private System.Windows.Forms.TextBox txtWhen;
        private System.Windows.Forms.Button btnAddWhen;
        private System.Windows.Forms.Button btnDeleteWhen;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown nudWhenMode;
        private System.Windows.Forms.ListView lvwDependencies;
        private System.Windows.Forms.TextBox txtDependency;
        private System.Windows.Forms.Button btnAddDependency;
        private System.Windows.Forms.Button btnDeleteDependency;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btnFetch;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
    }
}