namespace LibraryExplorer.Window.DockWindow {
    partial class FolderCompareWindow {
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
            this.topPanel1 = new System.Windows.Forms.Panel();
            this.destinationFolderPathTextBox1 = new System.Windows.Forms.TextBox();
            this.sourceFolderPathTextBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.mainPanel1 = new System.Windows.Forms.Panel();
            this.mainRightPanel1 = new System.Windows.Forms.Panel();
            this.mainRightTopPanel1 = new System.Windows.Forms.Panel();
            this.destinationFileRichTextBox1 = new System.Windows.Forms.RichTextBox();
            this.splitter3 = new System.Windows.Forms.Splitter();
            this.sourceFileRichTextBox1 = new System.Windows.Forms.RichTextBox();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.diffResultRichTextBox1 = new System.Windows.Forms.RichTextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.mainLeftPanel1 = new System.Windows.Forms.Panel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.topPanel1.SuspendLayout();
            this.mainPanel1.SuspendLayout();
            this.mainRightPanel1.SuspendLayout();
            this.mainRightTopPanel1.SuspendLayout();
            this.mainLeftPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel1
            // 
            this.topPanel1.Controls.Add(this.destinationFolderPathTextBox1);
            this.topPanel1.Controls.Add(this.sourceFolderPathTextBox1);
            this.topPanel1.Controls.Add(this.label2);
            this.topPanel1.Controls.Add(this.label1);
            this.topPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel1.Location = new System.Drawing.Point(0, 0);
            this.topPanel1.Name = "topPanel1";
            this.topPanel1.Size = new System.Drawing.Size(990, 71);
            this.topPanel1.TabIndex = 0;
            // 
            // destinationFolderPathTextBox1
            // 
            this.destinationFolderPathTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.destinationFolderPathTextBox1.Location = new System.Drawing.Point(70, 31);
            this.destinationFolderPathTextBox1.Name = "destinationFolderPathTextBox1";
            this.destinationFolderPathTextBox1.ReadOnly = true;
            this.destinationFolderPathTextBox1.Size = new System.Drawing.Size(908, 19);
            this.destinationFolderPathTextBox1.TabIndex = 1;
            // 
            // sourceFolderPathTextBox1
            // 
            this.sourceFolderPathTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sourceFolderPathTextBox1.Location = new System.Drawing.Point(70, 6);
            this.sourceFolderPathTextBox1.Name = "sourceFolderPathTextBox1";
            this.sourceFolderPathTextBox1.ReadOnly = true;
            this.sourceFolderPathTextBox1.Size = new System.Drawing.Size(908, 19);
            this.sourceFolderPathTextBox1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "フォルダ2：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "フォルダ1：";
            // 
            // mainPanel1
            // 
            this.mainPanel1.Controls.Add(this.mainRightPanel1);
            this.mainPanel1.Controls.Add(this.splitter1);
            this.mainPanel1.Controls.Add(this.mainLeftPanel1);
            this.mainPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel1.Location = new System.Drawing.Point(0, 71);
            this.mainPanel1.Name = "mainPanel1";
            this.mainPanel1.Size = new System.Drawing.Size(990, 507);
            this.mainPanel1.TabIndex = 0;
            // 
            // mainRightPanel1
            // 
            this.mainRightPanel1.Controls.Add(this.mainRightTopPanel1);
            this.mainRightPanel1.Controls.Add(this.splitter2);
            this.mainRightPanel1.Controls.Add(this.diffResultRichTextBox1);
            this.mainRightPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainRightPanel1.Location = new System.Drawing.Point(222, 0);
            this.mainRightPanel1.Name = "mainRightPanel1";
            this.mainRightPanel1.Size = new System.Drawing.Size(768, 507);
            this.mainRightPanel1.TabIndex = 2;
            // 
            // mainRightTopPanel1
            // 
            this.mainRightTopPanel1.Controls.Add(this.destinationFileRichTextBox1);
            this.mainRightTopPanel1.Controls.Add(this.splitter3);
            this.mainRightTopPanel1.Controls.Add(this.sourceFileRichTextBox1);
            this.mainRightTopPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainRightTopPanel1.Location = new System.Drawing.Point(0, 0);
            this.mainRightTopPanel1.Name = "mainRightTopPanel1";
            this.mainRightTopPanel1.Size = new System.Drawing.Size(768, 335);
            this.mainRightTopPanel1.TabIndex = 2;
            // 
            // destinationFileRichTextBox1
            // 
            this.destinationFileRichTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.destinationFileRichTextBox1.Location = new System.Drawing.Point(362, 0);
            this.destinationFileRichTextBox1.Name = "destinationFileRichTextBox1";
            this.destinationFileRichTextBox1.ReadOnly = true;
            this.destinationFileRichTextBox1.Size = new System.Drawing.Size(406, 335);
            this.destinationFileRichTextBox1.TabIndex = 2;
            this.destinationFileRichTextBox1.Text = "";
            // 
            // splitter3
            // 
            this.splitter3.Location = new System.Drawing.Point(359, 0);
            this.splitter3.Name = "splitter3";
            this.splitter3.Size = new System.Drawing.Size(3, 335);
            this.splitter3.TabIndex = 1;
            this.splitter3.TabStop = false;
            // 
            // sourceFileRichTextBox1
            // 
            this.sourceFileRichTextBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.sourceFileRichTextBox1.Location = new System.Drawing.Point(0, 0);
            this.sourceFileRichTextBox1.Name = "sourceFileRichTextBox1";
            this.sourceFileRichTextBox1.ReadOnly = true;
            this.sourceFileRichTextBox1.Size = new System.Drawing.Size(359, 335);
            this.sourceFileRichTextBox1.TabIndex = 0;
            this.sourceFileRichTextBox1.Text = "";
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 335);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(768, 3);
            this.splitter2.TabIndex = 1;
            this.splitter2.TabStop = false;
            // 
            // diffResultRichTextBox1
            // 
            this.diffResultRichTextBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.diffResultRichTextBox1.Location = new System.Drawing.Point(0, 338);
            this.diffResultRichTextBox1.Name = "diffResultRichTextBox1";
            this.diffResultRichTextBox1.ReadOnly = true;
            this.diffResultRichTextBox1.Size = new System.Drawing.Size(768, 169);
            this.diffResultRichTextBox1.TabIndex = 0;
            this.diffResultRichTextBox1.Text = "";
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(219, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 507);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // mainLeftPanel1
            // 
            this.mainLeftPanel1.Controls.Add(this.listView1);
            this.mainLeftPanel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.mainLeftPanel1.Location = new System.Drawing.Point(0, 0);
            this.mainLeftPanel1.Name = "mainLeftPanel1";
            this.mainLeftPanel1.Size = new System.Drawing.Size(219, 507);
            this.mainLeftPanel1.TabIndex = 0;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(219, 507);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ファイル名";
            this.columnHeader1.Width = 120;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "比較結果";
            // 
            // FolderCompareWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(990, 578);
            this.Controls.Add(this.mainPanel1);
            this.Controls.Add(this.topPanel1);
            this.Name = "FolderCompareWindow";
            this.Text = "フォルダの比較";
            this.topPanel1.ResumeLayout(false);
            this.topPanel1.PerformLayout();
            this.mainPanel1.ResumeLayout(false);
            this.mainRightPanel1.ResumeLayout(false);
            this.mainRightTopPanel1.ResumeLayout(false);
            this.mainLeftPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel topPanel1;
        private System.Windows.Forms.TextBox destinationFolderPathTextBox1;
        private System.Windows.Forms.TextBox sourceFolderPathTextBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel mainPanel1;
        private System.Windows.Forms.Panel mainRightPanel1;
        private System.Windows.Forms.RichTextBox diffResultRichTextBox1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel mainLeftPanel1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Panel mainRightTopPanel1;
        private System.Windows.Forms.RichTextBox destinationFileRichTextBox1;
        private System.Windows.Forms.Splitter splitter3;
        private System.Windows.Forms.RichTextBox sourceFileRichTextBox1;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}