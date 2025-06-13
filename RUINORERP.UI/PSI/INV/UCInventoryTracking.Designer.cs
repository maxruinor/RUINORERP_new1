namespace RUINORERP.UI.PSI.INV
{
    partial class UCInventoryTracking
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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.纵向库存跟踪ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.库存异常检测ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripTracker = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.导出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelMain)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStripTracker.SuspendLayout();
            this.SuspendLayout();
            // 
            // frm
            // 
            this.frm.Location = new System.Drawing.Point(130, 130);
            // 
            // kryptonPanelMain
            // 
            this.kryptonPanelMain.Size = new System.Drawing.Size(964, 562);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.纵向库存跟踪ToolStripMenuItem,
            this.库存异常检测ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 48);
            // 
            // 纵向库存跟踪ToolStripMenuItem
            // 
            this.纵向库存跟踪ToolStripMenuItem.Name = "纵向库存跟踪ToolStripMenuItem";
            this.纵向库存跟踪ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.纵向库存跟踪ToolStripMenuItem.Text = "纵向库存跟踪";
            this.纵向库存跟踪ToolStripMenuItem.Click += new System.EventHandler(this.纵向库存跟踪ToolStripMenuItem_Click);
            // 
            // 库存异常检测ToolStripMenuItem
            // 
            this.库存异常检测ToolStripMenuItem.Name = "库存异常检测ToolStripMenuItem";
            this.库存异常检测ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.库存异常检测ToolStripMenuItem.Text = "库存异常检测";
            this.库存异常检测ToolStripMenuItem.Click += new System.EventHandler(this.库存异常检测ToolStripMenuItem_Click);
            // 
            // contextMenuStripTracker
            // 
            this.contextMenuStripTracker.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuStripTracker.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导出ToolStripMenuItem});
            this.contextMenuStripTracker.Name = "contextMenuStrip1";
            this.contextMenuStripTracker.Size = new System.Drawing.Size(101, 26);
            // 
            // 导出ToolStripMenuItem
            // 
            this.导出ToolStripMenuItem.Name = "导出ToolStripMenuItem";
            this.导出ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.导出ToolStripMenuItem.Text = "导出";
            this.导出ToolStripMenuItem.Click += new System.EventHandler(this.导出ToolStripMenuItem_Click);
            // 
            // UCInventoryTracking
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "UCInventoryTracking";
            this.Size = new System.Drawing.Size(964, 587);
            this.Load += new System.EventHandler(this.UCInventoryTracking_Load);
            this.Controls.SetChildIndex(this.kryptonPanelMain, 0);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelMain)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStripTracker.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 纵向库存跟踪ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 库存异常检测ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTracker;
        private System.Windows.Forms.ToolStripMenuItem 导出ToolStripMenuItem;
    }
}
