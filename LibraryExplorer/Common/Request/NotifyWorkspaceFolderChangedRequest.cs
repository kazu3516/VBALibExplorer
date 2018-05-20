using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using LibraryExplorer.Data;

namespace LibraryExplorer.Common.Request {
    
    #region NotifyWorkspaceFolderChangedRequest

    #region NotifyWorkspaceFolderChangedRequestData
    /// <summary>
    /// WorkSpaceFolderが変更されたことを通知するクラスです。
    /// </summary>
    public class NotifyWorkspaceFolderChangedRequestData : RequestData {

        #region フィールド(メンバ変数、プロパティ、イベント)

        #region WorkspaceFolder
        private LibraryFolder m_WorkspaceFolder;
        /// <summary>
        /// WorkspaceFolderを取得、設定します。
        /// </summary>
        public LibraryFolder WorkspaceFolder {
            get {
                return this.m_WorkspaceFolder;
            }
            set {
                this.m_WorkspaceFolder = value;
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
        /// NotifyWorkspaceFolderChangedRequestDataオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="workspaceFolder"></param>
        /// <param name="args"></param>
        public NotifyWorkspaceFolderChangedRequestData(LibraryFolder workspaceFolder,FileSystemEventArgs args) {
            this.m_WorkspaceFolder = workspaceFolder;
            this.m_FileSyatemEventArgs = args;
        }
        #endregion

    }
    #endregion

    #region NotifyWorkspaceFolderChangedRequestEventArgs
    /// <summary>
    /// WorkSpaceFolderが変更されたことを通知するクラスです。
    /// </summary>
    public class NotifyWorkspaceFolderChangedRequestEventArgs : RequestEventArgs {

        #region フィールド(メンバ変数、プロパティ、イベント)

        #endregion

        #region コンストラクタ
        /// <summary>
        /// NotifyWorkspaceFolderChangedRequestEventArgsオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="workspaceFolder"></param>
        /// <param name="args"></param>
        public NotifyWorkspaceFolderChangedRequestEventArgs(LibraryFolder workspaceFolder, FileSystemEventArgs args) : this(workspaceFolder,args,"") {
        }
        /// <summary>
        /// NotifyWorkspaceFolderChangedRequestEventArgsオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="workspaceFolder"></param>
        /// <param name="args"></param>
        /// <param name="ownerName"></param>
        public NotifyWorkspaceFolderChangedRequestEventArgs(LibraryFolder workspaceFolder, FileSystemEventArgs args, string ownerName) : base(new NotifyWorkspaceFolderChangedRequestData(workspaceFolder,args), ownerName) {
        }

        #endregion

    }
    #endregion

    #endregion

}
