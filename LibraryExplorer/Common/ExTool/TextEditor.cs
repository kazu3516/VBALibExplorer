using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryExplorer.Data;

namespace LibraryExplorer.Common.ExTool {

    /// <summary>
    /// 外部のエディタを表します。
    /// </summary>
    public class TextEditor:ExternalTool {


        #region フィールド(メンバ変数、プロパティ、イベント)

        #region TargetFile
        private LibraryFile m_TargetFile;
        /// <summary>
        /// TargetFileを取得、設定します。
        /// </summary>
        public LibraryFile TargetFile {
            get {
                return this.m_TargetFile;
            }
            set {
                this.m_TargetFile = value;
            }
        }
        #endregion

        #endregion

        #region コンストラクタ
        /// <summary>
        /// TextEditorオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public TextEditor() {
        }
        #endregion

        #region イベントハンドラ

        #endregion

        
        /// <summary>
        /// エディタを起動します。
        /// </summary>
        public override ExternalToolResult Start(ExternalToolInfo info) {
            if (!(info is TextEditorInfo editorInfo)) {
                return ExternalToolResult.Failed;
            }
            this.OpenFile(editorInfo.TargetFile);
            return ExternalToolResult.Success;
        }

        /// <summary>
        /// エディタを非同期に実行します。
        /// </summary>
        /// <returns></returns>
        public override Task<ExternalToolResult> StartAsync(ExternalToolInfo info) {
            return Task.Factory.StartNew<ExternalToolResult>(()=> {
                return this.Start(info);
            });
        }


        #region OpenFile
        /// <summary>
        /// ファイルを開きます。
        /// </summary>
        /// <param name="file"></param>
        public void OpenFile(LibraryFile file) {
            //パスの設定
            string editorPath = this.GetEditorPath();
            //引数の設定
            string editorArguments = this.GetEditorArguments(file);
            //プロセスの起動
            this.StartProcess(editorPath, editorArguments);
        }

        /// <summary>
        /// エディタのパスと引数を起動してプロセスを起動します。
        /// </summary>
        /// <param name="editorPath"></param>
        /// <param name="editorArguments"></param>
        private void StartProcess(string editorPath, string editorArguments) {
            ProcessStartInfo info = new ProcessStartInfo() { FileName = editorPath, Arguments = editorArguments };
            Process process = new Process() { StartInfo = info };
            try {
                process.Start();
            }
            catch (Exception ex) {
                string errorMessage = $"{this.GetType().Name}.OpenFile プロセスの起動に失敗しました。Exception={ex.GetType().Name}, Message={ex.Message}, Path={editorPath}, Arguments={editorArguments}";
                AppMain.logger.Error(errorMessage, ex);
                throw new ApplicationException(errorMessage, ex);
            }
        }

        /// <summary>
        /// エディタのパスを取得します。
        /// エディタが指定されていない場合、Windows標準のメモ帳を使用します。
        /// </summary>
        /// <returns></returns>
        private string GetEditorPath() {
            string editorPath = AppMain.g_AppMain.AppInfo.EditorPath;
            if (!File.Exists(editorPath)) {
                editorPath = "NotePad";
            }
            return editorPath;
        }

        /// <summary>
        /// エディタの引数を指定します。
        /// 引数内の%filename%は指定したLibraryFileのFileNameに置き換えられます。
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private string GetEditorArguments(LibraryFile file) {
            string editorArguments = AppMain.g_AppMain.AppInfo.EditorArguments;
            if (!editorArguments.Contains("%filename%")) {
                //引数に%filename%が含まれていない場合、末尾に追加する
                if (editorArguments.Length > 0) {
                    editorArguments += " ";
                }
                editorArguments += "%filename%";
            }
            editorArguments = editorArguments.Replace("%filename%", "\"" + file.FileName + "\"");
            return editorArguments;
        }


        #endregion

    }

    #region TextEditorInfo
    /// <summary>
    /// 外部のエディタを起動する時の付加情報を表すクラスです。
    /// </summary>
    public class TextEditorInfo :ExternalToolInfo{

        #region フィールド(メンバ変数、プロパティ、イベント)

        #region TargetFile
        private LibraryFile m_TargetFile;
        /// <summary>
        /// TargetFileを取得、設定します。
        /// </summary>
        public LibraryFile TargetFile {
            get {
                return this.m_TargetFile;
            }
            set {
                this.m_TargetFile = value;
            }
        }
        #endregion


        #endregion

        #region コンストラクタ
        /// <summary>
        /// TextEditorInfoオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="targetFile"></param>
        public TextEditorInfo(OfficeFile targetFile) {
        }
        #endregion

    }
    #endregion

    #region TextEditorResult
    /// <summary>
    /// 外部のエディタを起動した時の結果を表すクラスです。
    /// </summary>
    public class TextEditorResult:ExternalToolResult {

        #region フィールド(メンバ変数、プロパティ、イベント)

        #endregion

        #region コンストラクタ
        /// <summary>
        /// TextEditorResultオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public TextEditorResult() {
        }
        #endregion

        
    }
    #endregion

}
