using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryExplorer.Common.Request {

    #region RefreshDisplayRequest

    #region RefreshDisplayRequestData
    /// <summary>
    /// 表示の更新を要求するクラスです。
    /// </summary>
    public class RefreshDisplayRequestData : RequestData {

        #region フィールド(メンバ変数、プロパティ、イベント)


        #region KeepDisplay
        private bool m_KeepDisplay;
        /// <summary>
        /// KeepDisplayを取得、設定します。
        /// </summary>
        public bool KeepDisplay {
            get {
                return this.m_KeepDisplay;
            }
            set {
                this.m_KeepDisplay = value;
            }
        }
        #endregion



        #endregion

        #region コンストラクタ
        /// <summary>
        /// RefreshDisplayRequestDataオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public RefreshDisplayRequestData(bool keep) {
            this.m_KeepDisplay = keep;
        }
        #endregion

    }
    #endregion

    #region RefreshDisplayRequestEventArgs
    /// <summary>
    /// 表示の更新を要求するRequestEventArgsクラスの派生クラスです。
    /// </summary>
    public class RefreshDisplayRequestEventArgs : RequestEventArgs {

        #region フィールド(メンバ変数、プロパティ、イベント)

        #endregion

        #region コンストラクタ
        /// <summary>
        /// RefreshDisplayRequestEventArgsオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="keepDisplay"></param>
        public RefreshDisplayRequestEventArgs(bool keepDisplay) : this(keepDisplay,"") {
        }
        /// <summary>
        /// RefreshDisplayRequestEventArgsオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="keepDisplay"></param>
        /// <param name="ownerName"></param>
        public RefreshDisplayRequestEventArgs(bool keepDisplay,string ownerName) : base(new RefreshDisplayRequestData(keepDisplay), ownerName) {
        }

        #endregion

    }
    #endregion

    #endregion
}
