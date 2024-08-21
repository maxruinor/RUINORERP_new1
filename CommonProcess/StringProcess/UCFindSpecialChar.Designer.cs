using HLH.WinControl.ComBoBoxEx;
namespace CommonProcess.StringProcess
{
    partial class UCFindSpecialChar
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
            CheckBoxProperties checkBoxProperties1 = new CheckBoxProperties();
            this.chk替换字符 = new System.Windows.Forms.CheckBox();
            this.watermarkTextBox1 = new WinLib.WatermarkTextBox();
            this.rdb特殊字符正则式 = new System.Windows.Forms.RadioButton();
            this.rdb常见特列字符 = new System.Windows.Forms.RadioButton();
            this.cmbRegexStr = new System.Windows.Forms.ComboBox();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.cmbSpcChar = new CheckBoxComboBox();
            this.lblForRexDesc = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chk替换字符
            // 
            this.chk替换字符.AutoSize = true;
            this.chk替换字符.Location = new System.Drawing.Point(33, 106);
            this.chk替换字符.Name = "chk替换字符";
            this.chk替换字符.Size = new System.Drawing.Size(72, 16);
            this.chk替换字符.TabIndex = 14;
            this.chk替换字符.Text = "替换字符";
            this.chk替换字符.UseVisualStyleBackColor = true;
            // 
            // watermarkTextBox1
            // 
            this.watermarkTextBox1.EmptyTextTip = "请输入替换特殊字符的【新字符】,如果为空则替换为默认字符";
            this.watermarkTextBox1.Location = new System.Drawing.Point(146, 104);
            this.watermarkTextBox1.Name = "watermarkTextBox1";
            this.watermarkTextBox1.Size = new System.Drawing.Size(342, 21);
            this.watermarkTextBox1.TabIndex = 11;
            this.watermarkTextBox1.Visible = false;
            // 
            // rdb特殊字符正则式
            // 
            this.rdb特殊字符正则式.AutoSize = true;
            this.rdb特殊字符正则式.Location = new System.Drawing.Point(33, 4);
            this.rdb特殊字符正则式.Name = "rdb特殊字符正则式";
            this.rdb特殊字符正则式.Size = new System.Drawing.Size(107, 16);
            this.rdb特殊字符正则式.TabIndex = 15;
            this.rdb特殊字符正则式.Text = "特殊字符正则式";
            this.rdb特殊字符正则式.UseVisualStyleBackColor = true;
            this.rdb特殊字符正则式.CheckedChanged += new System.EventHandler(this.rdb特殊字符正则式_CheckedChanged);
            // 
            // rdb常见特列字符
            // 
            this.rdb常见特列字符.AutoSize = true;
            this.rdb常见特列字符.Checked = true;
            this.rdb常见特列字符.Location = new System.Drawing.Point(33, 56);
            this.rdb常见特列字符.Name = "rdb常见特列字符";
            this.rdb常见特列字符.Size = new System.Drawing.Size(95, 16);
            this.rdb常见特列字符.TabIndex = 15;
            this.rdb常见特列字符.TabStop = true;
            this.rdb常见特列字符.Text = "常见特列字符";
            this.rdb常见特列字符.UseVisualStyleBackColor = true;
            this.rdb常见特列字符.CheckedChanged += new System.EventHandler(this.rdb常见特列字符_CheckedChanged);
            // 
            // cmbRegexStr
            // 
            this.cmbRegexStr.FormattingEnabled = true;
            this.cmbRegexStr.Location = new System.Drawing.Point(146, 4);
            this.cmbRegexStr.Name = "cmbRegexStr";
            this.cmbRegexStr.Size = new System.Drawing.Size(245, 20);
            this.cmbRegexStr.TabIndex = 17;
            this.cmbRegexStr.SelectedIndexChanged += new System.EventHandler(this.cmbRegexStr_SelectedIndexChanged);
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Location = new System.Drawing.Point(397, 60);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(48, 16);
            this.chkSelectAll.TabIndex = 69;
            this.chkSelectAll.Text = "全选";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // cmbSpcChar
            // 
            checkBoxProperties1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbSpcChar.CheckBoxProperties = checkBoxProperties1;
            this.cmbSpcChar.DisplayMemberSingleItem = "";
            this.cmbSpcChar.FormattingEnabled = true;
            this.cmbSpcChar.Location = new System.Drawing.Point(146, 55);
            this.cmbSpcChar.Name = "cmbSpcChar";
            this.cmbSpcChar.Size = new System.Drawing.Size(245, 20);
            this.cmbSpcChar.TabIndex = 70;
            // 
            // lblForRexDesc
            // 
            this.lblForRexDesc.AutoSize = true;
            this.lblForRexDesc.Location = new System.Drawing.Point(397, 8);
            this.lblForRexDesc.Name = "lblForRexDesc";
            this.lblForRexDesc.Size = new System.Drawing.Size(0, 12);
            this.lblForRexDesc.TabIndex = 71;
            // 
            // UCFindSpecialChar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblForRexDesc);
            this.Controls.Add(this.cmbSpcChar);
            this.Controls.Add(this.chkSelectAll);
            this.Controls.Add(this.cmbRegexStr);
            this.Controls.Add(this.rdb常见特列字符);
            this.Controls.Add(this.rdb特殊字符正则式);
            this.Controls.Add(this.chk替换字符);
            this.Controls.Add(this.watermarkTextBox1);
            this.Name = "UCFindSpecialChar";
            this.Size = new System.Drawing.Size(551, 139);
            this.Load += new System.EventHandler(this.UCFindSpecialChar_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chk替换字符;
        private WinLib.WatermarkTextBox watermarkTextBox1;
        private System.Windows.Forms.RadioButton rdb特殊字符正则式;
        private System.Windows.Forms.RadioButton rdb常见特列字符;
        private System.Windows.Forms.ComboBox cmbRegexStr;
        private System.Windows.Forms.CheckBox chkSelectAll;
        private CheckBoxComboBox cmbSpcChar;
        private System.Windows.Forms.Label lblForRexDesc;



    }
}
