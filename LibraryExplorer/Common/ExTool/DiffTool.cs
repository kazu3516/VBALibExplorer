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
    /// 外部の差分比較ツールを表します。
    /// </summary>
    public class DiffTool :ExternalTool{


        #region フィールド(メンバ変数、プロパティ、イベント)

        #endregion

        #region コンストラクタ
        /// <summary>
        /// DiffToolオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public DiffTool() {
        }
        #endregion

        #region イベントハンドラ

        #endregion



        /// <summary>
        /// 比較ツールを実行します。
        /// </summary>
        public override ExternalToolResult Start(ExternalToolInfo info) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 比較ツールを非同期に実行します。
        /// </summary>
        /// <returns></returns>
        public override Task<ExternalToolResult> StartAsync(ExternalToolInfo info) {
            throw new NotImplementedException();
        }


        #region CheckDiff

        /// <summary>
        /// 比較ツールのパスと引数を起動してプロセスを起動します。
        /// </summary>
        /// <param name="diffToolPath"></param>
        /// <param name="diffToolArguments"></param>
        private void StartProcess(string diffToolPath, string diffToolArguments) {
            ProcessStartInfo info = new ProcessStartInfo() { FileName = diffToolPath, Arguments = diffToolArguments };
            Process process = new Process() { StartInfo = info };
            try {
                process.Start();
            }
            catch (Exception ex) {
                string errorMessage = $"{this.GetType().Name}.CheckDiff プロセスの起動に失敗しました。Exception={ex.GetType().Name}, Message={ex.Message}, Path={diffToolPath}, Arguments={diffToolArguments}";
                AppMain.logger.Error(errorMessage, ex);
                throw new ApplicationException(errorMessage, ex);
            }
        }

        /// <summary>
        /// 比較ツールのパスを取得します。
        /// 比較ツールが指定されていない場合、空文字列を返し、標準機能を使用します。
        /// </summary>
        /// <returns></returns>
        private string GetDiffToolPath() {
            string editorPath = AppMain.g_AppMain.AppInfo.DiffToolPath;
            if (!File.Exists(editorPath)) {
                editorPath = "";
            }
            return editorPath;
        }

        /// <summary>
        /// 比較ツールの引数を指定します。
        /// 引数内の%foldername1%,%foldername2%は指定したLibraryFolderのPathに置き換えられます。
        /// </summary>
        /// <param name="folder1"></param>
        /// <param name="folder2"></param>
        /// <returns></returns>
        private string GetDiffToolArguments(LibraryFolder folder1, LibraryFolder folder2) {
            string DiffToolArguments = AppMain.g_AppMain.AppInfo.DiffToolArguments;

            if (!DiffToolArguments.Contains("%foldername1%")) {
                //引数に%foldername1%が含まれていない場合、末尾に追加する
                if (DiffToolArguments.Length > 0) {
                    DiffToolArguments += " ";
                }
                DiffToolArguments += "%foldername1%";
            }
            if (!DiffToolArguments.Contains("%foldername2%")) {
                //引数に%foldername2%が含まれていない場合、末尾に追加する
                if (DiffToolArguments.Length > 0) {
                    DiffToolArguments += " ";
                }
                DiffToolArguments += "%foldername2%";
            }

            DiffToolArguments = DiffToolArguments.Replace("%foldername1%", "\"" + folder1.Path + "\"");
            DiffToolArguments = DiffToolArguments.Replace("%foldername2%", "\"" + folder2.Path + "\"");

            return DiffToolArguments;
        }


        #endregion

    }
}
