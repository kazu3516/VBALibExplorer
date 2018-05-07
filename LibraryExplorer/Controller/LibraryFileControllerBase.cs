using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryExplorer.Data;
using LibraryExplorer.Common;
using LibraryExplorer.Common.Request;
namespace LibraryExplorer.Controller {

    /// <summary>
    /// ExplorerListとLibrary間のインターフェースとなるコントローラを表すクラスです。
    /// </summary>
    public abstract class LibraryFileControllerBase : ILibraryController {


        #region フィールド(メンバ変数、プロパティ、イベント)

        #region BeforeRefreshイベント
        /// <summary>
        /// Controllerの更新前に発生するイベントです。
        /// </summary>
        public event EventHandler BeforeRefresh;
        /// <summary>
        /// BeforeRefreshイベントを発生させます。
        /// </summary>
        /// <param name="e"></param>
        protected void OnBeforeRefresh(EventArgs e) {
            if (this.BeforeRefresh != null) {
                this.BeforeRefresh(this, e);
            }
        }
        #endregion

        #region AfterRefreshイベント
        /// <summary>
        /// Controllerの更新後に発生するイベントです。
        /// </summary>
        public event EventHandler AfterRefresh;
        /// <summary>
        /// AfterRefreshイベントを発生させます。
        /// </summary>
        /// <param name="e"></param>
        protected void OnAfterRefresh(EventArgs e) {
            if (this.AfterRefresh != null) {
                this.AfterRefresh(this, e);
            }
        }
        #endregion

        #region RequestRefreshイベント
        /// <summary>
        /// Controllerの更新要求を行うイベントです。
        /// </summary>
        public event EventHandler RequestRefresh;
        /// <summary>
        /// RequestRefreshイベントを発生させます。
        /// </summary>
        /// <param name="e"></param>
        protected void OnRequestRefresh(EventArgs e) {
            if (this.RequestRefresh != null) {
                this.RequestRefresh(this, e);
            }
        }
        #endregion

        #region OutputLogRequestイベント
        /// <summary>
        /// 出力ウィンドウへのメッセージ出力要求を表すイベントです。
        /// </summary>
        public event OutputLogRequestEventHandler OutputLogRequest;
        /// <summary>
        /// OutputLogRequestイベントを発生させます。
        /// </summary>
        /// <param name="e"></param>
        protected void OnOutputLogRequest(OutputLogRequestEventArgs e) {
            this.OutputLogRequest?.Invoke(this, e);
        }

        #endregion


        #region TargetFolder
        /// <summary>
        /// TargetFolderを設定します。
        /// このメソッドによるTargetFolderの変更は、TargetFolderChangedイベントを発生させません。
        /// </summary>
        /// <param name="value"></param>
        protected void SetTargetFolderWithoutEvent(LibraryFolder value) {
            this.m_TargetFolder = value;
        }

        private LibraryFolder m_TargetFolder;
        /// <summary>
        /// TargetFolderが変更された場合に発生するイベントです。
        /// </summary>
        public event EventHandler<EventArgs<LibraryFolder>> TargetFolderChanged;
        /// <summary>
        /// TargetFolderが変更された場合に呼び出されます。
        /// </summary>
        /// <param name="e">イベントパラメータ</param>
        protected void OnTargetFolderChanged(EventArgs<LibraryFolder> e) {
            this.TargetFolderChanged?.Invoke(this, e);
        }
        /// <summary>
        /// TargetFolderを取得、設定します。
        /// </summary>
        public LibraryFolder TargetFolder {
            get {
                return this.m_TargetFolder;
            }
            set {
                this.SetProperty(ref this.m_TargetFolder, value, ((oldValue) => {
                    if (this.TargetFolderChanged != null) {
                        this.OnTargetFolderChanged(new EventArgs<LibraryFolder>(oldValue));
                    }
                }));
            }
        }
        #endregion


        #region virtual RefreshComplete
        /// <summary>
        /// RefreshCompleteを取得します。
        /// このプロパティは既定ではtrueを返します。
        /// </summary>
        public virtual bool RefreshComplete {
            get {
                return true;
            }
        }
        #endregion

        #region virtual IsRequiredAsyncRefresh
        /// <summary>
        /// IsRequiredAsyncRefreshを取得します。
        /// このプロパティでは既定ではfalseを返します。
        /// </summary>
        public virtual bool IsRequiredAsyncRefresh {
            get {
                return false;
            }
        }
        #endregion


        #region PropertyChanged/SetProperty
        /// <summary>
        /// Propertyが変更されたことを通知するイベントです。
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// INotifyPropertyChangedを実装し、PropertyChangedイベントを発生させるためのsetterメソッド。
        /// fireEventデリゲートを指定することで個別のプロパティのChangedイベントを実装することができます。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="fireEvent"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected virtual bool SetProperty<T>(ref T field, T value, Action<T> fireEvent, [System.Runtime.CompilerServices.CallerMemberName]string propertyName = null) {
            if (Equals(field, value)) {
                return false;
            }
            T oldValue = field;
            field = value;
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
            fireEvent(oldValue);
            return true;
        }
        /// <summary>
        /// PropertyChangedイベントを発生させます。
        /// </summary>
        /// <param name="e"></param>
        protected void OnPropertyChanged(PropertyChangedEventArgs e) {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(e.PropertyName));
        }

        #endregion

        #endregion

        #region コンストラクタ
        /// <summary>
        /// LibraryFileControllerBaseオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        protected LibraryFileControllerBase() {
        }
        #endregion

        #region イベントハンドラ

        #endregion

        #region virtual IsRequiredClearItem

        /// <summary>
        /// アイテムのクリアが必要かどうかを返します。
        /// </summary>
        /// <param name="keep"></param>
        /// <returns></returns>
        public virtual bool IsRequiredClearItem(bool keep) {
            return !keep;
        }
        #endregion


        #region Refresh
        /// <summary>
        /// kControllerの更新を行います。
        /// </summary>
        /// <param name="keep"></param>
        public void Refresh(bool keep) {
            this.OnBeforeRefresh(EventArgs.Empty);

            this.OnRefresh(keep);

            this.OnAfterRefresh(EventArgs.Empty);
        }
        /// <summary>
        /// 派生クラスでオーバーライドされると、Refresh時の動作を定義します。
        /// </summary>
        /// <param name="keep"></param>
        protected virtual void OnRefresh(bool keep) {
        }
        #endregion

        #region RefreshAsync

        /// <summary>
        /// Controllerを非同期に更新します。
        /// </summary>
        /// <param name="keep"></param>
        /// <returns></returns>
        public async Task RefreshAsync(bool keep) {
            this.OnBeforeRefresh(EventArgs.Empty);

            await this.OnRefreshAsync(keep);

            this.OnAfterRefresh(EventArgs.Empty);
        }

        /// <summary>
        /// 派生クラスでオーバーライドされると、RefreshAsync時の動作を定義します。
        /// </summary>
        /// <param name="keep"></param>
        /// <returns></returns>
        protected async virtual Task OnRefreshAsync(bool keep) {
            await Task.Run(() => {
                ;
            }).ConfigureAwait(false);
        }
        #endregion


        #region OpenFile
        /// <summary>
        /// ファイルを開きます。
        /// </summary>
        /// <param name="file"></param>
        public void OpenFile(LibraryFile file) {
            //パスの設定
            string editorPath = this.GetEditorPath();
            //引数の設定
            string editorArguments = this.GetEditorArguments(file);
            //プロセスの起動
            this.StartProcess(editorPath, editorArguments);
        }

        /// <summary>
        /// エディタのパスと引数を起動してプロセスを起動します。
        /// </summary>
        /// <param name="editorPath"></param>
        /// <param name="editorArguments"></param>
        private void StartProcess(string editorPath, string editorArguments) {
            ProcessStartInfo info = new ProcessStartInfo() { FileName = editorPath, Arguments = editorArguments };
            Process process = new Process() { StartInfo = info };
            try {
                process.Start();
            }
            catch (Exception ex) {
                string errorMessage = $"{this.GetType().Name}.OpenFile プロセスの起動に失敗しました。Exception={ex.GetType().Name}, Message={ex.Message}, Path={editorPath}, Arguments={editorArguments}";
                AppMain.logger.Error(errorMessage, ex);
                throw new ApplicationException(errorMessage, ex);
            }
        }

        /// <summary>
        /// エディタのパスを取得します。
        /// エディタが指定されていない場合、Windows標準のメモ帳を使用します。
        /// </summary>
        /// <returns></returns>
        private string GetEditorPath() {
            string editorPath = AppMain.g_AppMain.AppInfo.EditorPath;
            if (!File.Exists(editorPath)) {
                editorPath = "NotePad";
            }
            return editorPath;
        }

        /// <summary>
        /// エディタの引数を指定します。
        /// 引数内の%filename%は指定したLibraryFileのFileNameに置き換えられます。
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private string GetEditorArguments(LibraryFile file) {
            string editorArguments = AppMain.g_AppMain.AppInfo.EditorArguments;
            if (!editorArguments.Contains("%filename%")) {
                //引数に%filename%が含まれていない場合、末尾に追加する
                if (editorArguments.Length > 0) {
                    editorArguments += " ";
                }
                editorArguments += "%filename%";
            }
            editorArguments = editorArguments.Replace("%filename%", "\"" + file.FileName + "\"");
            return editorArguments;
        }


        #endregion

    }
}
