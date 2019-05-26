using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{   
    /// <summary>
    /// 実行ボタンを押す
    /// </summary>
    class Submit : Operation
    {

        //基底クラスのコンストラクタを「: base(webBrowser)」で呼び出すためのコンストラクタ
        public Submit(WebBrowserReceiver webBrowserReceiver)
            : base(webBrowserReceiver,true)
        {

        }

        /// <summary>
        /// ボタンを押す処理を実行
        /// </summary>
        /// <param name="inValue">対象ボタンのあるForm番号</param>
        protected override object doneImpl(object inValue)
        {
            int index = (int)inValue;

            //submit
            if (webBrowserReceiver.webBrowser.Document.Forms.Count > 0)
            {
                webBrowserReceiver.webBrowser.Document.Forms[index].InvokeMember("submit");

                //ログ出力
                setLogInfoStr("Submit", "Index", index.ToString());

            } else
            {
                //FormのIndexがない場合

                //ログ出力
                setLogWarnStr("Submit", "Index", "Form Elementが存在しない");
            }

            //ダミーの値を戻す
            return null;
        }
    }
}
