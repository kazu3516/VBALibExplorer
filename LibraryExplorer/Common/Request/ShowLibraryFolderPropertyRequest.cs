using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryExplorer.Data;

namespace LibraryExplorer.Common.Request {

    #region ShowLibraryFolderPropertyRequest

    #region ShowLibraryFolderPropertyRequestData
    /// <summary>
    /// LibraryFolderのプロパティ表示を要求するクラスです。
    /// </summary>
    public class ShowLibraryFolderPropertyRequestData : RequestData {

        #region フィールド(メンバ変数、プロパティ、イベント)

        #region TargetFolder
        private LibraryFolder m_TargetFolder;
        /// <summary>
        /// TargetFileを取得、設定します。
        /// </summary>
        public LibraryFolder TargetFolder {
            get {
                return this.m_TargetFolder;
            }
            set {
                this.m_TargetFolder = value;
            }
        }
        #endregion

        #endregion

        #region コンストラクタ
        /// <summary>
        /// ShowLibraryFolderPropertyRequestDataオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="targetFolder"></param>
        public ShowLibraryFolderPropertyRequestData(LibraryFolder targetFolder) {
            this.m_TargetFolder = targetFolder;
        }
        #endregion

    }
    #endregion

    #region ShowLibraryFolderPropertyRequestEventArgs
    /// <summary>
    /// LibraryFolderのプロパティ表示を要求する時に使用するRequestEventArgsクラスの派生クラスです。
    /// </summary>
    public class ShowLibraryFolderPropertyRequestEventArgs : RequestEventArgs {

        #region フィールド(メンバ変数、プロパティ、イベント)

        #endregion

        #region コンストラクタ
        /// <summary>
        /// ShowLibraryFolderPropertyRequestEventArgsオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="targetFolder"></param>
        public ShowLibraryFolderPropertyRequestEventArgs(LibraryFolder targetFolder) : this(targetFolder, "") {
        }
        /// <summary>
        /// ShowLibraryFolderPropertyRequestEventArgsオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="targetFolder"></param>
        /// <param name="ownerName"></param>
        public ShowLibraryFolderPropertyRequestEventArgs(LibraryFolder targetFolder, string ownerName) : base(new ShowLibraryFolderPropertyRequestData(targetFolder), ownerName) {
        }

        #endregion

    }
    #endregion

    #endregion

}
