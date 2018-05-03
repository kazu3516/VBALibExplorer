using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryExplorer.Common;
using LibraryExplorer.Control;
using LibraryExplorer.Data;

namespace LibraryExplorer.Window.DockWindow {

    /// <summary>
    /// LibraryFileを一覧表示するウィンドウを表すクラスです。
    /// </summary>
    public partial class LibraryFileListWindow : ExplorerListWindow {


        #region フィールド(メンバ変数、プロパティ、イベント)




        #endregion

        #region コンストラクタ
        /// <summary>
        /// /// LibraryFileListWindowオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public LibraryFileListWindow() {
            InitializeComponent();
        }
        #endregion

        #region イベントハンドラ

        #endregion

        /// <summary>
        /// 指定したLibraryFolderをTargetFolderに設定します。
        /// </summary>
        /// <param name="folder"></param>
        public void SetTargetFolder(LibraryFolder folder) {
            this.TargetFolder = folder;
        }

    }
}
