using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ctrl = System.Windows.Forms.Control;

namespace LibraryExplorer.Control {

    /// <summary>
    /// TabControlに各ステップのUIを定義できるウィザードコントロール
    /// Startメソッドを呼ぶと、TabControlを非表示にし、TabPageの内容を順に表示していく。
    /// </summary>
    public partial class EditableWizardControl : UserControl {


        #region フィールド(メンバ変数、プロパティ、イベント)

        private Panel m_CurrentPanel;


        #region BeforePageChangeイベント
        /// <summary>
        /// Pageが変更される前に発生するイベントです。
        /// </summary>
        public event EventHandler BeforePageChange;
        /// <summary>
        /// BeforePageChangeイベントを発生させます。
        /// </summary>
        /// <param name="e"></param>
        protected void OnBeforePageChange(EventArgs e) {
            this.BeforePageChange?.Invoke(this, e);
        } 
        #endregion

        #region AfterPageChangeイベント
        /// <summary>
        /// Pageが変更された後に発生するイベントです。
        /// </summary>
        public event EventHandler AfterPageChange;
        /// <summary>
        /// AfterPageChangeイベントを発生させます。
        /// </summary>
        /// <param name="e"></param>
        protected void OnAfterPageChange(EventArgs e) {
            this.AfterPageChange?.Invoke(this, e);
        }
        #endregion


        #region WizardFinishedイベント
        /// <summary>
        /// ウィザードが終了した時に発生するイベントです。
        /// </summary>
        public event EventHandler WizardFinished;
        /// <summary>
        /// WizardFinishedイベントを発生させます。
        /// </summary>
        /// <param name="e"></param>
        protected void OnWizardFinished(EventArgs e) {
            if (this.WizardFinished != null) {
                this.WizardFinished(this, e);
            }
        }
        #endregion

        #region WizardCanceledイベント
        /// <summary>
        /// ウィザードがキャンセルされたときに発生するイベントです。
        /// </summary>
        public event EventHandler WizardCanceled;
        /// <summary>
        /// WizardCanceledイベントを発生させます。
        /// </summary>
        /// <param name="e"></param>
        protected void OnWizardCanceled(EventArgs e) {
            if (this.WizardCanceled != null) {
                this.WizardCanceled(this, e);
            }
        } 
        #endregion



        #region CurrentStepNo
        private int m_CurrentStepNo;
        /// <summary>
        /// CurrentStepNoを取得します。
        /// </summary>
        [Browsable(false)]
        public int CurrentStepNo {
            get {
                return this.m_CurrentStepNo;
            }
        }
        #endregion

        

        #region Initialized
        private bool m_Initialized;
        /// <summary>
        /// Initializedを取得、設定します。
        /// </summary>
        [Browsable(false)]
        public bool Initialized {
            get {
                return this.m_Initialized;
            }
            set {
                this.m_Initialized = value;
            }
        }
        #endregion

        #region TabPages
        /// <summary>
        /// ウィザードに使用するコントロールを一時的に格納するTabPageのコレクションを取得します。
        /// </summary>
        public TabControl.TabPageCollection TabPages {
            get {
                return this.tabControl1.TabPages;
            }
        }

        #endregion


        #region CanGoNext
        /// <summary>
        /// 次へ進めるかどうかを取得します。
        /// </summary>
        [Browsable(false)]
        public bool CanGoNext {
            get {
                return this.m_CurrentStepNo < this.tabControl1.TabPages.Count - 1;
            }
        }
        #endregion

        #region CanGoBack
        /// <summary>
        /// 前に戻れるかどうかを取得します。
        /// </summary>
        [Browsable(false)]
        public bool CanGoBack {
            get {
                return 0 < this.m_CurrentStepNo;
            }
        }

        #endregion


        #endregion

        #region コンストラクタ

        /// <summary>
        /// EditableWizardControlオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public EditableWizardControl() {
            InitializeComponent();

            this.m_CurrentStepNo = 0;
            this.m_Initialized = false;
            this.m_CurrentPanel = null;

        }

        #endregion

        #region イベントハンドラ

        /// <summary>
        /// 次へボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goNextButton_Click(object sender, EventArgs e) {
            if (this.CanGoNext) {
                this.GoNext();
            }
            else {
                this.Finish();
            }
        }
        /// <summary>
        /// 戻るボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goBackButton_Click(object sender, EventArgs e) {
            this.GoBack();

        }
        /// <summary>
        /// キャンセルボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, EventArgs e) {
            this.Cancel();
        }

        #endregion

        #region ウィザードを操作するメソッド

        #region Initialize
        /// <summary>
        /// ウィザードの初期化を行います。
        /// Startメソッドを呼ぶ前にこのメソッドを呼び出す必要があります。
        /// </summary>
        public void Initialize() {
            //TabPageに配置されているコントロールを、Panelにまとめておく
            this.tabControl1.TabPages.Cast<TabPage>().ToList().ForEach(page => this.InitializePage(page));
        }
        private void InitializePage(TabPage page) {
            //Panelを作成し、コントロールを移動
            Panel panel = new Panel();
            panel.BorderStyle = BorderStyle.None;
            panel.Controls.AddRange(page.Controls.Cast<ctrl>().ToArray());

            //もとのコントロールは移動されるため削除不要
            //page.Controls.Clear();

            page.Controls.Add(panel);

            this.m_Initialized = true;
        } 
        #endregion

        /// <summary>
        /// ウィザードを開始します。
        /// このメソッドを呼ぶ前にInitializeメソッドが呼び出されていない場合、例外が発生します。
        /// </summary>
        public void Start() {
            if (!this.Initialized) {
                throw new InvalidOperationException($"Initialize method should be called , before Start method is called.");
            }
            this.tabControl1.Visible = false;

            this.ChangePage(-1, 0);

            this.OnStart();
        }

        /// <summary>
        /// 1ページ進みます。
        /// </summary>
        public void GoNext() {
            if (!this.CanGoNext) {
                return;
            }

            this.ChangePage(this.m_CurrentStepNo, m_CurrentStepNo + 1);

            this.OnGoNext();
        }

        /// <summary>
        /// 1ページ戻ります。
        /// </summary>
        public void GoBack() {
            if (!this.CanGoBack) {
                return;
            }

            this.ChangePage(this.m_CurrentStepNo, m_CurrentStepNo - 1);

            this.OnGoBack();
        }
        /// <summary>
        /// ウィザードをキャンセルします。
        /// </summary>
        public void Cancel() {

            this.ChangePage(this.m_CurrentStepNo, -1);

            this.OnCancel();

            this.OnWizardCanceled(EventArgs.Empty);
        }

        /// <summary>
        /// ウィザードを終了します。
        /// </summary>
        public void Finish() {
            this.ChangePage(this.m_CurrentStepNo, -1);

            this.OnFinish();

            this.OnWizardFinished(EventArgs.Empty);
        }


        #region ChangePage
        private void ChangePage(int prevPageIndex, int nextPageIndex) {

            this.OnBeforePageChange(EventArgs.Empty);

            //===========================================
            //現在表示しているページを元に戻す
            if (prevPageIndex >= 0) {
                TabPage prevPage = this.TabPages[prevPageIndex];
                prevPage.Controls.Add(this.m_CurrentPanel);
            }

            //===========================================
            //次に表示するページの状態確認
            if (nextPageIndex >= 0) {
                TabPage nextPage = this.TabPages[nextPageIndex];
                if (nextPage.Controls.Count != 1) {
                    this.InitializePage(nextPage);
                }
                this.m_CurrentPanel = nextPage.Controls[0] as Panel;
                this.m_CurrentStepNo = nextPageIndex;

                //---------------------------------
                this.SuspendLayout();

                this.m_CurrentPanel.Dock = DockStyle.Fill;

                this.MainPanel.Controls.Add(this.m_CurrentPanel);
                this.ResumeLayout();
                //---------------------------------
            }

            this.ChangeButtonStatus();

            this.OnAfterPageChange(EventArgs.Empty);
        }
        private void ChangeButtonStatus() {
            this.goNextButton.Text = this.CanGoNext ? "次へ" : "完了";
            this.goBackButton.Enabled = this.CanGoBack;
        }

        #endregion

        #endregion

        #region 各ページ遷移のタイミングで呼び出されるvirtualメソッド
        /// <summary>
        /// 派生クラスでオーバーライドされると、Start時の動作を定義します。
        /// </summary>
        protected virtual void OnStart() {
        }
        /// <summary>
        /// 派生クラスでオーバーライドされると、GoNext時の動作を定義します。
        /// </summary>
        protected virtual void OnGoNext() {
        }
        /// <summary>
        /// 派生クラスでオーバーライドされると、GoBack時の動作を定義します。
        /// </summary>
        protected virtual void OnGoBack() {
        }
        /// <summary>
        /// 派生クラスでオーバーライドされると、Cancel時の動作を定義します。
        /// </summary>
        protected virtual void OnCancel() {
        }
        /// <summary>
        /// 派生クラスでオーバーライドされると、Finish時の動作を定義します。
        /// </summary>
        protected virtual void OnFinish() {
        }
        #endregion

    }
}
