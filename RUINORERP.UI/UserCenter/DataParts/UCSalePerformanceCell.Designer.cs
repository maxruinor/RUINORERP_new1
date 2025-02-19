namespace RUINORERP.UI.UserCenter.DataParts
{
    partial class UCSalePerformanceCell
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCSalePerformanceCell));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.kryptonHeaderGroup1 = new Krypton.Toolkit.KryptonHeaderGroup();
            this.buttonSpecHeaderGroup1 = new Krypton.Toolkit.ButtonSpecHeaderGroup();
            this.kryptonCommandRefresh = new Krypton.Toolkit.KryptonCommand();
            this.kryptonPanelSaleMain = new Krypton.Toolkit.KryptonPanel();
            this.lblMonthlyplans = new Krypton.Toolkit.KryptonLabel();
            this.lblMonthly潜客数 = new Krypton.Toolkit.KryptonLabel();
            this.lblMonthlyFollowupRecords = new Krypton.Toolkit.KryptonLabel();
            this.lblMonthlySalePerformance = new Krypton.Toolkit.KryptonLabel();
            this.lblMonthly商机 = new Krypton.Toolkit.KryptonLabel();
            this.lblMonthlyCustomer = new Krypton.Toolkit.KryptonLabel();
            this.lblMonthlyOrderPerformance = new Krypton.Toolkit.KryptonLabel();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1.Panel)).BeginInit();
            this.kryptonHeaderGroup1.Panel.SuspendLayout();
            this.kryptonHeaderGroup1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelSaleMain)).BeginInit();
            this.kryptonPanelSaleMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // kryptonHeaderGroup1
            // 
            this.kryptonHeaderGroup1.ButtonSpecs.Add(this.buttonSpecHeaderGroup1);
            this.kryptonHeaderGroup1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonHeaderGroup1.Location = new System.Drawing.Point(0, 0);
            this.kryptonHeaderGroup1.Name = "kryptonHeaderGroup1";
            // 
            // kryptonHeaderGroup1.Panel
            // 
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.kryptonPanelSaleMain);
            this.kryptonHeaderGroup1.Size = new System.Drawing.Size(483, 326);
            this.kryptonHeaderGroup1.TabIndex = 0;
            this.kryptonHeaderGroup1.ValuesPrimary.Heading = "销售情况概览";
            // 
            // buttonSpecHeaderGroup1
            // 
            this.buttonSpecHeaderGroup1.Enabled = Krypton.Toolkit.ButtonEnabled.True;
            this.buttonSpecHeaderGroup1.KryptonCommand = this.kryptonCommandRefresh;
            this.buttonSpecHeaderGroup1.Text = "2314234";
            this.buttonSpecHeaderGroup1.UniqueName = "2a2f048c9e8b44ddbec5fa6edfc938b8";
            // 
            // kryptonCommandRefresh
            // 
            this.kryptonCommandRefresh.AssignedButtonSpec = this.buttonSpecHeaderGroup1;
            this.kryptonCommandRefresh.Text = "刷新";
            this.kryptonCommandRefresh.Execute += new System.EventHandler(this.kryptonCommand1_Execute);
            // 
            // kryptonPanelSaleMain
            // 
            this.kryptonPanelSaleMain.Controls.Add(this.lblMonthlyplans);
            this.kryptonPanelSaleMain.Controls.Add(this.lblMonthly潜客数);
            this.kryptonPanelSaleMain.Controls.Add(this.lblMonthlyFollowupRecords);
            this.kryptonPanelSaleMain.Controls.Add(this.lblMonthlySalePerformance);
            this.kryptonPanelSaleMain.Controls.Add(this.lblMonthly商机);
            this.kryptonPanelSaleMain.Controls.Add(this.lblMonthlyCustomer);
            this.kryptonPanelSaleMain.Controls.Add(this.lblMonthlyOrderPerformance);
            this.kryptonPanelSaleMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanelSaleMain.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanelSaleMain.Name = "kryptonPanelSaleMain";
            this.kryptonPanelSaleMain.Size = new System.Drawing.Size(481, 273);
            this.kryptonPanelSaleMain.TabIndex = 0;
            // 
            // lblMonthlyplans
            // 
            this.lblMonthlyplans.LabelStyle = Krypton.Toolkit.LabelStyle.BoldControl;
            this.lblMonthlyplans.Location = new System.Drawing.Point(284, 216);
            this.lblMonthlyplans.Name = "lblMonthlyplans";
            this.lblMonthlyplans.Size = new System.Drawing.Size(38, 34);
            this.lblMonthlyplans.StateNormal.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lblMonthlyplans.StateNormal.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.lblMonthlyplans.TabIndex = 8;
            this.lblMonthlyplans.Values.Image = ((System.Drawing.Image)(resources.GetObject("lblMonthlyplans.Values.Image")));
            this.lblMonthlyplans.Values.Text = "";
            // 
            // lblMonthly潜客数
            // 
            this.lblMonthly潜客数.LabelStyle = Krypton.Toolkit.LabelStyle.BoldControl;
            this.lblMonthly潜客数.Location = new System.Drawing.Point(29, 160);
            this.lblMonthly潜客数.Name = "lblMonthly潜客数";
            this.lblMonthly潜客数.Size = new System.Drawing.Size(38, 34);
            this.lblMonthly潜客数.StateNormal.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lblMonthly潜客数.StateNormal.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.lblMonthly潜客数.TabIndex = 7;
            this.lblMonthly潜客数.Values.Image = ((System.Drawing.Image)(resources.GetObject("lblMonthly潜客数.Values.Image")));
            this.lblMonthly潜客数.Values.Text = "";
            // 
            // lblMonthlyFollowupRecords
            // 
            this.lblMonthlyFollowupRecords.LabelStyle = Krypton.Toolkit.LabelStyle.BoldControl;
            this.lblMonthlyFollowupRecords.Location = new System.Drawing.Point(29, 216);
            this.lblMonthlyFollowupRecords.Name = "lblMonthlyFollowupRecords";
            this.lblMonthlyFollowupRecords.Size = new System.Drawing.Size(38, 34);
            this.lblMonthlyFollowupRecords.StateNormal.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lblMonthlyFollowupRecords.StateNormal.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.lblMonthlyFollowupRecords.TabIndex = 6;
            this.lblMonthlyFollowupRecords.Values.Image = ((System.Drawing.Image)(resources.GetObject("lblMonthlyFollowupRecords.Values.Image")));
            this.lblMonthlyFollowupRecords.Values.Text = "";
            // 
            // lblMonthlySalePerformance
            // 
            this.lblMonthlySalePerformance.LabelStyle = Krypton.Toolkit.LabelStyle.BoldControl;
            this.lblMonthlySalePerformance.Location = new System.Drawing.Point(29, 47);
            this.lblMonthlySalePerformance.Name = "lblMonthlySalePerformance";
            this.lblMonthlySalePerformance.Size = new System.Drawing.Size(38, 34);
            this.lblMonthlySalePerformance.StateNormal.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lblMonthlySalePerformance.StateNormal.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.lblMonthlySalePerformance.TabIndex = 5;
            this.lblMonthlySalePerformance.ToolTipValues.Description = "订金金额减去了退货金额";
            this.lblMonthlySalePerformance.ToolTipValues.EnableToolTips = true;
            this.lblMonthlySalePerformance.ToolTipValues.Heading = "";
            this.lblMonthlySalePerformance.Values.Image = global::RUINORERP.UI.Properties.Resources.money1;
            this.lblMonthlySalePerformance.Values.Text = "";
            // 
            // lblMonthly商机
            // 
            this.lblMonthly商机.LabelStyle = Krypton.Toolkit.LabelStyle.BoldControl;
            this.lblMonthly商机.Location = new System.Drawing.Point(284, 160);
            this.lblMonthly商机.Name = "lblMonthly商机";
            this.lblMonthly商机.Size = new System.Drawing.Size(38, 34);
            this.lblMonthly商机.StateNormal.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lblMonthly商机.StateNormal.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.lblMonthly商机.TabIndex = 4;
            this.lblMonthly商机.Values.Image = ((System.Drawing.Image)(resources.GetObject("lblMonthly商机.Values.Image")));
            this.lblMonthly商机.Values.Text = "";
            // 
            // lblMonthlyCustomer
            // 
            this.lblMonthlyCustomer.LabelStyle = Krypton.Toolkit.LabelStyle.BoldControl;
            this.lblMonthlyCustomer.Location = new System.Drawing.Point(29, 97);
            this.lblMonthlyCustomer.Name = "lblMonthlyCustomer";
            this.lblMonthlyCustomer.Size = new System.Drawing.Size(38, 34);
            this.lblMonthlyCustomer.StateNormal.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lblMonthlyCustomer.StateNormal.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.lblMonthlyCustomer.TabIndex = 2;
            this.lblMonthlyCustomer.Values.Image = global::RUINORERP.UI.Properties.Resources.users;
            this.lblMonthlyCustomer.Values.Text = "";
            // 
            // lblMonthlyOrderPerformance
            // 
            this.lblMonthlyOrderPerformance.LabelStyle = Krypton.Toolkit.LabelStyle.BoldControl;
            this.lblMonthlyOrderPerformance.Location = new System.Drawing.Point(29, 6);
            this.lblMonthlyOrderPerformance.Name = "lblMonthlyOrderPerformance";
            this.lblMonthlyOrderPerformance.Size = new System.Drawing.Size(38, 34);
            this.lblMonthlyOrderPerformance.StateNormal.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lblMonthlyOrderPerformance.StateNormal.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.lblMonthlyOrderPerformance.TabIndex = 1;
            this.lblMonthlyOrderPerformance.ToolTipValues.Description = "订金金额减去了退货金额";
            this.lblMonthlyOrderPerformance.ToolTipValues.EnableToolTips = true;
            this.lblMonthlyOrderPerformance.ToolTipValues.Heading = "";
            this.lblMonthlyOrderPerformance.Values.Image = global::RUINORERP.UI.Properties.Resources.money1;
            this.lblMonthlyOrderPerformance.Values.Text = "";
            // 
            // UCSalePerformanceCell
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonHeaderGroup1);
            this.Name = "UCSalePerformanceCell";
            this.Size = new System.Drawing.Size(483, 326);
            this.Load += new System.EventHandler(this.UCSaleCell_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1.Panel)).EndInit();
            this.kryptonHeaderGroup1.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1)).EndInit();
            this.kryptonHeaderGroup1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelSaleMain)).EndInit();
            this.kryptonPanelSaleMain.ResumeLayout(false);
            this.kryptonPanelSaleMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonHeaderGroup kryptonHeaderGroup1;
        private Krypton.Toolkit.KryptonPanel kryptonPanelSaleMain;
        private System.Windows.Forms.Timer timer1;
        private Krypton.Toolkit.ButtonSpecHeaderGroup buttonSpecHeaderGroup1;
        private Krypton.Toolkit.KryptonCommand kryptonCommandRefresh;
        private Krypton.Toolkit.KryptonLabel lblMonthlyOrderPerformance;
        private Krypton.Toolkit.KryptonLabel lblMonthlyCustomer;
        private Krypton.Toolkit.KryptonLabel lblMonthly商机;
        private Krypton.Toolkit.KryptonLabel lblMonthlySalePerformance;
        private Krypton.Toolkit.KryptonLabel lblMonthlyFollowupRecords;
        private Krypton.Toolkit.KryptonLabel lblMonthly潜客数;
        private Krypton.Toolkit.KryptonLabel lblMonthlyplans;
    }
}
