namespace CommonProcess.StringProcess
{
    partial class UC正则式提取
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
            this.chk提取结果替换 = new System.Windows.Forms.CheckBox();
            this.groupBox使用正则式 = new System.Windows.Forms.GroupBox();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.lklblhart = new System.Windows.Forms.LinkLabel();
            this.chk正则模式 = new System.Windows.Forms.CheckBox();
            this.chk循环匹配 = new System.Windows.Forms.CheckBox();
            this.chk大小写转换 = new System.Windows.Forms.CheckBox();
            this.chk是否为贪婪模式 = new System.Windows.Forms.CheckBox();
            this.groupBox保留设置 = new System.Windows.Forms.GroupBox();
            this.rdb去掉提取部分 = new System.Windows.Forms.RadioButton();
            this.rdb保留提取部分 = new System.Windows.Forms.RadioButton();
            this.chk是否包含开始结束的标记 = new System.Windows.Forms.CheckBox();
            this.txtStart = new WinLib.WatermaskRichTextBox();
            this.txtEnd = new WinLib.WatermaskRichTextBox();
            this.txt提取结果替换 = new WinLib.WatermarkTextBox();
            this.btnDebugRegex = new System.Windows.Forms.Button();
            this.chk结果去首尾空格 = new System.Windows.Forms.CheckBox();
            this.chk提取源码来源表段值 = new System.Windows.Forms.CheckBox();
            this.cmbField = new System.Windows.Forms.ComboBox();
            this.lblrsInfo = new System.Windows.Forms.Label();
            this.txt循环分割字符 = new WinLib.WatermarkTextBox();
            this.chk循环匹配时去重复 = new System.Windows.Forms.CheckBox();
            this.linkLabel通配符 = new System.Windows.Forms.LinkLabel();
            this.groupBox使用正则式.SuspendLayout();
            this.groupBox保留设置.SuspendLayout();
            this.SuspendLayout();
            // 
            // chk提取结果替换
            // 
            this.chk提取结果替换.AutoSize = true;
            this.chk提取结果替换.Location = new System.Drawing.Point(6, 192);
            this.chk提取结果替换.Name = "chk提取结果替换";
            this.chk提取结果替换.Size = new System.Drawing.Size(108, 16);
            this.chk提取结果替换.TabIndex = 103;
            this.chk提取结果替换.Text = "修补提取[结果]";
            this.chk提取结果替换.UseVisualStyleBackColor = true;
            // 
            // groupBox使用正则式
            // 
            this.groupBox使用正则式.Controls.Add(this.linkLabel2);
            this.groupBox使用正则式.Controls.Add(this.label3);
            this.groupBox使用正则式.Controls.Add(this.linkLabel1);
            this.groupBox使用正则式.Controls.Add(this.linkLabel3);
            this.groupBox使用正则式.Location = new System.Drawing.Point(312, 133);
            this.groupBox使用正则式.Name = "groupBox使用正则式";
            this.groupBox使用正则式.Size = new System.Drawing.Size(298, 33);
            this.groupBox使用正则式.TabIndex = 102;
            this.groupBox使用正则式.TabStop = false;
            this.groupBox使用正则式.Visible = false;
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new System.Drawing.Point(175, 15);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(47, 12);
            this.linkLabel2.TabIndex = 85;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "[参数2]";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(57, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 83;
            this.label3.Text = "组合结果:";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(122, 15);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(47, 12);
            this.linkLabel1.TabIndex = 84;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "[参数1]";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // linkLabel3
            // 
            this.linkLabel3.AutoSize = true;
            this.linkLabel3.Location = new System.Drawing.Point(224, 15);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(65, 12);
            this.linkLabel3.TabIndex = 86;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "...[参数N]";
            // 
            // lklblhart
            // 
            this.lklblhart.AutoSize = true;
            this.lklblhart.Location = new System.Drawing.Point(210, 142);
            this.lklblhart.Name = "lklblhart";
            this.lklblhart.Size = new System.Drawing.Size(41, 12);
            this.lklblhart.TabIndex = 87;
            this.lklblhart.TabStop = true;
            this.lklblhart.Text = "[参数]";
            this.lklblhart.Visible = false;
            this.lklblhart.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lklblhart_LinkClicked);
            // 
            // chk正则模式
            // 
            this.chk正则模式.AutoSize = true;
            this.chk正则模式.Location = new System.Drawing.Point(6, 141);
            this.chk正则模式.Name = "chk正则模式";
            this.chk正则模式.Size = new System.Drawing.Size(198, 16);
            this.chk正则模式.TabIndex = 101;
            this.chk正则模式.Text = "正则参数模式【支持(*)通配符】";
            this.chk正则模式.UseVisualStyleBackColor = true;
            this.chk正则模式.CheckedChanged += new System.EventHandler(this.chk正则模式_CheckedChanged);
            // 
            // chk循环匹配
            // 
            this.chk循环匹配.AutoSize = true;
            this.chk循环匹配.Location = new System.Drawing.Point(183, 15);
            this.chk循环匹配.Name = "chk循环匹配";
            this.chk循环匹配.Size = new System.Drawing.Size(96, 16);
            this.chk循环匹配.TabIndex = 100;
            this.chk循环匹配.Text = "结果循环匹配";
            this.chk循环匹配.UseVisualStyleBackColor = true;
            // 
            // chk大小写转换
            // 
            this.chk大小写转换.AutoSize = true;
            this.chk大小写转换.Location = new System.Drawing.Point(331, 44);
            this.chk大小写转换.Name = "chk大小写转换";
            this.chk大小写转换.Size = new System.Drawing.Size(84, 16);
            this.chk大小写转换.TabIndex = 99;
            this.chk大小写转换.Text = "大小写转换";
            this.chk大小写转换.UseVisualStyleBackColor = true;
            this.chk大小写转换.Visible = false;
            // 
            // chk是否为贪婪模式
            // 
            this.chk是否为贪婪模式.AutoSize = true;
            this.chk是否为贪婪模式.Location = new System.Drawing.Point(6, 15);
            this.chk是否为贪婪模式.Name = "chk是否为贪婪模式";
            this.chk是否为贪婪模式.Size = new System.Drawing.Size(144, 16);
            this.chk是否为贪婪模式.TabIndex = 97;
            this.chk是否为贪婪模式.Text = "是否尽可能匹配到结尾";
            this.chk是否为贪婪模式.UseVisualStyleBackColor = true;
            // 
            // groupBox保留设置
            // 
            this.groupBox保留设置.Controls.Add(this.rdb去掉提取部分);
            this.groupBox保留设置.Controls.Add(this.rdb保留提取部分);
            this.groupBox保留设置.Location = new System.Drawing.Point(412, 33);
            this.groupBox保留设置.Name = "groupBox保留设置";
            this.groupBox保留设置.Size = new System.Drawing.Size(194, 29);
            this.groupBox保留设置.TabIndex = 96;
            this.groupBox保留设置.TabStop = false;
            // 
            // rdb去掉提取部分
            // 
            this.rdb去掉提取部分.AutoSize = true;
            this.rdb去掉提取部分.Location = new System.Drawing.Point(99, 11);
            this.rdb去掉提取部分.Name = "rdb去掉提取部分";
            this.rdb去掉提取部分.Size = new System.Drawing.Size(95, 16);
            this.rdb去掉提取部分.TabIndex = 1;
            this.rdb去掉提取部分.Text = "去掉提取结果";
            this.rdb去掉提取部分.UseVisualStyleBackColor = true;
            // 
            // rdb保留提取部分
            // 
            this.rdb保留提取部分.AutoSize = true;
            this.rdb保留提取部分.Checked = true;
            this.rdb保留提取部分.Location = new System.Drawing.Point(6, 11);
            this.rdb保留提取部分.Name = "rdb保留提取部分";
            this.rdb保留提取部分.Size = new System.Drawing.Size(95, 16);
            this.rdb保留提取部分.TabIndex = 0;
            this.rdb保留提取部分.TabStop = true;
            this.rdb保留提取部分.Text = "保留提取结果";
            this.rdb保留提取部分.UseVisualStyleBackColor = true;
            // 
            // chk是否包含开始结束的标记
            // 
            this.chk是否包含开始结束的标记.AutoSize = true;
            this.chk是否包含开始结束的标记.Checked = true;
            this.chk是否包含开始结束的标记.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk是否包含开始结束的标记.Location = new System.Drawing.Point(6, 33);
            this.chk是否包含开始结束的标记.Name = "chk是否包含开始结束的标记";
            this.chk是否包含开始结束的标记.Size = new System.Drawing.Size(144, 16);
            this.chk是否包含开始结束的标记.TabIndex = 0;
            this.chk是否包含开始结束的标记.Text = "是否包含开始结束标记";
            this.chk是否包含开始结束的标记.UseVisualStyleBackColor = true;
            // 
            // txtStart
            // 
            this.txtStart.EmptyTextTip = "【开始标记】";
            this.txtStart.Location = new System.Drawing.Point(6, 68);
            this.txtStart.Name = "txtStart";
            this.txtStart.Size = new System.Drawing.Size(279, 65);
            this.txtStart.TabIndex = 105;
            this.txtStart.Text = "";
            // 
            // txtEnd
            // 
            this.txtEnd.EmptyTextTip = "【结束标记】";
            this.txtEnd.Location = new System.Drawing.Point(331, 68);
            this.txtEnd.Name = "txtEnd";
            this.txtEnd.Size = new System.Drawing.Size(279, 65);
            this.txtEnd.TabIndex = 105;
            this.txtEnd.Text = "";
            // 
            // txt提取结果替换
            // 
            this.txt提取结果替换.EmptyTextTip = "[替换前缀][结果]";
            this.txt提取结果替换.Location = new System.Drawing.Point(120, 187);
            this.txt提取结果替换.Name = "txt提取结果替换";
            this.txt提取结果替换.Size = new System.Drawing.Size(285, 21);
            this.txt提取结果替换.TabIndex = 106;
            // 
            // btnDebugRegex
            // 
            this.btnDebugRegex.Location = new System.Drawing.Point(6, 163);
            this.btnDebugRegex.Name = "btnDebugRegex";
            this.btnDebugRegex.Size = new System.Drawing.Size(75, 23);
            this.btnDebugRegex.TabIndex = 112;
            this.btnDebugRegex.Text = "调试正则式";
            this.btnDebugRegex.UseVisualStyleBackColor = true;
            this.btnDebugRegex.Click += new System.EventHandler(this.btnDebugRegex_Click);
            // 
            // chk结果去首尾空格
            // 
            this.chk结果去首尾空格.AutoSize = true;
            this.chk结果去首尾空格.Location = new System.Drawing.Point(412, 189);
            this.chk结果去首尾空格.Name = "chk结果去首尾空格";
            this.chk结果去首尾空格.Size = new System.Drawing.Size(108, 16);
            this.chk结果去首尾空格.TabIndex = 111;
            this.chk结果去首尾空格.Text = "结果去首尾空格";
            this.chk结果去首尾空格.UseVisualStyleBackColor = true;
            // 
            // chk提取源码来源表段值
            // 
            this.chk提取源码来源表段值.AutoSize = true;
            this.chk提取源码来源表段值.Location = new System.Drawing.Point(295, 13);
            this.chk提取源码来源表段值.Name = "chk提取源码来源表段值";
            this.chk提取源码来源表段值.Size = new System.Drawing.Size(96, 16);
            this.chk提取源码来源表段值.TabIndex = 114;
            this.chk提取源码来源表段值.Text = "提取源码来源";
            this.chk提取源码来源表段值.UseVisualStyleBackColor = true;
            // 
            // cmbField
            // 
            this.cmbField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbField.FormattingEnabled = true;
            this.cmbField.Location = new System.Drawing.Point(400, 11);
            this.cmbField.Name = "cmbField";
            this.cmbField.Size = new System.Drawing.Size(210, 20);
            this.cmbField.TabIndex = 113;
            // 
            // lblrsInfo
            // 
            this.lblrsInfo.AutoSize = true;
            this.lblrsInfo.ForeColor = System.Drawing.Color.Red;
            this.lblrsInfo.Location = new System.Drawing.Point(6, 217);
            this.lblrsInfo.Name = "lblrsInfo";
            this.lblrsInfo.Size = new System.Drawing.Size(0, 12);
            this.lblrsInfo.TabIndex = 115;
            // 
            // txt循环分割字符
            // 
            this.txt循环分割字符.EmptyTextTip = "循环分割字符";
            this.txt循环分割字符.Location = new System.Drawing.Point(183, 38);
            this.txt循环分割字符.Name = "txt循环分割字符";
            this.txt循环分割字符.Size = new System.Drawing.Size(100, 21);
            this.txt循环分割字符.TabIndex = 116;
            this.txt循环分割字符.Text = "#||#";
            // 
            // chk循环匹配时去重复
            // 
            this.chk循环匹配时去重复.AutoSize = true;
            this.chk循环匹配时去重复.Location = new System.Drawing.Point(6, 51);
            this.chk循环匹配时去重复.Name = "chk循环匹配时去重复";
            this.chk循环匹配时去重复.Size = new System.Drawing.Size(120, 16);
            this.chk循环匹配时去重复.TabIndex = 117;
            this.chk循环匹配时去重复.Text = "循环匹配时去重复";
            this.chk循环匹配时去重复.UseVisualStyleBackColor = true;
            // 
            // linkLabel通配符
            // 
            this.linkLabel通配符.AutoSize = true;
            this.linkLabel通配符.Location = new System.Drawing.Point(257, 142);
            this.linkLabel通配符.Name = "linkLabel通配符";
            this.linkLabel通配符.Size = new System.Drawing.Size(23, 12);
            this.linkLabel通配符.TabIndex = 118;
            this.linkLabel通配符.TabStop = true;
            this.linkLabel通配符.Text = "(*)";
            this.linkLabel通配符.Visible = false;
            this.linkLabel通配符.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel通配符_LinkClicked);
            // 
            // UC正则式提取
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.linkLabel通配符);
            this.Controls.Add(this.chk循环匹配时去重复);
            this.Controls.Add(this.txt循环分割字符);
            this.Controls.Add(this.lblrsInfo);
            this.Controls.Add(this.chk提取源码来源表段值);
            this.Controls.Add(this.cmbField);
            this.Controls.Add(this.btnDebugRegex);
            this.Controls.Add(this.chk结果去首尾空格);
            this.Controls.Add(this.txt提取结果替换);
            this.Controls.Add(this.txtEnd);
            this.Controls.Add(this.txtStart);
            this.Controls.Add(this.chk是否包含开始结束的标记);
            this.Controls.Add(this.chk提取结果替换);
            this.Controls.Add(this.lklblhart);
            this.Controls.Add(this.groupBox使用正则式);
            this.Controls.Add(this.chk正则模式);
            this.Controls.Add(this.chk循环匹配);
            this.Controls.Add(this.chk大小写转换);
            this.Controls.Add(this.chk是否为贪婪模式);
            this.Controls.Add(this.groupBox保留设置);
            this.Name = "UC正则式提取";
            this.Size = new System.Drawing.Size(654, 232);
            this.groupBox使用正则式.ResumeLayout(false);
            this.groupBox使用正则式.PerformLayout();
            this.groupBox保留设置.ResumeLayout(false);
            this.groupBox保留设置.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chk提取结果替换;
        private System.Windows.Forms.GroupBox groupBox使用正则式;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel lklblhart;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel linkLabel3;
        private System.Windows.Forms.CheckBox chk正则模式;
        private System.Windows.Forms.CheckBox chk循环匹配;
        private System.Windows.Forms.CheckBox chk大小写转换;
        private System.Windows.Forms.CheckBox chk是否为贪婪模式;
        private System.Windows.Forms.GroupBox groupBox保留设置;
        private System.Windows.Forms.RadioButton rdb去掉提取部分;
        private System.Windows.Forms.RadioButton rdb保留提取部分;
        private System.Windows.Forms.CheckBox chk是否包含开始结束的标记;
        private WinLib.WatermaskRichTextBox txtStart;
        private WinLib.WatermaskRichTextBox txtEnd;
        private WinLib.WatermarkTextBox txt提取结果替换;
        private System.Windows.Forms.Button btnDebugRegex;
        private System.Windows.Forms.CheckBox chk结果去首尾空格;
        public System.Windows.Forms.CheckBox chk提取源码来源表段值;
        public System.Windows.Forms.ComboBox cmbField;
        private System.Windows.Forms.Label lblrsInfo;
        private WinLib.WatermarkTextBox txt循环分割字符;
        private System.Windows.Forms.CheckBox chk循环匹配时去重复;
        private System.Windows.Forms.LinkLabel linkLabel通配符;
    }
}
