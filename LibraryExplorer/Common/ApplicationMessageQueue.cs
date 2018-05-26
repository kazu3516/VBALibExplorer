using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace LibraryExplorer.Common {

    #region ApplicationMessageQueue
    /// <summary>
    /// アプリケーション固有のメッセージキューを表します。
    /// Application_Idleのタイミングでキューに登録されているメッセージの処理を行います。
    /// </summary>
    public class ApplicationMessageQueue {


        #region フィールド(メンバ変数、プロパティ、イベント)
        private Queue<ApplicationMessage> m_MessageQueue;


        #region Owner
        private System.Windows.Forms.Control m_Owner;
        /// <summary>
        /// Ownerを取得します。
        /// </summary>
        public System.Windows.Forms.Control Owner {
            get {
                return this.m_Owner;
            }
        }
        #endregion

        #region Running
        private bool m_Running;
        /// <summary>
        /// Runningを取得、設定します。
        /// </summary>
        public bool Running {
            get {
                return this.m_Running;
            }
            set {
                this.m_Running = value;
            }
        }
        #endregion


        #endregion

        #region コンストラクタ
        /// <summary>
        /// ApplicationMessageQueueオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="owner"></param>
        public ApplicationMessageQueue(System.Windows.Forms.Control owner) :this(){
            this.m_Owner = owner;
        }

        /// <summary>
        /// ApplicationMessageQueueオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public ApplicationMessageQueue() {
            this.m_MessageQueue = new Queue<ApplicationMessage>();
        }


        #endregion

        #region イベントハンドラ
        private void Application_Idle(object sender, EventArgs e) {
            //Console.WriteLine($"[Owner:{this.m_Owner.GetType().Name}]MessageProcess Start.Queueing Message Count = {this.m_MessageQueue.Count}");
            while (this.m_MessageQueue.Count != 0) {
                ApplicationMessage msg = this.m_MessageQueue.Dequeue();

                msg.Action();

                if (msg.SuspendMessageProcess) {
                    //Console.WriteLine($"{this.GetType().FullName}:SuspendMessageProcess");
                    break;
                }
            }
            //Console.WriteLine("MessageProcess End");
        }
        #endregion

        /// <summary>
        /// キューイングを開始します。
        /// </summary>
        public void Start() {
            if (!this.m_Running) {
                Application.Idle += this.Application_Idle;
            }
            this.m_Running = true;
        }
        /// <summary>
        /// キューイングを停止します。
        /// 停止中に追加されたメッセージは保持され、再開後に処理されます。
        /// </summary>
        public void Stop() {
            if (this.m_Running) {
                Application.Idle -= this.Application_Idle;
            }
            this.m_Running = false;
        }
        /// <summary>
        /// キューにメッセージを追加します。
        /// </summary>
        /// <param name="message"></param>
        public void AddMessage(ApplicationMessage message) {
            this.m_MessageQueue.Enqueue(message);
        }
    } 
    #endregion

    #region ApplicationMessage
    /// <summary>
    /// ApplicationMessageQueueに登録するメッセージを表すクラスです。
    /// </summary>
    public class ApplicationMessage {

        #region フィールド(メンバ変数、プロパティ、イベント)

        #region Action
        private Action m_Action;
        /// <summary>
        /// Actionを取得、設定します。
        /// </summary>
        public Action Action {
            get {
                return this.m_Action;
            }
            set {
                this.m_Action = value;
            }
        }
        #endregion

        #region SuspendMessageProcess
        private bool m_SuspendMessageProcess;
        /// <summary>
        /// SuspendMessageProcessを取得、設定します。
        /// trueの場合、このメッセージの処理完了後、メッセージキューの処理を中断します。
        /// </summary>
        public bool SuspendMessageProcess {
            get {
                return this.m_SuspendMessageProcess;
            }
            set {
                this.m_SuspendMessageProcess = value;
            }
        }
        #endregion

        #endregion

        #region コンストラクタ
        /// <summary>
        /// ApplicationMessageオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="action"></param>
        public ApplicationMessage(Action action) {
            this.m_Action = action;
        }
        /// <summary>
        /// ApplicationMessageオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="action"></param>
        /// <param name="suspendMessageProcess"></param>
        public ApplicationMessage(Action action, bool suspendMessageProcess) : this(action) {
            this.m_SuspendMessageProcess = suspendMessageProcess;
        }
        #endregion

    } 
    #endregion

}
