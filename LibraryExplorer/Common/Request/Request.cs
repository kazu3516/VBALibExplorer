using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryExplorer.Data;

namespace LibraryExplorer.Common.Request {

    //NOTE:イベントを使用して他クラスへ要求を発行する場合、RequestData,RequestEventArgsを継承する。
    //複数のオブジェクトで使用する場合、インターフェースも定義する。

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

        #region イベントハンドラ

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

        #region イベントハンドラ

        #endregion

    }
    #endregion

    #endregion

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

        #region イベントハンドラ

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

        #region イベントハンドラ

        #endregion

    }
    #endregion

    #endregion

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

        #region イベントハンドラ

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

        #region イベントハンドラ

        #endregion

    }
    #endregion 

    #endregion



}
