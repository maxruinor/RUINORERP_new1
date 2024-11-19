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
            this.kryptonManager = new Krypton.Toolkit.KryptonManager(this.components);
            this.kryptonDockingManager1 = new Krypton.Docking.KryptonDockingManager();
            this.kryptonPanelMainBig = new Krypton.Toolkit.KryptonPanel();
            this.kryptonDockableWorkspaceQuery = new Krypton.Docking.KryptonDockableWorkspace();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelMainBig)).BeginInit();
            this.kryptonPanelMainBig.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonDockableWorkspaceQuery)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonManager
            // 
            this.kryptonManager.GlobalPaletteMode = Krypton.Toolkit.PaletteMode.Office2007Blue;
            // 
            // kryptonDockingManager1
            // 
            this.kryptonDockingManager1.DefaultCloseRequest = Krypton.Docking.DockingCloseRequest.RemovePageAndDispose;
            // 
            // kryptonPanelMainBig
            // 
            this.kryptonPanelMainBig.Controls.Add(this.kryptonDockableWorkspaceQuery);
            this.kryptonPanelMainBig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanelMainBig.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanelMainBig.Name = "kryptonPanelMainBig";
            this.kryptonPanelMainBig.Size = new System.Drawing.Size(840, 567);
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
            this.kryptonDockableWorkspaceQuery.Location = new System.Drawing.Point(0, 0);
            this.kryptonDockableWorkspaceQuery.Name = "kryptonDockableWorkspaceQuery";
            // 
            // 
            // 
            this.kryptonDockableWorkspaceQuery.Root.UniqueName = "1E691A582DF14E8443907289238B58BD";
            this.kryptonDockableWorkspaceQuery.Root.WorkspaceControl = this.kryptonDockableWorkspaceQuery;
            this.kryptonDockableWorkspaceQuery.SeparatorStyle = Krypton.Toolkit.SeparatorStyle.LowProfile;
            this.kryptonDockableWorkspaceQuery.ShowMaximizeButton = false;
            this.kryptonDockableWorkspaceQuery.Size = new System.Drawing.Size(840, 567);
            this.kryptonDockableWorkspaceQuery.SplitterWidth = 5;
            this.kryptonDockableWorkspaceQuery.TabIndex = 0;
            this.kryptonDockableWorkspaceQuery.TabStop = true;
            // 
            // UCWorkbenches
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonPanelMainBig);
            this.Name = "UCWorkbenches";
            this.Size = new System.Drawing.Size(840, 567);
            this.Load += new System.EventHandler(this.UCWorkbenches_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelMainBig)).EndInit();
            this.kryptonPanelMainBig.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonDockableWorkspaceQuery)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal Krypton.Toolkit.KryptonManager kryptonManager;
        internal Krypton.Docking.KryptonDockingManager kryptonDockingManager1;
        public Krypton.Toolkit.KryptonPanel kryptonPanelMainBig;
        public Krypton.Docking.KryptonDockableWorkspace kryptonDockableWorkspaceQuery;
    }
}
