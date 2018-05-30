using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryExplorer.Window.DockWindow {

    /// <summary>
    /// [最新の情報に更新]が使用できるオブジェクトであることを表すインターフェースです。
    /// </summary>
    public interface IRefreshDisplay {
        /// <summary>
        /// 表示を更新します。
        /// </summary>
        /// <param name="keep"></param>
        void RefreshDisplay(bool keep = false);
    }

}
