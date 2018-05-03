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
    /// LibraryFileを一覧表示するウインドウを表すクラスです。
    /// </summary>
    public partial class ExplorerListWindow : DockContent,IRefreshDisplay,IOutputLogRequest {


        #region フィールド(メンバ変数、プロパティ、イベント)


        #region SelectedItemChanged
        /// <summary>
        /// 選択されているアイテムが変更されたときに発生するイベントです。
        /// </summary>
        public event EventHandler<LibraryExplorerList.EventArgs<LibraryFileListViewItem>> SelectedItemChanged;
        /// <summary>
        /// SelectedItemChangedイベントを発生させます。
        /// </summary>
        /// <param name="e"></param>
        protected void OnSelectedItemChanged(LibraryExplorerList.EventArgs<LibraryFileListViewItem> e) {
            if (this.SelectedItemChanged != null) {
                this.SelectedItemChanged(this, e);
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


        #region TargetFolder
        /// <summary>
        /// TargetFolderを取得または設定します。
        /// </summary>
        public LibraryFolder TargetFolder {
            get {
                return this.libraryExplorerList1.TargetFolder;
            }
            protected set {
                this.libraryExplorerList1.TargetFolder = value;
            }
        }
        #endregion

        #region SelectedItem
        /// <summary>
        /// SelectedItemを取得または設定します。
        /// </summary>
        public LibraryFileListViewItem SelectedItem {
            get {
                return this.libraryExplorerList1.SelectedItem;
            }
            protected set {
                this.libraryExplorerList1.SelectedItem = value;
            }
        }
        #endregion

        #region SelectedFile

        /// <summary>
        /// SelectedFileを取得、設定します。
        /// </summary>
        public LibraryFile SelectedFile {
            get {
                return this.libraryExplorerList1.SelectedFile;
            }
            set {
                this.libraryExplorerList1.SelectedFile = value;
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
        /// ExplorerListWindowオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public ExplorerListWindow() {
            InitializeComponent();

            this.libraryExplorerList1.OutputLogRequest += this.LibraryExplorerList1_OutputLogRequest;
            this.libraryExplorerList1.SelectedItemChanged += this.LibraryExplorerList1_SelectedItemChanged;
        }

        #endregion

        #region イベントハンドラ
        private void LibraryExplorerList1_SelectedItemChanged(object sender, LibraryExplorerList.EventArgs<LibraryFileListViewItem> e) {
            this.OnSelectedItemChanged(e);
        }
        private void LibraryExplorerList1_OutputLogRequest(object sender, OutputLogRequestEventArgs e) {
            this.OnOutputLogRequest(e);
        }

        #endregion

        #region RefreshDisplay
        /// <summary>
        /// 表示を更新します。
        /// </summary>
        /// <param name="keep"></param>
        public void RefreshDisplay(bool keep = false) {
            AppMain.logger.Debug($"{this.GetType().Name}.RefreshDisplay / Start.");

            this.libraryExplorerList1.RefreshDisplay(keep);

            AppMain.logger.Debug($"{this.GetType().Name}.RefreshDisplay / Complete.");
        }

        #endregion


        #region FindItem
        /// <summary>
        /// 指定した条件に一致するLibraryFileListViewItemを返します。
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public LibraryFileListViewItem FindItem(Predicate<LibraryFileListViewItem> predicate) {
            return this.libraryExplorerList1.FindItem(predicate);
        }

        #endregion


        
    }
}
