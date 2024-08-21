namespace RUINORERP.Assistant
{
    partial class MainForm
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
            this.btnPermissionInfoInit = new System.Windows.Forms.Button();
            this.btnMenuConfigure = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPermissionInfoInit
            // 
            this.btnPermissionInfoInit.Location = new System.Drawing.Point(12, 12);
            this.btnPermissionInfoInit.Name = "btnPermissionInfoInit";
            this.btnPermissionInfoInit.Size = new System.Drawing.Size(122, 56);
            this.btnPermissionInfoInit.TabIndex = 0;
            this.btnPermissionInfoInit.Text = "权限基础数据设置";
            this.btnPermissionInfoInit.UseVisualStyleBackColor = true;
            this.btnPermissionInfoInit.Click += new System.EventHandler(this.btnPermissionInfoInit_Click);
            // 
            // btnMenuConfigure
            // 
            this.btnMenuConfigure.Location = new System.Drawing.Point(12, 107);
            this.btnMenuConfigure.Name = "btnMenuConfigure";
            this.btnMenuConfigure.Size = new System.Drawing.Size(122, 52);
            this.btnMenuConfigure.TabIndex = 1;
            this.btnMenuConfigure.Text = "全局菜单配置";
            this.btnMenuConfigure.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnMenuConfigure);
            this.splitContainer1.Panel1.Controls.Add(this.btnPermissionInfoInit);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(975, 673);
            this.splitContainer1.SplitterDistance = 145;
            this.splitContainer1.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(826, 673);
            this.panel1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(975, 673);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MainForm";
            this.Text = "ERP系统工具";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPermissionInfoInit;
        private System.Windows.Forms.Button btnMenuConfigure;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
    }
}

