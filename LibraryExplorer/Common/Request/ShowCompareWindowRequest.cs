using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryExplorer.Window.DockWindow;
namespace LibraryExplorer.Common.Request {

    #region ShowCompareWindowRequest

    #region ShowCompareWindowRequestData
    /// <summary>
    /// FolderCompareWizardからMainWindowへのCompareWindowの表示要求を表すクラスのサンプルです。
    /// </summary>
    public class ShowCompareWindowRequestData : RequestData {

        #region フィールド(メンバ変数、プロパティ、イベント)

        #region SourceFolderPath
        private string m_SourceFolderPath;
        /// <summary>
        /// SourceFolderPathを取得します。
        /// </summary>
        public string SourceFolderPath {
            get {
                return this.m_SourceFolderPath;
            }
        }
        #endregion

        #region DestinationFolderPath
        private string m_DestinationFolderPath;
        /// <summary>
        /// DestinationFolderPathを取得します。
        /// </summary>
        public string DestinationFolderPath {
            get {
                return this.m_DestinationFolderPath;
            }
        }
        #endregion

        #endregion

        #region コンストラクタ
        /// <summary>
        /// SampleRequestDataオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public ShowCompareWindowRequestData(string sourceFolderPath,string destinationFolderPath) {
            this.m_SourceFolderPath = sourceFolderPath;
            this.m_DestinationFolderPath = destinationFolderPath;
        }
        #endregion

    }
    #endregion

    #region ShowCompareWindowRequestEventArgs
    /// <summary>
    /// FolderCompareWizardからMainWindowへのCompareWindowの表示要求を表すRequestEventArgsクラスの派生クラスです。。
    /// </summary>
    public class ShowCompareWindowRequestEventArgs : RequestEventArgs {

        #region フィールド(メンバ変数、プロパティ、イベント)

        #endregion

        #region コンストラクタ
        /// <summary>
        /// SampleRequestEventArgsオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="sourceFolderPath"></param>
        /// <param name="destinationFolderPath"></param>
        public ShowCompareWindowRequestEventArgs(string sourceFolderPath, string destinationFolderPath) : this(sourceFolderPath,destinationFolderPath, "") {
        }
        /// <summary>
        /// SampleRequestEventArgsオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="sourceFolderPath"></param>
        /// <param name="destinationFolderPath"></param>
        /// <param name="ownerName"></param>
        public ShowCompareWindowRequestEventArgs(string sourceFolderPath, string destinationFolderPath, string ownerName) : base(new ShowCompareWindowRequestData(sourceFolderPath,destinationFolderPath), ownerName) {
        }

        #endregion

    }
    #endregion

    #endregion

}
