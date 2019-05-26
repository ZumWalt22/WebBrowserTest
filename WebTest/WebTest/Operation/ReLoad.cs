using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    /// <summary>
    /// ページの際読み込みを行う
    /// </summary>
    class ReLoad : Operation
    {

        //基底クラスのコンストラクタを「: base(webBrowser)」で呼び出すためのコンストラクタ
        public ReLoad(WebBrowserReceiver webBrowserReceiver)
            : base(webBrowserReceiver, true)
        {
        }

        /// <summary>
        /// ページを再読み込み
        /// </summary>
        /// <param name="inValue">表示対照のURL</param>
        protected override object doneImpl(object inValue)
        {
            webBrowserReceiver.webBrowser.Refresh();

            //ログ出力
            setLogInfoStr("ReLoad", "", "");

            //ダミーの値を戻す
            return null;
        }
    }
}
