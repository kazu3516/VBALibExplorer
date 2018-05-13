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
using LibraryExplorer.Common.ExTool;
using LibraryExplorer.Common.Request;
using LibraryExplorer.Controller;
using LibraryExplorer.Data;

namespace LibraryExplorer.Control {
    /// <summary>
    /// LibraryFileを一覧表示するコントロールを表すクラスです。
    /// </summary>
    public partial class LibraryExplorerList : UserControl,IOutputLogRequest {


        #region フィールド(メンバ変数、プロパティ、イベント)

        private bool m_SuspendedRefresh;

        private string m_DisplayedFolderPath;

        private ApplicationMessageQueue m_ApplicationMessageQueue;

        private TextEditor m_TextEditor;

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
        private LibraryFolder m_TargetFolder;
        /// <summary>
        /// TargetFolderが変更された場合に発生するイベントです。
        /// </summary>
        public event EventHandler<EventArgs<LibraryFolder>> TargetFolderChanged;
        /// <summary>
        /// TargetFolderが変更された場合に呼び出されます。
        /// </summary>
        /// <param name="e">イベントパラメータ</param>
        protected void OnTargetFolderChanged(EventArgs<LibraryFolder> e) {
            this.TargetFolderChanged?.Invoke(this, e);
        }
        /// <summary>
        /// TargetFolderを取得、設定します。
        /// </summary>
        public LibraryFolder TargetFolder {
            get {
                return this.m_TargetFolder;
            }
            set {
                this.SetProperty(ref this.m_TargetFolder, value, ((oldValue) => {
                    if (this.TargetFolderChanged != null) {
                        this.OnTargetFolderChanged(new EventArgs<LibraryFolder>(oldValue));
                    }
                }));
            }
        }
        #endregion

        #region SelectedItem
        private LibraryFileListViewItem m_SelectedItem;
        /// <summary>
        /// SelectedItemが変更された場合に発生するイベントです。
        /// </summary>
        public event EventHandler<EventArgs<LibraryFileListViewItem>> SelectedItemChanged;
        /// <summary>
        /// SelectedItemが変更された場合に呼び出されます。
        /// </summary>
        /// <param name="e">イベントパラメータ</param>
        protected void OnSelectedItemChanged(EventArgs<LibraryFileListViewItem> e) {
            this.SelectedItemChanged?.Invoke(this, e);
        }
        /// <summary>
        /// SelectedItemを取得、設定します。
        /// </summary>
        public LibraryFileListViewItem SelectedItem {
            get {
                return this.m_SelectedItem;
            }
            protected internal set {
                this.SetProperty(ref this.m_SelectedItem, value, ((oldValue) => {
                    if (this.SelectedItemChanged != null) {
                        this.OnSelectedItemChanged(new EventArgs<LibraryFileListViewItem>(oldValue));
                    }
                }));
            }
        }
        #endregion

        #region SelectedFile

        /// <summary>
        /// SelectedFileを取得、設定します。
        /// </summary>
        public LibraryFile SelectedFile {
            get {
                return this.SelectedItem?.LibraryFile;
            }
            set {
                if (value == null) {
                    this.listView1.SelectedIndices.Clear();
                    return;
                }
                LibraryFileListViewItem item = this.FindItem(x => x.LibraryFile?.FileName == value?.FileName);
                if (item != null) {
                    //this.listView1.SelectedItems.Clear();
                    item.Selected = true;
                }
                else {
                    throw new ArgumentException($"指定したファイルは、現在のLibraryFolderには存在しません。LibraryFolder={this.TargetFolder?.Path ?? ""} , FileName={value?.FileName ?? ""}");
                }
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
        /// LibraryExplorerListオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public LibraryExplorerList() {
            this.m_SuspendedRefresh = false;
            this.m_DisplayedFolderPath = "";
            this.m_TextEditor = new TextEditor();
            this.m_ApplicationMessageQueue = new ApplicationMessageQueue(this);
            this.m_ApplicationMessageQueue.Start();

            InitializeComponent();


            this.TargetFolderChanged += this.LibraryExplorerList_TargetFolderChanged;

        }




        #endregion

        #region イベントハンドラ

        private void LibraryExplorerList_TargetFolderChanged(object sender, EventArgs<LibraryFolder> e) {
            this.ShowTargetFolderPath(this.TargetFolder?.Path ?? "");
            this.RefreshDisplay();
        }


        #region ListView
        private void listView1_SelectedIndexChanged(object sender, EventArgs e) {
            if (this.listView1.SelectedIndices.Count != 1) {
                this.SelectedItem = null;
            }
            else {
                this.SelectedItem = (LibraryFileListViewItem)this.listView1.Items[this.listView1.SelectedIndices[0]];
            }
        }
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                if (this.m_SelectedItem != null && this.m_SelectedItem.LibraryFile != null) {
                    //選択されているファイルをエディタで開く
                    this.OpenFile(this.m_SelectedItem.LibraryFile);
                }
            }
        }

        #endregion

        #endregion

        #region ShowTargetFolderPath
        /// <summary>
        /// 指定した文字列をフォルダ名として表示します。
        /// </summary>
        /// <param name="targetPath"></param>
        protected virtual void ShowTargetFolderPath(string targetPath) {
            //ApplicationMessageQueueを使用し、Application.Idleのタイミングで表示を更新する。
            //FileControllerの非同期処理により、別スレッドから呼び出される可能性があるため、Invoke処理が必要な個所だが、
            //別スレッドの処理が完了し、Idleのタイミングで実行することにより回避
            this.m_ApplicationMessageQueue.AddMessage(new ApplicationMessage(() => {
                this.label2.Text = targetPath;
                this.toolTip1.SetToolTip(this.label2, targetPath);
                AppMain.logger.Debug($"{this.ParentForm?.GetType().Name} : Change TargetFolderPath : {targetPath}");
            }));
            //if (this.label2.InvokeRequired) {
            //    this.label2.Invoke(new Action(() => {
            //        this.label2.Text = targetPath;
            //    }));
            //}
            //else {
            //    this.label2.Text = targetPath;
            //}
            //if (this.InvokeRequired) {
            //    this.Invoke(new Action(() => {
            //        this.toolTip1.SetToolTip(this.label2, targetPath);
            //    }));
            //}
            //else {
            //    this.toolTip1.SetToolTip(this.label2, targetPath);
            //}
        } 
        #endregion

        #region RefreshDisplay
        /// <summary>
        /// 表示の更新
        /// </summary>
        /// <param name="keep"></param>
        public void RefreshDisplay(bool keep = false) {
            if (this.m_SuspendedRefresh) {
                return;
            }
            try {
                this.listView1.BeginUpdate();

                //アイテムの初期化が必要かどうかを判定し、初期化を行う。
                this.OnClearListViewItem(keep);

                //表示の更新
                this.OnRefreshDisplay(keep);
            }
            finally {
                this.listView1.EndUpdate();
            }
        }

        #region OnRefreshDisplay

        /// <summary>
        /// 表示の更新を行います。
        /// </summary>
        /// <param name="keep"></param>
        protected virtual void OnRefreshDisplay(bool keep) {
            string targetPath = this.TargetFolder?.Path ?? "";

            if (Directory.Exists(targetPath)) {
                List<LibraryFile> files = this.TargetFolder?.GetLibraryFiles();
                AppMain.logger.Debug($"{this.GetType().Name}.RefreshDisplay / Show Files. count={files.Count()}");

                //ListViewItemがfilesになければ削除
                this.OnRemoveDiscardedItem(files);

                //ListViewに無いFileを追加
                this.OnAddRequisiteItem(files);

                //列幅調整
                this.ResizeColumns();

                AppMain.logger.Debug($"{this.GetType().Name}.RefreshDisplay / Add ListViewItem complete. count={this.listView1.Items.Count}");
            }

        }

        #region ResizeColumns
        /// <summary>
        /// 列幅の調整を行います。
        /// 既定では0～3列目のみ調整を行います。
        /// </summary>
        protected virtual void ResizeColumns() {
            this.ResizeColumn(0);
            this.ResizeColumn(1);
            this.ResizeColumn(2);
            this.ResizeColumn(3);
        }
        /// <summary>
        /// 指定したindexで表される列の幅を調整します。
        /// 列幅は、ヘッダとリスト項目のうち長い方に変更されます。
        /// </summary>
        /// <param name="index"></param>
        protected void ResizeColumn(int index) {
            this.listView1.AutoResizeColumn(index, ColumnHeaderAutoResizeStyle.ColumnContent);
            int w0 = this.listView1.Columns[index].Width;
            this.listView1.AutoResizeColumn(index, ColumnHeaderAutoResizeStyle.HeaderSize);
            int w1 = this.listView1.Columns[index].Width;
            if (w0 < w1) {
                this.listView1.AutoResizeColumn(index, ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            else {
                this.listView1.AutoResizeColumn(index, ColumnHeaderAutoResizeStyle.ColumnContent);
            }

        }
        #endregion

        #region OnClearListViewItem
        /// <summary>
        /// ListViewItemの初期化を行います。
        /// このメソッドは、IsRequiredClearItemメソッドがTrueを返したときに、アイテムのクリア、パス表示の変更を行います。
        /// </summary>
        /// <param name="keep"></param>
        protected virtual void OnClearListViewItem(bool keep) {
            string targetPath = this.TargetFolder?.Path ?? "";

            //keep指定または表示フォルダが変わった場合、初期化
            if (this.IsRequiredClearItem(keep)) {
                AppMain.logger.Debug($"{this.GetType().Name}.RefreshDisplay / Clear Items.");
                //NOTE:先にSelectedIndices.Clearを呼ばないと、SelectedIndexChangedイベントが発生しないため、SelectedItem,SelectedFileがクリアされずに残る
                this.listView1.SelectedIndices.Clear();
                this.listView1.Items.Clear();
                this.m_DisplayedFolderPath = targetPath;
            }
        }
        /// <summary>
        /// RefreshDisplayメソッドにより、ListViewItemの初期化が必要かどうかを返します。
        /// </summary>
        /// <param name="keep"></param>
        /// <returns></returns>
        protected virtual bool IsRequiredClearItem(bool keep) {
            string targetPath = this.TargetFolder?.Path ?? "";
            return !keep || this.m_DisplayedFolderPath != targetPath;
        }

        #endregion

        #region OnRemoveDiscardedItem
        /// <summary>
        /// ファイルが存在しないListViewItemを削除します。
        /// </summary>
        /// <param name="files"></param>
        protected virtual void OnRemoveDiscardedItem(List<LibraryFile> files) {
            //リストの複製を用意し、複製で削除判定。複製のForEach内で本体から削除する。(本体のForEachではないので、列挙中の削除例外は発生しない)
            List<LibraryFileListViewItem> cloneList = this.listView1.Items.Cast<LibraryFileListViewItem>().ToList();
            cloneList.Where(x => !files.Any(f => f.FileName == x.LibraryFile.FileName)).ToList()
                //このToListの時点で、filesに存在しないアイテムがリストアップされている
                .ForEach(item => this.listView1.Items.Remove(item));
        }

        #endregion

        #region OnAddRequisiteItem
        /// <summary>
        /// ListViewItemが無いファイルが存在する場合、ListViewItemを追加します。
        /// </summary>
        /// <param name="files"></param>
        protected virtual void OnAddRequisiteItem(List<LibraryFile> files) {
            files.ForEach(f => {
                //対象ファイルならば、リストに存在するかどうかを確認
                LibraryFileListViewItem item = this.listView1.Items.Cast<LibraryFileListViewItem>().FirstOrDefault(x => x.LibraryFile?.FileName == f.FileName);
                if (item == null) {
                    //存在しない場合、新規作成
                    item = new LibraryFileListViewItem();
                    this.listView1.Items.Add(item);
                }
                item.LibraryFile = f;
                item.SubItems.Clear();
                item.Text = Path.GetFileName(f.FileName);
                item.SubItems.AddRange(new string[] { f.ModuleName, f.Package, f.Name, f.Function, f.Description, f.Reference, f.Implements, f.Revision });
            });

        } 
        #endregion


        #endregion

        #endregion

        #region Suspend/Resume Refresh
        //NOTE:Excelファイルが自動エクスポートではなくなったので、Suspend/Resume Refreshは不要かも。

        /// <summary>
        /// Refreshを中断します。このメソッドが呼ばれると、Refreshメソッドが呼び出されたとしても更新処理は行われません。
        /// 再開する場合、ResumeRefreshメソッドを呼び出してください。
        /// </summary>
        protected void SuspendRefresh() {
            this.m_SuspendedRefresh = true;
        }
        /// <summary>
        /// Refreshを再開する。forceRefreshにfalseを指定すると、このメソッド内からのRefresh呼び出しは行われません。
        /// </summary>
        /// <param name="keep"></param>
        /// <param name="forceRefresh"></param>
        protected void ResumeRefresh(bool keep,bool forceRefresh = true) {
            this.m_SuspendedRefresh = false;
            if (forceRefresh) {
                this.RefreshDisplay(keep);
            }
        }
        #endregion

        #region FindItem
        /// <summary>
        /// 指定した条件に一致するLibraryFileListViewItemを返します。
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public LibraryFileListViewItem FindItem(Predicate<LibraryFileListViewItem> predicate) {
            return this.listView1.Items.Cast<LibraryFileListViewItem>().FirstOrDefault(new Func<LibraryFileListViewItem, bool>(predicate));
        }

        #endregion

        #region OpenFile
        //TODO:OpenFileもNotifyParentRequestに乗せて、MainWindow側で実装するか検討(各ウインドウは表示系がメインの方がよいはず。LibraryFileControllerの要否と合わせて検討)
        private void OpenFile(LibraryFile file) {
            try {
                this.m_TextEditor.Start(new TextEditorInfo(file));
            }
            catch (ApplicationException ex) {
                MessageBox.Show(ex.Message, "エディタ起動エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        #endregion

    }
}
