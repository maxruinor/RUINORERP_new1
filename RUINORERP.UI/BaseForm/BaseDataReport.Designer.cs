namespace RUINORERP.UI.BaseForm
{
    partial class BaseDataReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseDataReport));
            this.BaseToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.最大行数 = new System.Windows.Forms.ToolStripLabel();
            this.txtMaxRow = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnAdvQuery = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton13 = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnExport = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton12 = new System.Windows.Forms.ToolStripButton();
            this.toolStripbtnFunction = new System.Windows.Forms.ToolStripDropDownButton();
            this.复制性新增ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BasekryptonSplitContainer = new Krypton.Toolkit.KryptonSplitContainer();
            this.kryptonPanelQuery = new Krypton.Toolkit.KryptonPanel();
            this.kryptonPanelData = new Krypton.Toolkit.KryptonPanel();
            this.BaseToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BasekryptonSplitContainer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BasekryptonSplitContainer.Panel1)).BeginInit();
            this.BasekryptonSplitContainer.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BasekryptonSplitContainer.Panel2)).BeginInit();
            this.BasekryptonSplitContainer.Panel2.SuspendLayout();
            this.BasekryptonSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelQuery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelData)).BeginInit();
            this.SuspendLayout();
            // 
            // BaseToolStrip
            // 
            this.BaseToolStrip.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BaseToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton4,
            this.toolStripSeparator2,
            this.toolStripBtnAdvQuery,
            this.toolStripButton13,
            this.toolStripSeparator1,
            this.toolStripBtnExport,
            this.toolStripButton12,
            this.toolStripbtnFunction,
            this.toolStripSeparator3,
            this.最大行数,
            this.txtMaxRow});
            this.BaseToolStrip.Location = new System.Drawing.Point(0, 0);
            this.BaseToolStrip.Name = "BaseToolStrip";
            this.BaseToolStrip.Size = new System.Drawing.Size(964, 25);
            this.BaseToolStrip.TabIndex = 5;
            this.BaseToolStrip.Text = "toolStrip1";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Margin = new System.Windows.Forms.Padding(100, 0, 0, 0);
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // 最大行数
            // 
            this.最大行数.Name = "最大行数";
            this.最大行数.Size = new System.Drawing.Size(59, 22);
            this.最大行数.Text = "最大行数";
            // 
            // txtMaxRow
            // 
            this.txtMaxRow.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.txtMaxRow.Name = "txtMaxRow";
            this.txtMaxRow.Size = new System.Drawing.Size(100, 25);
            this.txtMaxRow.Text = "200";
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(53, 22);
            this.toolStripButton4.Text = "查询";
            // 
            // toolStripBtnAdvQuery
            // 
            this.toolStripBtnAdvQuery.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBtnAdvQuery.Image")));
            this.toolStripBtnAdvQuery.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnAdvQuery.Name = "toolStripBtnAdvQuery";
            this.toolStripBtnAdvQuery.Size = new System.Drawing.Size(79, 22);
            this.toolStripBtnAdvQuery.Text = "高级查询";
            this.toolStripBtnAdvQuery.Visible = false;
            // 
            // toolStripButton13
            // 
            this.toolStripButton13.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton13.Image")));
            this.toolStripButton13.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton13.Name = "toolStripButton13";
            this.toolStripButton13.Size = new System.Drawing.Size(53, 22);
            this.toolStripButton13.Text = "打印";
            // 
            // toolStripBtnExport
            // 
            this.toolStripBtnExport.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBtnExport.Image")));
            this.toolStripBtnExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnExport.Name = "toolStripBtnExport";
            this.toolStripBtnExport.Size = new System.Drawing.Size(53, 22);
            this.toolStripBtnExport.Text = "导出";
            // 
            // toolStripButton12
            // 
            this.toolStripButton12.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton12.Image")));
            this.toolStripButton12.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton12.Name = "toolStripButton12";
            this.toolStripButton12.Size = new System.Drawing.Size(53, 22);
            this.toolStripButton12.Text = "关闭";
            // 
            // toolStripbtnFunction
            // 
            this.toolStripbtnFunction.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.复制性新增ToolStripMenuItem});
            this.toolStripbtnFunction.Image = global::RUINORERP.UI.Properties.Resources.objectItem;
            this.toolStripbtnFunction.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripbtnFunction.Name = "toolStripbtnFunction";
            this.toolStripbtnFunction.Size = new System.Drawing.Size(62, 22);
            this.toolStripbtnFunction.Text = "功能";
            // 
            // 复制性新增ToolStripMenuItem
            // 
            this.复制性新增ToolStripMenuItem.Image = global::RUINORERP.UI.Properties.Resources.add;
            this.复制性新增ToolStripMenuItem.Name = "复制性新增ToolStripMenuItem";
            this.复制性新增ToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.复制性新增ToolStripMenuItem.Text = "复制性新增";
            // 
            // BasekryptonSplitContainer
            // 
            this.BasekryptonSplitContainer.Cursor = System.Windows.Forms.Cursors.Default;
            this.BasekryptonSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BasekryptonSplitContainer.Location = new System.Drawing.Point(0, 25);
            this.BasekryptonSplitContainer.Name = "BasekryptonSplitContainer";
            this.BasekryptonSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // BasekryptonSplitContainer.Panel1
            // 
            this.BasekryptonSplitContainer.Panel1.Controls.Add(this.kryptonPanelQuery);
            // 
            // BasekryptonSplitContainer.Panel2
            // 
            this.BasekryptonSplitContainer.Panel2.Controls.Add(this.kryptonPanelData);
            this.BasekryptonSplitContainer.Size = new System.Drawing.Size(964, 735);
            this.BasekryptonSplitContainer.SplitterDistance = 160;
            this.BasekryptonSplitContainer.TabIndex = 6;
            // 
            // kryptonPanelQuery
            // 
            this.kryptonPanelQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanelQuery.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanelQuery.Name = "kryptonPanelQuery";
            this.kryptonPanelQuery.Size = new System.Drawing.Size(964, 160);
            this.kryptonPanelQuery.TabIndex = 0;
            // 
            // kryptonPanelData
            // 
            this.kryptonPanelData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanelData.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanelData.Name = "kryptonPanelData";
            this.kryptonPanelData.Size = new System.Drawing.Size(964, 570);
            this.kryptonPanelData.TabIndex = 0;
            // 
            // BaseDataReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.BasekryptonSplitContainer);
            this.Controls.Add(this.BaseToolStrip);
            this.Name = "BaseDataReport";
            this.Size = new System.Drawing.Size(964, 760);
            this.BaseToolStrip.ResumeLayout(false);
            this.BaseToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BasekryptonSplitContainer.Panel1)).EndInit();
            this.BasekryptonSplitContainer.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BasekryptonSplitContainer.Panel2)).EndInit();
            this.BasekryptonSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BasekryptonSplitContainer)).EndInit();
            this.BasekryptonSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelQuery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ToolStrip BaseToolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        internal System.Windows.Forms.ToolStripButton toolStripBtnAdvQuery;
        private System.Windows.Forms.ToolStripButton toolStripButton13;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripBtnExport;
        private System.Windows.Forms.ToolStripButton toolStripButton12;
        private System.Windows.Forms.ToolStripDropDownButton toolStripbtnFunction;
        private System.Windows.Forms.ToolStripMenuItem 复制性新增ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel 最大行数;
        public System.Windows.Forms.ToolStripTextBox txtMaxRow;
        internal Krypton.Toolkit.KryptonSplitContainer BasekryptonSplitContainer;
        private Krypton.Toolkit.KryptonPanel kryptonPanelQuery;
        private Krypton.Toolkit.KryptonPanel kryptonPanelData;
    }
}
