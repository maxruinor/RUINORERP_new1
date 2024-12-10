namespace RUINORERP.UI.CRM
{
    partial class UCCRMConfig
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
            this.btnOk = new Krypton.Toolkit.KryptonButton();
            this.btnCancel = new Krypton.Toolkit.KryptonButton();
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            this.lblCS_UseLeadsFunction = new Krypton.Toolkit.KryptonLabel();
            this.chkCS_UseLeadsFunction = new Krypton.Toolkit.KryptonCheckBox();
            this.lblCS_NewCustToLeadsCustDays = new Krypton.Toolkit.KryptonLabel();
            this.txtCS_NewCustToLeadsCustDays = new Krypton.Toolkit.KryptonTextBox();
            this.lblCS_SleepingCustomerDays = new Krypton.Toolkit.KryptonLabel();
            this.txtCS_SleepingCustomerDays = new Krypton.Toolkit.KryptonTextBox();
            this.lblCS_LostCustomersDays = new Krypton.Toolkit.KryptonLabel();
            this.txtCS_LostCustomersDays = new Krypton.Toolkit.KryptonTextBox();
            this.lblCS_ActiveCustomers = new Krypton.Toolkit.KryptonLabel();
            this.txtCS_ActiveCustomers = new Krypton.Toolkit.KryptonTextBox();
            this.lblLS_ConvCustHasFollowUpDays = new Krypton.Toolkit.KryptonLabel();
            this.txtLS_ConvCustHasFollowUpDays = new Krypton.Toolkit.KryptonTextBox();
            this.lblLS_ConvCustNoTransDays = new Krypton.Toolkit.KryptonLabel();
            this.txtLS_ConvCustNoTransDays = new Krypton.Toolkit.KryptonTextBox();
            this.lblLS_ConvCustLostDays = new Krypton.Toolkit.KryptonLabel();
            this.txtLS_ConvCustLostDays = new Krypton.Toolkit.KryptonTextBox();
            this.lblNoFollToPublicPoolDays = new Krypton.Toolkit.KryptonLabel();
            this.txtNoFollToPublicPoolDays = new Krypton.Toolkit.KryptonTextBox();
            this.lblCustomerNoOrderDays = new Krypton.Toolkit.KryptonLabel();
            this.txtCustomerNoOrderDays = new Krypton.Toolkit.KryptonTextBox();
            this.lblCustomerNoFollowUpDays = new Krypton.Toolkit.KryptonLabel();
            this.txtCustomerNoFollowUpDays = new Krypton.Toolkit.KryptonTextBox();
            this.lblCreated_at = new Krypton.Toolkit.KryptonLabel();
            this.dtpCreated_at = new Krypton.Toolkit.KryptonDateTimePicker();
            this.bindingSourceEdit = new System.Windows.Forms.BindingSource(this.components);
            this.errorProviderForAllInput = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(166, 340);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(285, 340);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.lblCS_UseLeadsFunction);
            this.kryptonPanel1.Controls.Add(this.chkCS_UseLeadsFunction);
            this.kryptonPanel1.Controls.Add(this.lblCS_NewCustToLeadsCustDays);
            this.kryptonPanel1.Controls.Add(this.txtCS_NewCustToLeadsCustDays);
            this.kryptonPanel1.Controls.Add(this.lblCS_SleepingCustomerDays);
            this.kryptonPanel1.Controls.Add(this.txtCS_SleepingCustomerDays);
            this.kryptonPanel1.Controls.Add(this.lblCS_LostCustomersDays);
            this.kryptonPanel1.Controls.Add(this.txtCS_LostCustomersDays);
            this.kryptonPanel1.Controls.Add(this.lblCS_ActiveCustomers);
            this.kryptonPanel1.Controls.Add(this.txtCS_ActiveCustomers);
            this.kryptonPanel1.Controls.Add(this.lblLS_ConvCustHasFollowUpDays);
            this.kryptonPanel1.Controls.Add(this.txtLS_ConvCustHasFollowUpDays);
            this.kryptonPanel1.Controls.Add(this.lblLS_ConvCustNoTransDays);
            this.kryptonPanel1.Controls.Add(this.txtLS_ConvCustNoTransDays);
            this.kryptonPanel1.Controls.Add(this.lblLS_ConvCustLostDays);
            this.kryptonPanel1.Controls.Add(this.txtLS_ConvCustLostDays);
            this.kryptonPanel1.Controls.Add(this.lblNoFollToPublicPoolDays);
            this.kryptonPanel1.Controls.Add(this.txtNoFollToPublicPoolDays);
            this.kryptonPanel1.Controls.Add(this.lblCustomerNoOrderDays);
            this.kryptonPanel1.Controls.Add(this.txtCustomerNoOrderDays);
            this.kryptonPanel1.Controls.Add(this.lblCustomerNoFollowUpDays);
            this.kryptonPanel1.Controls.Add(this.txtCustomerNoFollowUpDays);
            this.kryptonPanel1.Controls.Add(this.lblCreated_at);
            this.kryptonPanel1.Controls.Add(this.dtpCreated_at);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(481, 394);
            this.kryptonPanel1.TabIndex = 2;
            
            // 
            // lblCS_UseLeadsFunction
            // 
            this.lblCS_UseLeadsFunction.Location = new System.Drawing.Point(83, 13);
            this.lblCS_UseLeadsFunction.Name = "lblCS_UseLeadsFunction";
            this.lblCS_UseLeadsFunction.Size = new System.Drawing.Size(114, 20);
            this.lblCS_UseLeadsFunction.TabIndex = 16;
            this.lblCS_UseLeadsFunction.Values.Text = "是否使用线索功能";
            // 
            // chkCS_UseLeadsFunction
            // 
            this.chkCS_UseLeadsFunction.Location = new System.Drawing.Point(203, 15);
            this.chkCS_UseLeadsFunction.Name = "chkCS_UseLeadsFunction";
            this.chkCS_UseLeadsFunction.Size = new System.Drawing.Size(19, 13);
            this.chkCS_UseLeadsFunction.TabIndex = 17;
            this.chkCS_UseLeadsFunction.Values.Text = "";
            // 
            // lblCS_NewCustToLeadsCustDays
            // 
            this.lblCS_NewCustToLeadsCustDays.Location = new System.Drawing.Point(96, 40);
            this.lblCS_NewCustToLeadsCustDays.Name = "lblCS_NewCustToLeadsCustDays";
            this.lblCS_NewCustToLeadsCustDays.Size = new System.Drawing.Size(101, 20);
            this.lblCS_NewCustToLeadsCustDays.TabIndex = 18;
            this.lblCS_NewCustToLeadsCustDays.Values.Text = "新客转潜客天数";
            // 
            // txtCS_NewCustToLeadsCustDays
            // 
            this.txtCS_NewCustToLeadsCustDays.Location = new System.Drawing.Point(203, 34);
            this.txtCS_NewCustToLeadsCustDays.Name = "txtCS_NewCustToLeadsCustDays";
            this.txtCS_NewCustToLeadsCustDays.Size = new System.Drawing.Size(172, 23);
            this.txtCS_NewCustToLeadsCustDays.TabIndex = 19;
            // 
            // lblCS_SleepingCustomerDays
            // 
            this.lblCS_SleepingCustomerDays.Location = new System.Drawing.Point(83, 65);
            this.lblCS_SleepingCustomerDays.Name = "lblCS_SleepingCustomerDays";
            this.lblCS_SleepingCustomerDays.Size = new System.Drawing.Size(114, 20);
            this.lblCS_SleepingCustomerDays.TabIndex = 20;
            this.lblCS_SleepingCustomerDays.Values.Text = "定义休眠客户天数";
            // 
            // txtCS_SleepingCustomerDays
            // 
            this.txtCS_SleepingCustomerDays.Location = new System.Drawing.Point(203, 59);
            this.txtCS_SleepingCustomerDays.Name = "txtCS_SleepingCustomerDays";
            this.txtCS_SleepingCustomerDays.Size = new System.Drawing.Size(172, 23);
            this.txtCS_SleepingCustomerDays.TabIndex = 21;
            // 
            // lblCS_LostCustomersDays
            // 
            this.lblCS_LostCustomersDays.Location = new System.Drawing.Point(83, 90);
            this.lblCS_LostCustomersDays.Name = "lblCS_LostCustomersDays";
            this.lblCS_LostCustomersDays.Size = new System.Drawing.Size(114, 20);
            this.lblCS_LostCustomersDays.TabIndex = 22;
            this.lblCS_LostCustomersDays.Values.Text = "定义流失客户天数";
            // 
            // txtCS_LostCustomersDays
            // 
            this.txtCS_LostCustomersDays.Location = new System.Drawing.Point(203, 84);
            this.txtCS_LostCustomersDays.Name = "txtCS_LostCustomersDays";
            this.txtCS_LostCustomersDays.Size = new System.Drawing.Size(172, 23);
            this.txtCS_LostCustomersDays.TabIndex = 23;
            // 
            // lblCS_ActiveCustomers
            // 
            this.lblCS_ActiveCustomers.Location = new System.Drawing.Point(83, 115);
            this.lblCS_ActiveCustomers.Name = "lblCS_ActiveCustomers";
            this.lblCS_ActiveCustomers.Size = new System.Drawing.Size(114, 20);
            this.lblCS_ActiveCustomers.TabIndex = 24;
            this.lblCS_ActiveCustomers.Values.Text = "定义活跃客户天数";
            // 
            // txtCS_ActiveCustomers
            // 
            this.txtCS_ActiveCustomers.Location = new System.Drawing.Point(203, 109);
            this.txtCS_ActiveCustomers.Name = "txtCS_ActiveCustomers";
            this.txtCS_ActiveCustomers.Size = new System.Drawing.Size(172, 23);
            this.txtCS_ActiveCustomers.TabIndex = 25;
            // 
            // lblLS_ConvCustHasFollowUpDays
            // 
            this.lblLS_ConvCustHasFollowUpDays.Location = new System.Drawing.Point(44, 140);
            this.lblLS_ConvCustHasFollowUpDays.Name = "lblLS_ConvCustHasFollowUpDays";
            this.lblLS_ConvCustHasFollowUpDays.Size = new System.Drawing.Size(153, 20);
            this.lblLS_ConvCustHasFollowUpDays.TabIndex = 26;
            this.lblLS_ConvCustHasFollowUpDays.Values.Text = "转换为客户后有跟进天数";
            // 
            // txtLS_ConvCustHasFollowUpDays
            // 
            this.txtLS_ConvCustHasFollowUpDays.Location = new System.Drawing.Point(203, 134);
            this.txtLS_ConvCustHasFollowUpDays.Name = "txtLS_ConvCustHasFollowUpDays";
            this.txtLS_ConvCustHasFollowUpDays.Size = new System.Drawing.Size(172, 23);
            this.txtLS_ConvCustHasFollowUpDays.TabIndex = 27;
            // 
            // lblLS_ConvCustNoTransDays
            // 
            this.lblLS_ConvCustNoTransDays.Location = new System.Drawing.Point(44, 165);
            this.lblLS_ConvCustNoTransDays.Name = "lblLS_ConvCustNoTransDays";
            this.lblLS_ConvCustNoTransDays.Size = new System.Drawing.Size(153, 20);
            this.lblLS_ConvCustNoTransDays.TabIndex = 28;
            this.lblLS_ConvCustNoTransDays.Values.Text = "转换为客户后无成交天数";
            // 
            // txtLS_ConvCustNoTransDays
            // 
            this.txtLS_ConvCustNoTransDays.Location = new System.Drawing.Point(203, 159);
            this.txtLS_ConvCustNoTransDays.Name = "txtLS_ConvCustNoTransDays";
            this.txtLS_ConvCustNoTransDays.Size = new System.Drawing.Size(172, 23);
            this.txtLS_ConvCustNoTransDays.TabIndex = 29;
            // 
            // lblLS_ConvCustLostDays
            // 
            this.lblLS_ConvCustLostDays.Location = new System.Drawing.Point(44, 190);
            this.lblLS_ConvCustLostDays.Name = "lblLS_ConvCustLostDays";
            this.lblLS_ConvCustLostDays.Size = new System.Drawing.Size(153, 20);
            this.lblLS_ConvCustLostDays.TabIndex = 31;
            this.lblLS_ConvCustLostDays.Values.Text = "转换为客户后已丢失天数";
            // 
            // txtLS_ConvCustLostDays
            // 
            this.txtLS_ConvCustLostDays.Location = new System.Drawing.Point(203, 184);
            this.txtLS_ConvCustLostDays.Name = "txtLS_ConvCustLostDays";
            this.txtLS_ConvCustLostDays.Size = new System.Drawing.Size(172, 23);
            this.txtLS_ConvCustLostDays.TabIndex = 30;
            // 
            // lblNoFollToPublicPoolDays
            // 
            this.lblNoFollToPublicPoolDays.Location = new System.Drawing.Point(44, 215);
            this.lblNoFollToPublicPoolDays.Name = "lblNoFollToPublicPoolDays";
            this.lblNoFollToPublicPoolDays.Size = new System.Drawing.Size(153, 20);
            this.lblNoFollToPublicPoolDays.TabIndex = 32;
            this.lblNoFollToPublicPoolDays.Values.Text = "无跟进转换到公海的天数";
            // 
            // txtNoFollToPublicPoolDays
            // 
            this.txtNoFollToPublicPoolDays.Location = new System.Drawing.Point(203, 209);
            this.txtNoFollToPublicPoolDays.Name = "txtNoFollToPublicPoolDays";
            this.txtNoFollToPublicPoolDays.Size = new System.Drawing.Size(172, 23);
            this.txtNoFollToPublicPoolDays.TabIndex = 33;
            // 
            // lblCustomerNoOrderDays
            // 
            this.lblCustomerNoOrderDays.Location = new System.Drawing.Point(44, 240);
            this.lblCustomerNoOrderDays.Name = "lblCustomerNoOrderDays";
            this.lblCustomerNoOrderDays.Size = new System.Drawing.Size(153, 20);
            this.lblCustomerNoOrderDays.TabIndex = 34;
            this.lblCustomerNoOrderDays.Values.Text = "客户无返单间隔提醒天数";
            // 
            // txtCustomerNoOrderDays
            // 
            this.txtCustomerNoOrderDays.Location = new System.Drawing.Point(203, 234);
            this.txtCustomerNoOrderDays.Name = "txtCustomerNoOrderDays";
            this.txtCustomerNoOrderDays.Size = new System.Drawing.Size(172, 23);
            this.txtCustomerNoOrderDays.TabIndex = 35;
            // 
            // lblCustomerNoFollowUpDays
            // 
            this.lblCustomerNoFollowUpDays.Location = new System.Drawing.Point(44, 265);
            this.lblCustomerNoFollowUpDays.Name = "lblCustomerNoFollowUpDays";
            this.lblCustomerNoFollowUpDays.Size = new System.Drawing.Size(153, 20);
            this.lblCustomerNoFollowUpDays.TabIndex = 36;
            this.lblCustomerNoFollowUpDays.Values.Text = "客户无回访间隔提醒天数";
            // 
            // txtCustomerNoFollowUpDays
            // 
            this.txtCustomerNoFollowUpDays.Location = new System.Drawing.Point(203, 259);
            this.txtCustomerNoFollowUpDays.Name = "txtCustomerNoFollowUpDays";
            this.txtCustomerNoFollowUpDays.Size = new System.Drawing.Size(172, 23);
            this.txtCustomerNoFollowUpDays.TabIndex = 37;
            // 
            // lblCreated_at
            // 
            this.lblCreated_at.Location = new System.Drawing.Point(135, 286);
            this.lblCreated_at.Name = "lblCreated_at";
            this.lblCreated_at.Size = new System.Drawing.Size(62, 20);
            this.lblCreated_at.TabIndex = 38;
            this.lblCreated_at.Values.Text = "创建时间";
            // 
            // dtpCreated_at
            // 
            this.dtpCreated_at.Location = new System.Drawing.Point(203, 286);
            this.dtpCreated_at.Name = "dtpCreated_at";
            this.dtpCreated_at.Size = new System.Drawing.Size(172, 21);
            this.dtpCreated_at.TabIndex = 39;
            // 
            // errorProviderForAllInput
            // 
            this.errorProviderForAllInput.ContainerControl = this;
            // 
            // UCCRMConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCCRMConfig";
            this.Size = new System.Drawing.Size(481, 394);
            this.Load += new System.EventHandler(this.UCLeadsEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonLabel lblCS_UseLeadsFunction;
        private Krypton.Toolkit.KryptonCheckBox chkCS_UseLeadsFunction;
        private Krypton.Toolkit.KryptonLabel lblCS_NewCustToLeadsCustDays;
        private Krypton.Toolkit.KryptonTextBox txtCS_NewCustToLeadsCustDays;
        private Krypton.Toolkit.KryptonLabel lblCS_SleepingCustomerDays;
        private Krypton.Toolkit.KryptonTextBox txtCS_SleepingCustomerDays;
        private Krypton.Toolkit.KryptonLabel lblCS_LostCustomersDays;
        private Krypton.Toolkit.KryptonTextBox txtCS_LostCustomersDays;
        private Krypton.Toolkit.KryptonLabel lblCS_ActiveCustomers;
        private Krypton.Toolkit.KryptonTextBox txtCS_ActiveCustomers;
        private Krypton.Toolkit.KryptonLabel lblLS_ConvCustHasFollowUpDays;
        private Krypton.Toolkit.KryptonTextBox txtLS_ConvCustHasFollowUpDays;
        private Krypton.Toolkit.KryptonLabel lblLS_ConvCustNoTransDays;
        private Krypton.Toolkit.KryptonTextBox txtLS_ConvCustNoTransDays;
        private Krypton.Toolkit.KryptonLabel lblLS_ConvCustLostDays;
        private Krypton.Toolkit.KryptonTextBox txtLS_ConvCustLostDays;
        private Krypton.Toolkit.KryptonLabel lblNoFollToPublicPoolDays;
        private Krypton.Toolkit.KryptonTextBox txtNoFollToPublicPoolDays;
        private Krypton.Toolkit.KryptonLabel lblCustomerNoOrderDays;
        private Krypton.Toolkit.KryptonTextBox txtCustomerNoOrderDays;
        private Krypton.Toolkit.KryptonLabel lblCustomerNoFollowUpDays;
        private Krypton.Toolkit.KryptonTextBox txtCustomerNoFollowUpDays;
        private Krypton.Toolkit.KryptonLabel lblCreated_at;
        private Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;
        internal System.Windows.Forms.BindingSource bindingSourceEdit;
        public System.Windows.Forms.ErrorProvider errorProviderForAllInput;
    }
}
