using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using LibraryExplorer.Data;

namespace LibraryExplorer.Common.Request {

    #region NotifyFileChangedRequest

    #region NotifyFileChangedRequestData
    /// <summary>
    /// ファイルが変更されたことを通知するクラスです。
    /// </summary>
    public class NotifyFileChangedRequestData : RequestData {

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

        #region FileSystemEventArgs
        private FileSystemEventArgs m_FileSyatemEventArgs;
        /// <summary>
        /// FileSystemEventArgsを取得、設定します。
        /// </summary>
        public FileSystemEventArgs FileSyatemEventArgs {
            get {
                return this.m_FileSyatemEventArgs;
            }
            set {
                this.m_FileSyatemEventArgs = value;
            }
        }
        #endregion

        #endregion

        #region コンストラクタ
        /// <summary>
        /// NotifyFileChangedRequestDataオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="targetFile"></param>
        /// <param name="args"></param>
        public NotifyFileChangedRequestData(OfficeFile targetFile, FileSystemEventArgs args) {
            this.m_TargetFile = targetFile;
            this.m_FileSyatemEventArgs = args;
        }
        #endregion

    }
    #endregion

    #region NotifyFileChangedRequestEventArgs
    /// <summary>
    /// ファイルが変更されたことを通知するクラスです。
    /// </summary>
    public class NotifyFileChangedRequestEventArgs : RequestEventArgs {

        #region フィールド(メンバ変数、プロパティ、イベント)

        #endregion

        #region コンストラクタ
        /// <summary>
        /// NotifyFileChangedRequestEventArgsオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="targetFile"></param>
        /// <param name="args"></param>
        public NotifyFileChangedRequestEventArgs(OfficeFile targetFile, FileSystemEventArgs args) : this(targetFile,args,"") {
        }
        /// <summary>
        /// NotifyFileChangedRequestEventArgsオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="targetFile"></param>
        /// <param name="args"></param>
        /// <param name="ownerName"></param>
        public NotifyFileChangedRequestEventArgs(OfficeFile targetFile, FileSystemEventArgs args,string ownerName) : base(new NotifyFileChangedRequestData(targetFile,args), ownerName) {
        }

        #endregion

    }
    #endregion

    #endregion

}
