namespace LibraryExplorer.Window.Dialog {
    partial class LibraryPropertyDialog {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LibraryPropertyDialog));
            this.closeButton = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.pathTextBox1 = new System.Windows.Forms.TextBox();
            this.nameTextBox1 = new System.Windows.Forms.TextBox();
            this.pathLabel1 = new System.Windows.Forms.Label();
            this.nameLabel1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.historyGroupBox1 = new System.Windows.Forms.GroupBox();
            this.deleteHistoryButton1 = new System.Windows.Forms.Button();
            this.openHistoryFolderButton1 = new System.Windows.Forms.Button();
            this.historyListView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.exportDateTextBox1 = new System.Windows.Forms.TextBox();
            this.exportDateLabel1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.フォルダを開くOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.削除DToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.すべて選択AToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.historyGroupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(323, 12);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = "閉じる";
            this.closeButton.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(410, 415);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Transparent;
            this.tabPage1.Controls.Add(this.pathTextBox1);
            this.tabPage1.Controls.Add(this.nameTextBox1);
            this.tabPage1.Controls.Add(this.pathLabel1);
            this.tabPage1.Controls.Add(this.nameLabel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(402, 389);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "一般";
            // 
            // pathTextBox1
            // 
            this.pathTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pathTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.pathTextBox1.Location = new System.Drawing.Point(87, 56);
            this.pathTextBox1.Name = "pathTextBox1";
            this.pathTextBox1.ReadOnly = true;
            this.pathTextBox1.Size = new System.Drawing.Size(307, 12);
            this.pathTextBox1.TabIndex = 1;
            this.pathTextBox1.Text = "***";
            // 
            // nameTextBox1
            // 
            this.nameTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nameTextBox1.Location = new System.Drawing.Point(87, 26);
            this.nameTextBox1.Name = "nameTextBox1";
            this.nameTextBox1.ReadOnly = true;
            this.nameTextBox1.Size = new System.Drawing.Size(309, 12);
            this.nameTextBox1.TabIndex = 1;
            this.nameTextBox1.Text = "***";
            // 
            // pathLabel1
            // 
            this.pathLabel1.AutoSize = true;
            this.pathLabel1.Location = new System.Drawing.Point(8, 56);
            this.pathLabel1.Name = "pathLabel1";
            this.pathLabel1.Size = new System.Drawing.Size(42, 12);
            this.pathLabel1.TabIndex = 0;
            this.pathLabel1.Text = "フルパス";
            // 
            // nameLabel1
            // 
            this.nameLabel1.AutoSize = true;
            this.nameLabel1.Location = new System.Drawing.Point(8, 26);
            this.nameLabel1.Name = "nameLabel1";
            this.nameLabel1.Size = new System.Drawing.Size(52, 12);
            this.nameLabel1.TabIndex = 0;
            this.nameLabel1.Text = "フォルダ名";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.historyGroupBox1);
            this.tabPage2.Controls.Add(this.exportDateTextBox1);
            this.tabPage2.Controls.Add(this.exportDateLabel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(402, 389);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "インポート/エクスポート";
            // 
            // historyGroupBox1
            // 
            this.historyGroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.historyGroupBox1.Controls.Add(this.deleteHistoryButton1);
            this.historyGroupBox1.Controls.Add(this.openHistoryFolderButton1);
            this.historyGroupBox1.Controls.Add(this.historyListView1);
            this.historyGroupBox1.Location = new System.Drawing.Point(10, 68);
            this.historyGroupBox1.Name = "historyGroupBox1";
            this.historyGroupBox1.Size = new System.Drawing.Size(386, 315);
            this.historyGroupBox1.TabIndex = 4;
            this.historyGroupBox1.TabStop = false;
            this.historyGroupBox1.Text = "履歴";
            // 
            // deleteHistoryButton1
            // 
            this.deleteHistoryButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteHistoryButton1.Location = new System.Drawing.Point(305, 18);
            this.deleteHistoryButton1.Name = "deleteHistoryButton1";
            this.deleteHistoryButton1.Size = new System.Drawing.Size(75, 23);
            this.deleteHistoryButton1.TabIndex = 1;
            this.deleteHistoryButton1.Text = "履歴の削除";
            this.deleteHistoryButton1.UseVisualStyleBackColor = true;
            this.deleteHistoryButton1.Click += new System.EventHandler(this.deleteHistoryButton1_Click);
            // 
            // openHistoryFolderButton1
            // 
            this.openHistoryFolderButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.openHistoryFolderButton1.Location = new System.Drawing.Point(224, 18);
            this.openHistoryFolderButton1.Name = "openHistoryFolderButton1";
            this.openHistoryFolderButton1.Size = new System.Drawing.Size(75, 23);
            this.openHistoryFolderButton1.TabIndex = 1;
            this.openHistoryFolderButton1.Text = "フォルダを開く";
            this.openHistoryFolderButton1.UseVisualStyleBackColor = true;
            this.openHistoryFolderButton1.Click += new System.EventHandler(this.openHistoryFolderButton1_Click);
            // 
            // historyListView1
            // 
            this.historyListView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.historyListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.historyListView1.ContextMenuStrip = this.contextMenuStrip1;
            this.historyListView1.FullRowSelect = true;
            this.historyListView1.GridLines = true;
            this.historyListView1.Location = new System.Drawing.Point(6, 47);
            this.historyListView1.Name = "historyListView1";
            this.historyListView1.Size = new System.Drawing.Size(374, 252);
            this.historyListView1.TabIndex = 0;
            this.historyListView1.UseCompatibleStateImageBehavior = false;
            this.historyListView1.View = System.Windows.Forms.View.Details;
            this.historyListView1.SelectedIndexChanged += new System.EventHandler(this.historyListView1_SelectedIndexChanged);
            this.historyListView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.historyListView1_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "フォルダ名";
            this.columnHeader1.Width = 200;
            // 
            // exportDateTextBox1
            // 
            this.exportDateTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.exportDateTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.exportDateTextBox1.Location = new System.Drawing.Point(97, 26);
            this.exportDateTextBox1.Name = "exportDateTextBox1";
            this.exportDateTextBox1.ReadOnly = true;
            this.exportDateTextBox1.Size = new System.Drawing.Size(299, 12);
            this.exportDateTextBox1.TabIndex = 3;
            this.exportDateTextBox1.Text = "***";
            // 
            // exportDateLabel1
            // 
            this.exportDateLabel1.AutoSize = true;
            this.exportDateLabel1.Location = new System.Drawing.Point(8, 26);
            this.exportDateLabel1.Name = "exportDateLabel1";
            this.exportDateLabel1.Size = new System.Drawing.Size(83, 12);
            this.exportDateLabel1.TabIndex = 2;
            this.exportDateLabel1.Text = "エクスポート日付";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.closeButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 415);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(410, 47);
            this.panel1.TabIndex = 2;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.フォルダを開くOToolStripMenuItem,
            this.toolStripMenuItem1,
            this.削除DToolStripMenuItem,
            this.toolStripMenuItem2,
            this.すべて選択AToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(192, 82);
            this.contextMenuStrip1.Opened += new System.EventHandler(this.contextMenuStrip1_Opened);
            // 
            // フォルダを開くOToolStripMenuItem
            // 
            this.フォルダを開くOToolStripMenuItem.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Bold);
            this.フォルダを開くOToolStripMenuItem.Name = "フォルダを開くOToolStripMenuItem";
            this.フォルダを開くOToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.フォルダを開くOToolStripMenuItem.Text = "フォルダを開く(&O)";
            this.フォルダを開くOToolStripMenuItem.Click += new System.EventHandler(this.フォルダを開くOToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(188, 6);
            // 
            // 削除DToolStripMenuItem
            // 
            this.削除DToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("削除DToolStripMenuItem.Image")));
            this.削除DToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.削除DToolStripMenuItem.Name = "削除DToolStripMenuItem";
            this.削除DToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.削除DToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.削除DToolStripMenuItem.Text = "削除(&D)";
            this.削除DToolStripMenuItem.Click += new System.EventHandler(this.削除DToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(188, 6);
            // 
            // すべて選択AToolStripMenuItem
            // 
            this.すべて選択AToolStripMenuItem.Name = "すべて選択AToolStripMenuItem";
            this.すべて選択AToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.すべて選択AToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.すべて選択AToolStripMenuItem.Text = "すべて選択(&A)";
            this.すべて選択AToolStripMenuItem.Click += new System.EventHandler(this.すべて選択AToolStripMenuItem_Click);
            // 
            // LibraryPropertyDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(410, 462);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LibraryPropertyDialog";
            this.ShowInTaskbar = false;
            this.Text = "プロパティ";
            this.VisibleChanged += new System.EventHandler(this.LibraryPropertyDialog_VisibleChanged);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.historyGroupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox nameTextBox1;
        private System.Windows.Forms.Label pathLabel1;
        private System.Windows.Forms.Label nameLabel1;
        private System.Windows.Forms.TextBox pathTextBox1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox exportDateTextBox1;
        private System.Windows.Forms.Label exportDateLabel1;
        private System.Windows.Forms.GroupBox historyGroupBox1;
        private System.Windows.Forms.Button openHistoryFolderButton1;
        private System.Windows.Forms.ListView historyListView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button deleteHistoryButton1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem フォルダを開くOToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 削除DToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem すべて選択AToolStripMenuItem;
    }
}