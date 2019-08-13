using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using ExcelMng;
using System.Reflection;

namespace WindowsFormsApplication1
{
    class WebTestController
    {
        //親フォームリンク
        Form parentForm;

        //webBrowserインスタンス
        public WebBrowser webBrowser;

        //ExcelControllerインスタンス
        public ExcelController excelController;

        //Log Form
        public FormLog formLog;

        //WebBrowserスレッド実行クラス
        WebBrowserReceiver webBrowserReceiver;

        /// <summary>
        /// テスト情報
        /// </summary>
        List<WebTestDto> webTestList = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="parentForm"></param>
        /// <param name="webBrowser"></param>
        public WebTestController(Form parentForm, WebBrowser webBrowser, ExcelController excelController, FormLog formLog)
        {
            this.parentForm = parentForm;
            this.webBrowser = webBrowser;
            this.excelController = excelController;
            this.formLog = formLog;
        }

        /// <summary>
        /// WebBrowserを設定
        /// </summary>
        /// <param name="parentForm"></param>
        /// <param name="webBrowser"></param>
        public void setWebBrowser(WebBrowser webBrowser)
        {
            this.webBrowser = webBrowser;
        }

        /// <summary>
        /// テスト情報を取得VOを作成する
        /// </summary>
        /// <param name="oXls"></param>
        /// <param name="testFilePass"></param>
        //public List<WebTestDto> makeTestInfoVO(Microsoft.Office.Interop.Excel.Application oXls, string testFilePass)
        public List<WebTestDto> makeTestInfoVO(string testFilePass)
        {
            //テスト情報
            WebTestDto webTest;

            //テストNo
            string no = "0";

            //テスト名称
            string testName = "";

            // URL
            string url = "";

            // SubmitボタンName
            string buttonName = "";

            // PostBack回数
            int postBackCnt = 0;

            //Submit Type
            int submitType = 0;

            Dictionary<string, int> idDictionary = new Dictionary<string, int>();

            //画面設定項目 マスタ
            List<FormElement> webTestDictionaryList = null;

            //画面設定項目 設定
            List<FormElement> webTestDictionaryListSet = null;

            //テスト情報クリア
            webTestList = new List<WebTestDto>();

            //Excelファイルに設定されているテスト項目を読み取り
            //ExcelController excelController = new ExcelController(oXls);
            ExcelController excelController = new ExcelController();
            excelController.setExcelFilePass(testFilePass);

            //ファイルをOpenする
            excelController.openExcelFile();

            //シートリスト情報を取得
            List<String> sheetList = excelController.getSheetList();

            try
            {
                //シートごとループ
                foreach(String sheetName in sheetList) {

                    string captureSQL = "";

                    // URL取得
                    url = excelController.getValue(sheetName, 3, 2);

                    // DBエビデンス取得SQL
                    captureSQL = excelController.getValue(sheetName, 3, 3);

                    //設定カラム名称を取得
                    List<string> colNameList = new List<string>();
                    int colSetStart = 10;
                    int colEbidenceStart = 10;
                    int col = 10;
                    int inOutFlag = 0; // IN
                    int index = 0;

                    //設定カラム名称を取得、マスタ情報を作成
                    webTestDictionaryList = new List<FormElement>();
                    for (col = colSetStart; !string.IsNullOrWhiteSpace(excelController.getValue(sheetName, 1, col)); col++)
                    {
                        string elementVal = excelController.getValue(sheetName, 1, col);
                        string elementType = excelController.getValue(sheetName, 2, col);

                        //設定情報開始位置
                        if ("TestResoult".Equals(elementVal))
                        {
                            colEbidenceStart = col;
                            inOutFlag = 1; //OUT
                        }
                        else
                        {
                            switch (elementType)
                            {
                                case "TextBox":
                                    webTestDictionaryList.Add(new FormElement(elementVal, "", FormElement.ElementType.TextBox, inOutFlag, index));
                                    break;
                                case "DropDownList":
                                    webTestDictionaryList.Add(new FormElement(elementVal, "", FormElement.ElementType.DropDownList, inOutFlag, index));
                                    break;
                                case "RadioButton":
                                    webTestDictionaryList.Add(new FormElement(elementVal, "", FormElement.ElementType.RadioButton,inOutFlag, index));//ラジオボタン　チェック「1」チェック外す「0」
                                    break;
                                case "CheckBox":
                                    webTestDictionaryList.Add(new FormElement(elementVal, "", FormElement.ElementType.CheckBox, inOutFlag, index));//ラジオボタン　チェック「1」チェック外す「0」RadioButtonと動作は同じ
                                    break;
                                case "Label":
                                    webTestDictionaryList.Add(new FormElement(elementVal, "", FormElement.ElementType.Label, inOutFlag, index));
                                    break;
                                case "HiddenValue":
                                    webTestDictionaryList.Add(new FormElement(elementVal, "", FormElement.ElementType.HiddenValue, inOutFlag, index));
                                    break;
                                default:
                                    //TODO エラーログ出力
                                    break;
                            }
                        }

                        colNameList.Add(excelController.getValue(sheetName, 1, col));
                    }

                    // 設定値情報を取得(テスト番号が設定されている行を実行)
                    idDictionary.Clear();
                    for (int cnt = 3; !string.IsNullOrWhiteSpace(excelController.getValue(sheetName, cnt, 4)); cnt++)
                    {
                        // テストNO取得
                        no = excelController.getValue(sheetName,cnt, 4);

                        // テスト名称取得
                        testName = excelController.getValue(sheetName, cnt, 5);

                        // SubmitボタンID取得
                        buttonName = excelController.getValue(sheetName, cnt, 6);

                        //ポストバック回数を取得
                        postBackCnt = int.Parse(excelController.getValue(sheetName, cnt, 7));

                        //Submitタイプを取得
                        submitType = int.Parse(excelController.getValue(sheetName, cnt, 8));

                        // 設定項目情報取得
                        int cntSub = colSetStart;
                        webTestDictionaryListSet = new List<FormElement>();
                        idDictionary.Clear();
                        foreach (FormElement formElement in webTestDictionaryList)
                        {
                            //エビデンス取得開始行のマーカー行はスキップする
                            if (colEbidenceStart.Equals(cntSub)){
                                cntSub ++;
                            }

                            //Index の値を設定する
                            index = 0;
                            if (idDictionary.ContainsKey(formElement.id))
                            {
                                index = idDictionary[formElement.id];
                                idDictionary.Remove(formElement.id);
                            }

                            webTestDictionaryListSet.Add(new FormElement(
                                formElement.id,
                                excelController.getValue(sheetName, cnt, cntSub), 
                                formElement.elementType,
                                formElement.inOutFlag,
                                index)
                            );

                            //加算
                            index++;
                            idDictionary.Add(formElement.id, index);

                            cntSub ++;
                        }

                        // テストデータをコマンドクラスに設定
                        webTest = new WebTestDto(
                            sheetName, 
                            testName, 
                            int.Parse(no), 
                            url, 
                            captureSQL, 
                            webTestDictionaryListSet, 
                            buttonName, 
                            postBackCnt, 
                            submitType);

                        webTestList.Add(webTest);
                    }
                } 
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                //ファイルをCloseする
                excelController.closeExcelFile();
            }

            //作成情報を返す(グローバルにも情報を保管)
            return webTestList;
        }

        /// <summary>
        /// テスト実行
        /// </summary>
        /// <param name="webBrowser">Formで設定されているwebBrowser</param>
        /// <param name="auctionBidNo">一覧で選択されたテスト</param>
        public void testDone(List<int> testNoList)
        {
            //個別実行史時、ウインドウが設定されていない場合チェック
            if (webBrowser == null)
            {
                //TODO Webウインドウがないエラーを表示

                return;
            }

            //テスト情報を取得
            List<WebTestDto> webTestSelectList = new List<WebTestDto>();

            //テスト対象リストを生成
            foreach (var testNo in testNoList)
            {
                webTestSelectList.Add(webTestList[testNo]);
            }

            WebTestDo WebTestDo = new WebTestDo(webTestSelectList);

            //テストを実施（スレッド実行）
            ThreadPool.QueueUserWorkItem(new WaitCallback(testDoneThread), WebTestDo);
        }

        /// <summary>
        /// テストマニュアル実行
        /// </summary>
        /// <param name="webBrowser">Formで設定されているwebBrowser</param>
        /// <param name="auctionBidNo">一覧で選択されたテスト</param>
        /// <param name="doneType">実行樹別（0:URL 1:画面設定　2:サブミット 3:エビデンス）</param>
        public void testDoneManual(int testNo,int doneType)
        {
            if (testNo < 0)
            {
                return;
            }

            if (webBrowser == null)
            {
                return;
            }

            //テスト情報を取得
            WebTestDto webTestDto = webTestList[testNo];

            //マニュアル実行用のVOクラスを生成
            WebTestDto wtManual = new WebTestDto(webTestDto);

            switch (doneType)
            {
                case 0:
                    //wtManual.url = "";                // URL情報を削除
                    wtManual.formElementList = new List<FormElement>();    // 画面設定情報を削除
					wtManual.clickButtonName = "";      // Submitボタン情報を削除
                    wtManual.testNo = 0;                //エビデンス取得削除
                    break;
                case 1:
                    wtManual.url = "";                  // URL情報を削除
                    //wtManual.formElementList = new List<FormElement>();    // 画面設定情報を削除
                    wtManual.clickButtonName = "";      // Submitボタン情報を削除
                    wtManual.testNo = 0;                //エビデンス取得削除
                    break;
                case 2:
                    wtManual.url = "";                  // URL情報を削除
                    wtManual.formElementList = new List<FormElement>();     // 画面設定情報を削除
                    //wtManual.clickButtonName = "";    // Submitボタン情報を削除
                    wtManual.testNo = 0;                //エビデンス取得削除
                    break;
                case 3:
                    wtManual.url = "";                // URL情報を削除
                    wtManual.formElementList = new List<FormElement>();     // 画面設定情報を削除
					wtManual.clickButtonName = "";    // Submitボタン情報を削除
                    //wtManual.testNo = 0;            //エビデンス取得削除
                    break;
                default:
                    break;
            }

            //テスト情報を取得
            List<WebTestDto> webTestSelectList = new List<WebTestDto>();
            webTestSelectList.Add(wtManual);

            WebTestDo WebTestDo = new WebTestDo(webTestSelectList);

            //テストを実施（スレッド実行）
            ThreadPool.QueueUserWorkItem(new WaitCallback(testDoneThread), WebTestDo);
        }

        /// <summary>
        /// テスト情報取得
        /// </summary>
        /// <param name="webBrowser">Formで設定されているwebBrowser</param>
        /// <param name="auctionBidNo">一覧で選択されたテスト</param>
        public WebTestDto getWebTestInfo(int testNo)
        {
            //未選択の場合は無視
            if (testNo < 0)
            {
                return webTestList[0];
            }

            //テスト情報を取得
            return webTestList[testNo];
        }

        /// <summary>
        /// テスト実行用スレッド
        /// </summary>
        public void testDoneThread(object obj)
        {
            //テスト情報を取得する
            WebTestDo webTestDo = (WebTestDo)obj;

            webBrowserReceiver = new WebBrowserReceiver(parentForm, webBrowser, excelController, formLog);

            //値を設定
            webBrowserReceiver.done(webTestDo);
        }

        /// <summary>
        /// 処理を中止する
        /// </summary>
        public void doAbort()
        {
            webBrowserReceiver.doAbort();
        }
    }
}