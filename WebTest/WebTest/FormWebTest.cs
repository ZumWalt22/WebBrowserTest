using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;
using ExcelMng;

namespace WindowsFormsApplication1
{
    public partial class FormWebTest : Form
    {
        WebTestController webTestController;

        public FormLog formLog = new FormLog();

        /// <summary>
        /// テストケース設置Excelファイル（デフォルト）
        /// </summary>
        //string TEST_CASE_NAME = "TestCase.xlsx";
        //string TEST_CASE_NAME = "TestCase1.csv";

        public int selectedIndex = 0;

        /// <summary>
        /// エビデンスファイルExcelコントローラー
        /// </summary>
        public ExcelController excelController = null;

        /// <summary>
        /// EXCELアプリ
        /// </summary>
        //Microsoft.Office.Interop.Excel.Application oXls = new Microsoft.Office.Interop.Excel.Application();

        /// <summary>
        /// Excel出力
        /// </summary>
        //ExcelOutput excelOutput = null;

        /// <summary>
        /// Webブラウザ遷移回数
        /// </summary>
        int documentCompletedCount = 0;

        #region Form系

        public FormWebTest()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Formロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            //excelControllerインスタンス化
            //excelController = new ExcelController(oXls);

            //excelOutput = new ExcelOutput(excelController);

            // カレントディレクトリを取得する
            string stCurrentDir = System.IO.Directory.GetCurrentDirectory();

            webTestController = new WebTestController(this, null, excelController, formLog);

            //------------テストケース

            //フルパスを作成
            //string fullPass = stCurrentDir + @"\excel\" + TEST_CASE_NAME;
            string fullPass = stCurrentDir + @"\TestCase\"; //Dirのみを指定

            //テストケース設定
            setTestCase(fullPass);

            //-----------エビデンス

            ////フルパスを作成
            //string fullEbiPass = stCurrentDir + @"\Ebidence\" + "Ebidence.xlsx";
            //excelController.setExcelFilePass(fullEbiPass);
            //excelController.openExcelFile();
        }

        /// <summary>
        /// Formクローズ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormWebTest_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Excel切断
            //excelController.closeExcelFile();
        }

        // 新しくウィンドウを開かれようとするときに発生する
        void webBrowser_NewWindow2(object sender, WebBrowserNewWindow2EventArgs e)
        {
            WebBrowserEx webBrowserEx = new WebBrowserEx();
            
            //新しいタブウインドウを設定
            setNewWebTab(webBrowserEx);

            e.ppDisp = webBrowserEx.Application;
        }

        /// <summary>
        /// 新しいタブウインドウを設定する
        /// </summary>
        /// <param name="e"></param>
        private void setNewWebTab(WebBrowserEx newBrowser)
        {
            // 新しい WebBrowser の初期化
            newBrowser.Dock = DockStyle.Fill;

            //新しい WebBrowser のコンテナ（下記はタブの場合）
            var tabPage = new TabPage();
            tabPage.Controls.Add(newBrowser);
            tabControl1.TabPages.Add(tabPage);

            // 新しい WebBrowser に表示させる設定
            newBrowser.RegisterAsBrowser = true;

            // 新しい WebBrowser からさらにウィンドウを開かれるときも同じようにする
            newBrowser.NewWindow2 += webBrowser_NewWindow2;

            //新しいタブに名称を設定する
            //tabPage.Name = newBrowser.Document.Title;

            //新規タブを選択する
            tabControl1.SelectedTab = tabPage;

            //Ｗｅｂ画面表示完了イベント登録
            newBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowserDocumentCompleted);

            //テスト対象WebBrowserを変更する
            if (tabControl1.SelectedTab.Controls.Count > 0)
            {
                webTestController.setWebBrowser((WebBrowserEx)tabControl1.SelectedTab.Controls[0]);
            }
        }

        //Ｗｅｂ画面表示完了
        private void webBrowserDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //画面推移回数をインクリメント
            documentCompletedCount++;

            //ポストバックカウントを表示する
            toolStripStatusLabel1.Text = "post Back Count : " + documentCompletedCount.ToString();

            //遷移先URLを設定
            comboBoxUrl.Text = e.Url.AbsolutePath;
        }

        //選択タブが変更された場合
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Controls.Count > 0)
            {
                //テスト対象WebBrowserを変更する
                webTestController.setWebBrowser((WebBrowserEx)tabControl1.SelectedTab.Controls[0]);
            }
        }

        #endregion


        #region テストファル設定

        /// <summary>
        /// テストケース設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            makeTestInfoVO();
        }

        /// <summary>
        /// エビデンスファイル設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonEvidensSet_Click(object sender, EventArgs e)
        {
            setEvidenseFile();
        }

        #endregion

        /// <summary>
        /// テスト自動実行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonTestAllDo_Click(object sender, EventArgs e)
        {
            //TestCase選択されているかを確認
            if (checkedListBox1.CheckedIndices.Count == 0)
            {
                MessageBox.Show("対象のテストケースが選択されていません", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            //ポストバックカウントをリセット
            documentCompletedCount = 0;

            ////----------URLを設定

            if (webTestController.webBrowser == null || tabControl1.SelectedTab == null)
            {
                WebBrowserEx webBrowserEx = new WebBrowserEx();

                //新しいタブウインドウを設定
                setNewWebTab(webBrowserEx);

                webTestController.webBrowser = webBrowserEx;

            }

            //ウエイト
            //System.Threading.Thread.Sleep(1000);

            //テスト自動実行
            webTestController.testDone(checkedListBox1.CheckedIndices.Cast<int>().ToList());
        }

        #region テスト実施　マニュアル操作

        /// <summary>
        /// URLに移動
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            //ポストバックカウントをリセット
            documentCompletedCount = 0;

            // URLに移動
            webTestController.webBrowser.Navigate(comboBoxUrl.Text);

            //URLは登録済みか確認
            foreach (string item in comboBoxUrl.Items)
	        {
                //同じ場合、設定しないで戻る
                if (comboBoxUrl.Text.Equals(item))
                {
                    return;
                }
	        }

            // 設定されたURLを保持
            comboBoxUrl.Items.Add(comboBoxUrl.Text);
        }

        /// <summary>
        /// URLを設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonUrlSet_Click(object sender, EventArgs e)
        {
            //一覧選択チェック
            if (!checkTestCaseListSelected())
            {
                return;
            }

            ////URLを設定
            //comboBoxUrl.Text = webTestController.getWebTestInfo(checkedListBox1.SelectedIndex).url;

            ////マニュアル実行
            //webTestController.testDoneManual(checkedListBox1.SelectedIndex, 0);


            WebBrowserEx webBrowserEx = new WebBrowserEx();

            //新しいタブウインドウを設定
            setNewWebTab(webBrowserEx);

            webTestController.webBrowser = webBrowserEx;

            ////新しいウインドウが開かれる時の動作のメソッドをハンドルに設定する
            //webBrowserEx1.NewWindow2 += new WebBrowserNewWindow2EventHandler(webBrowser_NewWindow2);

            ////Ｗｅｂ画面表示完了イベント登録(デフォルトWebブラウザ)
            //webBrowserEx1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowserDocumentCompleted);

            WebTestDto webTestDto = webTestController.getWebTestInfo(checkedListBox1.SelectedIndex);

            //ポストバックカウントをリセット
            documentCompletedCount = 0;

            // URLに移動
            webTestController.webBrowser.Navigate(webTestDto.url);
		}

        /// <summary>
        /// ケース設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            //一覧選択チェック
            if (!checkTestCaseListSelected() || !checkWebWindowOpened())
            {
                return;
            }

            //マニュアル実行
            webTestController.testDoneManual(checkedListBox1.SelectedIndex,1);
        }

        /// <summary>
        /// テスト実施
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //一覧選択チェック
            if (!checkTestCaseListSelected() || !checkWebWindowOpened())
            {
                return;
            }

            //マニュアル実行
            webTestController.testDoneManual(checkedListBox1.SelectedIndex, 2);
        }

        /// <summary>
        /// 画面キャプチャ取得
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            //一覧選択チェック
            if (!checkTestCaseListSelected() || !checkWebWindowOpened())
            {
                return;
            }

            //マニュアル実行
            webTestController.testDoneManual(checkedListBox1.SelectedIndex, 3);
        }

        ///// <summary>
        ///// DBエビデンス作成
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void button6_Click(object sender, EventArgs e)
        //{
        //    //マニュアル実行
        //    webTestController.testDoneManual(checkedListBox1.SelectedIndex, 3);

        //    //webBrowserEx1.Document.InvokeScript("funcWithoutPram");
        //}

        /// <summary>
        /// テストケースリスト選択確認
        /// </summary>
        /// <returns></returns>
        public bool checkTestCaseListSelected()
        {
            //一覧選択チェック
            if (checkedListBox1.SelectedIndex == -1)
            {
                MessageBox.Show("対象のテストケースが選択されていません", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Web画面設定確認
        /// </summary>
        /// <returns></returns>
        public bool checkWebWindowOpened()
        {
            //一覧選択チェック
            if (webTestController.webBrowser == null)
            {
                MessageBox.Show("テスト対象のWeb画面が設定されていません", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
        #endregion

        #region 処理サブルーチン

        /// <summary>
        /// EXCELファイル情報からVOを作成する
        /// </summary>
        private void makeTestInfoVO()
        {
            //FolderBrowserDialogクラスのインスタンスを作成
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            //上部に表示する説明テキストを指定する
            fbd.Description = "フォルダを指定してください。";
            //ルートフォルダを指定する
            //デフォルトでDesktop
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            //最初に選択するフォルダを指定する
            //RootFolder以下にあるフォルダである必要がある
            //fbd.SelectedPath = TEST_CASE_NAME;
            //ユーザーが新しいフォルダを作成できるようにする
            //デフォルトでTrue
            fbd.ShowNewFolderButton = true;

            //ダイアログを表示する
            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                //選択されたフォルダを表示する
                //Console.WriteLine(fbd.SelectedPath);
                setTestCase(fbd.SelectedPath);
            }

            ////OpenFileDialogクラスのインスタンスを作成
            //OpenFileDialog ofd = new OpenFileDialog();

            ////はじめのファイル名を指定する
            ////はじめに「ファイル名」で表示される文字列を指定する
            ////ofd.FileName = TEST_CASE_NAME;

            ////はじめに表示されるフォルダを指定する
            ////指定しない（空の文字列）の時は、現在のディレクトリが表示される
            ////ofd.InitialDirectory = @"C:\";
            ////[ファイルの種類]に表示される選択肢を指定する
            ////指定しないとすべてのファイルが表示される
            //ofd.Filter = @"Excelファイル\(*.xls\;*.xlsx\)|";

            ////[ファイルの種類]ではじめに
            ////「すべてのファイル」が選択されているようにする
            //ofd.FilterIndex = 1;

            ////タイトルを設定する
            //ofd.Title = "テストケースファイルを選択してください";

            ////ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
            //ofd.RestoreDirectory = true;

            ////存在しないファイルの名前が指定されたとき警告を表示する
            ////デフォルトでTrueなので指定する必要はない
            //ofd.CheckFileExists = true;

            ////存在しないパスが指定されたとき警告を表示する
            ////デフォルトでTrueなので指定する必要はない
            //ofd.CheckPathExists = true;

            ////ダイアログを表示する
            //if (ofd.ShowDialog() == DialogResult.OK)
            //{
            //    setTestCase(ofd.FileName);
            //}
        }

        /// <summary>
        /// テストケースを一覧に設定
        /// </summary>
        /// <param name="ofd"></param>
        private void setTestCase(string fullPass)
        {
            //ラベルに表示
            //labelTestCaseExcelFile.Text =  System.IO.Path.GetFileName(fullPass);
            labelTestCaseExcelFile.Text = fullPass;

            //VO情報を作成
            //List<WebTestDto> webTestList = webTestController.makeTestInfoVO(oXls, fullPass);
            List<WebTestDto> webTestList = webTestController.makeTestInfoVO(fullPass);

            //テスト一覧に表示
            checkedListBox1.Items.Clear();
            foreach (var webTest in webTestList)
            {
                checkedListBox1.Items.Add(webTest.sheetName + " : " + webTest.testNo + " : " + webTest.testName);
            }
        }

        //------------------キャプチャ

        //[System.Runtime.InteropServices.DllImport("User32.dll")]
        //private extern static bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);

        /// <summary>
        /// エビデンスファイルを設定
        /// </summary>
        public void setEvidenseFile()
        {

            //openfiledialogクラスのインスタンスを作成
            OpenFileDialog ofd = new OpenFileDialog();

            //はじめのファイル名を指定する
            //はじめに「ファイル名」で表示される文字列を指定する
            //ofd.filename = "default.html";
            //はじめに表示されるフォルダを指定する
            //指定しない（空の文字列）の時は、現在のディレクトリが表示される
            //ofd.initialdirectory = @"c:\";
            //[ファイルの種類]に表示される選択肢を指定する
            //指定しないとすべてのファイルが表示される
            ofd.Filter = @"excelファイル(*.xls;*.xlsx)|*.xls;*.xlsx|すべてのファイル(*.*)|*.*";
            //[ファイルの種類]ではじめに
            //「すべてのファイル」が選択されているようにする
            // ofd.filterindex = 2;
            //タイトルを設定する
            ofd.Title = "開くファイルを選択してください";

            //ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
            ofd.RestoreDirectory = true;

            //存在しないファイルの名前が指定されたとき警告を表示する
            //デフォルトでTrueなので指定する必要はない
            ofd.CheckFileExists = true;

            //存在しないパスが指定されたとき警告を表示する
            //デフォルトでTrueなので指定する必要はない
            ofd.CheckPathExists = true;

            //ダイアログを表示する
            if (ofd.ShowDialog() == DialogResult.OK)
            {

                //excelファイルを設定する
                excelController.setExcelFilePass(ofd.FileName);

                //ファイルをopenする
                excelController.openExcelFile();
            }
        }
        #endregion

        /// <summary>
        /// エビデンスDir Open
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonEvidenceDir_Click(object sender, EventArgs e)
        {
            // カレントディレクトリを取得する
            string stCurrentDir = System.IO.Directory.GetCurrentDirectory();

            string dirPass = stCurrentDir + "\\" + "Ebidence" + "\\";

            //TODO Dir存在チェックを入れる

            //オプションに"/e"を指定して開く
            System.Diagnostics.Process.Start(
                "EXPLORER.EXE", @"/e, " + dirPass);
        }

        /// <summary>
        /// ログウインドウ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLog_Click(object sender, EventArgs e)
        {
			//画面項目情報を出力
			//formLog.setLogStrList(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " Screen Title : " + webTestController.webBrowser.Document.Title);
			//foreach (HtmlElement element in webTestController.webBrowser.Document.All)
			//{
			//	formLog.setLogStrList(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
			//		+ " Item Info "
			//		+ " ID : " + element.Id
			//		+ " Name : " + element.Name
			//		+ " Value : " + element.InnerText);
			//}

			formLog.Visible = true;
        }

        //全選択
        private void buttonAllSel_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {       //チェックを入れる
                checkedListBox1.SetItemChecked(i, true);
            }
        }
        //全解除
        private void buttonAllOff_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {       //チェックを入れる
                checkedListBox1.SetItemChecked(i, false);
            }
        }

        /// <summary>
        /// 中止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAbort_Click(object sender, EventArgs e)
        {
            webTestController.doAbort();
        }
    }
}
