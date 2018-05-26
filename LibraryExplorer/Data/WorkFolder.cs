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

        #region FolderNamePrefix
        private string m_FolderNamePrefix;
        /// <summary>
        /// FolderNamePrefixを取得、設定します。
        /// </summary>
        public string FolderNamePrefix {
            get {
                return this.m_FolderNamePrefix;
            }
            set {
                this.m_FolderNamePrefix = value;
            }
        }
        #endregion

        #region FolderNameSuffix
        private string m_FolderNameSuffix;
        /// <summary>
        /// FolderNameSuffixを取得、設定します。
        /// </summary>
        public string FolderNameSuffix {
            get {
                return this.m_FolderNameSuffix;
            }
            set {
                this.m_FolderNameSuffix = value;
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

        #region BaseFolderPath
        private string m_BaseFolderPath;
        /// <summary>
        /// BaseFolderPathを取得、設定します。
        /// 既定ではAppMain.g_AppMain.TemporaryFolderPathを使用します。
        /// </summary>
        public string BaseFolderPath {
            get {
                return this.m_BaseFolderPath;
            }
            set {
                this.m_BaseFolderPath = value;
            }
        }
        #endregion
        
        #endregion

        #region コンストラクタ
        /// <summary>
        /// TemporaryFolderオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public WorkFolder():base() {
            this.m_FolderNameFormatString = $"yyyyMMdd_HHmmss";
            this.m_DeleteAtClose = false;
            this.m_BaseFolderPath = AppMain.g_AppMain.TemporaryFolderPath;
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
            DirectoryInfo directory = new DirectoryInfo(this.Path);
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
            this.Path = this.GetWorkFolderName();
            this.CreateFolder(this.Path);
        }
        /// <summary>
        /// 指定されたフォルダ名で一時フォルダを作成します。
        /// 作成されたフォルダのパスはこのインスタンスが保持します。
        /// </summary>
        /// <param name="workFolderPath"></param>
        public void Create(string workFolderPath) {
            if (workFolderPath == "") {
                //空文字列の場合は無効なパスとみなし、引数なしのCreateメソッド経由で再度呼び出される
                this.Create();
                return;
            }
            else {
                this.Path = workFolderPath;
                this.CreateFolder(this.Path);
            }
        }


        /// <summary>
        /// GenerateWorkFolderNameメソッドで得られた名前と、BaseFolderPathを合成し、一時フォルダのフルパスを取得します。
        /// </summary>
        /// <returns></returns>
        protected virtual string GetWorkFolderName() {
            string workFolderName = this.GenerateWorkFolderName();
            return fsPath.Combine(this.m_BaseFolderPath, workFolderName);
        }
        /// <summary>
        /// 一時フォルダのフォルダ名を生成します。
        /// 既定ではDateTime.Now.ToString($"yyyyMMdd_hhmmss")にPrefixとSuffixを付加して返します。
        /// </summary>
        /// <returns></returns>
        protected virtual string GenerateWorkFolderName() {
            return this.m_FolderNamePrefix + DateTime.Now.ToString(this.m_FolderNameFormatString) + this.m_FolderNameSuffix;
        }

        /// <summary>
        /// パスを指定して、一時フォルダを作成します。
        /// </summary>
        /// <param name="path"></param>
        protected void CreateFolder(string path) {
            try {
                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                    AppMain.logger.Info($"create work folder. path = {path}");
                }
            }
            catch (Exception ex) {
                AppMain.logger.Warn($"error occured when create work folder.", ex);
                throw new ApplicationException("一時フォルダの作成時に例外が発生しました。", ex);
            }
        }

        #endregion

        #region Delete
        /// <summary>
        /// このインスタンスが保持するパスを使用して、一時フォルダを削除します。
        /// </summary>
        public void Delete() {
            this.Delete(this.Path);
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

        #region CopyFolder
        /// <summary>
        /// このインスタンスが保持するフォルダを、指定したパスへコピーします。
        /// </summary>
        /// <param name="destinationPath">コピー先のパス</param>
        /// <param name="destinationName">コピー先のフォルダ名</param>
        public void CopyFolder(string destinationPath,string destinationName) {
            if (!this.Exist()) {
                return;
            }
            this.CopyFolder(this.Path, destinationPath, destinationName);
        }

        private void CopyFolder(string srcPath,string dstPath,string dstName) {
            DirectoryInfo srcDir = new DirectoryInfo(srcPath);
            if (!srcDir.Exists) {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + srcPath);
            }

            string dstFullPath = fsPath.Combine(dstPath, dstName);
            DirectoryInfo dstDir = new DirectoryInfo(dstFullPath);
            //コピー先のディレクトリがなければ作成する
            if (!dstDir.Exists) {
                dstDir.Create();
                dstDir.Attributes = srcDir.Attributes;
            }

            //ファイルのコピー
            foreach (FileInfo fileInfo in srcDir.GetFiles()) {
                //同じファイルが存在していたら、常に上書きする
                string dstFullName = fsPath.Combine(dstFullPath, fileInfo.Name);
                fileInfo.CopyTo(dstFullName, true);
            }

            //ディレクトリのコピー（再帰を使用）
            foreach (DirectoryInfo subDir in srcDir.GetDirectories()) {
                this.CopyFolder(subDir.FullName,dstFullPath,subDir.Name);
            }
        }
        #endregion

    }
}
