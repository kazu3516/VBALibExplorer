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
        public abstract void Start();

        /// <summary>
        /// 派生クラスでオーバーライドされると、外部ツールを非同期で起動します。
        /// </summary>
        /// <returns></returns>
        public abstract Task StartAsync();

    }

}
