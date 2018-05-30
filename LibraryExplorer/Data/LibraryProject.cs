using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kucl.Xml;
using Kucl.Xml.XmlCfg;
using LibraryExplorer.Common;
using LibraryExplorer.Common.Request;

namespace LibraryExplorer.Data {

    /// <summary>
    /// 複数のライブラリ、複数のファイルをまとめたProjectを表します。
    /// </summary>
    public class LibraryProject: IUseConfig {


        #region フィールド(メンバ変数、プロパティ、イベント)
        private UseConfigHelper m_ConfigHelper;

        private FileSystemWatcher m_WorkspaceFolderWatcher;

        #region FolderClosedイベント
        /// <summary>
        /// Libraryが閉じられたときに発生するイベントです。
        /// </summary>
        public event EventHandler<EventArgs<Library>> FolderClosed;
        /// <summary>
        /// FolderClosedイベントを発生させます。
        /// </summary>
        /// <param name="e"></param>
        protected void OnFolderClosed(EventArgs<Library> e) {
            this.FolderClosed?.Invoke(this, e);
        } 
        #endregion

        #region FileClosedイベント
        /// <summary>
        /// OfficeFileが閉じられたときに発生するイベントです。
        /// </summary>
        public event EventHandler<EventArgs<OfficeFile>> FileClosed;
        /// <summary>
        /// FileClosedイベントを発生させます。
        /// </summary>
        /// <param name="e"></param>
        protected void OnFileClosed(EventArgs<OfficeFile> e) {
            this.FileClosed?.Invoke(this, e);
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



        #region Libraries
        private IList<Library> m_Libraries;
        /// <summary>
        /// Librariesを取得します。
        /// </summary>
        public IList<Library> Libraries {
            get {
                return this.m_Libraries;
            }
        }
        #endregion

        #region ExcelFiles
        private List<ExcelFile> m_ExcelFilesBackup;
        private ObservableCollection<OfficeFile> m_ExcelFiles;
        /// <summary>
        /// ExcelFilesを取得します。
        /// </summary>
        public IList<OfficeFile> ExcelFiles {
            get {
                return this.m_ExcelFiles;
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
        /// LibraryProjectオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public LibraryProject() {
            this.m_ConfigHelper = new UseConfigHelper(this.CreateDefaultConfig());
            
            this.m_Libraries = new List<Library>();

            this.m_ExcelFilesBackup = new List<ExcelFile>();
            this.m_ExcelFiles = new ObservableCollection<OfficeFile>();
            this.m_ExcelFiles.CollectionChanged += this.M_ExcelFiles_CollectionChanged;

            this.m_WorkspaceFolderWatcher = new FileSystemWatcher();
            this.StartWorkspaceFolderWatcher();

        }

        private void StartWorkspaceFolderWatcher() {
            this.m_WorkspaceFolderWatcher.IncludeSubdirectories = false;
            //this.m_TargetFileWatcher.SynchronizingObject = AppMain.g_AppMain.MainWindow;

            this.m_WorkspaceFolderWatcher.Path = AppMain.g_AppMain.WorkspaceFolderPath;
            this.m_WorkspaceFolderWatcher.Filter = "";
            this.m_WorkspaceFolderWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.DirectoryName;

            //イベントハンドラ登録
            this.m_WorkspaceFolderWatcher.Deleted += this.WorkspaceFolderWatcher_EventHandler;
            this.m_WorkspaceFolderWatcher.Renamed += this.WorkspaceFolderWatcher_EventHandler;

            //イベントを有効にして監視開始
            this.m_WorkspaceFolderWatcher.EnableRaisingEvents = true;

        }

        #endregion

        #region イベントハンドラ

        #region ExcelFiles_CollectionChanged
        private void M_ExcelFiles_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            //NotypyParentRequestのイベントハンドラを動的に登録/解除する
            switch (e.Action) {
                case NotifyCollectionChangedAction.Add:
                    foreach (ExcelFile file in e.NewItems) {
                        this.m_ExcelFilesBackup.Add(file);
                        this.ExcelFiles_OnAddItem(file);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (ExcelFile file in e.OldItems) {
                        this.m_ExcelFilesBackup.Remove(file);
                        this.ExcelFiles_OnRemoveItem(file);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (e.OldItems.Count != 0 && e.NewItems.Count != 0) {
                        ExcelFile oldFile = (ExcelFile)e.OldItems[0];
                        ExcelFile newFile = (ExcelFile)e.NewItems[0];
                        this.m_ExcelFilesBackup[e.OldStartingIndex] = newFile;
                        this.ExcelFiles_OnReplaceItem(e.OldStartingIndex, oldFile, newFile);
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    if (e.NewItems.Count != 0) {
                        ExcelFile file = (ExcelFile)e.NewItems[0];
                        this.m_ExcelFilesBackup.RemoveAt(e.OldStartingIndex);
                        this.m_ExcelFilesBackup.Insert(e.NewStartingIndex, file);
                        this.ExcelFiles_OnMoveItem(file, e.OldStartingIndex, e.NewStartingIndex);
                    }
                    //処理無し
                    break;
                case NotifyCollectionChangedAction.Reset:
                    this.ExcelFiles_OnClearItem(this.m_ExcelFilesBackup);
                    this.m_ExcelFilesBackup.Clear();
                    break;
            }
#if DEBUG
            System.Diagnostics.Debug.Assert(this.m_ExcelFiles.Count == this.m_ExcelFilesBackup.Count, "LibraryProject.ExcelFilesの内部整合性エラー");
#endif
        }
        #endregion

        #region ExcelFiles_CollectionChangedから呼び出される個別のメソッド

        #region OnAdd
        private void ExcelFiles_OnAddItem(ExcelFile file) {
            //ExcelFilesにアイテムが追加された
            if (file != null) {
                file.NotifyParentRequest += this.ExcelFile_NotifyParentRequest;
            }

            string debugMessage = $"LibraryProject : Add ExcelFile. path = {file.FileName}";
            AppMain.logger.Debug(debugMessage);
        }
        #endregion

        #region OnRemove
        private void ExcelFiles_OnRemoveItem(ExcelFile file) {
            //ExcelFilesからアイテムが削除された
            if (file != null) {
                file.NotifyParentRequest -= this.ExcelFile_NotifyParentRequest;
            }

            string debugMessage = $"LibraryProject : Remove ExcelFile. path = {file.FileName}";
            AppMain.logger.Debug(debugMessage);
        }
        #endregion

        #region OnReplace
        private void ExcelFiles_OnReplaceItem(int index, ExcelFile oldFile, ExcelFile newFile) {
            //ExcelFilesのアイテムが置き換えられた
            if (oldFile != null) {
                oldFile.NotifyParentRequest -= this.ExcelFile_NotifyParentRequest;
            }
            if (newFile != null) {
                newFile.NotifyParentRequest += this.ExcelFile_NotifyParentRequest;
            }


            string debugMessage = $"LibraryProjet : Replace ExcelFile. path = {oldFile.FileName}";
            AppMain.logger.Debug(debugMessage);
        }
        #endregion

        #region OnMove
        private void ExcelFiles_OnMoveItem(ExcelFile file, int oldIndex, int newIndex) {
            //ExcelFilesのアイテムが移動された


            string debugMessage = $"LibraryProject : Move ExcelFile. path = {file.FileName}";
            AppMain.logger.Debug(debugMessage);
        }
        #endregion

        #region OnClear
        private void ExcelFiles_OnClearItem(IList<ExcelFile> files) {
            //ExcelFilesのアイテムがクリアされた
            foreach (ExcelFile file in files) {
                if (file != null) {
                    file.NotifyParentRequest -= this.ExcelFile_NotifyParentRequest;
                }
            }

            string debugMessage = $"LibraryProject : Clear ExcelFiles. Count = {files.Count}";
            AppMain.logger.Debug(debugMessage);
        }
        #endregion

        #endregion

        #region ExcelFileからの通知

        //Fileからの通知を受け取って処理を行う。
        //NOTE:FileSystemWatcherは複数の通知を発生させることがあるため注意。(FileSystemへアクセスするアプリケーション依存のため、仕方ない。)
        //①Flagで記憶。ApplicationMessageQueueを使用して、IdleになってからFlagを確認し、上位へ通知すれば1回にまとまる？　FileSystem依存のため、Application_Idleのタイミングがうまく合わないかも。
        //　⇒Applicationとしては途中でIdle状態になってしまうため、不可
        //②Flagで記憶。初回の通知を受け取ったらタイマーを起動し、数秒後にFlagを確認し、上位へ通知すれば1回にまとまる？　何秒待てばよいか不明。
        //③表示の更新のみ数回であれば許容する。
        private void ExcelFile_NotifyParentRequest(object sender, RequestEventArgs e) {
            string logMessage = $"Receive event from {sender.GetType().Name}. EventType = {e.RequestData.GetType().Name}";
            AppMain.logger.Info(logMessage);

            switch (e.RequestData) {
                case NotifyWorkspaceFolderChangedRequestData folder_data:
                    //Workspaceフォルダが変更されたため、Refresh要求を出す
                    this.OnNotifyParentRequest(new RefreshDisplayRequestEventArgs(true));
                    break;
                case NotifyFileChangedRequestData file_data:
                    //Fileが変更されたため、再エクスポートを促す表示に変更する要求を出す（ExplorerTreeに対してRefresh要求）
                    this.OnNotifyParentRequest(new RefreshDisplayRequestEventArgs(true));
                    break;
            }
        }

        #endregion

        #region Workspaceの変更
        //エクスポートフォルダがDelete or Renameされた場合、対象のOfficeFileを特定し、警告表示を行う
        private void WorkspaceFolderWatcher_EventHandler(object sender, FileSystemEventArgs e) {
            string logMessage = $"LibraryProject : WorkspaceFolder Changed. Type = {e.ChangeType}, Path = {e.FullPath}, Name = {e.Name}";
            AppMain.logger.Debug(logMessage);

            //this.ExcelFiles.Where(x => !x.ExistWorkspaceFolder()).ToList().ForEach(file => {
            //    Console.WriteLine(file?.FileName);
            //});
            this.OnNotifyParentRequest(new RefreshDisplayRequestEventArgs(true));
        }

        #endregion

        #endregion

        #region CloseLibrary
        /// <summary>
        /// 指定したライブラリを閉じます。
        /// </summary>
        /// <param name="library"></param>
        public void CloseLibrary(Library library) {
            this.Libraries.Remove(library);
            //CloseFolderとイベントは共通
            this.OnFolderClosed(new EventArgs<Library>(library));
        }
        #endregion

        #region CloseFolder
        /// <summary>
        /// 指定したフォルダを閉じます。
        /// </summary>
        /// <param name="folder"></param>
        public void CloseFolder(LibraryFolder folder) {
            //選択されているフォルダパスを取得(LibraryFolder以外を選択しているとSelectedFolder==nullとなり、path==""となる)
            string path = folder?.Path ?? "";

            while (Directory.Exists(path)) {
                //LibraryのTargetFolderと一致したら、pathがLibraryのtopフォルダを示している
                Library lib = this.Libraries.FirstOrDefault(x => x.TargetFolder == path);
                if (lib != null) {
                    //ライブラリから削除
                    this.Libraries.Remove(lib);
                    //イベント発生
                    this.OnFolderClosed(new EventArgs<Library>(lib));
                    return;
                }
                //pathを親ディレクトリに変更
                path = Path.GetDirectoryName(path);
            }
        }
        #endregion

        #region CloseFile
        /// <summary>
        /// 指定したファイルを閉じます。
        /// deleteHistoryにtrueを指定した場合、履歴情報も同時に削除します。
        /// </summary>
        /// <param name="file"></param>
        /// <param name="deleteHistory"></param>
        public void CloseFile(OfficeFile file, bool deleteHistory = false) {
            string filename = file?.FileName ?? "";

            //OfficeFile file = this.m_Project.ExcelFiles.FirstOrDefault(x => x.FileName == filename);
            if (file != null) {
                this.ExcelFiles.Remove(file);

                //Closeメソッドを呼び出し、テンポラリフォルダを削除する
                file.Close(deleteHistory);

                //イベント発生
                this.OnFileClosed(new EventArgs<OfficeFile>(file));
            }
        }
        #endregion

        #region Reset
        /// <summary>
        /// LibraryProjectを初期状態に戻します。
        /// Workspaceフォルダ、Historyフォルダもすべて初期化します。
        /// </summary>
        public void Reset() {
            //ライブラリとファイルを閉じる
            this.Libraries.ToList().ForEach(lib => this.CloseLibrary(lib));
            this.ExcelFiles.ToList().ForEach(file => this.CloseFile(file, true));

            //残っているHistoryとWorkspaceをクリアする
            this.ResetDirectory(AppMain.g_AppMain.HistoryFolderPath);
            this.ResetDirectory(AppMain.g_AppMain.WorkspaceFolderPath);

            this.OnOutputLogRequest(new OutputLogRequestEventArgs("プロジェクトを初期化しました。"));
        }

        private void ResetDirectory(string path) {
            DirectoryInfo directory = new DirectoryInfo(path);
            foreach (FileInfo file in directory.GetFiles()) {
                file.Delete();
            }
            foreach (DirectoryInfo subDirectory in directory.GetDirectories()) {
                subDirectory.Delete(true);
            }
        }
        #endregion

        #region Config

        #region IUseConfig
        /// <summary>
        /// configとして保存するデフォルト設定を作成します。
        /// </summary>
        /// <returns></returns>
        public XmlConfigModel CreateDefaultConfig() {
            XmlConfigModel config = new XmlConfigModel();
            this.OnCreateDefaultConfig(config);
            return config;
        }
        /// <summary>
        /// 使用するConfigを取得または設定します。
        /// </summary>
        public XmlConfigModel Config {
            get {
                return this.m_ConfigHelper.Config;
            }
            set {
                this.m_ConfigHelper.Config = value;
            }
        }
        /// <summary>
        /// configを読み込んで適用します。
        /// </summary>
        /// <param name="value"></param>
        public void ApplyConfig(XmlConfigModel value) {
            this.m_ConfigHelper.Config = value;
            this.OnApplyConfig(value);
        }
        /// <summary>
        /// 現在の設定をconfigに反映します。
        /// </summary>
        /// <param name="config"></param>
        public void ReflectConfig(XmlConfigModel config) {
            this.OnReflectConfig(config);
        }
        /// <summary>
        /// configの値がデフォルト値かどうかを判定します。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsDefaultValue(string name, XmlContentsItem value) {
            return this.m_ConfigHelper.IsDefaultValue(name, value);
        }
        #endregion

        #region Configの適用と更新

        //Configの適用
        private void OnApplyConfig(XmlConfigModel config) {
            //LibraryFolders
            this.m_Libraries.Clear();
            int folder_count = this.m_ConfigHelper.GetIntValue("LibraryExplorer.project:Project.LibraryFolders.Count");
            for (int i = 0; i < folder_count; i++) {
                string path = this.m_ConfigHelper.GetStringValue($"LibraryExplorer.project:Project.LibraryFolders.Folders.{i + 1}");
                this.m_Libraries.Add(Library.FromFolder(path));
            }
            //OfficeFiles
            this.m_ExcelFiles.Clear();
            int file_count = this.m_ConfigHelper.GetIntValue("LibraryExplorer.project:Project.OfficeFiles.Count");
            for (int i = 0; i < file_count; i++) {
                string path = this.m_ConfigHelper.GetStringValue($"LibraryExplorer.project:Project.OfficeFiles.{i + 1}.Path");
                string exportPath = this.m_ConfigHelper.GetStringValue($"LibraryExplorer.project:Project.OfficeFiles.{i + 1}.ExportPath");
                string dateString = this.m_ConfigHelper.GetStringValue($"LibraryExplorer.project:Project.OfficeFiles.{i + 1}.ExportDate");
                DateTime? exportDate = (dateString != "" ? DateTime.Parse(dateString) : (DateTime?)null);

                OfficeFile file = new ExcelFile(path,exportPath) { ExportDate = exportDate };

                int backupCount = this.m_ConfigHelper.GetIntValue($"LibraryExplorer.project:Project.OfficeFiles.{i + 1}.BackupCount",0);
                for (int j = 0; j < backupCount; j++) {
                    string backupPath = this.m_ConfigHelper.GetStringValue($"LibraryExplorer.project:Project.OfficeFiles.{i + 1}.Backup.{j + 1}","");
                    file.BackupPathList.Add(backupPath);
                }

                this.m_ExcelFiles.Add(file);
            }

        }


        //Configの更新
        private void OnReflectConfig(XmlConfigModel config) {
            //LibraryFolders
            config.AddXmlContentsItem("LibraryExplorer.project:Project.LibraryFolders.Count", this.m_Libraries.Count);
            for (int i = 0; i < this.m_Libraries.Count; i++) {
                config.AddXmlContentsItem($"LibraryExplorer.project:Project.LibraryFolders.Folders.{i + 1}", this.m_Libraries[i].TargetFolder);
            }
            //OfficeFiels
            config.AddXmlContentsItem("LibraryExplorer.project:Project.OfficeFiles.Count", this.m_ExcelFiles.Count);
            for (int i = 0; i < this.m_ExcelFiles.Count; i++) {
                OfficeFile file = this.m_ExcelFiles[i];
                config.AddXmlContentsItem($"LibraryExplorer.project:Project.OfficeFiles.{i + 1}.Path", file.FileName);
                config.AddXmlContentsItem($"LibraryExplorer.project:Project.OfficeFiles.{i + 1}.ExportPath", file.WorkspaceFolder.Path);
                config.AddXmlContentsItem($"LibraryExplorer.project:Project.OfficeFiles.{i + 1}.ExportDate", file.ExportDate?.ToString() ?? "");
                config.AddXmlContentsItem($"LibraryExplorer.project:Project.OfficeFiles.{i + 1}.BackupCount", file.BackupPathList.Count);
                for (int j = 0; j < file.BackupPathList.Count; j++) {
                    config.AddXmlContentsItem($"LibraryExplorer.project:Project.OfficeFiles.{i + 1}.Backup.{j + 1}", file.BackupPathList[j]);
                }
            }


        }

        //既定のConfig
        private void OnCreateDefaultConfig(XmlConfigModel config) {
            //LibraryProject
            config.AddXmlContentsItem("LibraryExplorer.project:Project.LibraryFolders.Count", 0);
            config.AddXmlContentsItem("LibraryExplorer.project:Project.OfficeFiles.Count", 0);
        }


        #endregion

        #endregion

    }




    //TODO:LibraryFolderもOfficeFileも、ILibraryProjectItemを実装する。LibraryFileControllerも不要になるが、ExcelFileのOutputLogRequestの伝達方法は要検討
    //ただし、ExplorerTreeは、LibraryFolderとOfficeFileで明確に区別するべきなので、現状通りでよい。

    /// <summary>
    /// LibraryProjectを構成する要素が実装するインターフェースです。
    /// </summary>
    public interface ILibraryProjectItem {


        /// <summary>
        /// 表示用のパスを取得します。
        /// </summary>
        string DisplayPath {get;}
        /// <summary>
        /// 表示用の名前を取得します。
        /// </summary>
        string DisplayName {get;}

        /// <summary>
        /// パスを取得します。
        /// </summary>
        string Path {get;}
        /// <summary>
        /// 名前を取得します。
        /// </summary>
        string Name {get;}
        

    }

}
