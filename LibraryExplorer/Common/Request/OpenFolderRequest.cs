using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryExplorer.Common.Request {


    #region OpenFolderRequest

    #region OpenFolderRequestData
    /// <summary>
    /// フォルダを開く要求を表すクラスです。
    /// </summary>
    public class OpenFolderRequestData : RequestData {

        #region フィールド(メンバ変数、プロパティ、イベント)


        #region Folders
        private IList<string> m_Folders;
        /// <summary>
        /// Foldersを取得します。
        /// </summary>
        public IList<string> Folders {
            get {
                return this.m_Folders;
            }
        }
        #endregion

        #endregion

        #region コンストラクタ
        /// <summary>
        /// OpenFolderRequestDataオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="folders"></param>
        public OpenFolderRequestData(IList<string> folders) {
            this.m_Folders = folders;
        }
        #endregion

    }
    #endregion

    #region OpenFolderRequestEventArgs
    /// <summary>
    /// フォルダを開く要求を表す、RequestEventArgsクラスの派生クラスです。
    /// </summary>
    public class OpenFolderRequestEventArgs : RequestEventArgs {

        #region フィールド(メンバ変数、プロパティ、イベント)

        #endregion

        #region コンストラクタ
        /// <summary>
        /// OpenFolderRequestEventArgsオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="folders"></param>
        public OpenFolderRequestEventArgs(IList<string> folders) : this(folders,"") {
        }
        /// <summary>
        /// OpenFolderRequestEventArgsオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="folders"></param>
        /// <param name="ownerName"></param>
        public OpenFolderRequestEventArgs(IList<string> folders,string ownerName) : base(new OpenFolderRequestData(folders), ownerName) {
        }

        #endregion

    }
    #endregion

    #endregion

}
