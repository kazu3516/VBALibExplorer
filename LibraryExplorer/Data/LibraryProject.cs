using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kucl.Xml;
using Kucl.Xml.XmlCfg;
using LibraryExplorer.Common;

namespace LibraryExplorer.Data {

    /// <summary>
    /// 複数のライブラリ、複数のファイルをまとめたProjectを表します。
    /// Libraryは保存されますが、ファイルは実行時に読み込むため、
    /// Projectの情報は保存されず、実行時に動的に構成されます。
    /// </summary>
    public class LibraryProject: IUseConfig {


        #region フィールド(メンバ変数、プロパティ、イベント)
        private UseConfigHelper m_ConfigHelper;


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
        private IList<Library> m_Libraries;
        /// <summary>
        /// Librariesを取得します。
        /// </summary>
        public IList<Library> Libraries {
            get {
                return this.m_Libraries;
            }
        }
        #endregion

        #region ExcelFiles
        private List<ExcelFile> m_ExcelFilesBackup;
        private ObservableCollection<OfficeFile> m_ExcelFiles;
        /// <summary>
        /// ExcelFilesを取得します。
        /// </summary>
        public IList<OfficeFile> ExcelFiles {
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
            this.m_ConfigHelper = new UseConfigHelper(this.CreateDefaultConfig());

            this.m_Libraries = new List<Library>();

            this.m_ExcelFilesBackup = new List<ExcelFile>();

            this.m_ExcelFiles = new ObservableCollection<OfficeFile>();
            this.m_ExcelFiles.CollectionChanged += this.M_ExcelFiles_CollectionChanged;
            
        }

        #endregion

        #region イベントハンドラ

        #region ExcelFiles_CollectionChanged
        private void M_ExcelFiles_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            //NotypyParentRequestのイベントハンドラを動的に登録/解除する
            switch (e.Action) {
                case NotifyCollectionChangedAction.Add:
                    foreach (ExcelFile file in e.NewItems) {
                        this.m_ExcelFilesBackup.Add(file);
                        this.ExcelFiles_OnAddItem(file);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (ExcelFile file in e.OldItems) {
                        this.m_ExcelFilesBackup.Remove(file);
                        this.ExcelFiles_OnRemoveItem(file);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (e.OldItems.Count != 0 && e.NewItems.Count != 0) {
                        ExcelFile oldFile = (ExcelFile)e.OldItems[0];
                        ExcelFile newFile = (ExcelFile)e.NewItems[0];
                        this.m_ExcelFilesBackup[e.OldStartingIndex] = newFile;
                        this.ExcelFiles_OnReplaceItem(e.OldStartingIndex, oldFile, newFile);
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    if (e.NewItems.Count != 0) {
                        ExcelFile file = (ExcelFile)e.NewItems[0];
                        this.m_ExcelFilesBackup.RemoveAt(e.OldStartingIndex);
                        this.m_ExcelFilesBackup.Insert(e.NewStartingIndex, file);
                        this.ExcelFiles_OnMoveItem(file, e.OldStartingIndex, e.NewStartingIndex);
                    }
                    //処理無し
                    break;
                case NotifyCollectionChangedAction.Reset:
                    this.ExcelFiles_OnClearItem(this.m_ExcelFilesBackup);
                    this.m_ExcelFilesBackup.Clear();
                    break;
            }
#if DEBUG
            System.Diagnostics.Debug.Assert(this.m_ExcelFiles.Count == this.m_ExcelFilesBackup.Count, "LibraryProject.ExcelFilesの内部整合性エラー");
#endif
        }
        private void ExcelFiles_OnAddItem(ExcelFile file) {
            //ExcelFilesにアイテムが追加された
            if (file != null) {
                file.NotifyParentRequest += this.ExcelFile_NotifyParentRequest;
            }

            string debugMessage = $"Project_ExcelFiles_Add_{file.FileName}";
            AppMain.logger.Debug(debugMessage);
        }


        private void ExcelFiles_OnRemoveItem(ExcelFile file) {
            //ExcelFilesからアイテムが削除された
            if (file != null) {
                file.NotifyParentRequest -= this.ExcelFile_NotifyParentRequest;
            }

            string debugMessage = $"Project_ExcelFiles_Remove_{file.FileName}";
            AppMain.logger.Debug(debugMessage);
        }
        private void ExcelFiles_OnReplaceItem(int index,ExcelFile oldFile, ExcelFile newFile) {
            //ExcelFilesのアイテムが置き換えられた
            if (oldFile != null) {
                oldFile.NotifyParentRequest -= this.ExcelFile_NotifyParentRequest;
            }
            if (newFile != null) {
                newFile.NotifyParentRequest += this.ExcelFile_NotifyParentRequest;
            }


            string debugMessage = $"Project_ExcelFiles_Replace_{oldFile.FileName}";
            AppMain.logger.Debug(debugMessage);
        }
        private void ExcelFiles_OnMoveItem(ExcelFile file, int oldIndex, int newIndex) {
            //ExcelFilesにアイテムが移動された


            string debugMessage = $"Project_ExcelFiles_Move_{file.FileName}";
            AppMain.logger.Debug(debugMessage);
        }
        private void ExcelFiles_OnClearItem(IList<ExcelFile> files) {
            //ExcelFilesにアイテムがクリアされた
            foreach (ExcelFile file in files) {
                if (file != null) {
                    file.NotifyParentRequest -= this.ExcelFile_NotifyParentRequest;
                }
            }

            string debugMessage = $"Project_ExcelFiles_Clear_{files.Count}";
            AppMain.logger.Debug(debugMessage);
        }
        #endregion

        #region ExcelFile
        private void ExcelFile_NotifyParentRequest(object sender, Common.Request.RequestEventArgs e) {

        }

        #endregion


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



        #region Config

        #region IUseConfig
        /// <summary>
        /// configとして保存するデフォルト設定を作成します。
        /// </summary>
        /// <returns></returns>
        public XmlConfigModel CreateDefaultConfig() {
            XmlConfigModel config = new XmlConfigModel();
            this.OnCreateDefaultConfig(config);
            return config;
        }
        /// <summary>
        /// 使用するConfigを取得または設定します。
        /// </summary>
        public XmlConfigModel Config {
            get {
                return this.m_ConfigHelper.Config;
            }
            set {
                this.m_ConfigHelper.Config = value;
            }
        }
        /// <summary>
        /// configを読み込んで適用します。
        /// </summary>
        /// <param name="value"></param>
        public void ApplyConfig(XmlConfigModel value) {
            this.m_ConfigHelper.Config = value;
            this.OnApplyConfig(value);
        }
        /// <summary>
        /// 現在の設定をconfigに反映します。
        /// </summary>
        /// <param name="config"></param>
        public void ReflectConfig(XmlConfigModel config) {
            this.OnReflectConfig(config);
        }
        /// <summary>
        /// configの値がデフォルト値かどうかを判定します。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsDefaultValue(string name, XmlContentsItem value) {
            return this.m_ConfigHelper.IsDefaultValue(name, value);
        }
        #endregion

        #region Configの適用と更新

        //Configの適用
        private void OnApplyConfig(XmlConfigModel config) {
            //LibraryFolders
            this.m_Libraries.Clear();
            int folder_count = this.m_ConfigHelper.GetIntValue("LibraryExplorer.project:Project.LibraryFolders.Count");
            for (int i = 0; i < folder_count; i++) {
                string path = this.m_ConfigHelper.GetStringValue($"LibraryExplorer.project:Project.LibraryFolders.{i + 1}");
                this.m_Libraries.Add(Library.FromFolder(path));
            }
            //OfficeFiles
            this.m_ExcelFiles.Clear();
            int file_count = this.m_ConfigHelper.GetIntValue("LibraryExplorer.project:Project.OfficeFiles.Count");
            for (int i = 0; i < file_count; i++) {
                string path = this.m_ConfigHelper.GetStringValue($"LibraryExplorer.project:Project.OfficeFiles.{i + 1}.Path");
                string exportPath = this.m_ConfigHelper.GetStringValue($"LibraryExplorer.project:Project.OfficeFiles.{i + 1}.ExportPath");
                string dateString = this.m_ConfigHelper.GetStringValue($"LibraryExplorer.project:Project.OfficeFiles.{i + 1}.ExportDate");
                DateTime? exportDate = (dateString != "" ? DateTime.Parse(dateString) : (DateTime?)null);

                OfficeFile file = new ExcelFile() { FileName = path,ExportDate = exportDate };
                file.CreateWorkspaceFolder(exportPath);

                this.m_ExcelFiles.Add(file);
            }

        }


        //Configの更新
        private void OnReflectConfig(XmlConfigModel config) {
            //LibraryFolders
            config.AddXmlContentsItem("LibraryExplorer.project:Project.LibraryFolders.Count", this.m_Libraries.Count);
            for (int i = 0; i < this.m_Libraries.Count; i++) {
                config.AddXmlContentsItem($"LibraryExplorer.project:Project.LibraryFolders.{i + 1}", this.m_Libraries[i].TargetFolder);
            }
            //OfficeFiels
            config.AddXmlContentsItem("LibraryExplorer.project:Project.OfficeFiles.Count", this.m_ExcelFiles.Count);
            for (int i = 0; i < this.m_ExcelFiles.Count; i++) {
                OfficeFile file = this.m_ExcelFiles[i];
                config.AddXmlContentsItem($"LibraryExplorer.project:Project.OfficeFiles.{i + 1}.Path", file.FileName);
                config.AddXmlContentsItem($"LibraryExplorer.project:Project.OfficeFiles.{i + 1}.ExportPath", file.WorkspaceFolderName);
                config.AddXmlContentsItem($"LibraryExplorer.project:Project.OfficeFiles.{i + 1}.ExportDate", file.ExportDate?.ToString() ?? "");
            }


        }

        //既定のConfig
        private void OnCreateDefaultConfig(XmlConfigModel config) {
            //LibraryProject
            config.AddXmlContentsItem("LibraryExplorer.project:Project.LibraryFolders.Count", 0);
            config.AddXmlContentsItem("LibraryExplorer.project:Project.OfficeFiles.Count", 0);
        }


        #endregion

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
