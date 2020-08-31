namespace LibraryExplorer.Window.DockWindow {
    partial class HistoryViewWindow {
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
            this.historyListView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.フォルダを開くOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ファイルの場所を開くFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.削除DToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.すべて選択AToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // historyListView1
            // 
            this.historyListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.historyListView1.ContextMenuStrip = this.contextMenuStrip1;
            this.historyListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.historyListView1.FullRowSelect = true;
            this.historyListView1.GridLines = true;
            this.historyListView1.HideSelection = false;
            this.historyListView1.Location = new System.Drawing.Point(0, 0);
            this.historyListView1.Name = "historyListView1";
            this.historyListView1.Size = new System.Drawing.Size(796, 291);
            this.historyListView1.TabIndex = 0;
            this.historyListView1.UseCompatibleStateImageBehavior = false;
            this.historyListView1.View = System.Windows.Forms.View.Details;
            this.historyListView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.historyListView1_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "フォルダ名";
            this.columnHeader1.Width = 350;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "関連するファイル名";
            this.columnHeader2.Width = 300;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.フォルダを開くOToolStripMenuItem,
            this.ファイルの場所を開くFToolStripMenuItem,
            this.toolStripMenuItem2,
            this.削除DToolStripMenuItem,
            this.toolStripMenuItem3,
            this.すべて選択AToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(192, 104);
            this.contextMenuStrip1.Opened += new System.EventHandler(this.contextMenuStrip1_Opened);
            // 
            // フォルダを開くOToolStripMenuItem
            // 
            this.フォルダを開くOToolStripMenuItem.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Bold);
            this.フォルダを開くOToolStripMenuItem.Name = "フォルダを開くOToolStripMenuItem";
            this.フォルダを開くOToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.フォルダを開くOToolStripMenuItem.Text = "フォルダを開く(&O)";
            this.フォルダを開くOToolStripMenuItem.Click += new System.EventHandler(this.フォルダを開くOToolStripMenuItem_Click);
            // 
            // ファイルの場所を開くFToolStripMenuItem
            // 
            this.ファイルの場所を開くFToolStripMenuItem.Name = "ファイルの場所を開くFToolStripMenuItem";
            this.ファイルの場所を開くFToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.ファイルの場所を開くFToolStripMenuItem.Text = "ファイルの場所を開く(&F)";
            this.ファイルの場所を開くFToolStripMenuItem.Click += new System.EventHandler(this.ファイルの場所を開くFToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(183, 6);
            // 
            // 削除DToolStripMenuItem
            // 
            this.削除DToolStripMenuItem.Name = "削除DToolStripMenuItem";
            this.削除DToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.削除DToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.削除DToolStripMenuItem.Text = "削除(&D)";
            this.削除DToolStripMenuItem.Click += new System.EventHandler(this.削除DToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(183, 6);
            // 
            // すべて選択AToolStripMenuItem
            // 
            this.すべて選択AToolStripMenuItem.Name = "すべて選択AToolStripMenuItem";
            this.すべて選択AToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.すべて選択AToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.すべて選択AToolStripMenuItem.Text = "すべて選択(&A)";
            this.すべて選択AToolStripMenuItem.Click += new System.EventHandler(this.すべて選択AToolStripMenuItem_Click);
            // 
            // HistoryViewWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 291);
            this.Controls.Add(this.historyListView1);
            this.HideOnClose = true;
            this.Name = "HistoryViewWindow";
            this.Text = "履歴ビューア";
            this.VisibleChanged += new System.EventHandler(this.HistoryViewWindow_VisibleChanged);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView historyListView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem フォルダを開くOToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ファイルの場所を開くFToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem 削除DToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem すべて選択AToolStripMenuItem;
    }
}