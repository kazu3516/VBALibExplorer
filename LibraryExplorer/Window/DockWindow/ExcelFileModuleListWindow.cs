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
using LibraryExplorer.Data;
using WeifenLuo.WinFormsUI.Docking;

namespace LibraryExplorer.Window.DockWindow {

    /// <summary>
    /// ExcelFileからエクスポートしたModuleを一覧表示するウインドウを表すクラスです。
    /// </summary>
    public partial class ExcelFileModuleListWindow : ExplorerListWindow {

        #region フィールド(メンバ変数、プロパティ、イベント)

        private bool m_ExportComplete;

        #region TargetFile
        private ExcelFile m_TargetFile;
        /// <summary>
        /// TargetFileが変更された場合に発生するイベントです。
        /// </summary>
        public event EventHandler<EventArgs<ExcelFile>> TargetFileChanged;
        /// <summary>
        /// TargetFileが変更された場合に呼び出されます。
        /// </summary>
        /// <param name="e">イベントパラメータ</param>
        protected void OnTargetFileChanged(EventArgs<ExcelFile> e) {
            this.TargetFileChanged?.Invoke(this, e);
        }
        /// <summary>
        /// TargetFileを取得、設定します。
        /// </summary>
        public ExcelFile TargetFile {
            get {
                return this.m_TargetFile;
            }
            set {
                this.SetProperty(ref this.m_TargetFile, value, ((oldValue) => {
                    if (this.TargetFileChanged != null) {
                        this.OnTargetFileChanged(new EventArgs<ExcelFile>(oldValue));
                    }
                }));
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

            this.TargetFileChanged += this.ExcelFileModuleListWindow_TargetFileChanged;

            this.m_ExportComplete = true;
        }


        #endregion

        #region イベントハンドラ
        private void ExcelFileModuleListWindow_TargetFileChanged(object sender, EventArgs<ExcelFile> e) {
            if (e.OldValue != null) {
                e.OldValue.OutputLogRequest -= this.TargetFile_OutputLogRequest;
            }
            if (this.TargetFile != null) {
                this.TargetFile.OutputLogRequest += this.TargetFile_OutputLogRequest;

                //TargetFolderに設定しておく。
                //Exportする場合、Export後に再設定される。
                this.TargetFolder = this.TargetFile.TemporaryFolder;
            }
        }

        private void TargetFile_OutputLogRequest(object sender, Common.Request.OutputLogRequestEventArgs e) {
            this.OnOutputLogRequest(e);
        }


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
