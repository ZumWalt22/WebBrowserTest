using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace WindowsFormsApplication1
{
    /// <summary>
    /// ログ出力
    /// 
    /// 参考Hp
    /// http://dobon.net/vb/dotnet/programing/tracelisteners.html
    /// </summary>
    class LogWrite : IDisposable
    {
        // コンストラクタ

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventLogName">OSイベントログ名称</param>
        /// <param name="logFileName">ログファイル名称</param>
        public LogWrite(string eventLogName,string logFileName)
        {
            //OSのイベントログに出力する　引数には任意のイベントログの名称をつける
            //Trace.Listeners.Add(new EventLogTraceListener(eventLogName));
            
            //指定のテキストに出力
            Trace.Listeners.Add(new TextWriterTraceListener(logFileName));

            //コンソールに出力する
            //Trace.Listeners.Add(new ConsoleTraceListener(false));
        }

        /// <summary>
        /// ログの出力を実施
        /// </summary>
        /// <param name="message">ログに出力するメッセージ</param>
        public void WriteLog(object message)
        {
            Trace.WriteLine(string.Format("{0} : " + message, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
            Trace.Flush();
        }

        // デストラクタ
        public void Dispose()
        {
            Trace.Listeners.Clear();
            GC.SuppressFinalize(this);
        }
    }
}
