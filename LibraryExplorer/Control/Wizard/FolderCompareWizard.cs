using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryExplorer.Data;

namespace LibraryExplorer.Control.Wizard {

    //TODO:フォルダ比較ウィザードの実装

    
    //TODO:ContextMenu1,2の実装

    /// <summary>
    /// フォルダ比較ウィザードを表すクラスです。
    /// </summary>
    public partial class FolderCompareWizard : EditableWizardControl {


        #region フィールド(メンバ変数、プロパティ、イベント)

        //対象ファイルのModuleとライブラリのModuleのペア
        private List<TargetLibraryPair> m_TargetLibraryPairs;

        //対象ライブラリ
        private List<Library> m_TargetLibraries;

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

        #endregion

        #region コンストラクタ
        /// <summary>
        /// FolderCompareWizardオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public FolderCompareWizard() {
            InitializeComponent();
        }
        #endregion


        #region ウィザードの動作
        /// <summary>
        /// Start時の動作をオーバーライドします。
        /// </summary>
        protected override void OnStart() {
            base.OnStart();
            //対象ファイル
            this.targetFileNameTextBox1.Text = Path.GetFileName(this.TargetFile?.FileName ?? "");
            this.targetFileNameTextBox1.SelectionStart = 0;
            //対象プロジェクト
            this.targetLibraryNameListView1.Items.Clear();
            this.TargetProject.Libraries.ForEach(lib => {
                this.targetLibraryNameListView1.Items.Add(new ListViewItem(Path.GetFileName(lib.TargetFolder)) { Tag = lib});
            });
        }

        /// <summary>
        /// GoNext時の動作をオーバーライドします。
        /// </summary>
        protected override void OnGoNext() {
            base.OnGoNext();

            //TODO:以下、ウィザードのページNo毎に処理を記載する。
            switch (this.CurrentStepNo) {
                case 0:
                    //GoNextで0に遷移することは無い
                    break;
                case 1:
                    //モジュールの検索
                    this.SearchLibraryModules();
                    break;
                case 2:
                    //対応するモジュールのリストを作成(OnFinishで一時フォルダを作成しコピーする。
                    this.CreateModuleList();
                    break;
            }
        }
        /// <summary>
        /// Finish時の動作をオーバーライドします。
        /// </summary>
        protected override void OnFinish() {
            base.OnFinish();

            this.CopyFileToTempFolder();
        }

        #endregion

        #region Page1:ライブラリモジュールの検索

        #region イベントハンドラ

        #region ContextMenu

        #region ContextMenu1(ライブラリのModule)

        private void 選択SToolStripMenuItem_Click(object sender, EventArgs e) {
            this.ChangeTargetLibraryModuleSelectedStatus();
        }
        private void ファイルを開くOToolStripMenuItem_Click(object sender, EventArgs e) {
            //ライブラリのModuleを開く
        }

        #endregion

        #region ContextMenu2(ファイルのModule)
        private void ファイルを開くOToolStripMenuItem1_Click(object sender, EventArgs e) {
            //TODO:ファイルのModuleを開く
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
                if (!(this.m_TargetFile?.ExistTemporaryFolder() ?? false)) {
                    return;
                }

                List<LibraryFile> files = this.m_TargetFile.TemporaryFolder.GetLibraryFiles();
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

            private void SearchPair2(Library lib, string directoryName) {
                string path = directoryName;
                string baseFileName = Path.GetFileName(this.m_BaseFile.FileName);

                string filename = Directory.GetFiles(path).Where(f => Path.GetFileName(f) == baseFileName).FirstOrDefault() ?? "";
                if (File.Exists(filename)) {
                    LibraryFile file = LibraryFile.FromFile(filename);
                    this.PairFileList.Add(new SelectableLibraryFile(file, lib));
                }

                Directory.GetDirectories(path).ToList().ForEach(subDir => {
                    this.SearchPair2(lib, subDir);
                });


            }
        }
        #endregion

        #region SelectableLibraryFile
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
            //TODO:モジュールのリスト作成
        }
        #endregion

        #region 完了

        private void CopyFileToTempFolder() {
            //TODO:一時フォルダへファイルをコピー
        }

        #endregion

    }


}
