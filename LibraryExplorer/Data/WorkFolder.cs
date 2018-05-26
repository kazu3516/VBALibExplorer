using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fsPath = System.IO.Path;

using LibraryExplorer.Common;

namespace LibraryExplorer.Data {

    //TODO:TemporaryFolderの実装見直し（現状はOfficeFileから移植しただけ。OfficeFileで移譲を使えるように検討する。(フォルダ名の生成ルールをどうやってOfficeFile側から制御するか？)）

    /// <summary>
    /// LibraryFileを格納する一時フォルダを表すクラスです。
    /// </summary>
    public class WorkFolder:LibraryFolder, IDisposable {

        #region フィールド(メンバ変数、プロパティ、イベント)

        #region TemporaryFolderName
        private string m_TemporaryFolderName;
        /// <summary>
        /// TemporaryFolderNameを取得、設定します。
        /// </summary>
        public string TemporaryFolderName {
            get {
                return this.m_TemporaryFolderName;
            }
        }
        #endregion

        #region FolderNameFormatString
        private string m_FolderNameFormatString;
        /// <summary>
        /// FolderNameFormatStringを取得、設定します。
        /// </summary>
        public string FolderNameFormatString {
            get {
                return this.m_FolderNameFormatString;
            }
            set {
                this.m_FolderNameFormatString = value;
            }
        }
        #endregion

        #region DeleteAtClose
        private bool m_DeleteAtClose;
        /// <summary>
        /// DeleteAtCloseを取得、設定します。
        /// </summary>
        public bool DeleteAtClose {
            get {
                return this.m_DeleteAtClose;
            }
            set {
                this.m_DeleteAtClose = value;
            }
        }
        #endregion

        #endregion

        #region コンストラクタ
        /// <summary>
        /// TemporaryFolderオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public WorkFolder():base() {
            this.m_TemporaryFolderName = "";
            this.m_FolderNameFormatString = $"yyyyMMdd_HHmmss";
            this.m_DeleteAtClose = false;
        }

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには
        /// <summary>
        /// このインスタンスが保持するリソースを破棄します。
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    // マネージ状態を破棄します (マネージ オブジェクト)。
                }

                // アンマネージ リソース (アンマネージ オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                //大きなフィールドを null に設定します。
                if (this.DeleteAtClose) {
                    this.Delete();
                }

                disposedValue = true;
            }
        }

        // 上の Dispose(bool disposing) にアンマネージ リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
        /// <summary>
        /// このインスタンスのファイナライズを行います。
        /// </summary>
        ~WorkFolder() {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(false);
        }

        // このコードは、破棄可能なパターンを正しく実装できるように追加されました。
        /// <summary>
        /// このファイルを閉じます。
        /// </summary>
        public void Close() {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(true);
            // 上のファイナライザーがオーバーライドされる場合は、次の行のコメントを解除してください。
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// IDisposableインターフェースを実装します。
        /// このメソッドはCloseメソッド呼び出しと同じです。
        /// </summary>
        void IDisposable.Dispose() {
            this.Close();
        }
        #endregion

        #endregion

        #region イベントハンドラ

        #endregion

        #region Clear
        /// <summary>
        /// 一時フォルダ内のファイル、フォルダをすべて削除します。
        /// </summary>
        public void Clear() {
            DirectoryInfo directory = new DirectoryInfo(this.m_TemporaryFolderName);
            if (!directory.Exists) {
                return;
            }
            //ファイルの削除
            this.DeleteAllFiles(directory);
            //フォルダの削除
            this.DeleteAllDirectories(directory);
        }

        /// <summary>
        /// 一時フォルダ内のすべてのファイルを削除します。
        /// </summary>
        /// <param name="directory"></param>
        private void DeleteAllFiles(DirectoryInfo directory) {
            try {

                FileInfo[] files = directory.GetFiles();
                for (int i = files.Length - 1; i >= 0; i--) {
                    files[i].Delete();
                }
            }
            catch (Exception ex) {
                AppMain.logger.Error($"error occured when delete all files in temporary folder.", ex);
                throw new ApplicationException("一時フォルダ内のファイル削除時に例外が発生しました。", ex);
            }
        }
        /// <summary>
        /// 一時フォルダ内のすべてのフォルダを削除します。
        /// </summary>
        /// <param name="directory"></param>
        private void DeleteAllDirectories(DirectoryInfo directory) {
            try {

                DirectoryInfo[] subDirectories = directory.GetDirectories();
                for (int i = subDirectories.Length - 1; i >= 0; i--) {
                    subDirectories[i].Delete(true);
                }
            }
            catch (Exception ex) {
                AppMain.logger.Error($"error occured when delete all folders in temporary folder.", ex);
                throw new ApplicationException("一時フォルダ内のフォルダ削除時に例外が発生しました。", ex);
            }
        }
        #endregion

        #region Create
        /// <summary>
        /// 一時フォルダを作成します。
        /// 作成されたフォルダのパスはこのインスタンスが保持します。
        /// </summary>
        public void Create() {
            this.m_TemporaryFolderName = this.GetTemporaryFolderName();
            this.CreateFolder(this.m_TemporaryFolderName);
            
            this.Path = this.m_TemporaryFolderName;
        }
        /// <summary>
        /// 指定されたフォルダ名で一時フォルダを作成します。
        /// 作成されたフォルダのパスはこのインスタンスが保持します。
        /// </summary>
        /// <param name="workspaceFolderPath"></param>
        public void Create(string workspaceFolderPath) {
            if (workspaceFolderPath == "") {
                //空文字列の場合は無効なパスとみなし、引数なしのCreateメソッド経由で再度呼び出される
                this.Create();
                return;
            }
            else {
                this.Path = workspaceFolderPath;
                this.CreateFolder(this.Path);
            }
        }


        /// <summary>
        /// GenerateTemporaryFolderNameメソッドで得られた名前と、アプリケーションで保持するtemporaryFolderPathを合成し、一時フォルダのフルパスを取得します。
        /// </summary>
        /// <returns></returns>
        protected virtual string GetTemporaryFolderName() {
            string tempFolderName = this.GenerateTemporaryFolderName();
            return fsPath.Combine(AppMain.g_AppMain.TemporaryFolderPath, tempFolderName);
        }
        /// <summary>
        /// 一時フォルダのフォルダ名を生成します。
        /// 既定ではDateTime.Now.ToString($"yyyyMMdd_hhmmss")を返します。
        /// </summary>
        /// <returns></returns>
        protected virtual string GenerateTemporaryFolderName() {
            return DateTime.Now.ToString(this.m_FolderNameFormatString);
        }

        /// <summary>
        /// パスを指定して、一時フォルダを作成します。
        /// </summary>
        /// <param name="path"></param>
        protected void CreateFolder(string path) {
            try {
                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                    AppMain.logger.Info($"create temp folder. path = {path}");
                }
            }
            catch (Exception ex) {
                AppMain.logger.Warn($"error occured when create temp folder.", ex);
                throw new ApplicationException("一時フォルダの作成時に例外が発生しました。", ex);
            }
        }

        #endregion

        #region Delete
        /// <summary>
        /// このインスタンスが保持するパスを使用して、一時フォルダを削除します。
        /// </summary>
        public void Delete() {
            this.Delete(this.m_TemporaryFolderName);
        }
        /// <summary>
        /// パスを指定して、一時フォルダを削除します。
        /// </summary>
        /// <param name="path"></param>
        protected void Delete(string path) {
            try {
                if (Directory.Exists(path)) {
                    Directory.Delete(path, true);
                    AppMain.logger.Info($"delete temp folder. path = {path}");
                }
            }
            catch (Exception ex) {
                AppMain.logger.Warn($"error occured when delete temp folder.", ex);
                throw new ApplicationException("一時フォルダの削除時に例外が発生しました。", ex);
            }
        }
        #endregion

        #region Exist
        /// <summary>
        /// テンポラリフォルダが存在するかどうかを返します。
        /// </summary>
        /// <returns></returns>
        public bool Exist() {
            return Directory.Exists(this.m_TemporaryFolderName);
        }

        #endregion

        #region MakeEmptyFolder
        /// <summary>
        /// 空のテンポラリフォルダを用意します。
        /// </summary>
        public void MakeEmptyFolder() {
            if (this.Exist()) {
                this.Clear();
            }
            else {
                this.Create();
            }
        }
        #endregion

    }
}
