using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace LibraryExplorer.Window.DockWindow {

    //TODO:バージョン確認の実装

    /// <summary>
    /// フォルダ比較を行うウィンドウを表すクラスです。
    /// </summary>
    public partial class FolderCompareWindow : DockContent {


        #region フィールド(メンバ変数、プロパティ、イベント)


        #region SourceFolderPath
        private string m_SourceFolderPath;
        /// <summary>
        /// SourceFolderPathが変更された場合に発生するイベントです。
        /// </summary>
        public event EventHandler<EventArgs<string>> SourceFolderPathChanged;
        /// <summary>
        /// SourceFolderPathが変更された場合に呼び出されます。
        /// </summary>
        /// <param name="e">イベントパラメータ</param>
        protected void OnSourceFolderPathChanged(EventArgs<string> e) {
            this.SourceFolderPathChanged?.Invoke(this, e);
        }
        /// <summary>
        /// SourceFolderPathを取得、設定します。
        /// </summary>
        public string SourceFolderPath {
            get {
                return this.m_SourceFolderPath;
            }
            set {
                this.SetProperty(ref this.m_SourceFolderPath, value, ((oldValue) => {
                    if (this.SourceFolderPathChanged != null) {
                        this.OnSourceFolderPathChanged(new EventArgs<string>(oldValue));
                    }
                }));
            }
        }
        #endregion

        #region DestinationFolderPath
        private string m_DestinationFolderPath;
        /// <summary>
        /// DestinationFolderPathが変更された場合に発生するイベントです。
        /// </summary>
        public event EventHandler<EventArgs<string>> DestinationFolderPathChanged;
        /// <summary>
        /// DestinationFolderPathが変更された場合に呼び出されます。
        /// </summary>
        /// <param name="e">イベントパラメータ</param>
        protected void OnDestinationFolderPathChanged(EventArgs<string> e) {
            this.DestinationFolderPathChanged?.Invoke(this, e);
        }
        /// <summary>
        /// DestinationFolderPathを取得、設定します。
        /// </summary>
        public string DestinationFolderPath {
            get {
                return this.m_DestinationFolderPath;
            }
            set {
                this.SetProperty(ref this.m_DestinationFolderPath, value, ((oldValue) => {
                    if (this.DestinationFolderPathChanged != null) {
                        this.OnDestinationFolderPathChanged(new EventArgs<string>(oldValue));
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
        /// FolderCompareWindowオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        public FolderCompareWindow() {
            InitializeComponent();

            this.Initialize();
        }
        private void Initialize() {
            
            this.sourceFolderPathTextBox1.DataBindings.Add("Text", this, "SourceFolderPath", true, DataSourceUpdateMode.OnPropertyChanged);
            this.destinationFolderPathTextBox1.DataBindings.Add("Text", this,"DestinationFolderPath", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        #endregion

        #region イベントハンドラ

        #endregion


        #region CheckDiff

        /// <summary>
        /// 予めプロパティに設定されたSourceFolderPathとDestinationFolderPathを使用して、2つのフォルダを比較します。
        /// ファイルはテキストとして比較します。
        /// 一致不一致判断はCOMPコマンド。詳細はFCコマンドを使用して比較します。
        /// </summary>
        public void CheckDiff() {
            //TODO:CheckDiffの実装
            if (!Directory.Exists(this.m_SourceFolderPath)) {
                return;
            }
        }

        #endregion
    }
}
