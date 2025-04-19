namespace RUINORERP.UI.BI
{
    partial class UCPaymentMethodEdit
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
            this.chkCash = new Krypton.Toolkit.KryptonCheckBox();
            this.lblPaytype_Name = new Krypton.Toolkit.KryptonLabel();
            this.txtPaytype_Name = new Krypton.Toolkit.KryptonTextBox();
            this.lblDesc = new Krypton.Toolkit.KryptonLabel();
            this.txtDesc = new Krypton.Toolkit.KryptonTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(97, 263);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(215, 263);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.kryptonLabel1);
            this.kryptonPanel1.Controls.Add(this.chkCash);
            this.kryptonPanel1.Controls.Add(this.lblPaytype_Name);
            this.kryptonPanel1.Controls.Add(this.txtPaytype_Name);
            this.kryptonPanel1.Controls.Add(this.lblDesc);
            this.kryptonPanel1.Controls.Add(this.txtDesc);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(404, 300);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(47, 217);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(347, 20);
            this.kryptonLabel1.TabIndex = 84;
            this.kryptonLabel1.Values.Text = "（除账期外的，现金，银行卡，网络支付等都为即时到账）";
            // 
            // chkCash
            // 
            this.chkCash.Location = new System.Drawing.Point(83, 185);
            this.chkCash.Name = "chkCash";
            this.chkCash.Size = new System.Drawing.Size(101, 20);
            this.chkCash.TabIndex = 83;
            this.chkCash.Values.Text = "即时到账方式";
            // 
            // lblPaytype_Name
            // 
            this.lblPaytype_Name.Location = new System.Drawing.Point(12, 12);
            this.lblPaytype_Name.Name = "lblPaytype_Name";
            this.lblPaytype_Name.Size = new System.Drawing.Size(62, 20);
            this.lblPaytype_Name.TabIndex = 3;
            this.lblPaytype_Name.Values.Text = "付款方式";
            // 
            // txtPaytype_Name
            // 
            this.txtPaytype_Name.Location = new System.Drawing.Point(83, 12);
            this.txtPaytype_Name.Name = "txtPaytype_Name";
            this.txtPaytype_Name.Size = new System.Drawing.Size(251, 23);
            this.txtPaytype_Name.TabIndex = 4;
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(37, 61);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(36, 20);
            this.lblDesc.TabIndex = 5;
            this.lblDesc.Values.Text = "描述";
            // 
            // txtDesc
            // 
            this.txtDesc.Location = new System.Drawing.Point(83, 65);
            this.txtDesc.Multiline = true;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(251, 99);
            this.txtDesc.TabIndex = 6;
            // 
            // UCPaymentMethodEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCPaymentMethodEdit";
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
        private Krypton.Toolkit.KryptonLabel lblPaytype_Name;
        private Krypton.Toolkit.KryptonTextBox txtPaytype_Name;
        private Krypton.Toolkit.KryptonLabel lblDesc;
        private Krypton.Toolkit.KryptonTextBox txtDesc;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private Krypton.Toolkit.KryptonCheckBox chkCash;
    }
}
