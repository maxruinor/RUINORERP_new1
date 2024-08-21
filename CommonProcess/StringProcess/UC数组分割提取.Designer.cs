namespace CommonProcess.StringProcess
{
    partial class UC数组分割提取
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
            this.label1 = new System.Windows.Forms.Label();
            this.txt分割字符 = new WinLib.WatermarkTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtGetIndex = new WinLib.WatermarkTextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 110;
            this.label1.Text = "分割字符";
            // 
            // txt分割字符
            // 
            this.txt分割字符.EmptyTextTip = "|，# 或字符串等";
            this.txt分割字符.Location = new System.Drawing.Point(126, 16);
            this.txt分割字符.Name = "txt分割字符";
            this.txt分割字符.Size = new System.Drawing.Size(215, 21);
            this.txt分割字符.TabIndex = 109;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 12);
            this.label2.TabIndex = 111;
            this.label2.Text = "提取数组序号(0起):";
            // 
            // txtGetIndex
            // 
            this.txtGetIndex.EmptyTextTip = "只能为数字";
            this.txtGetIndex.Location = new System.Drawing.Point(126, 68);
            this.txtGetIndex.Name = "txtGetIndex";
            this.txtGetIndex.Size = new System.Drawing.Size(215, 21);
            this.txtGetIndex.TabIndex = 112;
            this.txtGetIndex.Text = "0";
            // 
            // UC数组分割提取
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtGetIndex);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt分割字符);
            this.Name = "UC数组分割提取";
            this.Size = new System.Drawing.Size(435, 129);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public WinLib.WatermarkTextBox txt分割字符;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public WinLib.WatermarkTextBox txtGetIndex;
    }
}
