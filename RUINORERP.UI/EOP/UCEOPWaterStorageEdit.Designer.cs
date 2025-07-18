namespace RUINORERP.UI.EOP
{
    partial class UCEOPWaterStorageEdit
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
            this.cmbEmployee_ID = new Krypton.Toolkit.KryptonComboBox();
            this.cmbPlatformType = new Krypton.Toolkit.KryptonComboBox();
            this.cmbProjectGroup_ID = new Krypton.Toolkit.KryptonComboBox();
            this.lblWSRNo = new Krypton.Toolkit.KryptonLabel();
            this.txtWSRNo = new Krypton.Toolkit.KryptonTextBox();
            this.lblPlatformOrderNo = new Krypton.Toolkit.KryptonLabel();
            this.txtPlatformOrderNo = new Krypton.Toolkit.KryptonTextBox();
            this.lblPlatformType = new Krypton.Toolkit.KryptonLabel();
            this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
            this.lblProjectGroup_ID = new Krypton.Toolkit.KryptonLabel();
            this.lblTotalAmount = new Krypton.Toolkit.KryptonLabel();
            this.txtTotalAmount = new Krypton.Toolkit.KryptonTextBox();
            this.lblPlatformFeeAmount = new Krypton.Toolkit.KryptonLabel();
            this.txtPlatformFeeAmount = new Krypton.Toolkit.KryptonTextBox();
            this.lblOrderDate = new Krypton.Toolkit.KryptonLabel();
            this.dtpOrderDate = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lblShippingAddress = new Krypton.Toolkit.KryptonLabel();
            this.txtShippingAddress = new Krypton.Toolkit.KryptonTextBox();
            this.lblShippingWay = new Krypton.Toolkit.KryptonLabel();
            this.txtShippingWay = new Krypton.Toolkit.KryptonTextBox();
            this.lblTrackNo = new Krypton.Toolkit.KryptonLabel();
            this.txtTrackNo = new Krypton.Toolkit.KryptonTextBox();
            this.lblNotes = new Krypton.Toolkit.KryptonLabel();
            this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmployee_ID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPlatformType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbProjectGroup_ID)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(276, 590);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(394, 590);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.cmbEmployee_ID);
            this.kryptonPanel1.Controls.Add(this.cmbPlatformType);
            this.kryptonPanel1.Controls.Add(this.cmbProjectGroup_ID);
            this.kryptonPanel1.Controls.Add(this.lblWSRNo);
            this.kryptonPanel1.Controls.Add(this.txtWSRNo);
            this.kryptonPanel1.Controls.Add(this.lblPlatformOrderNo);
            this.kryptonPanel1.Controls.Add(this.txtPlatformOrderNo);
            this.kryptonPanel1.Controls.Add(this.lblPlatformType);
            this.kryptonPanel1.Controls.Add(this.lblEmployee_ID);
            this.kryptonPanel1.Controls.Add(this.lblProjectGroup_ID);
            this.kryptonPanel1.Controls.Add(this.lblTotalAmount);
            this.kryptonPanel1.Controls.Add(this.txtTotalAmount);
            this.kryptonPanel1.Controls.Add(this.lblPlatformFeeAmount);
            this.kryptonPanel1.Controls.Add(this.txtPlatformFeeAmount);
            this.kryptonPanel1.Controls.Add(this.lblOrderDate);
            this.kryptonPanel1.Controls.Add(this.dtpOrderDate);
            this.kryptonPanel1.Controls.Add(this.lblShippingAddress);
            this.kryptonPanel1.Controls.Add(this.txtShippingAddress);
            this.kryptonPanel1.Controls.Add(this.lblShippingWay);
            this.kryptonPanel1.Controls.Add(this.txtShippingWay);
            this.kryptonPanel1.Controls.Add(this.lblTrackNo);
            this.kryptonPanel1.Controls.Add(this.txtTrackNo);
            this.kryptonPanel1.Controls.Add(this.lblNotes);
            this.kryptonPanel1.Controls.Add(this.txtNotes);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(817, 683);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // cmbEmployee_ID
            // 
            this.cmbEmployee_ID.DropDownWidth = 100;
            this.cmbEmployee_ID.EnableSearch = false;
            this.cmbEmployee_ID.IntegralHeight = false;
            this.cmbEmployee_ID.Location = new System.Drawing.Point(468, 90);
            this.cmbEmployee_ID.Name = "cmbEmployee_ID";
            this.cmbEmployee_ID.Size = new System.Drawing.Size(198, 21);
            this.cmbEmployee_ID.TabIndex = 105;
            // 
            // cmbPlatformType
            // 
            this.cmbPlatformType.DropDownWidth = 296;
            this.cmbPlatformType.EnableSearch = false;
            this.cmbPlatformType.IntegralHeight = false;
            this.cmbPlatformType.Location = new System.Drawing.Point(468, 33);
            this.cmbPlatformType.Name = "cmbPlatformType";
            this.cmbPlatformType.Size = new System.Drawing.Size(198, 21);
            this.cmbPlatformType.TabIndex = 70;
            // 
            // cmbProjectGroup_ID
            // 
            this.cmbProjectGroup_ID.DropDownWidth = 296;
            this.cmbProjectGroup_ID.EnableSearch = false;
            this.cmbProjectGroup_ID.IntegralHeight = false;
            this.cmbProjectGroup_ID.Location = new System.Drawing.Point(469, 60);
            this.cmbProjectGroup_ID.Name = "cmbProjectGroup_ID";
            this.cmbProjectGroup_ID.Size = new System.Drawing.Size(198, 21);
            this.cmbProjectGroup_ID.TabIndex = 69;
            // 
            // lblWSRNo
            // 
            this.lblWSRNo.Location = new System.Drawing.Point(68, 33);
            this.lblWSRNo.Name = "lblWSRNo";
            this.lblWSRNo.Size = new System.Drawing.Size(62, 20);
            this.lblWSRNo.TabIndex = 27;
            this.lblWSRNo.Values.Text = "蓄水编号";
            // 
            // txtWSRNo
            // 
            this.txtWSRNo.Location = new System.Drawing.Point(141, 32);
            this.txtWSRNo.Name = "txtWSRNo";
            this.txtWSRNo.Size = new System.Drawing.Size(185, 23);
            this.txtWSRNo.TabIndex = 28;
            // 
            // lblPlatformOrderNo
            // 
            this.lblPlatformOrderNo.Location = new System.Drawing.Point(68, 90);
            this.lblPlatformOrderNo.Name = "lblPlatformOrderNo";
            this.lblPlatformOrderNo.Size = new System.Drawing.Size(62, 20);
            this.lblPlatformOrderNo.TabIndex = 29;
            this.lblPlatformOrderNo.Values.Text = "平台单号";
            // 
            // txtPlatformOrderNo
            // 
            this.txtPlatformOrderNo.Location = new System.Drawing.Point(141, 89);
            this.txtPlatformOrderNo.Name = "txtPlatformOrderNo";
            this.txtPlatformOrderNo.Size = new System.Drawing.Size(185, 23);
            this.txtPlatformOrderNo.TabIndex = 30;
            // 
            // lblPlatformType
            // 
            this.lblPlatformType.Location = new System.Drawing.Point(395, 33);
            this.lblPlatformType.Name = "lblPlatformType";
            this.lblPlatformType.Size = new System.Drawing.Size(62, 20);
            this.lblPlatformType.TabIndex = 31;
            this.lblPlatformType.Values.Text = "平台类型";
            // 
            // lblEmployee_ID
            // 
            this.lblEmployee_ID.Location = new System.Drawing.Point(396, 90);
            this.lblEmployee_ID.Name = "lblEmployee_ID";
            this.lblEmployee_ID.Size = new System.Drawing.Size(49, 20);
            this.lblEmployee_ID.TabIndex = 35;
            this.lblEmployee_ID.Values.Text = "业务员";
            // 
            // lblProjectGroup_ID
            // 
            this.lblProjectGroup_ID.Location = new System.Drawing.Point(414, 60);
            this.lblProjectGroup_ID.Name = "lblProjectGroup_ID";
            this.lblProjectGroup_ID.Size = new System.Drawing.Size(49, 20);
            this.lblProjectGroup_ID.TabIndex = 38;
            this.lblProjectGroup_ID.Values.Text = "项目组";
            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.Location = new System.Drawing.Point(81, 122);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(49, 20);
            this.lblTotalAmount.TabIndex = 39;
            this.lblTotalAmount.Values.Text = "总金额";
            // 
            // txtTotalAmount
            // 
            this.txtTotalAmount.Location = new System.Drawing.Point(141, 121);
            this.txtTotalAmount.Name = "txtTotalAmount";
            this.txtTotalAmount.Size = new System.Drawing.Size(185, 23);
            this.txtTotalAmount.TabIndex = 40;
            // 
            // lblPlatformFeeAmount
            // 
            this.lblPlatformFeeAmount.Location = new System.Drawing.Point(396, 122);
            this.lblPlatformFeeAmount.Name = "lblPlatformFeeAmount";
            this.lblPlatformFeeAmount.Size = new System.Drawing.Size(62, 20);
            this.lblPlatformFeeAmount.TabIndex = 41;
            this.lblPlatformFeeAmount.Values.Text = "平台费用";
            // 
            // txtPlatformFeeAmount
            // 
            this.txtPlatformFeeAmount.Location = new System.Drawing.Point(469, 121);
            this.txtPlatformFeeAmount.Name = "txtPlatformFeeAmount";
            this.txtPlatformFeeAmount.Size = new System.Drawing.Size(198, 23);
            this.txtPlatformFeeAmount.TabIndex = 42;
            // 
            // lblOrderDate
            // 
            this.lblOrderDate.Location = new System.Drawing.Point(68, 60);
            this.lblOrderDate.Name = "lblOrderDate";
            this.lblOrderDate.Size = new System.Drawing.Size(62, 20);
            this.lblOrderDate.TabIndex = 43;
            this.lblOrderDate.Values.Text = "订单日期";
            // 
            // dtpOrderDate
            // 
            this.dtpOrderDate.Location = new System.Drawing.Point(141, 60);
            this.dtpOrderDate.Name = "dtpOrderDate";
            this.dtpOrderDate.Size = new System.Drawing.Size(185, 21);
            this.dtpOrderDate.TabIndex = 44;
            // 
            // lblShippingAddress
            // 
            this.lblShippingAddress.Location = new System.Drawing.Point(68, 200);
            this.lblShippingAddress.Name = "lblShippingAddress";
            this.lblShippingAddress.Size = new System.Drawing.Size(62, 20);
            this.lblShippingAddress.TabIndex = 45;
            this.lblShippingAddress.Values.Text = "收货地址";
            // 
            // txtShippingAddress
            // 
            this.txtShippingAddress.Location = new System.Drawing.Point(141, 196);
            this.txtShippingAddress.Multiline = true;
            this.txtShippingAddress.Name = "txtShippingAddress";
            this.txtShippingAddress.Size = new System.Drawing.Size(525, 177);
            this.txtShippingAddress.TabIndex = 46;
            // 
            // lblShippingWay
            // 
            this.lblShippingWay.Location = new System.Drawing.Point(68, 154);
            this.lblShippingWay.Name = "lblShippingWay";
            this.lblShippingWay.Size = new System.Drawing.Size(62, 20);
            this.lblShippingWay.TabIndex = 48;
            this.lblShippingWay.Values.Text = "发货方式";
            // 
            // txtShippingWay
            // 
            this.txtShippingWay.Location = new System.Drawing.Point(140, 153);
            this.txtShippingWay.Name = "txtShippingWay";
            this.txtShippingWay.Size = new System.Drawing.Size(185, 23);
            this.txtShippingWay.TabIndex = 47;
            // 
            // lblTrackNo
            // 
            this.lblTrackNo.Location = new System.Drawing.Point(394, 154);
            this.lblTrackNo.Name = "lblTrackNo";
            this.lblTrackNo.Size = new System.Drawing.Size(62, 20);
            this.lblTrackNo.TabIndex = 49;
            this.lblTrackNo.Values.Text = "物流单号";
            // 
            // txtTrackNo
            // 
            this.txtTrackNo.Location = new System.Drawing.Point(468, 153);
            this.txtTrackNo.Name = "txtTrackNo";
            this.txtTrackNo.Size = new System.Drawing.Size(198, 23);
            this.txtTrackNo.TabIndex = 50;
            // 
            // lblNotes
            // 
            this.lblNotes.Location = new System.Drawing.Point(94, 408);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(36, 20);
            this.lblNotes.TabIndex = 51;
            this.lblNotes.Values.Text = "备注";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(130, 404);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(536, 137);
            this.txtNotes.TabIndex = 52;
            // 
            // UCEOPWaterStorageRegisterEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(817, 683);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCEOPWaterStorageRegisterEdit";
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmployee_ID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPlatformType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbProjectGroup_ID)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonLabel lblWSRNo;
        private Krypton.Toolkit.KryptonTextBox txtWSRNo;
        private Krypton.Toolkit.KryptonLabel lblPlatformOrderNo;
        private Krypton.Toolkit.KryptonTextBox txtPlatformOrderNo;
        private Krypton.Toolkit.KryptonLabel lblPlatformType;
        private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
        private Krypton.Toolkit.KryptonLabel lblProjectGroup_ID;
        private Krypton.Toolkit.KryptonLabel lblTotalAmount;
        private Krypton.Toolkit.KryptonTextBox txtTotalAmount;
        private Krypton.Toolkit.KryptonLabel lblPlatformFeeAmount;
        private Krypton.Toolkit.KryptonTextBox txtPlatformFeeAmount;
        private Krypton.Toolkit.KryptonLabel lblOrderDate;
        private Krypton.Toolkit.KryptonDateTimePicker dtpOrderDate;
        private Krypton.Toolkit.KryptonLabel lblShippingAddress;
        private Krypton.Toolkit.KryptonTextBox txtShippingAddress;
        private Krypton.Toolkit.KryptonLabel lblShippingWay;
        private Krypton.Toolkit.KryptonTextBox txtShippingWay;
        private Krypton.Toolkit.KryptonLabel lblTrackNo;
        private Krypton.Toolkit.KryptonTextBox txtTrackNo;
        private Krypton.Toolkit.KryptonLabel lblNotes;
        private Krypton.Toolkit.KryptonTextBox txtNotes;
        private Krypton.Toolkit.KryptonComboBox cmbProjectGroup_ID;
        private Krypton.Toolkit.KryptonComboBox cmbPlatformType;
        private Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;
    }
}
