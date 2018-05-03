using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LibraryExplorer.Common;

namespace LibraryExplorer.Data {

    /// <summary>
    /// Libraryを構成するファイルを表すクラスです。
    /// </summary>
    public class LibraryFile {

        //この形のヘッダーが記載されているファイルをライブラリファイルとして扱う
        //
        //ヘッダーの識別子は設定要素とする
        //MY_LIBRARY_HEADER_START
        //MY_LIBRARY_HEADER_END
        //

        /*
        'MY_LIBRARY_HEADER_START===============================

        ' Package       : 
        ' Name          : 
        ' Function      : 
        ' Description   : 
        ' Reference     :
        ' Implements    :
        ' Rev           : 0.0
        '
        'MY_LIBRARY_HEADER_END=================================
        */


        #region フィールド(メンバ変数、プロパティ、イベント)

        #region FileName
        private string m_FileName;
        /// <summary>
        /// FileNameを取得、設定します。
        /// </summary>
        public string FileName {
            get {
                return this.m_FileName;
            }
            protected set {
                this.m_FileName = value;
            }
        }
        #endregion

        #region ModuleName
        private string m_ModuleName;
        /// <summary>
        /// ModuleNameを取得、設定します。
        /// </summary>
        public string ModuleName {
            get {
                return this.m_ModuleName;
            }
        }
        #endregion

        #region HeaderText
        private string m_HeaderText;
        /// <summary>
        /// HeaderTextを取得、設定します。
        /// </summary>
        public string HeaderText {
            get {
                return this.m_HeaderText;
            }
        }
        #endregion

        #region Package
        private string m_Package;
        /// <summary>
        /// Packageを取得します。
        /// </summary>
        public string Package {
            get {
                return this.m_Package;
            }
        }
        #endregion

        #region Name
        private string m_Name;
        /// <summary>
        /// Nameを取得します。
        /// </summary>
        public string Name {
            get {
                return this.m_Name;
            }
        }
        #endregion

        #region Function
        private string m_Function;
        /// <summary>
        /// Functionを取得します。
        /// </summary>
        public string Function {
            get {
                return this.m_Function;
            }
        }
        #endregion

        #region Description
        private string m_Description;
        /// <summary>
        /// Descriptionを取得します。
        /// </summary>
        public string Description {
            get {
                return this.m_Description;
            }
        }
        #endregion

        #region Reference
        private string m_Reference;
        /// <summary>
        /// Referenceを取得します。
        /// </summary>
        public string Reference {
            get {
                return this.m_Reference;
            }
        }
        #endregion

        #region Implements
        private string m_Implements;
        /// <summary>
        /// Implementsを取得します。
        /// </summary>
        public string Implements {
            get {
                return this.m_Implements;
            }
        }
        #endregion

        #region Revision
        private string m_Revision;
        /// <summary>
        /// Revisionを取得します。
        /// </summary>
        public string Revision {
            get {
                return this.m_Revision;
            }
        }
        #endregion

        #region Notes
        private string m_Notes;
        /// <summary>
        /// Notesを取得します。
        /// </summary>
        public string Notes {
            get {
                return this.m_Notes;
            }
        }
        #endregion

        #endregion

        #region コンストラクタ
        /// <summary>
        /// LibraryFileオブジェクトの新しいインスタンスを初期化します。
        /// </summary>
        protected LibraryFile() {
        }
        #endregion

        #region イベントハンドラ

        #endregion

        #region staticメソッド
        
        #region IsTargetFile
        /// <summary>
        /// 指定したファイルの拡張子が、ライブラリファイルとして扱う拡張子リストと一致するかどうかを返します。
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool IsTargetFile(string filename) {
            return AppMain.g_AppMain.AppInfo.TargetExtentions
                .Any(x => Path.GetExtension(filename).ToLower() == x.ToLower());
            //return AppMain.g_AppMain.AppInfo.TargetExtentions.Count(x => Path.GetExtension(filename).ToLower() == x.ToLower()) != 0;
        }
        #endregion

        #region HasOtherFiles
        /// <summary>
        /// 指定したファイルの拡張子が、複数ファイルで構成されるライブラリファイルとして扱う拡張子リストと一致するかどうかを返します。
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool HasOtherFiles(string filename) {
            return AppMain.g_AppMain.AppInfo.HasOtherFilesExtentions
                .Any(x => Path.GetExtension(filename).ToLower() == x.Key.ToLower());
            //return AppMain.g_AppMain.AppInfo.HasOtherFilesExtentions.Count(x => Path.GetExtension(filename).ToLower() == x.Key.ToLower()) != 0;
        }
        #endregion

        #endregion
        
        #region GetOtherFiles
        /// <summary>
        /// このインスタンスが、複数のファイルから構成されるライブラリを表している場合、残りのファイルのパスを返します。
        /// 単一ファイルのライブラリの場合、nullを返します。
        /// </summary>
        /// <returns></returns>
        public List<string> GetOtherFiles() {
            if (LibraryFile.HasOtherFiles(this.FileName)) {
                List<string> list = AppMain.g_AppMain.AppInfo.HasOtherFilesExtentions
                    .First(x => Path.GetExtension(this.FileName).ToLower() == x.Key.ToLower())
                    .Value;
                //拡張子を置換したリストを返す
                return list.Select(x => this.FileName.Replace(Path.GetExtension(this.FileName), x))
                    .ToList();
            }
            return null;
        }
        #endregion

        #region ReadFile
        /// <summary>
        /// ファイル名を指定して、LibraryFileクラスの新しいインスタンスを作成します。
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static LibraryFile FromFile(string filename) {
            LibraryFile libraryFile = new LibraryFile() { m_FileName = filename };
            libraryFile.ReadFileHeader();
            return libraryFile;
        }
        /// <summary>
        /// ヘッダを読み込みます。
        /// </summary>
        public void ReadFileHeader() {
            //ファイルを読み込む
            string source = this.OnReadFile(true,out bool foundHeader);

            //モジュール名の取得
            //モジュール名は標準で含まれるので、ヘッダーが見つからなかった場合も取得可能
            this.m_ModuleName = this.GetModuleName(source);

            //ヘッダが見つかった場合、ヘッダ情報の取得
            if (foundHeader) {
                this.m_HeaderText = source;
                this.m_Package = this.GetHeaderInfo(source, "Package");
                this.m_Name = this.GetHeaderInfo(source, "Name");
                this.m_Function = this.GetHeaderInfo(source, "Function");
                this.m_Description = this.GetHeaderInfo(source, "Description");
                this.m_Reference = this.GetHeaderInfo(source, "Reference");
                this.m_Implements = this.GetHeaderInfo(source, "Implements");
                this.m_Revision = this.GetHeaderInfo(source,"Rev");
                this.m_Notes = this.GetHeaderNotes(source);
            }
        }
        /// <summary>
        /// ファイルを全て読み込みます。
        /// </summary>
        /// <returns></returns>
        public string ReadFile() {
            //out変数は不要のため「値の破棄」を使用する
            string source = this.OnReadFile(false, out _);

            return source;
        }

        #region GetHeaderInfo
        /// <summary>
        /// キーワードを指定してヘッダ情報を取得します。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetHeaderInfo(string source, string key) {
            string headerStart = AppMain.g_AppMain.AppInfo.LibraryHeaderStart;
            string headerEnd = AppMain.g_AppMain.AppInfo.LibraryHeaderEnd;
            string value = this.GetGroup2($@"{headerStart}[\s\S]*'.*{key}\s*:\s*(.*)\s*$[\s\S]*{headerEnd}", source);
            return value.Trim();
        }

        #endregion

        #region GetHeaderNotes
        /// <summary>
        /// ヘッダの末尾に記載された注釈を取得します。
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private string GetHeaderNotes(string source) {
            string headerStart = AppMain.g_AppMain.AppInfo.LibraryHeaderStart;
            string headerEnd = AppMain.g_AppMain.AppInfo.LibraryHeaderEnd;
            string value = this.GetGroup2($@"{headerStart}[\s\S]*'.*Rev\s*:\s*.*\s*$([\s\S]*){headerEnd}", source);
            return value.Trim();
        } 
        #endregion

        #region GetModuleName
        private string GetModuleName(string source) {
            return this.GetGroup2("Attribute VB_Name = \"(.*)\"",source);
        }

        #endregion



        #region OnReadFile
        /// <summary>
        /// 指定されたファイルを開き、ファイルの内容を読み取って返します。
        /// readHeaderがtrueの場合、ヘッダーのみを返します。
        /// ヘッダーが見つからなかった場合、ファイルの内容を返します。
        /// </summary>
        /// <param name="readHeader"></param>
        /// <param name="foundHeader"></param>
        /// <returns></returns>
        private string OnReadFile(bool readHeader,out bool foundHeader) {
            string filename = this.FileName;
            StringBuilder stringBuffer = new StringBuilder();
            string headerEnd = AppMain.g_AppMain.AppInfo.LibraryHeaderEnd;
            foundHeader = false;
            try {
                using (StreamReader reader = new StreamReader(File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read), Encoding.GetEncoding("shift_jis"))) {
                    while (!reader.EndOfStream) {
                        string line = reader.ReadLine();
                        stringBuffer.Append(line);
                        stringBuffer.Append("\r\n");
                        if (readHeader && line.Contains(headerEnd)) {
                            foundHeader = true;
                            break;
                        }
                    }
                }
                return stringBuffer.ToString();
            }
            catch (FileNotFoundException ex) {
                throw new ApplicationException($"ファイルが見つかりません。FileName={filename}", ex);
            }
        } 
        #endregion

        #endregion

        #region ユーティリティ
        /// <summary>
        /// 指定したパターンを使用して正規表現オブジェクトを作成し、最初に見つかった文字列の中から、グループ指定された文字列を返します。
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        private string GetGroup2(string pattern, string source) {
            Regex regex1 = this.CreateRegEx(pattern);
            Match match1 = regex1.Match(source);
            if (match1.Groups.Count > 1) {
                //Groups[0]はマッチした文字列全体を示すため、Groups[1]を返す
                return match1.Groups[1].Value;
            }
            else {
                return "N/A";
            }

        }

        /// <summary>
        /// このインスタンスで使用する正規表現オブジェクトを作成します。
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        private Regex CreateRegEx(string pattern) {
            RegexOptions option1 = RegexOptions.Multiline | RegexOptions.IgnoreCase;
            return new Regex(pattern, option1);
        } 
        #endregion


    }
}
