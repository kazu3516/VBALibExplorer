using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryExplorer.Data;

namespace LibraryExplorer.Control {

    /// <summary>
    /// ExplorerListで使用するListViewItemを表すクラスです。
    /// </summary>
    public class LibraryFileListViewItem :ListViewItem {


        #region フィールド(メンバ変数、プロパティ、イベント)


        #region LibraryFile
        private LibraryFile m_LibraryFile;
        /// <summary>
        /// LibraryFileが変更された場合に発生するイベントです。
        /// </summary>
        public event EventHandler<EventArgs<LibraryFile>> LibraryFileChanged;
        /// <summary>
        /// LibraryFileが変更された場合に呼び出されます。
        /// </summary>
        /// <param name="e">イベントパラメータ</param>
        protected void OnLibraryFileChanged(EventArgs<LibraryFile> e) {
            this.LibraryFileChanged?.Invoke(this, e);
        }
        /// <summary>
        /// LibraryFileを取得、設定します。
        /// </summary>
        public LibraryFile LibraryFile {
            get {
                return this.m_LibraryFile;
            }
            set {
                this.SetProperty(ref this.m_LibraryFile, value, ((oldValue) => {
                    if (this.LibraryFileChanged != null) {
                        this.OnLibraryFileChanged(new EventArgs<LibraryFile>(oldValue));
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
        /// LibraryFileListViewItemオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public LibraryFileListViewItem() {
        }
        #endregion

        #region イベントハンドラ

        #endregion


    }
}
