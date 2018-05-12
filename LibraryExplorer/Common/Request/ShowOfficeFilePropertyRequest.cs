using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryExplorer.Data;

namespace LibraryExplorer.Common.Request {
    
    #region ShowOfficeFilePropertyRequest

    #region ShowOfficeFilePropertyRequestData
    /// <summary>
    /// OfficeFileのプロパティ表示を要求するクラスです。
    /// </summary>
    public class ShowOfficeFilePropertyRequestData : RequestData {

        #region フィールド(メンバ変数、プロパティ、イベント)

        #region TargetFile
        private OfficeFile m_TargetFile;
        /// <summary>
        /// TargetFileを取得、設定します。
        /// </summary>
        public OfficeFile TargetFile {
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
        /// ShowOfficeFilePropertyRequestDataオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="targetFile"></param>
        public ShowOfficeFilePropertyRequestData(OfficeFile targetFile) {
            this.m_TargetFile = targetFile;
        }
        #endregion

    }
    #endregion

    #region ShowOfficeFilePropertyRequestEventArgs
    /// <summary>
    /// OfficeFileのプロパティ表示を要求する時に使用するRequestEventArgsクラスの派生クラスです。
    /// </summary>
    public class ShowOfficeFilePropertyRequestEventArgs : RequestEventArgs {

        #region フィールド(メンバ変数、プロパティ、イベント)

        #endregion

        #region コンストラクタ
        /// <summary>
        /// ShowOfficeFilePropertyRequestEventArgsオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="targetFile"></param>
        public ShowOfficeFilePropertyRequestEventArgs(OfficeFile targetFile) : this(targetFile, "") {
        }
        /// <summary>
        /// ShowOfficeFilePropertyRequestEventArgsオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="targetFile"></param>
        /// <param name="ownerName"></param>
        public ShowOfficeFilePropertyRequestEventArgs(OfficeFile targetFile, string ownerName) : base(new ShowOfficeFilePropertyRequestData(targetFile), ownerName) {
        }

        #endregion

    }
    #endregion

    #endregion
}
