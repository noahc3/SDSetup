namespace SDSetupManifestGenerator {
    partial class FormFetch {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFetch));
            this.tvwItems = new System.Windows.Forms.TreeView();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.btnFetchUrl = new System.Windows.Forms.Button();
            this.btnFetchFromFile = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDeletePath = new System.Windows.Forms.Button();
            this.btnEditPath = new System.Windows.Forms.Button();
            this.btnNewPath = new System.Windows.Forms.Button();
            this.txtUploadUrl = new System.Windows.Forms.TextBox();
            this.btnFetchFolder = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tvwItems
            // 
            this.tvwItems.AllowDrop = true;
            this.tvwItems.FullRowSelect = true;
            this.tvwItems.LabelEdit = true;
            this.tvwItems.Location = new System.Drawing.Point(541, 12);
            this.tvwItems.Name = "tvwItems";
            this.tvwItems.PathSeparator = "/";
            this.tvwItems.Size = new System.Drawing.Size(387, 656);
            this.tvwItems.TabIndex = 52;
            this.tvwItems.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tvwItems_ItemDrag);
            this.tvwItems.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvwItems_DragDrop);
            this.tvwItems.DragOver += new System.Windows.Forms.DragEventHandler(this.tvwItems_DragOver);
            this.tvwItems.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tvwItems_MouseUp);
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(12, 12);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(523, 20);
            this.txtUrl.TabIndex = 53;
            // 
            // btnFetchUrl
            // 
            this.btnFetchUrl.Location = new System.Drawing.Point(12, 38);
            this.btnFetchUrl.Name = "btnFetchUrl";
            this.btnFetchUrl.Size = new System.Drawing.Size(523, 23);
            this.btnFetchUrl.TabIndex = 54;
            this.btnFetchUrl.Text = "Fetch from Url";
            this.btnFetchUrl.UseVisualStyleBackColor = true;
            this.btnFetchUrl.Click += new System.EventHandler(this.btnFetchUrl_Click);
            // 
            // btnFetchFromFile
            // 
            this.btnFetchFromFile.Location = new System.Drawing.Point(12, 67);
            this.btnFetchFromFile.Name = "btnFetchFromFile";
            this.btnFetchFromFile.Size = new System.Drawing.Size(258, 23);
            this.btnFetchFromFile.TabIndex = 56;
            this.btnFetchFromFile.Text = "Fetch from File";
            this.btnFetchFromFile.UseVisualStyleBackColor = true;
            this.btnFetchFromFile.Click += new System.EventHandler(this.btnFetchFromFile_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 674);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(102, 23);
            this.btnSave.TabIndex = 57;
            this.btnSave.Text = "Done";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDeletePath
            // 
            this.btnDeletePath.Location = new System.Drawing.Point(803, 674);
            this.btnDeletePath.Name = "btnDeletePath";
            this.btnDeletePath.Size = new System.Drawing.Size(125, 23);
            this.btnDeletePath.TabIndex = 60;
            this.btnDeletePath.Text = "Delete Path";
            this.btnDeletePath.UseVisualStyleBackColor = true;
            this.btnDeletePath.Click += new System.EventHandler(this.btnDeletePath_Click);
            // 
            // btnEditPath
            // 
            this.btnEditPath.Location = new System.Drawing.Point(672, 674);
            this.btnEditPath.Name = "btnEditPath";
            this.btnEditPath.Size = new System.Drawing.Size(125, 23);
            this.btnEditPath.TabIndex = 59;
            this.btnEditPath.Text = "Edit Path";
            this.btnEditPath.UseVisualStyleBackColor = true;
            this.btnEditPath.Click += new System.EventHandler(this.btnEditPath_Click);
            // 
            // btnNewPath
            // 
            this.btnNewPath.Location = new System.Drawing.Point(541, 674);
            this.btnNewPath.Name = "btnNewPath";
            this.btnNewPath.Size = new System.Drawing.Size(125, 23);
            this.btnNewPath.TabIndex = 58;
            this.btnNewPath.Text = "New Path";
            this.btnNewPath.UseVisualStyleBackColor = true;
            this.btnNewPath.Click += new System.EventHandler(this.btnNewPath_Click);
            // 
            // txtUploadUrl
            // 
            this.txtUploadUrl.Location = new System.Drawing.Point(12, 128);
            this.txtUploadUrl.Name = "txtUploadUrl";
            this.txtUploadUrl.Size = new System.Drawing.Size(523, 20);
            this.txtUploadUrl.TabIndex = 61;
            // 
            // btnFetchFolder
            // 
            this.btnFetchFolder.Location = new System.Drawing.Point(274, 67);
            this.btnFetchFolder.Name = "btnFetchFolder";
            this.btnFetchFolder.Size = new System.Drawing.Size(261, 23);
            this.btnFetchFolder.TabIndex = 62;
            this.btnFetchFolder.Text = "Fetch from Folder";
            this.btnFetchFolder.UseVisualStyleBackColor = true;
            this.btnFetchFolder.Click += new System.EventHandler(this.btnFetchFolder_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 63;
            this.label1.Text = "URL Prefix";
            // 
            // FormFetch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(940, 709);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnFetchFolder);
            this.Controls.Add(this.txtUploadUrl);
            this.Controls.Add(this.btnDeletePath);
            this.Controls.Add(this.btnEditPath);
            this.Controls.Add(this.btnNewPath);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnFetchFromFile);
            this.Controls.Add(this.btnFetchUrl);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.tvwItems);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormFetch";
            this.Text = "Fetch Files";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView tvwItems;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Button btnFetchUrl;
        private System.Windows.Forms.Button btnFetchFromFile;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDeletePath;
        private System.Windows.Forms.Button btnEditPath;
        private System.Windows.Forms.Button btnNewPath;
        private System.Windows.Forms.TextBox txtUploadUrl;
        private System.Windows.Forms.Button btnFetchFolder;
        private System.Windows.Forms.Label label1;
    }
}