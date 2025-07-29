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
            this.txtNotifyRecipients = new Krypton.Toolkit.KryptonTextBox();
            this.btnSelectNotifyRecipients = new Krypton.Toolkit.KryptonButton();
            this.txtNotifyRecipientNames = new Krypton.Toolkit.KryptonTextBox();
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
            this.lblIsEnabled = new Krypton.Toolkit.KryptonLabel();
            this.chkIsEnabled = new Krypton.Toolkit.KryptonCheckBox();
            this.lblNotifyChannels = new Krypton.Toolkit.KryptonLabel();
            this.lblEffectiveDate = new Krypton.Toolkit.KryptonLabel();
            this.dtpEffectiveDate = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lblExpireDate = new Krypton.Toolkit.KryptonLabel();
            this.dtpExpireDate = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lblCondition = new Krypton.Toolkit.KryptonLabel();
            this.txtCondition = new Krypton.Toolkit.KryptonTextBox();
            this.lblNotifyRecipients = new Krypton.Toolkit.KryptonLabel();
            this.lblNotifyMessage = new Krypton.Toolkit.KryptonLabel();
            this.txtNotifyMessage = new Krypton.Toolkit.KryptonTextBox();
            this.lblJsonConfig = new Krypton.Toolkit.KryptonLabel();
            this.txtJsonConfig = new Krypton.Toolkit.KryptonTextBox();
            this.cmbRuleEngineType = new Krypton.Toolkit.KryptonComboBox();
            this.chkNotifyChannels = new System.Windows.Forms.CheckedListBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPriority)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbReminderBizType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbRuleEngineType)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(344, 721);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(462, 721);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.chkNotifyChannels);
            this.kryptonPanel1.Controls.Add(this.txtNotifyRecipients);
            this.kryptonPanel1.Controls.Add(this.btnSelectNotifyRecipients);
            this.kryptonPanel1.Controls.Add(this.txtNotifyRecipientNames);
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
            this.kryptonPanel1.Controls.Add(this.lblIsEnabled);
            this.kryptonPanel1.Controls.Add(this.chkIsEnabled);
            this.kryptonPanel1.Controls.Add(this.lblNotifyChannels);
            this.kryptonPanel1.Controls.Add(this.lblEffectiveDate);
            this.kryptonPanel1.Controls.Add(this.dtpEffectiveDate);
            this.kryptonPanel1.Controls.Add(this.lblExpireDate);
            this.kryptonPanel1.Controls.Add(this.dtpExpireDate);
            this.kryptonPanel1.Controls.Add(this.lblCondition);
            this.kryptonPanel1.Controls.Add(this.txtCondition);
            this.kryptonPanel1.Controls.Add(this.lblNotifyRecipients);
            this.kryptonPanel1.Controls.Add(this.lblNotifyMessage);
            this.kryptonPanel1.Controls.Add(this.txtNotifyMessage);
            this.kryptonPanel1.Controls.Add(this.lblJsonConfig);
            this.kryptonPanel1.Controls.Add(this.txtJsonConfig);
            this.kryptonPanel1.Controls.Add(this.cmbRuleEngineType);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(958, 758);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // txtNotifyRecipients
            // 
            this.txtNotifyRecipients.Location = new System.Drawing.Point(777, 303);
            this.txtNotifyRecipients.Multiline = true;
            this.txtNotifyRecipients.Name = "txtNotifyRecipients";
            this.txtNotifyRecipients.Size = new System.Drawing.Size(30, 18);
            this.txtNotifyRecipients.TabIndex = 49;
            this.txtNotifyRecipients.Visible = false;
            // 
            // btnSelectNotifyRecipients
            // 
            this.btnSelectNotifyRecipients.Location = new System.Drawing.Point(777, 272);
            this.btnSelectNotifyRecipients.Name = "btnSelectNotifyRecipients";
            this.btnSelectNotifyRecipients.Size = new System.Drawing.Size(87, 25);
            this.btnSelectNotifyRecipients.TabIndex = 48;
            this.btnSelectNotifyRecipients.Values.Text = "选择接收人";
            this.btnSelectNotifyRecipients.Click += new System.EventHandler(this.btnSelectNotifyRecipients_Click);
            // 
            // txtNotifyRecipientNames
            // 
            this.txtNotifyRecipientNames.Location = new System.Drawing.Point(144, 272);
            this.txtNotifyRecipientNames.Multiline = true;
            this.txtNotifyRecipientNames.Name = "txtNotifyRecipientNames";
            this.txtNotifyRecipientNames.Size = new System.Drawing.Size(627, 53);
            this.txtNotifyRecipientNames.TabIndex = 47;
            // 
            // btnConfigParser
            // 
            this.btnConfigParser.Location = new System.Drawing.Point(777, 440);
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
            this.lblRuleName.Location = new System.Drawing.Point(79, 20);
            this.lblRuleName.Name = "lblRuleName";
            this.lblRuleName.Size = new System.Drawing.Size(62, 20);
            this.lblRuleName.TabIndex = 16;
            this.lblRuleName.Values.Text = "规则名称";
            // 
            // txtRuleName
            // 
            this.txtRuleName.Location = new System.Drawing.Point(144, 20);
            this.txtRuleName.Name = "txtRuleName";
            this.txtRuleName.Size = new System.Drawing.Size(361, 23);
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
            this.lblDescription.Location = new System.Drawing.Point(79, 49);
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
            this.txtDescription.Size = new System.Drawing.Size(361, 48);
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
            // lblIsEnabled
            // 
            this.lblIsEnabled.Location = new System.Drawing.Point(793, 23);
            this.lblIsEnabled.Name = "lblIsEnabled";
            this.lblIsEnabled.Size = new System.Drawing.Size(62, 20);
            this.lblIsEnabled.TabIndex = 26;
            this.lblIsEnabled.Values.Text = "是否启用";
            // 
            // chkIsEnabled
            // 
            this.chkIsEnabled.Location = new System.Drawing.Point(861, 27);
            this.chkIsEnabled.Name = "chkIsEnabled";
            this.chkIsEnabled.Size = new System.Drawing.Size(19, 13);
            this.chkIsEnabled.TabIndex = 27;
            this.chkIsEnabled.Values.Text = "";
            // 
            // lblNotifyChannels
            // 
            this.lblNotifyChannels.Location = new System.Drawing.Point(523, 102);
            this.lblNotifyChannels.Name = "lblNotifyChannels";
            this.lblNotifyChannels.Size = new System.Drawing.Size(62, 20);
            this.lblNotifyChannels.TabIndex = 29;
            this.lblNotifyChannels.Values.Text = "通知渠道";
            // 
            // lblEffectiveDate
            // 
            this.lblEffectiveDate.Location = new System.Drawing.Point(79, 118);
            this.lblEffectiveDate.Name = "lblEffectiveDate";
            this.lblEffectiveDate.Size = new System.Drawing.Size(62, 20);
            this.lblEffectiveDate.TabIndex = 30;
            this.lblEffectiveDate.Values.Text = "生效日期";
            // 
            // dtpEffectiveDate
            // 
            this.dtpEffectiveDate.Location = new System.Drawing.Point(144, 118);
            this.dtpEffectiveDate.Name = "dtpEffectiveDate";
            this.dtpEffectiveDate.Size = new System.Drawing.Size(122, 21);
            this.dtpEffectiveDate.TabIndex = 31;
            // 
            // lblExpireDate
            // 
            this.lblExpireDate.Location = new System.Drawing.Point(76, 160);
            this.lblExpireDate.Name = "lblExpireDate";
            this.lblExpireDate.Size = new System.Drawing.Size(62, 20);
            this.lblExpireDate.TabIndex = 32;
            this.lblExpireDate.Values.Text = "过期时间";
            // 
            // dtpExpireDate
            // 
            this.dtpExpireDate.Location = new System.Drawing.Point(144, 158);
            this.dtpExpireDate.Name = "dtpExpireDate";
            this.dtpExpireDate.Size = new System.Drawing.Size(122, 21);
            this.dtpExpireDate.TabIndex = 33;
            // 
            // lblCondition
            // 
            this.lblCondition.Location = new System.Drawing.Point(79, 207);
            this.lblCondition.Name = "lblCondition";
            this.lblCondition.Size = new System.Drawing.Size(62, 20);
            this.lblCondition.TabIndex = 34;
            this.lblCondition.Values.Text = "规则条件";
            // 
            // txtCondition
            // 
            this.txtCondition.Location = new System.Drawing.Point(144, 207);
            this.txtCondition.Multiline = true;
            this.txtCondition.Name = "txtCondition";
            this.txtCondition.Size = new System.Drawing.Size(627, 59);
            this.txtCondition.TabIndex = 35;
            // 
            // lblNotifyRecipients
            // 
            this.lblNotifyRecipients.Location = new System.Drawing.Point(53, 270);
            this.lblNotifyRecipients.Name = "lblNotifyRecipients";
            this.lblNotifyRecipients.Size = new System.Drawing.Size(88, 20);
            this.lblNotifyRecipients.TabIndex = 36;
            this.lblNotifyRecipients.Values.Text = "通知接收人员";
            // 
            // lblNotifyMessage
            // 
            this.lblNotifyMessage.Location = new System.Drawing.Point(50, 338);
            this.lblNotifyMessage.Name = "lblNotifyMessage";
            this.lblNotifyMessage.Size = new System.Drawing.Size(88, 20);
            this.lblNotifyMessage.TabIndex = 38;
            this.lblNotifyMessage.Values.Text = "通知消息模板";
            // 
            // txtNotifyMessage
            // 
            this.txtNotifyMessage.Location = new System.Drawing.Point(144, 337);
            this.txtNotifyMessage.Multiline = true;
            this.txtNotifyMessage.Name = "txtNotifyMessage";
            this.txtNotifyMessage.Size = new System.Drawing.Size(627, 94);
            this.txtNotifyMessage.TabIndex = 39;
            // 
            // lblJsonConfig
            // 
            this.lblJsonConfig.Location = new System.Drawing.Point(49, 440);
            this.lblJsonConfig.Name = "lblJsonConfig";
            this.lblJsonConfig.Size = new System.Drawing.Size(92, 20);
            this.lblJsonConfig.TabIndex = 40;
            this.lblJsonConfig.Values.Text = "扩展JSON配置";
            // 
            // txtJsonConfig
            // 
            this.txtJsonConfig.Location = new System.Drawing.Point(144, 440);
            this.txtJsonConfig.Multiline = true;
            this.txtJsonConfig.Name = "txtJsonConfig";
            this.txtJsonConfig.Size = new System.Drawing.Size(627, 200);
            this.txtJsonConfig.TabIndex = 41;
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
            // chkNotifyChannels
            // 
            this.chkNotifyChannels.FormattingEnabled = true;
            this.chkNotifyChannels.Location = new System.Drawing.Point(591, 102);
            this.chkNotifyChannels.Name = "chkNotifyChannels";
            this.chkNotifyChannels.Size = new System.Drawing.Size(180, 100);
            this.chkNotifyChannels.TabIndex = 50;
            // 
            // UCReminderRuleEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(958, 758);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCReminderRuleEdit";
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPriority)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbReminderBizType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbRuleEngineType)).EndInit();
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
        private Krypton.Toolkit.KryptonLabel lblIsEnabled;
        private Krypton.Toolkit.KryptonCheckBox chkIsEnabled;
        private Krypton.Toolkit.KryptonLabel lblNotifyChannels;
        private Krypton.Toolkit.KryptonLabel lblEffectiveDate;
        private Krypton.Toolkit.KryptonDateTimePicker dtpEffectiveDate;
        private Krypton.Toolkit.KryptonLabel lblExpireDate;
        private Krypton.Toolkit.KryptonDateTimePicker dtpExpireDate;
        private Krypton.Toolkit.KryptonLabel lblCondition;
        private Krypton.Toolkit.KryptonTextBox txtCondition;
        private Krypton.Toolkit.KryptonLabel lblNotifyRecipients;
        private Krypton.Toolkit.KryptonLabel lblNotifyMessage;
        private Krypton.Toolkit.KryptonTextBox txtNotifyMessage;
        private Krypton.Toolkit.KryptonLabel lblJsonConfig;
        private Krypton.Toolkit.KryptonTextBox txtJsonConfig;
        private Krypton.Toolkit.KryptonComboBox cmbPriority;
        private Krypton.Toolkit.KryptonComboBox cmbReminderBizType;
        private Krypton.Toolkit.KryptonButton btnConfigParser;
        private Krypton.Toolkit.KryptonTextBox txtNotifyRecipientNames;
        private Krypton.Toolkit.KryptonButton btnSelectNotifyRecipients;
        private Krypton.Toolkit.KryptonTextBox txtNotifyRecipients;
        private System.Windows.Forms.CheckedListBox chkNotifyChannels;
    }
}
