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

namespace LibraryExplorer.Window.Dialog {

    /// <summary>
    /// オプションダイアログを表すクラスです。
    /// </summary>
    public partial class OptionDialog : OptionDialogBase {
        
        //NOTE:設定項目が増えた場合、AppInfoChangedイベントハンドラでDataBindを登録する。

        #region フィールド(メンバ変数、プロパティ、イベント)


        #region AppInfo
        private AppInfo m_AppInfo;
        /// <summary>
        /// AppInfoが変更された場合に発生するイベントです。
        /// </summary>
        public event EventHandler<EventArgs<AppInfo>> AppInfoChanged;
        /// <summary>
        /// AppInfoが変更された場合に呼び出されます。
        /// </summary>
        /// <param name="e">イベントパラメータ</param>
        protected void OnAppInfoChanged(EventArgs<AppInfo> e) {
            this.AppInfoChanged?.Invoke(this, e);
        }
        /// <summary>
        /// AppInfoを取得、設定します。
        /// </summary>
        public AppInfo AppInfo {
            get {
                return this.m_AppInfo;
            }
            set {
                this.SetProperty(ref this.m_AppInfo, value, ((oldValue) => {
                    if (this.AppInfoChanged != null) {
                        this.OnAppInfoChanged(new EventArgs<AppInfo>(oldValue));
                    }
                }));
            }
        }
        #endregion



        #region PropertyChanged/SetProperty
        /// <summary>
        /// Propertyが変更されたことを通知するイベントです。
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// INotifyPropertyChangedを実装し、PropertyChangedイベントを発生させるためのsetterメソッド。
        /// fireEventデリゲートを指定することで個別のプロパティのChangedイベントを実装することができます。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="fireEvent"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected virtual bool SetProperty<T>(ref T field, T value, Action<T> fireEvent, [System.Runtime.CompilerServices.CallerMemberName]string propertyName = null) {
            if (Equals(field, value)) {
                return false;
            }
            T oldValue = field;
            field = value;
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
            fireEvent(oldValue);
            return true;
        }
        /// <summary>
        /// PropertyChangedイベントを発生させます。
        /// </summary>
        /// <param name="e"></param>
        protected void OnPropertyChanged(PropertyChangedEventArgs e) {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(e.PropertyName));
        }
        /// <summary>
        /// 変更前の値を保持するEventArgsクラスの派生クラスです。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class EventArgs<T> : EventArgs {

            #region OldValue
            private T m_OldValue;
            /// <summary>
            /// OldValueを取得します。
            /// </summary>
            public T OldValue {
                get {
                    return this.m_OldValue;
                }
            }
            #endregion

            /// <summary>
            /// EventArgsクラスの新しいインスタンスを初期化します。
            /// </summary>
            /// <param name="value"></param>
            public EventArgs(T value) {
                this.m_OldValue = value;
            }
        }
        #endregion

        #endregion

        #region コンストラクタ
        /// <summary>
        /// OptionDialogオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public OptionDialog() {
            InitializeComponent();

            this.Initialize();
        }
        private void Initialize() {
            this.AppInfoChanged += this.OptionDialog_AppInfoChanged;
        }

        #endregion

        #region イベントハンドラ
        
        #region AppInfo
        private void OptionDialog_AppInfoChanged(object sender, EventArgs<AppInfo> e) {
            if (this.m_AppInfo != null) {                

                //エディタ設定
                this.AddSingleDataBind(this.editorPathTextBox,"Text", this.m_AppInfo, "EditorPath");
                this.AddSingleDataBind(this.editorArgumentsTextBox, "Text", this.m_AppInfo, "EditorArguments");
                //ファイル比較
                this.AddSingleDataBind(this.diffToolPathTextBox, "Text", this.m_AppInfo, "DiffToolPath");
                this.AddSingleDataBind(this.diffToolArgumentsTextBox,"Text", this.m_AppInfo,"DiffToolArguments");

            }
        }
        #endregion

        #region Control
        /// <summary>
        /// エディタの参照ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editorPathReferenceButton_Click(object sender, EventArgs e) {
            this.edhitorPathOpenFileDialog1.FileName = this.editorPathTextBox.Text;
            if (this.edhitorPathOpenFileDialog1.ShowDialog() == DialogResult.OK) {
                this.editorPathTextBox.Text = this.edhitorPathOpenFileDialog1.FileName;
            }
        }

        /// <summary>
        /// 比較ツールの参照ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void diffToolPathReferenceButton_Click(object sender, EventArgs e) {
            this.diffToolPathOpenFileDialog1.FileName = this.diffToolPathTextBox.Text;
            if (this.diffToolPathOpenFileDialog1.ShowDialog() == DialogResult.OK) {
                this.diffToolPathTextBox.Text = this.diffToolPathOpenFileDialog1.FileName;
            }

        }

        #endregion

        #endregion
    }
}
