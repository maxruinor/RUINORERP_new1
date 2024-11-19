namespace RUINORERP.SecurityTool
{
    partial class frmMain
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.btn注册码 = new System.Windows.Forms.Button();
            this.btn加密解密 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn注册码
            // 
            this.btn注册码.Location = new System.Drawing.Point(69, 49);
            this.btn注册码.Name = "btn注册码";
            this.btn注册码.Size = new System.Drawing.Size(75, 23);
            this.btn注册码.TabIndex = 0;
            this.btn注册码.Text = "注册码生成";
            this.btn注册码.UseVisualStyleBackColor = true;
            this.btn注册码.Click += new System.EventHandler(this.btn注册码_Click);
            // 
            // btn加密解密
            // 
            this.btn加密解密.Location = new System.Drawing.Point(69, 97);
            this.btn加密解密.Name = "btn加密解密";
            this.btn加密解密.Size = new System.Drawing.Size(75, 23);
            this.btn加密解密.TabIndex = 1;
            this.btn加密解密.Text = "加密解密";
            this.btn加密解密.UseVisualStyleBackColor = true;
            this.btn加密解密.Click += new System.EventHandler(this.btn加密解密_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(837, 664);
            this.Controls.Add(this.btn加密解密);
            this.Controls.Add(this.btn注册码);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.Text = "ERP系统工具";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn注册码;
        private System.Windows.Forms.Button btn加密解密;
    }
}

