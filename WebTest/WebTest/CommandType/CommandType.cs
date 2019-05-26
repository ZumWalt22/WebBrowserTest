using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    /// <summary>
    /// コマンドクラス
    /// </summary>
    abstract class CommandType
    {
        /// <summary>
        /// 停止フラグ
        /// </summary>
        public bool abortFlg = false;


        protected WebBrowserReceiver webBrowserReceiver;

        ///継承先クラスのコンストラクタで、処理に必要な値を渡す

        /// <summary>
        /// WebBrowserReceiverインスタンスを設定
        /// </summary>
        /// <param name="webBrowserReceiver">コントロールクラスのインスタンス</param>
        public void setWebBrowserReceiver(WebBrowserReceiver webBrowserReceiver)
        {
            this.webBrowserReceiver = webBrowserReceiver;
        }

        /// <summary>
        /// 処理を実施
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public void execute(object obj)
        {
            //処理を実施
            executeDone(obj);

            //処理が完了したら、シグナル状態にする
            webBrowserReceiver.CommandCompleatEvent.Set();
        }

        /// <summary>
        /// 処理を記述
        /// 継承先で処理を実装する
        /// </summary>
        protected abstract void executeDone(object obj);
    }
}
