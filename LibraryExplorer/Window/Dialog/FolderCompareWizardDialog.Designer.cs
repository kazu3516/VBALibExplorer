namespace LibraryExplorer.Window.Dialog {
    partial class FolderCompareWizardDialog {
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
            this.folderCompareWizard1 = new LibraryExplorer.Control.Wizard.FolderCompareWizard();
            this.SuspendLayout();
            // 
            // folderCompareWizard1
            // 
            this.folderCompareWizard1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.folderCompareWizard1.Initialized = false;
            this.folderCompareWizard1.Location = new System.Drawing.Point(0, 0);
            this.folderCompareWizard1.MinimumSize = new System.Drawing.Size(954, 516);
            this.folderCompareWizard1.Name = "folderCompareWizard1";
            this.folderCompareWizard1.Size = new System.Drawing.Size(1029, 556);
            this.folderCompareWizard1.TabIndex = 0;
            this.folderCompareWizard1.TargetFile = null;
            this.folderCompareWizard1.TargetProject = null;
            this.folderCompareWizard1.WizardFinished += new System.EventHandler(this.folderCompareWizard1_WizardFinished);
            this.folderCompareWizard1.WizardCanceled += new System.EventHandler(this.folderCompareWizard1_WizardCanceled);
            // 
            // FolderCompareWizardDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1029, 556);
            this.Controls.Add(this.folderCompareWizard1);
            this.Name = "FolderCompareWizardDialog";
            this.Text = "フォルダの比較ウィザード";
            this.VisibleChanged += new System.EventHandler(this.FolderCompareWizardDialog_VisibleChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private Control.Wizard.FolderCompareWizard folderCompareWizard1;
    }
}