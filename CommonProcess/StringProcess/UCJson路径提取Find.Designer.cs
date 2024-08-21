namespace CommonProcess.StringProcess
{
    partial class UCJson路径提取Find
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
            this.chkisJson格式化 = new System.Windows.Forms.CheckBox();
            this.btnSetJsonPath = new System.Windows.Forms.Button();
            this.txtJsonPickUPPath = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chkisJson格式化
            // 
            this.chkisJson格式化.AutoSize = true;
            this.chkisJson格式化.Location = new System.Drawing.Point(114, 85);
            this.chkisJson格式化.Name = "chkisJson格式化";
            this.chkisJson格式化.Size = new System.Drawing.Size(144, 16);
            this.chkisJson格式化.TabIndex = 107;
            this.chkisJson格式化.Text = "是否Json格式化后提取";
            this.chkisJson格式化.UseVisualStyleBackColor = true;
            // 
            // btnSetJsonPath
            // 
            this.btnSetJsonPath.Location = new System.Drawing.Point(9, 3);
            this.btnSetJsonPath.Name = "btnSetJsonPath";
            this.btnSetJsonPath.Size = new System.Drawing.Size(97, 23);
            this.btnSetJsonPath.TabIndex = 106;
            this.btnSetJsonPath.Text = "设置JSON路径";
            this.btnSetJsonPath.UseVisualStyleBackColor = true;
            this.btnSetJsonPath.Click += new System.EventHandler(this.btnSetJsonPath_Click);
            // 
            // txtJsonPickUPPath
            // 
            this.txtJsonPickUPPath.Location = new System.Drawing.Point(114, 35);
            this.txtJsonPickUPPath.Name = "txtJsonPickUPPath";
            this.txtJsonPickUPPath.Size = new System.Drawing.Size(204, 21);
            this.txtJsonPickUPPath.TabIndex = 105;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 38);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 12);
            this.label7.TabIndex = 104;
            this.label7.Text = "json提取的路径：";
            // 
            // UCJsonFind
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnSetJsonPath);
            this.Controls.Add(this.chkisJson格式化);
            this.Controls.Add(this.txtJsonPickUPPath);
            this.Controls.Add(this.label7);
            this.Name = "UCJsonFind";
            this.Size = new System.Drawing.Size(478, 154);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox chkisJson格式化;
        private System.Windows.Forms.Button btnSetJsonPath;
        private System.Windows.Forms.TextBox txtJsonPickUPPath;
        private System.Windows.Forms.Label label7;
    }
}
