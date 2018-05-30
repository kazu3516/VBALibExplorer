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
using LibraryExplorer.Data;

namespace LibraryExplorer.Window.Dialog {

    /// <summary>
    /// Libraryの構成要素のプロパティを表示するダイアログデス。
    /// </summary>
    public partial class LibraryPropertyDialog : Form {


        #region フィールド(メンバ変数、プロパティ、イベント)

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

        #region TargetOfficeFile
        private OfficeFile m_TargetOfficeFile;
        /// <summary>
        /// TargetOfficeFileが変更された場合に発生するイベントです。
        /// </summary>
        public event EventHandler<EventArgs<OfficeFile>> TargetOfficeFileChanged;
        /// <summary>
        /// TargetOfficeFileが変更された場合に呼び出されます。
        /// </summary>
        /// <param name="e">イベントパラメータ</param>
        protected void OnTargetOfficeFileChanged(EventArgs<OfficeFile> e) {
            this.TargetOfficeFileChanged?.Invoke(this, e);
        }
        /// <summary>
        /// TargetOfficeFileを取得、設定します。
        /// </summary>
        public OfficeFile TargetOfficeFile {
            get {
                return this.m_TargetOfficeFile;
            }
            set {
                this.SetProperty(ref this.m_TargetOfficeFile, value, ((oldValue) => {
                    if (this.TargetOfficeFileChanged != null) {
                        this.OnTargetOfficeFileChanged(new EventArgs<OfficeFile>(oldValue));
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
        /// LibraryPropertyDialogオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public LibraryPropertyDialog() {
            InitializeComponent();

            this.Initialize();
        }

        private void Initialize() {

            this.TargetFolderChanged += this.LibraryPropertyDialog_TargetFolderChanged;
            this.TargetOfficeFileChanged += this.LibraryPropertyDialog_TargetOfficeFileChanged;
        }


        #endregion

        #region イベントハンドラ        

        #region LibraryPropertyDialog
        private void LibraryPropertyDialog_VisibleChanged(object sender, EventArgs e) {
            if (this.Visible) {
                this.tabControl1.SelectedIndex = 0;
                this.RefreshDisplay();
            }
        }

        #region Targetの変更
        private void LibraryPropertyDialog_TargetOfficeFileChanged(object sender, EventArgs<OfficeFile> e) {
            if (this.TargetOfficeFile != null) {
                this.TargetFolder = null;
                this.RefreshDisplay();
            }
        }

        private void LibraryPropertyDialog_TargetFolderChanged(object sender, EventArgs<LibraryFolder> e) {
            if (this.TargetFolder != null) {
                this.TargetOfficeFile = null;
                this.RefreshDisplay();
            }
        }
        #endregion

        #endregion

        #region ボタン

        /// <summary>
        /// [フォルダを開く]ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openHistoryFolderButton1_Click(object sender, EventArgs e) {
            this.OpenFolder();
        }
        /// <summary>
        /// [履歴の削除]ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteHistoryButton1_Click(object sender, EventArgs e) {
            this.DeleteHistory();
        }

        #endregion

        #region historyListView
        private void historyListView1_MouseDoubleClick(object sender, MouseEventArgs e) {
            this.OpenFolder();
        }
        private void historyListView1_SelectedIndexChanged(object sender, EventArgs e) {
            bool selected = this.historyListView1.SelectedItems.Count > 0;

            this.openHistoryFolderButton1.Enabled = selected;
            this.deleteHistoryButton1.Enabled = selected;
        }

        #endregion

        #region ContextMenu1
        private void contextMenuStrip1_Opened(object sender, EventArgs e) {
            bool selected = this.historyListView1.SelectedItems.Count > 0;

            this.フォルダを開くOToolStripMenuItem.Enabled = selected;
            this.削除DToolStripMenuItem.Enabled = selected;
        }

        private void フォルダを開くOToolStripMenuItem_Click(object sender, EventArgs e) {
            this.OpenFolder();
        }

        private void 削除DToolStripMenuItem_Click(object sender, EventArgs e) {
            this.DeleteHistory();
        }

        private void すべて選択AToolStripMenuItem_Click(object sender, EventArgs e) {
            foreach (ListViewItem item in this.historyListView1.Items) {
                item.Selected = true;
            }
        }

        #endregion

        #endregion
        
        #region RefreshDisplay

        private void RefreshDisplay() {
            if (this.TargetOfficeFile != null) {
                this.ShowOfficeFile();
            }
            if (this.TargetFolder != null) {
                this.ShowFolder();
            }
        }
        private void ShowOfficeFile() {
            this.nameLabel1.Text = "ファイル名";
            this.nameTextBox1.Text = Path.GetFileName(this.TargetOfficeFile.FileName);
            this.pathTextBox1.Text = this.TargetOfficeFile.FileName;
            this.exportDateTextBox1.Text = this.TargetOfficeFile.ExportDate?.ToString("yyyy/MM/dd HH:mm:ss") ?? "";

            this.historyListView1.SuspendLayout();
            this.historyListView1.Items.Clear();
            for (int i = 0; i < this.TargetOfficeFile.BackupPathList.Count; i++) {
                string path = this.TargetOfficeFile.BackupPathList[i];
                this.historyListView1.Items.Add(path);
            }
            this.historyListView1.ResumeLayout();

            //ボタンの初期状態はDisable
            this.openHistoryFolderButton1.Enabled = false;
            this.deleteHistoryButton1.Enabled = false;

            //インポート/エクスポート タブの表示
            if (!this.tabControl1.Controls.Contains(this.tabPage2)) {
                this.tabControl1.Controls.Add(this.tabPage2);
            }
        }
        private void ShowFolder() {
            this.nameLabel1.Text = "フォルダ名";
            this.nameTextBox1.Text = Path.GetFileName(this.TargetFolder.Path);
            this.pathTextBox1.Text = this.TargetFolder.Path;

            //インポート/エクスポート タブを隠す
            if (this.tabControl1.Controls.Contains(this.tabPage2)) {
                this.tabControl1.Controls.Remove(this.tabPage2);
            }
        } 
        #endregion

        #region OpenFolder
        private void OpenFolder() {
            if (this.historyListView1.SelectedItems.Count == 0) {
                return;
            }
            IList<string> folders = this.historyListView1.SelectedItems.Cast<ListViewItem>()
                .Select(item => Path.Combine(AppMain.g_AppMain.HistoryFolderPath, item.Text)).ToList();

            this.OnNotifyParentRequest(new OpenFolderRequestEventArgs(folders, this.GetType().Name));
        }
        #endregion

        #region DeleteHistory
        private void DeleteHistory() {
            if (this.historyListView1.SelectedItems.Count == 0) {
                return;
            }
            DialogResult result = MessageBox.Show("選択された履歴を削除します。よろしいですか？", "履歴の削除", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result == DialogResult.OK) {
                for (int i = this.historyListView1.SelectedItems.Count - 1; i >= 0; i--) {
                    ListViewItem item = this.historyListView1.SelectedItems[i];
                    this.TargetOfficeFile.DeleteHistory(item.Text);
                    this.historyListView1.Items.Remove(item);
                }
            }
        }
        #endregion

    }
}
