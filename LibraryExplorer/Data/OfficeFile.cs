using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryExplorer.Common;
using LibraryExplorer.Common.Request;
using System.Windows.Forms;
namespace LibraryExplorer.Data {

    #region OfficeFile
    /// <summary>
    /// Moduleを内部に保持するOfficeファイルを表します。
    /// </summary>
    public abstract class OfficeFile:IOutputLogRequest {

        #region フィールド(メンバ変数、プロパティ、イベント)

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


        #region ExportScriptName
        /// <summary>
        /// ExportScriptNameを取得します。
        /// </summary>
        protected abstract string ExportScriptName {
            get;
        }
        #endregion

        #region ImportScriptName
        /// <summary>
        /// ExportScriptNameを取得します。
        /// </summary>
        protected abstract string ImportScriptName {
            get;
        }
        #endregion


        #region FileType
        private OfficeFileType m_FileType;
        /// <summary>
        /// FileTypeを取得します。
        /// </summary>
        public OfficeFileType FileType {
            get {
                return this.m_FileType;
            }
        }
        #endregion

        #region FileName
        private string m_FileName;
        /// <summary>
        /// FileNameが変更された場合に発生するイベントです。
        /// </summary>
        public event EventHandler<EventArgs<string>> FileNameChanged;
        /// <summary>
        /// FileNameが変更された場合に呼び出されます。
        /// </summary>
        /// <param name="e">イベントパラメータ</param>
        protected void OnFileNameChanged(EventArgs<string> e) {
            this.FileNameChanged?.Invoke(this, e);
        }
        /// <summary>
        /// FileNameを取得、設定します。
        /// </summary>
        public string FileName {
            get {
                return this.m_FileName;
            }
            set {
                this.SetProperty(ref this.m_FileName, value, ((oldValue) => {
                    if (this.FileNameChanged != null) {
                        this.OnFileNameChanged(new EventArgs<string>(oldValue));
                    }
                }));
            }
        }
        #endregion


        #region ScriptOutputMessage
        private string m_ScriptOutputMessage;
        /// <summary>
        /// ScriptOutputMessageを取得します。
        /// このクラスの派生クラスではこのプロパティに値を設定することができます。。
        /// </summary>
        public string ScriptOutputMessage {
            get {
                return this.m_ScriptOutputMessage;
            }
            protected set {
                this.m_ScriptOutputMessage = value;
            }
        }
        #endregion

        #region HasError
        private bool m_HasError;
        /// <summary>
        /// HasErrorを取得します。
        /// </summary>
        public bool HasError {
            get {
                return this.m_HasError;
            }
        }
        #endregion


        #region Exist
        /// <summary>
        /// このインスタンスが示すファイルが存在するかどうかを取得します。
        /// </summary>
        public bool Exist {
            get {
                return File.Exists(this.FileName);
            }
        }
        #endregion

        #region UpdateDate
        /// <summary>
        /// このインスタンスが示すファイルの更新日時を取得します。
        /// ファイルが存在しない場合、DateTime.MinValueを返します。
        /// </summary>
        public DateTime UpdateDate {
            get {
                return File.Exists(this.FileName) ? File.GetLastWriteTime(this.FileName) : DateTime.MinValue;
            }
        }
        #endregion

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

        #region TemporaryFolder
        private LibraryFolder m_TemporaryFolder;
        /// <summary>
        /// TemporaryFolderを取得します。
        /// </summary>
        public LibraryFolder TemporaryFolder {
            get {
                return this.m_TemporaryFolder;
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
        /// <summary>
        /// 変更前の値を保持するEventArgsクラスの派生クラスです。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class EventArgs<T> : EventArgs {

            #region OldValue
            private T m_OldValue;
            /// <summary>
            /// OldValueを取得します。
            /// </summary>
            public T OldValue {
                get {
                    return this.m_OldValue;
                }
            }
            #endregion

            /// <summary>
            /// EventArgsクラスの新しいインスタンスを初期化します。
            /// </summary>
            /// <param name="value"></param>
            public EventArgs(T value) {
                this.m_OldValue = value;
            }
        }
        #endregion

        #endregion

        #region コンストラクタ
        /// <summary>
        /// OfficeFileオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="fileType"></param>
        protected OfficeFile(OfficeFileType fileType)  {
            this.m_ScriptOutputMessage = "";
            this.m_FileType = fileType;    
            this.m_FileName = "";
            this.m_TemporaryFolderName = "";
        }
        /// <summary>
        /// OfficeFileオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="fileType"></param>
        protected OfficeFile(string filename, OfficeFileType fileType) : this(fileType) {
            this.m_FileName = filename;
        }


        #endregion

        #region イベントハンドラ

        #endregion

        #region StartScript
        /// <summary>
        /// 指定したパスを使用してスクリプトを実行します。
        /// </summary>
        /// <param name="scriptPath"></param>
        /// <returns></returns>
        protected Task StartScript(string scriptPath) {
            string foldername = this.m_TemporaryFolderName;
            string filename = this.m_FileName;

            var tcs = new TaskCompletionSource<bool>();

            ProcessStartInfo info = new ProcessStartInfo() {
                FileName = "CScript.exe",
                Arguments = $"/Nologo \"{scriptPath}\" \"{foldername}\" \"{filename}\"",
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            //Processを非同期に実行
            Process process = new Process() {
                StartInfo = info,
                EnableRaisingEvents = true,
            };
            bool started = false;
            process.Exited += (sender, args) => {
                tcs.SetResult(true);
                process.Dispose();
                if (this.m_ScriptOutputMessage != "") {
                    this.OnOutputLogRequest(new OutputLogRequestEventArgs(this.m_ScriptOutputMessage, this.GetType().Name));
                }
            };
            process.OutputDataReceived += (sender, args) => {
                if (started) {
                    if (!string.IsNullOrEmpty(args.Data)) {
                        this.m_ScriptOutputMessage += $"{args.Data}\n";
                    }
                }
            };
            process.ErrorDataReceived += (sender, args) => {
                if (started) {
                    if (!string.IsNullOrEmpty(args.Data)) {
                        this.m_ScriptOutputMessage += $"Error : {args.Data}\n";
                        this.m_HasError = true;
                    }
                }
            };
            //開始メッセージ
            string startMessage = $"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} Script Process Start.\nFileName={this.FileName}\nScriptPath={scriptPath}\n";
            this.OnOutputLogRequest(new OutputLogRequestEventArgs(startMessage, this.GetType().Name));

            //プロセスからの情報を受け取る変数の初期化
            this.m_ScriptOutputMessage = "";
            this.m_HasError = false;
            //プロセスの開始
            started = process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            return tcs.Task;
        }

        #endregion

        #region ExportAll
        /// <summary>
        /// スクリプトを起動して、対象ファイルから全てのモジュールをエクスポートします。
        /// スクリプトは非同期に実行されます。
        /// </summary>
        /// <returns></returns>
        public async Task ExportAll() {
            string scriptPath = Path.Combine(AppMain.g_AppMain.ScriptFolderPath, this.ExportScriptName);

            await this.StartScript(scriptPath).ConfigureAwait(false);

        }

        /// <summary>
        /// スクリプトを起動して、対象ファイルから全てのモジュールをエクスポートします。
        /// スクリプトは非同期に実行されます。
        /// </summary>
        /// <param name="makeTempDir">trueを指定すると、MakeEmptyTemporaryFolderを実行した後にエクスポートを行います。</param>
        /// <returns></returns>
        public async Task ExportAll(bool makeTempDir) {
            if (makeTempDir) {
                this.MakeEmptyTemporaryFolder();
            }

            await this.ExportAll();
        }
        #endregion

        #region ImportAll
        /// <summary>
        /// スクリプトを起動して、対象ファイルから全てのモジュールをエクスポートします。
        /// スクリプトは非同期に実行されます。
        /// </summary>
        /// <returns></returns>
        public async Task ImportAll() {
            string scriptPath = Path.Combine(AppMain.g_AppMain.ScriptFolderPath, this.ImportScriptName);

            await this.StartScript(scriptPath).ConfigureAwait(false);
        }
        #endregion

        #region Close
        /// <summary>
        /// このファイルを閉じます。
        /// 既定ではテンポラリフォルダを削除します。
        /// </summary>
        public virtual void Close() {
            this.DeleteTemporaryFolder();
        }
        #endregion
        
        #region TemporaryFolder

        #region ClearFilesInTemporaryFolder
        /// <summary>
        /// 一時フォルダ内のファイル、フォルダをすべて削除します。
        /// </summary>
        public void ClearTemporaryFolder() {
            DirectoryInfo directory = new DirectoryInfo(this.m_TemporaryFolderName);
            if (!directory.Exists) {
                return;
            }
            //ファイルの削除
            this.DeleteAllFilesInTemporaryFolder(directory);
            //フォルダの削除
            this.DeleteAllDirectoriesInTemporaryFolder(directory);
        }

        /// <summary>
        /// 一時フォルダ内のすべてのファイルを削除します。
        /// </summary>
        /// <param name="directory"></param>
        private void DeleteAllFilesInTemporaryFolder(DirectoryInfo directory) {
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
        private void DeleteAllDirectoriesInTemporaryFolder(DirectoryInfo directory) {
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

        #region CreateTemporaryFolder
        /// <summary>
        /// 一時フォルダを作成します。
        /// 作成されたフォルダのパスはこのインスタンスが保持します。
        /// </summary>
        public void CreateTemporaryFolder() {
            this.m_TemporaryFolderName = this.GetTemporaryFolderName();
            this.CreateTemporaryFolder(this.m_TemporaryFolderName);
            //作成されたTemporaryFolderを表すLibraryFolderオブジェクトも作成しておく
            this.m_TemporaryFolder = new LibraryFolder() { Path = this.m_TemporaryFolderName };

        }

        /// <summary>
        /// GenerateTemporaryFolderNameメソッドで得られた名前と、アプリケーションで保持するtemporaryFolderPathを合成し、一時フォルダのフルパスを取得します。
        /// </summary>
        /// <returns></returns>
        protected virtual string GetTemporaryFolderName() {
            string tempFolderName = this.GenerateTemporaryFolderName();
            return Path.Combine(AppMain.g_AppMain.TemporaryFolderPath, tempFolderName);
        }
        /// <summary>
        /// 一時フォルダのフォルダ名を生成します。
        /// 既定ではDateTime.Now.ToString($"yyyyMMdd_hhmmss")を返します。
        /// </summary>
        /// <returns></returns>
        protected virtual string GenerateTemporaryFolderName() {
            return DateTime.Now.ToString($"yyyyMMdd_HHmmss");
        }

        /// <summary>
        /// パスを指定して、一時フォルダを作成します。
        /// </summary>
        /// <param name="path"></param>
        protected void CreateTemporaryFolder(string path) {
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

        #region DeleteTemporaryFolder
        /// <summary>
        /// このインスタンスが保持するパスを使用して、一時フォルダを削除します。
        /// </summary>
        public void DeleteTemporaryFolder() {
            this.DeleteTemporaryFolder(this.m_TemporaryFolderName);
        }
        /// <summary>
        /// パスを指定して、一時フォルダを削除します。
        /// </summary>
        /// <param name="path"></param>
        protected void DeleteTemporaryFolder(string path) {
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

        #region ExistTemporaryFolder
        /// <summary>
        /// テンポラリフォルダが存在するかどうかを返します。
        /// </summary>
        /// <returns></returns>
        public bool ExistTemporaryFolder() {
            return Directory.Exists(this.m_TemporaryFolderName);
        }

        #endregion

        #region MakeEmptyTemporaryFolder
        /// <summary>
        /// 空のテンポラリフォルダを用意します。
        /// </summary>
        public void MakeEmptyTemporaryFolder() {
            if (this.ExistTemporaryFolder()) {
                this.ClearTemporaryFolder();
            }
            else {
                this.CreateTemporaryFolder();
            }
        }
        #endregion

        #endregion

    }
    #endregion

    #region OfficeFileType
    /// <summary>
    /// OfficeFileの種類を表す列挙型です。
    /// </summary>
    public enum OfficeFileType {
        /// <summary>
        /// Word (未実装)
        /// </summary>
        Word,
        /// <summary>
        /// Excel
        /// </summary>
        Excel,
        /// <summary>
        /// PowerPoint (未実装)
        /// </summary>
        PowerPoint,
        /// <summary>
        /// OutLook (未実装)
        /// </summary>
        Outlook,
        /// <summary>
        /// Access (未実装)
        /// </summary>
        Access,
        /// <summary>
        /// Visio (未実装)
        /// </summary>
        Visio,
    }
    #endregion

    #region OfficeFileの派生クラス


    #region ExcelFile
    /// <summary>
    /// Moduleを内部に保持するExcelファイルを表すクラスです。
    /// </summary>
    public class ExcelFile:OfficeFile {



        #region フィールド(メンバ変数、プロパティ、イベント)
        /// <summary>
        /// ExportScriptNameを取得します。
        /// </summary>
        protected override string ExportScriptName => AppMain.g_AppMain.AppInfo.ExcelModuleExportScriptName;
        /// <summary>
        /// ImportScriptNameを取得します。
        /// </summary>
        protected override string ImportScriptName => AppMain.g_AppMain.AppInfo.ExcelModuleImportScriptName;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// ExcelFileオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public ExcelFile() :base(OfficeFileType.Excel){
        }
        #endregion

        #region イベントハンドラ

        #endregion

        /// <summary>
        /// TemporaryFolderの名前を生成します。
        /// </summary>
        /// <returns></returns>
        protected override string GenerateTemporaryFolderName() {
            return DateTime.Now.ToString($"yyyyMMdd_HHmmss_{Path.GetFileNameWithoutExtension(this.FileName)}");
        }
    }
    #endregion

    #endregion
}
