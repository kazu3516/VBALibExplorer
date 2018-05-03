using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryExplorer.Control {

    //TODO:フォルダ比較ウィザードの実装

    /// <summary>
    /// フォルダ比較ウィザードを表すクラスです。
    /// </summary>
    public partial class FolderCompareWizard : EditableWizardControl {


        #region フィールド(メンバ変数、プロパティ、イベント)

        #endregion

        #region コンストラクタ
        /// <summary>
        /// FolderCompareWizardオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public FolderCompareWizard() {
            InitializeComponent();
        }
        #endregion

        #region イベントハンドラ

        #endregion

        #region ウィザードの動作

        /// <summary>
        /// GoNext時の動作をオーバーライドします。
        /// </summary>
        protected override void OnGoNext() {
            base.OnGoNext();

            //TODO:以下、ウィザードのページNo毎に処理を記載する。
            switch (this.CurrentStepNo) {
                case 0:
                    //開始ページは説明のみのため動作なし
                    break;
            }
        }
        /// <summary>
        /// Finish時の動作をオーバーライドします。
        /// </summary>
        protected override void OnFinish() {
            base.OnFinish();

        }

        #endregion
    }
}
