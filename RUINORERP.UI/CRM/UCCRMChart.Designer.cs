namespace RUINORERP.UI.CRM
{
    partial class UCCRMChart
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
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            this.tabControlEx1 = new WinLib.TabControlEx();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.kryptonPanel条件生成容器 = new Krypton.Toolkit.KryptonPanel();
            this.cmbEmployee_ID = new Krypton.Toolkit.KryptonComboBox();
            this.lblEmployee = new Krypton.Toolkit.KryptonLabel();
            this.btnQuery = new Krypton.Toolkit.KryptonButton();
            this.dtpEnd = new Krypton.Toolkit.KryptonDateTimePicker();
            this.DtpStart = new Krypton.Toolkit.KryptonDateTimePicker();
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            this.lblStartDate = new Krypton.Toolkit.KryptonLabel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.cmbTimeType = new Krypton.Toolkit.KryptonComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            this.tabControlEx1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel条件生成容器)).BeginInit();
            this.kryptonPanel条件生成容器.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmployee_ID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTimeType)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.tabControlEx1);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(956, 638);
            this.kryptonPanel1.TabIndex = 3;
            // 
            // tabControlEx1
            // 
            this.tabControlEx1.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(79)))), ((int)(((byte)(125)))));
            this.tabControlEx1.Controls.Add(this.tabPage1);
            this.tabControlEx1.Controls.Add(this.tabPage2);
            this.tabControlEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlEx1.Location = new System.Drawing.Point(0, 0);
            this.tabControlEx1.Name = "tabControlEx1";
            this.tabControlEx1.SelectedIndex = 0;
            this.tabControlEx1.Size = new System.Drawing.Size(956, 638);
            this.tabControlEx1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.flowLayoutPanel1);
            this.tabPage1.Controls.Add(this.kryptonPanel条件生成容器);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(948, 608);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "业务效能";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 62);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(942, 543);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // kryptonPanel条件生成容器
            // 
            this.kryptonPanel条件生成容器.Controls.Add(this.cmbTimeType);
            this.kryptonPanel条件生成容器.Controls.Add(this.cmbEmployee_ID);
            this.kryptonPanel条件生成容器.Controls.Add(this.lblEmployee);
            this.kryptonPanel条件生成容器.Controls.Add(this.btnQuery);
            this.kryptonPanel条件生成容器.Controls.Add(this.dtpEnd);
            this.kryptonPanel条件生成容器.Controls.Add(this.DtpStart);
            this.kryptonPanel条件生成容器.Controls.Add(this.kryptonLabel1);
            this.kryptonPanel条件生成容器.Controls.Add(this.lblStartDate);
            this.kryptonPanel条件生成容器.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonPanel条件生成容器.Location = new System.Drawing.Point(3, 3);
            this.kryptonPanel条件生成容器.Name = "kryptonPanel条件生成容器";
            this.kryptonPanel条件生成容器.Size = new System.Drawing.Size(942, 59);
            this.kryptonPanel条件生成容器.TabIndex = 2;
            // 
            // cmbEmployee_ID
            // 
            this.cmbEmployee_ID.DropDownWidth = 100;
            this.cmbEmployee_ID.IntegralHeight = false;
            this.cmbEmployee_ID.Location = new System.Drawing.Point(657, 19);
            this.cmbEmployee_ID.Name = "cmbEmployee_ID";
            this.cmbEmployee_ID.Size = new System.Drawing.Size(131, 21);
            this.cmbEmployee_ID.TabIndex = 78;
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(602, 21);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(49, 20);
            this.lblEmployee.TabIndex = 77;
            this.lblEmployee.Values.Text = "责任人";
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(832, 15);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(90, 25);
            this.btnQuery.TabIndex = 76;
            this.btnQuery.Values.Text = "查询";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // dtpEnd
            // 
            this.dtpEnd.AlwaysActive = false;
            this.dtpEnd.CalendarTodayDate = new System.DateTime(((long)(0)));
            this.dtpEnd.Checked = false;
            this.dtpEnd.CustomNullText = "创建时间止";
            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEnd.Location = new System.Drawing.Point(255, 19);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(149, 21);
            this.dtpEnd.TabIndex = 75;
            // 
            // DtpStart
            // 
            this.DtpStart.AlwaysActive = false;
            this.DtpStart.CalendarTodayDate = new System.DateTime(((long)(0)));
            this.DtpStart.Checked = false;
            this.DtpStart.CustomNullText = "创建时间起";
            this.DtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpStart.Location = new System.Drawing.Point(73, 15);
            this.DtpStart.Name = "DtpStart";
            this.DtpStart.Size = new System.Drawing.Size(149, 21);
            this.DtpStart.TabIndex = 74;
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(230, 16);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(19, 20);
            this.kryptonLabel1.TabIndex = 73;
            this.kryptonLabel1.Values.Text = "~";
            // 
            // lblStartDate
            // 
            this.lblStartDate.Location = new System.Drawing.Point(5, 15);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(62, 20);
            this.lblStartDate.TabIndex = 73;
            this.lblStartDate.Values.Text = "创建时间";
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(948, 608);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "业绩转化";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // cmbTimeType
            // 
            this.cmbTimeType.DropDownWidth = 100;
            this.cmbTimeType.IntegralHeight = false;
            this.cmbTimeType.Location = new System.Drawing.Point(410, 19);
            this.cmbTimeType.Name = "cmbTimeType";
            this.cmbTimeType.Size = new System.Drawing.Size(90, 21);
            this.cmbTimeType.TabIndex = 79;
            // 
            // UCCRMChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCCRMChart";
            this.Size = new System.Drawing.Size(956, 638);
            this.Load += new System.EventHandler(this.UCCRMChart_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.tabControlEx1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel条件生成容器)).EndInit();
            this.kryptonPanel条件生成容器.ResumeLayout(false);
            this.kryptonPanel条件生成容器.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmployee_ID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTimeType)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private WinLib.TabControlEx tabControlEx1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        internal Krypton.Toolkit.KryptonPanel kryptonPanel条件生成容器;
        private Krypton.Toolkit.KryptonDateTimePicker dtpEnd;
        private Krypton.Toolkit.KryptonDateTimePicker DtpStart;
        private Krypton.Toolkit.KryptonLabel lblStartDate;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private Krypton.Toolkit.KryptonButton btnQuery;
        private Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;
        private Krypton.Toolkit.KryptonLabel lblEmployee;
        private Krypton.Toolkit.KryptonComboBox cmbTimeType;
    }
}
