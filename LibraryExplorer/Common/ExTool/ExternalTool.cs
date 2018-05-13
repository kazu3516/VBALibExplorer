using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryExplorer.Common.ExTool {

    /// <summary>
    /// 外部ツールを表すクラスの基本クラスです。
    /// </summary>
    public abstract class ExternalTool {

        /// <summary>
        /// 派生クラスでオーバーライドされると、外部ツールを起動します。
        /// </summary>
        /// <param name="info"></param>
        public abstract ExternalToolResult Start(ExternalToolInfo info);

        /// <summary>
        /// 派生クラスでオーバーライドされると、外部ツールを非同期で起動します。
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public abstract Task<ExternalToolResult> StartAsync(ExternalToolInfo info);

    }

    #region ExternalToolInfo
    /// <summary>
    /// 外部ツールを実行する時の付加情報を表すクラスです。
    /// </summary>
    public class ExternalToolInfo {

        #region フィールド(メンバ変数、プロパティ、イベント)

        #endregion

        #region コンストラクタ
        /// <summary>
        /// ExternalToolInfoオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public ExternalToolInfo() {
        }
        #endregion

    }
    #endregion


    #region ExternalToolResult
    /// <summary>
    /// 外部ツールの結果を表すクラスです。
    /// </summary>
    public class ExternalToolResult {

        #region static

        #region static コンストラクタ
        static ExternalToolResult() {
            ExternalToolResult.s_Success = new ExternalToolResult() { m_ResultCode = ExternalToolResultCode.Success };
            ExternalToolResult.s_Failed = new ExternalToolResult() { m_ResultCode = ExternalToolResultCode.Failed };
        }
        #endregion

        #region Success
        private static ExternalToolResult s_Success;
        /// <summary>
        /// 外部ツールの実行が成功したことを表すExternalToolResultクラスのインスタンスを返します。
        /// このインスタンスは追加情報が必要ない場合に使用されます。
        /// </summary>
        public static ExternalToolResult Success {
            get {
                return ExternalToolResult.s_Success;
            }
        }
        #endregion

        #region Failed
        private static ExternalToolResult s_Failed;
        /// <summary>
        /// 外部ツールの実行が失敗したことを表すExternalToolResultクラスのインスタンスを返します。
        /// このインスタンスは追加情報が必要ない場合に使用されます。
        /// </summary>
        public static ExternalToolResult Failed {
            get {
                return ExternalToolResult.s_Failed;
            }
        }
        #endregion

        #endregion

        #region フィールド(メンバ変数、プロパティ、イベント)


        #region ResultCode
        private ExternalToolResultCode m_ResultCode;
        /// <summary>
        /// ResultCodeを取得、設定します。
        /// </summary>
        public ExternalToolResultCode ResultCode {
            get {
                return this.m_ResultCode;
            }
            set {
                this.m_ResultCode = value;
            }
        }
        #endregion


        #endregion

        #region コンストラクタ
        /// <summary>
        /// ExternalToolResultオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public ExternalToolResult() {
        }
        #endregion

    }
    #endregion

    /// <summary>
    /// 外部ツールの結果の分類を表す列挙型です。
    /// </summary>
    public enum ExternalToolResultCode {
        /// <summary>
        /// 外部ツールが正常に終了したことを表します。
        /// </summary>
        Success,
        /// <summary>
        /// 外部ツールの実行に失敗したことを表します。
        /// </summary>
        Failed,

    }

}
