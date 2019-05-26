using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using ExcelMng;
using System.Runtime.InteropServices;

namespace WindowsFormsApplication1
{   
    /// <summary>
    /// 画面エビデンス取得
    /// </summary>
    class CaptEvidence : Operation
    {

        //キャプチャ種別
        static string CAPT_MODE = "0"; //0:範囲指定通常キャプチャ　1:範囲指定通常キャプチャ　2:ActiveXキャプチャ（スクロール対応）

        //基底クラスのコンストラクタを「: base(webBrowser)」で呼び出すためのコンストラクタ
        public CaptEvidence(WebBrowserReceiver webBrowserReceiver)
            : base(webBrowserReceiver, false)
        {

        }

        /// <summary>
        /// エビデンス取得処理を実行
        /// </summary>
        /// <param name="inValue">Excelコントローラークラス</param>
        protected override object doneImpl(object inValue)
        {
            // カレントディレクトリを取得する
            string stCurrentDir = System.IO.Directory.GetCurrentDirectory();

            WebTestDto webTestDto = (WebTestDto)inValue;

            Bitmap bmp = null;

            if ("0".Equals(CAPT_MODE)) {

                ////コントロールの外観を描画するBitmapの作成
                bmp = CaptureControl(webBrowserReceiver.webBrowser);

                //ログ出力
                setLogInfoStr("Capture", "BitMap", "Nomal1");
            }
            else if ("1".Equals(CAPT_MODE))
            {
                int w = webBrowserReceiver.webBrowser.Document.Body.ScrollRectangle.Width;
                int h = webBrowserReceiver.webBrowser.Document.Body.ScrollRectangle.Height;

                //コントロールの外観を描画するBitmapの作成
                bmp = new Bitmap(w, h);
                //キャプチャする
                webBrowserReceiver.webBrowser.DrawToBitmap(bmp, new Rectangle(0, 0, w, h));

                //ログ出力
                setLogInfoStr("Capture", "BitMap", "Nomal2");
            } 
            else if ("2".Equals(CAPT_MODE)) 
            {
                //Activ X
                bmp = captScrollEbi(webBrowserReceiver.webBrowser);

                //ログ出力
                setLogInfoStr("Capture", "BitMap", "ActivX");
            }

            try
            {
                //Dirがあるかを確認する、ない場合は作成する
                string evidenceDirPass = stCurrentDir + "\\" + "Ebidence" + "\\";
                if (!System.IO.File.Exists(evidenceDirPass))
                {
                    System.IO.DirectoryInfo di = System.IO.Directory.CreateDirectory(evidenceDirPass);
                }

                //BitmapをSaveする
                bmp.Save(evidenceDirPass + webTestDto.testNo + "_" + webTestDto.ebidenceSetMode + ".bmp");
            }
            catch (Exception e)
            {
                //ログ出力
                setLogWarnStr("Capture", "BitMap", "エビデンスBitmapファイルの保存に失敗しました");
            }
            finally
            {
                //後始末
                bmp.Dispose();
            }

            //ダミーの値を戻す
            return null;
        }

        public enum DVASPECT : int
        {
            CONTENT = 1,
            THUMBNAIL = 2,
            ICON = 4,
            DOCPRINT = 8
        }

        [DllImport("ole32.dll")]
        public static extern int OleDraw(IntPtr pUnk, int dwAspect, IntPtr hdcDraw, ref Rectangle lprcBounds);

        private Bitmap captScrollEbi(WebBrowser webBrowser)
        {
            string BodyStyle = webBrowser.Document.Body.Style;
            Size BrowserSize = webBrowser.Size;
            webBrowser.Visible = false;
            webBrowser.Document.Body.Style += "overflow-x:hidden;overflow-y:hidden";

            //スクロールバーを消す 
            Rectangle ImageRect = webBrowser.Document.Body.ScrollRectangle;
            webBrowser.Size = ImageRect.Size;
            Bitmap WebImage = new Bitmap(ImageRect.Size.Width, ImageRect.Size.Height);
                using (Graphics g = Graphics.FromImage(WebImage))
                {
                    IntPtr pUnk = Marshal.GetIUnknownForObject(webBrowser.ActiveXInstance);
                    IntPtr hDc = g.GetHdc();
                    int r = OleDraw(pUnk,1, hDc,ref ImageRect);
                    g.ReleaseHdc(hDc);
                    Marshal.Release(pUnk);
                }
                //WebImage.Save("WebImage.bmp");
            webBrowser.Size = BrowserSize;
            webBrowser.Document.Body.Style = BodyStyle;
            webBrowser.Visible = true;

            return WebImage;
        }

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private extern static bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);

        /// <summary>
        /// コントロールのイメージを取得する
        /// </summary>
        /// <param name="ctrl">キャプチャするコントロール</param>
        /// <returns>取得できたイメージ</returns>
        public Bitmap CaptureControl(Control ctrl)
        {
            Bitmap img = new Bitmap(ctrl.Width, ctrl.Height);
            Graphics memg = Graphics.FromImage(img);
            IntPtr dc = memg.GetHdc();
            PrintWindow(ctrl.Handle, dc, 0);
            memg.ReleaseHdc(dc);
            memg.Dispose();
            return img;
        }

        //キャプチャ画像をファイルに保存する
        //http://dobon.net/vb/dotnet/graphics/screencapture.html
        private void setCaptureImageIFile(string savefilePass)
        {
            //画面全体のイメージをクリップボードにコピー
            //SendKeys.SendWait("^{PRTSC}");
            //次のようにすると、アクティブなウィンドウのイメージをコピー
            SendKeys.SendWait("%{PRTSC}");
            SendKeys.SendWait("{PRTSC}");

            //クリップボードにあるデータの取得
            System.Windows.Forms.IDataObject d = System.Windows.Forms.Clipboard.GetDataObject();

            //クリップボードにデータがあったか確認
            if (d != null)
            {
                System.Drawing.Image img = (System.Drawing.Image)d.GetData(System.Windows.Forms.DataFormats.Bitmap);

                if (img != null)
                {
                    img.Save(savefilePass);
                    img.Dispose();
                }
            }
        }
    }
}
