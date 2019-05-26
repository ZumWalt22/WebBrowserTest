using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

    public class TabControlEx : TabControl
    {

        // タブの閉じるボタンクリックイベント
        public event EventHandler TabCloseButtonClick;

        // コンストラクタ
        public TabControlEx()
        {
        }

        // タブの閉じるボタンクリックイベント
        protected void OnCloseButtonClick(EventArgs e)
        {
            MessageBox.Show("タブを閉じますか？");
            if (this.TabCloseButtonClick != null)
            {
                this.TabCloseButtonClick(this, e);
            }

            //選択されているタブページをクリアする
            this.TabPages.Remove(this.SelectedTab);
        }

        // OnMouseUp
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            Point pt = new Point(e.X, e.Y);
            Rectangle rect = this.GetTabCloseButtonRect(pt);
            if (rect.Contains(pt))
            {
                this.OnCloseButtonClick(new EventArgs());
                this.Invalidate(rect);
            }
        }

        // タブの閉じるボタン場所を取得
        private Rectangle GetTabCloseButtonRect(Point pt)
        {
            Rectangle rect;
            for (int i = 0; i < base.TabCount; i++)
            {
                rect = this.GetTabCloseButtonRect(i);
                if (rect.Contains(pt))
                {
                    return rect;
                }
            }
            return Rectangle.Empty;
        }

        // タブの閉じるボタン場所を取得
        private Rectangle GetTabCloseButtonRect(int index)
        {
            Rectangle rect = this.GetTabRect(index);
            rect.X = rect.Right - 20;
            rect.Y = rect.Top + 2;
            rect.Width = 16;
            rect.Height = 16;

            return rect;
        }

        // タブに閉じるボタンを描画
        private void DrawTabCloseButton()
        {
            Graphics g = this.CreateGraphics();
            Rectangle rect = Rectangle.Empty;
            Point pt = this.PointToClient(Cursor.Position);
            for (int i = 0; i < this.TabPages.Count; i++)
            {
                rect = this.GetTabCloseButtonRect(i);
                // 閉じるボタン描画
                ControlPaint.DrawCaptionButton(g, rect, CaptionButton.Close, ButtonState.Flat);
            }
            g = null;
        }

        // WndProc
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            switch (m.Msg)
            {
                case 15:
                    this.DrawTabCloseButton();
                    break;
                default:
                    break;
            }
        }
    }
