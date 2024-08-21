namespace RUINORERP.UI.BI
{
    partial class UCProjectGroupEdit
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
            this.lblProjectGroupCode = new Krypton.Toolkit.KryptonLabel();
            this.txtProjectGroupCode = new Krypton.Toolkit.KryptonTextBox();
            this.lblProjectGroupName = new Krypton.Toolkit.KryptonLabel();
            this.txtProjectGroupName = new Krypton.Toolkit.KryptonTextBox();
            this.lblResponsiblePerson = new Krypton.Toolkit.KryptonLabel();
            this.txtResponsiblePerson = new Krypton.Toolkit.KryptonTextBox();
            this.lblPhone = new Krypton.Toolkit.KryptonLabel();
            this.txtPhone = new Krypton.Toolkit.KryptonTextBox();
            this.lblStartDate = new Krypton.Toolkit.KryptonLabel();
            this.dtpStartDate = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lblIs_enabled = new Krypton.Toolkit.KryptonLabel();
            this.chkIs_enabled = new Krypton.Toolkit.KryptonCheckBox();
            this.lblEndDate = new Krypton.Toolkit.KryptonLabel();
            this.dtpEndDate = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lblNotes = new Krypton.Toolkit.KryptonLabel();
            this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
            this.cmbDepartment = new Krypton.Toolkit.KryptonComboBox();
            this.lblDepartmentID = new Krypton.Toolkit.KryptonLabel();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDepartment)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(120, 402);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(273, 402);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.cmbDepartment);
            this.kryptonPanel1.Controls.Add(this.lblDepartmentID);
            this.kryptonPanel1.Controls.Add(this.lblProjectGroupCode);
            this.kryptonPanel1.Controls.Add(this.txtProjectGroupCode);
            this.kryptonPanel1.Controls.Add(this.lblProjectGroupName);
            this.kryptonPanel1.Controls.Add(this.txtProjectGroupName);
            this.kryptonPanel1.Controls.Add(this.lblResponsiblePerson);
            this.kryptonPanel1.Controls.Add(this.txtResponsiblePerson);
            this.kryptonPanel1.Controls.Add(this.lblPhone);
            this.kryptonPanel1.Controls.Add(this.txtPhone);
            this.kryptonPanel1.Controls.Add(this.lblStartDate);
            this.kryptonPanel1.Controls.Add(this.dtpStartDate);
            this.kryptonPanel1.Controls.Add(this.lblIs_enabled);
            this.kryptonPanel1.Controls.Add(this.chkIs_enabled);
            this.kryptonPanel1.Controls.Add(this.lblEndDate);
            this.kryptonPanel1.Controls.Add(this.dtpEndDate);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Controls.Add(this.lblNotes);
            this.kryptonPanel1.Controls.Add(this.txtNotes);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(492, 460);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // lblProjectGroupCode
            // 
            this.lblProjectGroupCode.Location = new System.Drawing.Point(40, 16);
            this.lblProjectGroupCode.Name = "lblProjectGroupCode";
            this.lblProjectGroupCode.Size = new System.Drawing.Size(75, 20);
            this.lblProjectGroupCode.TabIndex = 13;
            this.lblProjectGroupCode.Values.Text = "项目组代号";
            // 
            // txtProjectGroupCode
            // 
            this.txtProjectGroupCode.Location = new System.Drawing.Point(120, 12);
            this.txtProjectGroupCode.Name = "txtProjectGroupCode";
            this.txtProjectGroupCode.Size = new System.Drawing.Size(243, 23);
            this.txtProjectGroupCode.TabIndex = 14;
            // 
            // lblProjectGroupName
            // 
            this.lblProjectGroupName.Location = new System.Drawing.Point(40, 67);
            this.lblProjectGroupName.Name = "lblProjectGroupName";
            this.lblProjectGroupName.Size = new System.Drawing.Size(75, 20);
            this.lblProjectGroupName.TabIndex = 15;
            this.lblProjectGroupName.Values.Text = "项目组名称";
            // 
            // txtProjectGroupName
            // 
            this.txtProjectGroupName.Location = new System.Drawing.Point(120, 68);
            this.txtProjectGroupName.Name = "txtProjectGroupName";
            this.txtProjectGroupName.Size = new System.Drawing.Size(243, 23);
            this.txtProjectGroupName.TabIndex = 16;
            // 
            // lblResponsiblePerson
            // 
            this.lblResponsiblePerson.Location = new System.Drawing.Point(66, 101);
            this.lblResponsiblePerson.Name = "lblResponsiblePerson";
            this.lblResponsiblePerson.Size = new System.Drawing.Size(49, 20);
            this.lblResponsiblePerson.TabIndex = 17;
            this.lblResponsiblePerson.Values.Text = "负责人";
            // 
            // txtResponsiblePerson
            // 
            this.txtResponsiblePerson.Location = new System.Drawing.Point(120, 95);
            this.txtResponsiblePerson.Name = "txtResponsiblePerson";
            this.txtResponsiblePerson.Size = new System.Drawing.Size(243, 23);
            this.txtResponsiblePerson.TabIndex = 18;
            // 
            // lblPhone
            // 
            this.lblPhone.Location = new System.Drawing.Point(79, 126);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(36, 20);
            this.lblPhone.TabIndex = 19;
            this.lblPhone.Values.Text = "电话";
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(120, 125);
            this.txtPhone.Multiline = true;
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(243, 21);
            this.txtPhone.TabIndex = 20;
            // 
            // lblStartDate
            // 
            this.lblStartDate.Location = new System.Drawing.Point(53, 152);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(62, 20);
            this.lblStartDate.TabIndex = 21;
            this.lblStartDate.Values.Text = "启动时间";
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Location = new System.Drawing.Point(120, 154);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.ShowCheckBox = true;
            this.dtpStartDate.Size = new System.Drawing.Size(243, 21);
            this.dtpStartDate.TabIndex = 22;
            // 
            // lblIs_enabled
            // 
            this.lblIs_enabled.Location = new System.Drawing.Point(53, 181);
            this.lblIs_enabled.Name = "lblIs_enabled";
            this.lblIs_enabled.Size = new System.Drawing.Size(62, 20);
            this.lblIs_enabled.TabIndex = 23;
            this.lblIs_enabled.Values.Text = "是否启用";
            // 
            // chkIs_enabled
            // 
            this.chkIs_enabled.Checked = true;
            this.chkIs_enabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIs_enabled.Location = new System.Drawing.Point(120, 183);
            this.chkIs_enabled.Name = "chkIs_enabled";
            this.chkIs_enabled.Size = new System.Drawing.Size(19, 13);
            this.chkIs_enabled.TabIndex = 24;
            this.chkIs_enabled.Values.Text = "";
            // 
            // lblEndDate
            // 
            this.lblEndDate.Location = new System.Drawing.Point(53, 206);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(62, 20);
            this.lblEndDate.TabIndex = 25;
            this.lblEndDate.Values.Text = "结束时间";
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Location = new System.Drawing.Point(120, 206);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.ShowCheckBox = true;
            this.dtpEndDate.Size = new System.Drawing.Size(243, 21);
            this.dtpEndDate.TabIndex = 26;
            // 
            // lblNotes
            // 
            this.lblNotes.Location = new System.Drawing.Point(79, 245);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(36, 20);
            this.lblNotes.TabIndex = 9;
            this.lblNotes.Values.Text = "备注";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(120, 241);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(243, 105);
            this.txtNotes.TabIndex = 9;
            // 
            // cmbDepartment
            // 
            this.cmbDepartment.DropDownWidth = 100;
            this.cmbDepartment.IntegralHeight = false;
            this.cmbDepartment.Location = new System.Drawing.Point(120, 41);
            this.cmbDepartment.Name = "cmbDepartment";
            this.cmbDepartment.Size = new System.Drawing.Size(243, 21);
            this.cmbDepartment.TabIndex = 73;
            // 
            // lblDepartmentID
            // 
            this.lblDepartmentID.Location = new System.Drawing.Point(79, 40);
            this.lblDepartmentID.Name = "lblDepartmentID";
            this.lblDepartmentID.Size = new System.Drawing.Size(36, 20);
            this.lblDepartmentID.TabIndex = 72;
            this.lblDepartmentID.Values.Text = "部门";
            // 
            // UCProjectGroupEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 460);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCProjectGroupEdit";
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDepartment)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;










        private Krypton.Toolkit.KryptonLabel lblNotes;
        private Krypton.Toolkit.KryptonTextBox txtNotes;
        private Krypton.Toolkit.KryptonLabel lblProjectGroupCode;
        private Krypton.Toolkit.KryptonTextBox txtProjectGroupCode;
        private Krypton.Toolkit.KryptonLabel lblProjectGroupName;
        private Krypton.Toolkit.KryptonTextBox txtProjectGroupName;
        private Krypton.Toolkit.KryptonLabel lblResponsiblePerson;
        private Krypton.Toolkit.KryptonTextBox txtResponsiblePerson;
        private Krypton.Toolkit.KryptonLabel lblPhone;
        private Krypton.Toolkit.KryptonTextBox txtPhone;
        private Krypton.Toolkit.KryptonLabel lblStartDate;
        private Krypton.Toolkit.KryptonDateTimePicker dtpStartDate;
        private Krypton.Toolkit.KryptonLabel lblIs_enabled;
        private Krypton.Toolkit.KryptonCheckBox chkIs_enabled;
        private Krypton.Toolkit.KryptonLabel lblEndDate;
        private Krypton.Toolkit.KryptonDateTimePicker dtpEndDate;
        private Krypton.Toolkit.KryptonComboBox cmbDepartment;
        private Krypton.Toolkit.KryptonLabel lblDepartmentID;
    }
}
