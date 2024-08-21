namespace CommonProcess.StringProcess
{
    partial class UCDownloadFile
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCDownloadFile));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUrl = new WinLib.WatermarkTextBox();
            this.chk缓存HTML内容 = new System.Windows.Forms.CheckBox();
            this.txtEditorControlHTML缓存内容 = new ICSharpCode.TextEditor.TextEditorControl();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.chk保存时使用相对路径还是绝对路径 = new System.Windows.Forms.CheckBox();
            this.cmb下载文件名保存格式 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chk探测文件地址但不下载 = new System.Windows.Forms.CheckBox();
            this.chk探测文件并直接下载 = new System.Windows.Forms.CheckBox();
            this.chk下载图片 = new System.Windows.Forms.CheckBox();
            this.chk将相对地址补全为绝对地址 = new System.Windows.Forms.CheckBox();
            this.txtImgSaveDirectory = new WinLib.WatermarkTextBox();
            this.rdbHtml网页 = new System.Windows.Forms.RadioButton();
            this.rdbFile = new System.Windows.Forms.RadioButton();
            this.btnFindNeedPathForIMG = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txt分割字符 = new WinLib.WatermarkTextBox();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 110;
            this.label1.Text = "下载类型";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 112;
            this.label2.Text = "下载地址:";
            // 
            // txtUrl
            // 
            this.txtUrl.EmptyTextTip = "url";
            this.txtUrl.Location = new System.Drawing.Point(93, 82);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(353, 21);
            this.txtUrl.TabIndex = 111;
            // 
            // chk缓存HTML内容
            // 
            this.chk缓存HTML内容.AutoSize = true;
            this.chk缓存HTML内容.Location = new System.Drawing.Point(828, 34);
            this.chk缓存HTML内容.Name = "chk缓存HTML内容";
            this.chk缓存HTML内容.Size = new System.Drawing.Size(96, 16);
            this.chk缓存HTML内容.TabIndex = 113;
            this.chk缓存HTML内容.Text = "缓存HTML内容";
            this.chk缓存HTML内容.UseVisualStyleBackColor = true;
            this.chk缓存HTML内容.CheckedChanged += new System.EventHandler(this.chk缓存HTML内容_CheckedChanged);
            // 
            // txtEditorControlHTML缓存内容
            // 
            this.txtEditorControlHTML缓存内容.Encoding = ((System.Text.Encoding)(resources.GetObject("txtEditorControlHTML缓存内容.Encoding")));
            this.txtEditorControlHTML缓存内容.Location = new System.Drawing.Point(469, 67);
            this.txtEditorControlHTML缓存内容.Name = "txtEditorControlHTML缓存内容";
            this.txtEditorControlHTML缓存内容.ShowEOLMarkers = true;
            this.txtEditorControlHTML缓存内容.ShowSpaces = true;
            this.txtEditorControlHTML缓存内容.ShowTabs = true;
            this.txtEditorControlHTML缓存内容.ShowVRuler = true;
            this.txtEditorControlHTML缓存内容.Size = new System.Drawing.Size(464, 361);
            this.txtEditorControlHTML缓存内容.TabIndex = 114;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.chk保存时使用相对路径还是绝对路径);
            this.groupBox5.Controls.Add(this.cmb下载文件名保存格式);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.chk探测文件地址但不下载);
            this.groupBox5.Controls.Add(this.chk探测文件并直接下载);
            this.groupBox5.Controls.Add(this.chk下载图片);
            this.groupBox5.Controls.Add(this.chk将相对地址补全为绝对地址);
            this.groupBox5.Location = new System.Drawing.Point(21, 166);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(425, 197);
            this.groupBox5.TabIndex = 115;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "文件下载设置";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(41, 150);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(305, 36);
            this.label6.TabIndex = 71;
            this.label6.Text = "（最后执行) 多文件 默认分割标记#||#,如文件名来自采\r\n集，则格式应该为:http://xxxxabc.jpg?myPickFileName\r\n=[参数2]" +
    ".jpg&\r\n";
            // 
            // chk保存时使用相对路径还是绝对路径
            // 
            this.chk保存时使用相对路径还是绝对路径.AutoSize = true;
            this.chk保存时使用相对路径还是绝对路径.Location = new System.Drawing.Point(43, 108);
            this.chk保存时使用相对路径还是绝对路径.Name = "chk保存时使用相对路径还是绝对路径";
            this.chk保存时使用相对路径还是绝对路径.Size = new System.Drawing.Size(120, 16);
            this.chk保存时使用相对路径还是绝对路径.TabIndex = 70;
            this.chk保存时使用相对路径还是绝对路径.Text = "使用绝对路径保存";
            this.chk保存时使用相对路径还是绝对路径.UseVisualStyleBackColor = true;
            this.chk保存时使用相对路径还是绝对路径.CheckedChanged += new System.EventHandler(this.chk保存时使用相对路径还是绝对路径_CheckedChanged);
            // 
            // cmb下载文件名保存格式
            // 
            this.cmb下载文件名保存格式.FormattingEnabled = true;
            this.cmb下载文件名保存格式.Items.AddRange(new object[] {
            "【原文件名】",
            "【yyMMddHHmmssffff】",
            "【采集对应值为文件名=分割】",
            "【GUID】",
            "【GUID+原文件名】",
            "【原文件名+GUID】",
            "【yyMMddHHmmssffff+原文件名】",
            "【原文件名+yyMMddHHmmssffff】",
            "【yyMMddHH\\\\原文件名+GUID】"});
            this.cmb下载文件名保存格式.Location = new System.Drawing.Point(111, 127);
            this.cmb下载文件名保存格式.Name = "cmb下载文件名保存格式";
            this.cmb下载文件名保存格式.Size = new System.Drawing.Size(203, 20);
            this.cmb下载文件名保存格式.TabIndex = 69;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 12);
            this.label3.TabIndex = 68;
            this.label3.Text = "文件名保存格式:";
            // 
            // chk探测文件地址但不下载
            // 
            this.chk探测文件地址但不下载.AutoSize = true;
            this.chk探测文件地址但不下载.Location = new System.Drawing.Point(43, 86);
            this.chk探测文件地址但不下载.Name = "chk探测文件地址但不下载";
            this.chk探测文件地址但不下载.Size = new System.Drawing.Size(144, 16);
            this.chk探测文件地址但不下载.TabIndex = 66;
            this.chk探测文件地址但不下载.Text = "探测文件地址但不下载";
            this.chk探测文件地址但不下载.UseVisualStyleBackColor = true;
            // 
            // chk探测文件并直接下载
            // 
            this.chk探测文件并直接下载.AutoSize = true;
            this.chk探测文件并直接下载.Location = new System.Drawing.Point(43, 64);
            this.chk探测文件并直接下载.Name = "chk探测文件并直接下载";
            this.chk探测文件并直接下载.Size = new System.Drawing.Size(132, 16);
            this.chk探测文件并直接下载.TabIndex = 66;
            this.chk探测文件并直接下载.Text = "探测文件并直接下载";
            this.chk探测文件并直接下载.UseVisualStyleBackColor = true;
            // 
            // chk下载图片
            // 
            this.chk下载图片.AutoSize = true;
            this.chk下载图片.Location = new System.Drawing.Point(43, 42);
            this.chk下载图片.Name = "chk下载图片";
            this.chk下载图片.Size = new System.Drawing.Size(72, 16);
            this.chk下载图片.TabIndex = 66;
            this.chk下载图片.Text = "下载图片";
            this.chk下载图片.UseVisualStyleBackColor = true;
            // 
            // chk将相对地址补全为绝对地址
            // 
            this.chk将相对地址补全为绝对地址.AutoSize = true;
            this.chk将相对地址补全为绝对地址.Location = new System.Drawing.Point(43, 20);
            this.chk将相对地址补全为绝对地址.Name = "chk将相对地址补全为绝对地址";
            this.chk将相对地址补全为绝对地址.Size = new System.Drawing.Size(168, 16);
            this.chk将相对地址补全为绝对地址.TabIndex = 66;
            this.chk将相对地址补全为绝对地址.Text = "将相对地址补全为绝对地址";
            this.chk将相对地址补全为绝对地址.UseVisualStyleBackColor = true;
            // 
            // txtImgSaveDirectory
            // 
            this.txtImgSaveDirectory.EmptyTextTip = "SaveDirectory";
            this.txtImgSaveDirectory.Location = new System.Drawing.Point(93, 55);
            this.txtImgSaveDirectory.Name = "txtImgSaveDirectory";
            this.txtImgSaveDirectory.Size = new System.Drawing.Size(353, 21);
            this.txtImgSaveDirectory.TabIndex = 116;
            // 
            // rdbHtml网页
            // 
            this.rdbHtml网页.AutoSize = true;
            this.rdbHtml网页.Checked = true;
            this.rdbHtml网页.Location = new System.Drawing.Point(107, 12);
            this.rdbHtml网页.Name = "rdbHtml网页";
            this.rdbHtml网页.Size = new System.Drawing.Size(71, 16);
            this.rdbHtml网页.TabIndex = 116;
            this.rdbHtml网页.TabStop = true;
            this.rdbHtml网页.Text = "Html网页";
            this.rdbHtml网页.UseVisualStyleBackColor = true;
            this.rdbHtml网页.CheckedChanged += new System.EventHandler(this.rdbHtml网页_CheckedChanged);
            // 
            // rdbFile
            // 
            this.rdbFile.AutoSize = true;
            this.rdbFile.Location = new System.Drawing.Point(194, 12);
            this.rdbFile.Name = "rdbFile";
            this.rdbFile.Size = new System.Drawing.Size(47, 16);
            this.rdbFile.TabIndex = 117;
            this.rdbFile.Text = "文件";
            this.rdbFile.UseVisualStyleBackColor = true;
            // 
            // btnFindNeedPathForIMG
            // 
            this.btnFindNeedPathForIMG.Location = new System.Drawing.Point(484, 9);
            this.btnFindNeedPathForIMG.Name = "btnFindNeedPathForIMG";
            this.btnFindNeedPathForIMG.Size = new System.Drawing.Size(75, 23);
            this.btnFindNeedPathForIMG.TabIndex = 117;
            this.btnFindNeedPathForIMG.Text = "选择路径";
            this.btnFindNeedPathForIMG.UseVisualStyleBackColor = true;
            this.btnFindNeedPathForIMG.Click += new System.EventHandler(this.btnFindNeedPathForIMG_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 12);
            this.label4.TabIndex = 118;
            this.label4.Text = "文件保存路径:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(-6, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 120;
            this.label5.Text = "多个文件分割符";
            // 
            // txt分割字符
            // 
            this.txt分割字符.EmptyTextTip = "#||#";
            this.txt分割字符.Location = new System.Drawing.Point(93, 109);
            this.txt分割字符.Name = "txt分割字符";
            this.txt分割字符.Size = new System.Drawing.Size(215, 21);
            this.txt分割字符.TabIndex = 119;
            this.txt分割字符.Text = "#||#";
            // 
            // UCDownloadFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txt分割字符);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnFindNeedPathForIMG);
            this.Controls.Add(this.rdbFile);
            this.Controls.Add(this.txtImgSaveDirectory);
            this.Controls.Add(this.rdbHtml网页);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.txtEditorControlHTML缓存内容);
            this.Controls.Add(this.chk缓存HTML内容);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.label1);
            this.Name = "UCDownloadFile";
            this.Size = new System.Drawing.Size(945, 431);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public WinLib.WatermarkTextBox txtUrl;
        private System.Windows.Forms.CheckBox chk缓存HTML内容;
        internal ICSharpCode.TextEditor.TextEditorControl txtEditorControlHTML缓存内容;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chk保存时使用相对路径还是绝对路径;
        private System.Windows.Forms.ComboBox cmb下载文件名保存格式;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chk探测文件地址但不下载;
        private System.Windows.Forms.CheckBox chk探测文件并直接下载;
        private System.Windows.Forms.CheckBox chk下载图片;
        private System.Windows.Forms.CheckBox chk将相对地址补全为绝对地址;
        public WinLib.WatermarkTextBox txtImgSaveDirectory;
        private System.Windows.Forms.RadioButton rdbHtml网页;
        private System.Windows.Forms.RadioButton rdbFile;
        private System.Windows.Forms.Button btnFindNeedPathForIMG;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        public WinLib.WatermarkTextBox txt分割字符;
    }
}
