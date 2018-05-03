using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryExplorer.Data {

    /// <summary>
    /// 複数のライブラリ、複数のファイルをまとめたProjectを表します。
    /// Libraryは保存されますが、ファイルは実行時に読み込むため、
    /// Projectの情報は保存されず、実行時に動的に構成されます。
    /// </summary>
    public class LibraryProject {


        #region フィールド(メンバ変数、プロパティ、イベント)


        #region FolderClosedイベント
        /// <summary>
        /// Libraryが閉じられたときに発生するイベントです。
        /// </summary>
        public event EventHandler<EventArgs<Library>> FolderClosed;
        /// <summary>
        /// FolderClosedイベントを発生させます。
        /// </summary>
        /// <param name="e"></param>
        protected void OnFolderClosed(EventArgs<Library> e) {
            this.FolderClosed?.Invoke(this, e);
        } 
        #endregion

        #region FileClosedイベント
        /// <summary>
        /// OfficeFileが閉じられたときに発生するイベントです。
        /// </summary>
        public event EventHandler<EventArgs<OfficeFile>> FileClosed;
        /// <summary>
        /// FileClosedイベントを発生させます。
        /// </summary>
        /// <param name="e"></param>
        protected void OnFileClosed(EventArgs<OfficeFile> e) {
            this.FileClosed?.Invoke(this, e);
        } 
        #endregion


        #region Libraries
        private List<Library> m_Libraries;
        /// <summary>
        /// Librariesを取得します。
        /// </summary>
        public List<Library> Libraries {
            get {
                return this.m_Libraries;
            }
        }
        #endregion

        #region ExcelFiles
        private List<OfficeFile> m_ExcelFiles;
        /// <summary>
        /// ExcelFilesを取得します。
        /// </summary>
        public List<OfficeFile> ExcelFiles {
            get {
                return this.m_ExcelFiles;
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
        /// LibraryProjectオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public LibraryProject() {
            this.m_Libraries = new List<Library>();
            this.m_ExcelFiles = new List<OfficeFile>();
        }
        #endregion

        #region イベントハンドラ

        #endregion

        #region CloseFolder
        /// <summary>
        /// 指定したフォルダを閉じます。
        /// </summary>
        /// <param name="folder"></param>
        public void CloseFolder(LibraryFolder folder) {
            //選択されているフォルダパスを取得(LibraryFolder以外を選択しているとSelectedFolder==nullとなり、path==""となる)
            string path = folder?.Path ?? "";

            while (Directory.Exists(path)) {
                //LibraryのTargetFolderと一致したら、pathがLibraryのtopフォルダを示している
                Library lib = this.Libraries.FirstOrDefault(x => x.TargetFolder == path);
                if (lib != null) {
                    //ライブラリから削除
                    this.Libraries.Remove(lib);
                    //イベント発生
                    this.OnFolderClosed(new EventArgs<Library>(lib));
                    return;
                }
                //pathを親ディレクトリに変更
                path = Path.GetDirectoryName(path);
            }
        }
        #endregion

        #region CloseFile
        /// <summary>
        /// 指定したファイルを閉じます。
        /// </summary>
        /// <param name="file"></param>
        public void CloseFile(OfficeFile file) {
            string filename = file?.FileName ?? "";

            //OfficeFile file = this.m_Project.ExcelFiles.FirstOrDefault(x => x.FileName == filename);
            if (file != null) {
                this.ExcelFiles.Remove(file);

                //Closeメソッドを呼び出し、テンポラリフォルダを削除する
                file.Close();

                //イベント発生
                this.OnFileClosed(new EventArgs<OfficeFile>(file));
            }
        } 
        #endregion

    }




    //TODO:LibraryFolderもOfficeFileも、ILibraryProjectItemを実装する。LibraryFileControllerも不要になるが、ExcelFileのOutputLogRequestの伝達方法は要検討
    //ただし、ExplorerTreeは、LibraryFolderとOfficeFileで明確に区別するべきなので、現状通りでよい。

    /// <summary>
    /// LibraryProjectを構成する要素が実装するインターフェースです。
    /// </summary>
    public interface ILibraryProjectItem {


        /// <summary>
        /// 表示用のパスを取得します。
        /// </summary>
        string DisplayPath {get;}
        /// <summary>
        /// 表示用の名前を取得します。
        /// </summary>
        string DisplayName {get;}

        /// <summary>
        /// パスを取得します。
        /// </summary>
        string Path {get;}
        /// <summary>
        /// 名前を取得します。
        /// </summary>
        string Name {get;}
        

    }

}
