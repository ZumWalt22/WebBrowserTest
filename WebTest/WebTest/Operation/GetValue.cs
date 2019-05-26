using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    class GetValue : Operation
    {
        //基底クラスのコンストラクタを「: base(webBrowser)」で呼び出すためのコンストラクタ
        public GetValue(WebBrowserReceiver webBrowserReceiver)
            : base(webBrowserReceiver,false)
        {

        }

        /// <summary>
        /// 画面の値を取得
        /// </summary>
        /// <param name="formElementList">設定値パラメータ構造体のリスト</param>
        /// <returns>常にtrueを返却する</returns>
        protected override object doneImpl(object inValue)
        {
            List<FormElement> formElementList = (List<FormElement>)inValue;

            //設定値リストの内容を順に設定（複数項目ある場合すべての項目に値を設定するためループでまわす）
            foreach(FormElement formElement in formElementList)
            {
                //IDと値を設定 すべての項目に設定する(InOutフラグOutのもののみ入力)
                if (1.Equals(formElement.inOutFlag) && webBrowserReceiver.webBrowser.Document.GetElementById(formElement.id) != null)
                {
                    formElement.val = webBrowserReceiver.webBrowser.Document.All.GetElementsByName((string)inValue)[0].InnerText;

                    //ログ出力
                    setLogInfoStr("Get Regex Patern", "Id", formElement.id);
                }
                else
                {
                    //ログ出力
                    setLogWarnStr("Get Regex Patern", "Id Not Exist", formElement.id);
                }

            }

            //ダミーの値を戻す
            return null;
        }

    }
}
