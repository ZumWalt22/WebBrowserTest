using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    class CheckValue : Operation
    {
        //基底クラスのコンストラクタを「: base(webBrowser)」で呼び出すためのコンストラクタ
        public CheckValue(WebBrowserReceiver webBrowserReceiver)
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

            //設定値リストの内容を順に取得
            foreach(FormElement formElement in formElementList)
            {
                //IDと値を設定 すべての項目に設定する(InOutフラグOutのもののみ入力)
                if (1.Equals(formElement.inOutFlag) && webBrowserReceiver.webBrowser.Document.GetElementById(formElement.id) != null)
                {
                    string htmlDocument = webBrowserReceiver.webBrowser.Document.GetElementById(formElement.id).OuterHtml;
                    MatchCollection matche;

                    //value句情報を取得
                    string valStr = "";
                    switch (formElement.elementType)
                    {
                        case FormElement.ElementType.Label:

                            valStr = webBrowserReceiver.webBrowser.Document.GetElementById(formElement.id).InnerHtml;
                            break;
                        case FormElement.ElementType.CheckBox:
                        case FormElement.ElementType.RadioButton:

                            //HMTLから「CHECKED」の存在の有無を確認する
                            //TODO　複数パターンをテストする
                            matche = Regex.Matches(htmlDocument, "\\s([CHECKED|checked|checked=\"checked\"])");

                            if (matche.Count == 1)
                            {
                                valStr = "1";
                            }
                            else
                            {
                                valStr = "0";
                            }
                            break;
                        default:
                            //HMTLから「value=XX」のXX部分を切り出す
                            matche = Regex.Matches(htmlDocument, "value=\"?(.+?)\"?[\\s|>]");

                            if (matche.Count == 1)
                            {
                                valStr = matche[0].Groups[1].Value;
                            }
                            break;

                    };

                    //予測ケースと照合
                    if (formElement.val.Equals(valStr))
                    {
                        formElement.valCheck = true;
                    }
                    else
                    {
                        formElement.valCheck = false;
                    }


                    //ログ出力
                    setLogInfoStr("Check Value", "ID", formElement.id, formElement.valCheck.ToString());
                }
                else
                {
                    //ログ出力
                    setLogWarnStr("Check Value", "ID", formElement.id, formElement.valCheck.ToString());
                }
            }

            //ダミーの値を戻す
            return null;
        }
    }
}
