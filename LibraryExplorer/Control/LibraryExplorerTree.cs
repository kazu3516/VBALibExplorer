using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryExplorer.Common;
using LibraryExplorer.Common.Request;
using LibraryExplorer.Control;
using LibraryExplorer.Data;

namespace LibraryExplorer.Control {

    /// <summary>
    /// Libraryをツリー表示するコントロールを表すクラスです。
    /// </summary>
    public partial class LibraryExplorerTree : UserControl {

        #region フィールド(メンバ変数、プロパティ、イベント)
        private ExplorerTreeNode m_LibraryRootNode;
        private ExplorerTreeNode m_FileRootNode;


        #region NotifyParentRequestイベント
        /// <summary>
        /// 親コントロールへの要求を行うイベントです。
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


        #region RefreshLibrariesRequestイベント
        /// <summary>
        /// Libraryの更新要求を行うイベントです。
        /// </summary>
        public event EventHandler RefreshLibrariesRequest;
        /// <summary>
        /// RefreshLibrariesRequestイベントを発生させます。
        /// </summary>
        /// <param name="e"></param>
        protected void OnRefreshLibrariesRequest(EventArgs e) {
            this.RefreshLibrariesRequest?.Invoke(this, e);
        }
        #endregion


        #region TargetProject
        private LibraryProject m_TargetProject;
        /// <summary>
        /// TargetProjectが変更された場合に発生するイベントです。
        /// </summary>
        public event EventHandler<EventArgs<LibraryProject>> TargetProjectChanged;
        /// <summary>
        /// TargetProjectが変更された場合に呼び出されます。
        /// </summary>
        /// <param name="e">イベントパラメータ</param>
        protected void OnTargetProjectChanged(EventArgs<LibraryProject> e) {
            this.TargetProjectChanged?.Invoke(this, e);
        }
        /// <summary>
        /// TargetProjectを取得、設定します。
        /// </summary>
        public LibraryProject TargetProject {
            get {
                return this.m_TargetProject;
            }
            set {
                this.SetProperty(ref this.m_TargetProject, value, ((oldValue) => {
                    if (this.TargetProjectChanged != null) {
                        this.OnTargetProjectChanged(new EventArgs<LibraryProject>(oldValue));
                    }
                }));
            }
        }
        #endregion

        #region SelectedFolder
        /// <summary>
        /// SelectedFolderPathを取得、設定します。
        /// </summary>
        public LibraryFolder SelectedFolder {
            get {
                return (this.SelectedNode as LibraryFolderTreeNode)?.LibraryFolder;
            }
            set {
                //FolderNodeが選択されていて、SelectedFolderにnullが設定された場合、Nodeの選択を解除
                if (value == null){
                    if ((this.SelectedNode as LibraryFolderTreeNode) != null) {
                        this.treeView1.SelectedNode = null;
                    }
                    return;
                }
                LibraryFolderTreeNode node = this.FindFolderNode(x => (x as LibraryFolderTreeNode)?.LibraryFolder?.Path == value?.Path);
                if (node != null) {
                    this.treeView1.SelectedNode = node;
                }
                else {
                    throw new ArgumentException($"指定されたフォルダはLibraryに存在しません。Path={value?.Path ?? ""}");
                }
            }
        }
        #endregion

        #region SelectedFile
        
        /// <summary>
        /// SelectedFileを取得、設定します。
        /// </summary>
        public OfficeFile SelectedFile {
            get {
                return (this.SelectedNode as OfficeFileTreeNode)?.TargetFile;
            }
            set {
                if (value == null){
                    if((this.SelectedNode as OfficeFileTreeNode) != null) {
                        this.treeView1.SelectedNode = null;
                    }
                    return;
                }
                OfficeFileTreeNode node = this.FindOfficeFileNode(x => (x as OfficeFileTreeNode)?.TargetFile.FileName == value?.FileName);
                if (node != null) {
                    this.treeView1.SelectedNode = node;
                }
                else {
                    throw new ArgumentException($"指定されたファイルは開かれていません。Path={value?.FileName ?? ""}");
                }
            }
        }
        #endregion

        #region SelectedNode
        private ExplorerTreeNode m_SelectedNode;
        /// <summary>
        /// SelectedNodeが変更された場合に発生するイベントです。
        /// </summary>
        public event EventHandler<EventArgs<ExplorerTreeNode>> SelectedNodeChanged;
        /// <summary>
        /// SelectedNodeが変更された場合に呼び出されます。
        /// </summary>
        /// <param name="e">イベントパラメータ</param>
        protected void OnSelectedNodeChanged(EventArgs<ExplorerTreeNode> e) {
            this.SelectedNodeChanged?.Invoke(this, e);
        }
        /// <summary>
        /// SelectedNodeを取得、設定します。
        /// </summary>
        public ExplorerTreeNode SelectedNode {
            get {
                return this.m_SelectedNode;
            }
            protected set {
                this.SetProperty(ref this.m_SelectedNode, value, ((oldValue) => {
                    if (this.SelectedNodeChanged != null) {
                        this.OnSelectedNodeChanged(new EventArgs<ExplorerTreeNode>(oldValue));
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
        /// <summary>
        /// LibraryExplorerTreeオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public LibraryExplorerTree() {
            InitializeComponent();

            this.Initialize();
        }

        private void Initialize() {
            this.m_TargetProject = new LibraryProject();
            this.TargetProjectChanged += this.ExplorerTreeWindow_TargetProjectChanged;

            this.m_LibraryRootNode = new ApplicationFolderTreeNode("Librarys");
            this.m_FileRootNode = new ApplicationFolderTreeNode("Files");

            //ExcelFileTreeNode node1 = new ExcelFileTreeNode("Test");
            //node1.ImageIndex = 3;
            //node1.SelectedImageIndex = 3;
            //this.m_FileRootNode.Nodes.Add(node1);

            this.treeView1.Nodes.Add(this.m_LibraryRootNode);
            this.treeView1.Nodes.Add(this.m_FileRootNode);
        }


        #endregion

        #region イベントハンドラ


        private void ExplorerTreeWindow_TargetProjectChanged(object sender, EventArgs<LibraryProject> e) {
            this.RefreshDisplay();
        }

        #region ContextMenu
        /// <summary>
        /// ContextMenuを開いたときの処理。
        /// メニューの有効無効、表示非表示の切り替えを行う。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuStrip1_Opened(object sender, EventArgs e) {
            this.再エクスポートXToolStripMenuItem.Visible = this.SelectedFile != null;
            this.ファイルの場所を開くFToolStripMenuItem.Visible = this.SelectedFile != null;

            this.エクスプローラを開くOToolStripMenuItem.Visible = this.SelectedFolder != null;


            this.全て展開EToolStripMenuItem.Enabled = (this.treeView1.Nodes.Cast<TreeNode>().Select(node=>node.Nodes.Count).Sum()) != 0;
            this.全て折りたたむCToolStripMenuItem.Enabled = (this.treeView1.Nodes.Cast<TreeNode>().Select(node => node.Nodes.Count).Sum()) != 0;
        }

        private void エクスプローラを開くOToolStripMenuItem_Click(object sender, EventArgs e) {
            this.OpenExplorer();
        }
        private void ファイルの場所を開くFToolStripMenuItem_Click(object sender, EventArgs e) {
            this.OpenFilePath();
        }
        private void 再エクスポートXToolStripMenuItem_Click(object sender, EventArgs e) {
            this.ReExport();
        }
        private void 閉じるCToolStripMenuItem_Click(object sender, EventArgs e) {
            this.CloseItem();
        }
        private void 全て展開EToolStripMenuItem_Click(object sender, EventArgs e) {
            this.ExpandAll();
        }
        private void 全て折りたたむCToolStripMenuItem_Click(object sender, EventArgs e) {
            this.CollapseAll();
        }
        private void 最新の情報に更新RToolStripMenuItem_Click(object sender, EventArgs e) {
            this.RefreshLibrary();
        }
        private void プロパティPToolStripMenuItem_Click(object sender, EventArgs e) {
            this.ShowProperty();
        }

        #endregion

        #region TreeView
        private void treeView1_AfterCollapse(object sender, TreeViewEventArgs e) {
            ExplorerTreeNode node = (ExplorerTreeNode)e.Node;
            int imageIndex = 2;
            switch (node.NodeType) {
                case ExplorerTreeNodeType.LibraryFolder:
                case ExplorerTreeNodeType.ApplicationFolder:
                    imageIndex = 0;
                    break;
            }
            node.ImageIndex = imageIndex;
            node.SelectedImageIndex = imageIndex;
        }

        private void treeView1_AfterExpand(object sender, TreeViewEventArgs e) {
            ExplorerTreeNode node = (ExplorerTreeNode)e.Node;
            int imageIndex = 2;
            switch (node.NodeType) {
                case ExplorerTreeNodeType.LibraryFolder:
                case ExplorerTreeNodeType.ApplicationFolder:
                    imageIndex = 1;
                    break;
            }
            node.ImageIndex = imageIndex;
            node.SelectedImageIndex = imageIndex;

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e) {
            ExplorerTreeNode node = (ExplorerTreeNode)e.Node;
            switch (node.NodeType) {
                case ExplorerTreeNodeType.LibraryFolder:
                    this.SelectedNode = node;
                    break;
                case ExplorerTreeNodeType.OfficeFile:
                    this.SelectedNode = node;
                    break;
                default:
                    this.SelectedNode = null;
                    break;
            }
        }
        #endregion

        #endregion

        #region メニューの本体

        private void OpenExplorer() {
            if (this.SelectedFolder != null) {
                string folderPath = this.SelectedFolder.Path;

                this.OpenFolder(folderPath, "エクスプローラを開く");
            }
        }
        private void OpenFilePath() {
            if (this.SelectedFile != null) {
                string folderPath = Path.GetDirectoryName(this.SelectedFile.FileName);

                this.OpenFolder(folderPath, "ファイルの場所を開く");
            }
        }
        private void ReExport() {
            if (this.SelectedFile != null) {
                this.OnNotifyParentRequest(new ExportModuleRequestEventArgs(this.SelectedFile, this.GetType().Name));
            }
        }
        private void CloseItem() {
            if (this.SelectedFolder != null) {
                this.TargetProject.CloseFolder(this.SelectedFolder);
            }
            if (this.SelectedFile != null) {
                this.TargetProject.CloseFile(this.SelectedFile);
            }
        }
        private void ExpandAll() {
            this.treeView1.Nodes.Cast<TreeNode>().ToList().ForEach(node => {
                node.ExpandAll();
            });
        }
        private void CollapseAll() {
            this.treeView1.Nodes.Cast<TreeNode>().ToList().ForEach(node => {
                node.Collapse();
            });
        }
        private void RefreshLibrary() {
            //NOTE:LibraryExplorer実行中にエクスプローラ上でフォルダを新規作成し、[最新の情報に更新]を実行すると、LibraryFolderとファイルシステムの不整合により異常終了する。⇒ContextMenuからRefreshDisplayを呼び出す前に、MainWindowに対してLibraryのRefresh要求を出し、不整合を解消する。
            this.OnRefreshLibrariesRequest(EventArgs.Empty);

            this.RefreshDisplay();
        }
        private void ShowProperty() {
            if (this.SelectedFolder != null) {
                this.OnNotifyParentRequest(new ShowLibraryFolderPropertyRequestEventArgs(this.SelectedFolder));
            }
            if (this.SelectedFile != null) {
                this.OnNotifyParentRequest(new ShowOfficeFilePropertyRequestEventArgs(this.SelectedFile));
            }

        }

        #endregion

        #region RefreshDisplay
        /// <summary>
        /// 表示を更新します。
        /// </summary>
        /// <param name="keep"></param>
        public void RefreshDisplay(bool keep = false) {
            try {
                this.treeView1.BeginUpdate();

                if (!keep || this.m_TargetProject?.Libraries == null || this.m_TargetProject.Libraries.Count == 0) {
                    AppMain.logger.Debug($"{this.GetType().Name}.RefreshDisplay / Clear Nodes.");

                    //ノードのクリア
                    this.treeView1.Nodes.Clear();
                    //ノードの再構築
                    this.treeView1.Nodes.Add(this.m_LibraryRootNode);
                    this.treeView1.Nodes.Add(this.m_FileRootNode);
                }
                if (this.m_TargetProject?.Libraries != null) {
                    this.RefreshLibrariesDisplay();
                }
                if (this.m_TargetProject?.ExcelFiles != null) {
                    this.RefreshFilesDisplay();
                }
            }
            finally {
                this.treeView1.EndUpdate();
            }
        }

        #region Library

        private void RefreshLibrariesDisplay() {
            AppMain.logger.Debug($"{this.GetType().Name}.RefreshDisplay / Show Libraries. count={this.m_TargetProject.Libraries.Count}");
            /*
            【参考】from LibraryExplorerList
            //リストの複製を用意し、複製で削除判定。複製のForEach内で本体から削除する。(本体のForEachではないので、列挙中の削除例外は発生しない)
            List<LibraryFileListViewItem> cloneList = this.listView1.Items.Cast<LibraryFileListViewItem>().ToList();
            cloneList.Where(x => !files.Any(f => f.FileName == x.LibraryFile.FileName)).ToList()
                //このToListの時点で、filesに存在しないアイテムがリストアップされている
                .ForEach(item => this.listView1.Items.Remove(item));
 
             * */

            //Libraryに存在しないNodeを削除
            //リストの複製を用意し、複製で削除判定。複製のForEach内で本体から削除する。(本体のForEachではないので、列挙中の削除例外は発生しない)
            List<LibraryFolderTreeNode> cloneList = this.m_LibraryRootNode.Nodes.Cast<LibraryFolderTreeNode>().ToList();
            cloneList.Where(node => !this.m_TargetProject.Libraries.Any(lib => lib.TargetFolder == node.LibraryFolder.Path)).ToList()
                //このToListの時点で、Librariesに存在しないノードがリストアップされている
                .ForEach(node => this.m_LibraryRootNode.Nodes.Remove(node));

            //for (int i = this.m_LibraryRootNode.Nodes.Count - 1; i >= 0; i--) {
            //    LibraryFolderTreeNode node = (LibraryFolderTreeNode)this.m_LibraryRootNode.Nodes[i];
            //    if (!this.m_TargetProject.Libraries.Any(x => x.TargetFolder == node.LibraryFolder.Path)) {
            //        this.m_LibraryRootNode.Nodes.Remove(node);
            //    }
            //}

            //LibraryとNodeを同期
            this.m_TargetProject.Libraries.ForEach(lib => {
                AppMain.logger.Debug($"{this.GetType().Name}.RefreshDisplay / Show Library. Path={lib.TargetFolder}");
                //子フォルダへ再帰処理
                this.EnumerateFolder(this.m_LibraryRootNode, lib, lib.RootFolder);
            });
            //for (int i = 0; i < this.m_TargetProject.Libraries.Count; i++) {
                //Library lib = this.m_TargetProject.Libraries[i];
            //    AppMain.logger.Debug($"{this.GetType().Name}.RefreshDisplay / Show Library. Path={lib.TargetFolder}");
            //    //子フォルダへ再帰処理
            //    this.EnumerateFolder(this.m_LibraryRootNode, lib, lib.RootFolder);
            //}

            //ノードの展開状況を調整
            if (this.m_LibraryRootNode.Nodes.Count != 0) {
                this.m_LibraryRootNode.Nodes[0].EnsureVisible();
            }

        }

        /// <summary>
        /// フォルダを巡回して、ノードを作成する。
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="library"></param>
        /// <param name="folder"></param>
        private void EnumerateFolder(ExplorerTreeNode parent, Library library, LibraryFolder folder) {
            //NOTE:parentはLibraryRootNodeも含めるため、ExplorerTreeNodeで宣言する
            string name = Path.GetFileName(folder.Path);
            //指定されたフォルダに対応するノードの存在有無を確認し、無ければ追加する。
            LibraryFolderTreeNode node = parent.Nodes.Cast<LibraryFolderTreeNode>().FirstOrDefault(x => x.LibraryFolder?.Path == folder.Path);
            if (node == null) {
                node = new LibraryFolderTreeNode(folder);
                parent.Nodes.Add(node);
            }


            //子ノードに不要なノードがないかの確認
            //NOTE:RootNodeにはこのチェックは不要のため、子ノードからスタートしている
            List<LibraryFolderTreeNode> cloneList = node.Nodes.Cast<LibraryFolderTreeNode>().ToList();
            cloneList.Where(child => library.Find(X => X.Path == child.LibraryFolder?.Path) == null).ToList()
                .ForEach(child=>node.Nodes.Remove(child));

            //for (int i = node.Nodes.Count - 1; i >= 0; i--) {
            //    LibraryFolderTreeNode child = (LibraryFolderTreeNode)node.Nodes[i];
            //    LibraryFolder childFolder = library.Find(x => x.Path == child.LibraryFolder?.Path);
            //    if (childFolder == null) {
            //        node.Nodes.Remove(child);
            //    }
            //}


            //サブフォルダに対して再帰処理
            foreach (string subDirectory in Directory.GetDirectories(folder.Path)) {
                LibraryFolder subFolder = library.Find(x => x.Path == subDirectory);
                this.EnumerateFolder(node, library, subFolder);
            }
        }
        #endregion

        #region Files
        private void RefreshFilesDisplay() {
            //Filesに存在しないNodeを削除
            //Libraryに存在しないNodeを削除
            //リストの複製を用意し、複製で削除判定。複製のForEach内で本体から削除する。(本体のForEachではないので、列挙中の削除例外は発生しない)
            List<OfficeFileTreeNode> cloneList = this.m_FileRootNode.Nodes.Cast<OfficeFileTreeNode>().ToList();
            cloneList.Where(node => !this.m_TargetProject.ExcelFiles.Any(file => file.FileName == node.TargetFile.FileName)).ToList()
                //このToListの時点で、Librariesに存在しないノードがリストアップされている
                .ForEach(node => this.m_FileRootNode.Nodes.Remove(node));

            //for (int i = this.m_FileRootNode.Nodes.Count - 1; i >= 0; i--) {
            //    OfficeFileTreeNode node = (OfficeFileTreeNode)this.m_FileRootNode.Nodes[i];
            //    if (!this.m_TargetProject.ExcelFiles.Any(x => x.FileName == node.TargetFile.FileName)) {
            //        this.m_FileRootNode.Nodes.Remove(node);
            //    }
            //}

            //FilesとNodeを同期
            for (int i = 0; i < this.m_TargetProject.ExcelFiles.Count; i++) {
                OfficeFile file = this.m_TargetProject.ExcelFiles[i];
                AppMain.logger.Debug($"{this.GetType().Name}.RefreshDisplay / Show File. Path={file.FileName}");

                OfficeFileTreeNode node = this.m_FileRootNode.Nodes.Cast<OfficeFileTreeNode>().FirstOrDefault(x => x.TargetFile.FileName == file.FileName);
                if (node == null) {
                    node = new OfficeFileTreeNode(file) { ImageIndex = 3, SelectedImageIndex = 3 };
                    this.m_FileRootNode.Nodes.Add(node);
                }
                node.TargetFile = file;
            }

            if (this.m_FileRootNode.Nodes.Count != 0) {
                this.m_FileRootNode.Nodes[0].EnsureVisible();
            }
        }
        #endregion

        #endregion

        #region FindNode
        /// <summary>
        /// 指定した条件に一致するOfficeFileTreeNodeを返します。
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public OfficeFileTreeNode FindOfficeFileNode(Predicate<ExplorerTreeNode> predicate) {
            return this.FindNode(this.m_FileRootNode, predicate) as OfficeFileTreeNode;
        }
        /// <summary>
        /// 指定した条件に一致するLibraryFolderTreeNodeを返します。
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public LibraryFolderTreeNode FindFolderNode(Predicate<ExplorerTreeNode> predicate) {
            return this.FindNode(this.m_LibraryRootNode, predicate) as LibraryFolderTreeNode;
        }

        private ExplorerTreeNode FindNode(ExplorerTreeNode node, Predicate<ExplorerTreeNode> predicate) {
            if (predicate(node)) {
                return node;
            }
            foreach (ExplorerTreeNode child in node.Nodes) {
                ExplorerTreeNode child1 = this.FindNode(child, predicate);
                if (child1 != null) {
                    return child1;
                }
            }
            return null;
        }


        #endregion

        #region OpenFolder
        private void OpenFolder(string folderPath,string commandName) {
            try {
                ProcessStartInfo info = new ProcessStartInfo() { FileName = "explorer", Arguments = folderPath };
                Process process = new Process() { StartInfo = info };
                process.Start();
            }
            catch (Exception ex) {
                string errorMessage = $"{this.GetType().Name}.{commandName} フォルダを開けませんでした。ExceptionType={ex.GetType().FullName}, Message={ex.Message}, Path={folderPath}";
                AppMain.logger.Error(errorMessage, ex);
                MessageBox.Show(errorMessage, "エクスプローラ起動エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        #endregion

    }
}
