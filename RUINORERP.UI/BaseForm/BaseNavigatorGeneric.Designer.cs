namespace RUINORERP.UI.BaseForm
{
    partial class BaseNavigatorGeneric<M,C>
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
            this.kryptonWorkspace1 = new Krypton.Workspace.KryptonWorkspace();
            this.kryptonHeaderGroupTop = new Krypton.Toolkit.KryptonHeaderGroup();
            this.buttonSpecHeaderGroup1 = new Krypton.Toolkit.ButtonSpecHeaderGroup();
            this.kryptonPanelQuery = new Krypton.Toolkit.KryptonPanel();
            this.kryptonGroup中间 = new Krypton.Toolkit.KryptonGroup();
            this.groupLine1 = new WinLib.Line.GroupLine();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelMain)).BeginInit();
            this.kryptonPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspace1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroupTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroupTop.Panel)).BeginInit();
            this.kryptonHeaderGroupTop.Panel.SuspendLayout();
            this.kryptonHeaderGroupTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelQuery)).BeginInit();
            this.kryptonPanelQuery.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup中间)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup中间.Panel)).BeginInit();
            this.kryptonGroup中间.Panel.SuspendLayout();
            this.kryptonGroup中间.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonPanelMain
            // 
            this.kryptonPanelMain.Controls.Add(this.kryptonGroup中间);
            this.kryptonPanelMain.Controls.Add(this.kryptonHeaderGroupTop);
            this.kryptonPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanelMain.Location = new System.Drawing.Point(0, 25);
            this.kryptonPanelMain.Name = "kryptonPanelMain";
            this.kryptonPanelMain.Size = new System.Drawing.Size(934, 688);
            this.kryptonPanelMain.TabIndex = 7;
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
            this.kryptonWorkspace1.Size = new System.Drawing.Size(932, 541);
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
            this.kryptonHeaderGroupTop.Size = new System.Drawing.Size(934, 145);
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
            this.kryptonPanelQuery.Size = new System.Drawing.Size(932, 119);
            this.kryptonPanelQuery.TabIndex = 1;
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
            this.kryptonGroup中间.Size = new System.Drawing.Size(934, 543);
            this.kryptonGroup中间.TabIndex = 2;
            // 
            // groupLine1
            // 
            this.groupLine1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupLine1.Location = new System.Drawing.Point(0, 118);
            this.groupLine1.Name = "groupLine1";
            this.groupLine1.Size = new System.Drawing.Size(932, 1);
            this.groupLine1.TabIndex = 2;
            // 
            // BaseNavigatorGeneric
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonPanelMain);
            this.Name = "BaseNavigatorGeneric";
            this.Size = new System.Drawing.Size(934, 713);
            this.Load += new System.EventHandler(this.BaseNavigatorGeneric_Load);
            this.Controls.SetChildIndex(this.kryptonPanelMain, 0);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelMain)).EndInit();
            this.kryptonPanelMain.ResumeLayout(false);
            this.kryptonPanelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspace1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroupTop.Panel)).EndInit();
            this.kryptonHeaderGroupTop.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroupTop)).EndInit();
            this.kryptonHeaderGroupTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelQuery)).EndInit();
            this.kryptonPanelQuery.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup中间.Panel)).EndInit();
            this.kryptonGroup中间.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup中间)).EndInit();
            this.kryptonGroup中间.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public Krypton.Toolkit.KryptonPanel kryptonPanelMain;
        internal Krypton.Workspace.KryptonWorkspace kryptonWorkspace1;
        private Krypton.Toolkit.KryptonHeaderGroup kryptonHeaderGroupTop;
        private Krypton.Toolkit.ButtonSpecHeaderGroup buttonSpecHeaderGroup1;
        internal Krypton.Toolkit.KryptonPanel kryptonPanelQuery;
        private Krypton.Toolkit.KryptonGroup kryptonGroup中间;
        private WinLib.Line.GroupLine groupLine1;
    }
}
