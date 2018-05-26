using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryExplorer.Common;
using LibraryExplorer.Common.ExTool;
using LibraryExplorer.Common.Request;
using LibraryExplorer.Data;
namespace LibraryExplorer.Control.Wizard {


    /// <summary>
    /// フォルダ比較ウィザードを表すクラスです。
    /// 対象のOfficeFileからエクスポートされたModuleをベースに、
    /// 同じファイル名を持つファイルをライブラリから検索し、テキストとして比較します。
    /// 同じファイル名のファイルが複数見つかった場合、どのファイルを使用するかを手動で選択することができます。
    /// </summary>
    public partial class FolderCompareWizard : EditableWizardControl {


        #region フィールド(メンバ変数、プロパティ、イベント)

        private TextEditor m_TextEditor;
        private DiffTool m_DiffTool;

        //対象ファイルのModuleとライブラリのModuleのペア
        private List<TargetLibraryPair> m_TargetLibraryPairs;

        //対象ライブラリ
        private List<Library> m_TargetLibraries;

        //一時フォルダにコピーするファイルのリスト
        private List<string> m_ModulePathList;

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


        #region TargetFile
        private OfficeFile m_TargetFile;
        /// <summary>
        /// TargetFileを取得、設定します。
        /// </summary>
        public OfficeFile TargetFile {
            get {
                return this.m_TargetFile;
            }
            set {
                this.m_TargetFile = value;
            }
        }
        #endregion

        #region TargetProject
        private LibraryProject m_TargetProject;
        /// <summary>
        /// TargetProjectを取得、設定します。
        /// </summary>
        public LibraryProject TargetProject {
            get {
                return this.m_TargetProject;
            }
            set {
                this.m_TargetProject = value;
            }
        }
        #endregion

        #region OutputFolder
        private LibraryFolder m_OutputFolder;
        /// <summary>
        /// OutputFolderを取得します。
        /// </summary>
        public LibraryFolder OutputFolder {
            get {
                return this.m_OutputFolder;
            }
        }
        #endregion

        #endregion

        #region コンストラクタ
        /// <summary>
        /// FolderCompareWizardオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public FolderCompareWizard() {
            InitializeComponent();

            this.m_TextEditor = new TextEditor();
            this.m_DiffTool = new DiffTool();
        }
        #endregion

        #region ウィザードの動作
        /// <summary>
        /// Start時の動作をオーバーライドします。
        /// </summary>
        protected override void OnStart() {
            base.OnStart();

            this.OnShowStartPage();

        }

        /// <summary>
        /// GoNext時の動作をオーバーライドします。
        /// </summary>
        protected override void OnGoNext() {
            base.OnGoNext();

            //以下、ウィザードのページNo毎に処理を記載する。
            switch (this.CurrentStepNo) {
                case 0:
                    //GoNextで0に遷移することは無いため処理無し
                    break;
                case 1:
                    //モジュールの検索
                    this.SearchLibraryModules();
                    break;
                case 2:
                    //対応するモジュールのリストを作成
                    this.CreateModuleList();
                    break;
                case 3:
                    //一時フォルダにファイルをコピー
                    //ファイルの比較
                    //一時フォルダのパスを表示
                    this.ShowResultPage();
                    break;
            }
        }

        /// <summary>
        /// 次へボタンのテキストを設定します。
        /// </summary>
        /// <param name="text"></param>
        protected override void SetGoNextButtonText(string text) {
            switch (this.CurrentStepNo) {
                case 2:
                    //2ページのみ[次へ]を[開始]に変更します。3ページは後戻り不可。
                    text = "開始";
                    break;
                default:
                    //その他はデフォルトのテキストから変更しない
                    break;
            }
            base.SetGoNextButtonText(text);
        }
        #endregion

        #region Page0:スタートページ

        #region イベントハンドラ
        /// <summary>
        /// 対象ライブラリのチェック状態が変更されたとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void targetLibraryNameListView1_ItemChecked(object sender, ItemCheckedEventArgs e) {
            //ボタン状態、エラー表示のために、検証を行う
            this.Validate_Page0();
        }

        #endregion

        #region OnShowStartPage
        /// <summary>
        /// スタートページを表示します。
        /// </summary>
        private void OnShowStartPage() {
            //対象ファイル
            this.targetFileNameTextBox1.Text = Path.GetFileName(this.TargetFile?.FileName ?? "");
            this.targetFileNameTextBox1.SelectionStart = 0;
            //対象プロジェクト
            this.targetLibraryNameListView1.Items.Clear();
            this.TargetProject.Libraries.ToList().ForEach(lib => {
                this.targetLibraryNameListView1.Items.Add(new ListViewItem(Path.GetFileName(lib.TargetFolder)) { Tag = lib, Checked = true });
            });
            //ボタン状態、エラー表示のために、検証を行う
            this.Validate_Page0();
        } 
        #endregion

        #region Validate_Page0
        /// <summary>
        /// Page0の検証を行います。
        /// </summary>
        private void Validate_Page0() {
            //対象ライブラリ0個は不可
            bool librarySelected = this.targetLibraryNameListView1.CheckedItems.Count != 0;
            //対象ファイルが指定されていること
            bool isNullTarget = (this.TargetFile == null);

            this.errorLabel1.Visible = isNullTarget;
            this.errorLabel2.Visible = !librarySelected;

            this.CanGoNext = !isNullTarget && librarySelected;
        }
        #endregion

        #endregion

        #region Page1:ライブラリモジュールの検索

        #region イベントハンドラ

        #region ContextMenu

        #region ContextMenu1(ライブラリのModule)
        private void contextMenuStrip1_Opened(object sender, EventArgs e) {
            bool selected = this.targetLibraryModuleListView1.SelectedItems.Count == 1;
            this.選択SToolStripMenuItem.Enabled = selected;
            this.ファイルを開くOToolStripMenuItem.Enabled = selected;
        }

        private void 選択SToolStripMenuItem_Click(object sender, EventArgs e) {
            this.ChangeTargetLibraryModuleSelectedStatus();
        }
        private void ファイルを開くOToolStripMenuItem_Click(object sender, EventArgs e) {
            //ライブラリのModuleを開く
            bool selected = this.targetLibraryModuleListView1.SelectedItems.Count == 1;
            if (!selected) {
                return;
            }
            if (!(this.targetLibraryModuleListView1.SelectedItems[0] is TargetLibraryModuleListViewItem item)) {
                return;
            }
            this.m_TextEditor.Start(new TextEditorInfo(item.TargetFile.TargetFile));
        }

        #endregion

        #region ContextMenu2(ファイルのModule)
        private void contextMenuStrip2_Opened(object sender, EventArgs e) {
            bool selected = this.targetFileModuleListView1.SelectedItems.Count == 1;
            this.ファイルを開くOToolStripMenuItem1.Enabled = selected;
        }

        private void ファイルを開くOToolStripMenuItem1_Click(object sender, EventArgs e) {
            //ファイルのModuleを開く
            bool selected = this.targetFileModuleListView1.SelectedItems.Count == 1;
            if (!selected) {
                return;
            }
            if (!(this.targetFileModuleListView1.SelectedItems[0] is TargetFileModuleListViewItem item)) {
                return;
            }
            this.m_TextEditor.Start(new TextEditorInfo(item.TargetFile));
        }

        #endregion

        #endregion

        #region ListView
        
        #region targetFileModuleListView1
        /// <summary>
        /// 対象ファイルのModuleが変わったら、表示するライブラリのModuleも変更する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void targetFileModuleListView1_SelectedIndexChanged(object sender, EventArgs e) {

            if (this.targetFileModuleListView1.SelectedItems.Count != 1) {
                return;
            }

            //選択されたアイテムから、TargetLibraryPairを取得
            //取得したPairはTagに保管しておく
            TargetFileModuleListViewItem item1 = this.targetFileModuleListView1.SelectedItems[0] as TargetFileModuleListViewItem;

            TargetLibraryPair pair = this.m_TargetLibraryPairs.FirstOrDefault(x => x.BaseFile == item1.TargetFile);
            this.targetLibraryModuleListView1.Tag = pair;

            try {
                this.targetLibraryModuleListView1.BeginUpdate();
                this.targetLibraryModuleListView1.Items.Clear();

                //PairのLibraryファイルをすべて表示
                this.targetLibraryModuleListView1.Items.AddRange(pair.PairFileList.Select<SelectableLibraryFile, TargetLibraryModuleListViewItem>(x => {
                    TargetLibraryModuleListViewItem item = new TargetLibraryModuleListViewItem(x);
                    if (x.Selected) {
                        item.BackColor = Color.Orange;
                    }
                    return item;
                }).ToArray());
            }
            finally {
                this.targetLibraryModuleListView1.EndUpdate();
            }

        }
        #endregion

        #region targetLibraryModuleListView1

        private void targetLibraryModuleListView1_MouseDoubleClick(object sender, MouseEventArgs e) {
            this.ChangeTargetLibraryModuleSelectedStatus();
        } 
        #endregion

        #endregion

        #region ボタン
        /// <summary>
        /// 再検索ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void researchButton1_Click(object sender, EventArgs e) {
            this.SearchLibraryModules();
        } 
        #endregion

        #endregion

        #region SearchLibraryModules
        private void SearchLibraryModules() {
            //====================================================================================
            //0ページ終了の処理
            //対象ライブラリを取得
            this.m_TargetLibraries = this.targetLibraryNameListView1.CheckedItems.Cast<ListViewItem>().Select<ListViewItem, Library>(x => x.Tag as Library).ToList();

            try {
                //====================================================================================
                //1ページ開始の処理

                this.targetFileModuleListView1.BeginUpdate();
                this.targetFileModuleListView1.Items.Clear();

                //nullまたはtemporaryFolderが存在しない場合、処理中断
                if (!(this.m_TargetFile?.ExistWorkspaceFolder() ?? false)) {
                    return;
                }

                List<LibraryFile> files = this.m_TargetFile.WorkspaceFolder.GetLibraryFiles();
                //対象ファイルのリストに表示
                this.targetFileModuleListView1.Items.AddRange(files.Select(x => new TargetFileModuleListViewItem(x)).ToArray());

                //対応するライブラリの検索用リストの作成
                //対象ファイルのモジュール毎にペアが作られる。
                //ペアが1対1ならOK
                //1対0なら対象無し
                //1対nなら競合の解決が必要(SelectedLibraryFile.Selectをどれか一つだけtrueにする)
                this.m_TargetLibraryPairs = new List<TargetLibraryPair>();
                this.m_TargetLibraryPairs.AddRange(files.Select(f => new TargetLibraryPair(f, this.m_TargetLibraries)));

                //競合の確認
                this.CheckPairingStatus();

            }
            finally {
                this.targetFileModuleListView1.EndUpdate();
            }
        }

        #endregion

        #region CheckPairingStatus
        /// <summary>
        /// 該当ファイル有無や競合の状態確認
        /// </summary>
        private void CheckPairingStatus() {
            //ライブラリ側に複数のファイルが存在する場合
            //(ファイル名が同じで別フォルダ)
            List<TargetLibraryPair> duplicatedFiles = this.m_TargetLibraryPairs.Where(pair => pair.PairFileList.Count > 1).ToList();
            bool _canGoNext = true;

            duplicatedFiles.ForEach(pair => {
                ListViewItem duplicatedItem = this.targetFileModuleListView1.Items.Cast<TargetFileModuleListViewItem>()
                    .FirstOrDefault(item => item.TargetFile == pair.BaseFile);
                if (pair.PairFileList.Any(file => file.Selected)) {
                    //どれか一つ選択されていれば解決済みでオレンジ
                    duplicatedItem.BackColor = Color.Orange;
                }
                else {
                    //未解決は赤
                    duplicatedItem.BackColor = Color.Red;
                    _canGoNext = false;
                }
            });
            //競合が見つかった場合、CanGoNext=Falseとして、競合解決するまで次に進むを無効にする
            this.CanGoNext = _canGoNext;

            //ライブラリ側にファイルが存在しない場合
            List<TargetLibraryPair> notExistsFiles = this.m_TargetLibraryPairs.Where(pair => pair.PairFileList.Count == 0).ToList();
            notExistsFiles.ForEach(pair => {
                ListViewItem notExistsItem = this.targetFileModuleListView1.Items.Cast<TargetFileModuleListViewItem>()
                   .FirstOrDefault(item => item.TargetFile == pair.BaseFile);
                //該当ファイル無しは黄色
                notExistsItem.BackColor = Color.Yellow;
            });
        } 
        #endregion

        #region ChangeTargetLibraryModuleSelectedStatus
        /// <summary>
        /// ライブラリModuleの選択状態の変更。
        /// [ダブルクリック]または[右クリック]-[選択]から呼び出し。
        /// </summary>
        private void ChangeTargetLibraryModuleSelectedStatus() {
            if (this.targetFileModuleListView1.SelectedItems.Count != 1) {
                return;
            }

            TargetLibraryPair pair = this.targetLibraryModuleListView1.Tag as TargetLibraryPair;
            if (pair == null) {
                //念のため確認。targetFileModuleListViewItem1にItemが存在するので、Tagがnullになることは無い。
                return;
            }

            TargetLibraryModuleListViewItem item = this.targetLibraryModuleListView1.SelectedItems[0] as TargetLibraryModuleListViewItem;
            //選択状態の更新
            pair.PairFileList.ForEach(x => {
                //選択されていたものをtrue。それ以外をfalse。
                x.Selected = (x == item.TargetFile);
            });
            //選択状態に従って色の変更
            this.targetLibraryModuleListView1.Items.Cast<TargetLibraryModuleListViewItem>().ToList().ForEach(x => {
                x.BackColor = x.TargetFile.Selected ? Color.Orange : Color.White;
            });

            //選択状態を変更したので、競合の再確認
            this.CheckPairingStatus();
        }
        #endregion

        #region private class

        #region TargetFileModuleListViewItem
        private class TargetFileModuleListViewItem : ListViewItem {

            #region フィールド(メンバ変数、プロパティ、イベント)

            #region TargetFile
            private LibraryFile m_TargetFile;
            /// <summary>
            /// TargetFileを取得します。
            /// </summary>
            public LibraryFile TargetFile {
                get {
                    return this.m_TargetFile;
                }
            }
            #endregion

            #endregion

            #region コンストラクタ
            public TargetFileModuleListViewItem(LibraryFile file) {
                this.m_TargetFile = file;

                this.Text = Path.GetFileName(file.FileName);
                this.SubItems.Add(file.Revision);
            }
            #endregion

        }
        #endregion

        #region TargetLibraryModuleListViewItem
        private class TargetLibraryModuleListViewItem : ListViewItem {

            #region フィールド(メンバ変数、プロパティ、イベント)

            #region TargetFile
            private SelectableLibraryFile m_TargetFile;
            /// <summary>
            /// TargetFileを取得します。
            /// </summary>
            public SelectableLibraryFile TargetFile {
                get {
                    return this.m_TargetFile;
                }
            }
            #endregion

            #endregion

            #region コンストラクタ
            public TargetLibraryModuleListViewItem(SelectableLibraryFile file) {
                this.m_TargetFile = file;

                this.Text = Path.GetFileName(file.TargetFile.FileName);
                this.SubItems.AddRange(new string[] { file.TargetFile.Revision, Path.GetFileName(file.TargetLibrary.TargetFolder), file.TargetFile.FileName });
            }
            #endregion

        }
        #endregion

        #region TargetLibraryPair
        /// <summary>
        /// エクスポートファイルとライブラリファイルのペアを表します。
        /// ライブラリには同名のファイルが存在する可能性があるため、ライブラリファイルはリストとして保持します。
        /// </summary>
        private class TargetLibraryPair {

            #region フィールド(メンバ変数、プロパティ、イベント)

            private string m_BaseFileName;

            #region BaseFile
            private LibraryFile m_BaseFile;
            /// <summary>
            /// BaseFileを取得します。
            /// </summary>
            public LibraryFile BaseFile {
                get {
                    return this.m_BaseFile;
                }
            }
            #endregion

            #region PairFileList
            private List<SelectableLibraryFile> m_PairFileList;
            /// <summary>
            /// PairFileListを取得します。
            /// </summary>
            public List<SelectableLibraryFile> PairFileList {
                get {
                    return this.m_PairFileList;
                }
            }
            #endregion

            #endregion

            #region コンストラクタ
            public TargetLibraryPair(LibraryFile baseFile, List<Library> targetLibraries) {
                this.m_PairFileList = new List<SelectableLibraryFile>();
                this.m_BaseFile = baseFile;
                this.m_BaseFileName = Path.GetFileName(this.m_BaseFile.FileName);

                this.SearchPair(targetLibraries);
            }
            #endregion

            private void SearchPair(List<Library> targetLibraries) {

                targetLibraries.ToList().ForEach(lib => {
                    this.SearchPair2(lib, lib.TargetFolder);
                });


                //NOTE:2018/5/5 LibraryFolder.GetLibraryFilesを経由すると、全てのファイルに対してLibraryFileオブジェクトを作成するためパフォーマンスが悪い。ここではファイル名のみの判断なので、Directory.GetFilesを使用した方法に変更。

                //Library⇒LibraryFolder⇒LibraryFileに変換し、BaseFileに対応するファイルを探す
                //最後は競合の解決用としてSelectableLibraryFileに変換して保存する
                //List<SelectableLibraryFile> files = project.Libraries
                //    .SelectMany<Library, LibraryFolder>(lib => lib.FindAll(folder => true))
                //    .SelectMany<LibraryFolder, LibraryFile>(folder => folder.GetLibraryFiles_Enumerable()
                //         .Where(file => Path.GetFileName(file.FileName) == baseFileName))
                //    .Select<LibraryFile,SelectableLibraryFile>(file=>new SelectableLibraryFile(file))
                //    .ToList();

                //結果を格納
                //this.PairFileList.AddRange(files);


            }

            /// <summary>
            /// BaseFileに対応するファイルをライブラリから検索します。
            /// </summary>
            /// <param name="lib"></param>
            /// <param name="directoryName"></param>
            private void SearchPair2(Library lib, string directoryName) {
                string path = directoryName;
                string baseFileName = Path.GetFileName(this.m_BaseFile.FileName);

                string filename = Directory.GetFiles(path).Where(f => Path.GetFileName(f) == baseFileName).FirstOrDefault() ?? "";
                if (File.Exists(filename)) {
                    LibraryFile file = LibraryFile.FromFile(filename);
                    this.PairFileList.Add(new SelectableLibraryFile(file, lib));
                }
                //サブディレクトリに対して再帰処理
                Directory.GetDirectories(path).ToList().ForEach(subDir => {
                    this.SearchPair2(lib, subDir);
                });


            }
        }
        #endregion

        #region SelectableLibraryFile
        /// <summary>
        /// 重複しているライブラリファイルの解決を行うために、
        /// Selectedプロパティに選択状態を記憶できるLibraryFileクラスです。
        /// </summary>
        private class SelectableLibraryFile {

            #region フィールド(メンバ変数、プロパティ、イベント)

            #region TargetFile
            private LibraryFile m_TargetFile;
            /// <summary>
            /// TargetFileを取得します。
            /// </summary>
            public LibraryFile TargetFile {
                get {
                    return this.m_TargetFile;
                }
            }
            #endregion

            #region TargetLibrary
            private Library m_TargetLibrary;
            /// <summary>
            /// TargetLibraryを取得します。
            /// </summary>
            public Library TargetLibrary {
                get {
                    return this.m_TargetLibrary;
                }
            }
            #endregion

            #region Selected
            private bool m_Selected;
            /// <summary>
            /// Selectedを取得、設定します。
            /// </summary>
            public bool Selected {
                get {
                    return this.m_Selected;
                }
                set {
                    this.m_Selected = value;
                }
            }
            #endregion

            #endregion

            #region コンストラクタ
            public SelectableLibraryFile(LibraryFile file, Library lib) {
                this.m_TargetFile = file;
                this.m_TargetLibrary = lib;
            }
            #endregion

        }
        #endregion

        #endregion

        #endregion

        #region Page2:対応するモジュールのリスト作成

        private void CreateModuleList() {
            //====================================================================================
            //1ページ終了の処理

            //モジュールのリスト作成
            this.m_ModulePathList = this.m_TargetLibraryPairs?
                .Where(pair => pair.PairFileList.Count > 0)
                .Select(pair => {
                    if (pair.PairFileList.Count == 1) {
                        return pair.PairFileList[0].TargetFile.FileName;
                    }
                    else {
                        return pair.PairFileList.First(file => file.Selected).TargetFile.FileName;
                    }
                })
                .ToList();
            //該当ファイル無しはスキップ
            //該当ファイルが一つならば無条件で採用。
            //そうでない場合、Selected==trueのファイルを採用

            //====================================================================================
            //2ページ開始の処理
            
            //特になし
        }
        #endregion

        #region Page3:一時フォルダへコピーして比較 (完了ページの表示)

        private void ShowResultPage() {
            //このページが表示された段階で、戻る/キャンセル不可。
            this.CanGoBack = false;
            this.CanCancel = false;
            //====================================================================================
            //2ページ終了の処理

            //特になし

            //====================================================================================
            //3ページ開始の処理

            //一時フォルダを作成してファイルをコピー
            this.CopyWorkFolder();

            //一時フォルダのパスを表示
            this.ShowWorkFolderPath();

            //ファイル比較を実行
            //外部ツール機能を実装する。
            this.CheckDiff();

            //TODO:一時フォルダをいつ削除するのかを検討(Projectに登録して、アプリケーション終了時に削除？⇒ファイナライザを実装したので終了時には消えた。。。再比較や閉じるにも対応したい。)

        }

        private void CopyWorkFolder() {
            WorkFolder workFolder = new WorkFolder();
            workFolder.DeleteAtClose = true;
            workFolder.FolderNameFormatString = $"Lib_yyyyMMdd_HHmmss_{Path.GetFileNameWithoutExtension(this.TargetFile.FileName)}";
            workFolder.Create();
            //作成した一時フォルダに各ファイルをコピー
            this.m_ModulePathList.ForEach(filename => {
                string dstName = Path.Combine(workFolder.Path, Path.GetFileName(filename));
                File.Copy(filename, dstName, true);
            });
            this.m_OutputFolder = workFolder;
        }
        private void ShowWorkFolderPath() {
            this.targetFileTempFolderTextBox1.Text = this.TargetFile.WorkspaceFolder.Path;
            this.targetLibraryWorkFolderTextBox1.Text = this.m_OutputFolder.Path;
        }

        private void CheckDiff() {
            DiffToolInfo info = new DiffToolInfo(this.TargetFile.WorkspaceFolder.Path, this.m_OutputFolder.Path);
            ExternalToolResult result = this.m_DiffTool.Start(info);

            if (result.ResultCode == ExternalToolResultCode.Failed) {
                //Failedの場合、詳細を確認
                if (result is DiffToolResult diffResult) {
                    //ツールが存在しなかった場合（未指定含む）、標準のFolderCompareWindowを表示
                    if (!diffResult.ExistTool) {
                        this.ShowCompareWindow();
                    }
                }
            }

        }
        private void ShowCompareWindow() {
            string sourceFolderPath = this.TargetFile.WorkspaceFolder.Path;
            string destinationFolderPath = this.m_OutputFolder.Path;

            this.OnNotifyParentRequest(new ShowCompareWindowRequestEventArgs(sourceFolderPath, destinationFolderPath));
        }

        #endregion

    }


}
