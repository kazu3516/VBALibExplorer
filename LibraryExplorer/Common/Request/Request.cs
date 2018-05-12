using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryExplorer.Data;

namespace LibraryExplorer.Common.Request {

    //NOTE:イベントを使用して他クラスへ要求を発行する場合、RequestData,RequestEventArgsを継承する。
    //複数のオブジェクトで使用する場合、インターフェースも定義する。
    //コピー元としてSampleRequestを作成。適宜コピーして作成する

    //NOTE:各WindowからLibraryへ要求を送信する場合、RequestEventを共通で使用する。
    //受信側はRequestDataの派生クラスの型で要求の種類を判定するため、追加データが必要ない場合もRequestDataクラスを継承したクラスを定義する。

    

    #region Requestの基本クラス

    #region RequestData
    /// <summary>
    /// イベントを通じて他のオブジェクトへの要求を発行します。
    /// </summary>
    public class RequestData {

        #region フィールド(メンバ変数、プロパティ、イベント)

        #endregion

        #region コンストラクタ
        /// <summary>
        /// RequestDataオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public RequestData() {
        }
        #endregion

        #region イベントハンドラ

        #endregion

    }
    #endregion

    /// <summary>
    /// イベントを通じて他のオブジェクトへの要求を発行するイベントで使用するデリゲートです。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void RequestEventHandler(object sender, RequestEventArgs e);

    #region RequestEventArgs
    /// <summary>
    /// イベントを通じて他のオブジェクトへの要求を発行するイベントで使用するEventArgsクラスの派生クラスです。
    /// </summary>
    public class RequestEventArgs : EventArgs {

        #region フィールド(メンバ変数、プロパティ、イベント)

        #region Request
        private RequestData m_RequestData;
        /// <summary>
        /// Requestを取得、設定します。
        /// </summary>
        public RequestData RequestData {
            get {
                return this.m_RequestData;
            }
            set {
                this.m_RequestData = value;
            }
        }
        #endregion

        #region OwnerName
        private string m_OwnerName;
        /// <summary>
        /// OwnerNameを取得、設定します。
        /// </summary>
        public string OwnerName {
            get {
                return this.m_OwnerName;
            }
            set {
                this.m_OwnerName = value;
            }
        }
        #endregion

        #endregion

        #region コンストラクタ
        /// <summary>
        /// RequestEventArgsオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="request"></param>
        public RequestEventArgs(RequestData request) : this(request, "") {
        }
        /// <summary>
        /// RequestEventArgsオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ownerName"></param>
        public RequestEventArgs(RequestData request, string ownerName) {
            this.m_RequestData = request;
            this.m_OwnerName = ownerName;
        }

        #endregion

        #region イベントハンドラ

        #endregion

    }
    #endregion

    #endregion


    namespace Sample {

        #region SampleRequest

        #region SampleRequestData
        /// <summary>
        /// 要求を表すクラスのサンプルです。
        /// </summary>
        public class SampleRequestData : RequestData {

            #region フィールド(メンバ変数、プロパティ、イベント)

            #endregion

            #region コンストラクタ
            /// <summary>
            /// SampleRequestDataオブジェクトの新しいインスタンスを初期化します。
            /// </summary>
            public SampleRequestData() {
            }
            #endregion

        }
        #endregion

        #region SampleRequestEventArgs
        /// <summary>
        /// RequestEventArgsクラスの派生クラスのサンプルです。
        /// </summary>
        public class SampleRequestEventArgs : RequestEventArgs {

            #region フィールド(メンバ変数、プロパティ、イベント)

            #endregion

            #region コンストラクタ
            /// <summary>
            /// SampleRequestEventArgsオブジェクトの新しいインスタンスを初期化します。
            /// </summary>
            public SampleRequestEventArgs() : this("") {
            }
            /// <summary>
            /// SampleRequestEventArgsオブジェクトの新しいインスタンスを初期化します。
            /// </summary>
            /// <param name="ownerName"></param>
            public SampleRequestEventArgs(string ownerName) : base(new SampleRequestData(), ownerName) {
            }

            #endregion

        }
        #endregion

        #endregion

    }


}
