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
using WeifenLuo.WinFormsUI.Docking;

namespace LibraryExplorer.Window.DockWindow {

    /// <summary>
    /// 出力を表示するウインドウです。
    /// </summary>
    public partial class OutputLogWindow : DockContent {

        private ApplicationMessageQueue m_MessageQueue;

        #region フィールド(メンバ変数、プロパティ、イベント)

        #endregion

        #region コンストラクタ
        /// <summary>
        /// OutputLogWindowオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public OutputLogWindow() {
            InitializeComponent();

            this.Initialize();
        }
        private void Initialize() {
            this.m_MessageQueue = new ApplicationMessageQueue(this);
            this.m_MessageQueue.Start();
        }
        #endregion

        #region イベントハンドラ

        #endregion

        /// <summary>
        /// メッセージを追加します。
        /// </summary>
        /// <param name="message"></param>
        public void AppendLogMessage(string message) {
            this.m_MessageQueue.AddMessage(new ApplicationMessage(() => {
                this.richTextBox1.AppendText(message);
                this.richTextBox1.SelectionStart = this.richTextBox1.Text.Length;
                this.richTextBox1.ScrollToCaret();
            }));
        }

        /// <summary>
        /// メッセージをクリアします。
        /// </summary>
        public void ClearLogMessage() {
            this.richTextBox1.ResetText();
        }
    }
}
