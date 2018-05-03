using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryExplorer.Data;

namespace LibraryExplorer.Window.Dialog {

    /// <summary>
    /// Libraryの構成要素のプロパティを表示するダイアログデス。
    /// </summary>
    public partial class LibraryPropertyDialog : Form {


        #region フィールド(メンバ変数、プロパティ、イベント)

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

        #region ボタン

        private void cancelButton_Click(object sender, EventArgs e) {
            //TODO:キャンセルボタンの実装
        }

        private void okButton_Click(object sender, EventArgs e) {
            //TODO:OKボタンの実装
        } 
        #endregion

        private void LibraryPropertyDialog_TargetOfficeFileChanged(object sender, EventArgs<OfficeFile> e) {
            if (this.TargetOfficeFile != null) {
                this.TargetFolder = null;

                this.nameLabel1.Text = "ファイル名";
                this.nameTextBox1.Text = Path.GetFileName(this.TargetOfficeFile.FileName);
                this.pathTextBox1.Text = this.TargetOfficeFile.FileName;
            }
        }

        private void LibraryPropertyDialog_TargetFolderChanged(object sender, EventArgs<LibraryFolder> e) {
            if (this.TargetFolder != null) {
                this.TargetOfficeFile = null;

                this.nameLabel1.Text = "フォルダ名";
                this.nameTextBox1.Text = Path.GetFileName(this.TargetFolder.Path);
                this.pathTextBox1.Text = this.TargetFolder.Path;
            }
        }

        #endregion
    }
}
