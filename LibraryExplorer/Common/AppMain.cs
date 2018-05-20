using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kucl.App;
using Kucl.Xml;
using Kucl.Xml.XmlCfg;

using log4net;
using LibraryExplorer.Window;

namespace LibraryExplorer.Common {

    /// <summary>
    /// アプリケーションのエントリポイントを提供するクラスです。
    /// </summary>
    public class AppMain:AppMainBase,IUseConfig {

        #region Main
        /// <summary>
        /// AppMainクラスの既定のインスタンスです。
        /// </summary>
        public static AppMain g_AppMain;
        /// <summary>
        /// アプリケーションで作成するLoggerオブジェクトです。
        /// </summary>
        public static readonly ILog logger = LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AppMain.g_AppMain = new AppMain();
            AppMain.g_AppMain.DoWriteLogOnStart = true;
            AppMain.g_AppMain.UseConfig = true;
            AppMain.g_AppMain.UseExDLL = false;
            AppMain.g_AppMain.Start();

        }

        #endregion

        #region フィールド
        private UseConfigHelper m_ConfigHelper;

        #endregion

        #region プロパティ


        #region TemporaryFolderPath
        private string m_TemporaryFolderPath;
        /// <summary>
        /// TemporaryFolderPathを取得、設定します。
        /// </summary>
        public string TemporaryFolderPath {
            get {
                return this.m_TemporaryFolderPath;
            }
            set {
                this.m_TemporaryFolderPath = value;
            }
        }
        #endregion
        
        #region ScriptFolderPath
        private string m_ScriptFolderPath;
        /// <summary>
        /// ScriptFolderPathを取得、設定します。
        /// </summary>
        public string ScriptFolderPath {
            get {
                return this.m_ScriptFolderPath;
            }
            set {
                this.m_ScriptFolderPath = value;
            }
        }
        #endregion

        #region WorkspaceFolderPath
        private string m_WorkspaceFolderPath;
        /// <summary>
        /// WorkspaceFolderPathを取得、設定します。
        /// </summary>
        public string WorkspaceFolderPath {
            get {
                return this.m_WorkspaceFolderPath;
            }
            set {
                this.m_WorkspaceFolderPath = value;
            }
        }
        #endregion

        #region HistoryFolderPath
        private string m_HistoryFolderPath;
        /// <summary>
        /// HistoryFolderPathを取得、設定します。
        /// </summary>
        public string HistoryFolderPath {
            get {
                return this.m_HistoryFolderPath;
            }
            set {
                this.m_HistoryFolderPath = value;
            }
        }
        #endregion




        #region AppInfo
        private AppInfo m_AppInfo;
        /// <summary>
        /// AppInfoを取得します。
        /// </summary>
        public AppInfo AppInfo {
            get {
                return this.m_AppInfo;
            }
        }
        #endregion

        #endregion

        #region OnStart
        /// <summary>
        /// アプリケーションの開始時の動作をオーバーライドします。
        /// </summary>
        protected override void OnStart() {
            AppMain.logger.Info("LibraryExplorer:Start");

            //フォルダ構造の初期化
            int logFolderIndex = this.AddUserPath("log");   //フルパス不要だが、念のためindexを保存
            int tempFolderIndex = this.AddUserPath("temp");
            int scriptFolderIndex = this.AddUserPath("script");
            int workspaceFolderIndex = this.AddUserPath("Workspace");
            int historyFolderIndex = this.AddUserPath("History");
            //int dataFolderIndex = this.AddUserPath("Data");

            base.OnStart();

            //初期化したフォルダのフルパスを取得
            this.m_TemporaryFolderPath = this.UserPathCollection[tempFolderIndex];
            this.m_ScriptFolderPath = this.UserPathCollection[scriptFolderIndex];
            this.m_WorkspaceFolderPath = this.UserPathCollection[workspaceFolderIndex];
            this.m_HistoryFolderPath = this.UserPathCollection[historyFolderIndex];
            //this.m_DataDirectory = this.UserPathCollection[dataFolderIndex];

            //***************************
            //初期化

            //AppInfo
            this.m_AppInfo = new AppInfo();
            this.RegisterUseConfigObject(this.m_AppInfo);


            //Window
            MainWindow form = new MainWindow();
            this.MainWindow = form;

            //config
            this.m_ConfigHelper = new UseConfigHelper(this.CreateDefaultConfig());

            this.RegisterUseConfigObject(this);
            this.RegisterUseConfigObject(form.Project);
        }
        #endregion

        #region OnRun
        /// <summary>
        /// アプリケーションの実行時の動作をオーバーライドします。
        /// </summary>
        protected override void Run() {
            //Configを使用した動作はここから。

            //
            string[] args = Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length; i++) {
                AppMain.logger.Info($"Args({i}:{args[i]}");
            }
            //ファイルを開く / 新規ドキュメント
            //MainWindow form = (MainWindow)this.MainWindow;
            //bool openFile = args.Length > 1;
            //openFile = true;
            //if (openFile) {
            //    //HACK:コマンドライン引数の扱いを見直し(現状はコマンドラインスイッチは無い)
            //    string filename = args[1];
            //    //string filename = string.Format("{0}\\test1.xml", this.m_DataDirectory);

            //    form.OpenDocument(filename);
            //}
            //else {
            //    //CreateNewDocument内でConfigを使用するため、Runメソッド内で呼び出す。（OnStart内はConfig読み込み前のため不可）
            //    form.CreateNewDocument();
            //}


            base.Run();
        }

        #endregion

        #region OnEnd
        /// <summary>
        /// アプリケーション終了時の動作をオーバーライドします。
        /// </summary>
        protected override void OnEnd() {
            base.OnEnd();

            AppMain.logger.Info("LibraryExplorer:End\r\n");
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
            //this.m_DataDirectory = this.m_ConfigHelper.GetStringValue("setting.AccountBook:Data.Directory");
        }
        //Configの更新
        private void OnReflectConfig(XmlConfigModel config) {
            //config.AddXmlContentsItem("setting.AccountBook:Data.Directory",this.m_DataDirectory);
        }
        //既定のConfig
        private void OnCreateDefaultConfig(XmlConfigModel config) {
            //config.AddXmlContentsItem("setting.AccountBook:Data.Directory","Data");
        }


        #endregion

        #endregion


    }
}
