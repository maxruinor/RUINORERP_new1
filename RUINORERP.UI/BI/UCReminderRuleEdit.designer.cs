namespace RUINORERP.UI.BI
{
    partial class UCReminderRuleEdit
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
            this.btnOk = new Krypton.Toolkit.KryptonButton();
            this.btnCancel = new Krypton.Toolkit.KryptonButton();
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            this.txtNotifyRecipientNames = new Krypton.Toolkit.KryptonCheckedListBox();
            this.clbNotifyChannels = new Krypton.Toolkit.KryptonCheckedListBox();
            this.btnSelectNotifyRecipients = new Krypton.Toolkit.KryptonButton();
            this.btnConfigParser = new Krypton.Toolkit.KryptonButton();
            this.cmbPriority = new Krypton.Toolkit.KryptonComboBox();
            this.cmbReminderBizType = new Krypton.Toolkit.KryptonComboBox();
            this.lblRuleName = new Krypton.Toolkit.KryptonLabel();
            this.txtRuleName = new Krypton.Toolkit.KryptonTextBox();
            this.lblRuleEngineType = new Krypton.Toolkit.KryptonLabel();
            this.lblDescription = new Krypton.Toolkit.KryptonLabel();
            this.txtDescription = new Krypton.Toolkit.KryptonTextBox();
            this.lblReminderBizType = new Krypton.Toolkit.KryptonLabel();
            this.lblPriority = new Krypton.Toolkit.KryptonLabel();
            this.chkIsEnabled = new Krypton.Toolkit.KryptonCheckBox();
            this.lblNotifyChannels = new Krypton.Toolkit.KryptonLabel();
            this.lblEffectiveDate = new Krypton.Toolkit.KryptonLabel();
            this.dtpEffectiveDate = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lblExpireDate = new Krypton.Toolkit.KryptonLabel();
            this.dtpExpireDate = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lblNotifyRecipients = new Krypton.Toolkit.KryptonLabel();
            this.lblJsonConfig = new Krypton.Toolkit.KryptonLabel();
            this.cmbRuleEngineType = new Krypton.Toolkit.KryptonComboBox();
            this.jsonViewer1 = new RUINORERP.UI.Monitoring.Auditing.JsonViewer();
            // 链路关联相关控件实例化
            this.linkGroupBox = new Krypton.Toolkit.KryptonGroupBox();
            this.btnAddLink = new Krypton.Toolkit.KryptonButton();
            this.btnRemoveLink = new Krypton.Toolkit.KryptonButton();
            this.dgvLinkedLinks = new Krypton.Toolkit.KryptonDataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPriority)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbReminderBizType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbRuleEngineType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.jsonViewer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.linkGroupBox.Panel)).BeginInit();
            this.linkGroupBox.Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(348, 766);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(466, 766);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.jsonViewer1);
            this.kryptonPanel1.Controls.Add(this.linkGroupBox);
            this.kryptonPanel1.Controls.Add(this.txtNotifyRecipientNames);
            this.kryptonPanel1.Controls.Add(this.clbNotifyChannels);
            this.kryptonPanel1.Controls.Add(this.btnSelectNotifyRecipients);
            this.kryptonPanel1.Controls.Add(this.btnConfigParser);
            this.kryptonPanel1.Controls.Add(this.cmbPriority);
            this.kryptonPanel1.Controls.Add(this.cmbReminderBizType);
            this.kryptonPanel1.Controls.Add(this.lblRuleName);
            this.kryptonPanel1.Controls.Add(this.txtRuleName);
            this.kryptonPanel1.Controls.Add(this.lblRuleEngineType);
            this.kryptonPanel1.Controls.Add(this.lblDescription);
            this.kryptonPanel1.Controls.Add(this.txtDescription);
            this.kryptonPanel1.Controls.Add(this.lblReminderBizType);
            this.kryptonPanel1.Controls.Add(this.lblPriority);
            this.kryptonPanel1.Controls.Add(this.chkIsEnabled);
            this.kryptonPanel1.Controls.Add(this.lblNotifyChannels);
            this.kryptonPanel1.Controls.Add(this.lblEffectiveDate);
            this.kryptonPanel1.Controls.Add(this.dtpEffectiveDate);
            this.kryptonPanel1.Controls.Add(this.lblExpireDate);
            this.kryptonPanel1.Controls.Add(this.dtpExpireDate);
            this.kryptonPanel1.Controls.Add(this.lblNotifyRecipients);
            this.kryptonPanel1.Controls.Add(this.lblJsonConfig);
            this.kryptonPanel1.Controls.Add(this.cmbRuleEngineType);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            
            // 添加链路关联控件到分组框的Panel
            this.linkGroupBox.Panel.Controls.Add(this.btnAddLink);
            this.linkGroupBox.Panel.Controls.Add(this.btnRemoveLink);
            this.linkGroupBox.Panel.Controls.Add(this.dgvLinkedLinks);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(911, 819);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // txtNotifyRecipientNames
            // 
            this.txtNotifyRecipientNames.Location = new System.Drawing.Point(143, 142);
            this.txtNotifyRecipientNames.Name = "txtNotifyRecipientNames";
            this.txtNotifyRecipientNames.Size = new System.Drawing.Size(259, 243);
            this.txtNotifyRecipientNames.TabIndex = 52;
            // 
            // clbNotifyChannels
            // 
            this.clbNotifyChannels.Location = new System.Drawing.Point(592, 141);
            this.clbNotifyChannels.Name = "clbNotifyChannels";
            this.clbNotifyChannels.Size = new System.Drawing.Size(180, 244);
            this.clbNotifyChannels.TabIndex = 51;
            // 
            // btnSelectNotifyRecipients
            // 
            this.btnSelectNotifyRecipients.Location = new System.Drawing.Point(420, 142);
            this.btnSelectNotifyRecipients.Name = "btnSelectNotifyRecipients";
            this.btnSelectNotifyRecipients.Size = new System.Drawing.Size(78, 25);
            this.btnSelectNotifyRecipients.TabIndex = 48;
            this.btnSelectNotifyRecipients.Values.Text = "选择接收人";
            this.btnSelectNotifyRecipients.Click += new System.EventHandler(this.btnSelectNotifyRecipients_Click);
            // 
            // btnConfigParser
            // 
            this.btnConfigParser.Location = new System.Drawing.Point(779, 394);
            this.btnConfigParser.Name = "btnConfigParser";
            this.btnConfigParser.Size = new System.Drawing.Size(87, 25);
            this.btnConfigParser.TabIndex = 46;
            this.btnConfigParser.Values.Text = "配置解析器";
            this.btnConfigParser.Click += new System.EventHandler(this.btnConfigParser_Click);
            // 
            // cmbPriority
            // 
            this.cmbPriority.DropDownWidth = 100;
            this.cmbPriority.IntegralHeight = false;
            this.cmbPriority.Location = new System.Drawing.Point(591, 74);
            this.cmbPriority.Name = "cmbPriority";
            this.cmbPriority.Size = new System.Drawing.Size(180, 21);
            this.cmbPriority.TabIndex = 43;
            // 
            // cmbReminderBizType
            // 
            this.cmbReminderBizType.DropDownWidth = 100;
            this.cmbReminderBizType.IntegralHeight = false;
            this.cmbReminderBizType.Location = new System.Drawing.Point(591, 48);
            this.cmbReminderBizType.Name = "cmbReminderBizType";
            this.cmbReminderBizType.Size = new System.Drawing.Size(180, 21);
            this.cmbReminderBizType.TabIndex = 42;
            // 
            // lblRuleName
            // 
            this.lblRuleName.Location = new System.Drawing.Point(75, 20);
            this.lblRuleName.Name = "lblRuleName";
            this.lblRuleName.Size = new System.Drawing.Size(62, 20);
            this.lblRuleName.TabIndex = 16;
            this.lblRuleName.Values.Text = "规则名称";
            // 
            // txtRuleName
            // 
            this.txtRuleName.Location = new System.Drawing.Point(144, 20);
            this.txtRuleName.Name = "txtRuleName";
            this.txtRuleName.Size = new System.Drawing.Size(271, 23);
            this.txtRuleName.TabIndex = 17;
            // 
            // lblRuleEngineType
            // 
            this.lblRuleEngineType.Location = new System.Drawing.Point(523, 23);
            this.lblRuleEngineType.Name = "lblRuleEngineType";
            this.lblRuleEngineType.Size = new System.Drawing.Size(62, 20);
            this.lblRuleEngineType.TabIndex = 18;
            this.lblRuleEngineType.Values.Text = "引擎类型";
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(75, 49);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(62, 20);
            this.lblDescription.TabIndex = 20;
            this.lblDescription.Values.Text = "规则描述";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(144, 49);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(271, 48);
            this.txtDescription.TabIndex = 21;
            // 
            // lblReminderBizType
            // 
            this.lblReminderBizType.Location = new System.Drawing.Point(523, 49);
            this.lblReminderBizType.Name = "lblReminderBizType";
            this.lblReminderBizType.Size = new System.Drawing.Size(62, 20);
            this.lblReminderBizType.TabIndex = 22;
            this.lblReminderBizType.Values.Text = "业务类型";
            // 
            // lblPriority
            // 
            this.lblPriority.Location = new System.Drawing.Point(536, 72);
            this.lblPriority.Name = "lblPriority";
            this.lblPriority.Size = new System.Drawing.Size(49, 20);
            this.lblPriority.TabIndex = 24;
            this.lblPriority.Values.Text = "优先级";
            // 
            // chkIsEnabled
            // 
            this.chkIsEnabled.Location = new System.Drawing.Point(722, 101);
            this.chkIsEnabled.Name = "chkIsEnabled";
            this.chkIsEnabled.Size = new System.Drawing.Size(49, 20);
            this.chkIsEnabled.TabIndex = 27;
            this.chkIsEnabled.Values.Text = "启用";
            // 
            // lblNotifyChannels
            // 
            this.lblNotifyChannels.Location = new System.Drawing.Point(524, 142);
            this.lblNotifyChannels.Name = "lblNotifyChannels";
            this.lblNotifyChannels.Size = new System.Drawing.Size(62, 20);
            this.lblNotifyChannels.TabIndex = 29;
            this.lblNotifyChannels.Values.Text = "通知渠道";
            // 
            // lblEffectiveDate
            // 
            this.lblEffectiveDate.Location = new System.Drawing.Point(75, 103);
            this.lblEffectiveDate.Name = "lblEffectiveDate";
            this.lblEffectiveDate.Size = new System.Drawing.Size(62, 20);
            this.lblEffectiveDate.TabIndex = 30;
            this.lblEffectiveDate.Values.Text = "生效日期";
            // 
            // dtpEffectiveDate
            // 
            this.dtpEffectiveDate.Location = new System.Drawing.Point(144, 103);
            this.dtpEffectiveDate.Name = "dtpEffectiveDate";
            this.dtpEffectiveDate.Size = new System.Drawing.Size(122, 21);
            this.dtpEffectiveDate.TabIndex = 31;
            // 
            // lblExpireDate
            // 
            this.lblExpireDate.Location = new System.Drawing.Point(523, 101);
            this.lblExpireDate.Name = "lblExpireDate";
            this.lblExpireDate.Size = new System.Drawing.Size(62, 20);
            this.lblExpireDate.TabIndex = 32;
            this.lblExpireDate.Values.Text = "过期时间";
            // 
            // dtpExpireDate
            // 
            this.dtpExpireDate.Location = new System.Drawing.Point(591, 100);
            this.dtpExpireDate.Name = "dtpExpireDate";
            this.dtpExpireDate.Size = new System.Drawing.Size(113, 21);
            this.dtpExpireDate.TabIndex = 33;
            // 
            // lblNotifyRecipients
            // 
            this.lblNotifyRecipients.Location = new System.Drawing.Point(50, 140);
            this.lblNotifyRecipients.Name = "lblNotifyRecipients";
            this.lblNotifyRecipients.Size = new System.Drawing.Size(88, 20);
            this.lblNotifyRecipients.TabIndex = 36;
            this.lblNotifyRecipients.Values.Text = "通知接收人员";
            // 
            // lblJsonConfig
            // 
            this.lblJsonConfig.Location = new System.Drawing.Point(46, 394);
            this.lblJsonConfig.Name = "lblJsonConfig";
            this.lblJsonConfig.Size = new System.Drawing.Size(92, 20);
            this.lblJsonConfig.TabIndex = 40;
            this.lblJsonConfig.Values.Text = "扩展JSON配置";
            // 
            // cmbRuleEngineType
            // 
            this.cmbRuleEngineType.DropDownWidth = 100;
            this.cmbRuleEngineType.IntegralHeight = false;
            this.cmbRuleEngineType.Location = new System.Drawing.Point(591, 23);
            this.cmbRuleEngineType.Name = "cmbRuleEngineType";
            this.cmbRuleEngineType.Size = new System.Drawing.Size(180, 21);
            this.cmbRuleEngineType.TabIndex = 7;
            // 
            // linkGroupBox
            // 
            this.linkGroupBox.Location = new System.Drawing.Point(507, 142);
            this.linkGroupBox.Name = "linkGroupBox";
            this.linkGroupBox.Size = new System.Drawing.Size(365, 243);
            this.linkGroupBox.TabIndex = 54;
            this.linkGroupBox.Values.Heading = "关联链路";
            // 
            // btnAddLink
            // 
            this.btnAddLink.Location = new System.Drawing.Point(260, 15);
            this.btnAddLink.Name = "btnAddLink";
            this.btnAddLink.Size = new System.Drawing.Size(90, 25);
            this.btnAddLink.TabIndex = 0;
            this.btnAddLink.Values.Text = "添加链路";
            // 
            // btnRemoveLink
            // 
            this.btnRemoveLink.Location = new System.Drawing.Point(260, 46);
            this.btnRemoveLink.Name = "btnRemoveLink";
            this.btnRemoveLink.Size = new System.Drawing.Size(90, 25);
            this.btnRemoveLink.TabIndex = 1;
            this.btnRemoveLink.Values.Text = "移除链路";
            // 
            // dgvLinkedLinks
            // 
            this.dgvLinkedLinks.AllowUserToAddRows = false;
            this.dgvLinkedLinks.AllowUserToDeleteRows = false;
            this.dgvLinkedLinks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLinkedLinks.Location = new System.Drawing.Point(10, 15);
            this.dgvLinkedLinks.Name = "dgvLinkedLinks";
            this.dgvLinkedLinks.ReadOnly = true;
            this.dgvLinkedLinks.Size = new System.Drawing.Size(244, 215);
            this.dgvLinkedLinks.TabIndex = 2;
            // 
            // jsonViewer1
            // 
            this.jsonViewer1.Location = new System.Drawing.Point(144, 394);
            this.jsonViewer1.Name = "jsonViewer1";
            this.jsonViewer1.Size = new System.Drawing.Size(629, 366);
            this.jsonViewer1.TabIndex = 55;
            // 
            // UCReminderRuleEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(911, 819);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCReminderRuleEdit";
            this.Load += new System.EventHandler(this.UCReminderRuleEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPriority)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbReminderBizType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbRuleEngineType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLinkedLinks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.linkGroupBox.Panel)).EndInit();
            this.linkGroupBox.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.linkGroupBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.jsonViewer1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonComboBox cmbRuleEngineType;
        private Krypton.Toolkit.KryptonLabel lblRuleName;
        private Krypton.Toolkit.KryptonTextBox txtRuleName;
        private Krypton.Toolkit.KryptonLabel lblRuleEngineType;
        private Krypton.Toolkit.KryptonLabel lblDescription;
        private Krypton.Toolkit.KryptonTextBox txtDescription;
        private Krypton.Toolkit.KryptonLabel lblReminderBizType;
        private Krypton.Toolkit.KryptonLabel lblPriority;
        private Krypton.Toolkit.KryptonCheckBox chkIsEnabled;
        private Krypton.Toolkit.KryptonLabel lblNotifyChannels;
        private Krypton.Toolkit.KryptonLabel lblEffectiveDate;
        private Krypton.Toolkit.KryptonDateTimePicker dtpEffectiveDate;
        private Krypton.Toolkit.KryptonLabel lblExpireDate;
        private Krypton.Toolkit.KryptonDateTimePicker dtpExpireDate;
        private Krypton.Toolkit.KryptonLabel lblNotifyRecipients;
        private Krypton.Toolkit.KryptonLabel lblJsonConfig;
        private Krypton.Toolkit.KryptonComboBox cmbPriority;
        private Krypton.Toolkit.KryptonComboBox cmbReminderBizType;
        private Krypton.Toolkit.KryptonButton btnConfigParser;
        private Krypton.Toolkit.KryptonButton btnSelectNotifyRecipients;
        private Krypton.Toolkit.KryptonCheckedListBox clbNotifyChannels;
        private Krypton.Toolkit.KryptonCheckedListBox txtNotifyRecipientNames;
        private Krypton.Toolkit.KryptonGroupBox linkGroupBox;
        private Krypton.Toolkit.KryptonButton btnAddLink;
        private Krypton.Toolkit.KryptonButton btnRemoveLink;
        private Krypton.Toolkit.KryptonDataGridView dgvLinkedLinks;
        private Monitoring.Auditing.JsonViewer jsonViewer1;
    }
}
