namespace CommonProcess.StringProcess
{
    partial class UCSetSpecFieldValue
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
            this.cmbtarget = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt固定值 = new System.Windows.Forms.TextBox();
            this.rdb设置为固定字符 = new System.Windows.Forms.RadioButton();
            this.rdb来源表中字段值 = new System.Windows.Forms.RadioButton();
            this.txt选择结果替换 = new WinLib.WatermarkTextBox();
            this.chk选择字段修补 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cmbtarget
            // 
            this.cmbtarget.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbtarget.FormattingEnabled = true;
            this.cmbtarget.Location = new System.Drawing.Point(146, 23);
            this.cmbtarget.Name = "cmbtarget";
            this.cmbtarget.Size = new System.Drawing.Size(251, 20);
            this.cmbtarget.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.OrangeRed;
            this.label1.Location = new System.Drawing.Point(32, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(209, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "来源字段值将会覆盖将要处理字段的值";
            // 
            // txt固定值
            // 
            this.txt固定值.Location = new System.Drawing.Point(147, 57);
            this.txt固定值.Multiline = true;
            this.txt固定值.Name = "txt固定值";
            this.txt固定值.Size = new System.Drawing.Size(250, 45);
            this.txt固定值.TabIndex = 5;
            // 
            // rdb设置为固定字符
            // 
            this.rdb设置为固定字符.AutoSize = true;
            this.rdb设置为固定字符.Location = new System.Drawing.Point(21, 61);
            this.rdb设置为固定字符.Name = "rdb设置为固定字符";
            this.rdb设置为固定字符.Size = new System.Drawing.Size(107, 16);
            this.rdb设置为固定字符.TabIndex = 6;
            this.rdb设置为固定字符.TabStop = true;
            this.rdb设置为固定字符.Text = "设置为固定字符";
            this.rdb设置为固定字符.UseVisualStyleBackColor = true;
            // 
            // rdb来源表中字段值
            // 
            this.rdb来源表中字段值.AutoSize = true;
            this.rdb来源表中字段值.Location = new System.Drawing.Point(21, 24);
            this.rdb来源表中字段值.Name = "rdb来源表中字段值";
            this.rdb来源表中字段值.Size = new System.Drawing.Size(107, 16);
            this.rdb来源表中字段值.TabIndex = 7;
            this.rdb来源表中字段值.TabStop = true;
            this.rdb来源表中字段值.Text = "来源表中字段值";
            this.rdb来源表中字段值.UseVisualStyleBackColor = true;
            // 
            // txt选择结果替换
            // 
            this.txt选择结果替换.EmptyTextTip = "[替换前缀][结果][替换后缀]";
            this.txt选择结果替换.Location = new System.Drawing.Point(180, 109);
            this.txt选择结果替换.Name = "txt选择结果替换";
            this.txt选择结果替换.Size = new System.Drawing.Size(215, 21);
            this.txt选择结果替换.TabIndex = 108;
            // 
            // chk选择字段修补
            // 
            this.chk选择字段修补.AutoSize = true;
            this.chk选择字段修补.Location = new System.Drawing.Point(32, 113);
            this.chk选择字段修补.Name = "chk选择字段修补";
            this.chk选择字段修补.Size = new System.Drawing.Size(144, 16);
            this.chk选择字段修补.TabIndex = 107;
            this.chk选择字段修补.Text = "[选择字段修补][结果]";
            this.chk选择字段修补.UseVisualStyleBackColor = true;
            // 
            // UCSetSpecFieldValue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txt选择结果替换);
            this.Controls.Add(this.chk选择字段修补);
            this.Controls.Add(this.rdb来源表中字段值);
            this.Controls.Add(this.rdb设置为固定字符);
            this.Controls.Add(this.txt固定值);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbtarget);
            this.Name = "UCSetSpecFieldValue";
            this.Size = new System.Drawing.Size(452, 142);
            this.Load += new System.EventHandler(this.UCFieldCopy_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ComboBox cmbtarget;
        public System.Windows.Forms.RadioButton rdb来源表中字段值;
        public System.Windows.Forms.RadioButton rdb设置为固定字符;
        public System.Windows.Forms.TextBox txt固定值;
        public WinLib.WatermarkTextBox txt选择结果替换;
        public System.Windows.Forms.CheckBox chk选择字段修补;
    }
}
