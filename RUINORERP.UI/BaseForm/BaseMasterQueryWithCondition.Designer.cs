namespace RUINORERP.UI.BaseForm
{
    partial class BaseMasterQueryWithCondition<M>
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseMasterQueryWithCondition));
            this.BaseToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripBtnAdvQuery = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton13 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripBtnExport = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton12 = new System.Windows.Forms.ToolStripButton();
            this.toolStripbtnFunction = new System.Windows.Forms.ToolStripDropDownButton();
            this.复制性新增ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.最大行数 = new System.Windows.Forms.ToolStripLabel();
            this.txtMaxRow = new System.Windows.Forms.ToolStripTextBox();
            this.kryptonPanelQuery = new Krypton.Toolkit.KryptonPanel();
            this.kryptonDockableWorkspaceQuery = new Krypton.Docking.KryptonDockableWorkspace();
            this.kryptonDockingManagerQuery = new Krypton.Docking.KryptonDockingManager();
            this.BaseToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelQuery)).BeginInit();
            this.kryptonPanelQuery.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonDockableWorkspaceQuery)).BeginInit();
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
            this.BaseToolStrip.Size = new System.Drawing.Size(889, 25);
            this.BaseToolStrip.TabIndex = 3;
            this.BaseToolStrip.Text = "toolStrip1";
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(53, 22);
            this.toolStripButton4.Text = "查询";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
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
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
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
            // kryptonPanelQuery
            // 
            this.kryptonPanelQuery.Controls.Add(this.kryptonDockableWorkspaceQuery);
            this.kryptonPanelQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanelQuery.Location = new System.Drawing.Point(0, 25);
            this.kryptonPanelQuery.Name = "kryptonPanelQuery";
            this.kryptonPanelQuery.Size = new System.Drawing.Size(889, 696);
            this.kryptonPanelQuery.TabIndex = 4;
            // 
            // kryptonDockableWorkspaceQuery
            // 
            this.kryptonDockableWorkspaceQuery.ActivePage = null;
            this.kryptonDockableWorkspaceQuery.AutoHiddenHost = false;
            this.kryptonDockableWorkspaceQuery.CompactFlags = ((Krypton.Workspace.CompactFlags)(((Krypton.Workspace.CompactFlags.RemoveEmptyCells | Krypton.Workspace.CompactFlags.RemoveEmptySequences) 
            | Krypton.Workspace.CompactFlags.PromoteLeafs)));
            this.kryptonDockableWorkspaceQuery.ContainerBackStyle = Krypton.Toolkit.PaletteBackStyle.PanelClient;
            this.kryptonDockableWorkspaceQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonDockableWorkspaceQuery.Location = new System.Drawing.Point(0, 0);
            this.kryptonDockableWorkspaceQuery.Name = "kryptonDockableWorkspaceQuery";
            // 
            // 
            // 
            this.kryptonDockableWorkspaceQuery.Root.UniqueName = "1E691A582DF14E8443907289238B58BD";
            this.kryptonDockableWorkspaceQuery.Root.WorkspaceControl = this.kryptonDockableWorkspaceQuery;
            this.kryptonDockableWorkspaceQuery.SeparatorStyle = Krypton.Toolkit.SeparatorStyle.LowProfile;
            this.kryptonDockableWorkspaceQuery.ShowMaximizeButton = false;
            this.kryptonDockableWorkspaceQuery.Size = new System.Drawing.Size(889, 696);
            this.kryptonDockableWorkspaceQuery.SplitterWidth = 5;
            this.kryptonDockableWorkspaceQuery.TabIndex = 0;
            this.kryptonDockableWorkspaceQuery.TabStop = true;
            // 
            // BaseMasterQueryWithCondition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonPanelQuery);
            this.Controls.Add(this.BaseToolStrip);
            this.Name = "BaseMasterQueryWithCondition";
            this.Size = new System.Drawing.Size(889, 721);
            this.Load += new System.EventHandler(this.BaseBillQueryMC_Load);
            this.BaseToolStrip.ResumeLayout(false);
            this.BaseToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelQuery)).EndInit();
            this.kryptonPanelQuery.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonDockableWorkspaceQuery)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.ToolStrip BaseToolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        internal System.Windows.Forms.ToolStripButton toolStripBtnAdvQuery;
        private System.Windows.Forms.ToolStripButton toolStripButton13;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripBtnExport;
        private System.Windows.Forms.ToolStripButton toolStripButton12;
        private System.Windows.Forms.ToolStripDropDownButton toolStripbtnFunction;
        private System.Windows.Forms.ToolStripMenuItem 复制性新增ToolStripMenuItem;
        private Krypton.Toolkit.KryptonPanel kryptonPanelQuery;
        private Krypton.Docking.KryptonDockableWorkspace kryptonDockableWorkspaceQuery;
        private Krypton.Docking.KryptonDockingManager kryptonDockingManagerQuery;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel 最大行数;
        public System.Windows.Forms.ToolStripTextBox txtMaxRow;
    }
}