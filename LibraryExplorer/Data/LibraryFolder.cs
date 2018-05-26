using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kucl;

namespace LibraryExplorer.Data {

    /// <summary>
    /// Libraryを構成するフォルダを表すクラスデス。
    /// </summary>
    public class LibraryFolder {

        #region フィールド(メンバ変数、プロパティ、イベント)

        #region Path
        private string m_Path;
        /// <summary>
        /// Pathが変更された場合に発生するイベントです。
        /// </summary>
        public event EventHandler<EventArgs<string>> PathChanged;
        /// <summary>
        /// Pathが変更された場合に呼び出されます。
        /// </summary>
        /// <param name="e">イベントパラメータ</param>
        protected void OnPathChanged(EventArgs<string> e) {
            this.PathChanged?.Invoke(this, e);
        }
        /// <summary>
        /// Pathを取得、設定します。
        /// </summary>
        public string Path {
            get {
                return this.m_Path;
            }
            set {
                this.SetProperty(ref this.m_Path, value, ((oldValue) => {
                    if (this.PathChanged != null) {
                        this.OnPathChanged(new EventArgs<string>(oldValue));
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
        /// LibraryFolderオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public LibraryFolder() :base(){
        }

        #endregion

        #region イベントハンドラ

        #endregion


        #region Exist
        /// <summary>
        /// フォルダが存在するかどうかを返します。
        /// </summary>
        /// <returns></returns>
        public bool Exist() {
            return Directory.Exists(this.Path);
        }

        #endregion

        #region GetLibraryFiles
        /// <summary>
        /// このフォルダに含まれるライブラリファイルを全て取得します。
        /// </summary>
        /// <returns></returns>
        public List<LibraryFile> GetLibraryFiles() {
            //if (Directory.Exists(this.Path)) {
            //    return Directory.GetFiles(this.Path).Where(x => LibraryFile.IsTargetFile(x))
            //        .Select<string, LibraryFile>(x => LibraryFile.FromFile(x)).ToList();
            //}
            //return null;

            return this.GetLibraryFiles_Enumerable().ToList();
        }

        /// <summary>
        /// このフォルダに含まれるライブラリファイルを列挙するIEnumerableを返します。
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LibraryFile> GetLibraryFiles_Enumerable() {
            if (this.Exist()) {
                return Directory.GetFiles(this.Path).Where(x => LibraryFile.IsTargetFile(x))
                    .Select<string, LibraryFile>(x => LibraryFile.FromFile(x));
            }
            return Enumerable.Empty<LibraryFile>();
        } 
        #endregion

    }
}
