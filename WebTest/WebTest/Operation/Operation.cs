using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace WindowsFormsApplication1
{
    /// <summary>
    /// 画面更新処理の単体処理の基底クラス
    /// 非同期処理を同期処理として扱うためのクラス
    /// </summary>
    abstract class Operation
    {
       /// <summary>
       /// 
       /// </summary>
       protected WebBrowserReceiver webBrowserReceiver;

       ///更新処理系で使用する関数のデリゲートを定義
       ///TODO スレッド化する関数ごとに個々に作成してもいいが、派生クラスで個々に定義する必要あり
       protected delegate object DonTheadDelegate(object inValue);

       /// <summary>
       /// デリゲートを定義
       /// </summary>
       /// <param name="inValue"></param>
       protected delegate object OperationDelegate(object inValue);

    　 /// <summary>
    　 /// デリゲートを生成
    　 /// </summary>
       protected OperationDelegate operationDelegate;

       /// <summary>
       /// 表示完了まで待たせるかのフラグ
       /// </summary>
       protected bool moveWaitFlag = false;

       /// <summary>
       /// コンストラクタ
       /// </summary>
      /// <param name="webBrowserReceiver">コントロールクラスのインスタンス</param>
       public Operation(WebBrowserReceiver webBrowserReceiver,bool moveWaitFlag)
       {
           this.webBrowserReceiver = webBrowserReceiver;

           this.moveWaitFlag = moveWaitFlag;

           operationDelegate = new OperationDelegate(doneImpl);
       }

       //処理をスレッドで実行
       public virtual object done(object inValue)
       {
            //シグナル状態をリセットする
           webBrowserReceiver.UrlMoveCompleatEvent.Reset();

           //画面推移カウント回数をリセット
           webBrowserReceiver.documentCompletedCount = 0;

           //Formスレッドの関数をInvokeで実行する
           object obj = webBrowserReceiver.parentForm.Invoke(operationDelegate, new object[] { inValue });

           ////シグナル状態になるまでスレッドをブロックする
           if (moveWaitFlag)
           {
               webBrowserReceiver.UrlMoveCompleatEvent.WaitOne();
           }

           return obj;
       }

       //継承先で実処理を記述
       protected abstract object doneImpl(object inValue);

       /// <summary>
       /// ログ文字列(情報)を生成
       /// </summary>
       /// <param name="doType"></param>
       /// <param name="setType"></param>
       /// <param name="value"></param>
       public void setLogInfoStr(string doType, string setType, string index)
       {
           webBrowserReceiver.formLog.setLogStrList(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " Info " + " Do Type : " + doType + " Set Type : " + setType + " Index : " + index);
       }

       /// <summary>
       /// ログ文字列(情報)を生成
       /// </summary>
       /// <param name="doType"></param>
       /// <param name="setType"></param>
       /// <param name="value"></param>
       public void setLogInfoStr(string doType, string setType, string index,string value)
       {
           webBrowserReceiver.formLog.setLogStrList(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " Info " + " Do Type : " + doType + " Set Type : " + setType + " Index : " + index + " Value : " + value);
       }

       /// <summary>
       /// ログ文字列(警告)を生成
       /// </summary>
       /// <param name="doType"></param>
       /// <param name="setType"></param>
       /// <param name="value"></param>
       public void setLogWarnStr(string doType, string value, string message)
       {
           webBrowserReceiver.formLog.setLogStrList(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " Warn " + " Do Type : " + doType + " Value : " + value + " Message:" + message);
       }

       /// <summary>
       /// ログ文字列(警告)を生成
       /// </summary>
       /// <param name="doType"></param>
       /// <param name="setType"></param>
       /// <param name="value"></param>
       public void setLogWarnStr(string doType,string index, string value, string message)
       {
           webBrowserReceiver.formLog.setLogStrList(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " Warn " + " Do Type : " + doType + " Index : " + index + " Value : " + value + " Message:" + message);
       }
    }
}
