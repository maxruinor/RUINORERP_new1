namespace RUINORERP.UI.UserPersonalized
{
    partial class UCGridColSetting
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
            this.lblisFixed = new Krypton.Toolkit.KryptonLabel();
            this.chkisFixed = new Krypton.Toolkit.KryptonCheckBox();
            this.lblVisble = new Krypton.Toolkit.KryptonLabel();
            this.chkVisble = new Krypton.Toolkit.KryptonCheckBox();
            this.trackBarColWidth = new System.Windows.Forms.TrackBar();
            this.lblColWidth = new Krypton.Toolkit.KryptonLabel();
            this.lblColCaption = new Krypton.Toolkit.KryptonLabel();
            this.txtColCaption = new Krypton.Toolkit.KryptonTextBox();
            this.lblColWidthShow = new Krypton.Toolkit.KryptonLabel();
            this.txtSort = new Krypton.Toolkit.KryptonNumericUpDown();
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarColWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // lblisFixed
            // 
            this.lblisFixed.Location = new System.Drawing.Point(18, 83);
            this.lblisFixed.Name = "lblisFixed";
            this.lblisFixed.Size = new System.Drawing.Size(49, 20);
            this.lblisFixed.TabIndex = 35;
            this.lblisFixed.Values.Text = "冻结列";
            // 
            // chkisFixed
            // 
            this.chkisFixed.Location = new System.Drawing.Point(73, 88);
            this.chkisFixed.Name = "chkisFixed";
            this.chkisFixed.Size = new System.Drawing.Size(19, 13);
            this.chkisFixed.TabIndex = 36;
            this.chkisFixed.ToolTipValues.Description = "打开查询页面，第一个获取焦点的输入框。";
            this.chkisFixed.ToolTipValues.Heading = "";
            this.chkisFixed.Values.Text = "";
            // 
            // lblVisble
            // 
            this.lblVisble.Location = new System.Drawing.Point(5, 57);
            this.lblVisble.Name = "lblVisble";
            this.lblVisble.Size = new System.Drawing.Size(62, 20);
            this.lblVisble.TabIndex = 39;
            this.lblVisble.Values.Text = "是否可见";
            // 
            // chkVisble
            // 
            this.chkVisble.Location = new System.Drawing.Point(73, 60);
            this.chkVisble.Name = "chkVisble";
            this.chkVisble.Size = new System.Drawing.Size(19, 13);
            this.chkVisble.TabIndex = 40;
            this.chkVisble.Values.Text = "";
            // 
            // trackBarColWidth
            // 
            this.trackBarColWidth.Location = new System.Drawing.Point(73, 117);
            this.trackBarColWidth.Maximum = 500;
            this.trackBarColWidth.Name = "trackBarColWidth";
            this.trackBarColWidth.Size = new System.Drawing.Size(469, 45);
            this.trackBarColWidth.TabIndex = 41;
            this.trackBarColWidth.TickFrequency = 3;
            this.trackBarColWidth.Value = 30;
            this.trackBarColWidth.Scroll += new System.EventHandler(this.trackBarColWidth_Scroll);
            // 
            // lblColWidth
            // 
            this.lblColWidth.Location = new System.Drawing.Point(31, 127);
            this.lblColWidth.Name = "lblColWidth";
            this.lblColWidth.Size = new System.Drawing.Size(36, 20);
            this.lblColWidth.TabIndex = 42;
            this.lblColWidth.Values.Text = "宽度";
            // 
            // lblColCaption
            // 
            this.lblColCaption.Location = new System.Drawing.Point(5, 22);
            this.lblColCaption.Name = "lblColCaption";
            this.lblColCaption.Size = new System.Drawing.Size(62, 20);
            this.lblColCaption.TabIndex = 43;
            this.lblColCaption.Values.Text = "列显示名";
            // 
            // txtColCaption
            // 
            this.txtColCaption.Location = new System.Drawing.Point(73, 22);
            this.txtColCaption.Name = "txtColCaption";
            this.txtColCaption.Size = new System.Drawing.Size(172, 23);
            this.txtColCaption.TabIndex = 44;
            // 
            // lblColWidthShow
            // 
            this.lblColWidthShow.Location = new System.Drawing.Point(73, 151);
            this.lblColWidthShow.Name = "lblColWidthShow";
            this.lblColWidthShow.Size = new System.Drawing.Size(6, 2);
            this.lblColWidthShow.TabIndex = 45;
            this.lblColWidthShow.Values.Text = "";
            // 
            // txtSort
            // 
            this.txtSort.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtSort.Location = new System.Drawing.Point(236, 57);
            this.txtSort.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.txtSort.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtSort.Name = "txtSort";
            this.txtSort.Size = new System.Drawing.Size(68, 22);
            this.txtSort.TabIndex = 46;
            this.txtSort.ToolTipValues.Description = "查询条件过多时，显示不完整，可以将数字调大。";
            this.txtSort.ToolTipValues.EnableToolTips = true;
            this.txtSort.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(191, 57);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(39, 20);
            this.kryptonLabel1.TabIndex = 47;
            this.kryptonLabel1.Values.Text = "排序:";
            // 
            // UCGridColSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(616, 183);
            this.ControlBox = false;
            this.Controls.Add(this.txtSort);
            this.Controls.Add(this.kryptonLabel1);
            this.Controls.Add(this.lblColWidthShow);
            this.Controls.Add(this.lblColCaption);
            this.Controls.Add(this.txtColCaption);
            this.Controls.Add(this.lblColWidth);
            this.Controls.Add(this.trackBarColWidth);
            this.Controls.Add(this.lblVisble);
            this.Controls.Add(this.chkVisble);
            this.Controls.Add(this.lblisFixed);
            this.Controls.Add(this.chkisFixed);
            this.Name = "UCGridColSetting";
            this.Leave += new System.EventHandler(this.UCQueryCondition_Leave);
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarColWidth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Krypton.Toolkit.KryptonLabel lblisFixed;
        private Krypton.Toolkit.KryptonCheckBox chkisFixed;
        private Krypton.Toolkit.KryptonLabel lblVisble;
        private Krypton.Toolkit.KryptonCheckBox chkVisble;
        private System.Windows.Forms.TrackBar trackBarColWidth;
        private Krypton.Toolkit.KryptonLabel lblColWidth;
        private Krypton.Toolkit.KryptonLabel lblColCaption;
        private Krypton.Toolkit.KryptonTextBox txtColCaption;
        private Krypton.Toolkit.KryptonLabel lblColWidthShow;
        public Krypton.Toolkit.KryptonNumericUpDown txtSort;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
    }
}
