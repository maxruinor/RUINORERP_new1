namespace RUINORERP.UI.BI
{
    partial class UCFMExpenseTypeEdit
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
            this.btnOk = new Krypton.Toolkit.KryptonButton();
            this.btnCancel = new Krypton.Toolkit.KryptonButton();
            this.lblsubject_id = new Krypton.Toolkit.KryptonLabel();
            this.cmbsubject_id = new Krypton.Toolkit.KryptonComboBox();
            this.lblExpense_name = new Krypton.Toolkit.KryptonLabel();
            this.txtExpense_name = new Krypton.Toolkit.KryptonTextBox();
            this.lblEXPOrINC = new Krypton.Toolkit.KryptonLabel();
            this.chkEXPOrINC = new Krypton.Toolkit.KryptonCheckBox();
            this.lblNotes = new Krypton.Toolkit.KryptonLabel();
            this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbsubject_id)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.lblsubject_id);
            this.kryptonPanel1.Controls.Add(this.cmbsubject_id);
            this.kryptonPanel1.Controls.Add(this.lblExpense_name);
            this.kryptonPanel1.Controls.Add(this.txtExpense_name);
            this.kryptonPanel1.Controls.Add(this.lblEXPOrINC);
            this.kryptonPanel1.Controls.Add(this.chkEXPOrINC);
            this.kryptonPanel1.Controls.Add(this.lblNotes);
            this.kryptonPanel1.Controls.Add(this.txtNotes);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(498, 377);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(152, 251);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(270, 251);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblsubject_id
            // 
            this.lblsubject_id.Location = new System.Drawing.Point(101, 48);
            this.lblsubject_id.Name = "lblsubject_id";
            this.lblsubject_id.Size = new System.Drawing.Size(35, 20);
            this.lblsubject_id.TabIndex = 5;
            this.lblsubject_id.Values.Text = "科目";
            // 
            // cmbsubject_id
            // 
            this.cmbsubject_id.DropDownWidth = 100;
            this.cmbsubject_id.Location = new System.Drawing.Point(153, 47);
            this.cmbsubject_id.Name = "cmbsubject_id";
            this.cmbsubject_id.Size = new System.Drawing.Size(248, 21);
            this.cmbsubject_id.TabIndex = 6;
            // 
            // lblExpense_name
            // 
            this.lblExpense_name.Location = new System.Drawing.Point(51, 88);
            this.lblExpense_name.Name = "lblExpense_name";
            this.lblExpense_name.Size = new System.Drawing.Size(85, 20);
            this.lblExpense_name.TabIndex = 7;
            this.lblExpense_name.Values.Text = "费用业务名称";
            // 
            // txtExpense_name
            // 
            this.txtExpense_name.Location = new System.Drawing.Point(153, 88);
            this.txtExpense_name.Name = "txtExpense_name";
            this.txtExpense_name.Size = new System.Drawing.Size(248, 20);
            this.txtExpense_name.TabIndex = 8;
            // 
            // lblEXPOrINC
            // 
            this.lblEXPOrINC.Location = new System.Drawing.Point(76, 121);
            this.lblEXPOrINC.Name = "lblEXPOrINC";
            this.lblEXPOrINC.Size = new System.Drawing.Size(60, 20);
            this.lblEXPOrINC.TabIndex = 9;
            this.lblEXPOrINC.Values.Text = "收支标识";
            // 
            // chkEXPOrINC
            // 
            this.chkEXPOrINC.Checked = true;
            this.chkEXPOrINC.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEXPOrINC.Location = new System.Drawing.Point(153, 128);
            this.chkEXPOrINC.Name = "chkEXPOrINC";
            this.chkEXPOrINC.Size = new System.Drawing.Size(19, 13);
            this.chkEXPOrINC.TabIndex = 10;
            this.chkEXPOrINC.Values.Text = "";
            // 
            // lblNotes
            // 
            this.lblNotes.Location = new System.Drawing.Point(101, 161);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(35, 20);
            this.lblNotes.TabIndex = 11;
            this.lblNotes.Values.Text = "备注";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(153, 161);
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(248, 20);
            this.txtNotes.TabIndex = 12;
            // 
            // UCFMExpenseTypeEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 377);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCFMExpenseTypeEdit";
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbsubject_id)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonLabel lblsubject_id;
        private Krypton.Toolkit.KryptonComboBox cmbsubject_id;
        private Krypton.Toolkit.KryptonLabel lblExpense_name;
        private Krypton.Toolkit.KryptonTextBox txtExpense_name;
        private Krypton.Toolkit.KryptonLabel lblEXPOrINC;
        private Krypton.Toolkit.KryptonCheckBox chkEXPOrINC;
        private Krypton.Toolkit.KryptonLabel lblNotes;
        private Krypton.Toolkit.KryptonTextBox txtNotes;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonButton btnOk;
    }
}
