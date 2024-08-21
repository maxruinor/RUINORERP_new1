namespace RUINORERP.UI.BaseForm
{
    partial class BaseNavigatorAnalysis<M,C>
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
            this.kryptonHeaderGroupTop = new Krypton.Toolkit.KryptonHeaderGroup();
            this.buttonSpecHeaderGroup1 = new Krypton.Toolkit.ButtonSpecHeaderGroup();
            this.kryptonPanelQuery = new Krypton.Toolkit.KryptonPanel();
            this.groupLine1 = new WinLib.Line.GroupLine();
            this.kryptonNavigator1 = new Krypton.Navigator.KryptonNavigator();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelMain)).BeginInit();
            this.kryptonPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup中间)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup中间.Panel)).BeginInit();
            this.kryptonGroup中间.Panel.SuspendLayout();
            this.kryptonGroup中间.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroupTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroupTop.Panel)).BeginInit();
            this.kryptonHeaderGroupTop.Panel.SuspendLayout();
            this.kryptonHeaderGroupTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelQuery)).BeginInit();
            this.kryptonPanelQuery.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonNavigator1)).BeginInit();
            this.kryptonNavigator1.SuspendLayout();
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
            // kryptonGroup中间
            // 
            this.kryptonGroup中间.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonGroup中间.Location = new System.Drawing.Point(0, 145);
            this.kryptonGroup中间.Name = "kryptonGroup中间";
            // 
            // kryptonGroup中间.Panel
            // 
            this.kryptonGroup中间.Panel.Controls.Add(this.kryptonNavigator1);
            this.kryptonGroup中间.Size = new System.Drawing.Size(934, 543);
            this.kryptonGroup中间.TabIndex = 2;
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
            // groupLine1
            // 
            this.groupLine1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupLine1.Location = new System.Drawing.Point(0, 118);
            this.groupLine1.Name = "groupLine1";
            this.groupLine1.Size = new System.Drawing.Size(932, 1);
            this.groupLine1.TabIndex = 2;
            // 
            // kryptonNavigator1
            // 
            this.kryptonNavigator1.ControlKryptonFormFeatures = false;
            this.kryptonNavigator1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonNavigator1.Location = new System.Drawing.Point(0, 0);
            this.kryptonNavigator1.Name = "kryptonNavigator1";
            this.kryptonNavigator1.NavigatorMode = Krypton.Navigator.NavigatorMode.BarTabGroup;
            this.kryptonNavigator1.Owner = null;
            this.kryptonNavigator1.PageBackStyle = Krypton.Toolkit.PaletteBackStyle.ControlClient;
            this.kryptonNavigator1.Size = new System.Drawing.Size(932, 541);
            this.kryptonNavigator1.TabIndex = 0;
            this.kryptonNavigator1.Text = "kryptonNavigator1";
            // 
            // BaseNavigatorAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonPanelMain);
            this.Name = "BaseNavigatorAnalysis";
            this.Size = new System.Drawing.Size(934, 713);
            this.Load += new System.EventHandler(this.BaseNavigatorGeneric_Load);
            this.Controls.SetChildIndex(this.kryptonPanelMain, 0);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelMain)).EndInit();
            this.kryptonPanelMain.ResumeLayout(false);
            this.kryptonPanelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup中间.Panel)).EndInit();
            this.kryptonGroup中间.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup中间)).EndInit();
            this.kryptonGroup中间.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroupTop.Panel)).EndInit();
            this.kryptonHeaderGroupTop.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroupTop)).EndInit();
            this.kryptonHeaderGroupTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelQuery)).EndInit();
            this.kryptonPanelQuery.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonNavigator1)).EndInit();
            this.kryptonNavigator1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public Krypton.Toolkit.KryptonPanel kryptonPanelMain;
        private Krypton.Toolkit.KryptonHeaderGroup kryptonHeaderGroupTop;
        private Krypton.Toolkit.ButtonSpecHeaderGroup buttonSpecHeaderGroup1;
        internal Krypton.Toolkit.KryptonPanel kryptonPanelQuery;
        private Krypton.Toolkit.KryptonGroup kryptonGroup中间;
        private WinLib.Line.GroupLine groupLine1;
        private Krypton.Navigator.KryptonNavigator kryptonNavigator1;
    }
}
