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
            this.chkSelectPrinter = new Krypton.Toolkit.KryptonCheckBox();
            this.GroupBoxSelectPrinter = new Krypton.Toolkit.KryptonGroupBox();
            this.cmbPrinterList = new Krypton.Toolkit.KryptonComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GroupBoxSelectPrinter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GroupBoxSelectPrinter.Panel)).BeginInit();
            this.GroupBoxSelectPrinter.Panel.SuspendLayout();
            this.GroupBoxSelectPrinter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPrinterList)).BeginInit();
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
            this.kryptonPanel1.Controls.Add(this.chkSelectPrinter);
            this.kryptonPanel1.Controls.Add(this.GroupBoxSelectPrinter);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(607, 459);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // chkSelectPrinter
            // 
            this.chkSelectPrinter.Location = new System.Drawing.Point(37, 72);
            this.chkSelectPrinter.Name = "chkSelectPrinter";
            this.chkSelectPrinter.Size = new System.Drawing.Size(114, 20);
            this.chkSelectPrinter.TabIndex = 21;
            this.chkSelectPrinter.Values.Text = "设置指定打印机";
            // 
            // GroupBoxSelectPrinter
            // 
            this.GroupBoxSelectPrinter.Location = new System.Drawing.Point(37, 98);
            this.GroupBoxSelectPrinter.Name = "GroupBoxSelectPrinter";
            // 
            // GroupBoxSelectPrinter.Panel
            // 
            this.GroupBoxSelectPrinter.Panel.Controls.Add(this.cmbPrinterList);
            this.GroupBoxSelectPrinter.Size = new System.Drawing.Size(535, 57);
            this.GroupBoxSelectPrinter.TabIndex = 20;
            this.GroupBoxSelectPrinter.Values.Heading = "专属打印机默认名称";
            // 
            // cmbPrinterList
            // 
            this.cmbPrinterList.DropDownWidth = 429;
            this.cmbPrinterList.IntegralHeight = false;
            this.cmbPrinterList.Location = new System.Drawing.Point(3, 7);
            this.cmbPrinterList.Name = "cmbPrinterList";
            this.cmbPrinterList.Size = new System.Drawing.Size(514, 21);
            this.cmbPrinterList.TabIndex = 11;
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
            ((System.ComponentModel.ISupportInitialize)(this.GroupBoxSelectPrinter.Panel)).EndInit();
            this.GroupBoxSelectPrinter.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GroupBoxSelectPrinter)).EndInit();
            this.GroupBoxSelectPrinter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbPrinterList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonGroupBox GroupBoxSelectPrinter;
        private Krypton.Toolkit.KryptonComboBox cmbPrinterList;
        private Krypton.Toolkit.KryptonCheckBox chkSelectPrinter;
    }
}
