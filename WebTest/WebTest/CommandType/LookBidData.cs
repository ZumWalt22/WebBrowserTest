using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    /// <summary>
    /// 画面情報確認　コマンドクラス
    /// </summary>
    class LookBid : CommandType
    {
        //BidData bidData;

        //public LookBid(BidData BidData)
        //{
        //    this.bidData = BidData;
        //}

        /// <summary>
        /// 画面情報確認処理
        /// </summary>
        /// <returns>値を返却するための構造体</returns>
        protected override void executeDone(object obj)
        {

            ////ページを再読み込み
            //webBrowserReceiver.reLoad(1);

            ////金額を取得する
            //string strBid = webBrowserReceiver.getStateBySource("<strong property=\"auction:Price\">(.+) 円</strong>");
 
            ////金額のカンマを除去
            //strBid = strBid.Replace(",", "");

            //bidData.bid = int.Parse(strBid);

            ////最高額入札者名を取得する
            //bidData.bidUser = webBrowserReceiver.getStateBySource("<th >最高額入札者</th>\n<td>：&nbsp;</td>\n<td>\\n(.+?)\n</td>");

            ////入札終了フラグを取得する
            //string strEndTime = webBrowserReceiver.getStateBySource("<th >残り時間</th>\n<td>：&nbsp;</td>\n<td><strong>(.+?) </strong>");

            //if (strEndTime.Equals("終了"))
            //{
            //    bidData.bidEnd = true;
            //}
            //else
            //{
            //    bidData.bidEnd = false;
            //}
        }
    }
}