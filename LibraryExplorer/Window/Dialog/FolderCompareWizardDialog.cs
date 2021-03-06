﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryExplorer.Control.Wizard;
using LibraryExplorer.Common.Request;
using LibraryExplorer.Data;

namespace LibraryExplorer.Window.Dialog {

    /// <summary>
    /// フォルダ比較を行うウィザードダイアログを表すクラスです。
    /// </summary>
    public partial class FolderCompareWizardDialog : Form {


        #region フィールド(メンバ変数、プロパティ、イベント)

        #region NotifyParentRequestイベント
        /// <summary>
        /// 親コントロールに対して要求を送信するイベントです。
        /// </summary>
        public event RequestEventHandler NotifyParentRequest;
        /// <summary>
        /// NotifyParentRequestイベントを発生させます。
        /// </summary>
        /// <param name="e"></param>
        protected void OnNotifyParentRequest(RequestEventArgs e) {
            this.NotifyParentRequest?.Invoke(this, e);
        }
        #endregion


        #region TargetFile
        /// <summary>
        /// TargetFileを取得、設定します。
        /// </summary>
        public OfficeFile TargetFile {
            get {
                return this.folderCompareWizard1.TargetFile;
            }
            set {
                this.folderCompareWizard1.TargetFile = value;
            }
        }
        #endregion
        
        #region TargetProject
        /// <summary>
        /// TargetProjectを取得、設定します。
        /// </summary>
        public LibraryProject TargetProject {
            get {
                return this.folderCompareWizard1.TargetProject;
            }
            set {
                this.folderCompareWizard1.TargetProject = value;
            }
        }
        #endregion



        #endregion

        #region コンストラクタ
        /// <summary>
        /// FolderCompareWizardDialogオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public FolderCompareWizardDialog() {
            InitializeComponent();

            this.Initialize();
        }

        private void Initialize() {
            this.folderCompareWizard1.Initialize();
            
        }
        #endregion

        #region イベントハンドラ
        private void FolderCompareWizardDialog_VisibleChanged(object sender, EventArgs e) {
            if (this.Visible) {
                this.folderCompareWizard1.Start();
            }
        }

        private void folderCompareWizard1_WizardCanceled(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
        }

        private void folderCompareWizard1_WizardFinished(object sender, EventArgs e) {
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// NotifyParentChangedを中継します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void folderCompareWizard1_NotifyParentRequest(object sender, RequestEventArgs e) {
            this.OnNotifyParentRequest(e);
        }

        #endregion

    }
}
