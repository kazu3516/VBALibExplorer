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
using LibraryExplorer.Common;
using LibraryExplorer.Common.Request;
using LibraryExplorer.Control;
using LibraryExplorer.Data;
using WeifenLuo.WinFormsUI.Docking;

namespace LibraryExplorer.Window.DockWindow {

    /// <summary>
    /// Libraryをツリー表示するウィンドウを表すクラスです。
    /// </summary>
    public partial class ExplorerTreeWindow : DockContent,IRefreshDisplay,IOutputLogRequest {

        #region フィールド(メンバ変数、プロパティ、イベント)


        #region NotifyLibraryRequestイベント
        /// <summary>
        /// Libraryに対して要求を送信するイベントです。
        /// </summary>
        public event RequestEventHandler NotifyLibraryRequest;
        /// <summary>
        /// NotifyLibraryRequestイベントを発生させます。
        /// </summary>
        /// <param name="e"></param>
        protected void OnNotifyLibraryRequest(RequestEventArgs e) {
            this.NotifyLibraryRequest?.Invoke(this, e);
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
            if (this.RefreshLibrariesRequest != null) {
                this.RefreshLibrariesRequest(this, e);
            }
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

        #region TargetProject

        /// <summary>
        /// TargetProjectを取得、設定します。
        /// </summary>
        public LibraryProject TargetProject {
            get {
                return this.libraryExplorerTree1.TargetProject;
            }
            set {
                this.libraryExplorerTree1.TargetProject = value;
            }
        }


        #endregion


        #region SelectedFolder
        /// <summary>
        /// SelectedFolderPathを取得、設定します。
        /// </summary>
        public LibraryFolder SelectedFolder {
            get {
                return this.libraryExplorerTree1.SelectedFolder;
            }
            set {
                this.libraryExplorerTree1.SelectedFolder = value;
            }
        }
        #endregion


        #region SelectedFile
        /// <summary>
        /// SelectedFileを取得、設定します。
        /// </summary>
        public OfficeFile SelectedFile {
            get {
                return this.libraryExplorerTree1.SelectedFile;
            }
            set {
                this.libraryExplorerTree1.SelectedFile = value;
            }
        }
        #endregion



        #region SelectedNode


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
        /// SelectedNodeを取得します。
        /// </summary>
        public ExplorerTreeNode SelectedNode {
            get {
                return this.libraryExplorerTree1.SelectedNode;
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
        /// ExplorerTreeWindowオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public ExplorerTreeWindow() {
            InitializeComponent();

            this.Initialize();
        }

        private void Initialize() {
            this.libraryExplorerTree1.RefreshLibrariesRequest += this.LibraryExplorerTree1_RefreshLibrariesRequest;
            this.libraryExplorerTree1.SelectedNodeChanged += this.LibraryExplorerTree1_SelectedNodeChanged;
        }

        #endregion

        #region イベントハンドラ
        private void LibraryExplorerTree1_RefreshLibrariesRequest(object sender, EventArgs e) {
            this.OnRefreshLibrariesRequest(e);
        }

        private void LibraryExplorerTree1_SelectedNodeChanged(object sender, LibraryExplorerTree.EventArgs<ExplorerTreeNode> e) {
            this.OnSelectedNodeChanged(new EventArgs<ExplorerTreeNode>(e.OldValue));
        }

        /// <summary>
        /// コントロールで発生したLibraryへのRequestを中継します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void libraryExplorerTree1_NotifyLibraryRequest(object sender, RequestEventArgs e) {
            this.OnNotifyLibraryRequest(e);
        }

        #endregion


        #region RefreshDisplay
        /// <summary>
        /// 表示を更新します。
        /// </summary>
        /// <param name="keep"></param>
        public void RefreshDisplay(bool keep = false) {
            AppMain.logger.Debug($"{this.GetType().Name}.RefreshDisplay / Start.");

            this.libraryExplorerTree1.RefreshDisplay(keep);

            AppMain.logger.Debug($"{this.GetType().Name}.RefreshDisplay / Complete.");
        }



        #endregion

        #region FindNode
        /// <summary>
        /// 指定した条件に一致するOfficeFileTreeNodeを返します。
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public OfficeFileTreeNode FindOfficeFileNode(Predicate<ExplorerTreeNode> predicate) {
            return this.libraryExplorerTree1.FindOfficeFileNode(predicate);
        }
        /// <summary>
        /// 指定した条件に一致するLibrarFolderTreeNodeを返します。
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public LibraryFolderTreeNode FindFolderNode(Predicate<ExplorerTreeNode> predicate) {
            return this.libraryExplorerTree1.FindFolderNode(predicate);
        }


        #endregion

    }
}
