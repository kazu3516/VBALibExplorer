namespace LibraryExplorer.Window.Dialog {
    partial class OptionDialog {
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
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.editorPathReferenceButton = new System.Windows.Forms.Button();
            this.editorArgumentsTextBox = new System.Windows.Forms.TextBox();
            this.editorPathTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.edhitorPathOpenFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.diffToolPathReferenceButton = new System.Windows.Forms.Button();
            this.diffToolArgumentsTextBox = new System.Windows.Forms.TextBox();
            this.diffToolPathTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.diffToolPathOpenFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(438, 300);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "エディタ";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.editorPathReferenceButton);
            this.groupBox1.Controls.Add(this.editorArgumentsTextBox);
            this.groupBox1.Controls.Add(this.editorPathTextBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(8, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(422, 149);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "エディタ";
            // 
            // editorPathReferenceButton
            // 
            this.editorPathReferenceButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.editorPathReferenceButton.Location = new System.Drawing.Point(389, 35);
            this.editorPathReferenceButton.Name = "editorPathReferenceButton";
            this.editorPathReferenceButton.Size = new System.Drawing.Size(27, 23);
            this.editorPathReferenceButton.TabIndex = 2;
            this.editorPathReferenceButton.Text = "...";
            this.editorPathReferenceButton.UseVisualStyleBackColor = true;
            this.editorPathReferenceButton.Click += new System.EventHandler(this.editorPathReferenceButton_Click);
            // 
            // editorArgumentsTextBox
            // 
            this.editorArgumentsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.editorArgumentsTextBox.Location = new System.Drawing.Point(57, 64);
            this.editorArgumentsTextBox.Name = "editorArgumentsTextBox";
            this.editorArgumentsTextBox.Size = new System.Drawing.Size(325, 19);
            this.editorArgumentsTextBox.TabIndex = 1;
            // 
            // editorPathTextBox
            // 
            this.editorPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.editorPathTextBox.Location = new System.Drawing.Point(58, 39);
            this.editorPathTextBox.Name = "editorPathTextBox";
            this.editorPathTextBox.Size = new System.Drawing.Size(325, 19);
            this.editorPathTextBox.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(347, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = " 　引数に%filename%が含まれない場合、自動的に末尾に追加されます。";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(303, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "※引数に%filename%が含まれる場合、ファイル名に置換します。";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "引数(&A)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "パス(&P)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(294, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "エディタ設定　※パスを指定しない場合、メモ帳を使用します。";
            // 
            // edhitorPathOpenFileDialog1
            // 
            this.edhitorPathOpenFileDialog1.FileName = "openFileDialog1";
            this.edhitorPathOpenFileDialog1.Filter = "実行ファイル(*.exe)|*.exe|バッチファイル(*.bat;*.ps1;*.cmd;*.vbs;*.js;*.wsh)|*.bat;*.ps1;*.cmd" +
    ";*.vbs;*.js;*.wsh|全てのファイル(*.*)|*.*";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(438, 300);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "ファイル比較";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.diffToolPathReferenceButton);
            this.groupBox2.Controls.Add(this.diffToolArgumentsTextBox);
            this.groupBox2.Controls.Add(this.diffToolPathTextBox);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Location = new System.Drawing.Point(8, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(422, 149);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "外部ツール";
            // 
            // diffToolPathReferenceButton
            // 
            this.diffToolPathReferenceButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.diffToolPathReferenceButton.Location = new System.Drawing.Point(389, 35);
            this.diffToolPathReferenceButton.Name = "diffToolPathReferenceButton";
            this.diffToolPathReferenceButton.Size = new System.Drawing.Size(27, 23);
            this.diffToolPathReferenceButton.TabIndex = 2;
            this.diffToolPathReferenceButton.Text = "...";
            this.diffToolPathReferenceButton.UseVisualStyleBackColor = true;
            this.diffToolPathReferenceButton.Click += new System.EventHandler(this.diffToolPathReferenceButton_Click);
            // 
            // diffToolArgumentsTextBox
            // 
            this.diffToolArgumentsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.diffToolArgumentsTextBox.Location = new System.Drawing.Point(57, 64);
            this.diffToolArgumentsTextBox.Name = "diffToolArgumentsTextBox";
            this.diffToolArgumentsTextBox.Size = new System.Drawing.Size(325, 19);
            this.diffToolArgumentsTextBox.TabIndex = 1;
            // 
            // diffToolPathTextBox
            // 
            this.diffToolPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.diffToolPathTextBox.Location = new System.Drawing.Point(58, 39);
            this.diffToolPathTextBox.Name = "diffToolPathTextBox";
            this.diffToolPathTextBox.Size = new System.Drawing.Size(325, 19);
            this.diffToolPathTextBox.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 112);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(360, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = " 　引数に%foldername%が含まれない場合、自動的に末尾に追加されます。";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 96);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(404, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "※引数に%foldername1%、%foldername2%が含まれる場合、ファイル名に置換します。";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 67);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 12);
            this.label8.TabIndex = 0;
            this.label8.Text = "引数(&A)";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 42);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(39, 12);
            this.label9.TabIndex = 0;
            this.label9.Text = "パス(&P)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 15);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(366, 12);
            this.label10.TabIndex = 0;
            this.label10.Text = "比較ツール設定　※パスを指定しない場合、標準の比較機能を使用します。";
            // 
            // diffToolPathOpenFileDialog1
            // 
            this.diffToolPathOpenFileDialog1.FileName = "openFileDialog2";
            this.diffToolPathOpenFileDialog1.Filter = "実行ファイル(*.exe)|*.exe|バッチファイル(*.bat;*.ps1;*.cmd;*.vbs;*.js;*.wsh)|*.bat;*.ps1;*.cmd" +
    ";*.vbs;*.js;*.wsh|全てのファイル(*.*)|*.*";
            // 
            // OptionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 361);
            this.Name = "OptionDialog";
            this.Text = "OptionDialog";
            this.tabControl1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button editorPathReferenceButton;
        private System.Windows.Forms.TextBox editorPathTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog edhitorPathOpenFileDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox editorArgumentsTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button diffToolPathReferenceButton;
        private System.Windows.Forms.TextBox diffToolArgumentsTextBox;
        private System.Windows.Forms.TextBox diffToolPathTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.OpenFileDialog diffToolPathOpenFileDialog1;
    }
}