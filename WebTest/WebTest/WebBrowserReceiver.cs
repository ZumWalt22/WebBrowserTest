using System;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;
using ExcelMng;

namespace WindowsFormsApplication1
{
    /// <summary>
    /// webBrowserコントロールクラス
    /// </summary>
    class WebBrowserReceiver
    {
        //------------Operationクラス
        SetUrl       setUrlImpl;
        SetValue     setValueImpl;
        Click        clickImpl;
        Submit       submitImpl;
        GetValue     getValueImpl;
        ReLoad       reLoadImpl;
        CheckValue   checkValueImpl;

        CaptEvidence captEvidence;
        CaptHtml     captHtml;
        //DBEvidence   dbEvidence;
        DoJavaScript doJavaScript;
        
        GetStateBySource getStateBySourceImpl;

        //以下のインスタンスは、Operationクラスから参照するためPublicにする

        //webBrowserインスタンス
        public WebBrowser webBrowser;

        //ExcelControllerインスタンス
        public ExcelController excelController;

        //親フォームのインスタンス
        public Form parentForm;

        //コマンドクラスのインスタンス
        CommandType commandType;

        //画面推移完了のイベント(画面推移完了時に実行される関数からシグナル状態にされる)
        public AutoResetEvent UrlMoveCompleatEvent;

        //画面推移回数
        public int documentCompletedCount = 0;

        //画面処理完了までの推移回数　リダイレクトで複数回画面推移が行われる場合のため        
        int documentCompletedCountMax = 0;  

        //Command処理完了のイベント
        public AutoResetEvent CommandCompleatEvent;

        //Log出力画面を設定
        public FormLog formLog;

        /// <summary>
        /// webBrowserコンストラクタ
        /// webBrowserインスタンスはForm上からでないと実行できないっぽい（ActiveXの関係らしい？）
        /// </summary>
        /// <param name="webBrowser">webBrowserインスタンスを設定</param>
        public WebBrowserReceiver(Form parentForm, WebBrowser webBrowser, ExcelController excelController,FormLog formLog)
        {
            //webBrowserのインスタンスを設定
            this.webBrowser = webBrowser;

            //excelControllerのインスタンスを設定
            this.excelController = excelController;

            //親フォームのインスタンスを設定
            this.parentForm = parentForm;

            //ログ出力画面を設定
            this.formLog = formLog;

            //Ｗｅｂ画面表示完了イベント登録
            this.webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(documentCompleted);

            //非シグナル状態でAutoResetEventオブジェクトを作成
            UrlMoveCompleatEvent = new System.Threading.AutoResetEvent(false);
            CommandCompleatEvent = new System.Threading.AutoResetEvent(false);

            //operationクラスを生成
            setUrlImpl      = new SetUrl(this);
            setValueImpl    = new SetValue(this);
            clickImpl       = new Click(this);
            submitImpl      = new Submit(this);
            getValueImpl    = new GetValue(this);
            reLoadImpl      = new ReLoad(this);
            checkValueImpl  = new CheckValue(this);

            captEvidence    = new CaptEvidence(this);
            captHtml        = new CaptHtml(this);
            //dbEvidence      = new DBEvidence(this);
            doJavaScript    = new DoJavaScript(this);

            getStateBySourceImpl = new GetStateBySource(this);
        }

        //Ｗｅｂ画面表示完了
        private void documentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //画面推移回数をインクリメント
            documentCompletedCount++;

            //推移回数を確認　リダイレクトで複数回画面推移が行われる場合のため
            if (documentCompletedCount >= documentCompletedCountMax)
            {
                //シグナル状態にする
                UrlMoveCompleatEvent.Set();
            }

			//画面項目情報を出力
			formLog.setLogStrList(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " Screen Title : " + webBrowser.Document.Title);
			foreach (HtmlElement element in webBrowser.Document.All)
			{
				formLog.setLogStrList(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
					+ " Item Info "
					+ " ID : " + element.Id
					+ " Name : " + element.Name
					+ " Value : " + element.InnerText);
			}
		}

        /// <summary>
        /// 処理を中止する
        /// </summary>
        public void doAbort(){

            commandType.abortFlg = true;
        }

        /// <summary>
        /// 処理を実行する
        /// 
        /// 注意　この関数は必ずスレッド内で実行すること（スレッド化しないと「Commandスレッド処理がおわるまで待機」で処理がすすまなくなる）
        /// </summary>
        /// <param name="commandType">実行する処理のコマンドクラス</param>
        public void done(CommandType commandType)
        {
            //Command処理完了完了シグナルをリセット
            CommandCompleatEvent.Reset();

            //コマンドクラスを内部に設定
            this.commandType = commandType;

            //コマンドクラスのインスタンスにWebBrowserReceiverのリンクを渡す
            commandType.setWebBrowserReceiver(this);

            //メソッドをスレッドプールのキューに追加する
            ThreadPool.QueueUserWorkItem(new WaitCallback(commandType.execute), null);

            //Commandスレッド処理がおわるまで待機
            CommandCompleatEvent.WaitOne();
        }

        //------------ここから下はコマンドクラスで使用される関数

        #region コマンドクラス
        
        /// <summary>
        /// URLのページに移動する
        /// </summary>
        /// <param name="url"></param>
        public void setUrl(string url, int documentCompletedCountMax)
        {
            this.documentCompletedCountMax = documentCompletedCountMax;

            setUrlImpl.done(url);
        }

        /// <summary>
        /// 値を設定する
        /// </summary>
        /// <param name="formElementList">設定する値構造体のリスト</param>
        public void setValue(List<FormElement> formElementList)
        {
            setValueImpl.done(formElementList);
        }

        /// <summary>
        /// 値を取得する
        /// </summary>
        /// <param name="formElementList">設定する値構造体のリスト</param>
        public void getValue(List<FormElement> formElementList)
        {
            getValueImpl.done(formElementList);
        }

        /// <summary>
        /// 値の差異を確認する
        /// </summary>
        /// <param name="formElementList">設定する値構造体のリスト</param>
        public void checkValue(List<FormElement> formElementList)
        {
            checkValueImpl.done(formElementList);
        }

        /// <summary>
        /// ボタンを押す(Submit)
        /// </summary>
        /// <param name="buttonName"ボタン名称</param>
        public bool submit(int index, int documentCompletedCountMax)
        {
            this.documentCompletedCountMax = documentCompletedCountMax;

            submitImpl.done(index);

            return true;
        }

        /// <summary>
        /// ボタンを押す(Click)
        /// </summary>
        /// <param name="buttonName"ボタン名称</param>
        public bool click(string buttonName, int documentCompletedCountMax)
        {
            bool buttonExistFlag = (bool)clickImpl.done(buttonName);

            //ボタンが存在する場合のみ画面遷移カウントを設定
            if (buttonExistFlag)
            {
             this.documentCompletedCountMax = documentCompletedCountMax;
            }

            return buttonExistFlag;
        }

        /// <summary>
        /// 画面の値を取得する(正規表現)
        /// </summary>
        /// <param name="id">値を取得するための正規表現</param>
        public string getStateBySource(string regex)
        {
            return (string)getStateBySourceImpl.done(regex);
        }

        /// <summary>
        /// 画面を再描画する
        /// </summary>
        public void reLoad(int documentCompletedCountMax)
        {
            this.documentCompletedCountMax = documentCompletedCountMax;

            reLoadImpl.done(null);
        }

        /// <summary>
        /// 画面キャプチャを取得する
        /// </summary>
        public void getWindowCapt(WebTestDto webTest)
        {
            captEvidence.done(webTest);
        }

        /// <summary>
        /// Htmlソーキャプチャを取得する
        /// </summary>
        public void getHtmlCapt(WebTestDto webTest)
        {
            captHtml.done(webTest);
        }

        /// <summary>
        /// DBキャプチャを取得する
        /// </summary>
        public void getDBCapt(WebTestDto webTest)
        {
            //dbEvidence.done(webTest);
        }

        /// <summary>
        /// JavaScriptを実行する
        /// </summary>
        public void javaScript(string javaScrStr, int documentCompletedCountMax)
        {
            this.documentCompletedCountMax = documentCompletedCountMax;

            doJavaScript.done(javaScrStr);
        }
        #endregion
    }
}