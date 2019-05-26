using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    class SetValue : Operation
    {
        //値設定種別　0:ID　1:Name
        static string VALUE_SET_MODE = "0";

        //基底クラスのコンストラクタを「: base(webBrowser)」で呼び出すためのコンストラクタ
        public SetValue(WebBrowserReceiver webBrowserReceiver)
            : base(webBrowserReceiver,false)
        {

        }

        /// <summary>
        /// 画面に値を設定
        /// </summary>
        /// <param name="formElementList">設定値パラメータ構造体のリスト</param>
        /// <returns>常にtrueを返却する</returns>
        protected override object doneImpl(object inValue)
        {
            List<FormElement> formElementList = (List<FormElement>)inValue;


            //設定値リストの内容を順に設定（複数項目ある場合すべての項目に値を設定するためループでまわす）
            foreach(FormElement formElement in formElementList)
            {
                //値が設定されていない場合は処理をスキップする
                if (string.IsNullOrEmpty(formElement.val))
                {
                    continue;
                }
                
                //IDと値を設定 すべての項目に設定する(InOutフラグInのもののみ入力)
                if (0.Equals(formElement.inOutFlag) && webBrowserReceiver.webBrowser.Document.GetElementById(formElement.id) != null)
                {
                    if("0".Equals(VALUE_SET_MODE)){

                        //---------------------------Id指定

                        //（GetElementsByIdの場合は一意に決まるので、ループの必要なし）
                        switch (formElement.elementType)
                        {
                            case FormElement.ElementType.TextBox :
                                //テキストボックス
                                webBrowserReceiver.webBrowser.Document.GetElementById(formElement.id).InnerText = formElement.val.ToString();
                                setLogInfoStr("Set Value TextBox", "Id", formElement.id, formElement.val.ToString());
                                break;
                            case FormElement.ElementType.DropDownList:
                                //ドロップダウンリスト設定

                                //valueで指定
                                webBrowserReceiver.webBrowser.Document.GetElementById(formElement.id).SetAttribute("value", formElement.val);
                                setLogInfoStr("Set Value DropDownList", "Id", formElement.id, formElement.val.ToString());

                                //selectedindexで指定　（先頭0から計算） 
                                //webBrowserReceiver.webBrowser.Document.All.GetElementsByName(formElement.id)[cntSub].SetAttribute("selectedindex", formElement.val);
                                break;
                            case FormElement.ElementType.RadioButton:
                                //ラジオボタン
                                webBrowserReceiver.webBrowser.Document.GetElementById(formElement.id).SetAttribute("CHECKED", formElement.val.ToString());
                                setLogInfoStr("Set Value RadioButton", "Id", formElement.id, formElement.val.ToString());
                                break;
                            default:
                                break;
                        }

                    }
                    else if ("1".Equals(VALUE_SET_MODE))
                    {

                        //---------------------------Name指定

                        //GetElementsByName（Nameで設定、複数ある可能性ありのため、ループ処理）
                        for (int cntSub = 0; cntSub < webBrowserReceiver.webBrowser.Document.All.GetElementsByName(formElement.id).Count; cntSub++)
                        {
                            //指定Indexで値を設定する
                            if (formElement.index.Equals(cntSub))
                            {
                                switch (formElement.elementType)
                                {
                                    case FormElement.ElementType.TextBox:
                                        //テキストボックス
                                        webBrowserReceiver.webBrowser.Document.All.GetElementsByName(formElement.id)[cntSub].InnerText = formElement.val;
                                        setLogInfoStr("Set Value TextBox", "Name", formElement.id, formElement.val.ToString());
                                        break;
                                    case FormElement.ElementType.DropDownList:
                                        //ドロップダウンリスト設定

                                        //valueで指定
                                        webBrowserReceiver.webBrowser.Document.All.GetElementsByName(formElement.id)[cntSub].SetAttribute("value", formElement.val);

                                        //selectedindexで指定　（先頭0から計算） 
                                        //webBrowserReceiver.webBrowser.Document.All.GetElementsByName(formElement.id)[cntSub].SetAttribute("selectedindex", formElement.val);
                                        setLogInfoStr("Set Value DropDownList", "Name", formElement.id, formElement.val.ToString());
                                        break;
                                    case FormElement.ElementType.RadioButton:
                                        //ラジオボタン
                                        webBrowserReceiver.webBrowser.Document.All.GetElementsByName(formElement.id)[cntSub].SetAttribute("CHECKED", formElement.val.ToString());
                                        setLogInfoStr("Set Value RadioButton", "Id", formElement.id, formElement.val.ToString());
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                    else
                    {

                    }
                }
            }

            //ダミーの値を戻す
            return null;
        }

    }
}
