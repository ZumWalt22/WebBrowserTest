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
            string postBackCntStr = "";
            int postBackCnt = 0;

            //Submit Type
            string submitTypeStr = "";
            int submitType = 0;

            bool isNumber = false;

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

            //エラーメッセージ文字列
            string errorMsgStr = "";

            try
            {
                //シートごとループ
                foreach(String sheetName in sheetList) {

                    string captureSQL = "";

                    // URL取得
                    url = excelController.getValue(sheetName, CommonConst.CSV_URL_COL, CommonConst.CSV_URL_ROW);
                    errorMsgStr += checkAndSetCsvErrorMsgStrValNotSet(url,"テスト対象URL", sheetName, CommonConst.CSV_URL_COL, CommonConst.CSV_URL_ROW);

                    // DBエビデンス取得SQL
                    captureSQL = excelController.getValue(sheetName, 3, 3);

                    //設定カラム名称を取得
                    List<string> colNameList = new List<string>();
                    int rowSetStart = 10;
                    int rowEbidenceStart = 10;
                    int row = 10;
                    int inOutFlag = 0; // IN
                    int index = 0;

                    //設定カラム名称を取得、マスタ情報を作成
                    webTestDictionaryList = new List<FormElement>();
                    for (row = rowSetStart; !string.IsNullOrWhiteSpace(excelController.getValue(sheetName, 1, row)); row++)
                    {
                        string elementVal = excelController.getValue(sheetName, CommonConst.TESTCASE_ELEMENT_VAL_COL, row);
                        string elementType = excelController.getValue(sheetName, CommonConst.TESTCASE_ELEMENT_TYPE_COL, row);

                        errorMsgStr += checkAndSetCsvErrorMsgStrValNotSet(elementVal, "項目値", sheetName, CommonConst.TESTCASE_ELEMENT_VAL_COL, row);
                        errorMsgStr += checkAndSetCsvErrorMsgStrValNotSet(elementType, "項目種別", sheetName, CommonConst.TESTCASE_ELEMENT_TYPE_COL, row);


                        //設定情報開始位置
                        if ("TestResoult".Equals(elementVal))
                        {
                            rowEbidenceStart = row;
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
                                    errorMsgStr += setCsvErrorMsgStr("指定外の項目タイプです", sheetName, CommonConst.TESTCASE_ELEMENT_TYPE_COL,row);
                                    break;
                            }
                        }

                        colNameList.Add(excelController.getValue(sheetName, 1, row));
                    }

                    // 設定値情報を取得(テスト番号が設定されている行を実行)
                    idDictionary.Clear();
                    for (int col = 3; !string.IsNullOrWhiteSpace(excelController.getValue(sheetName, col, 4)); col++)
                    {
                        int valNum = -1;

                        // テストNO取得
                        no = excelController.getValue(sheetName,col, CommonConst.TESTCASE_TEST_NO_ROW);
                        errorMsgStr += checkAndSetCsvErrorMsgStrValNotSet(no, "テストNO", sheetName, col, CommonConst.TESTCASE_TEST_NO_ROW);

                        // テスト名称取得
                        testName = excelController.getValue(sheetName, col, CommonConst.TESTCASE_TEST_NAME_ROW);
                        errorMsgStr += checkAndSetCsvErrorMsgStrValNotSet(no, "テスト名称", sheetName, col, CommonConst.TESTCASE_TEST_NAME_ROW);

                        // SubmitボタンID取得
                        buttonName = excelController.getValue(sheetName, col, CommonConst.TESTCASE_SUBMIT_BUTTON_ID_ROW);
                        errorMsgStr += checkAndSetCsvErrorMsgStrValNotSet(no, "SubmitボタンID", sheetName, col, CommonConst.TESTCASE_SUBMIT_BUTTON_ID_ROW);

                        //ポストバック回数を取得
                        postBackCntStr = excelController.getValue(sheetName, col, CommonConst.TESTCASE_POSTBACK_COUNT_ROW);
                        errorMsgStr += checkAndSetCsvErrorMsgStrValNotSet(postBackCntStr, "ポストバック回数", sheetName, col, CommonConst.TESTCASE_POSTBACK_COUNT_ROW);
                        //valueが数値かを確認
                        isNumber = int.TryParse(postBackCntStr, out postBackCnt);
                        if (!isNumber || postBackCnt < 0)
                        {
                            errorMsgStr += setCsvErrorMsgStr("ポストバック回数は、０または正の値で指定してください", sheetName, CommonConst.TESTCASE_POSTBACK_COUNT_ROW, row);
                        }

                        //Submitタイプを取得
                        submitTypeStr = excelController.getValue(sheetName, col, CommonConst.TESTCASE_SUBMIT_TYPE_ROW);
                        errorMsgStr += checkAndSetCsvErrorMsgStrValNotSet(no, "押下ボタン種別", sheetName, col, CommonConst.TESTCASE_SUBMIT_TYPE_ROW);
                        //valueが数値かを確認
                        isNumber = int.TryParse(submitTypeStr, out submitType);
                        if (!isNumber || submitType < 0 || submitType > 1)
                        {
                            errorMsgStr += setCsvErrorMsgStr("押下ボタン種別は、Submit「1」、click「0」いずれかで指定してください", sheetName, CommonConst.TESTCASE_SUBMIT_TYPE_ROW, row);
                        }

                        // 設定項目情報取得
                        int rowSub = rowSetStart;
                        webTestDictionaryListSet = new List<FormElement>();
                        idDictionary.Clear();
                        foreach (FormElement formElement in webTestDictionaryList)
                        {
                            //エビデンス取得開始行のマーカー行はスキップする
                            if (rowEbidenceStart.Equals(rowSub)){
                                rowSub ++;
                            }

                            //Index の値を設定する
                            index = 0;
                            if (idDictionary.ContainsKey(formElement.id))
                            {
                                index = idDictionary[formElement.id];
                                idDictionary.Remove(formElement.id);
                            }

                            //テスト値取得
                            string valStr = excelController.getValue(sheetName, col, rowSub);

                            //テスト値妥当性確認
                            isNumber = int.TryParse(valStr, out valNum);
                            switch (formElement.elementType)
                            {
                                case FormElement.ElementType.TextBox:
                                    break;
                                case FormElement.ElementType.DropDownList:
                                    if (!isNumber)
                                    {
                                        errorMsgStr += setCsvErrorMsgStr("Index値を数字で指定してください", sheetName, CommonConst.TESTCASE_ELEMENT_VAL_COL, row);
                                    }
                                    break;
                                case FormElement.ElementType.RadioButton:
                                    if (!isNumber || valNum < 0 || valNum > 1)
                                    {
                                        errorMsgStr += setCsvErrorMsgStr("チェック有り「1」、チェック無し「0」いずれかで指定してください", sheetName, CommonConst.TESTCASE_ELEMENT_VAL_COL, row);
                                    }
                                    break;
                                case FormElement.ElementType.CheckBox:
                                    if (!isNumber || valNum < 0 || valNum > 1)
                                    {
                                        errorMsgStr += setCsvErrorMsgStr("チェック有り「1」、チェック無し「0」いずれかで指定してください", sheetName, CommonConst.TESTCASE_ELEMENT_VAL_COL, row);
                                    }
                                    break;
                                case FormElement.ElementType.Label:
                                    break;
                                case FormElement.ElementType.HiddenValue:
                                    break;
                                default:
                                    break;
                            }

                            //テストNoごとの情報を追加
                            webTestDictionaryListSet.Add(new FormElement(
                                formElement.id,
                                valStr, 
                                formElement.elementType,
                                formElement.inOutFlag,
                                index)
                            );

                            //加算
                            index++;
                            idDictionary.Add(formElement.id, index);

                            rowSub ++;
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

            if(!string.IsNullOrEmpty(errorMsgStr))
            {
                formLog.setLogStrList(errorMsgStr);
            }

            //作成情報を返す(グローバルにも情報を保管)
            return webTestList;
        }

        /// <summary>
        /// testCase取得時エラーメッセージを組み立て
        /// </summary>
        /// <param name="errorMsgStr"></param>
        /// <param name="sheetName"></param>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public string setCsvErrorMsgStr(string errorMsgStr,string sheetName,int col,int row)
        {
            string str = string.Format("SheetName : {0} Col : {1} Row : {2} Message : {3} \r\n", sheetName, col,row, errorMsgStr);

            return str;
        }

        /// <summary>
        /// testCase取得時エラーメッセージを組み立て　値が未設定
        /// </summary>
        /// <param name="errorMsgStr"></param>
        /// <param name="sheetName"></param>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public string checkAndSetCsvErrorMsgStrValNotSet(string val, string elementName, string sheetName, int col, int row)
        {
            if (String.IsNullOrEmpty(val))
            {
                return string.Format("SheetName : {0} Col : {1} Row : {2} Message : {3} が設定されていません) \r\n", sheetName, col, row, elementName);

            }

            return "";
        }

        /// <summary>
        /// テスト実行
        /// </summary>
        /// <param name="webBrowser">Formで設定されているwebBrowser</param>
        /// <param name="auctionBidNo">一覧で選択されたテスト</param>
        public void testDone(List<int> testNoList)
        {
            //個別実行時、ウインドウが設定されていない場合チェック
            if (webBrowser == null)
            {
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

            //個別実行時、ウインドウが設定されていない場合チェック
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