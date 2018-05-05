using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryExplorer.Common;
using LibraryExplorer.Control;
using LibraryExplorer.Controller;
using LibraryExplorer.Data;
using WeifenLuo.WinFormsUI.Docking;

namespace LibraryExplorer.Window.DockWindow {

    /// <summary>
    /// ExcelFileからエクスポートしたModuleを一覧表示するウインドウを表すクラスです。
    /// </summary>
    public partial class ExcelFileModuleListWindow : ExplorerListWindow {

        #region フィールド(メンバ変数、プロパティ、イベント)

        private ExcelFileController m_ExcelFileController;

        private bool m_ExportComplete;

        #region TargetFile
        /// <summary>
        /// TargetFileを取得または設定します。
        /// </summary>
        public ExcelFile TargetFile {
            get {
                return this.m_ExcelFileController.TargetFile;
            }
            set {
                this.m_ExcelFileController.TargetFile = value;
            }
        }
        #endregion

        #endregion

        #region コンストラクタ
        /// <summary>
        /// ExcelFileModuleListWindowオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public ExcelFileModuleListWindow() {
            InitializeComponent();

            this.m_ExcelFileController = new ExcelFileController();
            this.m_ExcelFileController.TargetFileChanged += this.M_ExcelFileController_TargetFileChanged;

            this.libraryExplorerList1.LibraryFileController = this.m_ExcelFileController;

            this.m_ExportComplete = true;
        }

        private void M_ExcelFileController_TargetFileChanged(object sender, Controller.EventArgs<ExcelFile> e) {
            this.TargetFolder = this.TargetFile.TemporaryFolder;
        }


        #endregion

        #region イベントハンドラ


        #endregion

        /// <summary>
        /// Moduleのエクスポートを非同期に行います。
        /// </summary>
        public async Task ExportModules() {
            if (!this.m_ExportComplete) {
                return;
            }
            this.m_ExportComplete = false;
            this.Cursor = Cursors.WaitCursor;

            await this.TargetFile.ExportAll(true);
            //Exportが終わったらTargetFolderの設定を行う。
            this.TargetFolder = this.TargetFile.TemporaryFolder;

            this.Cursor = Cursors.Default;
            this.m_ExportComplete = true;
        }

    }
}
