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
    /// <remarks>NotifyParentRequestイベントにはNotifyFileChangedRequest/NotifyWorkspaceFolderChangedRequestが渡されます。</remarks>
    public abstract class OfficeFile:IOutputLogRequest {

        #region フィールド(メンバ変数、プロパティ、イベント)

        #region 内部変数
        private FileSystemWatcher m_TargetFileWatcher;
        private FileSystemWatcher m_WorkspaceFolderWatcher;

        private bool m_FolderChangedByExport;
        private FileSystemEventArgs m_EventArgsByExport;

        private bool m_FileChangedByImport;
        private FileSystemEventArgs m_EventArgsByImport;

        private DateTime m_LatestFileUpdateTime;

        #endregion

        #region protectedなプロパティ
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

        #region protected Importing
        private bool m_Importing;
        /// <summary>
        /// Importingが変更された場合に発生するイベントです。
        /// </summary>
        protected event EventHandler<EventArgs<bool>> ImportingChanged;
        /// <summary>
        /// Importingが変更された場合に呼び出されます。
        /// </summary>
        /// <param name="e">イベントパラメータ</param>
        protected void OnImportingChanged(EventArgs<bool> e) {
            this.ImportingChanged?.Invoke(this, e);
        }
        /// <summary>
        /// Importingを取得、設定します。
        /// </summary>
        protected bool Importing {
            get {
                return this.m_Importing;
            }
            set {
                this.SetProperty(ref this.m_Importing, value, ((oldValue) => {
                    if (this.ImportingChanged != null) {
                        this.OnImportingChanged(new EventArgs<bool>(oldValue));
                    }
                }));
            }
        }
        #endregion


        #endregion

        #region イベント
        
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

        #endregion

        #region 定数扱いのプロパティ
        
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
        /// ImportScriptNameを取得します。
        /// </summary>
        protected abstract string ImportScriptName {
            get;
        }
        #endregion

        #endregion


        #region プロパティ

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


        #region BackupPathList
        private List<string> m_BackupPathList;
        /// <summary>
        /// BackupPathListを取得、設定します。
        /// </summary>
        public List<string> BackupPathList {
            get {
                return this.m_BackupPathList;
            }
            set {
                this.m_BackupPathList = value;
            }
        }
        #endregion


        #region Status
        /// <summary>
        /// Statusを取得します。
        /// </summary>
        public OfficeFileStatus Status {
            get {
                OfficeFileStatus st= OfficeFileStatus.Unknown;
                if (!this.Exist) {
                    st |= OfficeFileStatus.FileNotFound;
                }
                if (!this.ExistWorkspaceFolder()) {
                    st |= OfficeFileStatus.WorkspaceNotFound;
                }
                if (this.RequiredReExport) {
                    st |= OfficeFileStatus.UpdatedFile;
                }
                if (this.ModifiedWorkspace) {
                    st |= OfficeFileStatus.UpdatedWorkspace;
                }

                //どの異常も発生していない場合、Normal
                if (st == OfficeFileStatus.Unknown) {
                    st |= OfficeFileStatus.Normal;
                }
                return st;
            }
        }
        #endregion

        #region StatusMessage
        /// <summary>
        /// StatusMessageを取得します。
        /// </summary>
        public string StatusMessage {
            get {
                OfficeFileStatus st = this.Status;
                if (st.HasFlag(OfficeFileStatus.FileNotFound)) {
                    return "ファイルが存在しません。不要なファイルであればファイルを閉じてください。";
                }
                if (st.HasFlag(OfficeFileStatus.WorkspaceNotFound)) {
                    return "エクスポートフォルダが存在しません。再エクスポートしてください。";
                }
                if (st.HasFlag(OfficeFileStatus.UpdatedFile)) {
                    return "ファイルが更新されています。再エクスポートしてください。";
                }
                if (st.HasFlag(OfficeFileStatus.UpdatedWorkspace)) {
                    return "Workspaceフォルダのファイルが更新されています。反映する場合、インポートしてください。";
                }
                return "";
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

        #region RequiredReExport
        /// <summary>
        /// 再エクスポートが必要かどうかを返します。
        /// エクスポート後にファイルが更新された場合にtrueを返します。
        /// </summary>
        public bool RequiredReExport {
            get {
                if (this.m_ExportDate == null) {
                    return true;
                }
                TimeSpan time = this.m_ExportDate.Value - this.UpdateDate;
                double deltaSeconds = Math.Abs(time.TotalSeconds);
                if (deltaSeconds <= 2) {
                    //HACK:2秒は決め打ち
                    //一定秒数以内は同一時刻とみなす
                    return false;
                }
                return this.m_ExportDate < this.UpdateDate;
            }
        }
        #endregion

        #region ModifiedWorkspace
        /// <summary>
        /// エクスポート後にWorkspaceフォルダ内のファイルが変更されたかどうかを取得します。
        /// </summary>
        public bool ModifiedWorkspace {
            get {
                if (this.m_ExportDate == null) {
                    return true;
                }
                TimeSpan time = this.m_ExportDate.Value - this.m_LatestFileUpdateTime;
                double deltaSeconds = Math.Abs(time.TotalSeconds);
                if (deltaSeconds <= 2) {
                    //HACK:2秒は決め打ち
                    //一定秒数以内は同一時刻とみなす
                    return false;
                }
                return this.m_ExportDate < this.m_LatestFileUpdateTime;
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


        #region CanExport
        /// <summary>
        /// エクスポート可能かどうかを取得します。
        /// </summary>
        public bool CanExport {
            get {
                return !this.m_Exporting && !this.m_Importing;
            }
        }
        #endregion

        #region CanImport
        /// <summary>
        /// インポート可能かどうかを取得します。
        /// </summary>
        public bool CanImport {
            get {
                return !this.m_Exporting && !this.m_Importing;
            }
        }
        #endregion


        #region WorkspaceFolder
        private WorkFolder m_WorkspaceFolder;
        /// <summary>
        /// WorkspaceFolderを取得します。
        /// </summary>
        public WorkFolder WorkspaceFolder {
            get {
                return this.m_WorkspaceFolder;
            }
        }
        #endregion

        #region WorkspaceFolderPath

        /// <summary>
        /// WorkspaceFolderPathを取得、設定します。
        /// </summary>
        public string WorkspaceFolderPath {
            get {
                return this.m_WorkspaceFolder.Path;
            }
            set {
                this.m_WorkspaceFolder.Path = value;
            }
        }
        #endregion


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
            this.m_ExportDate = null;

            this.m_BackupPathList = new List<string>();

            //WorkspacelFolder
            this.m_WorkspaceFolder = new WorkFolder {
                BaseFolderPath = AppMain.g_AppMain.WorkspaceFolderPath,
                DeleteAtClose = false
            };
            this.m_WorkspaceFolder.PathChanged += this.M_WorkspaceFolder_PathChanged;
            this.m_WorkspaceFolder.BeforeDeleteFolder += this.M_WorkspaceFolder_BeforeDeleteFolder;
            this.m_WorkspaceFolder.AfterCreateFolder += this.M_WorkspaceFolder_AfterCreateFolder;

            //FileSystemWatcherのインスタンスはコンストラクタで生成するが、監視は別タイミングで開始する
            this.m_TargetFileWatcher = new FileSystemWatcher();
            this.m_WorkspaceFolderWatcher = new FileSystemWatcher();

            //FileNameの変更タイミングでファイル監視を開始/終了する
            this.FileNameChanged += this.OfficeFile_FileNameChanged;

            //Workspaceフォルダ監視のタイミング調整のため、Exportingプロパティを監視
            this.ExportingChanged += this.OfficeFile_ExportingChanged;
            //TargetFileの監視タイミング調整のため、Importingプロパティを監視
            this.ImportingChanged += this.OfficeFile_ImportingChanged;
        }

        /// <summary>
        /// OfficeFileオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="fileType"></param>
        protected OfficeFile(string filename, OfficeFileType fileType) : this(fileType) {
            this.FileName = filename;
        }

        /// <summary>
        /// OfficeFileオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="workspaceFolderPath"></param>
        /// <param name="fileType"></param>
        protected OfficeFile(string filename, string workspaceFolderPath, OfficeFileType fileType) : this(filename, fileType) {
            this.m_WorkspaceFolder.Path = workspaceFolderPath;

            //コンストラクタでWorkspaceFolderのパスを指定された場合、フォルダが既に存在しているため、更新日時のチェックを行う。
            //以降の変更はFileSystemWatcherでキャッチする。
            this.CheckLatestFileUpdateTime();
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
            string logMessage = $"ExcelFile : TargetFile Changed. Type = {e.ChangeType}, Path = {e.FullPath}";
            AppMain.logger.Debug(logMessage);

            if (!this.Importing) {
                this.OnNotifyParentRequest(new NotifyFileChangedRequestEventArgs(this, e));
            }
            else {
                if (!this.m_FileChangedByImport) {
                    this.m_FileChangedByImport = true;
                    this.m_EventArgsByImport = e;
                }
            }
        }
        private void OfficeFile_ImportingChanged(object sender, EventArgs<bool> e) {
            if (!this.Importing) {
                if (this.m_FileChangedByImport) {
                    //上位に通知して、変更記憶をクリア
                    this.OnNotifyParentRequest(new NotifyFileChangedRequestEventArgs(this, this.m_EventArgsByImport));
                    this.m_FileChangedByImport = false;
                    this.m_EventArgsByImport = null;
                }
            }
        }

        #endregion

        #region WorkspaceFolderWatcher
        /// <summary>
        /// Changed,Created,Deleted,Renamedを上位に伝えるイベントです。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkspaceFolderWatcher_EventHandler(object sender, FileSystemEventArgs e) {
            string logMessage = $"ExcelFile : WorkspaceFolder Changed. Type = {e.ChangeType}, Path = {e.FullPath}";
            AppMain.logger.Debug(logMessage);

            if (!this.Exporting) {
                //ファイルの更新日付をチェック
                this.CheckLatestFileUpdateTime();

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
                    //ファイルの更新日付をチェック
                    this.CheckLatestFileUpdateTime();

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

        #region WorkspaceFolder
        private void M_WorkspaceFolder_PathChanged(object sender, LibraryFolder.EventArgs<string> e) {
            //既に存在するパスが指定された場合、フォルダ監視を始める
            //(新たにフォルダを作成しないケース)
            if (Directory.Exists(this.m_WorkspaceFolder.Path)) {
                this.StartFolderWatcher();
            }
        }

        private void M_WorkspaceFolder_AfterCreateFolder(object sender, EventArgs e) {
            //一時フォルダを作成したので、フォルダ監視を始める
            this.StartFolderWatcher();
        }

        private void M_WorkspaceFolder_BeforeDeleteFolder(object sender, EventArgs e) {
            //フォルダを削除する前にフォルダ監視を終了する。
            this.StopFolderWatcher();
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
            string foldername = this.m_WorkspaceFolder.Path;
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
        /// <param name="makeTempDir">trueを指定すると、MakeEmptyWorkspaceFolderを実行した後にエクスポートを行います。</param>
        /// <returns></returns>
        public async Task ExportAll(bool makeTempDir) {
            string scriptPath = Path.Combine(AppMain.g_AppMain.ScriptFolderPath, this.ExportScriptName);

            //-------------------------------------------------------
            this.Exporting = true;
            //エクスポート前のバックアップ
            this.CopyToHistory($"{DateTime.Now.ToString("yyyyMMdd_HHmmss")}_BeforeExport_{Path.GetFileName(this.FileName)}");

            //フォルダを空にする
            if (makeTempDir) {
                this.MakeEmptyWorkspaceFolder();
            }

            //エクスポート用のスクリプトを起動
            await this.StartScript(scriptPath).ConfigureAwait(false);

            this.m_ExportDate = DateTime.Now;

            //エクスポート後のバックアップ
            this.CopyToHistory($"{DateTime.Now.ToString("yyyyMMdd_HHmmss")}_AfterExport_{Path.GetFileName(this.FileName)}");

            this.Exporting = false;
            //-------------------------------------------------------
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

            //-------------------------------------------------------
            this.Importing = true;

            await this.StartScript(scriptPath).ConfigureAwait(false);

            if (!this.m_HasError) {
                //エラー無く完了した場合、エクスポートしたデータ＝ファイルのデータになるため、日付を更新する
                this.m_ExportDate = DateTime.Now;
            }

            //インポート後のバックアップ
            this.CopyToHistory($"{DateTime.Now.ToString("yyyyMMdd_HHmmss")}_AfterImport_{Path.GetFileName(this.FileName)}");

            this.Importing = false;
            //-------------------------------------------------------
        }
        #endregion

        #region Close
        /// <summary>
        /// このファイルを閉じます。
        /// 既定ではテンポラリフォルダを削除します。
        /// </summary>
        public virtual void Close(bool deleteHistory = false) {
            this.DeleteWorkspaceFolder();

            //履歴の削除
            if (deleteHistory) {
                this.DeleteAllHistory();
            }
        }
        #endregion

        #region CheckLatestFileUpdateDate
        /// <summary>
        /// 起動時にWorkspaceフォルダをチェックし、ファイルの更新日付のうち最も新しい日付を確認する。
        /// この日付がエクスポート日付以降の場合、手動での変更が行われたことを示す
        /// </summary>
        private void CheckLatestFileUpdateTime() {
            DirectoryInfo dir = new DirectoryInfo(this.WorkspaceFolderPath);
            if (dir.Exists) {
                //DateTime.MinValueを加えて、Maxを取ることで、ファイル数0の場合に対応する
                this.m_LatestFileUpdateTime = dir.GetFiles().Select(f => f.LastWriteTime).Concat(new DateTime[] { DateTime.MinValue }).Max();
            }
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

            this.m_WorkspaceFolderWatcher.Path = this.WorkspaceFolder.Path;
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
            this.m_WorkspaceFolder.Clear();
        }

        #endregion

        #region CreateWorkspaceFolder
        /// <summary>
        /// 既定のフォルダ名を使用して、一時フォルダを作成します。
        /// 作成されたフォルダのパスはこのインスタンスが保持します。
        /// </summary>
        public void CreateWorkspaceFolder() {
            this.m_WorkspaceFolder.Create();
        }
        /// <summary>
        /// 指定されたフォルダ名で一時フォルダを作成します。
        /// 作成されたフォルダのパスはこのインスタンスが保持します。
        /// </summary>
        /// <param name="workspaceFolderPath"></param>
        public void CreateWorkspaceFolder(string workspaceFolderPath) {
            this.m_WorkspaceFolder.Create(workspaceFolderPath);
        }

        #endregion

        #region DeleteWorkspaceFolder
        /// <summary>
        /// このインスタンスが保持するパスを使用して、一時フォルダを削除します。
        /// </summary>
        public void DeleteWorkspaceFolder() {
            this.m_WorkspaceFolder.Delete();
        }
        #endregion

        #region ExistWorkspaceFolder
        /// <summary>
        /// Workspaceフォルダが存在するかどうかを返します。
        /// </summary>
        /// <returns></returns>
        public bool ExistWorkspaceFolder() {
            return this.m_WorkspaceFolder.Exist();
        }

        #endregion

        #region MakeEmptyWorkspaceFolder
        /// <summary>
        /// 空のテンポラリフォルダを用意します。
        /// </summary>
        public void MakeEmptyWorkspaceFolder() {
            this.m_WorkspaceFolder.MakeEmptyFolder();
        }
        #endregion

        #endregion

        #region historyフォルダ関連

        #region 履歴の保存
        private void CopyToHistory(string folderName) {
            bool copySuccess = this.m_WorkspaceFolder.CopyFolder(AppMain.g_AppMain.HistoryFolderPath, folderName);

            if (copySuccess) {
                this.m_BackupPathList.Add(folderName);
            }
        } 
        #endregion

        #region 履歴の削除
        /// <summary>
        /// 指定した履歴情報を削除します。
        /// </summary>
        /// <param name="folderName"></param>
        public void DeleteHistory(string folderName) {
            string path = Path.Combine(AppMain.g_AppMain.HistoryFolderPath, folderName);
            if (Directory.Exists(path)) {
                Directory.Delete(path, true);
            }
            if (this.m_BackupPathList.Contains(folderName)) {
                this.m_BackupPathList.Remove(folderName);
            }
        }

        /// <summary>
        /// 全ての履歴情報を削除します。
        /// </summary>
        private void DeleteAllHistory() {
            for (int i = this.m_BackupPathList.Count - 1; i >= 0; i--) {
                this.DeleteHistory(this.m_BackupPathList[i]);
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
            this.Initialize();
        }

        /// <summary>
        /// ExcelFileオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="workspaceFolderPath"></param>
        public ExcelFile(string filename, string workspaceFolderPath) : base(filename,workspaceFolderPath,OfficeFileType.Excel) {
            this.Initialize();

            this.FileName = filename;
            this.WorkspaceFolderPath = workspaceFolderPath;
        }

        private void Initialize() {
            this.FileNameChanged += this.ExcelFile_FileNameChanged;
            this.WorkspaceFolder.FolderNamePrefix = "";
            this.WorkspaceFolder.FolderNameFormatString = "yyyyMMdd_HHmmss";
            this.WorkspaceFolder.FolderNameSuffix = $"_{Path.GetFileNameWithoutExtension(this.FileName)}";

        }
        #endregion

        #region イベントハンドラ
        private void ExcelFile_FileNameChanged(object sender, EventArgs<string> e) {
            //FileNameをWorkspaceフォルダ名に埋め込む
            //Initializeで設定すると、filename指定のコンストラクタ以外ではブランクになるため、イベントで処理する。
            this.WorkspaceFolder.FolderNameSuffix = $"_{Path.GetFileNameWithoutExtension(this.FileName)}";
        }

        #endregion

    }
    #endregion

    #endregion


    #region OfficeFileStatus
    /// <summary>
    /// OfficeFileの状態を表す列挙型です。
    /// </summary>
    public enum OfficeFileStatus {
        /// <summary>
        /// 不明な状態を表します。
        /// 通常、この状態は存在しません。
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// OfficeFileが正常な状態であることを表します。
        /// </summary>
        Normal = 1,
        /// <summary>
        /// OfficeFileが見つからない状態です。
        /// </summary>
        FileNotFound = 2,
        /// <summary>
        /// Workspaceフォルダが見つからない状態です。
        /// </summary>
        WorkspaceNotFound = 4,
        /// <summary>
        /// エクスポートした後にOfficeFileが更新された状態です。
        /// </summary>
        UpdatedFile = 8,
        /// <summary>
        /// エクスポートした後にWorkspaceフォルダが更新された状態です。
        /// </summary>
        UpdatedWorkspace = 16
    } 
    #endregion
}
