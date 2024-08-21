namespace CommonProcess.StringProcess
{
    partial class UCSmartPackaging
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
            this.lblDescForSmart = new System.Windows.Forms.Label();
            this.rdb手工指定 = new System.Windows.Forms.RadioButton();
            this.rdb智能分析 = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPackagingQty = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblDescForSmart
            // 
            this.lblDescForSmart.AutoSize = true;
            this.lblDescForSmart.Location = new System.Drawing.Point(140, 105);
            this.lblDescForSmart.Name = "lblDescForSmart";
            this.lblDescForSmart.Size = new System.Drawing.Size(137, 12);
            this.lblDescForSmart.TabIndex = 0;
            this.lblDescForSmart.Text = "基本原始价格，分档处理";
            // 
            // rdb手工指定
            // 
            this.rdb手工指定.AutoSize = true;
            this.rdb手工指定.Checked = true;
            this.rdb手工指定.Location = new System.Drawing.Point(17, 79);
            this.rdb手工指定.Name = "rdb手工指定";
            this.rdb手工指定.Size = new System.Drawing.Size(119, 16);
            this.rdb手工指定.TabIndex = 1;
            this.rdb手工指定.TabStop = true;
            this.rdb手工指定.Text = "手工指定打包倍数";
            this.rdb手工指定.UseVisualStyleBackColor = true;
            this.rdb手工指定.CheckedChanged += new System.EventHandler(this.rdb手工指定_CheckedChanged);
            // 
            // rdb智能分析
            // 
            this.rdb智能分析.AutoSize = true;
            this.rdb智能分析.Location = new System.Drawing.Point(17, 101);
            this.rdb智能分析.Name = "rdb智能分析";
            this.rdb智能分析.Size = new System.Drawing.Size(71, 16);
            this.rdb智能分析.TabIndex = 1;
            this.rdb智能分析.Text = "智能分析";
            this.rdb智能分析.UseVisualStyleBackColor = true;
            this.rdb智能分析.CheckedChanged += new System.EventHandler(this.rdb智能分析_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(16, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(323, 72);
            this.label2.TabIndex = 0;
            this.label2.Text = "将低价产品打包出售处理过程：\r\n------执行过程中，DataGridView中的数据行不能变动-----\r\n1）找出标题，描述数量关键部分。完成替换\r\n2）将" +
    "原始价格备份到其他字段\r\n3）基于原始价格进行指定或自动计算，得到最新价格\r\n4）检查所有输出项的正确性，最后才保存操作";
            // 
            // txtPackagingQty
            // 
            this.txtPackagingQty.Location = new System.Drawing.Point(142, 78);
            this.txtPackagingQty.Name = "txtPackagingQty";
            this.txtPackagingQty.Size = new System.Drawing.Size(100, 21);
            this.txtPackagingQty.TabIndex = 2;
            // 
            // UCSmartPackaging
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtPackagingQty);
            this.Controls.Add(this.rdb智能分析);
            this.Controls.Add(this.rdb手工指定);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblDescForSmart);
            this.Name = "UCSmartPackaging";
            this.Size = new System.Drawing.Size(424, 130);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDescForSmart;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox txtPackagingQty;
        public System.Windows.Forms.RadioButton rdb手工指定;
        public System.Windows.Forms.RadioButton rdb智能分析;
    }
}
