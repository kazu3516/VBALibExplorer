using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryExplorer.Window.Dialog {

    /// <summary>
    /// オプションダイアログを表すクラスです。
    /// </summary>
    public partial class OptionDialogBase : Form {


        #region フィールド(メンバ変数、プロパティ、イベント)

        #endregion

        #region コンストラクタ
        /// <summary>
        /// OptionDialogBaseオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public OptionDialogBase() {
            InitializeComponent();
        }
        #endregion

        #region イベントハンドラ

        #endregion

        /// <summary>
        /// コントロールのプロパティとオブジェクトのプロパティの間のデータバインドを設定します。
        /// </summary>
        /// <param name="control"></param>
        /// <param name="propertyName"></param>
        /// <param name="bindObject"></param>
        /// <param name="bindPropertyName"></param>
        protected void AddSingleDataBind(System.Windows.Forms.Control control,string propertyName,object bindObject,string bindPropertyName) {
            control.DataBindings.Add(propertyName, bindObject, bindPropertyName, true, DataSourceUpdateMode.OnPropertyChanged);
        }
    }
}
