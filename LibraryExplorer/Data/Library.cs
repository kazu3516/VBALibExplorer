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
    /// 複数のファイルで構成されたライブラリを表します。
    /// </summary>
    public class Library {


        #region フィールド(メンバ変数、プロパティ、イベント)
        private Tree<LibraryFolder> m_Tree;


        #region TargetFolder
        private string m_TargetFolder;
        /// <summary>
        /// TargetFolderを取得します。
        /// </summary>
        public string TargetFolder {
            get {
                return this.m_TargetFolder;
            }
        }
        #endregion


        #region RootFolder
        /// <summary>
        /// RootFolderを取得します。
        /// </summary>
        public LibraryFolder RootFolder {
            get {
                return this.m_Tree.RootItem.Value;
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
        /// Libraryオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="targetFolder"></param>
        protected Library(string targetFolder) {
            this.m_TargetFolder = targetFolder;

            this.m_Tree = new Tree<LibraryFolder>();
            this.m_Tree.RootItem.Value = new LibraryFolder() { Path = this.m_TargetFolder };

        }
        #endregion

        #region イベントハンドラ

        #endregion

        #region static FromFolder
        /// <summary>
        /// 指定したフォルダパスを使用して新しいLibraryクラスのインスタンスを作成します。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Library FromFolder(string path) {
            if (!Directory.Exists(path)) {
                return null;
            }

            Library library = new Library(path);
            library.Load();

            return library;
        }
        #endregion

        #region Load
        /// <summary>
        /// 設定されたフォルダパスからフォルダ構造を読み取ります。
        /// </summary>
        public void Load() {

            this.OnLoad(this.m_Tree.RootItem);
        }
        private void OnLoad(TreeItem<LibraryFolder> parent) {
            string path = parent.Value.Path;
            Directory.GetDirectories(path).
                Cast<string>().
                Select(x => new LibraryFolder() { Path = x }).
                ToList().
                ForEach(x => {
                    TreeItem<LibraryFolder> child = new TreeItem<LibraryFolder>(x);
                    parent.Children.Add(child);
                    this.OnLoad(child);
                });

        }

        #endregion

        #region Refresh
        /// <summary>
        /// Libraryを更新します。
        /// </summary>
        public void Refresh() {
            if (!Directory.Exists(this.m_TargetFolder)) {
                throw new DirectoryNotFoundException($"TopFolderが見つかりません。Path={this.m_TargetFolder}");
            }
            this.m_Tree.RootItem.Children.Clear();
            this.Load();
        }
        #endregion

        #region Find
        /// <summary>
        /// 指定した条件に一致するLibraryFolderを返します。
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public LibraryFolder Find(Predicate<LibraryFolder> predicate) {
            try {
                return this.m_Tree.Find(x => predicate(x.Value)).Value;
            }
            catch (Exception) {
                return null;
            }
        }
        #endregion

        #region FindAll
        /// <summary>
        /// 指定した条件に一致するLibraryFolderのコレクションを返します。
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IList<LibraryFolder> FindAll(Predicate<LibraryFolder> predicate) {
            return this.m_Tree.FindAll(x => predicate(x.Value)).Select(x => x.Value).ToList();
        } 
        #endregion

    }
}
