﻿using System;
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

    //TODO:ExportDateを保持するように改造したので、起動時にOfficeFileのインスタンスが作られる。⇒WorkspaceFolderが不正な状態になることが無いか再検討(フォルダが無い、ファイルが無い等)
    //ファイルを手動で削除：問題なし。(残っているファイルのみ表示OK。再エクスポートにより復元)
    //フォルダが無い：空の一時フォルダが再作成される。再エクスポートで復元：要検討★⇒ExportPathも保存した。問題なし
    //再エクスポートするとExportDateが変更される。この時点では、以前の一時フォルダにエクスポートされるが、
    //再起動すると、ExportDateをもとにフォルダ名を生成するため、また空のフォルダが作られてしまう。：要修正★★⇒ExportPathも保存した。問題なし


    //TODO:NotifyParentRequestを誰が拾うのか検討する。(MainWindow?ExplorerTree?LibraryProject?)

    //TODO:履歴管理のため、Export完了したらhistoryフォルダに丸ごとコピーする
    //TODO:履歴管理のため、Import完了したらhistoryフォルダに丸ごとコピーする

    #region OfficeFile
    /// <summary>
    /// Moduleを内部に保持するOfficeファイルを表します。
    /// </summary>
    /// <remarks>NotifyParentRequestイベントにはNotifyFileChangedRequest/NotifyWorkspaceFolderChangedRequestが渡されます。</remarks>
    public abstract class OfficeFile:IOutputLogRequest {

        #region フィールド(メンバ変数、プロパティ、イベント)

        private FileSystemWatcher m_TargetFileWatcher;
        private FileSystemWatcher m_WorkspaceFolderWatcher;

        private ApplicationMessageQueue m_MessageQueue;

        private bool m_FolderChangedByExport;
        private FileSystemEventArgs m_EventArgsByExport;

        #region protected Exporting
        private bool m_Exporting;
        /// <summary>
        /// Expotingが変更された場合に発生するイベントです。
        /// </summary>
        protected event EventHandler<EventArgs<bool>> ExportingChanged;
        /// <summary>
        /// Expotingが変更された場合に呼び出されます。
        /// </summary>
        /// <param name="e">イベントパラメータ</param>
        protected void OnExportingChanged(EventArgs<bool> e) {
            this.ExportingChanged?.Invoke(this, e);
        }
        /// <summary>
        /// Expotingを取得、設定します。
        /// </summary>
        protected bool Exporting {
            get {
                return this.m_Exporting;
            }
            set {
                this.SetProperty(ref this.m_Exporting, value, ((oldValue) => {
                    if (this.ExportingChanged != null) {
                        this.OnExportingChanged(new EventArgs<bool>(oldValue));
                    }
                }));
            }
        }
        #endregion


        #region NotifyParentRequestイベント
        /// <summary>
        /// 親コントロールに対して要求を送信するイベントです。
        /// </summary>
        public event RequestEventHandler NotifyParentRequest;
        /// <summary>
        /// NotifyParentRequestイベントを発生させます。
        /// </summary>
        /// <param name="e"></param>
        protected void OnNotifyParentRequest(RequestEventArgs e) {
            this.NotifyParentRequest?.Invoke(this, e);
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

        #region ExportDate
        private DateTime? m_ExportDate;
        /// <summary>
        /// ExportDateが変更された場合に発生するイベントです。
        /// </summary>
        public event EventHandler<EventArgs<DateTime?>> ExportDateChanged;
        /// <summary>
        /// ExportDateが変更された場合に呼び出されます。
        /// </summary>
        /// <param name="e">イベントパラメータ</param>
        protected void OnExportDateChanged(EventArgs<DateTime?> e) {
            this.ExportDateChanged?.Invoke(this, e);
        }
        /// <summary>
        /// ExportDateを取得、設定します。
        /// </summary>
        public DateTime? ExportDate {
            get {
                return this.m_ExportDate;
            }
            set {
                this.SetProperty(ref this.m_ExportDate, value, ((oldValue) => {
                    if (this.ExportDateChanged != null) {
                        this.OnExportDateChanged(new EventArgs<DateTime?>(oldValue));
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

        #region WorkspaceFolderName
        private string m_WorkspaceFolderName;
        /// <summary>
        /// WorkspaceFolderNameを取得、設定します。
        /// </summary>
        public string WorkspaceFolderName {
            get {
                return this.m_WorkspaceFolderName;
            }
        }
        #endregion

        #region WorkspaceFolder
        private LibraryFolder m_WorkspaceFolder;
        /// <summary>
        /// WorkspaceFolderを取得します。
        /// </summary>
        public LibraryFolder WorkspaceFolder {
            get {
                return this.m_WorkspaceFolder;
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
            this.m_WorkspaceFolderName = "";
            this.m_ExportDate = null;

            this.m_TargetFileWatcher = new FileSystemWatcher();
            this.m_WorkspaceFolderWatcher = new FileSystemWatcher();

            this.m_MessageQueue = new ApplicationMessageQueue();
            this.m_MessageQueue.Start();

            this.FileNameChanged += this.OfficeFile_FileNameChanged;

            //Workspaceフォルダ監視のタイミング調整のため、Exportingプロパティを監視
            this.ExportingChanged += this.OfficeFile_ExportingChanged;
        }



        /// <summary>
        /// OfficeFileオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="fileType"></param>
        protected OfficeFile(string filename, OfficeFileType fileType) : this(fileType) {
            this.FileName = filename;
        }


        #endregion

        #region イベントハンドラ

        #region TargetFileWatcher
        /// <summary>
        /// Deleted,Renamedを上位に伝えるイベントです。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TargetFileWatcher_EventHandler(object sender, FileSystemEventArgs e) {
            string debugMessage = $"ExcelFile : TargetFile Changed. Type = {e.ChangeType}, Path = {e.FullPath}";
            AppMain.logger.Debug(debugMessage);

            this.OnNotifyParentRequest(new NotifyFileChangedRequestEventArgs(this, e));
        }

        #endregion

        #region WorkspaceFolderWatcher
        /// <summary>
        /// Changed,Created,Deleted,Renamedを上位に伝えるイベントです。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkspaceFolderWatcher_EventHandler(object sender, FileSystemEventArgs e) {
            string debugMessage = $"ExcelFile : WorkspaceFolder Changed. Type = {e.ChangeType}, Path = {e.FullPath}";
            AppMain.logger.Debug(debugMessage);

            if (!this.Exporting) {
                //エクスポート中以外の変更ならば上位に通知
                this.OnNotifyParentRequest(new NotifyWorkspaceFolderChangedRequestEventArgs(this.WorkspaceFolder, e));
            }
            else {
                //エクスポート中の変更は、いったん記憶して、後で上位に通知
                if (!this.m_FolderChangedByExport) {
                    this.m_EventArgsByExport = e;
                    this.m_FolderChangedByExport = true;
                }
            }
        }

        /// <summary>
        /// Exportingがfalseに切り替わったらエクスポート完了として、記憶していたフォルダの変更を上位に通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OfficeFile_ExportingChanged(object sender, EventArgs<bool> e) {
            if (!this.Exporting){
                if (this.m_FolderChangedByExport) {
                    //上位に通知して、変更記憶をクリア
                    this.OnNotifyParentRequest(new NotifyWorkspaceFolderChangedRequestEventArgs(this.WorkspaceFolder, this.m_EventArgsByExport));
                    this.m_FolderChangedByExport = false;
                    this.m_EventArgsByExport = null;
                }
            }
        }


        #endregion


        #region FileNameChanged
        private void OfficeFile_FileNameChanged(object sender, EventArgs<string> e) {
            //ファイル名が設定されたとき、ファイルが存在していたら監視を開始
            if (File.Exists(this.m_FileName)) {
                this.StartFileWatcher();
            }
            else {
                this.StopFileWatcher();
            }
        }

        #endregion

        #endregion

        #region StartScript
        /// <summary>
        /// 指定したパスを使用してスクリプトを実行します。
        /// </summary>
        /// <param name="scriptPath"></param>
        /// <returns></returns>
        protected Task StartScript(string scriptPath) {
            string foldername = this.m_WorkspaceFolderName;
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
        private async Task ExportAll() {
            string scriptPath = Path.Combine(AppMain.g_AppMain.ScriptFolderPath, this.ExportScriptName);

            this.m_ExportDate = DateTime.Now;

            //publicなExportAllメソッドでtrueにしているが、クラス内からの直接呼び出しが増えたときのために念押しでtrueにしておく
            this.Exporting = true;

            await this.StartScript(scriptPath).ConfigureAwait(false);

            //trueと同じく念押し
            this.Exporting = false;
        }

        /// <summary>
        /// スクリプトを起動して、対象ファイルから全てのモジュールをエクスポートします。
        /// スクリプトは非同期に実行されます。
        /// </summary>
        /// <param name="makeTempDir">trueを指定すると、MakeEmptyWorkspaceFolderを実行した後にエクスポートを行います。</param>
        /// <returns></returns>
        public async Task ExportAll(bool makeTempDir) {
            this.Exporting = true;

            if (makeTempDir) {
                this.MakeEmptyWorkspaceFolder();
            }

            await this.ExportAll();

            this.Exporting = false;

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
            this.DeleteWorkspaceFolder();
        }
        #endregion

        #region FileSystemWatcher関連
        private void StartFileWatcher() {
            
            this.m_TargetFileWatcher.IncludeSubdirectories = false;
            //this.m_TargetFileWatcher.SynchronizingObject = AppMain.g_AppMain.MainWindow;

            this.m_TargetFileWatcher.Path = Path.GetDirectoryName(this.m_FileName);
            this.m_TargetFileWatcher.Filter = Path.GetFileName(this.m_FileName);
            this.m_TargetFileWatcher.NotifyFilter = NotifyFilters.LastAccess| NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName;

            //イベントハンドラ登録
            this.m_TargetFileWatcher.Changed += this.TargetFileWatcher_EventHandler;
            this.m_TargetFileWatcher.Deleted += this.TargetFileWatcher_EventHandler;
            this.m_TargetFileWatcher.Renamed += this.TargetFileWatcher_EventHandler;

            //イベントを有効にして監視開始
            this.m_TargetFileWatcher.EnableRaisingEvents = true;
        }


        private void StopFileWatcher() {
            this.m_TargetFileWatcher.EnableRaisingEvents = false;
        }


        private void StartFolderWatcher() {
            this.m_WorkspaceFolderWatcher.IncludeSubdirectories = false;
            //this.m_WorkspaceFolderWatcher.SynchronizingObject = AppMain.g_AppMain.MainWindow;

            this.m_WorkspaceFolderWatcher.Path = this.WorkspaceFolderName;
            this.m_WorkspaceFolderWatcher.Filter = "";
            this.m_WorkspaceFolderWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            //イベントハンドラ登録
            this.m_WorkspaceFolderWatcher.Changed += this.WorkspaceFolderWatcher_EventHandler;
            this.m_WorkspaceFolderWatcher.Created += this.WorkspaceFolderWatcher_EventHandler;
            this.m_WorkspaceFolderWatcher.Deleted += this.WorkspaceFolderWatcher_EventHandler;
            this.m_WorkspaceFolderWatcher.Renamed += this.WorkspaceFolderWatcher_EventHandler;

            //イベントを有効にして監視開始
            this.m_WorkspaceFolderWatcher.EnableRaisingEvents = true;
        }


        private void StopFolderWatcher() {
            this.m_WorkspaceFolderWatcher.EnableRaisingEvents = false;
        }
        #endregion

        #region WorkspaceFolder関連

        #region ClearFilesInWorkspaceFolder
        /// <summary>
        /// 一時フォルダ内のファイル、フォルダをすべて削除します。
        /// </summary>
        public void ClearWorkspaceFolder() {
            DirectoryInfo directory = new DirectoryInfo(this.m_WorkspaceFolderName);
            if (!directory.Exists) {
                return;
            }
            //ファイルの削除
            this.DeleteAllFilesInWorkspaceFolder(directory);
            //フォルダの削除
            this.DeleteAllDirectoriesInWorkspaceFolder(directory);
        }

        /// <summary>
        /// 一時フォルダ内のすべてのファイルを削除します。
        /// </summary>
        /// <param name="directory"></param>
        private void DeleteAllFilesInWorkspaceFolder(DirectoryInfo directory) {
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
        private void DeleteAllDirectoriesInWorkspaceFolder(DirectoryInfo directory) {
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

        #region CreateWorkspaceFolder
        /// <summary>
        /// 既定のフォルダ名を使用して、一時フォルダを作成します。
        /// 作成されたフォルダのパスはこのインスタンスが保持します。
        /// </summary>
        public void CreateWorkspaceFolder() {
            this.CreateWorkspaceFolder(this.GetWorkspaceFolderName());
        }
        /// <summary>
        /// 指定されたフォルダ名で一時フォルダを作成します。
        /// 作成されたフォルダのパスはこのインスタンスが保持します。
        /// </summary>
        /// <param name="temporaryFolderPath"></param>
        public void CreateWorkspaceFolder(string temporaryFolderPath) {
            if (temporaryFolderPath == "") {
                //空文字列の場合は無効なパスとみなし、引数なしのCreateWorkspaceFolderメソッド経由で再度呼び出される
                this.CreateWorkspaceFolder();
            }
            else {
                this.m_WorkspaceFolderName = temporaryFolderPath;
                this._CreateWorkspaceFolder(this.m_WorkspaceFolderName);
            }
        }

        /// <summary>
        /// GenerateWorkspaceFolderNameメソッドで得られた名前と、アプリケーションで保持するWorkspaceFolderPathを合成し、一時フォルダのフルパスを取得します。
        /// </summary>
        /// <returns></returns>
        protected virtual string GetWorkspaceFolderName() {
            string tempFolderName = this.GenerateWorkspaceFolderName();
            return Path.Combine(AppMain.g_AppMain.WorkspaceFolderPath, tempFolderName);
        }
        /// <summary>
        /// 一時フォルダのフォルダ名を生成します。
        /// date.ToString($"yyyyMMdd_hhmmss")を返します。
        /// dateの既定値はnullです。dateがnullの場合、DateTime.Nowを使用します。
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        protected virtual string GenerateWorkspaceFolderName(DateTime? date = null) {
            return (date ?? (DateTime.Now)).ToString($"yyyyMMdd_HHmmss");
        }

        /// <summary>
        /// パスを指定して、一時フォルダを作成します。
        /// </summary>
        /// <param name="path"></param>
        protected void _CreateWorkspaceFolder(string path) {
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
            //作成されたWorkspaceFolderを表すLibraryFolderオブジェクトも作成しておく
            this.m_WorkspaceFolder = new LibraryFolder() { Path = this.m_WorkspaceFolderName };

            //一時フォルダを作成したので、フォルダ監視を始める
            this.StartFolderWatcher();
        }

        #endregion

        #region DeleteWorkspaceFolder
        /// <summary>
        /// このインスタンスが保持するパスを使用して、一時フォルダを削除します。
        /// </summary>
        public void DeleteWorkspaceFolder() {
            this.DeleteWorkspaceFolder(this.m_WorkspaceFolderName);
        }
        /// <summary>
        /// パスを指定して、一時フォルダを削除します。
        /// </summary>
        /// <param name="path"></param>
        protected void DeleteWorkspaceFolder(string path) {
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

            //一時フォルダを削除したので、フォルダ監視終了
            this.StopFolderWatcher();
        }
        #endregion

        #region ExistWorkspaceFolder
        /// <summary>
        /// テンポラリフォルダが存在するかどうかを返します。
        /// </summary>
        /// <returns></returns>
        public bool ExistWorkspaceFolder() {
            return Directory.Exists(this.m_WorkspaceFolderName);
        }

        #endregion

        #region MakeEmptyWorkspaceFolder
        /// <summary>
        /// 空のテンポラリフォルダを用意します。
        /// </summary>
        public void MakeEmptyWorkspaceFolder() {
            if (this.ExistWorkspaceFolder()) {
                this.ClearWorkspaceFolder();
            }
            else {
                this.CreateWorkspaceFolder();
            }
        }
        #endregion

        #endregion

        #region historyフォルダ関連

        

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
        /// WorkspaceFolderの名前を生成します。
        /// </summary>
        /// <returns></returns>
        protected override string GenerateWorkspaceFolderName(DateTime? date = null) {
            return (date ?? (DateTime.Now)).ToString($"yyyyMMdd_HHmmss_{Path.GetFileNameWithoutExtension(this.FileName)}");
        }
    }
    #endregion

    #endregion
}
