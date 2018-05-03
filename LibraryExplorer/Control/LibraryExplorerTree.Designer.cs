namespace LibraryExplorer.Control {
    partial class LibraryExplorerTree {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LibraryExplorerTree));
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.エクスプローラを開くOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ファイルの場所を開くFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.再エクスポートXToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.全て展開EToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.全て折りたたむCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.最新の情報に更新RToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.閉じるCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.プロパティPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.ContextMenuStrip = this.contextMenuStrip1;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.FullRowSelect = true;
            this.treeView1.HideSelection = false;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(263, 326);
            this.treeView1.TabIndex = 1;
            this.treeView1.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCollapse);
            this.treeView1.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterExpand);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.エクスプローラを開くOToolStripMenuItem,
            this.ファイルの場所を開くFToolStripMenuItem,
            this.再エクスポートXToolStripMenuItem,
            this.toolStripMenuItem4,
            this.閉じるCToolStripMenuItem,
            this.toolStripMenuItem1,
            this.全て展開EToolStripMenuItem,
            this.全て折りたたむCToolStripMenuItem,
            this.toolStripMenuItem2,
            this.最新の情報に更新RToolStripMenuItem,
            this.toolStripMenuItem3,
            this.プロパティPToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(187, 226);
            this.contextMenuStrip1.Opened += new System.EventHandler(this.contextMenuStrip1_Opened);
            // 
            // エクスプローラを開くOToolStripMenuItem
            // 
            this.エクスプローラを開くOToolStripMenuItem.Name = "エクスプローラを開くOToolStripMenuItem";
            this.エクスプローラを開くOToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.エクスプローラを開くOToolStripMenuItem.Text = "エクスプローラを開く(O)";
            this.エクスプローラを開くOToolStripMenuItem.Click += new System.EventHandler(this.エクスプローラを開くOToolStripMenuItem_Click);
            // 
            // ファイルの場所を開くFToolStripMenuItem
            // 
            this.ファイルの場所を開くFToolStripMenuItem.Name = "ファイルの場所を開くFToolStripMenuItem";
            this.ファイルの場所を開くFToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.ファイルの場所を開くFToolStripMenuItem.Text = "ファイルの場所を開く(&F)";
            this.ファイルの場所を開くFToolStripMenuItem.Click += new System.EventHandler(this.ファイルの場所を開くFToolStripMenuItem_Click);
            // 
            // 再エクスポートXToolStripMenuItem
            // 
            this.再エクスポートXToolStripMenuItem.Name = "再エクスポートXToolStripMenuItem";
            this.再エクスポートXToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.再エクスポートXToolStripMenuItem.Text = "再エクスポート(&X)";
            this.再エクスポートXToolStripMenuItem.Click += new System.EventHandler(this.再エクスポートXToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(183, 6);
            // 
            // 全て展開EToolStripMenuItem
            // 
            this.全て展開EToolStripMenuItem.Name = "全て展開EToolStripMenuItem";
            this.全て展開EToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.全て展開EToolStripMenuItem.Text = "全て展開(&E)";
            this.全て展開EToolStripMenuItem.Click += new System.EventHandler(this.全て展開EToolStripMenuItem_Click);
            // 
            // 全て折りたたむCToolStripMenuItem
            // 
            this.全て折りたたむCToolStripMenuItem.Name = "全て折りたたむCToolStripMenuItem";
            this.全て折りたたむCToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.全て折りたたむCToolStripMenuItem.Text = "全て折りたたむ(&C)";
            this.全て折りたたむCToolStripMenuItem.Click += new System.EventHandler(this.全て折りたたむCToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(183, 6);
            // 
            // 最新の情報に更新RToolStripMenuItem
            // 
            this.最新の情報に更新RToolStripMenuItem.Name = "最新の情報に更新RToolStripMenuItem";
            this.最新の情報に更新RToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.最新の情報に更新RToolStripMenuItem.Text = "最新の情報に更新(&R)";
            this.最新の情報に更新RToolStripMenuItem.Click += new System.EventHandler(this.最新の情報に更新RToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(183, 6);
            // 
            // 閉じるCToolStripMenuItem
            // 
            this.閉じるCToolStripMenuItem.Name = "閉じるCToolStripMenuItem";
            this.閉じるCToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.閉じるCToolStripMenuItem.Text = "閉じる(&C)";
            this.閉じるCToolStripMenuItem.Click += new System.EventHandler(this.閉じるCToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "folder-closed_16.png");
            this.imageList1.Images.SetKeyName(1, "folder-open_16.png");
            this.imageList1.Images.SetKeyName(2, "documents_16.png");
            this.imageList1.Images.SetKeyName(3, "Excel.png");
            this.imageList1.Images.SetKeyName(4, "Visio.png");
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.treeView1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(263, 326);
            this.panel1.TabIndex = 1;
            // 
            // プロパティPToolStripMenuItem
            // 
            this.プロパティPToolStripMenuItem.Name = "プロパティPToolStripMenuItem";
            this.プロパティPToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.プロパティPToolStripMenuItem.Text = "プロパティ(&P)";
            this.プロパティPToolStripMenuItem.Click += new System.EventHandler(this.プロパティPToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(183, 6);
            // 
            // LibraryExplorerTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "LibraryExplorerTree";
            this.Size = new System.Drawing.Size(263, 326);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem エクスプローラを開くOToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 全て展開EToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全て折りたたむCToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem 最新の情報に更新RToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem ファイルの場所を開くFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 再エクスポートXToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem 閉じるCToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem プロパティPToolStripMenuItem;
    }
}
