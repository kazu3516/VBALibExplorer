using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryExplorer.Common.Request {

    #region CloseItemRequest

    #region CloseItemRequestData
    /// <summary>
    /// アイテムを閉じる要求を表すクラスです。
    /// </summary>
    public class CloseItemRequestData : RequestData {

        #region フィールド(メンバ変数、プロパティ、イベント)

        #endregion

        #region コンストラクタ
        /// <summary>
        /// CloseItemRequestDataオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public CloseItemRequestData() {
        }
        #endregion

    }
    #endregion

    #region CloseItemRequestEventArgs
    /// <summary>
    /// アイテムを閉じる要求を表すRequestEventArgsクラスの派生クラスです。
    /// </summary>
    public class CloseItemRequestEventArgs : RequestEventArgs {

        #region フィールド(メンバ変数、プロパティ、イベント)

        #endregion

        #region コンストラクタ
        /// <summary>
        /// CloseItemRequestEventArgsオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public CloseItemRequestEventArgs() : this("") {
        }
        /// <summary>
        /// CloseItemRequestEventArgsオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="ownerName"></param>
        public CloseItemRequestEventArgs(string ownerName) : base(new CloseItemRequestData(), ownerName) {
        }

        #endregion

    }
    #endregion

    #endregion
}
