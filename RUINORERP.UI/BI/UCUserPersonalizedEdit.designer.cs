namespace RUINORERP.UI.BI
{
    partial class UCUserPersonalizedEdit
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
            this.lblDesc = new Krypton.Toolkit.KryptonLabel();
            this.chkSelectTemplatePrint = new Krypton.Toolkit.KryptonCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(198, 380);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(316, 380);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.lblDesc);
            this.kryptonPanel1.Controls.Add(this.chkSelectTemplatePrint);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(607, 459);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(142, 51);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(243, 20);
            this.lblDesc.TabIndex = 23;
            this.lblDesc.Values.Text = "（单据要根据不同模板打印时，请勾选）";
            // 
            // chkSelectTemplatePrint
            // 
            this.chkSelectTemplatePrint.Location = new System.Drawing.Point(35, 51);
            this.chkSelectTemplatePrint.Name = "chkSelectTemplatePrint";
            this.chkSelectTemplatePrint.Size = new System.Drawing.Size(127, 20);
            this.chkSelectTemplatePrint.TabIndex = 22;
            this.chkSelectTemplatePrint.Values.Text = "手动选择打印模板";
            // 
            // UCUserPersonalizedEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCUserPersonalizedEdit";
            this.Size = new System.Drawing.Size(607, 459);
            this.Load += new System.EventHandler(this.UCUserPersonalizedEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonCheckBox chkSelectTemplatePrint;
        private Krypton.Toolkit.KryptonLabel lblDesc;
    }
}
