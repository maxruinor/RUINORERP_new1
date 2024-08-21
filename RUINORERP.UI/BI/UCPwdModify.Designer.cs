namespace RUINORERP.UI.BI
{
    partial class UCPwdModify
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
            this.btnCancel = new Krypton.Toolkit.KryptonButton();
            this.btnOk = new Krypton.Toolkit.KryptonButton();
            this.lblQtyDataPrecision = new Krypton.Toolkit.KryptonLabel();
            this.txtOldPwd = new Krypton.Toolkit.KryptonTextBox();
            this.lblTaxRateDataPrecision = new Krypton.Toolkit.KryptonLabel();
            this.txtNewPwd = new Krypton.Toolkit.KryptonTextBox();
            this.lblMoneyDataPrecision = new Krypton.Toolkit.KryptonLabel();
            this.txtNewPwdConfirm = new Krypton.Toolkit.KryptonTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Controls.Add(this.lblQtyDataPrecision);
            this.kryptonPanel1.Controls.Add(this.txtOldPwd);
            this.kryptonPanel1.Controls.Add(this.lblTaxRateDataPrecision);
            this.kryptonPanel1.Controls.Add(this.txtNewPwd);
            this.kryptonPanel1.Controls.Add(this.lblMoneyDataPrecision);
            this.kryptonPanel1.Controls.Add(this.txtNewPwdConfirm);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(536, 452);
            this.kryptonPanel1.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(326, 309);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(159, 309);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 10;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // lblQtyDataPrecision
            // 
            this.lblQtyDataPrecision.Location = new System.Drawing.Point(142, 132);
            this.lblQtyDataPrecision.Name = "lblQtyDataPrecision";
            this.lblQtyDataPrecision.Size = new System.Drawing.Size(48, 20);
            this.lblQtyDataPrecision.TabIndex = 4;
            this.lblQtyDataPrecision.Values.Text = "旧密码";
            // 
            // txtOldPwd
            // 
            this.txtOldPwd.Location = new System.Drawing.Point(215, 128);
            this.txtOldPwd.Name = "txtOldPwd";
            this.txtOldPwd.PasswordChar = '●';
            this.txtOldPwd.Size = new System.Drawing.Size(201, 20);
            this.txtOldPwd.TabIndex = 5;
            this.txtOldPwd.UseSystemPasswordChar = true;
            // 
            // lblTaxRateDataPrecision
            // 
            this.lblTaxRateDataPrecision.Location = new System.Drawing.Point(142, 185);
            this.lblTaxRateDataPrecision.Name = "lblTaxRateDataPrecision";
            this.lblTaxRateDataPrecision.Size = new System.Drawing.Size(48, 20);
            this.lblTaxRateDataPrecision.TabIndex = 6;
            this.lblTaxRateDataPrecision.Values.Text = "新密码";
            // 
            // txtNewPwd
            // 
            this.txtNewPwd.Location = new System.Drawing.Point(215, 181);
            this.txtNewPwd.Name = "txtNewPwd";
            this.txtNewPwd.PasswordChar = '●';
            this.txtNewPwd.Size = new System.Drawing.Size(201, 20);
            this.txtNewPwd.TabIndex = 7;
            this.txtNewPwd.UseSystemPasswordChar = true;
            // 
            // lblMoneyDataPrecision
            // 
            this.lblMoneyDataPrecision.Location = new System.Drawing.Point(117, 229);
            this.lblMoneyDataPrecision.Name = "lblMoneyDataPrecision";
            this.lblMoneyDataPrecision.Size = new System.Drawing.Size(73, 20);
            this.lblMoneyDataPrecision.TabIndex = 8;
            this.lblMoneyDataPrecision.Values.Text = "确认新密码";
            // 
            // txtNewPwdConfirm
            // 
            this.txtNewPwdConfirm.Location = new System.Drawing.Point(215, 229);
            this.txtNewPwdConfirm.Name = "txtNewPwdConfirm";
            this.txtNewPwdConfirm.PasswordChar = '●';
            this.txtNewPwdConfirm.Size = new System.Drawing.Size(201, 20);
            this.txtNewPwdConfirm.TabIndex = 9;
            this.txtNewPwdConfirm.UseSystemPasswordChar = true;
            // 
            // UCPwdModify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCPwdModify";
            this.Size = new System.Drawing.Size(536, 452);
            this.Load += new System.EventHandler(this.UCPwdModify_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonLabel lblQtyDataPrecision;
        private Krypton.Toolkit.KryptonTextBox txtOldPwd;
        private Krypton.Toolkit.KryptonLabel lblTaxRateDataPrecision;
        private Krypton.Toolkit.KryptonTextBox txtNewPwd;
        private Krypton.Toolkit.KryptonLabel lblMoneyDataPrecision;
        private Krypton.Toolkit.KryptonTextBox txtNewPwdConfirm;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonButton btnOk;
    }
}
