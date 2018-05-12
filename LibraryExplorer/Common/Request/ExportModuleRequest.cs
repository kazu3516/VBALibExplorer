using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryExplorer.Data;

namespace LibraryExplorer.Common.Request {
    #region ExportModuleRequest

    #region ExportModuleRequestData
    /// <summary>
    /// モジュールのエクスポート要求を表すクラスです。
    /// </summary>
    public class ExportModuleRequestData : RequestData {

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
        /// ExportModuleRequestDataオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="targetFile"></param>
        public ExportModuleRequestData(OfficeFile targetFile) {
            this.m_TargetFile = targetFile;
        }
        #endregion

    }
    #endregion

    #region ExportModuleRequestEventArgs
    /// <summary>
    /// モジュールのエクスポート要求を行う時に使用するRequestEventArgsクラスの派生クラスです。
    /// </summary>
    public class ExportModuleRequestEventArgs : RequestEventArgs {

        #region フィールド(メンバ変数、プロパティ、イベント)

        #endregion

        #region コンストラクタ
        /// <summary>
        /// ExportModuleRequestEventArgsオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="targetFile"></param>
        public ExportModuleRequestEventArgs(OfficeFile targetFile) : this(targetFile, "") {
        }
        /// <summary>
        /// ExportModuleRequestEventArgsオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="targetFile"></param>
        /// <param name="ownerName"></param>
        public ExportModuleRequestEventArgs(OfficeFile targetFile, string ownerName) : base(new ExportModuleRequestData(targetFile), ownerName) {
        }

        #endregion


    }
    #endregion

    #endregion
}
