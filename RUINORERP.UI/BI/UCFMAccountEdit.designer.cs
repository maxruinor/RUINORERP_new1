namespace RUINORERP.UI.BI
{
    partial class UCFMAccountEdit
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
            this.lblDepartmentID = new Krypton.Toolkit.KryptonLabel();
            this.cmbDepartmentID = new Krypton.Toolkit.KryptonComboBox();
            this.lblsubject_id = new Krypton.Toolkit.KryptonLabel();
            this.cmbsubject_id = new Krypton.Toolkit.KryptonComboBox();
            this.lblCurrency_ID = new Krypton.Toolkit.KryptonLabel();
            this.cmbCurrency_ID = new Krypton.Toolkit.KryptonComboBox();
            this.lblaccount_name = new Krypton.Toolkit.KryptonLabel();
            this.txtaccount_name = new Krypton.Toolkit.KryptonTextBox();
            this.lblaccount_No = new Krypton.Toolkit.KryptonLabel();
            this.txtaccount_No = new Krypton.Toolkit.KryptonTextBox();
            this.lblaccount_type = new Krypton.Toolkit.KryptonLabel();
            this.txtaccount_type = new Krypton.Toolkit.KryptonTextBox();
            this.lblBank = new Krypton.Toolkit.KryptonLabel();
            this.txtBank = new Krypton.Toolkit.KryptonTextBox();
            this.lblOpeningBalance = new Krypton.Toolkit.KryptonLabel();
            this.txtOpeningBalance = new Krypton.Toolkit.KryptonTextBox();
            this.lblCurrentBalance = new Krypton.Toolkit.KryptonLabel();
            this.txtCurrentBalance = new Krypton.Toolkit.KryptonTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDepartmentID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbsubject_id)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCurrency_ID)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(162, 425);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(280, 425);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.lblDepartmentID);
            this.kryptonPanel1.Controls.Add(this.cmbDepartmentID);
            this.kryptonPanel1.Controls.Add(this.lblsubject_id);
            this.kryptonPanel1.Controls.Add(this.cmbsubject_id);
            this.kryptonPanel1.Controls.Add(this.lblCurrency_ID);
            this.kryptonPanel1.Controls.Add(this.cmbCurrency_ID);
            this.kryptonPanel1.Controls.Add(this.lblaccount_name);
            this.kryptonPanel1.Controls.Add(this.txtaccount_name);
            this.kryptonPanel1.Controls.Add(this.lblaccount_No);
            this.kryptonPanel1.Controls.Add(this.txtaccount_No);
            this.kryptonPanel1.Controls.Add(this.lblaccount_type);
            this.kryptonPanel1.Controls.Add(this.txtaccount_type);
            this.kryptonPanel1.Controls.Add(this.lblBank);
            this.kryptonPanel1.Controls.Add(this.txtBank);
            this.kryptonPanel1.Controls.Add(this.lblOpeningBalance);
            this.kryptonPanel1.Controls.Add(this.txtOpeningBalance);
            this.kryptonPanel1.Controls.Add(this.lblCurrentBalance);
            this.kryptonPanel1.Controls.Add(this.txtCurrentBalance);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(568, 511);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // lblDepartmentID
            // 
            this.lblDepartmentID.Location = new System.Drawing.Point(103, 43);
            this.lblDepartmentID.Name = "lblDepartmentID";
            this.lblDepartmentID.Size = new System.Drawing.Size(35, 20);
            this.lblDepartmentID.TabIndex = 10;
            this.lblDepartmentID.Values.Text = "部门";
            // 
            // cmbDepartmentID
            // 
            this.cmbDepartmentID.DropDownWidth = 100;
            this.cmbDepartmentID.Location = new System.Drawing.Point(144, 42);
            this.cmbDepartmentID.Name = "cmbDepartmentID";
            this.cmbDepartmentID.Size = new System.Drawing.Size(295, 21);
            this.cmbDepartmentID.TabIndex = 11;
            // 
            // lblsubject_id
            // 
            this.lblsubject_id.Location = new System.Drawing.Point(78, 84);
            this.lblsubject_id.Name = "lblsubject_id";
            this.lblsubject_id.Size = new System.Drawing.Size(60, 20);
            this.lblsubject_id.TabIndex = 12;
            this.lblsubject_id.Values.Text = "会计科目";
            // 
            // cmbsubject_id
            // 
            this.cmbsubject_id.DropDownWidth = 100;
            this.cmbsubject_id.Location = new System.Drawing.Point(144, 83);
            this.cmbsubject_id.Name = "cmbsubject_id";
            this.cmbsubject_id.Size = new System.Drawing.Size(295, 21);
            this.cmbsubject_id.TabIndex = 13;
            // 
            // lblCurrency_ID
            // 
            this.lblCurrency_ID.Location = new System.Drawing.Point(103, 125);
            this.lblCurrency_ID.Name = "lblCurrency_ID";
            this.lblCurrency_ID.Size = new System.Drawing.Size(35, 20);
            this.lblCurrency_ID.TabIndex = 14;
            this.lblCurrency_ID.Values.Text = "币种";
            // 
            // cmbCurrency_ID
            // 
            this.cmbCurrency_ID.DropDownWidth = 100;
            this.cmbCurrency_ID.Location = new System.Drawing.Point(144, 124);
            this.cmbCurrency_ID.Name = "cmbCurrency_ID";
            this.cmbCurrency_ID.Size = new System.Drawing.Size(295, 21);
            this.cmbCurrency_ID.TabIndex = 15;
            // 
            // lblaccount_name
            // 
            this.lblaccount_name.Location = new System.Drawing.Point(78, 165);
            this.lblaccount_name.Name = "lblaccount_name";
            this.lblaccount_name.Size = new System.Drawing.Size(60, 20);
            this.lblaccount_name.TabIndex = 16;
            this.lblaccount_name.Values.Text = "账户名称";
            // 
            // txtaccount_name
            // 
            this.txtaccount_name.Location = new System.Drawing.Point(144, 165);
            this.txtaccount_name.Name = "txtaccount_name";
            this.txtaccount_name.Size = new System.Drawing.Size(295, 20);
            this.txtaccount_name.TabIndex = 17;
            // 
            // lblaccount_No
            // 
            this.lblaccount_No.Location = new System.Drawing.Point(103, 205);
            this.lblaccount_No.Name = "lblaccount_No";
            this.lblaccount_No.Size = new System.Drawing.Size(35, 20);
            this.lblaccount_No.TabIndex = 19;
            this.lblaccount_No.Values.Text = "账号";
            // 
            // txtaccount_No
            // 
            this.txtaccount_No.Location = new System.Drawing.Point(144, 205);
            this.txtaccount_No.Name = "txtaccount_No";
            this.txtaccount_No.Size = new System.Drawing.Size(295, 20);
            this.txtaccount_No.TabIndex = 18;
            // 
            // lblaccount_type
            // 
            this.lblaccount_type.Location = new System.Drawing.Point(78, 245);
            this.lblaccount_type.Name = "lblaccount_type";
            this.lblaccount_type.Size = new System.Drawing.Size(60, 20);
            this.lblaccount_type.TabIndex = 20;
            this.lblaccount_type.Values.Text = "账户类型";
            // 
            // txtaccount_type
            // 
            this.txtaccount_type.Location = new System.Drawing.Point(144, 245);
            this.txtaccount_type.Name = "txtaccount_type";
            this.txtaccount_type.Size = new System.Drawing.Size(295, 20);
            this.txtaccount_type.TabIndex = 21;
            // 
            // lblBank
            // 
            this.lblBank.Location = new System.Drawing.Point(78, 285);
            this.lblBank.Name = "lblBank";
            this.lblBank.Size = new System.Drawing.Size(60, 20);
            this.lblBank.TabIndex = 22;
            this.lblBank.Values.Text = "所属银行";
            // 
            // txtBank
            // 
            this.txtBank.Location = new System.Drawing.Point(144, 285);
            this.txtBank.Name = "txtBank";
            this.txtBank.Size = new System.Drawing.Size(295, 20);
            this.txtBank.TabIndex = 23;
            // 
            // lblOpeningBalance
            // 
            this.lblOpeningBalance.Location = new System.Drawing.Point(78, 325);
            this.lblOpeningBalance.Name = "lblOpeningBalance";
            this.lblOpeningBalance.Size = new System.Drawing.Size(60, 20);
            this.lblOpeningBalance.TabIndex = 24;
            this.lblOpeningBalance.Values.Text = "初始余额";
            // 
            // txtOpeningBalance
            // 
            this.txtOpeningBalance.Location = new System.Drawing.Point(144, 325);
            this.txtOpeningBalance.Name = "txtOpeningBalance";
            this.txtOpeningBalance.Size = new System.Drawing.Size(295, 20);
            this.txtOpeningBalance.TabIndex = 25;
            // 
            // lblCurrentBalance
            // 
            this.lblCurrentBalance.Location = new System.Drawing.Point(78, 365);
            this.lblCurrentBalance.Name = "lblCurrentBalance";
            this.lblCurrentBalance.Size = new System.Drawing.Size(60, 20);
            this.lblCurrentBalance.TabIndex = 26;
            this.lblCurrentBalance.Values.Text = "当前余额";
            // 
            // txtCurrentBalance
            // 
            this.txtCurrentBalance.Location = new System.Drawing.Point(144, 365);
            this.txtCurrentBalance.Name = "txtCurrentBalance";
            this.txtCurrentBalance.Size = new System.Drawing.Size(295, 20);
            this.txtCurrentBalance.TabIndex = 27;
            // 
            // UCFMAccountEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 511);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCFMAccountEdit";
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDepartmentID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbsubject_id)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCurrency_ID)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonLabel lblDepartmentID;
        private Krypton.Toolkit.KryptonComboBox cmbDepartmentID;
        private Krypton.Toolkit.KryptonLabel lblsubject_id;
        private Krypton.Toolkit.KryptonComboBox cmbsubject_id;
        private Krypton.Toolkit.KryptonLabel lblCurrency_ID;
        private Krypton.Toolkit.KryptonComboBox cmbCurrency_ID;
        private Krypton.Toolkit.KryptonLabel lblaccount_name;
        private Krypton.Toolkit.KryptonTextBox txtaccount_name;
        private Krypton.Toolkit.KryptonLabel lblaccount_No;
        private Krypton.Toolkit.KryptonTextBox txtaccount_No;
        private Krypton.Toolkit.KryptonLabel lblaccount_type;
        private Krypton.Toolkit.KryptonTextBox txtaccount_type;
        private Krypton.Toolkit.KryptonLabel lblBank;
        private Krypton.Toolkit.KryptonTextBox txtBank;
        private Krypton.Toolkit.KryptonLabel lblOpeningBalance;
        private Krypton.Toolkit.KryptonTextBox txtOpeningBalance;
        private Krypton.Toolkit.KryptonLabel lblCurrentBalance;
        private Krypton.Toolkit.KryptonTextBox txtCurrentBalance;
    }
}
