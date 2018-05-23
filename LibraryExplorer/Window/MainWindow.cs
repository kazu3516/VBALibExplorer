using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kucl.Cmd;
using Kucl.Forms.Controls;
using LibraryExplorer.Control;
using LibraryExplorer.Common;
using LibraryExplorer.Common.Request;
using LibraryExplorer.Data;
using LibraryExplorer.Window.DockWindow;
using LibraryExplorer.Window.Dialog;
using WeifenLuo.WinFormsUI.Docking;
using WeifenLuo.WinFormsUI.ThemeVS2015;
namespace LibraryExplorer.Window {

    /// <summary>
    /// アプリケーションのメインウィンドウを表すクラスです。
    /// </summary>
    public partial class MainWindow : Form {


        //TODO:フォルダ操作機能の実装（新規作成、名前の変更、コピー、切り取り、貼り付け、削除）
        //TODO:LibraryFileの操作機能の実装（コピー、切り取り、貼り付け、削除、フォルダ移動）

        //TODO:MainMenuの実装
        //TODO:ToolBarの実装
        //TODO:ヘルプファイルの作成

        //TODO:検索機能の実装

        //TODO:Excelファイルを指定し、ライブラリへの取り込み機能を作成する。(テンポラリフォルダからのコピー)
        //⇒ドラッグ＆ドロップでファイルのエクスポート/インポートを実施(ProjectHandlingのイメージ？)
        //⇒バックアップを作成し、元に戻せるようにする


        //TODO:Excel以外のOfficeファイルの対応(ライブラリが別になり、使用頻度も低いので優先度低。Exportのみ対応でも可)

        //TODO:個人用マクロブック用のエクスポート機能を検討


        #region フィールド(メンバ変数、プロパティ、イベント)
        //目視スクロールのために、大項目の区切りに■を置いておく
        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

        private DockPanel dockPanel;
        private DockContent m_ActiveDocument;

        private LibraryFileListWindow m_LibraryFileListWindow;
        private ExplorerTreeWindow m_ExplorerTreeWindow;
        private PreviewWindow m_PreviewWindow;
        private OutputLogWindow m_OutputLogWindow;

        private List<ExcelFileModuleListWindow> m_ExcelFileModuleListWindows;

        private OptionDialog m_OptionDialog;
        private AboutBox m_AboutBox;
        private LibraryPropertyDialog m_LibraryPropertyDialog;
        

        private bool m_FirstShowWindow;


        #region Project
        private LibraryProject m_Project;
        /// <summary>
        /// Projectを取得します。
        /// </summary>
        public LibraryProject Project {
            get {
                return this.m_Project;
            }
        }
        #endregion

        #region SelectedFolder
        private LibraryFolder m_SelectedFolder;
        /// <summary>
        /// SelectedFolderが変更された場合に発生するイベントです。
        /// </summary>
        public event EventHandler<EventArgs<LibraryFolder>> SelectedFolderChanged;
        /// <summary>
        /// SelectedFolderが変更された場合に呼び出されます。
        /// </summary>
        /// <param name="e">イベントパラメータ</param>
        protected void OnSelectedFolderChanged(EventArgs<LibraryFolder> e) {
            this.SelectedFolderChanged?.Invoke(this, e);
        }
        /// <summary>
        /// SelectedFolderを取得、設定します。
        /// </summary>
        public LibraryFolder SelectedFolder {
            get {
                return this.m_SelectedFolder;
            }
            set {
                this.SetProperty(ref this.m_SelectedFolder, value, ((oldValue) => {
                    if (this.SelectedFolderChanged != null) {
                        this.OnSelectedFolderChanged(new EventArgs<LibraryFolder>(oldValue));
                    }
                }));
            }
        }
        #endregion

        #region SelectedLibraryFile
        private LibraryFile m_SelectedLibraryFile;
        /// <summary>
        /// SelectedFileが変更された場合に発生するイベントです。
        /// </summary>
        public event EventHandler<EventArgs<LibraryFile>> SelectedLibraryFileChanged;
        /// <summary>
        /// SelectedFileが変更された場合に呼び出されます。
        /// </summary>
        /// <param name="e">イベントパラメータ</param>
        protected void OnSelectedLibraryFileChanged(EventArgs<LibraryFile> e) {
            this.SelectedLibraryFileChanged?.Invoke(this, e);
        }
        /// <summary>
        /// SelectedFileを取得、設定します。
        /// </summary>
        public LibraryFile SelectedLibraryFile {
            get {
                return this.m_SelectedLibraryFile;
            }
            set {
                this.SetProperty(ref this.m_SelectedLibraryFile, value, ((oldValue) => {
                    if (this.SelectedLibraryFileChanged != null) {
                        this.OnSelectedLibraryFileChanged(new EventArgs<LibraryFile>(oldValue));
                    }
                }));
            }
        }
        #endregion

        #region SelectedOfficeFile
        private OfficeFile m_SelectedOfficeFile;
        /// <summary>
        /// SelectedOfficeFileが変更された場合に発生するイベントです。
        /// </summary>
        public event EventHandler<EventArgs<OfficeFile>> SelectedOfficeFileChanged;
        /// <summary>
        /// SelectedOfficeFileが変更された場合に呼び出されます。
        /// </summary>
        /// <param name="e">イベントパラメータ</param>
        protected void OnSelectedOfficeFileChanged(EventArgs<OfficeFile> e) {
            this.SelectedOfficeFileChanged?.Invoke(this, e);
        }
        /// <summary>
        /// SelectedOfficeFileを取得、設定します。
        /// </summary>
        public OfficeFile SelectedOfficeFile {
            get {
                return this.m_SelectedOfficeFile;
            }
            set {
                this.SetProperty(ref this.m_SelectedOfficeFile, value, ((oldValue) => {
                    if (this.SelectedOfficeFileChanged != null) {
                        this.OnSelectedOfficeFileChanged(new EventArgs<OfficeFile>(oldValue));
                    }
                }));
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
        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

        /// <summary>
        /// MainWindowオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public MainWindow() {
            InitializeComponent();
            this.Initialize();
        }


        private void Initialize() {
#if !DEBUG
            this.デバッグメニューDToolStripMenuItem.Visible = false;
#endif

            //
            // DockPanel
            //
            this.dockPanel = new DockPanel();
            this.dockPanel.Dock = DockStyle.Fill;
            this.dockPanel.DocumentStyle = DocumentStyle.DockingWindow;
            this.dockPanel.Theme = new VS2015DarkTheme();
            this.dockPanel.ActiveDocumentChanged += this.DockPanel_ActiveDocumentChanged;

            this.toolStripContainer1.ContentPanel.Controls.Add(this.dockPanel);

            //
            //Dialog
            this.m_OptionDialog = new OptionDialog();
            this.m_AboutBox = new AboutBox();
            this.m_LibraryPropertyDialog = new LibraryPropertyDialog();

            //内部変数
            this.m_FirstShowWindow = true;
            this.m_ExcelFileModuleListWindows = new List<ExcelFileModuleListWindow>();

            //LibraryProject
            this.m_Project = new LibraryProject();
            this.m_Project.FileClosed += this.M_Project_FileClosed;
            this.m_Project.FolderClosed += this.M_Project_FolderClosed;

            //TreeViewで選択されているライブラリフォルダ
            this.m_SelectedFolder = null;
            this.SelectedFolderChanged += this.MainWindow_SelectedFolderChanged;

            //TreeViewで選択されているオフィスファイル
            this.m_SelectedOfficeFile = null;
            this.SelectedOfficeFileChanged += this.MainWindow_SelectedOfficeFileChanged;

            //ListViewで選択されているライブラリファイル
            this.m_SelectedLibraryFile = null;
            this.SelectedLibraryFileChanged += this.MainWindow_SelectedLibraryFileChanged;



            //this.RefreshDisplay(true);
            //
            //AccountBook
            //
            //ツールバーの状態を更新する
            //this.RefreshAllToolBarStatus();
        }



        #endregion

        #region イベントハンドラ
        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

        #region MainWindow
        //*******************************************************************************************************
        //*******************************************************************************************************
        
        #region FormClosed
        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e) {
            //全ての子ウインドウを閉じる
            dockPanel.Contents.Cast<DockContent>().ToList().ForEach(dock => dock.Close());

            //Formを閉じたときに、現在の状態をConfigに反映する
            if (this.WindowState == FormWindowState.Normal) {
                AppMain.g_AppMain.AppInfo.MainWindowWidth = this.Width;
                AppMain.g_AppMain.AppInfo.MainWindowHeight = this.Height;
                AppMain.g_AppMain.AppInfo.MainWindowLeft = this.Left;
                AppMain.g_AppMain.AppInfo.MainWindowTop = this.Top;
            }

        } 
        #endregion

        #region VisibleChanged
        private void MainWindow_VisibleChanged(object sender, EventArgs e) {
            //初回表示時のみConfigに従ってSize,Locationを設定する
            if (this.m_FirstShowWindow == true) {
                //前回起動時のウインドウサイズと位置を復元
                this.Size = new Size(AppMain.g_AppMain.AppInfo.MainWindowWidth, AppMain.g_AppMain.AppInfo.MainWindowHeight);
                this.Location = new Point(AppMain.g_AppMain.AppInfo.MainWindowLeft, AppMain.g_AppMain.AppInfo.MainWindowTop);

                //各ウインドウを表示しておく
                //表示処理中のエラーを出力するために、OutputWindowを最初に作成しておく
                this.ShowOutput(true);
                //TreeのRefreshDisplayはこの時点で呼び出ししておく
                //SelectedFolderに初期値を設定した時に、ライブラリに存在しないという例外を回避するため
                this.ShowExplorerTree();
                this.ShowExplorerList(true);
                this.ShowPreview(true);

                //ライブラリが存在する場合、1つめのライブラリを表示しておく
                if ((this.m_Project?.Libraries.Count ?? 0) > 0) {
                    this.SelectedFolder = this.m_Project.Libraries[0].RootFolder;
                }

                this.m_FirstShowWindow = false;
            }

            this.RefreshDisplay();
        } 
        #endregion

        #region SelectedFolderChanged
        /// <summary>
        /// SelectedFolderが変更されたとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_SelectedFolderChanged(object sender, EventArgs<LibraryFolder> e) {
            AppMain.logger.Debug($"MainWindow.MainWindow_SelectedFolderChanged. old={e.OldValue?.Path}, new={this.SelectedFolder?.Path}");
            //影響するウインドウに反映する
            if (this.m_ExplorerTreeWindow != null) {
                this.m_ExplorerTreeWindow.SelectedFolder = this.m_SelectedFolder;
            }

            if (this.SelectedFolder != null) {
                //nullの場合、表示は保持する
                if (this.m_LibraryFileListWindow == null) {
                    this.CreateLibraryFileListWindow();
                    this.ShowLibraryFileListWindow(this.m_LibraryFileListWindow);
                }
                this.m_LibraryFileListWindow.SetTargetFolder(this.m_SelectedFolder);
                this.m_LibraryFileListWindow.Activate();
            }
        }
        #endregion

        #region SelectedOfficeFileChanged
        /// <summary>
        /// SelectedOfficeFileが変更されたとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_SelectedOfficeFileChanged(object sender, EventArgs<OfficeFile> e) {
            AppMain.logger.Debug($"MainWindow.MainWindow_SelectedOfficeFileChanged. old={e.OldValue?.FileName}, new={this.SelectedOfficeFile?.FileName}");
            //影響するウインドウに反映する
            if (this.m_ExplorerTreeWindow != null) {
                this.m_ExplorerTreeWindow.SelectedFile = this.m_SelectedOfficeFile;
            }

            //選択されたファイルのウインドウを表示する。
            //表示済みの場合、Active化。
            if (this.SelectedOfficeFile != null) {
                //nullの場合、表示は保持する
                string filename = this.SelectedOfficeFile?.FileName ?? "";
                ExcelFileModuleListWindow window = this.m_ExcelFileModuleListWindows.FirstOrDefault(w => w.TargetFile?.FileName == filename);
                if (window == null) {
                    ExcelFile file = (ExcelFile)this.SelectedOfficeFile;
                    window = this.CreateExcelFileModuleListWindow();
                    this.ShowExcelFileModuleListWindow(window);
                    window.TargetFile = file;
                    this.RefreshDisplay(true);
                }
                else {
                    window.Activate();
                }
            }
        }
        #endregion

        #region SelectedLibraryFileChanged
        /// <summary>
        /// SelectedLibraryFileが変更されたとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_SelectedLibraryFileChanged(object sender, EventArgs<LibraryFile> e) {
            AppMain.logger.Debug($"MainWindow.MainWindow_SelectedLibraryFileChanged. old={e.OldValue?.FileName}, new={this.SelectedLibraryFile?.FileName}");
            //影響するウインドウに反映する
            if (this.m_LibraryFileListWindow != null) {
                this.m_LibraryFileListWindow.SelectedFile = this.m_SelectedLibraryFile;
            }
            if (this.m_PreviewWindow != null) {
                this.m_PreviewWindow.TargetFile = this.m_SelectedLibraryFile;
            }
        }
        #endregion

        #region ReceiveOutputLogRequest
        /// <summary>
        /// IOutputLogRequest.OutputLogRequestイベントを受け取る共通のイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReceiveOutputLogRequest(object sender, OutputLogRequestEventArgs e) {
            OutputLogRequestData data = (OutputLogRequestData)e.RequestData;
            this.m_OutputLogWindow.AppendLogMessage(data.Message);

            AppMain.logger.Info($"[Output Message({e.OwnerName})]{data.Message}");
        }
        #endregion

        #region ReceiveNotifyParentRequest
        /// <summary>
        /// NotifyParentRequestイベントを受け取る共通のイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private async void ReceiveNotifyParentRequest(object sender, RequestEventArgs e) {
            AppMain.logger.Info($"Receive event from {sender.GetType().Name}. EventType = {e.RequestData.GetType().Name}");
            switch (e.RequestData) {
                case ExportModuleRequestData data_export:
                    //モジュールの再エクスポート
                    await this.ReloadFile(data_export.TargetFile);
                    break;
                case ShowLibraryFolderPropertyRequestData data_property1:
                    //フォルダのプロパティ
                    this.m_LibraryPropertyDialog.TargetFolder = this.SelectedFolder;
                    this.m_LibraryPropertyDialog.ShowDialog();
                    break;
                case ShowOfficeFilePropertyRequestData data_property2:
                    //ファイルのプロパティ
                    this.m_LibraryPropertyDialog.TargetOfficeFile = this.SelectedOfficeFile;
                    this.m_LibraryPropertyDialog.ShowDialog();
                    break;
                case ShowCompareWindowRequestData data_compareWindow:
                    //フォルダ比較ウインドウの表示
                    FolderCompareWindow window = this.CreateFolderCompareWindow();
                    window.SourceFolderPath = data_compareWindow.SourceFolderPath;
                    window.DestinationFolderPath = data_compareWindow.DestinationFolderPath;
                    window.CheckDiff();
                    this.ShowFolderCompareWindow(window);
                    break;
            }
        } 
        #endregion

        #endregion

        #region DockPanel関連
        //*******************************************************************************************************
        //*******************************************************************************************************

        #region DockPanel
        /// <summary>
        /// ActiveDocumentが変更されたとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DockPanel_ActiveDocumentChanged(object sender, EventArgs e) {
            this.m_ActiveDocument = (DockContent)this.dockPanel.ActiveDocument;

            //ExplorerTreeの選択状態を合わせる
            switch (this.m_ActiveDocument) {
                case ExcelFileModuleListWindow excelWindow:
                    if (excelWindow.TargetFile != null) {
                        this.SelectedOfficeFile = excelWindow.TargetFile;
                    }
                    break;
                case LibraryFileListWindow libraryWindow:
                    if (libraryWindow.TargetFolder != null) {
                        this.SelectedFolder = libraryWindow.TargetFolder;
                    }
                    break;
            }
        }
        #endregion

        #region ExplorerTreeWindow
        //*******************************************************************************************************
        private void M_ExplorerTreeWindow_FormClosed(object sender, FormClosedEventArgs e) {
            this.m_ExplorerTreeWindow.RefreshLibrariesRequest -= this.M_ExplorerTreeWindow_RefreshLibrariesRequest;
            this.m_ExplorerTreeWindow.SelectedNodeChanged -= this.M_ExplorerTreeWindow_SelectedNodeChanged;
            this.m_ExplorerTreeWindow.VisibleChanged -= this.M_ExplorerTreeWindow_VisibleChanged;
            this.m_ExplorerTreeWindow.NotifyParentRequest -= this.ReceiveNotifyParentRequest;

            this.m_ExplorerTreeWindow = null;
        }
        /// <summary>
        /// ExplorerTreeWindowでSelectedNodeChangedイベントが発生した時に呼び出されます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void M_ExplorerTreeWindow_SelectedNodeChanged(object sender, ExplorerTreeWindow.EventArgs<ExplorerTreeNode> e) {
            AppMain.logger.Debug($"MainWindow.ExplorerTreeWindow_SelectedNodeChanged. old={e.OldValue?.Text ?? ""}, new={this.m_ExplorerTreeWindow.SelectedNode?.Text ?? ""}");
            this.SelectedFolder = this.m_ExplorerTreeWindow.SelectedFolder;
            this.SelectedOfficeFile = this.m_ExplorerTreeWindow.SelectedFile;
        } 
        /// <summary>
        /// ExplorerTreeWindowからLibraryの更新要求が発生したときに呼び出されます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void M_ExplorerTreeWindow_RefreshLibrariesRequest(object sender, EventArgs e) {
            this.m_Project.Libraries.ForEach(x => x.Refresh());
        }

        private void M_ExplorerTreeWindow_VisibleChanged(object sender, EventArgs e) {
            this.エクスプローラーTToolStripMenuItem.Checked = this.m_ExplorerTreeWindow?.Visible ?? false;
        }

        #endregion

        #region LibraryFileListWindow
        //*******************************************************************************************************
        private void M_LibraryFileListWindow_FormClosed(object sender, FormClosedEventArgs e) {
            this.m_LibraryFileListWindow.SelectedItemChanged -= this.M_LibraryFileListWindow_SelectedItemChanged;
            this.m_LibraryFileListWindow.OutputLogRequest -= this.ReceiveOutputLogRequest;

            this.m_LibraryFileListWindow = null;
        }
        /// <summary>
        /// LibraryFileListWindowでSelectedItemChangedイベントが発生した時に呼び出されます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void M_LibraryFileListWindow_SelectedItemChanged(object sender, LibraryExplorerList.EventArgs<LibraryFileListViewItem> e) {
            AppMain.logger.Debug($"MainWindow.LibraryFileListWindow_SelectedItemChanged. old={e.OldValue?.LibraryFile?.FileName}, new={this.m_LibraryFileListWindow.SelectedItem?.LibraryFile?.FileName}");
            this.SelectedLibraryFile = this.m_LibraryFileListWindow.SelectedFile;
        }

        #endregion

        #region ExcelFileModuleListWindow
        //*******************************************************************************************************
        /// <summary>
        /// ExcelFileModuleListWindowを閉じるとき、登録したイベントの後始末を行う
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExcelFileModuleListWindow_FormClosed(object sender, FormClosedEventArgs e) {
            ExcelFileModuleListWindow window = (ExcelFileModuleListWindow)sender;
            window.SelectedItemChanged -= this.excelFileModuleListWindow_SelectedItemChanged;
            window.OutputLogRequest -= this.ReceiveOutputLogRequest;

            //表示しているウインドウのリストから削除
            this.m_ExcelFileModuleListWindows.Remove(window);
        }

        private void excelFileModuleListWindow_SelectedItemChanged(object sender, LibraryExplorerList.EventArgs<LibraryFileListViewItem> e) {
            ExcelFileModuleListWindow window = (ExcelFileModuleListWindow)sender;
            AppMain.logger.Debug($"MainWindow.ExcelFileModuleListWindow_SelectedItemChanged. old={e.OldValue?.LibraryFile?.FileName}, new={window.SelectedItem?.LibraryFile?.FileName}");
            //SelectedLibraryFileはクリアする
            this.SelectedLibraryFile = null;
            //プレビューウインドウの表示を変更する
            if (this.m_PreviewWindow != null) {
                this.m_PreviewWindow.TargetFile = window.SelectedFile;
            }
        }

        /// <summary>
        /// Window.Activatedイベントハンドラ
        /// Floatingウインドウの場合、このイベントハンドラでActive化を検出する
        /// Dockingされている場合、DockPanel.ActiveDocumentChangedで検出する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void excelFileModuleListWindow_Activated(object sender, EventArgs e) {
            ExcelFileModuleListWindow window = (ExcelFileModuleListWindow)sender;
            this.SelectedOfficeFile = window.TargetFile;
        }

        #endregion

        #region PreviewWindow
        //*******************************************************************************************************
        private void M_PreviewWindow_FormClosed(object sender, FormClosedEventArgs e) {
            this.m_PreviewWindow.OutputLogRequest -= this.ReceiveOutputLogRequest;

            this.m_PreviewWindow = null;
        }
        private void M_PreviewWindow_VisibleChanged(object sender, EventArgs e) {
            this.プレビューPToolStripMenuItem.Checked = this.m_PreviewWindow?.Visible ?? false;
        }

        #endregion

        #region OutputLogWindow
        //*******************************************************************************************************

        private void M_OutputLogWindow_VisibleChanged(object sender, EventArgs e) {
            this.出力OToolStripMenuItem.Checked = this.m_OutputLogWindow?.Visible ?? false;
        }

        private void M_OutputLogWindow_FormClosed(object sender, FormClosedEventArgs e) {
            this.m_OutputLogWindow = null;
        }

        #endregion

        #endregion

        #region LibraryProject
        /// <summary>
        /// フォルダが閉じられた時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void M_Project_FolderClosed(object sender, LibraryProject.EventArgs<Library> e) {
            this.RefreshDisplay(true);
        }

        /// <summary>
        /// ファイルが閉じられた時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void M_Project_FileClosed(object sender, LibraryProject.EventArgs<OfficeFile> e) {
            this.RefreshDisplay(true);

            //ExcelFileModuleListWindowを開いていた場合、閉じる
            string filename = e.OldValue?.FileName ?? "";
            DockContent content = this.FindDocument(dock => ((dock as ExcelFileModuleListWindow)?.TargetFile?.FileName ?? "") == filename);
            if (content != null) {
                content.Close();
            }
        }

        #endregion

        #region メニュー
        //*******************************************************************************************************
        //*******************************************************************************************************

        #region ファイル
        private void ファイルFToolStripMenuItem_DropDownOpened(object sender, EventArgs e) {
            this.再読み込みRToolStripMenuItem.Enabled = (this.SelectedOfficeFile != null);
        }

        /// <summary>
        /// フォルダを開く
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 開くOToolStripMenuItem_Click(object sender, EventArgs e) {
            this.OpenFolder();
        }

        private async void ファイルを開くFToolStripMenuItem_Click(object sender, EventArgs e) {
            await this.OpenFile();
        }
        private async void 再読み込みRToolStripMenuItem_Click(object sender, EventArgs e) {
            await this.ReloadFile(this.SelectedOfficeFile);
        }

        private void 閉じるCToolStripMenuItem_Click(object sender, EventArgs e) {
            this.CloseContents();
        }
        private void 終了XToolStripMenuItem_Click(object sender, EventArgs e) {
            this.Close();
        }
        #endregion

        #region 表示
        private void エクスプローラーTToolStripMenuItem_Click(object sender, EventArgs e) {
            this.ShowExplorerTree();
        }

        private void プレビューPToolStripMenuItem_Click(object sender, EventArgs e) {
            this.ShowPreview();
        }

        private void 出力OToolStripMenuItem_Click(object sender, EventArgs e) {
            this.ShowOutput();
        }

        private void 最新の情報に更新RToolStripMenuItem_Click(object sender, EventArgs e) {
            this.RefreshDisplay();
        }
        #endregion

        #region ツール
        private void ツールTToolStripMenuItem_DropDownOpened(object sender, EventArgs e) {
            this.バージョン確認ツールVToolStripMenuItem.Enabled = (this.SelectedOfficeFile != null);
        }

        private void バージョン確認ツールVToolStripMenuItem_Click(object sender, EventArgs e) {
            this.CheckModuleVersion();
        }

        private void オプションOToolStripMenuItem_Click(object sender, EventArgs e) {
            this.ShowOptionDialog();
        }

        #endregion

        #region ヘルプ
        private void ヘルプHToolStripMenuItem1_Click(object sender, EventArgs e) {
            this.ShowHelp();
        }

        private void バージョン情報AToolStripMenuItem_Click(object sender, EventArgs e) {
            this.ShowVersionInfo();
        }
        #endregion

        #endregion

        #region ツールバー
        //*******************************************************************************************************
        //*******************************************************************************************************
        private void フォルダを開くOToolStripButton_Click(object sender, EventArgs e) {
            this.OpenFolder();
        }

        private async void ファイルを開くFToolStripButton_Click(object sender, EventArgs e) {
            await this.OpenFile();
        }

        private void ヘルプLToolStripButton_Click(object sender, EventArgs e) {
            this.ShowHelp();
        }

        #endregion

        #endregion

        #region メニュー、ツールバーの本体
        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

        #region ファイル
        //*******************************************************************************************************
        //*******************************************************************************************************
        
        /// <summary>
        /// フォルダを開く
        /// </summary>
        private void OpenFolder() {
            string initialPath = Application.StartupPath;
            this.folderBrowserDialog1.Description = "ライブラリのトップフォルダを指定してください。";
            this.folderBrowserDialog1.SelectedPath = initialPath;
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK) {
                string path = this.folderBrowserDialog1.SelectedPath;
                //フォルダを開く
                this.OpenFolder(path);
            }
        }
        /// <summary>
        /// 指定されたフォルダを開きます。
        /// </summary>
        /// <param name="folderPath"></param>
        private void OpenFolder(string folderPath) {
            //重複登録のためのクロスチェック
            if (!this.m_Project.Libraries.Any(x => x.TargetFolder.Contains(folderPath) || folderPath.Contains(x.TargetFolder))) {
                this.m_Project.Libraries.Add(Library.FromFolder(folderPath));
                this.RefreshDisplay(true);
            }
            else {
                MessageBox.Show("指定されたフォルダまたはその一部がすでに登録されているため登録できません。", "重複登録エラー", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        /// <summary>
        /// ファイルを開く
        /// </summary>
        private async Task OpenFile() {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK) {
                string filename = this.openFileDialog1.FileName;

                await this.OpenFile(filename);   
            }
        }

        /// <summary>
        /// 指定されたファイルを開きます。
        /// エクスポート済みファイルを開く場合、exportDateにエクスポートされた日時を指定します。
        /// exportDateがnullの場合、もしくは、ファイルの更新日付がexportDateを超えている場合は再度エクスポートします。
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="exportDate"></param>
        /// <returns></returns>
        private async Task OpenFile(string filename,DateTime? exportDate = null) {
            //重複オープンは不可
            if (!this.m_Project.ExcelFiles.Any(file => file.FileName == filename)) {
                ExcelFileModuleListWindow window = this.CreateExcelFileModuleListWindow();
                this.ShowExcelFileModuleListWindow(window);
                //
                ExcelFile file = new ExcelFile() { FileName = filename ,ExportDate = exportDate};
                this.m_Project.ExcelFiles.Add(file);

                window.TargetFile = file;
                //エクスポート済みかどうかを判定し、エクスポートされていて、かつ最新の場合はスキップする。
                if (file.ExportDate == null || file.UpdateDate > file.ExportDate) {
                    await window.ExportModules();
                }
                this.RefreshDisplay(true);
                this.SelectedOfficeFile = file;
            }
            else {
                MessageBox.Show("指定されたファイルは既に開かれているため開けません。", "重複オープンエラー", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 再読み込み
        /// </summary>
        private async Task ReloadFile(OfficeFile targetFile) {
            string filename = targetFile?.FileName ?? "";
            //ExcelFileModuleListWindow window = (ExcelFileModuleListWindow)this.FindDocument(w=>((w as ExcelFileModuleListWindow)?.TargetFile?.FileName??"") == filename);
            ExcelFileModuleListWindow window = this.FindDocument<ExcelFileModuleListWindow>(w => (w.TargetFile?.FileName ?? "") == filename);
            if (window != null) {
                await window.ExportModules();
                this.RefreshDisplay(true);
            }
        }

        /// <summary>
        /// 閉じる
        /// </summary>
        private void CloseContents() {
            if (this.SelectedFolder != null) {
                this.m_Project.CloseFolder(this.SelectedFolder);
            }
            if (this.SelectedOfficeFile != null) {
                this.m_Project.CloseFile(this.SelectedOfficeFile);
            }
        }

        /// <summary>
        /// 終了
        /// </summary>
        private void Exit() {
            this.Close();
        }

        #endregion

        #region 表示
        //*******************************************************************************************************
        //*******************************************************************************************************
        /// <summary>
        /// ツリー表示
        /// </summary>
        /// <param name="suspendRefreshDisplay"></param>
        private void ShowExplorerTree(bool suspendRefreshDisplay = false) {
            ExplorerTreeWindow window = this.CreateExplorerTreeWindow();
            if (window.Visible) {
                window.Hide();
            }
            else {
                this.ShowExplorerTreeWindow(window, suspendRefreshDisplay);
            }
        }
        /// <summary>
        /// ライブラリ一覧
        /// </summary>
        /// <param name="suspendRefreshDisplay"></param>
        private void ShowExplorerList(bool suspendRefreshDisplay = false) {
            LibraryFileListWindow window = this.CreateLibraryFileListWindow();
            this.ShowLibraryFileListWindow(window, suspendRefreshDisplay);
        }
        /// <summary>
        /// プレビュー表示
        /// </summary>
        /// <param name="suspendRefreshDisplay"></param>
        private void ShowPreview(bool suspendRefreshDisplay = false) {
            PreviewWindow window = this.CreatePreviewWindow();
            if (window.Visible) {
                window.Hide();
            }
            else {
                this.ShowPreviewWindow(window, suspendRefreshDisplay);
            }
        }
        /// <summary>
        /// 出力
        /// </summary>
        /// <param name="suspendRefreshDisplay"></param>
        private void ShowOutput(bool suspendRefreshDisplay = false) {
            OutputLogWindow window = this.CreateOutputLogWindow();
            if (window.Visible) {
                window.Hide();
            }
            else {
                this.ShowOutputLogWindow(window, suspendRefreshDisplay);
            }
        }
        /// <summary>
        /// 最新の情報に更新
        /// </summary>
        /// <param name="keep"></param>
        private void RefreshDisplay(bool keep = false) {
            //Libraryを更新
            this.m_Project.Libraries.ForEach(x => x.Refresh());

            //表示するデータの再設定
            if (this.m_ExplorerTreeWindow != null) {
                //ExplorerTreeWindowは常に全てのフォルダを表示しているため、Targetの再設定は不要
            }
            if (this.m_LibraryFileListWindow != null) {
                //対象フォルダを再設定
                this.m_LibraryFileListWindow.SetTargetFolder(this.m_SelectedFolder);
            }
            if (this.m_PreviewWindow != null) {
                //対象ファイルを再設定
                this.m_PreviewWindow.TargetFile = this.m_SelectedLibraryFile;
            }

            //IRefreshDisplayを実装した全てのDockContentsを更新する。
            this.dockPanel.Contents.OfType<IRefreshDisplay>().ToList().ForEach(window => window.RefreshDisplay(keep));
        }
        #endregion

        #region ツール
        //*******************************************************************************************************
        /// <summary>
        /// モジュールのバージョン確認を行う
        /// </summary>
        private void CheckModuleVersion() {
            if (this.SelectedOfficeFile == null) {
                return;
            }
            //フォルダ比較ウィザードを使用してバージョン確認を行う。
            FolderCompareWizardDialog dialog = new FolderCompareWizardDialog();
            dialog.TargetProject = this.m_Project;
            dialog.TargetFile = this.SelectedOfficeFile;
            dialog.NotifyParentRequest += this.ReceiveNotifyParentRequest;
            dialog.ShowDialog();
        }

        /// <summary>
        /// オプションダイアログの表示
        /// </summary>
        private void ShowOptionDialog() {
            //AppInfoのCloneを渡す。
            this.m_OptionDialog.AppInfo = AppMain.g_AppMain.AppInfo.Clone();
            if (this.m_OptionDialog.ShowDialog() == DialogResult.OK) {
                //OKボタンが押されたら、AppInfoの値をマスタにコピーする
                AppMain.g_AppMain.AppInfo.CopyFrom(this.m_OptionDialog.AppInfo);
            }
        }

        #endregion

        #region ヘルプ
        //*******************************************************************************************************
        private void ShowHelp() {
            //IMPORTANT:【常時注意】機能追加時はヘルプをフォローすること
            string filename = Path.Combine(Application.StartupPath, "help\\index.html");
            ProcessStartInfo info = new ProcessStartInfo(filename);
            Process p = new Process();
            p.StartInfo = info;
            p.Start();
        }
        private void ShowVersionInfo() {
            if (this.m_AboutBox == null) {
                this.m_AboutBox = new AboutBox();
            }
            this.m_AboutBox.ShowDialog();
        }

        #endregion

        #endregion

        #region DockPanel関連
        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

        #region Document関連
        //*******************************************************************************************************
        /// <summary>
        /// Documentエリアの中でActiveなDockContentを返す
        /// </summary>
        /// <returns></returns>
        private DockContent GetActiveDocument() {
            return (DockContent)this.dockPanel.ActiveDocument;
        }

        /// <summary>
        /// Documentをすべて閉じます。
        /// </summary>
        private void CloseAllDocument() {
            this.dockPanel.Documents.OfType<DockContent>().ToList().ForEach(document => document.Close());
            //foreach (DockContent document in this.dockPanel.Documents.OfType<DockContent>().ToArray()) {
            //    document.Close();
            //}
        }

        /// <summary>
        /// 指定した条件を満たすDockContentを取得します。
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        private DockContent FindDocument(Func<DockContent,bool> predicate) {
            return this.dockPanel.Documents.OfType<DockContent>().FirstOrDefault(predicate);
        }
        /// <summary>
        /// 指定した条件を満たすDockContentの派生クラスを取得します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        private T FindDocument<T>(Func<T, bool> predicate)where T : DockContent {
            return this.dockPanel.Documents.OfType<T>().FirstOrDefault(predicate);
        }


        #endregion

        #region ExplorerTreeWindow
        //*******************************************************************************************************
        private ExplorerTreeWindow CreateExplorerTreeWindow() {
            if (this.m_ExplorerTreeWindow == null) {
                //ExplorerTreeWindowはシングルトンとする
                this.m_ExplorerTreeWindow = new ExplorerTreeWindow();
                //this.m_ExplorerTreeWindow.TargetLibraries = this.m_Libraries;
                this.m_ExplorerTreeWindow.TargetProject = this.m_Project;

                this.m_ExplorerTreeWindow.FormClosed += this.M_ExplorerTreeWindow_FormClosed;
                this.m_ExplorerTreeWindow.RefreshLibrariesRequest += this.M_ExplorerTreeWindow_RefreshLibrariesRequest;
                this.m_ExplorerTreeWindow.SelectedNodeChanged += this.M_ExplorerTreeWindow_SelectedNodeChanged;
                this.m_ExplorerTreeWindow.VisibleChanged += this.M_ExplorerTreeWindow_VisibleChanged;
                this.m_ExplorerTreeWindow.NotifyParentRequest += this.ReceiveNotifyParentRequest;
                
            }
            return this.m_ExplorerTreeWindow;
        }


        private void ShowExplorerTreeWindow(ExplorerTreeWindow window, bool suspendRefreshDisplay = false) {
            window.Show(this.dockPanel, DockState.DockLeft);

            if (!suspendRefreshDisplay) {
                this.RefreshDisplay(true);
            }
        }
        #endregion

        #region LibraryFileListWindow
        //*******************************************************************************************************
        private LibraryFileListWindow CreateLibraryFileListWindow() {
            if (this.m_LibraryFileListWindow == null) {
                this.m_LibraryFileListWindow = new LibraryFileListWindow();
                this.m_LibraryFileListWindow.FormClosed += this.M_LibraryFileListWindow_FormClosed;
                this.m_LibraryFileListWindow.SelectedItemChanged += this.M_LibraryFileListWindow_SelectedItemChanged;
                this.m_LibraryFileListWindow.OutputLogRequest += this.ReceiveOutputLogRequest;
            }
            return this.m_LibraryFileListWindow;
        }


        private void ShowLibraryFileListWindow(LibraryFileListWindow window,bool suspendRefreshDisplay = false) {
            window.Show(this.dockPanel, DockState.Document);

            if (!suspendRefreshDisplay) {
                this.RefreshDisplay(true);
            }
        }
        #endregion

        #region PreviewWindow
        //*******************************************************************************************************
        private PreviewWindow CreatePreviewWindow() {
            if (this.m_PreviewWindow == null) {
                //PreviewWindowはシングルトンとする
                this.m_PreviewWindow = new PreviewWindow();
                this.m_PreviewWindow.FormClosed += this.M_PreviewWindow_FormClosed;
                this.m_PreviewWindow.VisibleChanged += this.M_PreviewWindow_VisibleChanged;
                this.m_PreviewWindow.OutputLogRequest += this.ReceiveOutputLogRequest;
            }
            return this.m_PreviewWindow;
        }


        private void ShowPreviewWindow(PreviewWindow window, bool suspendRefreshDisplay = false) {
            if (this.m_ActiveDocument == null) {
                window.Show(this.dockPanel, DockState.DockBottom);
            }
            else {
                DockPane pane = this.m_ActiveDocument.Pane;
                window.Show(pane, DockAlignment.Bottom, 0.5);
            }

            if (!suspendRefreshDisplay) {
                this.RefreshDisplay(true);
            }
        }
        #endregion

        #region ExcelFileModuleListWindow
        //*******************************************************************************************************
        private ExcelFileModuleListWindow CreateExcelFileModuleListWindow() {
            ExcelFileModuleListWindow window = new ExcelFileModuleListWindow();
            window.FormClosed += this.ExcelFileModuleListWindow_FormClosed;
            window.SelectedItemChanged += this.excelFileModuleListWindow_SelectedItemChanged;
            window.Activated += this.excelFileModuleListWindow_Activated;
            window.OutputLogRequest += this.ReceiveOutputLogRequest;
            return window;
        }


        private void ShowExcelFileModuleListWindow(ExcelFileModuleListWindow window, bool suspendRefreshDisplay = false) {
            //表示したウインドウを保持しておく（再表示用）
            this.m_ExcelFileModuleListWindows.Add(window);

            window.Show(this.dockPanel, DockState.Document);

            if (!suspendRefreshDisplay) {
                this.RefreshDisplay(true);
            }
        }
        #endregion

        #region OutputLogWindow
        //*******************************************************************************************************
        private OutputLogWindow CreateOutputLogWindow() {
            if (this.m_OutputLogWindow == null) {
                //OutputLogはシングルトンとする
                this.m_OutputLogWindow = new OutputLogWindow();
                this.m_OutputLogWindow.FormClosed += this.M_OutputLogWindow_FormClosed;
                this.m_OutputLogWindow.VisibleChanged += this.M_OutputLogWindow_VisibleChanged;
            }
            return this.m_OutputLogWindow;
        }

        private void ShowOutputLogWindow(OutputLogWindow window, bool suspendRefreshDisplay = false) {
            window.Show(this.dockPanel, DockState.DockBottom);

            if (!suspendRefreshDisplay) {
                this.RefreshDisplay(true);
            }
        }

        #endregion

        #region FolderCompareWindow

        private FolderCompareWindow CreateFolderCompareWindow() {
            FolderCompareWindow window = new FolderCompareWindow();

            return window;
        }

        private void ShowFolderCompareWindow(FolderCompareWindow window) {
            window.Show(this.dockPanel, DockState.Document);
            //this.RefreshDisplay(true);
            //FolderCompareWindowはLibraryとは独立して動くので、RefershDisplayは不要
        }
        #endregion

        #endregion

        #region デバッグメニュー
        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

        //デバッグ用
        private void explorerTreeViewの表示ToolStripMenuItem_Click(object sender, EventArgs e) {
            ExplorerTreeWindow window = this.CreateExplorerTreeWindow();
            this.ShowExplorerTreeWindow(window);
        }
        //デバッグ用
        private void explorerListViewの表示ToolStripMenuItem_Click(object sender, EventArgs e) {
            LibraryFileListWindow window = this.CreateLibraryFileListWindow();
            this.ShowLibraryFileListWindow(window);
        }
        //デバッグ用
        private void previewWindowの表示ToolStripMenuItem_Click(object sender, EventArgs e) {
            //if (this.m_PreviewWindow == null) {
            //    this.m_PreviewWindow = new PreviewWindow();
            //}
            //if (this.m_LibraryFileListWindow == null) {

            //    this.m_PreviewWindow.Show(this.dockPanel, DockState.DockBottom);
            //}
            //else {
            //    this.m_PreviewWindow.Show(this.m_LibraryFileListWindow.Pane, DockAlignment.Bottom, 0.5);
            //}
            PreviewWindow window = this.CreatePreviewWindow();
            this.ShowPreviewWindow(window);
        }
        //デバッグ用
        private void libraryFileの読み込みToolStripMenuItem_Click(object sender, EventArgs e) {
            string filename = @"E:\02_Document\Document\Visual Studio 2017\Projects\LibraryExplorer\LibraryExplorer\bin\Debug\TestData\自作ライブラリ\Common\FileEnumerator.cls";
            if (LibraryFile.IsTargetFile(filename)) {
                LibraryFile library = LibraryFile.FromFile(filename);
            }
        }//デバッグ用
        private void explorerTreeViewSelectedFolderPathToolStripMenuItem_Click(object sender, EventArgs e) {
            if (this.m_ExplorerTreeWindow != null) {
                this.m_ExplorerTreeWindow.SelectedFolder = new LibraryFolder() {
                    Path = @"E:\02_Document\Document\Visual Studio 2017\Projects\LibraryExplorer\LibraryExplorer\bin\Debug\TestData\自作ライブラリ\PowerPoint"
                };
            }
        }
        //デバッグ用
        private void explorerListWindowSelectedFileToolStripMenuItem_Click(object sender, EventArgs e) {
            if (this.m_LibraryFileListWindow != null) {
                string path = @"E:\02_Document\Document\Visual Studio 2017\Projects\LibraryExplorer\LibraryExplorer\bin\Debug\TestData\自作ライブラリ\PowerPoint\PPPresentation.cls";
                LibraryFileListViewItem item = this.m_LibraryFileListWindow.FindItem(x => x.LibraryFile?.FileName == path);
                LibraryFile library = item?.LibraryFile;

                this.m_LibraryFileListWindow.SelectedFile = library;
            }
        }

        //デバッグ用
        private ApplicationMessageQueue m_MessageQueue;
        private void applicationMessageQueueのテストToolStripMenuItem_Click(object sender, EventArgs e) {
            if (this.m_MessageQueue == null) {
                this.m_MessageQueue = new ApplicationMessageQueue(this);
            }
            this.m_MessageQueue.Start();

            this.m_MessageQueue.AddMessage(new ApplicationMessage(() => Console.WriteLine("1")));
            this.m_MessageQueue.AddMessage(new ApplicationMessage(() => {
                this.m_MessageQueue.AddMessage(new ApplicationMessage(() => Console.WriteLine("4")));
            }));
            this.m_MessageQueue.AddMessage(new ApplicationMessage(() => Console.WriteLine("2"), true));
            this.m_MessageQueue.AddMessage(new ApplicationMessage(() => Console.WriteLine("3")));
        }
        //デバッグ用
        private void folderCompareWizardの表示ToolStripMenuItem_Click(object sender, EventArgs e) {
            FolderCompareWizardDialog dialog = new FolderCompareWizardDialog();
            dialog.TargetProject = this.m_Project;
            dialog.TargetFile = this.SelectedOfficeFile;
            dialog.NotifyParentRequest += this.ReceiveNotifyParentRequest;
            dialog.ShowDialog();
        }

        FileSystemWatcher m_fileSystemWatcher;
        //デバッグ用
        private void fileSystemWatcherのテスト開始ToolStripMenuItem_Click(object sender, EventArgs e) {
            if (this.m_fileSystemWatcher == null) {
                this.m_fileSystemWatcher = new FileSystemWatcher();

                //イベントハンドラ登録
                this.m_fileSystemWatcher.Changed += this.fileSystemWatcher_EventHandler;
                this.m_fileSystemWatcher.Created += this.fileSystemWatcher_EventHandler;
                this.m_fileSystemWatcher.Deleted += this.fileSystemWatcher_EventHandler;
                this.m_fileSystemWatcher.Renamed += this.fileSystemWatcher_EventHandler;
            }

            this.m_fileSystemWatcher.Path = @"E:\02_Document\Document\Visual Studio 2017\Projects\LibraryExplorer\LibraryExplorer\bin\Debug";
            this.m_fileSystemWatcher.Filter = "";
            this.m_fileSystemWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.DirectoryName;
            this.m_fileSystemWatcher.IncludeSubdirectories = true;

            //イベントを有効にして監視開始
            this.m_fileSystemWatcher.EnableRaisingEvents = true;

        }
        //デバッグ用
        private void fileSystemWatcherのテスト停止ToolStripMenuItem_Click(object sender, EventArgs e) {
            if (this.m_fileSystemWatcher == null) {
                return;
            }
            this.m_fileSystemWatcher.EnableRaisingEvents = false;
        }
        //デバッグ用
        private void fileSystemWatcher_EventHandler(object sender, FileSystemEventArgs e) {
            Console.WriteLine($"{e.ChangeType.ToString()} : path={e.FullPath}");
        }

        #endregion
    }


    /// <summary>
    /// [最新の情報に更新]が使用できるオブジェクトであることを表すインターフェースです。
    /// </summary>
    public interface IRefreshDisplay {
        /// <summary>
        /// 表示を更新します。
        /// </summary>
        /// <param name="keep"></param>
        void RefreshDisplay(bool keep = false);
    }


}
