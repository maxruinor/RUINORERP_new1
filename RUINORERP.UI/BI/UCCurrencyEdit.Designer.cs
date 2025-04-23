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
            this.chkIs_BaseCurrency = new Krypton.Toolkit.KryptonCheckBox();
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            this.chkIs_enabled = new Krypton.Toolkit.KryptonCheckBox();
            this.lblCountry = new Krypton.Toolkit.KryptonLabel();
            this.txtCountry = new Krypton.Toolkit.KryptonTextBox();
            this.lblCurrencyCode = new Krypton.Toolkit.KryptonLabel();
            this.txtCurrencyCode = new Krypton.Toolkit.KryptonTextBox();
            this.lblCurrencyName = new Krypton.Toolkit.KryptonLabel();
            this.txtCurrencyName = new Krypton.Toolkit.KryptonTextBox();
            this.lblCurrencySymbol = new Krypton.Toolkit.KryptonLabel();
            this.txtCurrencySymbol = new Krypton.Toolkit.KryptonTextBox();
            this.lblIs_enabled = new Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel2 = new Krypton.Toolkit.KryptonLabel();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(133, 307);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(265, 307);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.kryptonLabel2);
            this.kryptonPanel1.Controls.Add(this.chkIs_BaseCurrency);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel1);
            this.kryptonPanel1.Controls.Add(this.chkIs_enabled);
            this.kryptonPanel1.Controls.Add(this.lblCountry);
            this.kryptonPanel1.Controls.Add(this.txtCountry);
            this.kryptonPanel1.Controls.Add(this.lblCurrencyCode);
            this.kryptonPanel1.Controls.Add(this.txtCurrencyCode);
            this.kryptonPanel1.Controls.Add(this.lblCurrencyName);
            this.kryptonPanel1.Controls.Add(this.txtCurrencyName);
            this.kryptonPanel1.Controls.Add(this.lblCurrencySymbol);
            this.kryptonPanel1.Controls.Add(this.txtCurrencySymbol);
            this.kryptonPanel1.Controls.Add(this.lblIs_enabled);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(479, 395);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // chkIs_BaseCurrency
            // 
            this.chkIs_BaseCurrency.Location = new System.Drawing.Point(124, 157);
            this.chkIs_BaseCurrency.Name = "chkIs_BaseCurrency";
            this.chkIs_BaseCurrency.Size = new System.Drawing.Size(19, 13);
            this.chkIs_BaseCurrency.TabIndex = 76;
            this.chkIs_BaseCurrency.Values.Text = "";
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(58, 154);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(62, 20);
            this.kryptonLabel1.TabIndex = 75;
            this.kryptonLabel1.Values.Text = "为本位币";
            // 
            // chkIs_enabled
            // 
            this.chkIs_enabled.Location = new System.Drawing.Point(124, 194);
            this.chkIs_enabled.Name = "chkIs_enabled";
            this.chkIs_enabled.Size = new System.Drawing.Size(19, 13);
            this.chkIs_enabled.TabIndex = 73;
            this.chkIs_enabled.Values.Text = "";
            // 
            // lblCountry
            // 
            this.lblCountry.Location = new System.Drawing.Point(82, 24);
            this.lblCountry.Name = "lblCountry";
            this.lblCountry.Size = new System.Drawing.Size(36, 20);
            this.lblCountry.TabIndex = 53;
            this.lblCountry.Values.Text = "国家";
            // 
            // txtCountry
            // 
            this.txtCountry.Location = new System.Drawing.Point(124, 24);
            this.txtCountry.Name = "txtCountry";
            this.txtCountry.Size = new System.Drawing.Size(290, 23);
            this.txtCountry.TabIndex = 54;
            // 
            // lblCurrencyCode
            // 
            this.lblCurrencyCode.Location = new System.Drawing.Point(59, 53);
            this.lblCurrencyCode.Name = "lblCurrencyCode";
            this.lblCurrencyCode.Size = new System.Drawing.Size(62, 20);
            this.lblCurrencyCode.TabIndex = 55;
            this.lblCurrencyCode.Values.Text = "币别代码";
            // 
            // txtCurrencyCode
            // 
            this.txtCurrencyCode.Location = new System.Drawing.Point(124, 53);
            this.txtCurrencyCode.Name = "txtCurrencyCode";
            this.txtCurrencyCode.Size = new System.Drawing.Size(290, 23);
            this.txtCurrencyCode.TabIndex = 56;
            // 
            // lblCurrencyName
            // 
            this.lblCurrencyName.Location = new System.Drawing.Point(59, 87);
            this.lblCurrencyName.Name = "lblCurrencyName";
            this.lblCurrencyName.Size = new System.Drawing.Size(62, 20);
            this.lblCurrencyName.TabIndex = 57;
            this.lblCurrencyName.Values.Text = "币别名称";
            // 
            // txtCurrencyName
            // 
            this.txtCurrencyName.Location = new System.Drawing.Point(124, 87);
            this.txtCurrencyName.Name = "txtCurrencyName";
            this.txtCurrencyName.Size = new System.Drawing.Size(290, 23);
            this.txtCurrencyName.TabIndex = 58;
            // 
            // lblCurrencySymbol
            // 
            this.lblCurrencySymbol.Location = new System.Drawing.Point(59, 116);
            this.lblCurrencySymbol.Name = "lblCurrencySymbol";
            this.lblCurrencySymbol.Size = new System.Drawing.Size(62, 20);
            this.lblCurrencySymbol.TabIndex = 61;
            this.lblCurrencySymbol.Values.Text = "币别符号";
            // 
            // txtCurrencySymbol
            // 
            this.txtCurrencySymbol.Location = new System.Drawing.Point(124, 116);
            this.txtCurrencySymbol.Name = "txtCurrencySymbol";
            this.txtCurrencySymbol.Size = new System.Drawing.Size(290, 23);
            this.txtCurrencySymbol.TabIndex = 62;
            // 
            // lblIs_enabled
            // 
            this.lblIs_enabled.Location = new System.Drawing.Point(58, 191);
            this.lblIs_enabled.Name = "lblIs_enabled";
            this.lblIs_enabled.Size = new System.Drawing.Size(62, 20);
            this.lblIs_enabled.TabIndex = 69;
            this.lblIs_enabled.Values.Text = "是否启用";
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(149, 157);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(295, 20);
            this.kryptonLabel2.TabIndex = 77;
            this.kryptonLabel2.Values.Text = "本位币是系统结算基本币，只有一个币种为本位币";
            // 
            // UCCurrencyEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 395);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCCurrencyEdit";
            this.Load += new System.EventHandler(this.UCCurrencyEdit_Load);
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
        private Krypton.Toolkit.KryptonLabel lblCountry;
        private Krypton.Toolkit.KryptonTextBox txtCountry;
        private Krypton.Toolkit.KryptonLabel lblCurrencyCode;
        private Krypton.Toolkit.KryptonTextBox txtCurrencyCode;
        private Krypton.Toolkit.KryptonLabel lblCurrencyName;
        private Krypton.Toolkit.KryptonTextBox txtCurrencyName;
        private Krypton.Toolkit.KryptonLabel lblCurrencySymbol;
        private Krypton.Toolkit.KryptonTextBox txtCurrencySymbol;
        private Krypton.Toolkit.KryptonLabel lblIs_enabled;
        private Krypton.Toolkit.KryptonCheckBox chkIs_enabled;
        private Krypton.Toolkit.KryptonCheckBox chkIs_BaseCurrency;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private Krypton.Toolkit.KryptonLabel kryptonLabel2;
    }
}
