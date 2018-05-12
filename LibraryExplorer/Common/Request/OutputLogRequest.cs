using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryExplorer.Common.Request {

    /// <summary>
    /// 出力へのメッセージ要求を発生するオブジェクトであることを表すインターフェースです。
    /// </summary>
    public interface IOutputLogRequest {

        /// <summary>
        /// 出力ウィンドウへのメッセージ出力要求を表すイベントです。
        /// </summary>
        event OutputLogRequestEventHandler OutputLogRequest;
    }

    #region OutputLogRequestData
    /// <summary>
    /// 出力ウィンドウへのメッセージ出力要求のデータを表すクラスです。
    /// </summary>
    public class OutputLogRequestData : RequestData {

        #region フィールド(メンバ変数、プロパティ、イベント)

        #region Message
        private string m_Message;
        /// <summary>
        /// Messageを取得します。
        /// </summary>
        public string Message {
            get {
                return this.m_Message;
            }
            set {
                this.m_Message = value;
            }
        }
        #endregion

        #endregion

        #region コンストラクタ
        /// <summary>
        /// OutputLogRequestDataオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="message"></param>
        public OutputLogRequestData(string message) {
            this.m_Message = message;
        }
        #endregion

        #region イベントハンドラ

        #endregion

    }
    #endregion

    #region OutputLogRequestEvent
    /// <summary>
    /// 出力ウィンドウへのメッセージ出力要求を表すイベントです。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void OutputLogRequestEventHandler(object sender, OutputLogRequestEventArgs e);

    #endregion

    #region OutputLogRequestEventArgs
    /// <summary>
    /// 出力ウィンドウへのメッセージ出力要求を表すイベントで使用する、RequestEventArgsクラスの派生クラスです。
    /// </summary>
    public class OutputLogRequestEventArgs : RequestEventArgs {

        #region フィールド(メンバ変数、プロパティ、イベント)

        #endregion

        #region コンストラクタ
        /// <summary>
        /// OutputLogRequestEventArgsオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="message"></param>
        public OutputLogRequestEventArgs(string message) :this(message,""){
        }
        /// <summary>
        /// OutputLogRequestEventArgsオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ownerName"></param>
        public OutputLogRequestEventArgs(string message, string ownerName) : base(new OutputLogRequestData(message),ownerName) {
        }

        #endregion

        #region イベントハンドラ

        #endregion

    }
    #endregion



}
