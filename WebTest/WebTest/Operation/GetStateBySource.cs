using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace WindowsFormsApplication1
{

    /// <summary>
    /// HTMLソースを元に値を取得する
    /// </summary>
    class GetStateBySource : Operation
    {
        //基底クラスのコンストラクタを「: base(webBrowser)」で呼び出すためのコンストラクタ
        public GetStateBySource(WebBrowserReceiver webBrowserReceiver)
            : base(webBrowserReceiver,false)
        {

        }

        /// <summary>
        /// 画面のIDに対する値を取得する
        /// 
        /// </summary>
        /// <param name="inValue">取得する値の正規表現</param>
        /// <returns>値</returns>
        protected override object doneImpl(object inValue)
        {           
            //HTMLのテキストを取得

            //文字コード変換(eucをsjisに)
            Encoding enceuc = Encoding.GetEncoding("euc-jp");
            Encoding encsjs = Encoding.GetEncoding("shift_jis");

            byte[] beuc = new byte[webBrowserReceiver.webBrowser.DocumentStream.Length];
            webBrowserReceiver.webBrowser.DocumentStream.Read(beuc, 0, (int)webBrowserReceiver.webBrowser.DocumentStream.Length);

            byte[] bsjs = Encoding.Convert(enceuc, encsjs, beuc);

            string htmlText = encsjs.GetString(bsjs);

            //正規表現
            string regexPatern = (string)inValue;

            //Regexオブジェクトを作成
            System.Text.RegularExpressions.Regex r = new Regex(regexPatern,RegexOptions.IgnoreCase);

            //正規表現と一致する対象を1つ検索
            Match m = r.Match(htmlText);

            string matchStr = "";
            if (m.Groups.Count > 0)
            {
                matchStr = m.Groups[1].Value;

                //ログ出力
                setLogInfoStr("Get Regex Patern", "Regex Patern", regexPatern);
            }
            else
            {
                //ログ出力
                setLogWarnStr("Get Regex Patern", "Regex Patern Not Exist", regexPatern);
            }

            //正規表現に一致した値を返却
            return matchStr;
        }
    }
}
