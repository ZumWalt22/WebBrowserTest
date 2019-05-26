using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using ExcelMng;
using TaskTop;

namespace WindowsFormsApplication1
{
    /* 環境依存のためコメント

    /// <summary>
    /// DBエビデンスキャプチャ
    /// </summary>
    class DBEvidence : Operation
    {
        //ファイル名
        static string TMP_FILE_NAME = "Db.csv";

        string dataSource;
        string userId;
        string password;

        //基底クラスのコンストラクタを「: base(webBrowser)」で呼び出すためのコンストラクタ
        public DBEvidence(WebBrowserReceiver webBrowserReceiver)
            : base(webBrowserReceiver, false)
        {
            Config config = new Config();
            dataSource = config.getdefaultItem("Data Source");
            userId = config.getdefaultItem("User ID");
            password = config.getdefaultItem("Password");
        }

        /// <summary>
        /// エビデンス取得処理を実行
        /// </summary>
        /// <param name="inValue">Excelコントローラークラス</param>
        protected override object doneImpl(object inValue)
        {
            IfDbMng odpDbMng = new OdpDbMng(dataSource, userId, password);

            //SQLを取得
            WebTestDto webTestDto = (WebTestDto)inValue;

            StringBuilder csvStr = new StringBuilder();

            string sqlListStr = webTestDto.captureSQL;

            string[] sqlList = sqlListStr.Split(new char[] { '\r', '\n' });

            foreach (string sql in sqlList)
            {
                //DB情報を取得
                TableInfoVO tableInfoVO = odpDbMng.SelectData(sql);

                //取得情報をcsvに反映

                //SQL定義
                csvStr.Append(sql).Append(",\r\n");

                //カラム名
                foreach (string colName in tableInfoVO.columnNameList)
                {　
                    csvStr.Append(colName).Append(",");
                }

                csvStr.Append(",\r\n");

                //値リスト
                foreach (List<string> valList in tableInfoVO.rowValList)
                {
                    foreach (string val in valList)
                    {
                        csvStr.Append(val).Append(",");
                    }
                    csvStr.Append(",\r\n");
                }

                csvStr.Append(",\r\n");

                //ログ出力
                setLogInfoStr("Capture", "DB", sql);
            }

            //CSVファイル出力

            // カレントディレクトリを取得する
            string stCurrentDir = System.IO.Directory.GetCurrentDirectory();

            //Shift JISで書き込む
            //書き込むファイルが既に存在している場合は、上書きする
            System.IO.StreamWriter sw = new System.IO.StreamWriter(
                @stCurrentDir + "\\" + "Ebidence" + "\\" + webTestDto.testNo + "_" + webTestDto.ebidenceSetMode + TMP_FILE_NAME,
                false,
                System.Text.Encoding.GetEncoding("shift_jis"));
            //TextBox1.Textの内容を書き込む
            sw.Write(csvStr.ToString());
            //閉じる
            sw.Close();

            //ダミーの値を戻す
            return null;
        }
    }
*/

}
