using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    /// <summary>
    /// WebTest　コマンドクラス
    /// </summary>
    class WebTestDto
    {
        /// <summary>
        /// ターゲットURL
        /// </summary>
        public string url = "";

        /// <summary>
        /// キャプチャSQL
        /// </summary>
        public string captureSQL = "";

        /// <summary>
        /// シート（ウインドウ）名称
        /// </summary>
        public string sheetName = "";

        /// <summary>
        /// テスト名称
        /// </summary>
        public string testName = "";

        /// <summary>
        /// テストNo
        /// </summary>
        public int testNo = 1;

        /// <summary>
        /// テスト項目リスト
        /// </summary>
        public List<FormElement> formElementList;

        /// <summary>
        /// Submitボタン名称
        /// </summary>
        public string clickButtonName = "";

        /// <summary>
        /// Submitタイプ（0:ボタン、1:JavaScript）
        /// </summary>
        public int submitType = 0;

        /// <summary>
        /// ポストバック回数
        /// </summary>
        public int postBackCnt = 0;

        /// <summary>
        /// エビデンス設定モード
        /// 0:通常　1:完了
        /// </summary>
        public int ebidenceSetMode = 0;

        /// <summary>
        /// コンストラクタ(クラス情報コピー)
        /// </summary>
        /// <param name="auctionID">QUIQテスト項目情報</param>
        public WebTestDto(WebTestDto wt)
        {
            //シート名称
            this.sheetName = wt.sheetName;

            //テスト名称
            this.testName = wt.testName;

            //テストNo
            this.testNo = wt.testNo;

            //URLを設定
            this.url = wt.url;

            //captureSQLを設定
            this.captureSQL = wt.captureSQL;

            //値を設定
            this.formElementList = wt.formElementList;

            //ボタン名称を設定
            this.clickButtonName = wt.clickButtonName;

            //ポストバック回数
            this.postBackCnt = wt.postBackCnt;

            //Submit type
            this.submitType = wt.submitType;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="auctionID">QUIQテスト項目情報</param>
        public WebTestDto(string sheetName, string testName, int testNo, string url, string captureSQL, List<FormElement> quiqTestNameList, string clickButtonName, int postBackCnt, int submitType)
        {
            //シート名称
            this.sheetName = sheetName;

            //テスト名称
            this.testName = testName;

            //テストNo
            this.testNo = testNo;

            //URLを設定
            this.url = url;

            //captureSQLを設定
            this.captureSQL = captureSQL;

            //値を設定
            this.formElementList = quiqTestNameList;

            //ボタン名称を設定
            this.clickButtonName = clickButtonName;

            //ポストバック回数
            this.postBackCnt = postBackCnt;

            //Submit type
            this.submitType = submitType;
        }
    }
}
