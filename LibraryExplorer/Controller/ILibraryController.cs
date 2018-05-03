using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryExplorer.Common;
using LibraryExplorer.Common.Request;
using LibraryExplorer.Data;

namespace LibraryExplorer.Controller {

    /// <summary>
    /// ExplorerListとオブジェクト間のインターフェースとなるコントローラクラスが実装するメンバを定義するインターフェースです。
    /// </summary>
    public interface ILibraryController:IOutputLogRequest {

        /// <summary>
        /// TargetFolderを取得します。
        /// </summary>
        LibraryFolder TargetFolder {
            get;set;
        }
        /// <summary>
        /// Controllerの更新に非同期動作が必要になるかどうかを返します。
        /// </summary>
        bool IsRequiredAsyncRefresh {get;}
        /// <summary>
        /// Controllerの更新が完了しているかどうかを返します。
        /// </summary>
        bool RefreshComplete { get; }

        /// <summary>
        /// Controllerの更新前に発生するイベントです。
        /// </summary>
        event EventHandler BeforeRefresh;
        /// <summary>
        /// Controllerの更新後に発生するイベントです。
        /// </summary>
        event EventHandler AfterRefresh;
        /// <summary>
        /// TargetFolderが変更されたときに発生するイベントです。
        /// </summary>
        event EventHandler<EventArgs<LibraryFolder>> TargetFolderChanged;
        /// <summary>
        /// Controllerから更新要求を行います。
        /// </summary>
        event EventHandler RequestRefresh;

        /// <summary>
        /// アイテムのクリアが必要かどうかを返します。
        /// </summary>
        /// <param name="keep"></param>
        /// <returns></returns>
        bool IsRequiredClearItem(bool keep);
        /// <summary>
        /// Controllerを更新します。
        /// </summary>
        /// <param name="keep"></param>
        void Refresh(bool keep);
        /// <summary>
        /// Controllerを非同期に更新します。
        /// </summary>
        /// <param name="keep"></param>
        /// <returns></returns>
        Task RefreshAsync(bool keep);

        /// <summary>
        /// LibraryFileを開きます。
        /// </summary>
        /// <param name="file"></param>
        void OpenFile(LibraryFile file);

    }

    /// <summary>
    /// 変更前の値を保持するEventArgsクラスの派生クラスです。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EventArgs<T> : EventArgs {

        #region OldValue
        private T m_OldValue;
        /// <summary>
        /// OldValueを取得します。
        /// </summary>
        public T OldValue {
            get {
                return this.m_OldValue;
            }
        }
        #endregion

        /// <summary>
        /// EventArgsクラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="value"></param>
        public EventArgs(T value) {
            this.m_OldValue = value;
        }
    }
}
