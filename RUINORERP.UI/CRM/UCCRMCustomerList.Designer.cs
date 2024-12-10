namespace RUINORERP.UI.CRM
{
    partial class UCCRMCustomerList
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCCRMCustomerList));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加跟进计划ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加跟进记录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroupTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroupTop.Panel)).BeginInit();
            this.kryptonHeaderGroupTop.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonHeaderGroupTop
            // 
            this.kryptonHeaderGroupTop.Size = new System.Drawing.Size(467, 85);
            this.kryptonHeaderGroupTop.ValuesPrimary.Heading = "";
            this.kryptonHeaderGroupTop.ValuesPrimary.Image = ((System.Drawing.Image)(resources.GetObject("kryptonHeaderGroupTop.ValuesPrimary.Image")));
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加跟进计划ToolStripMenuItem,
            this.添加跟进记录ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 70);
            // 
            // 添加跟进计划ToolStripMenuItem
            // 
            this.添加跟进计划ToolStripMenuItem.Name = "添加跟进计划ToolStripMenuItem";
            this.添加跟进计划ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.添加跟进计划ToolStripMenuItem.Text = "添加跟进计划";
            this.添加跟进计划ToolStripMenuItem.Click += new System.EventHandler(this.添加跟进计划ToolStripMenuItem_Click);
            // 
            // 添加跟进记录ToolStripMenuItem
            // 
            this.添加跟进记录ToolStripMenuItem.Name = "添加跟进记录ToolStripMenuItem";
            this.添加跟进记录ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.添加跟进记录ToolStripMenuItem.Text = "添加跟进记录";
            this.添加跟进记录ToolStripMenuItem.Click += new System.EventHandler(this.添加跟进记录ToolStripMenuItem_Click);
            // 
            // UCCRMCustomerList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "UCCRMCustomerList";
            this.Size = new System.Drawing.Size(467, 445);
            this.Load += new System.EventHandler(this.UCCRMCustomerList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroupTop.Panel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroupTop)).EndInit();
            this.kryptonHeaderGroupTop.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 添加跟进计划ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加跟进记录ToolStripMenuItem;
    }
}
