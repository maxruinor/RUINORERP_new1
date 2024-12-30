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
            this.btnEnDE = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.注册码生成器ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
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
            // btnEnDE
            // 
            this.btnEnDE.Location = new System.Drawing.Point(609, 49);
            this.btnEnDE.Name = "btnEnDE";
            this.btnEnDE.Size = new System.Drawing.Size(162, 36);
            this.btnEnDE.TabIndex = 2;
            this.btnEnDE.Text = "加密解密——新";
            this.btnEnDE.UseVisualStyleBackColor = true;
            this.btnEnDE.Click += new System.EventHandler(this.btnEnDE_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.注册码生成器ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(837, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 注册码生成器ToolStripMenuItem
            // 
            this.注册码生成器ToolStripMenuItem.Name = "注册码生成器ToolStripMenuItem";
            this.注册码生成器ToolStripMenuItem.Size = new System.Drawing.Size(97, 20);
            this.注册码生成器ToolStripMenuItem.Text = "注册码生成器";
            this.注册码生成器ToolStripMenuItem.Click += new System.EventHandler(this.注册码生成器ToolStripMenuItem_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(837, 664);
            this.Controls.Add(this.btnEnDE);
            this.Controls.Add(this.btn加密解密);
            this.Controls.Add(this.btn注册码);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.Text = "ERP系统工具";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn注册码;
        private System.Windows.Forms.Button btn加密解密;
        private System.Windows.Forms.Button btnEnDE;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 注册码生成器ToolStripMenuItem;
    }
}

