namespace RUINORERP.UI.UserCenter
{
    partial class UCWorkbenches
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCWorkbenches));
            this.kryptonManager = new Krypton.Toolkit.KryptonManager(this.components);
            this.kryptonPanelMainBig = new Krypton.Toolkit.KryptonPanel();
            this.kryptonDockableWorkspaceQuery = new Krypton.Docking.KryptonDockableWorkspace();
            this.BaseToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.kryptonContextMenu1 = new Krypton.Toolkit.KryptonContextMenu();
            this.kryptonDockingManager1 = new Krypton.Docking.KryptonDockingManager();
            this.btnReload = new System.Windows.Forms.ToolStripButton();
            this.btnSaveLayout = new System.Windows.Forms.ToolStripButton();
            this.toolStripbtnProperty = new System.Windows.Forms.ToolStripButton();
            this.btnLayout = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnGrid = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRow = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCol = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelMainBig)).BeginInit();
            this.kryptonPanelMainBig.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonDockableWorkspaceQuery)).BeginInit();
            this.BaseToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonManager
            // 
            this.kryptonManager.GlobalPaletteMode = Krypton.Toolkit.PaletteMode.Office2007Blue;
            // 
            // kryptonPanelMainBig
            // 
            this.kryptonPanelMainBig.Controls.Add(this.kryptonDockableWorkspaceQuery);
            this.kryptonPanelMainBig.Controls.Add(this.BaseToolStrip);
            this.kryptonPanelMainBig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanelMainBig.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanelMainBig.Name = "kryptonPanelMainBig";
            this.kryptonPanelMainBig.Size = new System.Drawing.Size(824, 528);
            this.kryptonPanelMainBig.TabIndex = 6;
            // 
            // kryptonDockableWorkspaceQuery
            // 
            this.kryptonDockableWorkspaceQuery.ActivePage = null;
            this.kryptonDockableWorkspaceQuery.AutoHiddenHost = false;
            this.kryptonDockableWorkspaceQuery.CompactFlags = ((Krypton.Workspace.CompactFlags)(((Krypton.Workspace.CompactFlags.RemoveEmptyCells | Krypton.Workspace.CompactFlags.RemoveEmptySequences) 
            | Krypton.Workspace.CompactFlags.PromoteLeafs)));
            this.kryptonDockableWorkspaceQuery.ContainerBackStyle = Krypton.Toolkit.PaletteBackStyle.PanelClient;
            this.kryptonDockableWorkspaceQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonDockableWorkspaceQuery.Location = new System.Drawing.Point(0, 25);
            this.kryptonDockableWorkspaceQuery.Name = "kryptonDockableWorkspaceQuery";
            // 
            // 
            // 
            this.kryptonDockableWorkspaceQuery.Root.UniqueName = "1E691A582DF14E8443907289238B58BD";
            this.kryptonDockableWorkspaceQuery.Root.WorkspaceControl = this.kryptonDockableWorkspaceQuery;
            this.kryptonDockableWorkspaceQuery.SeparatorStyle = Krypton.Toolkit.SeparatorStyle.LowProfile;
            this.kryptonDockableWorkspaceQuery.ShowMaximizeButton = false;
            this.kryptonDockableWorkspaceQuery.Size = new System.Drawing.Size(824, 503);
            this.kryptonDockableWorkspaceQuery.SplitterWidth = 5;
            this.kryptonDockableWorkspaceQuery.TabIndex = 0;
            this.kryptonDockableWorkspaceQuery.TabStop = true;
            // 
            // BaseToolStrip
            // 
            this.BaseToolStrip.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BaseToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnReload,
            this.toolStripSeparator2,
            this.btnSaveLayout,
            this.toolStripSeparator1,
            this.toolStripbtnProperty,
            this.btnLayout});
            this.BaseToolStrip.Location = new System.Drawing.Point(0, 0);
            this.BaseToolStrip.Name = "BaseToolStrip";
            this.BaseToolStrip.Size = new System.Drawing.Size(824, 25);
            this.BaseToolStrip.TabIndex = 4;
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
            // kryptonDockingManager1
            // 
            this.kryptonDockingManager1.DefaultCloseRequest = Krypton.Docking.DockingCloseRequest.RemovePageAndDispose;
            // 
            // btnReload
            // 
            this.btnReload.Image = global::RUINORERP.UI.Properties.Resources.reset;
            this.btnReload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(79, 22);
            this.btnReload.Text = "重置组件";
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // btnSaveLayout
            // 
            this.btnSaveLayout.Image = global::RUINORERP.UI.Properties.Resources.SaveLayout;
            this.btnSaveLayout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveLayout.Name = "btnSaveLayout";
            this.btnSaveLayout.Size = new System.Drawing.Size(79, 22);
            this.btnSaveLayout.Text = "保存布局";
            this.btnSaveLayout.Click += new System.EventHandler(this.btnSaveLayout_Click);
            // 
            // toolStripbtnProperty
            // 
            this.toolStripbtnProperty.Image = ((System.Drawing.Image)(resources.GetObject("toolStripbtnProperty.Image")));
            this.toolStripbtnProperty.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripbtnProperty.Name = "toolStripbtnProperty";
            this.toolStripbtnProperty.Size = new System.Drawing.Size(53, 22);
            this.toolStripbtnProperty.Text = "属性";
            this.toolStripbtnProperty.Click += new System.EventHandler(this.toolStripbtnProperty_Click);
            // 
            // btnLayout
            // 
            this.btnLayout.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnGrid,
            this.btnRow,
            this.btnCol});
            this.btnLayout.Image = global::RUINORERP.UI.Properties.Resources.Layout;
            this.btnLayout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLayout.Name = "btnLayout";
            this.btnLayout.Size = new System.Drawing.Size(62, 22);
            this.btnLayout.Text = "布局";
            this.btnLayout.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.btnLayout_DropDownItemClicked);
            // 
            // btnGrid
            // 
            this.btnGrid.Image = global::RUINORERP.UI.Properties.Resources.grid;
            this.btnGrid.Name = "btnGrid";
            this.btnGrid.Size = new System.Drawing.Size(180, 22);
            this.btnGrid.Text = "网格分布";
            // 
            // btnRow
            // 
            this.btnRow.Image = global::RUINORERP.UI.Properties.Resources.row;
            this.btnRow.Name = "btnRow";
            this.btnRow.Size = new System.Drawing.Size(180, 22);
            this.btnRow.Text = "横向分布";
            // 
            // btnCol
            // 
            this.btnCol.Image = global::RUINORERP.UI.Properties.Resources.col;
            this.btnCol.Name = "btnCol";
            this.btnCol.Size = new System.Drawing.Size(180, 22);
            this.btnCol.Text = "纵向分布";
            // 
            // UCWorkbenches
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonPanelMainBig);
            this.Name = "UCWorkbenches";
            this.Size = new System.Drawing.Size(824, 528);
            this.Load += new System.EventHandler(this.UCWorkbenches_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelMainBig)).EndInit();
            this.kryptonPanelMainBig.ResumeLayout(false);
            this.kryptonPanelMainBig.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonDockableWorkspaceQuery)).EndInit();
            this.BaseToolStrip.ResumeLayout(false);
            this.BaseToolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Krypton.Toolkit.KryptonManager kryptonManager;
        internal Krypton.Docking.KryptonDockingManager kryptonDockingManager1;
        public Krypton.Toolkit.KryptonPanel kryptonPanelMainBig;
        public Krypton.Docking.KryptonDockableWorkspace kryptonDockableWorkspaceQuery;
        private Krypton.Toolkit.KryptonContextMenu kryptonContextMenu1;
        internal System.Windows.Forms.ToolStrip BaseToolStrip;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnReload;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        public System.Windows.Forms.ToolStripButton toolStripbtnProperty;
        private System.Windows.Forms.ToolStripDropDownButton btnLayout;
        private System.Windows.Forms.ToolStripButton btnSaveLayout;
        private System.Windows.Forms.ToolStripMenuItem btnGrid;
        private System.Windows.Forms.ToolStripMenuItem btnRow;
        private System.Windows.Forms.ToolStripMenuItem btnCol;
    }
}
