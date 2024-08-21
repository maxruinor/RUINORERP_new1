namespace CommonProcess.StringProcess
{
    partial class UCCheckData
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtLength = new WinLib.WatermaskRichTextBox();
            this.rdbNotHtml = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(14, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "本功能只是数据检测";
            // 
            // txtLength
            // 
            this.txtLength.EmptyTextTip = "请输入数字";
            this.txtLength.Location = new System.Drawing.Point(233, 46);
            this.txtLength.Name = "txtLength";
            this.txtLength.Size = new System.Drawing.Size(94, 27);
            this.txtLength.TabIndex = 106;
            this.txtLength.Text = "";
            // 
            // rdbNotHtml
            // 
            this.rdbNotHtml.AutoSize = true;
            this.rdbNotHtml.Location = new System.Drawing.Point(16, 57);
            this.rdbNotHtml.Name = "rdbNotHtml";
            this.rdbNotHtml.Size = new System.Drawing.Size(197, 16);
            this.rdbNotHtml.TabIndex = 107;
            this.rdbNotHtml.TabStop = true;
            this.rdbNotHtml.Text = "检测内容长度（不包括html代码)";
            this.rdbNotHtml.UseVisualStyleBackColor = true;
            // 
            // UCCheckData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rdbNotHtml);
            this.Controls.Add(this.txtLength);
            this.Controls.Add(this.label1);
            this.Name = "UCCheckData";
            this.Size = new System.Drawing.Size(540, 169);
            this.Load += new System.EventHandler(this.UCFindSpecialChar_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.RadioButton rdbNotHtml;
        public WinLib.WatermaskRichTextBox txtLength;




    }
}
