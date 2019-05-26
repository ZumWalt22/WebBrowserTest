using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{   
    /// <summary>
    /// 実行ボタンを押す
    /// </summary>
    class Click : Operation
    {

        //基底クラスのコンストラクタを「: base(webBrowser)」で呼び出すためのコンストラクタ
        public Click(WebBrowserReceiver webBrowserReceiver)
            : base(webBrowserReceiver,true)
        {

        }

        /// <summary>
        /// ボタンを押す処理を実行
        /// </summary>
        /// <param name="inValue">対象ボタンのあるForm番号</param>
        protected override object doneImpl(object inValue)
        {
            string buttonName = (string)inValue;

            //ボタンクリックが行われたかのフラグ
            bool buttonClickFlg = false;

            if (webBrowserReceiver.webBrowser.Document.All.GetElementsByName(buttonName).Count > 0)
            {
                //click　Nameで検索
                webBrowserReceiver.webBrowser.Document.All.GetElementsByName(buttonName)[0].InvokeMember("click");

                setLogInfoStr("Click", "Name", buttonName);

                buttonClickFlg = true;
            }

            else if (webBrowserReceiver.webBrowser.Document.GetElementById(buttonName) != null)
            {
                //click IDで検索
                webBrowserReceiver.webBrowser.Document.GetElementById(buttonName).InvokeMember("click");

                setLogInfoStr("Click", "Id", buttonName);

                buttonClickFlg = true;
            }
            else
            {
                setLogWarnStr("Click", buttonName, "Value Not Exist");
            }

            //ダミーの値を戻す
            return buttonClickFlg;
        }
    }
}
