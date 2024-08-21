namespace RUINORERP.UI.BI
{
    partial class UCUnitEdit
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
            this.kryptonGroupBox1 = new Krypton.Toolkit.KryptonGroupBox();
            this.rdbis_enabledNo = new Krypton.Toolkit.KryptonRadioButton();
            this.rdbis_enabledYes = new Krypton.Toolkit.KryptonRadioButton();
            this.lblis_measurement_unit = new Krypton.Toolkit.KryptonLabel();
            this.btnCancel = new Krypton.Toolkit.KryptonButton();
            this.btnOk = new Krypton.Toolkit.KryptonButton();
            this.txtDesc = new Krypton.Toolkit.KryptonTextBox();
            this.lblDesc = new Krypton.Toolkit.KryptonLabel();
            this.txtUnitName = new Krypton.Toolkit.KryptonTextBox();
            this.lblUnitName = new Krypton.Toolkit.KryptonLabel();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).BeginInit();
            this.kryptonGroupBox1.Panel.SuspendLayout();
            this.kryptonGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.kryptonGroupBox1);
            this.kryptonPanel1.Controls.Add(this.lblis_measurement_unit);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Controls.Add(this.txtDesc);
            this.kryptonPanel1.Controls.Add(this.lblDesc);
            this.kryptonPanel1.Controls.Add(this.txtUnitName);
            this.kryptonPanel1.Controls.Add(this.lblUnitName);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(414, 324);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // kryptonGroupBox1
            // 
            this.kryptonGroupBox1.CaptionVisible = false;
            this.kryptonGroupBox1.Location = new System.Drawing.Point(101, 194);
            this.kryptonGroupBox1.Name = "kryptonGroupBox1";
            // 
            // kryptonGroupBox1.Panel
            // 
            this.kryptonGroupBox1.Panel.Controls.Add(this.rdbis_enabledNo);
            this.kryptonGroupBox1.Panel.Controls.Add(this.rdbis_enabledYes);
            this.kryptonGroupBox1.Size = new System.Drawing.Size(111, 40);
            this.kryptonGroupBox1.TabIndex = 66;
            this.kryptonGroupBox1.Values.Heading = "是";
            // 
            // rdbis_enabledNo
            // 
            this.rdbis_enabledNo.Checked = true;
            this.rdbis_enabledNo.Location = new System.Drawing.Point(58, 8);
            this.rdbis_enabledNo.Name = "rdbis_enabledNo";
            this.rdbis_enabledNo.Size = new System.Drawing.Size(35, 20);
            this.rdbis_enabledNo.TabIndex = 1;
            this.rdbis_enabledNo.Values.Text = "否";
            // 
            // rdbis_enabledYes
            // 
            this.rdbis_enabledYes.Location = new System.Drawing.Point(10, 8);
            this.rdbis_enabledYes.Name = "rdbis_enabledYes";
            this.rdbis_enabledYes.Size = new System.Drawing.Size(35, 20);
            this.rdbis_enabledYes.TabIndex = 0;
            this.rdbis_enabledYes.Values.Text = "是";
            // 
            // lblis_measurement_unit
            // 
            this.lblis_measurement_unit.Location = new System.Drawing.Point(15, 205);
            this.lblis_measurement_unit.Name = "lblis_measurement_unit";
            this.lblis_measurement_unit.Size = new System.Drawing.Size(75, 20);
            this.lblis_measurement_unit.TabIndex = 8;
            this.lblis_measurement_unit.Values.Text = "是否可换算";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(219, 257);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(101, 257);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 6;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // txtDesc
            // 
            this.txtDesc.Location = new System.Drawing.Point(101, 86);
            this.txtDesc.Multiline = true;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(208, 90);
            this.txtDesc.TabIndex = 5;
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(33, 86);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(57, 20);
            this.lblDesc.TabIndex = 4;
            this.lblDesc.Values.Text = "描      述";
            // 
            // txtUnitName
            // 
            this.txtUnitName.Location = new System.Drawing.Point(101, 23);
            this.txtUnitName.Name = "txtUnitName";
            this.txtUnitName.Size = new System.Drawing.Size(208, 23);
            this.txtUnitName.TabIndex = 3;
            // 
            // lblUnitName
            // 
            this.lblUnitName.Location = new System.Drawing.Point(33, 23);
            this.lblUnitName.Name = "lblUnitName";
            this.lblUnitName.Size = new System.Drawing.Size(62, 20);
            this.lblUnitName.TabIndex = 2;
            this.lblUnitName.Values.Text = "单位名称";
            // 
            // UCUnitEdit
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(414, 324);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCUnitEdit";
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).EndInit();
            this.kryptonGroupBox1.Panel.ResumeLayout(false);
            this.kryptonGroupBox1.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).EndInit();
            this.kryptonGroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonTextBox txtDesc;
        private Krypton.Toolkit.KryptonLabel lblDesc;
        private Krypton.Toolkit.KryptonTextBox txtUnitName;
        private Krypton.Toolkit.KryptonLabel lblUnitName;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonLabel lblis_measurement_unit;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBox1;
        private Krypton.Toolkit.KryptonRadioButton rdbis_enabledNo;
        private Krypton.Toolkit.KryptonRadioButton rdbis_enabledYes;
    }
}
