namespace WindowsFormsApplication1
{
    partial class FormWebTest
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.buttonAllSel = new System.Windows.Forms.Button();
            this.buttonAllOff = new System.Windows.Forms.Button();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.buttonAbort = new System.Windows.Forms.Button();
            this.buttonLog = new System.Windows.Forms.Button();
            this.buttonEvidenceDir = new System.Windows.Forms.Button();
            this.buttonUrlSet = new System.Windows.Forms.Button();
            this.buttonTestAllDo = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBoxUrl = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.tabControl1 = new TabControlEx();
            this.labelTestCaseExcelFile = new System.Windows.Forms.Label();
            this.buttonTestCaseSet = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 714);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 22, 0);
            this.statusStrip1.Size = new System.Drawing.Size(978, 30);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "-";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(19, 25);
            this.toolStripStatusLabel1.Text = "-";
            // 
            // buttonAllSel
            // 
            this.buttonAllSel.Location = new System.Drawing.Point(7, 56);
            this.buttonAllSel.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.buttonAllSel.Name = "buttonAllSel";
            this.buttonAllSel.Size = new System.Drawing.Size(73, 34);
            this.buttonAllSel.TabIndex = 17;
            this.buttonAllSel.Text = "全選択";
            this.buttonAllSel.UseVisualStyleBackColor = true;
            this.buttonAllSel.Click += new System.EventHandler(this.buttonAllSel_Click);
            // 
            // buttonAllOff
            // 
            this.buttonAllOff.Location = new System.Drawing.Point(85, 56);
            this.buttonAllOff.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.buttonAllOff.Name = "buttonAllOff";
            this.buttonAllOff.Size = new System.Drawing.Size(80, 34);
            this.buttonAllOff.TabIndex = 16;
            this.buttonAllOff.Text = "全解除";
            this.buttonAllOff.UseVisualStyleBackColor = true;
            this.buttonAllOff.Click += new System.EventHandler(this.buttonAllOff_Click);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(7, 107);
            this.checkedListBox1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(246, 584);
            this.checkedListBox1.TabIndex = 15;
            // 
            // buttonAbort
            // 
            this.buttonAbort.Location = new System.Drawing.Point(448, 51);
            this.buttonAbort.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.buttonAbort.Name = "buttonAbort";
            this.buttonAbort.Size = new System.Drawing.Size(69, 116);
            this.buttonAbort.TabIndex = 23;
            this.buttonAbort.Text = "停止";
            this.buttonAbort.UseVisualStyleBackColor = true;
            this.buttonAbort.Click += new System.EventHandler(this.buttonAbort_Click);
            // 
            // buttonLog
            // 
            this.buttonLog.Location = new System.Drawing.Point(864, 13);
            this.buttonLog.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.buttonLog.Name = "buttonLog";
            this.buttonLog.Size = new System.Drawing.Size(57, 34);
            this.buttonLog.TabIndex = 22;
            this.buttonLog.Text = "Log";
            this.buttonLog.UseVisualStyleBackColor = true;
            this.buttonLog.Click += new System.EventHandler(this.buttonLog_Click);
            // 
            // buttonEvidenceDir
            // 
            this.buttonEvidenceDir.Location = new System.Drawing.Point(733, 15);
            this.buttonEvidenceDir.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.buttonEvidenceDir.Name = "buttonEvidenceDir";
            this.buttonEvidenceDir.Size = new System.Drawing.Size(121, 34);
            this.buttonEvidenceDir.TabIndex = 21;
            this.buttonEvidenceDir.Text = "エビデンスDir";
            this.buttonEvidenceDir.UseVisualStyleBackColor = true;
            this.buttonEvidenceDir.Click += new System.EventHandler(this.buttonEvidenceDir_Click);
            // 
            // buttonUrlSet
            // 
            this.buttonUrlSet.Location = new System.Drawing.Point(533, 117);
            this.buttonUrlSet.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.buttonUrlSet.Name = "buttonUrlSet";
            this.buttonUrlSet.Size = new System.Drawing.Size(85, 34);
            this.buttonUrlSet.TabIndex = 18;
            this.buttonUrlSet.Text = "URL設定";
            this.buttonUrlSet.UseVisualStyleBackColor = true;
            this.buttonUrlSet.Click += new System.EventHandler(this.buttonUrlSet_Click);
            // 
            // buttonTestAllDo
            // 
            this.buttonTestAllDo.Location = new System.Drawing.Point(268, 51);
            this.buttonTestAllDo.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.buttonTestAllDo.Name = "buttonTestAllDo";
            this.buttonTestAllDo.Size = new System.Drawing.Size(170, 116);
            this.buttonTestAllDo.TabIndex = 20;
            this.buttonTestAllDo.Text = "テスト自動実行";
            this.buttonTestAllDo.UseVisualStyleBackColor = true;
            this.buttonTestAllDo.Click += new System.EventHandler(this.buttonTestAllDo_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(533, 81);
            this.button4.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(85, 34);
            this.button4.TabIndex = 16;
            this.button4.Text = "URL移動";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(622, 117);
            this.button5.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(99, 34);
            this.button5.TabIndex = 7;
            this.button5.Text = "ケース設定";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(724, 117);
            this.button1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(99, 34);
            this.button1.TabIndex = 10;
            this.button1.Text = "実行ボタン";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBoxUrl
            // 
            this.comboBoxUrl.FormattingEnabled = true;
            this.comboBoxUrl.Location = new System.Drawing.Point(622, 83);
            this.comboBoxUrl.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.comboBoxUrl.Name = "comboBoxUrl";
            this.comboBoxUrl.Size = new System.Drawing.Size(301, 26);
            this.comboBoxUrl.TabIndex = 12;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(826, 117);
            this.button2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(95, 34);
            this.button2.TabIndex = 13;
            this.button2.Text = "エビデンス";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Location = new System.Drawing.Point(268, 175);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(702, 524);
            this.tabControl1.TabIndex = 1;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // labelTestCaseExcelFile
            // 
            this.labelTestCaseExcelFile.Location = new System.Drawing.Point(175, 21);
            this.labelTestCaseExcelFile.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelTestCaseExcelFile.Name = "labelTestCaseExcelFile";
            this.labelTestCaseExcelFile.Size = new System.Drawing.Size(548, 26);
            this.labelTestCaseExcelFile.TabIndex = 9;
            this.labelTestCaseExcelFile.Text = "-";
            // 
            // buttonTestCaseSet
            // 
            this.buttonTestCaseSet.Location = new System.Drawing.Point(7, 13);
            this.buttonTestCaseSet.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.buttonTestCaseSet.Name = "buttonTestCaseSet";
            this.buttonTestCaseSet.Size = new System.Drawing.Size(158, 34);
            this.buttonTestCaseSet.TabIndex = 14;
            this.buttonTestCaseSet.Text = "テストケース設定";
            this.buttonTestCaseSet.UseVisualStyleBackColor = true;
            this.buttonTestCaseSet.Click += new System.EventHandler(this.button3_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Font = new System.Drawing.Font("MS UI Gothic", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupBox1.Location = new System.Drawing.Point(525, 56);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(445, 112);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "手動実行";
            // 
            // FormWebTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(978, 744);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.labelTestCaseExcelFile);
            this.Controls.Add(this.buttonTestCaseSet);
            this.Controls.Add(this.buttonLog);
            this.Controls.Add(this.buttonAbort);
            this.Controls.Add(this.buttonEvidenceDir);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.buttonUrlSet);
            this.Controls.Add(this.buttonAllSel);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.buttonAllOff);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonTestAllDo);
            this.Controls.Add(this.comboBoxUrl);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "FormWebTest";
            this.Text = "WebTest";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormWebTest_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox comboBoxUrl;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Button button4;
        private TabControlEx tabControl1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button buttonTestAllDo;
        private System.Windows.Forms.Button buttonUrlSet;
        private System.Windows.Forms.Button buttonEvidenceDir;
        private System.Windows.Forms.Button buttonLog;
        private System.Windows.Forms.Button buttonAllSel;
        private System.Windows.Forms.Button buttonAllOff;
        private System.Windows.Forms.Button buttonAbort;
        private System.Windows.Forms.Label labelTestCaseExcelFile;
        private System.Windows.Forms.Button buttonTestCaseSet;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

