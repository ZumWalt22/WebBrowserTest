using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using ExcelMng;

namespace WindowsFormsApplication1
{   
    /// <summary>
    /// Htmlエビデンス取得
    /// </summary>
    class CaptHtml : Operation
    {
        //Htmlファイル名
        static string TMP_FILE_NAME = "Html.txt";

        //基底クラスのコンストラクタを「: base(webBrowser)」で呼び出すためのコンストラクタ
        public CaptHtml(WebBrowserReceiver webBrowserReceiver)
            : base(webBrowserReceiver, false)
        {

        }

        /// <summary>
        /// エビデンス取得処理を実行
        /// </summary>
        /// <param name="inValue">Excelコントローラークラス</param>
        protected override object doneImpl(object inValue)
        {
            WebTestDto webTestDto = (WebTestDto)inValue;

            //表示中のHtmlソースを取得する
            string htmlStr = webBrowserReceiver.webBrowser.DocumentText;

            // カレントディレクトリを取得する
            string stCurrentDir = System.IO.Directory.GetCurrentDirectory();

            //Shift JISで書き込む
            //書き込むファイルが既に存在している場合は、上書きする
            System.IO.StreamWriter sw = new System.IO.StreamWriter(
                @stCurrentDir + "\\" + "Ebidence" + "\\" + webTestDto.testNo + "_" + webTestDto.ebidenceSetMode + TMP_FILE_NAME,
                false,
                System.Text.Encoding.GetEncoding("shift_jis"));
            //TextBox1.Textの内容を書き込む
            sw.Write(htmlStr);
            //閉じる
            sw.Close();

            //ログ出力
            setLogInfoStr("Capture", "HTML", "");

            //ダミーの値を戻す
            return null;
        }
    }
}
