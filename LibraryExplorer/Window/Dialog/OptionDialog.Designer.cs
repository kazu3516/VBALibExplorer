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
            this.editorPathTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.editorArgumentsTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(310, 6);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(391, 6);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Size = new System.Drawing.Size(478, 328);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(0, 328);
            this.panel1.Size = new System.Drawing.Size(478, 35);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(470, 302);
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
            this.groupBox1.Size = new System.Drawing.Size(454, 149);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "エディタ";
            // 
            // editorPathReferenceButton
            // 
            this.editorPathReferenceButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.editorPathReferenceButton.Location = new System.Drawing.Point(421, 35);
            this.editorPathReferenceButton.Name = "editorPathReferenceButton";
            this.editorPathReferenceButton.Size = new System.Drawing.Size(27, 23);
            this.editorPathReferenceButton.TabIndex = 2;
            this.editorPathReferenceButton.Text = "...";
            this.editorPathReferenceButton.UseVisualStyleBackColor = true;
            this.editorPathReferenceButton.Click += new System.EventHandler(this.editorPathReferenceButton_Click);
            // 
            // editorPathTextBox
            // 
            this.editorPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.editorPathTextBox.Location = new System.Drawing.Point(58, 39);
            this.editorPathTextBox.Name = "editorPathTextBox";
            this.editorPathTextBox.Size = new System.Drawing.Size(357, 19);
            this.editorPathTextBox.TabIndex = 1;
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
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "実行ファイル(*.exe)|*.exe|バッチファイル(*.bat;*.ps1;*.cmd;*.vbs;*.js;*.wsh)|*.bat;*.ps1;*.cmd" +
    ";*.vbs;*.js;*.wsh|全てのファイル(*.*)|*.*";
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "パス(&P)";
            // 
            // editorArgumentsTextBox
            // 
            this.editorArgumentsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.editorArgumentsTextBox.Location = new System.Drawing.Point(57, 64);
            this.editorArgumentsTextBox.Name = "editorArgumentsTextBox";
            this.editorArgumentsTextBox.Size = new System.Drawing.Size(357, 19);
            this.editorArgumentsTextBox.TabIndex = 1;
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
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(347, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = " 　引数に%filename%が含まれない場合、自動的に末尾に追加されます。";
            // 
            // OptionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 363);
            this.Name = "OptionDialog";
            this.Text = "OptionDialog";
            this.tabControl1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button editorPathReferenceButton;
        private System.Windows.Forms.TextBox editorPathTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox editorArgumentsTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
    }
}