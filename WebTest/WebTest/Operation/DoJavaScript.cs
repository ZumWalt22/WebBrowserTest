using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    /// <summary>
    /// JavaScriptを実行する
    /// </summary>
    class DoJavaScript : Operation
    {
        //基底クラスのコンストラクタを「: base(webBrowser)」で呼び出すためのコンストラクタ
        public DoJavaScript(WebBrowserReceiver webBrowserReceiver)
            : base(webBrowserReceiver, true)
        {
        }

        /// <summary>
        /// JavaScriptを実行する
        /// </summary>
        /// <param name="inValue">JavaScript文字列</param>
        protected override object doneImpl(object inValue)
        {
            string javaScriptString = (string)inValue;

            //カンマで分割
            List<string> strList = javaScriptString.Split(',').ToList<string>();

            if (strList.Count == 1)
            {
                //メソッド取得
                string methodeName = strList[0];

                //methode要素をリストから削除
                strList.RemoveAt(0);

                //引数ありJavaScriptを実施
                webBrowserReceiver.webBrowser.Document.InvokeScript(methodeName);

                //ログ出力
                setLogInfoStr("Do Java Script", "Methode Name", methodeName);
            }
            else if (strList.Count > 1)
            {
                //メソッド取得
                string methodeName = strList[0];

                //methode要素をリストから削除
                strList.RemoveAt(0);

                //引数ありJavaScriptを実施
                webBrowserReceiver.webBrowser.Document.InvokeScript(methodeName, strList.ToArray());

                //ログ出力
                setLogInfoStr("Do Java Script", "Methode Name", methodeName, strList.ToString());
            }

            //ダミーの値を戻す
            return null;
        }
    }

}
