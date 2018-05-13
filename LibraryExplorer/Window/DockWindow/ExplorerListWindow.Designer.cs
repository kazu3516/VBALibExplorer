namespace LibraryExplorer.Window.DockWindow {
    partial class ExplorerListWindow {
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
            this.libraryExplorerList1 = new LibraryExplorer.Control.LibraryExplorerList();
            this.SuspendLayout();
            // 
            // libraryExplorerList1
            // 
            this.libraryExplorerList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.libraryExplorerList1.Location = new System.Drawing.Point(0, 0);
            this.libraryExplorerList1.Name = "libraryExplorerList1";
            this.libraryExplorerList1.SelectedFile = null;
            this.libraryExplorerList1.Size = new System.Drawing.Size(854, 320);
            this.libraryExplorerList1.TabIndex = 0;
            // 
            // ExplorerListWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 320);
            this.Controls.Add(this.libraryExplorerList1);
            this.Name = "ExplorerListWindow";
            this.Text = "モジュール一覧";
            this.ResumeLayout(false);

        }

        #endregion

#pragma warning disable CS1591 // 公開されている型またはメンバーの XML コメントがありません
        protected Control.LibraryExplorerList libraryExplorerList1;
#pragma warning restore CS1591 // 公開されている型またはメンバーの XML コメントがありません
    }
}