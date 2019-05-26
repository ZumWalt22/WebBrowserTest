using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    /// <summary>
    /// WebTest　コマンドクラス
    /// </summary>
    class WebTestDo : CommandType
    {
        /// <summary>
        /// webTest実行リスト
        /// </summary>
        List<WebTestDto> webTestList = new List<WebTestDto>();

        /// <summary>
        /// コンストラクタ(クラス情報コピー)
        /// </summary>
        /// <param name="auctionID">QUIQテスト項目情報</param>
        public WebTestDo(List<WebTestDto> webTestList)
        {
            this.webTestList = webTestList;
        }

        /// <summary>
        /// 実装処理
        /// </summary>
        /// <returns>常にNull</returns>
        protected override void executeDone(object obj)
        {
            //中止フラグをリセット
            abortFlg = false;

            foreach (WebTestDto webTest in webTestList)
            {

                // TODO URLのポストバック設定機能がないためないためコメントにしておく

                //URLを設定（設定ありの場合のみ実行）
                if (!string.IsNullOrEmpty(webTest.url))
                {
                    webBrowserReceiver.setUrl(webTest.url, 1);
                }

                //TODO ここに拡張設定を記述

                //画面に値を設定（設定ありの場合のみ実行）
                if (webTest.formElementList != null && webTest.formElementList.Count > 0)
                {
                    webBrowserReceiver.setValue(webTest.formElementList);
                }

                //エビデンスを取得する（実行まえ）（マニュアル実行時は実行しない）
                if (webTest.testNo > 0)
                {
                    webTest.ebidenceSetMode = 0;    //モード：通常
                    webBrowserReceiver.getWindowCapt(webTest);
                    webBrowserReceiver.getHtmlCapt(webTest);
                    webBrowserReceiver.getDBCapt(webTest); //データ取得
                }

                //ボタンを押す（設定ありの場合のみ実行）
                if (!string.IsNullOrEmpty(webTest.clickButtonName))
                {
                    // 停止ボタン押下時、処理を中止
                    if (abortFlg)
                    {
                        return;
                    }


                    if (webTest.submitType.Equals(0))
                    {   //Submitボタン
                        webBrowserReceiver.submit(0, webTest.postBackCnt);
                    }else if (webTest.submitType.Equals(1))
                    {   //通常ボタン
                        webBrowserReceiver.click(webTest.clickButtonName, webTest.postBackCnt);
                    }
                    else if (webTest.submitType.Equals(2))
                    {   //JavaScriptボタン
                        webBrowserReceiver.javaScript(webTest.clickButtonName, webTest.postBackCnt);
                    }
                }
                
                //結果情報正誤チェック
                if (webTest.formElementList != null && webTest.formElementList.Count > 0)
                {
                    webBrowserReceiver.checkValue(webTest.formElementList);
                }

                //エビデンスを取得する（実行後）
                if (webTest.testNo > 0)
                {
                    webTest.ebidenceSetMode = 1;    //モード：処理後
                    webBrowserReceiver.getWindowCapt(webTest);
                    webBrowserReceiver.getHtmlCapt(webTest);
                    webBrowserReceiver.getDBCapt(webTest); //データ取得
                }
            }

            //結果比較（マニュアル実行時は実行しない）
            if (!(webTestList.Count == 1 && webTestList[0].testNo == 0))
            {
                //結果レポート表示
                MessageBox.Show(getTestResoult(),
                                "Test結果照合",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// テスト結果情報を取得する
        /// </summary>
        public string getTestResoult()
        {
            StringBuilder testLogStr = new StringBuilder();

            int totalCnt = 0;
            int tureCnt = 0;

            foreach (WebTestDto webTestDto in webTestList)
            {
                testLogStr.Append(webTestDto.testNo).Append(" : ");
                testLogStr.AppendLine(webTestDto.testName);

                foreach (FormElement formElement in webTestDto.formElementList)
                {
                    if (1.Equals(formElement.inOutFlag))
                    {
                        totalCnt++;

                        testLogStr.Append(formElement.id).Append(" : ");
                        testLogStr.AppendLine(formElement.valCheck.ToString());

                        if (formElement.valCheck)
                        {
                            tureCnt++;
                        }
                    }
                }
            }

            //CSVファイル出力

            // カレントディレクトリを取得する
            string stCurrentDir = System.IO.Directory.GetCurrentDirectory();

            //Shift JISで書き込む
            //書き込むファイルが既に存在している場合は、上書きする
            System.IO.StreamWriter sw = new System.IO.StreamWriter(
                @stCurrentDir + "\\" + "Ebidence" + "\\" + "TestResoultReport.csv",
                false,
                System.Text.Encoding.GetEncoding("shift_jis"));
            //TextBox1.Textの内容を書き込む
            sw.Write(testLogStr.ToString());
            //閉じる
            sw.Close();


            return "OkElementCount/TotalElementCount   " + tureCnt + "/" + totalCnt;
        }
    }
}
