namespace CommonProcess.StringProcess
{
    partial class UCRepairString
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.chkRemovehanzi = new System.Windows.Forms.CheckBox();
            this.txt替换后 = new WinLib.WatermarkTextBox();
            this.txt替换前 = new WinLib.WatermarkTextBox();
            this.chk替换中间字符 = new System.Windows.Forms.CheckBox();
            this.txt首尾字符 = new WinLib.WatermarkTextBox();
            this.chk去首尾字符 = new System.Windows.Forms.CheckBox();
            this.cmbRegexStr = new WinLib.ComboBoxEx();
            this.wtxtRegexStr = new WinLib.WatermarkTextBox();
            this.chk首尾添加字符 = new System.Windows.Forms.CheckBox();
            this.watermarktxt首尾添加字符 = new WinLib.WatermarkTextBox();
            this.chk结果去首尾空格 = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txt移除尾部内容 = new System.Windows.Forms.TextBox();
            this.txt移除头部内容 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // chkRemovehanzi
            // 
            this.chkRemovehanzi.AutoSize = true;
            this.chkRemovehanzi.Location = new System.Drawing.Point(3, 37);
            this.chkRemovehanzi.Name = "chkRemovehanzi";
            this.chkRemovehanzi.Size = new System.Drawing.Size(84, 16);
            this.chkRemovehanzi.TabIndex = 5;
            this.chkRemovehanzi.Text = "去全角字符";
            this.chkRemovehanzi.UseVisualStyleBackColor = true;
            // 
            // txt替换后
            // 
            this.txt替换后.EmptyTextTip = "替换后的内容";
            this.txt替换后.Location = new System.Drawing.Point(235, 97);
            this.txt替换后.Name = "txt替换后";
            this.txt替换后.Size = new System.Drawing.Size(95, 21);
            this.txt替换后.TabIndex = 4;
            // 
            // txt替换前
            // 
            this.txt替换前.EmptyTextTip = "被替换内容";
            this.txt替换前.Location = new System.Drawing.Point(106, 97);
            this.txt替换前.Name = "txt替换前";
            this.txt替换前.Size = new System.Drawing.Size(113, 21);
            this.txt替换前.TabIndex = 3;
            // 
            // chk替换中间字符
            // 
            this.chk替换中间字符.AutoSize = true;
            this.chk替换中间字符.Location = new System.Drawing.Point(4, 99);
            this.chk替换中间字符.Name = "chk替换中间字符";
            this.chk替换中间字符.Size = new System.Drawing.Size(96, 16);
            this.chk替换中间字符.TabIndex = 2;
            this.chk替换中间字符.Text = "替换中间字符";
            this.chk替换中间字符.UseVisualStyleBackColor = true;
            // 
            // txt首尾字符
            // 
            this.txt首尾字符.EmptyTextTip = "需要去掉的字符";
            this.txt首尾字符.Location = new System.Drawing.Point(85, 2);
            this.txt首尾字符.Name = "txt首尾字符";
            this.txt首尾字符.Size = new System.Drawing.Size(134, 21);
            this.txt首尾字符.TabIndex = 1;
            // 
            // chk去首尾字符
            // 
            this.chk去首尾字符.AutoSize = true;
            this.chk去首尾字符.Location = new System.Drawing.Point(3, 3);
            this.chk去首尾字符.Name = "chk去首尾字符";
            this.chk去首尾字符.Size = new System.Drawing.Size(84, 16);
            this.chk去首尾字符.TabIndex = 0;
            this.chk去首尾字符.Text = "去首尾字符";
            this.chk去首尾字符.UseVisualStyleBackColor = true;
            // 
            // cmbRegexStr
            // 
            this.cmbRegexStr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRegexStr.FormattingEnabled = true;
            this.cmbRegexStr.Items.AddRange(new object[] {
            "中文字符的正则：[\\u4e00-\\u9fa5]",
            "双字节(包括汉字)：[^\\x00-\\xff]",
            "匹配空行的正则表达式：\\n[\\s| ]*\\r",
            "title=\"[\\s\\S]*?\"",
            "非英文字符:  [^a-z0-9A-Z_]+"});
            this.cmbRegexStr.Location = new System.Drawing.Point(85, 35);
            this.cmbRegexStr.Name = "cmbRegexStr";
            this.cmbRegexStr.Size = new System.Drawing.Size(245, 20);
            this.cmbRegexStr.TabIndex = 9;
            this.cmbRegexStr.SelectedIndexChanged += new System.EventHandler(this.cmbRegexStr_SelectedIndexChanged);
            // 
            // wtxtRegexStr
            // 
            this.wtxtRegexStr.EmptyTextTip = "请输入移除字符的自定义正则式";
            this.wtxtRegexStr.Location = new System.Drawing.Point(85, 57);
            this.wtxtRegexStr.Name = "wtxtRegexStr";
            this.wtxtRegexStr.Size = new System.Drawing.Size(245, 21);
            this.wtxtRegexStr.TabIndex = 7;
            this.wtxtRegexStr.Visible = false;
            // 
            // chk首尾添加字符
            // 
            this.chk首尾添加字符.AutoSize = true;
            this.chk首尾添加字符.Location = new System.Drawing.Point(4, 135);
            this.chk首尾添加字符.Name = "chk首尾添加字符";
            this.chk首尾添加字符.Size = new System.Drawing.Size(96, 16);
            this.chk首尾添加字符.TabIndex = 112;
            this.chk首尾添加字符.Text = "首尾添加字符";
            this.chk首尾添加字符.UseVisualStyleBackColor = true;
            // 
            // watermarktxt首尾添加字符
            // 
            this.watermarktxt首尾添加字符.EmptyTextTip = "aa[原始值]bb";
            this.watermarktxt首尾添加字符.Location = new System.Drawing.Point(106, 130);
            this.watermarktxt首尾添加字符.Name = "watermarktxt首尾添加字符";
            this.watermarktxt首尾添加字符.Size = new System.Drawing.Size(224, 21);
            this.watermarktxt首尾添加字符.TabIndex = 111;
            // 
            // chk结果去首尾空格
            // 
            this.chk结果去首尾空格.AutoSize = true;
            this.chk结果去首尾空格.Location = new System.Drawing.Point(444, 7);
            this.chk结果去首尾空格.Name = "chk结果去首尾空格";
            this.chk结果去首尾空格.Size = new System.Drawing.Size(108, 16);
            this.chk结果去首尾空格.TabIndex = 113;
            this.chk结果去首尾空格.Text = "结果去首尾空格";
            this.chk结果去首尾空格.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(361, 78);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(77, 12);
            this.label12.TabIndex = 14;
            this.label12.Text = "移除尾部内容";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(361, 46);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(77, 12);
            this.label15.TabIndex = 15;
            this.label15.Text = "移除头部内容";
            // 
            // txt移除尾部内容
            // 
            this.txt移除尾部内容.Location = new System.Drawing.Point(444, 75);
            this.txt移除尾部内容.Name = "txt移除尾部内容";
            this.txt移除尾部内容.Size = new System.Drawing.Size(133, 21);
            this.txt移除尾部内容.TabIndex = 17;
            // 
            // txt移除头部内容
            // 
            this.txt移除头部内容.Location = new System.Drawing.Point(444, 37);
            this.txt移除头部内容.Name = "txt移除头部内容";
            this.txt移除头部内容.Size = new System.Drawing.Size(133, 21);
            this.txt移除头部内容.TabIndex = 16;
            // 
            // UCRepairString
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txt移除尾部内容);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txt移除头部内容);
            this.Controls.Add(this.chk结果去首尾空格);
            this.Controls.Add(this.chk首尾添加字符);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.watermarktxt首尾添加字符);
            this.Controls.Add(this.cmbRegexStr);
            this.Controls.Add(this.wtxtRegexStr);
            this.Controls.Add(this.chkRemovehanzi);
            this.Controls.Add(this.txt首尾字符);
            this.Controls.Add(this.txt替换后);
            this.Controls.Add(this.chk去首尾字符);
            this.Controls.Add(this.txt替换前);
            this.Controls.Add(this.chk替换中间字符);
            this.Name = "UCRepairString";
            this.Size = new System.Drawing.Size(802, 451);
            this.Load += new System.EventHandler(this.UCRepairString_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkRemovehanzi;
        private WinLib.WatermarkTextBox txt替换后;
        private WinLib.WatermarkTextBox txt替换前;
        private System.Windows.Forms.CheckBox chk替换中间字符;
        private WinLib.WatermarkTextBox txt首尾字符;
        private System.Windows.Forms.CheckBox chk去首尾字符;
        private WinLib.ComboBoxEx cmbRegexStr;
        private WinLib.WatermarkTextBox wtxtRegexStr;
        private System.Windows.Forms.CheckBox chk首尾添加字符;
        private WinLib.WatermarkTextBox watermarktxt首尾添加字符;
        private System.Windows.Forms.CheckBox chk结果去首尾空格;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txt移除尾部内容;
        private System.Windows.Forms.TextBox txt移除头部内容;
    }
}
