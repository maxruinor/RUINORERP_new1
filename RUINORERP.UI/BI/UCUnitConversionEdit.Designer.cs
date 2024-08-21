namespace RUINORERP.UI.BI
{
    partial class UCUnitConversionEdit
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
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            this.cmbTarget_unit_id = new Krypton.Toolkit.KryptonComboBox();
            this.cmbSource_unit_id = new Krypton.Toolkit.KryptonComboBox();
            this.lblUnitConversion_Name = new Krypton.Toolkit.KryptonLabel();
            this.txtUnitConversion_Name = new Krypton.Toolkit.KryptonTextBox();
            this.lblSource_unit_id = new Krypton.Toolkit.KryptonLabel();
            this.lblTarget_unit_id = new Krypton.Toolkit.KryptonLabel();
            this.lblConversion_ratio = new Krypton.Toolkit.KryptonLabel();
            this.txtConversion_ratio = new Krypton.Toolkit.KryptonTextBox();
            this.lblNotes = new Krypton.Toolkit.KryptonLabel();
            this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
            this.btnCancel = new Krypton.Toolkit.KryptonButton();
            this.btnOk = new Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTarget_unit_id)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSource_unit_id)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.kryptonLabel1);
            this.kryptonPanel1.Controls.Add(this.cmbTarget_unit_id);
            this.kryptonPanel1.Controls.Add(this.cmbSource_unit_id);
            this.kryptonPanel1.Controls.Add(this.lblUnitConversion_Name);
            this.kryptonPanel1.Controls.Add(this.txtUnitConversion_Name);
            this.kryptonPanel1.Controls.Add(this.lblSource_unit_id);
            this.kryptonPanel1.Controls.Add(this.lblTarget_unit_id);
            this.kryptonPanel1.Controls.Add(this.lblConversion_ratio);
            this.kryptonPanel1.Controls.Add(this.txtConversion_ratio);
            this.kryptonPanel1.Controls.Add(this.lblNotes);
            this.kryptonPanel1.Controls.Add(this.txtNotes);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(565, 447);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(89, 181);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(292, 68);
            this.kryptonLabel1.TabIndex = 130;
            this.kryptonLabel1.Values.Text = "换算比例,即换算因子。\r\n如:1打等于12个，源：打，目标：个，比例为：12\r\n米\t厘米\t100\r\n厘米\t米\t0.01";
            // 
            // cmbTarget_unit_id
            // 
            this.cmbTarget_unit_id.DropDownWidth = 207;
            this.cmbTarget_unit_id.IntegralHeight = false;
            this.cmbTarget_unit_id.Location = new System.Drawing.Point(89, 106);
            this.cmbTarget_unit_id.Name = "cmbTarget_unit_id";
            this.cmbTarget_unit_id.Size = new System.Drawing.Size(422, 21);
            this.cmbTarget_unit_id.TabIndex = 129;
            // 
            // cmbSource_unit_id
            // 
            this.cmbSource_unit_id.DropDownWidth = 207;
            this.cmbSource_unit_id.IntegralHeight = false;
            this.cmbSource_unit_id.Location = new System.Drawing.Point(89, 70);
            this.cmbSource_unit_id.Name = "cmbSource_unit_id";
            this.cmbSource_unit_id.Size = new System.Drawing.Size(422, 21);
            this.cmbSource_unit_id.TabIndex = 128;
            // 
            // lblUnitConversion_Name
            // 
            this.lblUnitConversion_Name.Location = new System.Drawing.Point(21, 36);
            this.lblUnitConversion_Name.Name = "lblUnitConversion_Name";
            this.lblUnitConversion_Name.Size = new System.Drawing.Size(62, 20);
            this.lblUnitConversion_Name.TabIndex = 8;
            this.lblUnitConversion_Name.Values.Text = "换算名称";
            // 
            // txtUnitConversion_Name
            // 
            this.txtUnitConversion_Name.Location = new System.Drawing.Point(89, 32);
            this.txtUnitConversion_Name.Name = "txtUnitConversion_Name";
            this.txtUnitConversion_Name.Size = new System.Drawing.Size(422, 23);
            this.txtUnitConversion_Name.TabIndex = 9;
            // 
            // lblSource_unit_id
            // 
            this.lblSource_unit_id.Location = new System.Drawing.Point(21, 73);
            this.lblSource_unit_id.Name = "lblSource_unit_id";
            this.lblSource_unit_id.Size = new System.Drawing.Size(62, 20);
            this.lblSource_unit_id.TabIndex = 10;
            this.lblSource_unit_id.Values.Text = "来源单位";
            // 
            // lblTarget_unit_id
            // 
            this.lblTarget_unit_id.Location = new System.Drawing.Point(21, 110);
            this.lblTarget_unit_id.Name = "lblTarget_unit_id";
            this.lblTarget_unit_id.Size = new System.Drawing.Size(62, 20);
            this.lblTarget_unit_id.TabIndex = 12;
            this.lblTarget_unit_id.Values.Text = "目标单位";
            // 
            // lblConversion_ratio
            // 
            this.lblConversion_ratio.Location = new System.Drawing.Point(21, 147);
            this.lblConversion_ratio.Name = "lblConversion_ratio";
            this.lblConversion_ratio.Size = new System.Drawing.Size(62, 20);
            this.lblConversion_ratio.TabIndex = 14;
            this.lblConversion_ratio.Values.Text = "换算比例";
            // 
            // txtConversion_ratio
            // 
            this.txtConversion_ratio.Location = new System.Drawing.Point(89, 142);
            this.txtConversion_ratio.Name = "txtConversion_ratio";
            this.txtConversion_ratio.Size = new System.Drawing.Size(422, 23);
            this.txtConversion_ratio.TabIndex = 15;
            // 
            // lblNotes
            // 
            this.lblNotes.Location = new System.Drawing.Point(47, 268);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(36, 20);
            this.lblNotes.TabIndex = 16;
            this.lblNotes.Values.Text = "备注";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(89, 268);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(422, 75);
            this.txtNotes.TabIndex = 17;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(307, 385);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(189, 385);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 6;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // UCUnitConversionEdit
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(565, 447);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCUnitConversionEdit";
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTarget_unit_id)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSource_unit_id)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonLabel lblUnitConversion_Name;
        private Krypton.Toolkit.KryptonTextBox txtUnitConversion_Name;
        private Krypton.Toolkit.KryptonLabel lblSource_unit_id;
        private Krypton.Toolkit.KryptonLabel lblTarget_unit_id;
        private Krypton.Toolkit.KryptonLabel lblConversion_ratio;
        private Krypton.Toolkit.KryptonTextBox txtConversion_ratio;
        private Krypton.Toolkit.KryptonLabel lblNotes;
        private Krypton.Toolkit.KryptonTextBox txtNotes;
        private Krypton.Toolkit.KryptonComboBox cmbTarget_unit_id;
        private Krypton.Toolkit.KryptonComboBox cmbSource_unit_id;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
    }
}
