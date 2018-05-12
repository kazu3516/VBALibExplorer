namespace LibraryExplorer.Window.DockWindow {
    partial class ExplorerTreeWindow {
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
            LibraryExplorer.Data.LibraryProject libraryProject1 = new LibraryExplorer.Data.LibraryProject();
            this.panel1 = new System.Windows.Forms.Panel();
            this.libraryExplorerTree1 = new LibraryExplorer.Control.LibraryExplorerTree();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.libraryExplorerTree1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(390, 406);
            this.panel1.TabIndex = 0;
            // 
            // libraryExplorerTree1
            // 
            this.libraryExplorerTree1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.libraryExplorerTree1.Location = new System.Drawing.Point(0, 0);
            this.libraryExplorerTree1.Name = "libraryExplorerTree1";
            this.libraryExplorerTree1.SelectedFile = null;
            this.libraryExplorerTree1.SelectedFolder = null;
            this.libraryExplorerTree1.Size = new System.Drawing.Size(390, 406);
            this.libraryExplorerTree1.TabIndex = 0;
            this.libraryExplorerTree1.TargetProject = libraryProject1;
            this.libraryExplorerTree1.NotifyParentRequest += new LibraryExplorer.Common.Request.RequestEventHandler(this.libraryExplorerTree1_NotifyParentRequest);
            // 
            // ExplorerTreeWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 406);
            this.Controls.Add(this.panel1);
            this.HideOnClose = true;
            this.Name = "ExplorerTreeWindow";
            this.Text = "エクスプローラ";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Control.LibraryExplorerTree libraryExplorerTree1;
    }
}