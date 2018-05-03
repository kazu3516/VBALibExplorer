using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryExplorer.Data;

namespace LibraryExplorer.Control {


    #region ExplorerTreeNode
    /// <summary>
    /// ExplorerTreeに表示するTreeNodeを表すクラスです。
    /// </summary>
    public class ExplorerTreeNode : TreeNode {

        #region フィールド(メンバ変数、プロパティ、イベント)

        #region NodeType
        private ExplorerTreeNodeType m_NodeType;
        /// <summary>
        /// NodeTypeを取得します。
        /// </summary>
        public ExplorerTreeNodeType NodeType {
            get {
                return this.m_NodeType;
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
        /// ExplorerTreeNodeオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public ExplorerTreeNode() : this(ExplorerTreeNodeType.None) {
        }
        /// <summary>
        /// ExplorerTreeNodeオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="text"></param>
        public ExplorerTreeNode(string text) : this() {
            this.Text = text;
        }
        /// <summary>
        /// ExplorerTreeNodeオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="nodeType"></param>
        protected ExplorerTreeNode(ExplorerTreeNodeType nodeType) {
            this.m_NodeType = nodeType;
        }
        #endregion

        #region イベントハンドラ

        #endregion

    }
    #endregion

    #region ExplorerTreeNodeType
    /// <summary>
    /// ExplorerTreeNodeの種類を表す列挙型です。
    /// </summary>
    public enum ExplorerTreeNodeType {
        /// <summary>
        /// 指定なし
        /// </summary>
        None,
        /// <summary>
        /// 通常のLibraryFolderです。
        /// </summary>
        LibraryFolder,
        /// <summary>
        /// OfficeFileからエクスポートされたフォルダです。
        /// </summary>
        OfficeFile,
        /// <summary>
        /// アプリケーション上で使用される仮想フォルダです。
        /// </summary>
        ApplicationFolder,
    }

    #endregion



    #region ApplicationFolderTreeNode
    /// <summary>
    /// Application上で仮想的に使用するフォルダノードを表します。
    /// </summary>
    public class ApplicationFolderTreeNode:ExplorerTreeNode {

        #region フィールド(メンバ変数、プロパティ、イベント)

        #endregion

        #region コンストラクタ
        /// <summary>
        /// ApplicationFolderTreeNodeクラスの新しいインタンスを初期化します。
        /// </summary>
        /// <param name="text"></param>
        public ApplicationFolderTreeNode(string text):base(ExplorerTreeNodeType.ApplicationFolder) {
            this.Text = text;
        }
        #endregion

        #region イベントハンドラ

        #endregion

    }
    #endregion

    #region LibraryFolderTreeNode
    /// <summary>
    /// ライブラリのフォルダノードを表します。
    /// </summary>
    public class LibraryFolderTreeNode : ExplorerTreeNode {

        #region フィールド(メンバ変数、プロパティ、イベント)

        #region LibraryFolder
        private LibraryFolder m_LibraryFolder;
        /// <summary>
        /// LibraryFolderが変更された場合に発生するイベントです。
        /// </summary>
        public event EventHandler<EventArgs<LibraryFolder>> LibraryFolderChanged;
        /// <summary>
        /// LibraryFolderが変更された場合に呼び出されます。
        /// </summary>
        /// <param name="e">イベントパラメータ</param>
        protected void OnLibraryFolderChanged(EventArgs<LibraryFolder> e) {
            this.LibraryFolderChanged?.Invoke(this, e);
        }
        /// <summary>
        /// LibraryFolderを取得、設定します。
        /// </summary>
        public LibraryFolder LibraryFolder {
            get {
                return this.m_LibraryFolder;
            }
            set {
                this.SetProperty(ref this.m_LibraryFolder, value, ((oldValue) => {
                    if (this.LibraryFolderChanged != null) {
                        this.OnLibraryFolderChanged(new EventArgs<LibraryFolder>(oldValue));
                    }
                }));
            }
        }
        #endregion

        #endregion

        #region コンストラクタ
        /// <summary>
        /// LibraryFolderTreeNodeオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public LibraryFolderTreeNode() : base(ExplorerTreeNodeType.LibraryFolder) {
            this.LibraryFolderChanged += this.LibraryFolderTreeNode_LibraryFolderChanged;
        }

        /// <summary>
        /// LibraryFolderTreeNodeオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="text"></param>
        public LibraryFolderTreeNode(string text) : this() {
            this.Text = text;
        }

        /// <summary>
        /// LibraryFolderTreeNodeオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="folder"></param>
        public LibraryFolderTreeNode(LibraryFolder folder) : this() {
            this.LibraryFolder = folder;
        }
        #endregion

        #region イベントハンドラ
        private void LibraryFolderTreeNode_LibraryFolderChanged(object sender, EventArgs<LibraryFolder> e) {
            this.Text = Path.GetFileName(this.LibraryFolder?.Path ?? "");
        }

        #endregion

    }
    #endregion

    #region OfficeFileTreeNode
    /// <summary>
    /// 参照しているオフィスファイルノードを表します。
    /// </summary>
    public class OfficeFileTreeNode : ExplorerTreeNode {

        #region フィールド(メンバ変数、プロパティ、イベント)
        
        #region TargetFile
        private OfficeFile m_TargetFile;
        /// <summary>
        /// TargetFileが変更された場合に発生するイベントです。
        /// </summary>
        public event EventHandler<EventArgs<OfficeFile>> TargetFileChanged;
        /// <summary>
        /// TargetFileが変更された場合に呼び出されます。
        /// </summary>
        /// <param name="e">イベントパラメータ</param>
        protected void OnTargetFileChanged(EventArgs<OfficeFile> e) {
            this.TargetFileChanged?.Invoke(this, e);
        }
        /// <summary>
        /// TargetFileを取得、設定します。
        /// </summary>
        public OfficeFile TargetFile {
            get {
                return this.m_TargetFile;
            }
            set {
                this.SetProperty(ref this.m_TargetFile, value, ((oldValue) => {
                    if (this.TargetFileChanged != null) {
                        this.OnTargetFileChanged(new EventArgs<OfficeFile>(oldValue));
                    }
                }));
            }
        }
        #endregion


        #endregion

        #region コンストラクタ
        /// <summary>
        /// OfficeFileTreeNodeオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public OfficeFileTreeNode() : base(ExplorerTreeNodeType.OfficeFile) {
            this.TargetFileChanged += this.ExcelFileTreeNode_TargetFileChanged;
        }
        /// <summary>
        /// OfficeFileTreeNodeオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="text"></param>
        public OfficeFileTreeNode(string text) : this() {
            this.Text = text;
        }
        /// <summary>
        /// OfficeFileTreeNodeオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="file"></param>
        public OfficeFileTreeNode(OfficeFile file) : this() {
            this.TargetFile = file;
        }
        #endregion

        #region イベントハンドラ
        private void ExcelFileTreeNode_TargetFileChanged(object sender, EventArgs<OfficeFile> e) {
            this.Text = Path.GetFileName(this.TargetFile.FileName);
        }

        #endregion

    } 
    #endregion
}
