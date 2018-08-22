namespace SDSetupManifestGenerator {
    partial class FetchWindow {
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
            this.tvwItems = new System.Windows.Forms.TreeView();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.btnFetchUrl = new System.Windows.Forms.Button();
            this.btnFetchFromFile = new System.Windows.Forms.Button();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDeletePath = new System.Windows.Forms.Button();
            this.btnEditPath = new System.Windows.Forms.Button();
            this.btnNewPath = new System.Windows.Forms.Button();
            this.txtUploadUrl = new System.Windows.Forms.TextBox();
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
            this.btnFetchFromFile.Location = new System.Drawing.Point(12, 106);
            this.btnFetchFromFile.Name = "btnFetchFromFile";
            this.btnFetchFromFile.Size = new System.Drawing.Size(523, 23);
            this.btnFetchFromFile.TabIndex = 56;
            this.btnFetchFromFile.Text = "Fetch from File";
            this.btnFetchFromFile.UseVisualStyleBackColor = true;
            this.btnFetchFromFile.Click += new System.EventHandler(this.btnFetchFromFile_Click);
            // 
            // txtFile
            // 
            this.txtFile.Location = new System.Drawing.Point(12, 80);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(523, 20);
            this.txtFile.TabIndex = 55;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 674);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(523, 23);
            this.btnSave.TabIndex = 57;
            this.btnSave.Text = "Save";
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
            this.txtUploadUrl.Location = new System.Drawing.Point(12, 611);
            this.txtUploadUrl.Name = "txtUploadUrl";
            this.txtUploadUrl.Size = new System.Drawing.Size(523, 20);
            this.txtUploadUrl.TabIndex = 61;
            // 
            // FetchWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(940, 709);
            this.Controls.Add(this.txtUploadUrl);
            this.Controls.Add(this.btnDeletePath);
            this.Controls.Add(this.btnEditPath);
            this.Controls.Add(this.btnNewPath);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnFetchFromFile);
            this.Controls.Add(this.txtFile);
            this.Controls.Add(this.btnFetchUrl);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.tvwItems);
            this.Name = "FetchWindow";
            this.Text = "FetchWindow";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView tvwItems;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Button btnFetchUrl;
        private System.Windows.Forms.Button btnFetchFromFile;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDeletePath;
        private System.Windows.Forms.Button btnEditPath;
        private System.Windows.Forms.Button btnNewPath;
        private System.Windows.Forms.TextBox txtUploadUrl;
    }
}