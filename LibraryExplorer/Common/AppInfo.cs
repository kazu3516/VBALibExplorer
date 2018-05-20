using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kucl.Xml;
using Kucl.Xml.XmlCfg;

namespace LibraryExplorer.Common {

    /// <summary>
    /// アプリケーション設定を表すクラスです。
    /// </summary>
    public class AppInfo:IUseConfig,ICloneable {

        //IMPORTANT:【常時注意】AppInfoに設定が追加された場合、オプションダイアログでの編集が必要かどうか検討する。

        #region フィールド(メンバ変数、プロパティ、イベント)
        private UseConfigHelper m_ConfigHelper;

        #region MainWindowの位置とサイズ


        #region MainWindowWidth
        private int m_MainWindowWidth;
        /// <summary>
        /// MainWindowWidthが変更された場合に発生するイベントです。
        /// </summary>
        public event EventHandler<EventArgs<int>> MainWindowWidthChanged;
        /// <summary>
        /// MainWindowWidthが変更された場合に呼び出されます。
        /// </summary>
        /// <param name="e">イベントパラメータ</param>
        protected void OnMainWindowWidthChanged(EventArgs<int> e) {
            this.MainWindowWidthChanged?.Invoke(this, e);
        }
        /// <summary>
        /// MainWindowWidthを取得、設定します。
        /// </summary>
        public int MainWindowWidth {
            get {
                return this.m_MainWindowWidth;
            }
            set {
                this.SetProperty(ref this.m_MainWindowWidth, value, ((oldValue) => {
                    if (this.MainWindowWidthChanged != null) {
                        this.OnMainWindowWidthChanged(new EventArgs<int>(oldValue));
                    }
                }));
            }
        }
        #endregion

        #region MainWindowHeight
        private int m_MainWindowHeight;
        /// <summary>
        /// MainWindowHeightが変更された場合に発生するイベントです。
        /// </summary>
        public event EventHandler<EventArgs<int>> MainWindowHeightChanged;
        /// <summary>
        /// MainWindowHeightが変更された場合に呼び出されます。
        /// </summary>
        /// <param name="e">イベントパラメータ</param>
        protected void OnMainWindowHeightChanged(EventArgs<int> e) {
            this.MainWindowHeightChanged?.Invoke(this, e);
        }
        /// <summary>
        /// MainWindowHeightを取得、設定します。
        /// </summary>
        public int MainWindowHeight {
            get {
                return this.m_MainWindowHeight;
            }
            set {
                this.SetProperty(ref this.m_MainWindowHeight, value, ((oldValue) => {
                    if (this.MainWindowHeightChanged != null) {
                        this.OnMainWindowHeightChanged(new EventArgs<int>(oldValue));
                    }
                }));
            }
        }
        #endregion

        #region MainWindowLeft
        private int m_MainWindowLeft;
        /// <summary>
        /// MainWindowLeftが変更された場合に発生するイベントです。
        /// </summary>
        public event EventHandler<EventArgs<int>> MainWindowLeftChanged;
        /// <summary>
        /// MainWindowLeftが変更された場合に呼び出されます。
        /// </summary>
        /// <param name="e">イベントパラメータ</param>
        protected void OnMainWindowLeftChanged(EventArgs<int> e) {
            this.MainWindowLeftChanged?.Invoke(this, e);
        }
        /// <summary>
        /// MainWindowLeftを取得、設定します。
        /// </summary>
        public int MainWindowLeft {
            get {
                return this.m_MainWindowLeft;
            }
            set {
                this.SetProperty(ref this.m_MainWindowLeft, value, ((oldValue) => {
                    if (this.MainWindowLeftChanged != null) {
                        this.OnMainWindowLeftChanged(new EventArgs<int>(oldValue));
                    }
                }));
            }
        }
        #endregion

        #region MainWindowTop
        private int m_MainWindowTop;
        /// <summary>
        /// MainWindowTopが変更された場合に発生するイベントです。
        /// </summary>
        public event EventHandler<EventArgs<int>> MainWindowTopChanged;
        /// <summary>
        /// MainWindowTopが変更された場合に呼び出されます。
        /// </summary>
        /// <param name="e">イベントパラメータ</param>
        protected void OnMainWindowTopChanged(EventArgs<int> e) {
            this.MainWindowTopChanged?.Invoke(this, e);
        }
        /// <summary>
        /// MainWindowTopを取得、設定します。
        /// </summary>
        public int MainWindowTop {
            get {
                return this.m_MainWindowTop;
            }
            set {
                this.SetProperty(ref this.m_MainWindowTop, value, ((oldValue) => {
                    if (this.MainWindowTopChanged != null) {
                        this.OnMainWindowTopChanged(new EventArgs<int>(oldValue));
                    }
                }));
            }
        }
        #endregion


        #endregion

        #region TargetExtentions
        private List<string> m_TargetExtentions;
        /// <summary>
        /// TargetExtentionsを取得します。
        /// </summary>
        public List<string> TargetExtentions {
            get {
                return this.m_TargetExtentions;
            }
        }
        #endregion

        #region HasOtherFilesExtentions
        private Dictionary<string,List<string>> m_HasOtherFilesExtentions;
        /// <summary>
        /// HasOtherFilesExtentionsを取得します。
        /// </summary>
        public Dictionary<string,List<string>> HasOtherFilesExtentions {
            get {
                return this.m_HasOtherFilesExtentions;
            }
        }
        #endregion

        #region Library関連

        #region LibraryHeaderStart
        private string m_LibraryHeaderStart;
        /// <summary>
        /// LibraryHeaderStartが変更された場合に発生するイベントです。
        /// </summary>
        public event EventHandler<EventArgs<string>> LibraryHeaderStartChanged;
        /// <summary>
        /// LibraryHeaderStartが変更された場合に呼び出されます。
        /// </summary>
        /// <param name="e">イベントパラメータ</param>
        protected void OnLibraryHeaderStartChanged(EventArgs<string> e) {
            this.LibraryHeaderStartChanged?.Invoke(this, e);
        }
        /// <summary>
        /// LibraryHeaderStartを取得、設定します。
        /// </summary>
        public string LibraryHeaderStart {
            get {
                return this.m_LibraryHeaderStart;
            }
            set {
                this.SetProperty(ref this.m_LibraryHeaderStart, value, ((oldValue) => {
                    if (this.LibraryHeaderStartChanged != null) {
                        this.OnLibraryHeaderStartChanged(new EventArgs<string>(oldValue));
                    }
                }));
            }
        }
        #endregion

        #region LibraryHeaderEnd
        private string m_LibraryHeaderEnd;
        /// <summary>
        /// LibraryHeaderEndが変更された場合に発生するイベントです。
        /// </summary>
        public event EventHandler<EventArgs<string>> LibraryHeaderEndChanged;
        /// <summary>
        /// LibraryHeaderEndが変更された場合に呼び出されます。
        /// </summary>
        /// <param name="e">イベントパラメータ</param>
        protected void OnLibraryHeaderEndChanged(EventArgs<string> e) {
            this.LibraryHeaderEndChanged?.Invoke(this, e);
        }
        /// <summary>
        /// LibraryHeaderEndを取得、設定します。
        /// </summary>
        public string LibraryHeaderEnd {
            get {
                return this.m_LibraryHeaderEnd;
            }
            set {
                this.SetProperty(ref this.m_LibraryHeaderEnd, value, ((oldValue) => {
                    if (this.LibraryHeaderEndChanged != null) {
                        this.OnLibraryHeaderEndChanged(new EventArgs<string>(oldValue));
                    }
                }));
            }
        }
        #endregion



        #endregion

        #region LibraryProject関連

        #region LibraryFolders
        private List<string> m_LibraryFolders;
        /// <summary>
        /// LibraryFoldersを取得します。
        /// </summary>
        public List<string> LibraryFolders {
            get {
                return this.m_LibraryFolders;
            }
        }
        #endregion

        #region OfficeFiles
        private List<string> m_OfficeFiles;
        /// <summary>
        /// OfficeFilesを取得、設定します。
        /// </summary>
        public List<string> OfficeFiles {
            get {
                return this.m_OfficeFiles;
            }
            set {
                this.m_OfficeFiles = value;
            }
        }
        #endregion

        #region OfficeFileExportDates
        private List<DateTime?> m_OfficeFileExportDates;
        /// <summary>
        /// OfficeFileExportDatesを取得、設定します。
        /// </summary>
        public List<DateTime?> OfficeFileExportDates {
            get {
                return this.m_OfficeFileExportDates;
            }
            set {
                this.m_OfficeFileExportDates = value;
            }
        }
        #endregion



        #endregion

        #region エディタ設定

        #region EditorPath
        private string m_EditorPath;
        /// <summary>
        /// EditorPathが変更された場合に発生するイベントです。
        /// </summary>
        public event EventHandler<EventArgs<string>> EditorPathChanged;
        /// <summary>
        /// EditorPathが変更された場合に呼び出されます。
        /// </summary>
        /// <param name="e">イベントパラメータ</param>
        protected void OnEditorPathChanged(EventArgs<string> e) {
            this.EditorPathChanged?.Invoke(this, e);
        }
        /// <summary>
        /// EditorPathを取得、設定します。
        /// </summary>
        public string EditorPath {
            get {
                return this.m_EditorPath;
            }
            set {
                this.SetProperty(ref this.m_EditorPath, value, ((oldValue) => {
                    if (this.EditorPathChanged != null) {
                        this.OnEditorPathChanged(new EventArgs<string>(oldValue));
                    }
                }));
            }
        }
        #endregion

        #region EditorArguments
        private string m_EditorArguments;
        /// <summary>
        /// EditorArgumentsが変更された場合に発生するイベントです。
        /// </summary>
        public event EventHandler<EventArgs<string>> EditorArgumentsChanged;
        /// <summary>
        /// EditorArgumentsが変更された場合に呼び出されます。
        /// </summary>
        /// <param name="e">イベントパラメータ</param>
        protected void OnEditorArgumentsChanged(EventArgs<string> e) {
            this.EditorArgumentsChanged?.Invoke(this, e);
        }
        /// <summary>
        /// EditorArgumentsを取得、設定します。
        /// </summary>
        public string EditorArguments {
            get {
                return this.m_EditorArguments;
            }
            set {
                this.SetProperty(ref this.m_EditorArguments, value, ((oldValue) => {
                    if (this.EditorArgumentsChanged != null) {
                        this.OnEditorArgumentsChanged(new EventArgs<string>(oldValue));
                    }
                }));
            }
        }
        #endregion

        #endregion

        #region 比較ツール設定

        #region DiffToolPath
        private string m_DiffToolPath;
        /// <summary>
        /// DiffToolPathが変更された場合に発生するイベントです。
        /// </summary>
        public event EventHandler<EventArgs<string>> DiffToolPathChanged;
        /// <summary>
        /// DiffToolPathが変更された場合に呼び出されます。
        /// </summary>
        /// <param name="e">イベントパラメータ</param>
        protected void OnDiffToolPathChanged(EventArgs<string> e) {
            this.DiffToolPathChanged?.Invoke(this, e);
        }
        /// <summary>
        /// DiffToolPathを取得、設定します。
        /// </summary>
        public string DiffToolPath {
            get {
                return this.m_DiffToolPath;
            }
            set {
                this.SetProperty(ref this.m_DiffToolPath, value, ((oldValue) => {
                    if (this.DiffToolPathChanged != null) {
                        this.OnDiffToolPathChanged(new EventArgs<string>(oldValue));
                    }
                }));
            }
        }
        #endregion

        #region DiffToolArguments
        private string m_DiffToolArguments;
        /// <summary>
        /// DiffToolArgumentsが変更された場合に発生するイベントです。
        /// </summary>
        public event EventHandler<EventArgs<string>> DiffToolArgumentsChanged;
        /// <summary>
        /// DiffToolArgumentsが変更された場合に呼び出されます。
        /// </summary>
        /// <param name="e">イベントパラメータ</param>
        protected void OnDiffToolArgumentsChanged(EventArgs<string> e) {
            this.DiffToolArgumentsChanged?.Invoke(this, e);
        }
        /// <summary>
        /// DiffToolArgumentsを取得、設定します。
        /// </summary>
        public string DiffToolArguments {
            get {
                return this.m_DiffToolArguments;
            }
            set {
                this.SetProperty(ref this.m_DiffToolArguments, value, ((oldValue) => {
                    if (this.DiffToolArgumentsChanged != null) {
                        this.OnDiffToolArgumentsChanged(new EventArgs<string>(oldValue));
                    }
                }));
            }
        }
        #endregion

        #endregion

        #region Script

        #region ExcelModuleExportScriptName
        private string m_ExcelModuleExportScriptName;
        /// <summary>
        /// ExcelModuleExportScriptNameを取得、設定します。
        /// </summary>
        public string ExcelModuleExportScriptName {
            get {
                return this.m_ExcelModuleExportScriptName;
            }
            set {
                this.m_ExcelModuleExportScriptName = value;
            }
        }
        #endregion

        #region ExcelModuleImportScriptName
        private string m_ExcelModuleImportScriptName;
        /// <summary>
        /// ExcelModuleImportScriptNameを取得、設定します。
        /// </summary>
        public string ExcelModuleImportScriptName {
            get {
                return this.m_ExcelModuleImportScriptName;
            }
            set {
                this.m_ExcelModuleImportScriptName = value;
            }
        }
        #endregion

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
        /// AppInfoクラスの新しいインスタンスを初期化します。
        /// </summary>
        public AppInfo() {
            this.m_ConfigHelper = new UseConfigHelper(this.CreateDefaultConfig());

            this.m_HasOtherFilesExtentions = new Dictionary<string, List<string>>();
            this.m_TargetExtentions = new List<string>();
            this.m_LibraryFolders = new List<string>();
            this.m_OfficeFiles = new List<string>();
            this.m_OfficeFileExportDates = new List<DateTime?>();

            this.Initialize();

        }

        private void Initialize() {
            //NOTE:AppInfoに設定される値を制限する場合、イベントハンドラを登録する
            this.MainWindowHeightChanged += this.AppInfo_MainWindowHeightChanged;
            this.MainWindowWidthChanged += this.AppInfo_MainWindowWidthChanged;
            this.MainWindowLeftChanged += this.AppInfo_MainWindowLeftChanged;
            this.MainWindowTopChanged += this.AppInfo_MainWindowTopChanged;
            
        }

        #endregion

        #region イベントハンドラ


        #region MainWindowの位置とサイズ
        //MainWindowの位置とサイズは0～ディスプレイサイズに収まるようにする。

        private void AppInfo_MainWindowLeftChanged(object sender, EventArgs<int> e) {
            if (this.MainWindowLeft < 0 || System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width < this.MainWindowLeft) {
                this.m_MainWindowLeft = e.OldValue;
            }
        }

        private void AppInfo_MainWindowTopChanged(object sender, EventArgs<int> e) {
            if (this.MainWindowTop < 0 || System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height < this.MainWindowTop) {
                this.m_MainWindowTop = e.OldValue;
            }
        }

        private void AppInfo_MainWindowWidthChanged(object sender, EventArgs<int> e) {
            if (this.MainWindowWidth < 0 || System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width < this.MainWindowWidth) {
                this.m_MainWindowWidth = e.OldValue;
            }
        }

        private void AppInfo_MainWindowHeightChanged(object sender, EventArgs<int> e) {
            if (this.MainWindowHeight < 0 || System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height < this.MainWindowHeight) {
                this.m_MainWindowHeight = e.OldValue;
            }
        } 
        #endregion
        
        #endregion

        #region CopyFrom
        /// <summary>
        /// 指定したインスタンスからこのインスタンスにデータをコピーします。
        /// このメソッドはAppInfoクラスのディープコピーを作成するために使用されます。
        /// </summary>
        /// <param name="srcAppInfo"></param>
        public void CopyFrom(AppInfo srcAppInfo) {
            //IMPORTANT:【常時注意】AppInfoにコピーするべきフィールドが追加された場合、このメソッドにも追加する。

            //対象拡張子
            this.m_TargetExtentions = new List<string>(srcAppInfo.m_TargetExtentions);
            this.m_HasOtherFilesExtentions = new Dictionary<string, List<string>>(srcAppInfo.m_HasOtherFilesExtentions);

            //ライブラリ
            this.m_LibraryHeaderStart = srcAppInfo.m_LibraryHeaderStart;
            this.m_LibraryHeaderEnd = srcAppInfo.m_LibraryHeaderEnd;

            //LibraryProject
            this.m_LibraryFolders = new List<string>(srcAppInfo.m_LibraryFolders);
            this.m_OfficeFiles = new List<string>(srcAppInfo.m_OfficeFiles);
            this.m_OfficeFileExportDates = new List<DateTime?>(srcAppInfo.m_OfficeFileExportDates);

            //エディタ設定
            this.m_EditorPath = srcAppInfo.m_EditorPath;
            this.m_EditorArguments = srcAppInfo.m_EditorArguments;

            //比較ツール設定
            this.m_DiffToolPath = srcAppInfo.m_DiffToolPath;
            this.m_DiffToolArguments = srcAppInfo.m_DiffToolArguments;

            //スクリプト
            this.m_ExcelModuleExportScriptName = srcAppInfo.m_ExcelModuleExportScriptName;
            this.m_ExcelModuleImportScriptName = srcAppInfo.m_ExcelModuleImportScriptName;

            //MainWindowの位置とサイズ
            this.m_MainWindowWidth = srcAppInfo.m_MainWindowWidth;
            this.m_MainWindowHeight = srcAppInfo.m_MainWindowHeight;
            this.m_MainWindowTop = srcAppInfo.m_MainWindowTop;
            this.m_MainWindowLeft = srcAppInfo.m_MainWindowLeft;
        } 
        #endregion

        #region ICloneableインターフェースの実装
        /// <summary>
        /// AppInfoオブジェクトのディープコピーを作成します。
        /// </summary>
        /// <returns></returns>
        public AppInfo Clone() {
            AppInfo clone = new AppInfo();
            clone.CopyFrom(this);
            return clone;
        }
        object ICloneable.Clone() {
            return this.Clone();
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
        public bool IsDefaultValue(string name,XmlContentsItem value) {
            return this.m_ConfigHelper.IsDefaultValue(name,value);
        }
        #endregion

        #region Configの適用と更新

        //Configの適用
        private void OnApplyConfig(XmlConfigModel config) {
            //対象拡張子
            this.OnApplyTargetExtentions();
            this.OnApplyHasOtherFilesExtentions();
            
            //Library関連
            this.OnApplyHeaderInfo();

            //エディタ設定
            this.OnApplyEditorInfo();

            //比較ツール設定
            this.OnApplyDiffToolInfo();

            //スクリプト
            this.OnApplyScriptInfo();

            //MainWindowの位置とサイズ
            this.OnApplyWindowBounds();

        }


        //Configの更新
        private void OnReflectConfig(XmlConfigModel config) {
            //対象拡張子
            this.OnReflectTargetExtentions(config);
            this.OnReflectHasOtherFilesExtentions(config);
            
            //Library関連
            this.OnReflectHeaderInfo(config);

            //エディタ設定
            this.OnReflectEditorInfo(config);

            //比較ツール設定
            this.OnReflectDiffToolInfo(config);

            //スクリプト
            this.OnReflectScriptInfo(config);

            //MainWindowの位置とサイズ
            this.OnReflectWindowBounds(config);

        }

        #region 対象拡張子
        /// <summary>
        /// 対象拡張子
        /// </summary>
        private void OnApplyTargetExtentions() {
            this.m_TargetExtentions.Clear();
            int ext_count = this.m_ConfigHelper.GetIntValue("LibraryExplorer.setting:Common.TargetExtentions.Count");
            for (int i = 0; i < ext_count; i++) {
                string ext = this.m_ConfigHelper.GetStringValue($"LibraryExplorer.setting:Common.TargetExtentions.{i + 1}");
                this.m_TargetExtentions.Add(ext);
            }
        } 
        /// <summary>
        /// 複数ファイルで構成されるファイル
        /// </summary>
        private void OnApplyHasOtherFilesExtentions() {
            this.m_HasOtherFilesExtentions.Clear();
            int ext_count2 = this.m_ConfigHelper.GetIntValue("LibraryExplorer.setting:Common.HasOtherFilesExtentions.Count");
            for (int i = 0; i < ext_count2; i++) {
                string ext = this.m_ConfigHelper.GetStringValue($"LibraryExplorer.setting:Common.HasOtherFilesExtentions.{i + 1}.Target");
                int ext_count3 = this.m_ConfigHelper.GetIntValue($"LibraryExplorer.setting:Common.HasOtherFilesExtentions.{i + 1}.Count");
                List<string> ext_list = new List<string>();
                for (int j = 0; j < ext_count3; j++) {
                    string ext2 = this.m_ConfigHelper.GetStringValue($"LibraryExplorer.setting:Common.HasOtherFilesExtentions.{i + 1}.{j + 1}");
                    ext_list.Add(ext2);
                }
                this.m_HasOtherFilesExtentions.Add(ext, ext_list);
            }
        }


        /// <summary>
        /// 対象拡張子
        /// </summary>
        /// <param name="config"></param>
        private void OnReflectTargetExtentions(XmlConfigModel config) {
            config.AddXmlContentsItem("LibraryExplorer.setting:Common.TargetExtentions.Count", this.m_TargetExtentions.Count);
            for (int i = 0; i < this.m_TargetExtentions.Count; i++) {
                config.AddXmlContentsItem($"LibraryExplorer.setting:Common.TargetExtentions.{i + 1}", this.m_TargetExtentions[i]);
            }
        }
        /// <summary>
        /// 複数ファイルで構成されるファイル
        /// </summary>
        /// <param name="config"></param>
        private void OnReflectHasOtherFilesExtentions(XmlConfigModel config) {
            config.AddXmlContentsItem("LibraryExplorer.setting:Common.HasOtherFilesExtentions.Count", this.m_HasOtherFilesExtentions.Count);
            for (int i = 0; i < this.m_HasOtherFilesExtentions.Keys.Count; i++) {
                string key = this.m_HasOtherFilesExtentions.Keys.ElementAt(i);
                config.AddXmlContentsItem($"LibraryExplorer.setting:Common.HasOtherFilesExtentions.{i + 1}.Target", key);
                List<string> list = this.m_HasOtherFilesExtentions[key];
                config.AddXmlContentsItem($"LibraryExplorer.setting:Common.HasOtherFilesExtentions.{i + 1}.Count", list.Count);
                for (int j = 0; j < list.Count; j++) {
                    config.AddXmlContentsItem($"LibraryExplorer.setting:Common.HasOtherFilesExtentions.{i + 1}.{j + 1}", list[j]);
                }
            }
        }

        #endregion

        #region Library関連
        /// <summary>
        /// ライブラリヘッダ
        /// </summary>
        private void OnApplyHeaderInfo() {
            this.m_LibraryHeaderStart = this.m_ConfigHelper.GetStringValue("LibraryExplorer.setting:Library.Header.Start");
            this.m_LibraryHeaderEnd = this.m_ConfigHelper.GetStringValue("LibraryExplorer.setting:Library.Header.End");
        }


        /// <summary>
        /// ライブラリヘッダ
        /// </summary>
        /// <param name="config"></param>
        private void OnReflectHeaderInfo(XmlConfigModel config) {
            config.AddXmlContentsItem("LibraryExplorer.setting:Library.Header.Start", this.m_LibraryHeaderStart);
            config.AddXmlContentsItem("LibraryExplorer.setting:Library.Header.End", this.m_LibraryHeaderEnd);
        }

        #endregion

        #region エディタ設定
        /// <summary>
        /// エディタ設定
        /// </summary>
        private void OnApplyEditorInfo() {
            this.m_EditorPath = this.m_ConfigHelper.GetStringValue("LibraryExplorer.setting:Editor.Path");
            this.m_EditorArguments = this.m_ConfigHelper.GetStringValue("LibraryExplorer.setting:Editor.Arguments");
        }


        /// <summary>
        /// エディタ設定
        /// </summary>
        /// <param name="config"></param>
        private void OnReflectEditorInfo(XmlConfigModel config) {
            Config.AddXmlContentsItem("LibraryExplorer.setting:Editor.Path", this.m_EditorPath);
            Config.AddXmlContentsItem("LibraryExplorer.setting:Editor.Arguments", this.m_EditorArguments);
        }

        #endregion

        #region 比較ツール設定
        /// <summary>
        /// 比較ツール設定
        /// </summary>
        private void OnApplyDiffToolInfo() {
            this.m_DiffToolPath = this.m_ConfigHelper.GetStringValue("LibraryExplorer.setting:DiffTool.Path");
            this.m_DiffToolArguments = this.m_ConfigHelper.GetStringValue("LibraryExplorer.setting:DiffTool.Arguments");
        }


        /// <summary>
        /// 比較ツール設定
        /// </summary>
        /// <param name="config"></param>
        private void OnReflectDiffToolInfo(XmlConfigModel config) {
            Config.AddXmlContentsItem("LibraryExplorer.setting:DiffTool.Path", this.m_DiffToolPath);
            Config.AddXmlContentsItem("LibraryExplorer.setting:DiffTool.Arguments", this.m_DiffToolArguments);
        }

        #endregion

        #region スクリプト
        /// <summary>
        /// スクリプト
        /// </summary>
        private void OnApplyScriptInfo() {
            this.m_ExcelModuleExportScriptName = this.m_ConfigHelper.GetStringValue("LibraryExplorer.setting:Script.ExcelModuleExport");
            this.m_ExcelModuleImportScriptName = this.m_ConfigHelper.GetStringValue("LibraryExplorer.setting:Script.ExcelModuleImport");
        }


        /// <summary>
        /// スクリプト
        /// </summary>
        /// <param name="config"></param>
        private void OnReflectScriptInfo(XmlConfigModel config) {
            config.AddXmlContentsItem("LibraryExplorer.setting:Script.ExcelModuleExport", this.m_ExcelModuleExportScriptName);
            config.AddXmlContentsItem("LibraryExplorer.setting:Script.ExcelModuleImport", this.m_ExcelModuleExportScriptName);
        }

        #endregion

        #region ウインドウ領域
        /// <summary>
        /// ウインドウ領域
        /// </summary>
        private void OnApplyWindowBounds() {
            this.m_MainWindowWidth = this.m_ConfigHelper.GetIntValue("LibraryExplorer.setting:Window.MainWindow.Size.Width");
            this.m_MainWindowHeight = this.m_ConfigHelper.GetIntValue("LibraryExplorer.setting:Window.MainWindow.Size.Height");
            this.m_MainWindowLeft = this.m_ConfigHelper.GetIntValue("LibraryExplorer.setting:Window.MainWindow.Location.Left");
            this.m_MainWindowTop = this.m_ConfigHelper.GetIntValue("LibraryExplorer.setting:Window.MainWindow.Location.Top");
        }


        /// <summary>
        /// ウインドウ領域
        /// </summary>
        /// <param name="config"></param>
        private void OnReflectWindowBounds(XmlConfigModel config) {
            config.AddXmlContentsItem("LibraryExplorer.setting:Window.MainWindow.Size.Width", this.m_MainWindowWidth);
            config.AddXmlContentsItem("LibraryExplorer.setting:Window.MainWindow.Size.Height", this.m_MainWindowHeight);
            config.AddXmlContentsItem("LibraryExplorer.setting:Window.MainWindow.Location.Left", this.m_MainWindowLeft);
            config.AddXmlContentsItem("LibraryExplorer.setting:Window.MainWindow.Location.Top", this.m_MainWindowTop);
        }

        #endregion

        //既定のConfig
        private void OnCreateDefaultConfig(XmlConfigModel config) {
            //対象拡張子
            config.AddXmlContentsItem("LibraryExplorer.setting:Common.TargetExtentions.Count", 3);
            config.AddXmlContentsItem("LibraryExplorer.setting:Common.TargetExtentions.1", ".bas");
            config.AddXmlContentsItem("LibraryExplorer.setting:Common.TargetExtentions.2", ".frm");
            config.AddXmlContentsItem("LibraryExplorer.setting:Common.TargetExtentions.3", ".cls");
            config.AddXmlContentsItem("LibraryExplorer.setting:Common.HasOtherFilesExtentions.Count", 1);
            config.AddXmlContentsItem("LibraryExplorer.setting:Common.HasOtherFilesExtentions.1.Target", ".frm");
            config.AddXmlContentsItem("LibraryExplorer.setting:Common.HasOtherFilesExtentions.1.Count", 1);
            config.AddXmlContentsItem("LibraryExplorer.setting:Common.HasOtherFilesExtentions.1.1", ".frx");

            //Library関連
            config.AddXmlContentsItem("LibraryExplorer.setting:Library.Header.Start", "MY_LIBRARY_HEADER_START");
            config.AddXmlContentsItem("LibraryExplorer.setting:Library.Header.End", "MY_LIBRARY_HEADER_END");

            //エディタ設定
            config.AddXmlContentsItem("LibraryExplorer.setting:Editor.Path", "");
            config.AddXmlContentsItem("LibraryExplorer.setting:Editor.Arguments", "");

            //比較ツール設定
            config.AddXmlContentsItem("LibraryExplorer.setting:DiffTool.Path", "");
            config.AddXmlContentsItem("LibraryExplorer.setting:DiffTool.Arguments", "");

            //スクリプト
            config.AddXmlContentsItem("LibraryExplorer.setting:Script.ExcelModuleExport", "ExcelModuleExport.vbs");
            config.AddXmlContentsItem("LibraryExplorer.setting:Script.ExcelModuleImport", "ExcelModuleImport.vbs");

            //MainWindowの位置とサイズ
            config.AddXmlContentsItem("LibraryExplorer.setting:Window.MainWindow.Size.Width", 400);
            config.AddXmlContentsItem("LibraryExplorer.setting:Window.MainWindow.Size.Height", 300);
            config.AddXmlContentsItem("LibraryExplorer.setting:Window.MainWindow.Location.Left", 50);
            config.AddXmlContentsItem("LibraryExplorer.setting:Window.MainWindow.Location.Top", 50);

        }


        #endregion

        #endregion

    }

}
