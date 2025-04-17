namespace RUINORERP.UI.BI
{
    partial class UCCurrencyExchangeRateEdit
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
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            this.lblConversionName = new Krypton.Toolkit.KryptonLabel();
            this.txtConversionName = new Krypton.Toolkit.KryptonTextBox();
            this.lblBaseCurrencyID = new Krypton.Toolkit.KryptonLabel();
            this.lblTargetCurrencyID = new Krypton.Toolkit.KryptonLabel();
            this.lblEffectiveDate = new Krypton.Toolkit.KryptonLabel();
            this.dtpEffectiveDate = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lblExpirationDate = new Krypton.Toolkit.KryptonLabel();
            this.dtpExpirationDate = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lblDefaultExchRate = new Krypton.Toolkit.KryptonLabel();
            this.txtDefaultExchRate = new Krypton.Toolkit.KryptonTextBox();
            this.lblExecuteExchRate = new Krypton.Toolkit.KryptonLabel();
            this.txtExecuteExchRate = new Krypton.Toolkit.KryptonTextBox();
            this.lblIs_enabled = new Krypton.Toolkit.KryptonLabel();
            this.chkIs_enabled = new Krypton.Toolkit.KryptonCheckBox();
            this.lblIs_available = new Krypton.Toolkit.KryptonLabel();
            this.chkIs_available = new Krypton.Toolkit.KryptonCheckBox();
            this.lblNotes = new Krypton.Toolkit.KryptonLabel();
            this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
            this.cmbBaseCurrencyID = new Krypton.Toolkit.KryptonComboBox();
            this.cmbTargetCurrencyID = new Krypton.Toolkit.KryptonComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBaseCurrencyID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTargetCurrencyID)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(186, 391);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(304, 391);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.cmbTargetCurrencyID);
            this.kryptonPanel1.Controls.Add(this.cmbBaseCurrencyID);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel1);
            this.kryptonPanel1.Controls.Add(this.lblConversionName);
            this.kryptonPanel1.Controls.Add(this.txtConversionName);
            this.kryptonPanel1.Controls.Add(this.lblBaseCurrencyID);
            this.kryptonPanel1.Controls.Add(this.lblTargetCurrencyID);
            this.kryptonPanel1.Controls.Add(this.lblEffectiveDate);
            this.kryptonPanel1.Controls.Add(this.dtpEffectiveDate);
            this.kryptonPanel1.Controls.Add(this.lblExpirationDate);
            this.kryptonPanel1.Controls.Add(this.dtpExpirationDate);
            this.kryptonPanel1.Controls.Add(this.lblDefaultExchRate);
            this.kryptonPanel1.Controls.Add(this.txtDefaultExchRate);
            this.kryptonPanel1.Controls.Add(this.lblExecuteExchRate);
            this.kryptonPanel1.Controls.Add(this.txtExecuteExchRate);
            this.kryptonPanel1.Controls.Add(this.lblIs_enabled);
            this.kryptonPanel1.Controls.Add(this.chkIs_enabled);
            this.kryptonPanel1.Controls.Add(this.lblIs_available);
            this.kryptonPanel1.Controls.Add(this.chkIs_available);
            this.kryptonPanel1.Controls.Add(this.lblNotes);
            this.kryptonPanel1.Controls.Add(this.txtNotes);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(590, 426);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(118, 218);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(317, 20);
            this.kryptonLabel1.TabIndex = 71;
            this.kryptonLabel1.Values.Text = "例:基本币为美元，1美元换人民币7.2  那么汇率则为7.2";
            // 
            // lblConversionName
            // 
            this.lblConversionName.Location = new System.Drawing.Point(77, 28);
            this.lblConversionName.Name = "lblConversionName";
            this.lblConversionName.Size = new System.Drawing.Size(62, 20);
            this.lblConversionName.TabIndex = 53;
            this.lblConversionName.Values.Text = "换算名称";
            // 
            // txtConversionName
            // 
            this.txtConversionName.Location = new System.Drawing.Point(140, 29);
            this.txtConversionName.Name = "txtConversionName";
            this.txtConversionName.Size = new System.Drawing.Size(305, 23);
            this.txtConversionName.TabIndex = 54;
            // 
            // lblBaseCurrencyID
            // 
            this.lblBaseCurrencyID.Location = new System.Drawing.Point(77, 53);
            this.lblBaseCurrencyID.Name = "lblBaseCurrencyID";
            this.lblBaseCurrencyID.Size = new System.Drawing.Size(62, 20);
            this.lblBaseCurrencyID.TabIndex = 55;
            this.lblBaseCurrencyID.Values.Text = "基本币别";
            // 
            // lblTargetCurrencyID
            // 
            this.lblTargetCurrencyID.Location = new System.Drawing.Point(77, 78);
            this.lblTargetCurrencyID.Name = "lblTargetCurrencyID";
            this.lblTargetCurrencyID.Size = new System.Drawing.Size(62, 20);
            this.lblTargetCurrencyID.TabIndex = 57;
            this.lblTargetCurrencyID.Values.Text = "目标币别";
            // 
            // lblEffectiveDate
            // 
            this.lblEffectiveDate.Location = new System.Drawing.Point(77, 103);
            this.lblEffectiveDate.Name = "lblEffectiveDate";
            this.lblEffectiveDate.Size = new System.Drawing.Size(62, 20);
            this.lblEffectiveDate.TabIndex = 59;
            this.lblEffectiveDate.Values.Text = "生效日期";
            // 
            // dtpEffectiveDate
            // 
            this.dtpEffectiveDate.Location = new System.Drawing.Point(140, 104);
            this.dtpEffectiveDate.Name = "dtpEffectiveDate";
            this.dtpEffectiveDate.Size = new System.Drawing.Size(305, 21);
            this.dtpEffectiveDate.TabIndex = 60;
            // 
            // lblExpirationDate
            // 
            this.lblExpirationDate.Location = new System.Drawing.Point(77, 128);
            this.lblExpirationDate.Name = "lblExpirationDate";
            this.lblExpirationDate.Size = new System.Drawing.Size(62, 20);
            this.lblExpirationDate.TabIndex = 62;
            this.lblExpirationDate.Values.Text = "有效日期";
            // 
            // dtpExpirationDate
            // 
            this.dtpExpirationDate.Location = new System.Drawing.Point(140, 129);
            this.dtpExpirationDate.Name = "dtpExpirationDate";
            this.dtpExpirationDate.ShowCheckBox = true;
            this.dtpExpirationDate.Size = new System.Drawing.Size(305, 21);
            this.dtpExpirationDate.TabIndex = 61;
            // 
            // lblDefaultExchRate
            // 
            this.lblDefaultExchRate.Location = new System.Drawing.Point(77, 153);
            this.lblDefaultExchRate.Name = "lblDefaultExchRate";
            this.lblDefaultExchRate.Size = new System.Drawing.Size(62, 20);
            this.lblDefaultExchRate.TabIndex = 63;
            this.lblDefaultExchRate.Values.Text = "预设汇率";
            // 
            // txtDefaultExchRate
            // 
            this.txtDefaultExchRate.Location = new System.Drawing.Point(140, 154);
            this.txtDefaultExchRate.Name = "txtDefaultExchRate";
            this.txtDefaultExchRate.Size = new System.Drawing.Size(305, 23);
            this.txtDefaultExchRate.TabIndex = 64;
            // 
            // lblExecuteExchRate
            // 
            this.lblExecuteExchRate.Location = new System.Drawing.Point(77, 178);
            this.lblExecuteExchRate.Name = "lblExecuteExchRate";
            this.lblExecuteExchRate.Size = new System.Drawing.Size(62, 20);
            this.lblExecuteExchRate.TabIndex = 65;
            this.lblExecuteExchRate.Values.Text = "执行汇率";
            // 
            // txtExecuteExchRate
            // 
            this.txtExecuteExchRate.Location = new System.Drawing.Point(140, 179);
            this.txtExecuteExchRate.Name = "txtExecuteExchRate";
            this.txtExecuteExchRate.Size = new System.Drawing.Size(305, 23);
            this.txtExecuteExchRate.TabIndex = 66;
            // 
            // lblIs_enabled
            // 
            this.lblIs_enabled.Location = new System.Drawing.Point(74, 244);
            this.lblIs_enabled.Name = "lblIs_enabled";
            this.lblIs_enabled.Size = new System.Drawing.Size(62, 20);
            this.lblIs_enabled.TabIndex = 67;
            this.lblIs_enabled.Values.Text = "是否启用";
            // 
            // chkIs_enabled
            // 
            this.chkIs_enabled.Checked = true;
            this.chkIs_enabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIs_enabled.Location = new System.Drawing.Point(137, 245);
            this.chkIs_enabled.Name = "chkIs_enabled";
            this.chkIs_enabled.Size = new System.Drawing.Size(19, 13);
            this.chkIs_enabled.TabIndex = 68;
            this.chkIs_enabled.Values.Text = "";
            // 
            // lblIs_available
            // 
            this.lblIs_available.Location = new System.Drawing.Point(356, 249);
            this.lblIs_available.Name = "lblIs_available";
            this.lblIs_available.Size = new System.Drawing.Size(62, 20);
            this.lblIs_available.TabIndex = 69;
            this.lblIs_available.Values.Text = "是否可用";
            // 
            // chkIs_available
            // 
            this.chkIs_available.Checked = true;
            this.chkIs_available.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIs_available.Location = new System.Drawing.Point(419, 250);
            this.chkIs_available.Name = "chkIs_available";
            this.chkIs_available.Size = new System.Drawing.Size(19, 13);
            this.chkIs_available.TabIndex = 70;
            this.chkIs_available.Values.Text = "";
            // 
            // lblNotes
            // 
            this.lblNotes.Location = new System.Drawing.Point(94, 275);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(36, 20);
            this.lblNotes.TabIndex = 51;
            this.lblNotes.Values.Text = "备注";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(136, 275);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(307, 96);
            this.txtNotes.TabIndex = 52;
            // 
            // cmbBaseCurrencyID
            // 
            this.cmbBaseCurrencyID.DropDownWidth = 100;
            this.cmbBaseCurrencyID.IntegralHeight = false;
            this.cmbBaseCurrencyID.Location = new System.Drawing.Point(140, 53);
            this.cmbBaseCurrencyID.Name = "cmbBaseCurrencyID";
            this.cmbBaseCurrencyID.Size = new System.Drawing.Size(305, 21);
            this.cmbBaseCurrencyID.TabIndex = 72;
            // 
            // cmbTargetCurrencyID
            // 
            this.cmbTargetCurrencyID.DropDownWidth = 100;
            this.cmbTargetCurrencyID.IntegralHeight = false;
            this.cmbTargetCurrencyID.Location = new System.Drawing.Point(140, 77);
            this.cmbTargetCurrencyID.Name = "cmbTargetCurrencyID";
            this.cmbTargetCurrencyID.Size = new System.Drawing.Size(305, 21);
            this.cmbTargetCurrencyID.TabIndex = 73;
            // 
            // UCCurrencyExchangeRateEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 426);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCCurrencyExchangeRateEdit";
            this.Load += new System.EventHandler(this.UCCurrencyEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBaseCurrencyID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTargetCurrencyID)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonLabel lblNotes;
        private Krypton.Toolkit.KryptonTextBox txtNotes;
        private Krypton.Toolkit.KryptonLabel lblConversionName;
        private Krypton.Toolkit.KryptonTextBox txtConversionName;
        private Krypton.Toolkit.KryptonLabel lblBaseCurrencyID;
        private Krypton.Toolkit.KryptonLabel lblTargetCurrencyID;
        private Krypton.Toolkit.KryptonLabel lblEffectiveDate;
        private Krypton.Toolkit.KryptonDateTimePicker dtpEffectiveDate;
        private Krypton.Toolkit.KryptonLabel lblExpirationDate;
        private Krypton.Toolkit.KryptonDateTimePicker dtpExpirationDate;
        private Krypton.Toolkit.KryptonLabel lblDefaultExchRate;
        private Krypton.Toolkit.KryptonTextBox txtDefaultExchRate;
        private Krypton.Toolkit.KryptonLabel lblExecuteExchRate;
        private Krypton.Toolkit.KryptonTextBox txtExecuteExchRate;
        private Krypton.Toolkit.KryptonLabel lblIs_enabled;
        private Krypton.Toolkit.KryptonCheckBox chkIs_enabled;
        private Krypton.Toolkit.KryptonLabel lblIs_available;
        private Krypton.Toolkit.KryptonCheckBox chkIs_available;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private Krypton.Toolkit.KryptonComboBox cmbTargetCurrencyID;
        private Krypton.Toolkit.KryptonComboBox cmbBaseCurrencyID;
    }
}
