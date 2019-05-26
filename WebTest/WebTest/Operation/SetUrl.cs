using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    /// <summary>
    /// URLのページに移動する
    /// </summary>
    class SetUrl : Operation
    {

        //基底クラスのコンストラクタを「: base(webBrowser)」で呼び出すためのコンストラクタ
        public SetUrl(WebBrowserReceiver webBrowserReceiver) : base(webBrowserReceiver,true)
        {
        }

        /// <summary>
        /// URLのHpに移動
        /// </summary>
        /// <param name="inValue">表示対照のURL</param>
        protected override object doneImpl(object inValue)
        {
            webBrowserReceiver.webBrowser.Navigate((string)inValue);

            //ダミーの値を戻す
            return null;
        }
    }

}
