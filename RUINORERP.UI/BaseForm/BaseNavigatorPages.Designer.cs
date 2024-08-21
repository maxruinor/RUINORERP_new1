namespace RUINORERP.UI.BaseForm
{
    partial class BaseNavigatorPages<M,C>
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
            this.kryptonPanelMain = new Krypton.Toolkit.KryptonPanel();
            this.kryptonGroup中间 = new Krypton.Toolkit.KryptonGroup();
            this.kryptonWorkspace1 = new Krypton.Workspace.KryptonWorkspace();
            this.kryptonHeaderGroupTop = new Krypton.Toolkit.KryptonHeaderGroup();
            this.buttonSpecHeaderGroup1 = new Krypton.Toolkit.ButtonSpecHeaderGroup();
            this.kryptonPanelQuery = new Krypton.Toolkit.KryptonPanel();
            this.groupLine1 = new WinLib.Line.GroupLine();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.kryptonPanelBig = new Krypton.Toolkit.KryptonPanel();
            this.kryptonSplitContainerMain = new Krypton.Toolkit.KryptonSplitContainer();
            this.kryptonHeaderGroupLeft = new Krypton.Toolkit.KryptonHeaderGroup();
            this.buttonSpecHeaderGroup2 = new Krypton.Toolkit.ButtonSpecHeaderGroup();
            this.kryptonTreeViewMenu = new Krypton.Toolkit.KryptonTreeView();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelMain)).BeginInit();
            this.kryptonPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup中间)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup中间.Panel)).BeginInit();
            this.kryptonGroup中间.Panel.SuspendLayout();
            this.kryptonGroup中间.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspace1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroupTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroupTop.Panel)).BeginInit();
            this.kryptonHeaderGroupTop.Panel.SuspendLayout();
            this.kryptonHeaderGroupTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelQuery)).BeginInit();
            this.kryptonPanelQuery.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelBig)).BeginInit();
            this.kryptonPanelBig.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainerMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainerMain.Panel1)).BeginInit();
            this.kryptonSplitContainerMain.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainerMain.Panel2)).BeginInit();
            this.kryptonSplitContainerMain.Panel2.SuspendLayout();
            this.kryptonSplitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroupLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroupLeft.Panel)).BeginInit();
            this.kryptonHeaderGroupLeft.Panel.SuspendLayout();
            this.kryptonHeaderGroupLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonPanelMain
            // 
            this.kryptonPanelMain.Controls.Add(this.kryptonGroup中间);
            this.kryptonPanelMain.Controls.Add(this.kryptonHeaderGroupTop);
            this.kryptonPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanelMain.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanelMain.Name = "kryptonPanelMain";
            this.kryptonPanelMain.Size = new System.Drawing.Size(757, 666);
            this.kryptonPanelMain.TabIndex = 7;
            // 
            // kryptonGroup中间
            // 
            this.kryptonGroup中间.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonGroup中间.Location = new System.Drawing.Point(0, 145);
            this.kryptonGroup中间.Name = "kryptonGroup中间";
            // 
            // kryptonGroup中间.Panel
            // 
            this.kryptonGroup中间.Panel.Controls.Add(this.kryptonWorkspace1);
            this.kryptonGroup中间.Size = new System.Drawing.Size(757, 521);
            this.kryptonGroup中间.TabIndex = 2;
            // 
            // kryptonWorkspace1
            // 
            this.kryptonWorkspace1.ActivePage = null;
            this.kryptonWorkspace1.CompactFlags = ((Krypton.Workspace.CompactFlags)((((Krypton.Workspace.CompactFlags.RemoveEmptyCells | Krypton.Workspace.CompactFlags.RemoveEmptySequences) 
            | Krypton.Workspace.CompactFlags.PromoteLeafs) 
            | Krypton.Workspace.CompactFlags.AtLeastOneVisibleCell)));
            this.kryptonWorkspace1.ContainerBackStyle = Krypton.Toolkit.PaletteBackStyle.PanelClient;
            this.kryptonWorkspace1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonWorkspace1.Location = new System.Drawing.Point(0, 0);
            this.kryptonWorkspace1.Name = "kryptonWorkspace1";
            // 
            // 
            // 
            this.kryptonWorkspace1.Root.UniqueName = "82f0493ab5334d7285ddbb04c1fac0a8";
            this.kryptonWorkspace1.Root.WorkspaceControl = this.kryptonWorkspace1;
            this.kryptonWorkspace1.SeparatorStyle = Krypton.Toolkit.SeparatorStyle.LowProfile;
            this.kryptonWorkspace1.Size = new System.Drawing.Size(755, 519);
            this.kryptonWorkspace1.SplitterWidth = 5;
            this.kryptonWorkspace1.TabIndex = 0;
            this.kryptonWorkspace1.TabStop = true;
            this.kryptonWorkspace1.WorkspaceCellAdding += new System.EventHandler<Krypton.Workspace.WorkspaceCellEventArgs>(this.workspaceCellAdding);
            // 
            // kryptonHeaderGroupTop
            // 
            this.kryptonHeaderGroupTop.AutoSize = true;
            this.kryptonHeaderGroupTop.ButtonSpecs.Add(this.buttonSpecHeaderGroup1);
            this.kryptonHeaderGroupTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonHeaderGroupTop.HeaderVisibleSecondary = false;
            this.kryptonHeaderGroupTop.Location = new System.Drawing.Point(0, 0);
            this.kryptonHeaderGroupTop.Name = "kryptonHeaderGroupTop";
            // 
            // kryptonHeaderGroupTop.Panel
            // 
            this.kryptonHeaderGroupTop.Panel.Controls.Add(this.kryptonPanelQuery);
            this.kryptonHeaderGroupTop.Size = new System.Drawing.Size(757, 145);
            this.kryptonHeaderGroupTop.StateCommon.HeaderPrimary.Border.DrawBorders = ((Krypton.Toolkit.PaletteDrawBorders)((((Krypton.Toolkit.PaletteDrawBorders.Top | Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | Krypton.Toolkit.PaletteDrawBorders.Left) 
            | Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonHeaderGroupTop.TabIndex = 1;
            this.kryptonHeaderGroupTop.ValuesPrimary.Heading = "";
            this.kryptonHeaderGroupTop.ValuesPrimary.Image = global::RUINORERP.UI.Properties.Resources.searcher1;
            // 
            // buttonSpecHeaderGroup1
            // 
            this.buttonSpecHeaderGroup1.Type = Krypton.Toolkit.PaletteButtonSpecStyle.ArrowUp;
            this.buttonSpecHeaderGroup1.UniqueName = "6c9eda2b76ae4d0eb4649936669149f4";
            // 
            // kryptonPanelQuery
            // 
            this.kryptonPanelQuery.Controls.Add(this.groupLine1);
            this.kryptonPanelQuery.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonPanelQuery.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanelQuery.Name = "kryptonPanelQuery";
            this.kryptonPanelQuery.Size = new System.Drawing.Size(755, 119);
            this.kryptonPanelQuery.TabIndex = 1;
            // 
            // groupLine1
            // 
            this.groupLine1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupLine1.Location = new System.Drawing.Point(0, 118);
            this.groupLine1.Name = "groupLine1";
            this.groupLine1.Size = new System.Drawing.Size(755, 1);
            this.groupLine1.TabIndex = 2;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.statusStrip1.Location = new System.Drawing.Point(0, 691);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this.statusStrip1.Size = new System.Drawing.Size(934, 22);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // kryptonPanelBig
            // 
            this.kryptonPanelBig.Controls.Add(this.kryptonSplitContainerMain);
            this.kryptonPanelBig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanelBig.Location = new System.Drawing.Point(0, 25);
            this.kryptonPanelBig.Name = "kryptonPanelBig";
            this.kryptonPanelBig.Size = new System.Drawing.Size(934, 666);
            this.kryptonPanelBig.TabIndex = 9;
            // 
            // kryptonSplitContainerMain
            // 
            this.kryptonSplitContainerMain.Cursor = System.Windows.Forms.Cursors.Default;
            this.kryptonSplitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonSplitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.kryptonSplitContainerMain.Name = "kryptonSplitContainerMain";
            // 
            // kryptonSplitContainerMain.Panel1
            // 
            this.kryptonSplitContainerMain.Panel1.Controls.Add(this.kryptonHeaderGroupLeft);
            // 
            // kryptonSplitContainerMain.Panel2
            // 
            this.kryptonSplitContainerMain.Panel2.Controls.Add(this.kryptonPanelMain);
            this.kryptonSplitContainerMain.Size = new System.Drawing.Size(934, 666);
            this.kryptonSplitContainerMain.SplitterDistance = 172;
            this.kryptonSplitContainerMain.TabIndex = 0;
            // 
            // kryptonHeaderGroupLeft
            // 
            this.kryptonHeaderGroupLeft.ButtonSpecs.Add(this.buttonSpecHeaderGroup2);
            this.kryptonHeaderGroupLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonHeaderGroupLeft.Location = new System.Drawing.Point(0, 0);
            this.kryptonHeaderGroupLeft.Name = "kryptonHeaderGroupLeft";
            // 
            // kryptonHeaderGroupLeft.Panel
            // 
            this.kryptonHeaderGroupLeft.Panel.Controls.Add(this.kryptonTreeViewMenu);
            this.kryptonHeaderGroupLeft.Size = new System.Drawing.Size(172, 666);
            this.kryptonHeaderGroupLeft.TabIndex = 0;
            this.kryptonHeaderGroupLeft.ValuesPrimary.Heading = "导航菜单";
            this.kryptonHeaderGroupLeft.ValuesPrimary.Image = null;
            this.kryptonHeaderGroupLeft.ValuesSecondary.Heading = "选择操作菜单";
            // 
            // buttonSpecHeaderGroup2
            // 
            this.buttonSpecHeaderGroup2.Type = Krypton.Toolkit.PaletteButtonSpecStyle.ArrowLeft;
            this.buttonSpecHeaderGroup2.UniqueName = "22f3c630b57949c38b9fa13a88c74aa0";
            // 
            // kryptonTreeViewMenu
            // 
            this.kryptonTreeViewMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonTreeViewMenu.Location = new System.Drawing.Point(0, 0);
            this.kryptonTreeViewMenu.Name = "kryptonTreeViewMenu";
            this.kryptonTreeViewMenu.Size = new System.Drawing.Size(170, 613);
            this.kryptonTreeViewMenu.TabIndex = 0;
            this.kryptonTreeViewMenu.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.kryptonTreeViewMenu_AfterSelect);
            // 
            // BaseNavigatorPages
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonPanelBig);
            this.Controls.Add(this.statusStrip1);
            this.Name = "BaseNavigatorPages";
            this.Size = new System.Drawing.Size(934, 713);
            this.Load += new System.EventHandler(this.BaseNavigatorGeneric_Load);
            this.Controls.SetChildIndex(this.statusStrip1, 0);
            this.Controls.SetChildIndex(this.kryptonPanelBig, 0);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelMain)).EndInit();
            this.kryptonPanelMain.ResumeLayout(false);
            this.kryptonPanelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup中间.Panel)).EndInit();
            this.kryptonGroup中间.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup中间)).EndInit();
            this.kryptonGroup中间.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspace1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroupTop.Panel)).EndInit();
            this.kryptonHeaderGroupTop.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroupTop)).EndInit();
            this.kryptonHeaderGroupTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelQuery)).EndInit();
            this.kryptonPanelQuery.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelBig)).EndInit();
            this.kryptonPanelBig.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainerMain.Panel1)).EndInit();
            this.kryptonSplitContainerMain.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainerMain.Panel2)).EndInit();
            this.kryptonSplitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainerMain)).EndInit();
            this.kryptonSplitContainerMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroupLeft.Panel)).EndInit();
            this.kryptonHeaderGroupLeft.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroupLeft)).EndInit();
            this.kryptonHeaderGroupLeft.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public Krypton.Toolkit.KryptonPanel kryptonPanelMain;
        internal Krypton.Workspace.KryptonWorkspace kryptonWorkspace1;
        private Krypton.Toolkit.ButtonSpecHeaderGroup buttonSpecHeaderGroup1;
        internal Krypton.Toolkit.KryptonPanel kryptonPanelQuery;
        private Krypton.Toolkit.KryptonGroup kryptonGroup中间;
        private WinLib.Line.GroupLine groupLine1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private Krypton.Toolkit.KryptonPanel kryptonPanelBig;
        private Krypton.Toolkit.KryptonSplitContainer kryptonSplitContainerMain;
        private Krypton.Toolkit.KryptonHeaderGroup kryptonHeaderGroupLeft;
        private Krypton.Toolkit.ButtonSpecHeaderGroup buttonSpecHeaderGroup2;
        internal Krypton.Toolkit.KryptonHeaderGroup kryptonHeaderGroupTop;
        internal Krypton.Toolkit.KryptonTreeView kryptonTreeViewMenu;
    }
}
