namespace RUINORERP.UI.BI
{
    partial class UCBillingInformationEdit
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
            this.lblCustomerVendor_ID = new Krypton.Toolkit.KryptonLabel();
            this.cmbCustomerVendor_ID = new Krypton.Toolkit.KryptonComboBox();
            this.lblTitle = new Krypton.Toolkit.KryptonLabel();
            this.txtTitle = new Krypton.Toolkit.KryptonTextBox();
            this.lblTaxNumber = new Krypton.Toolkit.KryptonLabel();
            this.txtTaxNumber = new Krypton.Toolkit.KryptonTextBox();
            this.lblAddress = new Krypton.Toolkit.KryptonLabel();
            this.txtAddress = new Krypton.Toolkit.KryptonTextBox();
            this.lblPITEL = new Krypton.Toolkit.KryptonLabel();
            this.txtPITEL = new Krypton.Toolkit.KryptonTextBox();
            this.lblBankAccount = new Krypton.Toolkit.KryptonLabel();
            this.txtBankAccount = new Krypton.Toolkit.KryptonTextBox();
            this.lblBankName = new Krypton.Toolkit.KryptonLabel();
            this.txtBankName = new Krypton.Toolkit.KryptonTextBox();
            this.lblEmail = new Krypton.Toolkit.KryptonLabel();
            this.txtEmail = new Krypton.Toolkit.KryptonTextBox();
            this.lblNotes = new Krypton.Toolkit.KryptonLabel();
            this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
            this.lblIsActive = new Krypton.Toolkit.KryptonLabel();
            this.chkIsActive = new Krypton.Toolkit.KryptonCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCustomerVendor_ID)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(193, 397);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(311, 397);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.lblCustomerVendor_ID);
            this.kryptonPanel1.Controls.Add(this.cmbCustomerVendor_ID);
            this.kryptonPanel1.Controls.Add(this.lblTitle);
            this.kryptonPanel1.Controls.Add(this.txtTitle);
            this.kryptonPanel1.Controls.Add(this.lblTaxNumber);
            this.kryptonPanel1.Controls.Add(this.txtTaxNumber);
            this.kryptonPanel1.Controls.Add(this.lblAddress);
            this.kryptonPanel1.Controls.Add(this.txtAddress);
            this.kryptonPanel1.Controls.Add(this.lblPITEL);
            this.kryptonPanel1.Controls.Add(this.txtPITEL);
            this.kryptonPanel1.Controls.Add(this.lblBankAccount);
            this.kryptonPanel1.Controls.Add(this.txtBankAccount);
            this.kryptonPanel1.Controls.Add(this.lblBankName);
            this.kryptonPanel1.Controls.Add(this.txtBankName);
            this.kryptonPanel1.Controls.Add(this.lblEmail);
            this.kryptonPanel1.Controls.Add(this.txtEmail);
            this.kryptonPanel1.Controls.Add(this.lblNotes);
            this.kryptonPanel1.Controls.Add(this.txtNotes);
            this.kryptonPanel1.Controls.Add(this.lblIsActive);
            this.kryptonPanel1.Controls.Add(this.chkIsActive);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(554, 459);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // lblCustomerVendor_ID
            // 
            this.lblCustomerVendor_ID.Location = new System.Drawing.Point(63, 16);
            this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
            this.lblCustomerVendor_ID.Size = new System.Drawing.Size(62, 20);
            this.lblCustomerVendor_ID.TabIndex = 25;
            this.lblCustomerVendor_ID.Values.Text = "往来单位";
            // 
            // cmbCustomerVendor_ID
            // 
            this.cmbCustomerVendor_ID.DropDownWidth = 100;
            this.cmbCustomerVendor_ID.IntegralHeight = false;
            this.cmbCustomerVendor_ID.Location = new System.Drawing.Point(136, 12);
            this.cmbCustomerVendor_ID.Name = "cmbCustomerVendor_ID";
            this.cmbCustomerVendor_ID.Size = new System.Drawing.Size(368, 21);
            this.cmbCustomerVendor_ID.TabIndex = 26;
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(89, 41);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(36, 20);
            this.lblTitle.TabIndex = 27;
            this.lblTitle.Values.Text = "抬头";
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(136, 37);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(368, 23);
            this.txtTitle.TabIndex = 28;
            // 
            // lblTaxNumber
            // 
            this.lblTaxNumber.Location = new System.Drawing.Point(89, 66);
            this.lblTaxNumber.Name = "lblTaxNumber";
            this.lblTaxNumber.Size = new System.Drawing.Size(36, 20);
            this.lblTaxNumber.TabIndex = 29;
            this.lblTaxNumber.Values.Text = "税号";
            // 
            // txtTaxNumber
            // 
            this.txtTaxNumber.Location = new System.Drawing.Point(136, 62);
            this.txtTaxNumber.Name = "txtTaxNumber";
            this.txtTaxNumber.Size = new System.Drawing.Size(368, 23);
            this.txtTaxNumber.TabIndex = 30;
            // 
            // lblAddress
            // 
            this.lblAddress.Location = new System.Drawing.Point(89, 91);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(36, 20);
            this.lblAddress.TabIndex = 31;
            this.lblAddress.Values.Text = "地址";
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(136, 87);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(368, 23);
            this.txtAddress.TabIndex = 32;
            // 
            // lblPITEL
            // 
            this.lblPITEL.Location = new System.Drawing.Point(89, 116);
            this.lblPITEL.Name = "lblPITEL";
            this.lblPITEL.Size = new System.Drawing.Size(36, 20);
            this.lblPITEL.TabIndex = 33;
            this.lblPITEL.Values.Text = "电话";
            // 
            // txtPITEL
            // 
            this.txtPITEL.Location = new System.Drawing.Point(136, 112);
            this.txtPITEL.Name = "txtPITEL";
            this.txtPITEL.Size = new System.Drawing.Size(368, 23);
            this.txtPITEL.TabIndex = 34;
            // 
            // lblBankAccount
            // 
            this.lblBankAccount.Location = new System.Drawing.Point(63, 141);
            this.lblBankAccount.Name = "lblBankAccount";
            this.lblBankAccount.Size = new System.Drawing.Size(62, 20);
            this.lblBankAccount.TabIndex = 36;
            this.lblBankAccount.Values.Text = "银行账号";
            // 
            // txtBankAccount
            // 
            this.txtBankAccount.Location = new System.Drawing.Point(136, 137);
            this.txtBankAccount.Name = "txtBankAccount";
            this.txtBankAccount.Size = new System.Drawing.Size(368, 23);
            this.txtBankAccount.TabIndex = 35;
            // 
            // lblBankName
            // 
            this.lblBankName.Location = new System.Drawing.Point(76, 166);
            this.lblBankName.Name = "lblBankName";
            this.lblBankName.Size = new System.Drawing.Size(49, 20);
            this.lblBankName.TabIndex = 37;
            this.lblBankName.Values.Text = "开户行";
            // 
            // txtBankName
            // 
            this.txtBankName.Location = new System.Drawing.Point(136, 162);
            this.txtBankName.Name = "txtBankName";
            this.txtBankName.Size = new System.Drawing.Size(368, 23);
            this.txtBankName.TabIndex = 38;
            // 
            // lblEmail
            // 
            this.lblEmail.Location = new System.Drawing.Point(89, 191);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(36, 20);
            this.lblEmail.TabIndex = 39;
            this.lblEmail.Values.Text = "邮箱";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(136, 187);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(368, 23);
            this.txtEmail.TabIndex = 40;
            // 
            // lblNotes
            // 
            this.lblNotes.Location = new System.Drawing.Point(89, 220);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(36, 20);
            this.lblNotes.TabIndex = 46;
            this.lblNotes.Values.Text = "备注";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(136, 216);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(368, 111);
            this.txtNotes.TabIndex = 45;
            // 
            // lblIsActive
            // 
            this.lblIsActive.Location = new System.Drawing.Point(89, 338);
            this.lblIsActive.Name = "lblIsActive";
            this.lblIsActive.Size = new System.Drawing.Size(36, 20);
            this.lblIsActive.TabIndex = 69;
            this.lblIsActive.Values.Text = "激活";
            // 
            // chkIsActive
            // 
            this.chkIsActive.Checked = true;
            this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsActive.Location = new System.Drawing.Point(136, 342);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(19, 13);
            this.chkIsActive.TabIndex = 70;
            this.chkIsActive.Values.Text = "";
            // 
            // UCBillingInformationEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 459);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCBillingInformationEdit";
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCustomerVendor_ID)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
        private Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;
        private Krypton.Toolkit.KryptonLabel lblTitle;
        private Krypton.Toolkit.KryptonTextBox txtTitle;
        private Krypton.Toolkit.KryptonLabel lblTaxNumber;
        private Krypton.Toolkit.KryptonTextBox txtTaxNumber;
        private Krypton.Toolkit.KryptonLabel lblAddress;
        private Krypton.Toolkit.KryptonTextBox txtAddress;
        private Krypton.Toolkit.KryptonLabel lblPITEL;
        private Krypton.Toolkit.KryptonTextBox txtPITEL;
        private Krypton.Toolkit.KryptonLabel lblBankAccount;
        private Krypton.Toolkit.KryptonTextBox txtBankAccount;
        private Krypton.Toolkit.KryptonLabel lblBankName;
        private Krypton.Toolkit.KryptonTextBox txtBankName;
        private Krypton.Toolkit.KryptonLabel lblEmail;
        private Krypton.Toolkit.KryptonTextBox txtEmail;
        private Krypton.Toolkit.KryptonLabel lblNotes;
        private Krypton.Toolkit.KryptonTextBox txtNotes;
        private Krypton.Toolkit.KryptonLabel lblIsActive;
        private Krypton.Toolkit.KryptonCheckBox chkIsActive;
    }
}
