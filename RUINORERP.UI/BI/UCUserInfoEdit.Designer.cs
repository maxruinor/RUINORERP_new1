namespace RUINORERP.UI.BI
{
    partial class UCUserInfoEdit
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
            this.chkModifyPwd = new Krypton.Toolkit.KryptonCheckBox();
            this.kryptonGroupBox3 = new Krypton.Toolkit.KryptonGroupBox();
            this.rdbIsSuperUserNo = new Krypton.Toolkit.KryptonRadioButton();
            this.rdbIsSuperUserYes = new Krypton.Toolkit.KryptonRadioButton();
            this.lbSuperUser = new Krypton.Toolkit.KryptonLabel();
            this.cmbEmployee = new Krypton.Toolkit.KryptonComboBox();
            this.lblEmployee = new Krypton.Toolkit.KryptonLabel();
            this.kryptonGroupBox2 = new Krypton.Toolkit.KryptonGroupBox();
            this.rdbis_availableNo = new Krypton.Toolkit.KryptonRadioButton();
            this.rdbis_availableYes = new Krypton.Toolkit.KryptonRadioButton();
            this.kryptonGroupBox1 = new Krypton.Toolkit.KryptonGroupBox();
            this.rdbis_enabledNo = new Krypton.Toolkit.KryptonRadioButton();
            this.rdbis_enabledYes = new Krypton.Toolkit.KryptonRadioButton();
            this.lblUserName = new Krypton.Toolkit.KryptonLabel();
            this.txtUserName = new Krypton.Toolkit.KryptonTextBox();
            this.lblPassword = new Krypton.Toolkit.KryptonLabel();
            this.txtPassword = new Krypton.Toolkit.KryptonTextBox();
            this.lblis_enabled = new Krypton.Toolkit.KryptonLabel();
            this.lblis_available = new Krypton.Toolkit.KryptonLabel();
            this.lblNotes = new Krypton.Toolkit.KryptonLabel();
            this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox3.Panel)).BeginInit();
            this.kryptonGroupBox3.Panel.SuspendLayout();
            this.kryptonGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmployee)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2.Panel)).BeginInit();
            this.kryptonGroupBox2.Panel.SuspendLayout();
            this.kryptonGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).BeginInit();
            this.kryptonGroupBox1.Panel.SuspendLayout();
            this.kryptonGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(177, 424);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(295, 424);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.chkModifyPwd);
            this.kryptonPanel1.Controls.Add(this.kryptonGroupBox3);
            this.kryptonPanel1.Controls.Add(this.lbSuperUser);
            this.kryptonPanel1.Controls.Add(this.cmbEmployee);
            this.kryptonPanel1.Controls.Add(this.lblEmployee);
            this.kryptonPanel1.Controls.Add(this.kryptonGroupBox2);
            this.kryptonPanel1.Controls.Add(this.kryptonGroupBox1);
            this.kryptonPanel1.Controls.Add(this.lblUserName);
            this.kryptonPanel1.Controls.Add(this.txtUserName);
            this.kryptonPanel1.Controls.Add(this.lblPassword);
            this.kryptonPanel1.Controls.Add(this.txtPassword);
            this.kryptonPanel1.Controls.Add(this.lblis_enabled);
            this.kryptonPanel1.Controls.Add(this.lblis_available);
            this.kryptonPanel1.Controls.Add(this.lblNotes);
            this.kryptonPanel1.Controls.Add(this.txtNotes);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(570, 502);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // chkModifyPwd
            // 
            this.chkModifyPwd.Location = new System.Drawing.Point(473, 53);
            this.chkModifyPwd.Name = "chkModifyPwd";
            this.chkModifyPwd.Size = new System.Drawing.Size(75, 20);
            this.chkModifyPwd.TabIndex = 76;
            this.chkModifyPwd.Values.Text = "修改密码";
            this.chkModifyPwd.CheckedChanged += new System.EventHandler(this.chkModifyPwd_CheckedChanged);
            // 
            // kryptonGroupBox3
            // 
            this.kryptonGroupBox3.CaptionVisible = false;
            this.kryptonGroupBox3.Location = new System.Drawing.Point(206, 233);
            this.kryptonGroupBox3.Name = "kryptonGroupBox3";
            // 
            // kryptonGroupBox3.Panel
            // 
            this.kryptonGroupBox3.Panel.Controls.Add(this.rdbIsSuperUserNo);
            this.kryptonGroupBox3.Panel.Controls.Add(this.rdbIsSuperUserYes);
            this.kryptonGroupBox3.Size = new System.Drawing.Size(106, 56);
            this.kryptonGroupBox3.TabIndex = 75;
            // 
            // rdbIsSuperUserNo
            // 
            this.rdbIsSuperUserNo.Checked = true;
            this.rdbIsSuperUserNo.Location = new System.Drawing.Point(58, 14);
            this.rdbIsSuperUserNo.Name = "rdbIsSuperUserNo";
            this.rdbIsSuperUserNo.Size = new System.Drawing.Size(35, 20);
            this.rdbIsSuperUserNo.TabIndex = 3;
            this.rdbIsSuperUserNo.Values.Text = "否";
            // 
            // rdbIsSuperUserYes
            // 
            this.rdbIsSuperUserYes.Location = new System.Drawing.Point(10, 14);
            this.rdbIsSuperUserYes.Name = "rdbIsSuperUserYes";
            this.rdbIsSuperUserYes.Size = new System.Drawing.Size(35, 20);
            this.rdbIsSuperUserYes.TabIndex = 2;
            this.rdbIsSuperUserYes.Values.Text = "是";
            // 
            // lbSuperUser
            // 
            this.lbSuperUser.Location = new System.Drawing.Point(129, 249);
            this.lbSuperUser.Name = "lbSuperUser";
            this.lbSuperUser.Size = new System.Drawing.Size(62, 20);
            this.lbSuperUser.TabIndex = 74;
            this.lbSuperUser.Values.Text = "超级用户";
            // 
            // cmbEmployee
            // 
            this.cmbEmployee.DropDownWidth = 100;
            this.cmbEmployee.IntegralHeight = false;
            this.cmbEmployee.Location = new System.Drawing.Point(206, 85);
            this.cmbEmployee.Name = "cmbEmployee";
            this.cmbEmployee.Size = new System.Drawing.Size(261, 21);
            this.cmbEmployee.TabIndex = 73;
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(155, 86);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(36, 20);
            this.lblEmployee.TabIndex = 72;
            this.lblEmployee.Values.Text = "员工";
            // 
            // kryptonGroupBox2
            // 
            this.kryptonGroupBox2.CaptionVisible = false;
            this.kryptonGroupBox2.Location = new System.Drawing.Point(206, 166);
            this.kryptonGroupBox2.Name = "kryptonGroupBox2";
            // 
            // kryptonGroupBox2.Panel
            // 
            this.kryptonGroupBox2.Panel.Controls.Add(this.rdbis_availableNo);
            this.kryptonGroupBox2.Panel.Controls.Add(this.rdbis_availableYes);
            this.kryptonGroupBox2.Size = new System.Drawing.Size(106, 56);
            this.kryptonGroupBox2.TabIndex = 33;
            // 
            // rdbis_availableNo
            // 
            this.rdbis_availableNo.Location = new System.Drawing.Point(58, 14);
            this.rdbis_availableNo.Name = "rdbis_availableNo";
            this.rdbis_availableNo.Size = new System.Drawing.Size(35, 20);
            this.rdbis_availableNo.TabIndex = 3;
            this.rdbis_availableNo.Values.Text = "否";
            // 
            // rdbis_availableYes
            // 
            this.rdbis_availableYes.Location = new System.Drawing.Point(10, 14);
            this.rdbis_availableYes.Name = "rdbis_availableYes";
            this.rdbis_availableYes.Size = new System.Drawing.Size(35, 20);
            this.rdbis_availableYes.TabIndex = 2;
            this.rdbis_availableYes.Values.Text = "是";
            // 
            // kryptonGroupBox1
            // 
            this.kryptonGroupBox1.CaptionVisible = false;
            this.kryptonGroupBox1.Location = new System.Drawing.Point(206, 119);
            this.kryptonGroupBox1.Name = "kryptonGroupBox1";
            // 
            // kryptonGroupBox1.Panel
            // 
            this.kryptonGroupBox1.Panel.Controls.Add(this.rdbis_enabledNo);
            this.kryptonGroupBox1.Panel.Controls.Add(this.rdbis_enabledYes);
            this.kryptonGroupBox1.Size = new System.Drawing.Size(106, 40);
            this.kryptonGroupBox1.TabIndex = 32;
            this.kryptonGroupBox1.Values.Heading = "是";
            // 
            // rdbis_enabledNo
            // 
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
            // lblUserName
            // 
            this.lblUserName.Location = new System.Drawing.Point(142, 12);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(49, 20);
            this.lblUserName.TabIndex = 14;
            this.lblUserName.Values.Text = "用户名";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(206, 12);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(261, 23);
            this.txtUserName.TabIndex = 15;
            // 
            // lblPassword
            // 
            this.lblPassword.Location = new System.Drawing.Point(155, 51);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(36, 20);
            this.lblPassword.TabIndex = 16;
            this.lblPassword.Values.Text = "密码";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(206, 51);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(261, 23);
            this.txtPassword.TabIndex = 17;
            // 
            // lblis_enabled
            // 
            this.lblis_enabled.Location = new System.Drawing.Point(129, 129);
            this.lblis_enabled.Name = "lblis_enabled";
            this.lblis_enabled.Size = new System.Drawing.Size(62, 20);
            this.lblis_enabled.TabIndex = 18;
            this.lblis_enabled.Values.Text = "是否启用";
            // 
            // lblis_available
            // 
            this.lblis_available.Location = new System.Drawing.Point(129, 182);
            this.lblis_available.Name = "lblis_available";
            this.lblis_available.Size = new System.Drawing.Size(62, 20);
            this.lblis_available.TabIndex = 21;
            this.lblis_available.Values.Text = "是否可用";
            // 
            // lblNotes
            // 
            this.lblNotes.Location = new System.Drawing.Point(129, 295);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(62, 20);
            this.lblNotes.TabIndex = 30;
            this.lblNotes.Values.Text = "备注说明";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(206, 295);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(261, 102);
            this.txtNotes.TabIndex = 31;
            // 
            // UCUserInfoEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 502);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCUserInfoEdit";
            this.Load += new System.EventHandler(this.UCUserInfoEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox3.Panel)).EndInit();
            this.kryptonGroupBox3.Panel.ResumeLayout(false);
            this.kryptonGroupBox3.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox3)).EndInit();
            this.kryptonGroupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmployee)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2.Panel)).EndInit();
            this.kryptonGroupBox2.Panel.ResumeLayout(false);
            this.kryptonGroupBox2.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2)).EndInit();
            this.kryptonGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).EndInit();
            this.kryptonGroupBox1.Panel.ResumeLayout(false);
            this.kryptonGroupBox1.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).EndInit();
            this.kryptonGroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonLabel lblUserName;
        private Krypton.Toolkit.KryptonTextBox txtUserName;
        private Krypton.Toolkit.KryptonLabel lblPassword;
        private Krypton.Toolkit.KryptonTextBox txtPassword;
        private Krypton.Toolkit.KryptonLabel lblis_enabled;
        private Krypton.Toolkit.KryptonLabel lblis_available;
        private Krypton.Toolkit.KryptonLabel lblNotes;
        private Krypton.Toolkit.KryptonTextBox txtNotes;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBox1;
        private Krypton.Toolkit.KryptonRadioButton rdbis_enabledYes;
        private Krypton.Toolkit.KryptonRadioButton rdbis_enabledNo;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBox2;
        private Krypton.Toolkit.KryptonRadioButton rdbis_availableNo;
        private Krypton.Toolkit.KryptonRadioButton rdbis_availableYes;
        private Krypton.Toolkit.KryptonComboBox cmbEmployee;
        private Krypton.Toolkit.KryptonLabel lblEmployee;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBox3;
        private Krypton.Toolkit.KryptonRadioButton rdbIsSuperUserNo;
        private Krypton.Toolkit.KryptonRadioButton rdbIsSuperUserYes;
        private Krypton.Toolkit.KryptonLabel lbSuperUser;
        private Krypton.Toolkit.KryptonCheckBox chkModifyPwd;
    }
}
