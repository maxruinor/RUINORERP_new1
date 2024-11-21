namespace RUINORERP.UI.UserCenter.DataParts
{
    partial class UCSaleCell
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.kryptonHeaderGroup1 = new Krypton.Toolkit.KryptonHeaderGroup();
            this.buttonSpecHeaderGroup1 = new Krypton.Toolkit.ButtonSpecHeaderGroup();
            this.kryptonCommandRefresh = new Krypton.Toolkit.KryptonCommand();
            this.buttonSpecHeaderGroup2 = new Krypton.Toolkit.ButtonSpecHeaderGroup();
            this.kryptonPanelCell = new Krypton.Toolkit.KryptonPanel();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1.Panel)).BeginInit();
            this.kryptonHeaderGroup1.Panel.SuspendLayout();
            this.kryptonHeaderGroup1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelCell)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // kryptonHeaderGroup1
            // 
            this.kryptonHeaderGroup1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            this.kryptonHeaderGroup1.ButtonSpecs.Add(this.buttonSpecHeaderGroup1);
            this.kryptonHeaderGroup1.ButtonSpecs.Add(this.buttonSpecHeaderGroup2);
            this.kryptonHeaderGroup1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonHeaderGroup1.Location = new System.Drawing.Point(0, 0);
            this.kryptonHeaderGroup1.Name = "kryptonHeaderGroup1";
            // 
            // kryptonHeaderGroup1.Panel
            // 
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.kryptonPanelCell);
            this.kryptonHeaderGroup1.Size = new System.Drawing.Size(1200, 353);
            this.kryptonHeaderGroup1.TabIndex = 0;
            this.kryptonHeaderGroup1.ValuesPrimary.Heading = "销售";
            // 
            // buttonSpecHeaderGroup1
            // 
            this.buttonSpecHeaderGroup1.Enabled = Krypton.Toolkit.ButtonEnabled.True;
            this.buttonSpecHeaderGroup1.KryptonCommand = this.kryptonCommandRefresh;
            this.buttonSpecHeaderGroup1.UniqueName = "8303ef0a912e4bd8ab0de4162c460d75";
            // 
            // kryptonCommandRefresh
            // 
            this.kryptonCommandRefresh.AssignedButtonSpec = this.buttonSpecHeaderGroup1;
            this.kryptonCommandRefresh.Text = "刷新";
            this.kryptonCommandRefresh.Execute += new System.EventHandler(this.kryptonCommandRefresh_Execute);
            // 
            // buttonSpecHeaderGroup2
            // 
            this.buttonSpecHeaderGroup2.Image = global::RUINORERP.UI.Properties.Resources._6372970_about_detail_faq_help_info_icon;
            this.buttonSpecHeaderGroup2.ToolTipStyle = Krypton.Toolkit.LabelStyle.SuperTip;
            this.buttonSpecHeaderGroup2.ToolTipTitle = "更多详情";
            this.buttonSpecHeaderGroup2.UniqueName = "dbb013541e5748e1a5001fc3b7bbcc3f";
            this.buttonSpecHeaderGroup2.Click += new System.EventHandler(this.buttonSpecHeaderGroup2_Click);
            // 
            // kryptonPanelCell
            // 
            this.kryptonPanelCell.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanelCell.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanelCell.Name = "kryptonPanelCell";
            this.kryptonPanelCell.Size = new System.Drawing.Size(1198, 300);
            this.kryptonPanelCell.TabIndex = 0;
            // 
            // UCSaleCell
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.kryptonHeaderGroup1);
            this.Name = "UCSaleCell";
            this.Size = new System.Drawing.Size(1200, 353);
            this.Load += new System.EventHandler(this.UCPURCell_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1.Panel)).EndInit();
            this.kryptonHeaderGroup1.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1)).EndInit();
            this.kryptonHeaderGroup1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelCell)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonHeaderGroup kryptonHeaderGroup1;
        private Krypton.Toolkit.KryptonPanel kryptonPanelCell;
        private System.Windows.Forms.Timer timer1;
        private Krypton.Toolkit.KryptonCommand kryptonCommandRefresh;
        private Krypton.Toolkit.ButtonSpecHeaderGroup buttonSpecHeaderGroup1;
        private Krypton.Toolkit.ButtonSpecHeaderGroup buttonSpecHeaderGroup2;
    }
}
