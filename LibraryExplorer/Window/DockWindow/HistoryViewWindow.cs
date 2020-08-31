using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryExplorer.Common;
using LibraryExplorer.Data;
using WeifenLuo.WinFormsUI.Docking;

namespace LibraryExplorer.Window.DockWindow {

    /// <summary>
    /// 履歴情報ビューアを表します
    /// </summary>
    public partial class HistoryViewWindow : DockContent, IRefreshDisplay {

        #region フィールド(メンバ変数、プロパティ、イベント)

        #region Project
        private LibraryProject m_Project;
        /// <summary>
        /// Projectが変更された場合に発生するイベントです。
        /// </summary>
        public event EventHandler<EventArgs<LibraryProject>> ProjectChanged;
        /// <summary>
        /// Projectが変更された場合に呼び出されます。
        /// </summary>
        /// <param name="e">イベントパラメータ</param>
        protected void OnProjectChanged(EventArgs<LibraryProject> e) {
            this.ProjectChanged?.Invoke(this, e);
        }
        /// <summary>
        /// Projectを取得、設定します。
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public LibraryProject Project {
            get {
                return this.m_Project;
            }
            set {
                this.SetProperty(ref this.m_Project, value, ((oldValue) => {
                    if (this.ProjectChanged != null) {
                        this.OnProjectChanged(new EventArgs<LibraryProject>(oldValue));
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
        /// HistoryViewWindowオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public HistoryViewWindow() {
            InitializeComponent();

            this.Initialize();
        }

        private void Initialize() {
            this.ProjectChanged += this.HistoryViewWindow_ProjectChanged;
        }

        #endregion

        #region イベントハンドラ
        private void HistoryViewWindow_ProjectChanged(object sender, EventArgs<LibraryProject> e) {
            this.RefreshDisplay();
        }

        private void HistoryViewWindow_VisibleChanged(object sender, EventArgs e) {
            if (this.Visible) {
                this.RefreshDisplay();
            }
        }

        private void historyListView1_MouseDoubleClick(object sender, MouseEventArgs e) {
            this.OpenFolder();
        }

        #region ContextMenu1
        private void contextMenuStrip1_Opened(object sender, EventArgs e) {
            bool selected = this.historyListView1.SelectedItems.Count > 0;

            this.フォルダを開くOToolStripMenuItem.Enabled = this.CanOpenFolder;
            this.ファイルの場所を開くFToolStripMenuItem.Enabled = this.CanOpenFileLocation;
            this.削除DToolStripMenuItem.Enabled = selected;
        }

        private void フォルダを開くOToolStripMenuItem_Click(object sender, EventArgs e) {
            this.OpenFolder();
        }

        private void ファイルの場所を開くFToolStripMenuItem_Click(object sender, EventArgs e) {
            this.OpenFileLocation();
        }

        private void 削除DToolStripMenuItem_Click(object sender, EventArgs e) {
            this.DeleteFolder();
        }

        private void すべて選択AToolStripMenuItem_Click(object sender, EventArgs e) {
            this.SelectAll();
        }

        #endregion

        #endregion

        #region 表示の更新
        /// <summary>
        /// 表示を更新します。
        /// </summary>
        public void RefreshDisplay(bool keep = false) {
            string path = AppMain.g_AppMain.HistoryFolderPath;

            try {
                this.historyListView1.SuspendLayout();
                this.historyListView1.Items.Clear();

                foreach (string folder in Directory.GetDirectories(path)) {
                    string folderName = Path.GetFileName(folder);
                    OfficeFile relatedFile = this.Project?.ExcelFiles.Where(file => file.BackupPathList.Contains(folderName)).FirstOrDefault();
                    string fileName = Path.GetFileName(relatedFile?.FileName ?? "");
                    ListViewItem item = new ListViewItem(new string[] { folderName, fileName });

                    this.historyListView1.Items.Add(item);
                }
            }
            finally {
                this.historyListView1.ResumeLayout();
            }
        }

        #endregion

        #region メニューの本体
        /// <summary>
        /// [フォルダを開く]が有効かどうかを返します。
        /// </summary>
        private bool CanOpenFolder {
            get {
                return this.historyListView1.SelectedItems.Count == 1;
            }
        }
        /// <summary>
        /// [ファイルの場所を開く]が有効かどうかを返します。
        /// </summary>
        private bool CanOpenFileLocation {
            get {
                return this.historyListView1.SelectedItems.Count == 1 && this.historyListView1.SelectedItems[0].SubItems[1].Text != "";
            }
        }
        /// <summary>
        /// フォルダを開く
        /// </summary>
        private void OpenFolder() {
            if (this.historyListView1.SelectedItems.Count != 1) {
                return;
            }
            string name = this.historyListView1.SelectedItems[0].Text;
            string path = Path.Combine(AppMain.g_AppMain.HistoryFolderPath, name);
            //explorerの起動
            this.RunExplorer(path);
        }
        /// <summary>
        /// ファイルの場所を開く
        /// </summary>
        private void OpenFileLocation() {
            if (this.historyListView1.SelectedItems.Count != 1) {
                return;
            }
            string name = this.historyListView1.SelectedItems[0].Text;
            OfficeFile relatedFile = this.Project?.ExcelFiles.Where(file => file.BackupPathList.Contains(name)).FirstOrDefault();
            string filename = relatedFile?.FileName ?? "";

            if (File.Exists(filename)) {
                string folderName = Path.GetDirectoryName(filename);
                //explorerの起動
                this.RunExplorer(folderName);
            }
        }
        private void RunExplorer(string path) {
            if (Directory.Exists(path)) {
                using (Process p = Process.Start("explorer", $"\"{path}\"")) {
                }
            }
        }
        /// <summary>
        /// 削除
        /// </summary>
        private void DeleteFolder() {
            DialogResult result = MessageBox.Show("選択されたフォルダを削除します。よろしいですか？", "削除確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result == DialogResult.OK) {
                foreach (ListViewItem item in this.historyListView1.SelectedItems) {
                    string name = item.Text;
                    //関連ファイルの登録を削除
                    OfficeFile relatedFile = this.Project?.ExcelFiles.Where(file => file.BackupPathList.Contains(name)).FirstOrDefault();
                    relatedFile.BackupPathList.Remove(name);
                    //実フォルダの削除
                    string folderName = Path.Combine(AppMain.g_AppMain.HistoryFolderPath, name);
                    if (Directory.Exists(folderName)) {
                        Directory.Delete(folderName, true);
                    }
                }
                this.RefreshDisplay();
            }
        }
        /// <summary>
        /// すべて選択
        /// </summary>
        private void SelectAll() {
            foreach (ListViewItem item in this.historyListView1.Items) {
                item.Selected = true;
            }
        }
        #endregion
    }
}
