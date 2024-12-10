namespace RUINORERP.UI.CRM
{
    partial class UCCRMFollowUpRecordsEdit
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
            this.kryptonLabel4 = new Krypton.Toolkit.KryptonLabel();
            this.cmbFollowUpMethod = new Krypton.Toolkit.KryptonComboBox();
            this.kryptonLabel2 = new Krypton.Toolkit.KryptonLabel();
            this.cmbPlanID = new Krypton.Toolkit.KryptonComboBox();
            this.lblLeads = new Krypton.Toolkit.KryptonLabel();
            this.cmbLeads = new Krypton.Toolkit.KryptonComboBox();
            this.lblFollowUpSubject = new Krypton.Toolkit.KryptonLabel();
            this.txtFollowUpSubject = new Krypton.Toolkit.KryptonTextBox();
            this.kPanelPlanSubject = new Krypton.Toolkit.KryptonPanel();
            this.lblFollowUpDate = new Krypton.Toolkit.KryptonLabel();
            this.dtpFollowUpDate = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lblCustomer_id = new Krypton.Toolkit.KryptonLabel();
            this.cmbCustomer_id = new Krypton.Toolkit.KryptonComboBox();
            this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
            this.lblFollowUpContent = new Krypton.Toolkit.KryptonLabel();
            this.txtFollowUpContent = new Krypton.Toolkit.KryptonTextBox();
            this.cmbEmployee_ID = new Krypton.Toolkit.KryptonComboBox();
            this.lblNotes = new Krypton.Toolkit.KryptonLabel();
            this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFollowUpMethod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPlanID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLeads)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kPanelPlanSubject)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCustomer_id)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmployee_ID)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(170, 573);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(288, 573);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.kryptonLabel4);
            this.kryptonPanel1.Controls.Add(this.cmbFollowUpMethod);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel2);
            this.kryptonPanel1.Controls.Add(this.cmbPlanID);
            this.kryptonPanel1.Controls.Add(this.lblLeads);
            this.kryptonPanel1.Controls.Add(this.cmbLeads);
            this.kryptonPanel1.Controls.Add(this.lblFollowUpSubject);
            this.kryptonPanel1.Controls.Add(this.txtFollowUpSubject);
            this.kryptonPanel1.Controls.Add(this.kPanelPlanSubject);
            this.kryptonPanel1.Controls.Add(this.lblFollowUpDate);
            this.kryptonPanel1.Controls.Add(this.dtpFollowUpDate);
            this.kryptonPanel1.Controls.Add(this.lblCustomer_id);
            this.kryptonPanel1.Controls.Add(this.cmbCustomer_id);
            this.kryptonPanel1.Controls.Add(this.lblEmployee_ID);
            this.kryptonPanel1.Controls.Add(this.lblFollowUpContent);
            this.kryptonPanel1.Controls.Add(this.txtFollowUpContent);
            this.kryptonPanel1.Controls.Add(this.cmbEmployee_ID);
            this.kryptonPanel1.Controls.Add(this.lblNotes);
            this.kryptonPanel1.Controls.Add(this.txtNotes);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(769, 618);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // kryptonLabel4
            // 
            this.kryptonLabel4.Location = new System.Drawing.Point(38, 65);
            this.kryptonLabel4.Name = "kryptonLabel4";
            this.kryptonLabel4.Size = new System.Drawing.Size(62, 20);
            this.kryptonLabel4.TabIndex = 181;
            this.kryptonLabel4.Values.Text = "跟进方式";
            // 
            // cmbFollowUpMethod
            // 
            this.cmbFollowUpMethod.DropDownWidth = 100;
            this.cmbFollowUpMethod.IntegralHeight = false;
            this.cmbFollowUpMethod.Location = new System.Drawing.Point(103, 65);
            this.cmbFollowUpMethod.Name = "cmbFollowUpMethod";
            this.cmbFollowUpMethod.Size = new System.Drawing.Size(228, 21);
            this.cmbFollowUpMethod.TabIndex = 180;
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(376, 13);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(62, 20);
            this.kryptonLabel2.TabIndex = 177;
            this.kryptonLabel2.Values.Text = "来源计划";
            // 
            // cmbPlanID
            // 
            this.cmbPlanID.DropDownWidth = 100;
            this.cmbPlanID.IntegralHeight = false;
            this.cmbPlanID.Location = new System.Drawing.Point(441, 11);
            this.cmbPlanID.Name = "cmbPlanID";
            this.cmbPlanID.Size = new System.Drawing.Size(228, 21);
            this.cmbPlanID.TabIndex = 176;
            // 
            // lblLeads
            // 
            this.lblLeads.Location = new System.Drawing.Point(376, 38);
            this.lblLeads.Name = "lblLeads";
            this.lblLeads.Size = new System.Drawing.Size(62, 20);
            this.lblLeads.TabIndex = 175;
            this.lblLeads.Values.Text = "来源线索";
            // 
            // cmbLeads
            // 
            this.cmbLeads.DropDownWidth = 100;
            this.cmbLeads.IntegralHeight = false;
            this.cmbLeads.Location = new System.Drawing.Point(441, 38);
            this.cmbLeads.Name = "cmbLeads";
            this.cmbLeads.Size = new System.Drawing.Size(228, 21);
            this.cmbLeads.TabIndex = 174;
            // 
            // lblFollowUpSubject
            // 
            this.lblFollowUpSubject.Location = new System.Drawing.Point(29, 100);
            this.lblFollowUpSubject.Name = "lblFollowUpSubject";
            this.lblFollowUpSubject.Size = new System.Drawing.Size(62, 20);
            this.lblFollowUpSubject.TabIndex = 172;
            this.lblFollowUpSubject.Values.Text = "跟进主题";
            // 
            // txtFollowUpSubject
            // 
            this.txtFollowUpSubject.Location = new System.Drawing.Point(102, 96);
            this.txtFollowUpSubject.Name = "txtFollowUpSubject";
            this.txtFollowUpSubject.Size = new System.Drawing.Size(567, 23);
            this.txtFollowUpSubject.TabIndex = 173;
            // 
            // kPanelPlanSubject
            // 
            this.kPanelPlanSubject.Location = new System.Drawing.Point(103, 128);
            this.kPanelPlanSubject.Name = "kPanelPlanSubject";
            this.kPanelPlanSubject.Size = new System.Drawing.Size(566, 180);
            this.kPanelPlanSubject.TabIndex = 171;
            // 
            // lblFollowUpDate
            // 
            this.lblFollowUpDate.Location = new System.Drawing.Point(36, 320);
            this.lblFollowUpDate.Name = "lblFollowUpDate";
            this.lblFollowUpDate.Size = new System.Drawing.Size(62, 20);
            this.lblFollowUpDate.TabIndex = 164;
            this.lblFollowUpDate.Values.Text = "跟进日期";
            // 
            // dtpFollowUpDate
            // 
            this.dtpFollowUpDate.Location = new System.Drawing.Point(102, 320);
            this.dtpFollowUpDate.Name = "dtpFollowUpDate";
            this.dtpFollowUpDate.Size = new System.Drawing.Size(227, 21);
            this.dtpFollowUpDate.TabIndex = 165;
            // 
            // lblCustomer_id
            // 
            this.lblCustomer_id.Location = new System.Drawing.Point(37, 38);
            this.lblCustomer_id.Name = "lblCustomer_id";
            this.lblCustomer_id.Size = new System.Drawing.Size(62, 20);
            this.lblCustomer_id.TabIndex = 163;
            this.lblCustomer_id.Values.Text = "目标客户";
            // 
            // cmbCustomer_id
            // 
            this.cmbCustomer_id.DropDownWidth = 100;
            this.cmbCustomer_id.IntegralHeight = false;
            this.cmbCustomer_id.Location = new System.Drawing.Point(102, 38);
            this.cmbCustomer_id.Name = "cmbCustomer_id";
            this.cmbCustomer_id.Size = new System.Drawing.Size(228, 21);
            this.cmbCustomer_id.TabIndex = 162;
            // 
            // lblEmployee_ID
            // 
            this.lblEmployee_ID.Location = new System.Drawing.Point(50, 12);
            this.lblEmployee_ID.Name = "lblEmployee_ID";
            this.lblEmployee_ID.Size = new System.Drawing.Size(53, 20);
            this.lblEmployee_ID.TabIndex = 161;
            this.lblEmployee_ID.Values.Text = " 跟进人";
            // 
            // lblFollowUpContent
            // 
            this.lblFollowUpContent.Location = new System.Drawing.Point(35, 347);
            this.lblFollowUpContent.Name = "lblFollowUpContent";
            this.lblFollowUpContent.Size = new System.Drawing.Size(62, 20);
            this.lblFollowUpContent.TabIndex = 159;
            this.lblFollowUpContent.Values.Text = "跟进内容";
            // 
            // txtFollowUpContent
            // 
            this.txtFollowUpContent.Location = new System.Drawing.Point(103, 347);
            this.txtFollowUpContent.Multiline = true;
            this.txtFollowUpContent.Name = "txtFollowUpContent";
            this.txtFollowUpContent.Size = new System.Drawing.Size(396, 90);
            this.txtFollowUpContent.TabIndex = 160;
            // 
            // cmbEmployee_ID
            // 
            this.cmbEmployee_ID.DropDownWidth = 100;
            this.cmbEmployee_ID.IntegralHeight = false;
            this.cmbEmployee_ID.Location = new System.Drawing.Point(102, 12);
            this.cmbEmployee_ID.Name = "cmbEmployee_ID";
            this.cmbEmployee_ID.Size = new System.Drawing.Size(228, 21);
            this.cmbEmployee_ID.TabIndex = 77;
            // 
            // lblNotes
            // 
            this.lblNotes.Location = new System.Drawing.Point(64, 444);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(36, 20);
            this.lblNotes.TabIndex = 51;
            this.lblNotes.Values.Text = "备注";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(103, 443);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(397, 124);
            this.txtNotes.TabIndex = 52;
            // 
            // UCCRMFollowUpRecordsEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(769, 618);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCCRMFollowUpRecordsEdit";
            this.Load += new System.EventHandler(this.UCLeadsEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFollowUpMethod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPlanID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLeads)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kPanelPlanSubject)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCustomer_id)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmployee_ID)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonLabel lblNotes;
        private Krypton.Toolkit.KryptonTextBox txtNotes;
        private Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;
        private Krypton.Toolkit.KryptonLabel lblFollowUpContent;
        private Krypton.Toolkit.KryptonTextBox txtFollowUpContent;
        private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
        private Krypton.Toolkit.KryptonComboBox cmbCustomer_id;
        private Krypton.Toolkit.KryptonLabel lblCustomer_id;
        private Krypton.Toolkit.KryptonLabel lblFollowUpDate;
        private Krypton.Toolkit.KryptonDateTimePicker dtpFollowUpDate;
        private Krypton.Toolkit.KryptonPanel kPanelPlanSubject;
        private Krypton.Toolkit.KryptonLabel lblFollowUpSubject;
        private Krypton.Toolkit.KryptonTextBox txtFollowUpSubject;
        private Krypton.Toolkit.KryptonLabel lblLeads;
        private Krypton.Toolkit.KryptonComboBox cmbLeads;
        private Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private Krypton.Toolkit.KryptonComboBox cmbPlanID;
        private Krypton.Toolkit.KryptonLabel kryptonLabel4;
        private Krypton.Toolkit.KryptonComboBox cmbFollowUpMethod;
    }
}
