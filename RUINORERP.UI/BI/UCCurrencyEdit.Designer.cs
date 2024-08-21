namespace RUINORERP.UI.BI
{
    partial class UCCurrencyEdit
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
            this.dtpAdjustDate = new Krypton.Toolkit.KryptonDateTimePicker();
            this.chkIs_available = new Krypton.Toolkit.KryptonCheckBox();
            this.chkIs_enabled = new Krypton.Toolkit.KryptonCheckBox();
            this.lblGroupName = new Krypton.Toolkit.KryptonLabel();
            this.txtGroupName = new Krypton.Toolkit.KryptonTextBox();
            this.lblCurrencyCode = new Krypton.Toolkit.KryptonLabel();
            this.txtCurrencyCode = new Krypton.Toolkit.KryptonTextBox();
            this.lblCurrencyName = new Krypton.Toolkit.KryptonLabel();
            this.txtCurrencyName = new Krypton.Toolkit.KryptonTextBox();
            this.lblAdjustDate = new Krypton.Toolkit.KryptonLabel();
            this.lbl预设汇率 = new Krypton.Toolkit.KryptonLabel();
            this.txtDefaultExchRate = new Krypton.Toolkit.KryptonTextBox();
            this.lblBuyExchRate = new Krypton.Toolkit.KryptonLabel();
            this.txtBuyExchRate = new Krypton.Toolkit.KryptonTextBox();
            this.lblSellOutExchRate = new Krypton.Toolkit.KryptonLabel();
            this.txtSellOutExchRate = new Krypton.Toolkit.KryptonTextBox();
            this.lblMonthEndExchRate = new Krypton.Toolkit.KryptonLabel();
            this.txtMonthEndExchRate = new Krypton.Toolkit.KryptonTextBox();
            this.lblIs_enabled = new Krypton.Toolkit.KryptonLabel();
            this.lblIs_available = new Krypton.Toolkit.KryptonLabel();
            this.lblNotes = new Krypton.Toolkit.KryptonLabel();
            this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(193, 465);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(311, 465);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.dtpAdjustDate);
            this.kryptonPanel1.Controls.Add(this.chkIs_available);
            this.kryptonPanel1.Controls.Add(this.chkIs_enabled);
            this.kryptonPanel1.Controls.Add(this.lblGroupName);
            this.kryptonPanel1.Controls.Add(this.txtGroupName);
            this.kryptonPanel1.Controls.Add(this.lblCurrencyCode);
            this.kryptonPanel1.Controls.Add(this.txtCurrencyCode);
            this.kryptonPanel1.Controls.Add(this.lblCurrencyName);
            this.kryptonPanel1.Controls.Add(this.txtCurrencyName);
            this.kryptonPanel1.Controls.Add(this.lblAdjustDate);
            this.kryptonPanel1.Controls.Add(this.lbl预设汇率);
            this.kryptonPanel1.Controls.Add(this.txtDefaultExchRate);
            this.kryptonPanel1.Controls.Add(this.lblBuyExchRate);
            this.kryptonPanel1.Controls.Add(this.txtBuyExchRate);
            this.kryptonPanel1.Controls.Add(this.lblSellOutExchRate);
            this.kryptonPanel1.Controls.Add(this.txtSellOutExchRate);
            this.kryptonPanel1.Controls.Add(this.lblMonthEndExchRate);
            this.kryptonPanel1.Controls.Add(this.txtMonthEndExchRate);
            this.kryptonPanel1.Controls.Add(this.lblIs_enabled);
            this.kryptonPanel1.Controls.Add(this.lblIs_available);
            this.kryptonPanel1.Controls.Add(this.lblNotes);
            this.kryptonPanel1.Controls.Add(this.txtNotes);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(606, 526);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // dtpAdjustDate
            // 
            this.dtpAdjustDate.Location = new System.Drawing.Point(184, 88);
            this.dtpAdjustDate.Name = "dtpAdjustDate";
            this.dtpAdjustDate.ShowCheckBox = true;
            this.dtpAdjustDate.Size = new System.Drawing.Size(289, 21);
            this.dtpAdjustDate.TabIndex = 75;
            // 
            // chkIs_available
            // 
            this.chkIs_available.Location = new System.Drawing.Point(183, 241);
            this.chkIs_available.Name = "chkIs_available";
            this.chkIs_available.Size = new System.Drawing.Size(19, 13);
            this.chkIs_available.TabIndex = 74;
            this.chkIs_available.Values.Text = "";
            // 
            // chkIs_enabled
            // 
            this.chkIs_enabled.Location = new System.Drawing.Point(184, 215);
            this.chkIs_enabled.Name = "chkIs_enabled";
            this.chkIs_enabled.Size = new System.Drawing.Size(19, 13);
            this.chkIs_enabled.TabIndex = 73;
            this.chkIs_enabled.Values.Text = "";
            // 
            // lblGroupName
            // 
            this.lblGroupName.Location = new System.Drawing.Point(118, 12);
            this.lblGroupName.Name = "lblGroupName";
            this.lblGroupName.Size = new System.Drawing.Size(62, 20);
            this.lblGroupName.TabIndex = 53;
            this.lblGroupName.Values.Text = "组合名称";
            // 
            // txtGroupName
            // 
            this.txtGroupName.Location = new System.Drawing.Point(183, 12);
            this.txtGroupName.Name = "txtGroupName";
            this.txtGroupName.Size = new System.Drawing.Size(290, 23);
            this.txtGroupName.TabIndex = 54;
            // 
            // lblCurrencyCode
            // 
            this.lblCurrencyCode.Location = new System.Drawing.Point(118, 37);
            this.lblCurrencyCode.Name = "lblCurrencyCode";
            this.lblCurrencyCode.Size = new System.Drawing.Size(62, 20);
            this.lblCurrencyCode.TabIndex = 55;
            this.lblCurrencyCode.Values.Text = "外币代码";
            // 
            // txtCurrencyCode
            // 
            this.txtCurrencyCode.Location = new System.Drawing.Point(183, 37);
            this.txtCurrencyCode.Name = "txtCurrencyCode";
            this.txtCurrencyCode.Size = new System.Drawing.Size(290, 23);
            this.txtCurrencyCode.TabIndex = 56;
            // 
            // lblCurrencyName
            // 
            this.lblCurrencyName.Location = new System.Drawing.Point(118, 62);
            this.lblCurrencyName.Name = "lblCurrencyName";
            this.lblCurrencyName.Size = new System.Drawing.Size(62, 20);
            this.lblCurrencyName.TabIndex = 57;
            this.lblCurrencyName.Values.Text = "外币名称";
            // 
            // txtCurrencyName
            // 
            this.txtCurrencyName.Location = new System.Drawing.Point(183, 62);
            this.txtCurrencyName.Name = "txtCurrencyName";
            this.txtCurrencyName.Size = new System.Drawing.Size(290, 23);
            this.txtCurrencyName.TabIndex = 58;
            // 
            // lblAdjustDate
            // 
            this.lblAdjustDate.Location = new System.Drawing.Point(118, 87);
            this.lblAdjustDate.Name = "lblAdjustDate";
            this.lblAdjustDate.Size = new System.Drawing.Size(62, 20);
            this.lblAdjustDate.TabIndex = 59;
            this.lblAdjustDate.Values.Text = "调整日期";
            // 
            // lbl预设汇率
            // 
            this.lbl预设汇率.Location = new System.Drawing.Point(118, 112);
            this.lbl预设汇率.Name = "lbl预设汇率";
            this.lbl预设汇率.Size = new System.Drawing.Size(62, 20);
            this.lbl预设汇率.TabIndex = 61;
            this.lbl预设汇率.Values.Text = "预设汇率";
            // 
            // txtDefaultExchRate
            // 
            this.txtDefaultExchRate.Location = new System.Drawing.Point(183, 112);
            this.txtDefaultExchRate.Name = "txtDefaultExchRate";
            this.txtDefaultExchRate.Size = new System.Drawing.Size(290, 23);
            this.txtDefaultExchRate.TabIndex = 62;
            // 
            // lblBuyExchRate
            // 
            this.lblBuyExchRate.Location = new System.Drawing.Point(118, 137);
            this.lblBuyExchRate.Name = "lblBuyExchRate";
            this.lblBuyExchRate.Size = new System.Drawing.Size(62, 20);
            this.lblBuyExchRate.TabIndex = 63;
            this.lblBuyExchRate.Values.Text = "买入汇率";
            // 
            // txtBuyExchRate
            // 
            this.txtBuyExchRate.Location = new System.Drawing.Point(183, 137);
            this.txtBuyExchRate.Name = "txtBuyExchRate";
            this.txtBuyExchRate.Size = new System.Drawing.Size(290, 23);
            this.txtBuyExchRate.TabIndex = 64;
            // 
            // lblSellOutExchRate
            // 
            this.lblSellOutExchRate.Location = new System.Drawing.Point(118, 162);
            this.lblSellOutExchRate.Name = "lblSellOutExchRate";
            this.lblSellOutExchRate.Size = new System.Drawing.Size(62, 20);
            this.lblSellOutExchRate.TabIndex = 66;
            this.lblSellOutExchRate.Values.Text = "卖出汇率";
            // 
            // txtSellOutExchRate
            // 
            this.txtSellOutExchRate.Location = new System.Drawing.Point(183, 162);
            this.txtSellOutExchRate.Name = "txtSellOutExchRate";
            this.txtSellOutExchRate.Size = new System.Drawing.Size(290, 23);
            this.txtSellOutExchRate.TabIndex = 65;
            // 
            // lblMonthEndExchRate
            // 
            this.lblMonthEndExchRate.Location = new System.Drawing.Point(118, 187);
            this.lblMonthEndExchRate.Name = "lblMonthEndExchRate";
            this.lblMonthEndExchRate.Size = new System.Drawing.Size(62, 20);
            this.lblMonthEndExchRate.TabIndex = 67;
            this.lblMonthEndExchRate.Values.Text = "月末汇率";
            // 
            // txtMonthEndExchRate
            // 
            this.txtMonthEndExchRate.Location = new System.Drawing.Point(183, 187);
            this.txtMonthEndExchRate.Name = "txtMonthEndExchRate";
            this.txtMonthEndExchRate.Size = new System.Drawing.Size(290, 23);
            this.txtMonthEndExchRate.TabIndex = 68;
            // 
            // lblIs_enabled
            // 
            this.lblIs_enabled.Location = new System.Drawing.Point(118, 212);
            this.lblIs_enabled.Name = "lblIs_enabled";
            this.lblIs_enabled.Size = new System.Drawing.Size(62, 20);
            this.lblIs_enabled.TabIndex = 69;
            this.lblIs_enabled.Values.Text = "是否启用";
            // 
            // lblIs_available
            // 
            this.lblIs_available.Location = new System.Drawing.Point(118, 237);
            this.lblIs_available.Name = "lblIs_available";
            this.lblIs_available.Size = new System.Drawing.Size(62, 20);
            this.lblIs_available.TabIndex = 71;
            this.lblIs_available.Values.Text = "是否可用";
            // 
            // lblNotes
            // 
            this.lblNotes.Location = new System.Drawing.Point(79, 276);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(36, 20);
            this.lblNotes.TabIndex = 51;
            this.lblNotes.Values.Text = "备注";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(166, 276);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(307, 96);
            this.txtNotes.TabIndex = 52;
            // 
            // UCCurrencyEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 526);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCCurrencyEdit";
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonLabel lblNotes;
        private Krypton.Toolkit.KryptonTextBox txtNotes;
        private Krypton.Toolkit.KryptonLabel lblGroupName;
        private Krypton.Toolkit.KryptonTextBox txtGroupName;
        private Krypton.Toolkit.KryptonLabel lblCurrencyCode;
        private Krypton.Toolkit.KryptonTextBox txtCurrencyCode;
        private Krypton.Toolkit.KryptonLabel lblCurrencyName;
        private Krypton.Toolkit.KryptonTextBox txtCurrencyName;
        private Krypton.Toolkit.KryptonLabel lblAdjustDate;
        private Krypton.Toolkit.KryptonLabel lbl预设汇率;
        private Krypton.Toolkit.KryptonTextBox txtDefaultExchRate;
        private Krypton.Toolkit.KryptonLabel lblBuyExchRate;
        private Krypton.Toolkit.KryptonTextBox txtBuyExchRate;
        private Krypton.Toolkit.KryptonLabel lblSellOutExchRate;
        private Krypton.Toolkit.KryptonTextBox txtSellOutExchRate;
        private Krypton.Toolkit.KryptonLabel lblMonthEndExchRate;
        private Krypton.Toolkit.KryptonTextBox txtMonthEndExchRate;
        private Krypton.Toolkit.KryptonLabel lblIs_enabled;
        private Krypton.Toolkit.KryptonLabel lblIs_available;
        private Krypton.Toolkit.KryptonCheckBox chkIs_enabled;
        private Krypton.Toolkit.KryptonCheckBox chkIs_available;
        private Krypton.Toolkit.KryptonDateTimePicker dtpAdjustDate;
    }
}
