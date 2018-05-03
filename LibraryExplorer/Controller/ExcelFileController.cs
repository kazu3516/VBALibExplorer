using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryExplorer.Common;
using LibraryExplorer.Common.Request;
using LibraryExplorer.Data;
namespace LibraryExplorer.Controller {

    /// <summary>
    /// ExplorerListとExcelFile間のインターフェースとなるコントローラを表すクラスです。
    /// </summary>
    public class ExcelFileController: LibraryFileControllerBase {

        //NOTE:更新による自動エクスポートは廃止し、手動で再読み込みとしたため、再読み込み機能をコメントアウト。OutputLogRequestの上位伝達のみ残す

        #region フィールド(メンバ変数、プロパティ、イベント)
        //private DateTime m_TargetFileUpdateDate;

        //private bool m_RefreshComplete;


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


        #region IsRequiredAsyncRefresh
        //public override bool IsRequiredAsyncRefresh {
        //    get {
        //        return true;
        //    }
        //}
        #endregion

        #region RefreshComplete
        //public override bool RefreshComplete {
        //    get {
        //        return this.m_RefreshComplete;
        //    }
        //} 
        #endregion


        #endregion

        #region コンストラクタ
        /// <summary>
        /// ExcelFileControllerオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public ExcelFileController():base() {
            //this.m_RefreshComplete = true;

            this.TargetFileChanged += this.ExcelFileController_TargetFileChanged;
        }

        #endregion

        #region イベントハンドラ
        private void ExcelFileController_TargetFileChanged(object sender, EventArgs<ExcelFile> e) {
            if (e.OldValue != null) {
                e.OldValue.OutputLogRequest -= this.TargetFile_OutputLogRequest;
            }
            if (this.TargetFile != null) {
                this.TargetFile.OutputLogRequest += this.TargetFile_OutputLogRequest;
            }

            //FileControllerのRefreshのみ行うことは無く、必ずExplorerListのRefreshと同時に行うため、FileControllerからはEventを発報するのみとする。

            //this.OnRequestRefresh(EventArgs.Empty);

        }

        private void TargetFile_OutputLogRequest(object sender, OutputLogRequestEventArgs e) {
            this.OnOutputLogRequest(e);
        }

        #endregion

        #region ExportModule
        //private async Task ExportModule() {

        //    //エクスポート先フォルダの用意
        //    this.TargetFile.MakeEmptyTemporaryFolder();

        //    //最終更新日の記録
        //    this.m_TargetFileUpdateDate = this.TargetFile.UpdateDate;

        //    //
        //    //モジュールをエクスポートするスクリプトを起動
        //    //スクリプト完了後の処理を渡す
        //    await this.TargetFile.ExportAll().ConfigureAwait(false);
            

        //    this.TargetFolder = new LibraryFolder() { Path = this.m_TargetFile.TemporaryFolderName };
        //    //NOTE:LibraryExplorerList側でSuspendRefresh呼び出しを追加。SetTargetFolderWithoutEventは廃止。
        //    //this.SetTargetFolderWithoutEvent(new LibraryFolder() { Path = this.m_TargetFile.TemporaryFolderName });


        //    //TargetFolderChangedによりRefreshDisplay(false)が呼び出され、再度ExportModuleが呼ばれてしまい、無限ループとなるため、更新を停止する。
        //    //this.SuspendRefresh();
        //    //this.TargetFolder = new LibraryFolder() { Path = this.m_TargetFile.TemporaryFolderName };
        //    //this.ResumeRefresh(true);
        //    //RefreshDisplay(false)を呼び出すと再度ExportModuleが呼ばれてしまうため、keep=trueとする
        //}

        #endregion

        #region IsRequiredClearItem
        //public override bool IsRequiredClearItem(bool keep) {
        //    return !keep;
            
        //    //自動エクスポートを行わないようにする
            
        //    //keep=falseまたはTargetFileが更新された場合に初期化する。
        //    //return !keep || (this.TargetFile != null && this.TargetFile.Exist && this.m_TargetFileUpdateDate != this.TargetFile.UpdateDate);
        //}

        #endregion

        #region Refresh

        //protected override void OnRefresh(bool keep) {
        //    //インターフェースは同期メソッドなので、ここでTask.Wait
        //    //呼び出した先は全てConfigreAwait(false)でデッドロック回避
            
        //    //NOTE:同期処理のため、ここでWaitしている。UIスレッドが空かず、Cursor変更等は反映されないため、RefreshAsyncを使用する
        //    this.OnRefreshController(keep).Wait();

        //    base.OnRefresh(keep);
        //}
        #endregion

        #region async RefreshAsync
        //protected async override Task OnRefreshAsync(bool keep) {
        //    await this.OnRefreshController(keep).ConfigureAwait(false);
        //}
        #endregion

        #region async RefreshDisplay
        //protected async Task OnRefreshController(bool keep) {

        //    if (this.IsRequiredClearItem(keep)) {
        //        this.m_RefreshComplete = false;

        //        //await this.ExportModule().ConfigureAwait(false);
        //        await Task.Run(new Action(() => {
        //        }));

        //        this.m_RefreshComplete = true;
        //    }

        //}

        #endregion


    }
}
