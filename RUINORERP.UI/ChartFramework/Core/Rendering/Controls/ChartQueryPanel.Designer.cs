namespace RUINORERP.UI.ChartFramework.Core.Rendering.Controls
{
    partial class ChartQueryPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChartQueryPanel));
            this.kryptonPanelQuery = new Krypton.Toolkit.KryptonPanel();
            this.groupLine3 = new WinLib.Line.GroupLine();
            this.BaseToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripBtnExport = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripbtnProperty = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton12 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.最大行数 = new System.Windows.Forms.ToolStripLabel();
            this.txtMaxRow = new System.Windows.Forms.ToolStripTextBox();
            this.dtpStart = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lblDateTIme = new Krypton.Toolkit.KryptonLabel();
            this.dtpEnd = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lblCustomer_id = new Krypton.Toolkit.KryptonLabel();
            this.cmbRangeType = new Krypton.Toolkit.KryptonComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelQuery)).BeginInit();
            this.kryptonPanelQuery.SuspendLayout();
            this.BaseToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbRangeType)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonPanelQuery
            // 
            this.kryptonPanelQuery.Controls.Add(this.lblCustomer_id);
            this.kryptonPanelQuery.Controls.Add(this.cmbRangeType);
            this.kryptonPanelQuery.Controls.Add(this.dtpEnd);
            this.kryptonPanelQuery.Controls.Add(this.dtpStart);
            this.kryptonPanelQuery.Controls.Add(this.lblDateTIme);
            this.kryptonPanelQuery.Controls.Add(this.groupLine3);
            this.kryptonPanelQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanelQuery.Location = new System.Drawing.Point(0, 25);
            this.kryptonPanelQuery.Name = "kryptonPanelQuery";
            this.kryptonPanelQuery.Size = new System.Drawing.Size(622, 84);
            this.kryptonPanelQuery.TabIndex = 4;
            // 
            // groupLine3
            // 
            this.groupLine3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupLine3.Location = new System.Drawing.Point(0, 83);
            this.groupLine3.Name = "groupLine3";
            this.groupLine3.Size = new System.Drawing.Size(622, 1);
            this.groupLine3.TabIndex = 2;
            // 
            // BaseToolStrip
            // 
            this.BaseToolStrip.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BaseToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton4,
            this.toolStripSeparator2,
            this.toolStripBtnExport,
            this.toolStripSeparator4,
            this.toolStripbtnProperty,
            this.toolStripSeparator1,
            this.toolStripButton12,
            this.toolStripSeparator3,
            this.最大行数,
            this.txtMaxRow});
            this.BaseToolStrip.Location = new System.Drawing.Point(0, 0);
            this.BaseToolStrip.Name = "BaseToolStrip";
            this.BaseToolStrip.Size = new System.Drawing.Size(622, 25);
            this.BaseToolStrip.TabIndex = 5;
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
            // toolStripBtnExport
            // 
            this.toolStripBtnExport.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBtnExport.Image")));
            this.toolStripBtnExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnExport.Name = "toolStripBtnExport";
            this.toolStripBtnExport.Size = new System.Drawing.Size(53, 22);
            this.toolStripBtnExport.Text = "导出";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripbtnProperty
            // 
            this.toolStripbtnProperty.Image = ((System.Drawing.Image)(resources.GetObject("toolStripbtnProperty.Image")));
            this.toolStripbtnProperty.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripbtnProperty.Name = "toolStripbtnProperty";
            this.toolStripbtnProperty.Size = new System.Drawing.Size(53, 22);
            this.toolStripbtnProperty.Text = "属性";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton12
            // 
            this.toolStripButton12.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton12.Image")));
            this.toolStripButton12.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton12.Name = "toolStripButton12";
            this.toolStripButton12.Size = new System.Drawing.Size(53, 22);
            this.toolStripButton12.Text = "关闭";
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
            this.txtMaxRow.ReadOnly = true;
            this.txtMaxRow.Size = new System.Drawing.Size(100, 25);
            this.txtMaxRow.Text = "200";
            // 
            // dtpStart
            // 
            this.dtpStart.Enabled = false;
            this.dtpStart.Location = new System.Drawing.Point(111, 11);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.ShowCheckBox = true;
            this.dtpStart.ShowUpDown = true;
            this.dtpStart.Size = new System.Drawing.Size(157, 21);
            this.dtpStart.TabIndex = 148;
            // 
            // lblDateTIme
            // 
            this.lblDateTIme.Location = new System.Drawing.Point(25, 12);
            this.lblDateTIme.Name = "lblDateTIme";
            this.lblDateTIme.Size = new System.Drawing.Size(36, 20);
            this.lblDateTIme.TabIndex = 147;
            this.lblDateTIme.Values.Text = "日期";
            // 
            // dtpEnd
            // 
            this.dtpEnd.Enabled = false;
            this.dtpEnd.Location = new System.Drawing.Point(307, 11);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.ShowCheckBox = true;
            this.dtpEnd.ShowUpDown = true;
            this.dtpEnd.Size = new System.Drawing.Size(157, 21);
            this.dtpEnd.TabIndex = 149;
            // 
            // lblCustomer_id
            // 
            this.lblCustomer_id.Location = new System.Drawing.Point(25, 38);
            this.lblCustomer_id.Name = "lblCustomer_id";
            this.lblCustomer_id.Size = new System.Drawing.Size(36, 20);
            this.lblCustomer_id.TabIndex = 150;
            this.lblCustomer_id.Values.Text = "周期";
            // 
            // cmbRangeType
            // 
            this.cmbRangeType.DropDownWidth = 100;
            this.cmbRangeType.IntegralHeight = false;
            this.cmbRangeType.Location = new System.Drawing.Point(111, 38);
            this.cmbRangeType.Name = "cmbRangeType";
            this.cmbRangeType.Size = new System.Drawing.Size(157, 21);
            this.cmbRangeType.TabIndex = 151;
            // 
            // ChartQueryPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonPanelQuery);
            this.Controls.Add(this.BaseToolStrip);
            this.Name = "ChartQueryPanel";
            this.Size = new System.Drawing.Size(622, 109);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelQuery)).EndInit();
            this.kryptonPanelQuery.ResumeLayout(false);
            this.kryptonPanelQuery.PerformLayout();
            this.BaseToolStrip.ResumeLayout(false);
            this.BaseToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbRangeType)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal Krypton.Toolkit.KryptonPanel kryptonPanelQuery;
        private WinLib.Line.GroupLine groupLine3;
        internal System.Windows.Forms.ToolStrip BaseToolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripBtnExport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        public System.Windows.Forms.ToolStripButton toolStripbtnProperty;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton12;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel 最大行数;
        public System.Windows.Forms.ToolStripTextBox txtMaxRow;
        private Krypton.Toolkit.KryptonDateTimePicker dtpEnd;
        private Krypton.Toolkit.KryptonDateTimePicker dtpStart;
        private Krypton.Toolkit.KryptonLabel lblDateTIme;
        private Krypton.Toolkit.KryptonLabel lblCustomer_id;
        private Krypton.Toolkit.KryptonComboBox cmbRangeType;
    }
}
