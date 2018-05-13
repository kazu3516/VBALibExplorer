namespace LibraryExplorer.Control.Wizard {
    partial class FolderCompareWizard {
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
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.targetLibraryNameListView1 = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.targetFileNameTextBox1 = new System.Windows.Forms.TextBox();
            this.errorLabel2 = new System.Windows.Forms.Label();
            this.errorLabel1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.researchButton1 = new System.Windows.Forms.Button();
            this.targetLibraryModuleListView1 = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.選択SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.ファイルを開くOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.targetFileModuleListView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ファイルを開くOToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label15 = new System.Windows.Forms.Label();
            this.targetFileTempFolderTextBox1 = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.targetLibraryTempFolderTextBox1 = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.RightToLeftLayout = true;
            this.tabControl1.Size = new System.Drawing.Size(954, 477);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(946, 451);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.targetLibraryNameListView1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.targetFileNameTextBox1);
            this.panel1.Controls.Add(this.errorLabel2);
            this.panel1.Controls.Add(this.errorLabel1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(946, 451);
            this.panel1.TabIndex = 5;
            // 
            // targetLibraryNameListView1
            // 
            this.targetLibraryNameListView1.CheckBoxes = true;
            this.targetLibraryNameListView1.Location = new System.Drawing.Point(30, 151);
            this.targetLibraryNameListView1.Name = "targetLibraryNameListView1";
            this.targetLibraryNameListView1.Size = new System.Drawing.Size(181, 97);
            this.targetLibraryNameListView1.TabIndex = 5;
            this.targetLibraryNameListView1.UseCompatibleStateImageBehavior = false;
            this.targetLibraryNameListView1.View = System.Windows.Forms.View.List;
            this.targetLibraryNameListView1.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.targetLibraryNameListView1_ItemChecked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(424, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "モジュールのバージョン確認のため、エクスポートされたモジュールフォルダの比較を行います。";
            // 
            // targetFileNameTextBox1
            // 
            this.targetFileNameTextBox1.BackColor = System.Drawing.SystemColors.Control;
            this.targetFileNameTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.targetFileNameTextBox1.Location = new System.Drawing.Point(115, 93);
            this.targetFileNameTextBox1.Name = "targetFileNameTextBox1";
            this.targetFileNameTextBox1.ReadOnly = true;
            this.targetFileNameTextBox1.Size = new System.Drawing.Size(427, 12);
            this.targetFileNameTextBox1.TabIndex = 4;
            this.targetFileNameTextBox1.Text = "***";
            // 
            // errorLabel2
            // 
            this.errorLabel2.AutoSize = true;
            this.errorLabel2.ForeColor = System.Drawing.Color.Red;
            this.errorLabel2.Location = new System.Drawing.Point(28, 344);
            this.errorLabel2.Name = "errorLabel2";
            this.errorLabel2.Size = new System.Drawing.Size(201, 12);
            this.errorLabel2.TabIndex = 3;
            this.errorLabel2.Text = "対象ライブラリを1つ以上選択してください。";
            // 
            // errorLabel1
            // 
            this.errorLabel1.AutoSize = true;
            this.errorLabel1.ForeColor = System.Drawing.Color.Red;
            this.errorLabel1.Location = new System.Drawing.Point(28, 321);
            this.errorLabel1.Name = "errorLabel1";
            this.errorLabel1.Size = new System.Drawing.Size(386, 12);
            this.errorLabel1.TabIndex = 3;
            this.errorLabel1.Text = "対象ファイルが選択されていません。キャンセルし、対象ファイルを確認してください。";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 281);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(274, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "次へを押すと、対応するライブラリモジュールを検索します。";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(28, 136);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(76, 12);
            this.label13.TabIndex = 3;
            this.label13.Text = "対象ライブラリ：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(28, 93);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 12);
            this.label5.TabIndex = 3;
            this.label5.Text = "対象ファイル：";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(426, 206);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.researchButton1);
            this.panel2.Controls.Add(this.targetLibraryModuleListView1);
            this.panel2.Controls.Add(this.targetFileModuleListView1);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(426, 206);
            this.panel2.TabIndex = 0;
            // 
            // researchButton1
            // 
            this.researchButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.researchButton1.Location = new System.Drawing.Point(824, 88);
            this.researchButton1.Name = "researchButton1";
            this.researchButton1.Size = new System.Drawing.Size(75, 23);
            this.researchButton1.TabIndex = 17;
            this.researchButton1.Text = "再検索";
            this.researchButton1.UseVisualStyleBackColor = true;
            this.researchButton1.Click += new System.EventHandler(this.researchButton1_Click);
            // 
            // targetLibraryModuleListView1
            // 
            this.targetLibraryModuleListView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.targetLibraryModuleListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader6,
            this.columnHeader5});
            this.targetLibraryModuleListView1.ContextMenuStrip = this.contextMenuStrip1;
            this.targetLibraryModuleListView1.FullRowSelect = true;
            this.targetLibraryModuleListView1.GridLines = true;
            this.targetLibraryModuleListView1.HideSelection = false;
            this.targetLibraryModuleListView1.Location = new System.Drawing.Point(331, 73);
            this.targetLibraryModuleListView1.MultiSelect = false;
            this.targetLibraryModuleListView1.Name = "targetLibraryModuleListView1";
            this.targetLibraryModuleListView1.Size = new System.Drawing.Size(48, 3);
            this.targetLibraryModuleListView1.TabIndex = 15;
            this.targetLibraryModuleListView1.UseCompatibleStateImageBehavior = false;
            this.targetLibraryModuleListView1.View = System.Windows.Forms.View.Details;
            this.targetLibraryModuleListView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.targetLibraryModuleListView1_MouseDoubleClick);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Name";
            this.columnHeader3.Width = 138;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Revision";
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Library";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Path";
            this.columnHeader5.Width = 200;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.選択SToolStripMenuItem,
            this.toolStripMenuItem1,
            this.ファイルを開くOToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(155, 54);
            this.contextMenuStrip1.Opened += new System.EventHandler(this.contextMenuStrip1_Opened);
            // 
            // 選択SToolStripMenuItem
            // 
            this.選択SToolStripMenuItem.Name = "選択SToolStripMenuItem";
            this.選択SToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.選択SToolStripMenuItem.Text = "選択(&S)";
            this.選択SToolStripMenuItem.Click += new System.EventHandler(this.選択SToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(151, 6);
            // 
            // ファイルを開くOToolStripMenuItem
            // 
            this.ファイルを開くOToolStripMenuItem.Name = "ファイルを開くOToolStripMenuItem";
            this.ファイルを開くOToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.ファイルを開くOToolStripMenuItem.Text = "ファイルを開く(&O)";
            this.ファイルを開くOToolStripMenuItem.Click += new System.EventHandler(this.ファイルを開くOToolStripMenuItem_Click);
            // 
            // targetFileModuleListView1
            // 
            this.targetFileModuleListView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.targetFileModuleListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.targetFileModuleListView1.ContextMenuStrip = this.contextMenuStrip2;
            this.targetFileModuleListView1.FullRowSelect = true;
            this.targetFileModuleListView1.GridLines = true;
            this.targetFileModuleListView1.HideSelection = false;
            this.targetFileModuleListView1.Location = new System.Drawing.Point(30, 73);
            this.targetFileModuleListView1.MultiSelect = false;
            this.targetFileModuleListView1.Name = "targetFileModuleListView1";
            this.targetFileModuleListView1.Size = new System.Drawing.Size(249, 3);
            this.targetFileModuleListView1.TabIndex = 16;
            this.targetFileModuleListView1.UseCompatibleStateImageBehavior = false;
            this.targetFileModuleListView1.View = System.Windows.Forms.View.Details;
            this.targetFileModuleListView1.SelectedIndexChanged += new System.EventHandler(this.targetFileModuleListView1_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 138;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Revision";
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ファイルを開くOToolStripMenuItem1});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(155, 26);
            this.contextMenuStrip2.Opened += new System.EventHandler(this.contextMenuStrip2_Opened);
            // 
            // ファイルを開くOToolStripMenuItem1
            // 
            this.ファイルを開くOToolStripMenuItem1.Name = "ファイルを開くOToolStripMenuItem1";
            this.ファイルを開くOToolStripMenuItem1.Size = new System.Drawing.Size(154, 22);
            this.ファイルを開くOToolStripMenuItem1.Text = "ファイルを開く(&O)";
            this.ファイルを開くOToolStripMenuItem1.Click += new System.EventHandler(this.ファイルを開くOToolStripMenuItem1_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(329, 58);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 12);
            this.label7.TabIndex = 8;
            this.label7.Text = "ライブラリ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(28, 58);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "対象ファイル";
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(329, 88);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(409, 12);
            this.label11.TabIndex = 10;
            this.label11.Text = "[ダブルクリック]または[右クリックメニュー]-[選択]にて使用するファイルを選択してください。";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(28, 93);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(198, 12);
            this.label9.TabIndex = 10;
            this.label9.Text = "赤：該当のライブラリが複数見つかりました";
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(28, 132);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(119, 12);
            this.label10.TabIndex = 11;
            this.label10.Text = "黄：該当のライブラリ無し";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(28, 111);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(251, 12);
            this.label8.TabIndex = 12;
            this.label8.Text = "オレンジ：該当のライブラリが複数見つかり、解決済み";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 166);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 12);
            this.label3.TabIndex = 13;
            this.label3.Text = "次へを押してください。";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(28, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(205, 12);
            this.label4.TabIndex = 14;
            this.label4.Text = "対応するライブラリモジュールを検索します。";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.panel3);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(426, 206);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label12);
            this.panel3.Controls.Add(this.label14);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(420, 200);
            this.panel3.TabIndex = 16;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(28, 28);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(229, 12);
            this.label12.TabIndex = 15;
            this.label12.Text = "モジュールのバージョン確認の準備ができました。";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(28, 67);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(433, 12);
            this.label14.TabIndex = 15;
            this.label14.Text = "開始を押すと、対応するライブラリファイルを一時フォルダにコピーし、テキスト比較を行います。";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.panel4);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(426, 206);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label15);
            this.panel4.Controls.Add(this.targetFileTempFolderTextBox1);
            this.panel4.Controls.Add(this.label16);
            this.panel4.Controls.Add(this.targetLibraryTempFolderTextBox1);
            this.panel4.Controls.Add(this.label17);
            this.panel4.Controls.Add(this.label18);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(420, 200);
            this.panel4.TabIndex = 18;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(28, 28);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(276, 12);
            this.label15.TabIndex = 16;
            this.label15.Text = "下記のフォルダにエクスポートされたファイルを比較しました。";
            // 
            // targetFileTempFolderTextBox1
            // 
            this.targetFileTempFolderTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.targetFileTempFolderTextBox1.BackColor = System.Drawing.SystemColors.Control;
            this.targetFileTempFolderTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.targetFileTempFolderTextBox1.Location = new System.Drawing.Point(247, 112);
            this.targetFileTempFolderTextBox1.Name = "targetFileTempFolderTextBox1";
            this.targetFileTempFolderTextBox1.ReadOnly = true;
            this.targetFileTempFolderTextBox1.Size = new System.Drawing.Size(101, 12);
            this.targetFileTempFolderTextBox1.TabIndex = 17;
            this.targetFileTempFolderTextBox1.Text = "***";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(28, 60);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(361, 12);
            this.label16.TabIndex = 16;
            this.label16.Text = "完了を押してこのダイアログを閉じ、表示された比較結果を確認してください。";
            // 
            // targetLibraryTempFolderTextBox1
            // 
            this.targetLibraryTempFolderTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.targetLibraryTempFolderTextBox1.BackColor = System.Drawing.SystemColors.Control;
            this.targetLibraryTempFolderTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.targetLibraryTempFolderTextBox1.Location = new System.Drawing.Point(247, 156);
            this.targetLibraryTempFolderTextBox1.Name = "targetLibraryTempFolderTextBox1";
            this.targetLibraryTempFolderTextBox1.ReadOnly = true;
            this.targetLibraryTempFolderTextBox1.Size = new System.Drawing.Size(101, 12);
            this.targetLibraryTempFolderTextBox1.TabIndex = 17;
            this.targetLibraryTempFolderTextBox1.Text = "***";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(28, 112);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(145, 12);
            this.label17.TabIndex = 16;
            this.label17.Text = "対象ファイルのエクスポート先：";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(28, 156);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(211, 12);
            this.label18.TabIndex = 16;
            this.label18.Text = "対応するライブラリモジュールの一時フォルダ：";
            // 
            // FolderCompareWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.MinimumSize = new System.Drawing.Size(954, 516);
            this.Name = "FolderCompareWizard";
            this.Size = new System.Drawing.Size(954, 516);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox targetFileNameTextBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button researchButton1;
        private System.Windows.Forms.ListView targetLibraryModuleListView1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ListView targetFileModuleListView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 選択SToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ファイルを開くOToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem ファイルを開くOToolStripMenuItem1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ListView targetLibraryNameListView1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label errorLabel1;
        private System.Windows.Forms.Label errorLabel2;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox targetFileTempFolderTextBox1;
        private System.Windows.Forms.TextBox targetLibraryTempFolderTextBox1;
    }
}
