using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryExplorer.Data;
using WeifenLuo.WinFormsUI.Docking;

namespace LibraryExplorer.Window.DockWindow {

    /// <summary>
    /// LibraryFileの内容をプレビューするウインドウを表すクラスです。
    /// </summary>
    public partial class PreviewWindow : DockContent,IRefreshDisplay {


        #region フィールド(メンバ変数、プロパティ、イベント)


        #region TargetFile
        private LibraryFile m_TargetFile;
        /// <summary>
        /// TargetFileが変更された場合に発生するイベントです。
        /// </summary>
        public event EventHandler<EventArgs<LibraryFile>> TargetFileChanged;
        /// <summary>
        /// TargetFileが変更された場合に呼び出されます。
        /// </summary>
        /// <param name="e">イベントパラメータ</param>
        protected void OnTargetFileChanged(EventArgs<LibraryFile> e) {
            this.TargetFileChanged?.Invoke(this, e);
        }
        /// <summary>
        /// TargetFileを取得、設定します。
        /// </summary>
        public LibraryFile TargetFile {
            get {
                return this.m_TargetFile;
            }
            set {
                this.SetProperty(ref this.m_TargetFile, value, ((oldValue) => {
                    if (this.TargetFileChanged != null) {
                        this.OnTargetFileChanged(new EventArgs<LibraryFile>(oldValue));
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
        /// PreviewWindowオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public PreviewWindow() {
            InitializeComponent();

            this.TargetFileChanged += this.PreviewWindow_TargetFileChanged;
        }


        #endregion

        #region イベントハンドラ
        private void PreviewWindow_TargetFileChanged(object sender, EventArgs<LibraryFile> e) {
            this.RefreshDisplay();
        }


        #endregion

        /// <summary>
        /// 表示を更新します。
        /// </summary>
        /// <param name="keep"></param>
        public void RefreshDisplay(bool keep = false) {
            if (this.TargetFile == null) {
                this.label2.Text = "";
                this.richTextBox1.Text = "";
            }
            else {
                this.label2.Text = this.TargetFile.FileName;
                this.toolTip1.SetToolTip(this.label2, this.TargetFile.FileName);

                string source = this.TargetFile.ReadFile();

                if (!keep || this.richTextBox1.Text != source) {
                    this.richTextBox1.Text = source;
                }
            }
        }
    }
}
