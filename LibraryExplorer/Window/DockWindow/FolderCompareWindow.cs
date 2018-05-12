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

        private List<FileDiffInfo> m_FileDiffInfos;

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
            this.destinationFolderPathTextBox1.DataBindings.Add("Text", this, "DestinationFolderPath", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        #endregion

        #region イベントハンドラ
        private void listView1_SelectedIndexChanged(object sender, EventArgs e) {
            if (!this.ShowResult()) {
                this.ClearResultDisplay();
            }
        }

        #endregion

        #region 結果の表示
        private bool ShowResult() {
            if (this.listView1.SelectedItems.Count != 1) {
                return false;
            }
            if (!(this.listView1.SelectedItems[0].Tag is FileDiffInfo info)) {
                return false;
            }

            this.sourceFileRichTextBox1.Text = info.SourceText;
            this.destinationFileRichTextBox1.Text = info.DestinationText;
            this.diffResultRichTextBox1.Text = info.FCResult;

            return true;
        }

        #region ClearResultDisplay
        /// <summary>
        /// 表示をクリアします。
        /// </summary>
        private void ClearResultDisplay() {
            this.sourceFileRichTextBox1.Text = "";
            this.destinationFileRichTextBox1.Text = "";
            this.diffResultRichTextBox1.Text = "";
        }

        #endregion
        
        #endregion

        #region CheckDiff

        /// <summary>
        /// 予めプロパティに設定されたSourceFolderPathとDestinationFolderPathを使用して、2つのフォルダを比較します。
        /// ファイルはテキストとして比較します。
        /// 一致不一致判断はFCコマンドを使用して比較します。
        /// </summary>
        public void CheckDiff() {
            this.m_FileDiffInfos = new List<FileDiffInfo>();

            //TODO:CheckDiffの実装
            if (!Directory.Exists(this.m_SourceFolderPath)) {
                return;
            }
            if (!Directory.Exists(this.m_DestinationFolderPath)) {
                return;
            }

            //SourceFolderのファイルをリスト化
            this.m_FileDiffInfos = Directory.GetFiles(this.m_SourceFolderPath)
                .Select(f => new FileDiffInfo(f, this.m_DestinationFolderPath)).ToList();

            //差分の比較
            this.m_FileDiffInfos.ForEach(f => f.CheckDiff());

            //リストビューに表示
            try {
                this.listView1.BeginUpdate();
                this.listView1.Items.Clear();

                this.m_FileDiffInfos.ForEach(f => {
                    ListViewItem item = new ListViewItem() { Text = Path.GetFileName(f.SourceFileName), Tag = f };
                    if (!f.FCResultCode) {
                        item.BackColor = Color.Yellow;
                    }
                    this.listView1.Items.Add(item);
                });
            }
            finally {
                this.listView1.EndUpdate();
            }
        }

        #endregion


        #region 内部で使用するクラス

        private class FileDiffInfo{


            #region フィールド(メンバ変数、プロパティ、イベント)


            #region SourceFileName
            private string m_SourceFileName;
            /// <summary>
            /// SourceFileNameを取得します。
            /// </summary>
            public string SourceFileName {
                get {
                    return this.m_SourceFileName;
                }
            }
            #endregion

            #region DestinationFileName
            private string m_DestinationFileName;
            /// <summary>
            /// DestinationFileNameを取得します。
            /// </summary>
            public string DestinationFileName {
                get {
                    return this.m_DestinationFileName;
                }
            }
            #endregion

            #region SourceText
            private string m_SourceText;
            /// <summary>
            /// SourceTextを取得します。
            /// </summary>
            public string SourceText {
                get {
                    return this.m_SourceText;
                }
            }
            #endregion

            #region DestinationText
            private string m_DestinationText;
            /// <summary>
            /// DestinationTextを取得します。
            /// </summary>
            public string DestinationText {
                get {
                    return this.m_DestinationText;
                }
            }
            #endregion

            #region FCResult
            private string m_FCResult;
            /// <summary>
            /// FCResultを取得します。
            /// </summary>
            public string FCResult {
                get {
                    return this.m_FCResult;
                }
            }
            #endregion

            #region FCResultCode
            private bool m_FCResultCode;
            /// <summary>
            /// FCResultCodeを取得します。
            /// 正常終了(1)ならばtrue。それ以外の場合falseを返します。
            /// </summary>
            public bool FCResultCode {
                get {
                    return this.m_FCResultCode;
                }
            }
            #endregion

            #endregion

            #region コンストラクタ
            /// <summary>
            /// FileDiffInfoオブジェクトの新しいインスタンスを初期化します。
            /// </summary>
            /// <param name="sourceFilename"></param>
            /// <param name="destinationFolderName"></param>
            public FileDiffInfo(string sourceFilename,string destinationFolderName) {
                this.m_SourceFileName = sourceFilename;
                this.m_DestinationFileName = Path.Combine(destinationFolderName, Path.GetFileName(this.m_SourceFileName));
            }
            #endregion

            #region ファイルの存在確認
            public bool GetExistSource() {
                return File.Exists(this.m_SourceFileName);
            }
            public bool GetExistDestination() {
                return File.Exists(this.m_DestinationFileName);
            }
            #endregion

            #region ファイルの読み込み
            public void ReadSource() {
                if (this.GetExistSource()) {
                    this.m_SourceText = File.ReadAllText(this.m_SourceFileName,Encoding.GetEncoding("shift_jis"));
                }
            }
            public void ReadDestination() {
                if (this.GetExistDestination()) {
                    this.m_DestinationText = File.ReadAllText(this.m_DestinationFileName, Encoding.GetEncoding("shift_jis"));
                }
            }
            #endregion

            #region CheckDiff
            public void CheckDiff() {
                bool existSource = this.GetExistSource();
                bool existDestination = this.GetExistDestination();

                this.ReadSource();
                this.ReadDestination();

                //ファイルが両方とも存在する場合、FCコマンドで比較
                if (existSource && existDestination) {
                    this.StartCommand(this.m_SourceFileName, this.m_DestinationFileName);
                }
                else {
                    this.m_FCResult = "Source or Destination File is not found.";
                    this.m_FCResultCode = false;
                }
            }

            public async Task CheckDiffAsync() {
                bool existSource = this.GetExistSource();
                bool existDestination = this.GetExistDestination();

                this.ReadSource();
                this.ReadDestination();

                //ファイルが両方とも存在する場合、FCコマンドで比較
                if (existSource && existDestination) {
                    await this.StartCommandAsync(this.m_SourceFileName, this.m_DestinationFileName);
                }
                else {
                    this.m_FCResult = "Source or Destination File is not found.";
                    this.m_FCResultCode = false;
                }

            }

            #region StartCommand
            private void StartCommand(string sourceFileName, string destinationFileName) {
                //Processを非同期に実行し、完了を待つ
                using (Process process = this.CreateFCProcess(sourceFileName,destinationFileName)) {
                    this.StartCommandAsync(process).Wait();
                }
            }

            private async Task StartCommandAsync(string sourceFileName, string destinationFileName) {
                //Processを非同期に実行
                using (Process process = this.CreateFCProcess(sourceFileName, destinationFileName)) {
                    await this.StartCommandAsync(process);
                }
            }

            private Task StartCommandAsync(Process process) {
                var tcs = new TaskCompletionSource<bool>();
                bool started = false;

                process.Exited += (sender, args) => {
                    this.m_FCResultCode = (0 == process.ExitCode);
                    this.m_FCResult += $"ExitCode = {process.ExitCode}";
                    tcs.SetResult(true);
                    //process.Dispose();
                };
                process.OutputDataReceived += (sender, args) => {
                    //if (started) {
                    if (!string.IsNullOrEmpty(args.Data)) {
                        this.m_FCResult += $"{args.Data}\n";
                    }
                    //}
                };
                process.ErrorDataReceived += (sender, args) => {
                    //if (started) {
                    if (!string.IsNullOrEmpty(args.Data)) {
                        this.m_FCResult += $"Error : {args.Data}\n";
                    }
                    //}
                };

                //プロセスからの情報を受け取る変数の初期化
                this.m_FCResult = "";

                //プロセスの開始
                started = process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                return tcs.Task;
            }

            #region CreateFCProcess
            private Process CreateFCProcess(string sourceFileName,string destinationFileName) {
                ProcessStartInfo info = new ProcessStartInfo() {
                    FileName = "FC",
                    Arguments = $"\"{this.m_SourceFileName}\" \"{destinationFileName}\"",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                //Processを非同期に実行
                Process process = new Process() {
                    StartInfo = info,
                    EnableRaisingEvents = true,
                };
                return process;
            }
            #endregion

            #endregion

            #endregion


        }
        #endregion

    }
}
