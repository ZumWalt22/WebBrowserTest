using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class FormLog : Form
    {
        public FormLog()
        {
            InitializeComponent();
        }

        //閉じるボタンを無効にする
        protected override CreateParams CreateParams
        {
            [System.Security.Permissions.SecurityPermission(
                System.Security.Permissions.SecurityAction.LinkDemand,
                Flags = System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)]
            get
            {
                const int CS_NOCLOSE = 0x200;
                CreateParams cp = base.CreateParams;
                cp.ClassStyle = cp.ClassStyle | CS_NOCLOSE;

                return cp;
            }
        }

        //Log文字列を設定
        public void setLogStrList(string logStr){

            textBoxLog.Text += logStr + "\r\n"; 
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }
    }
}
