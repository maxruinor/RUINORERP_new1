namespace RUINORERP.UI.CRM.DockUI
{
    partial class UCFollowUpRecord
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
            this.kryptonLabel4 = new Krypton.Toolkit.KryptonLabel();
            this.cmbFollowUpMethod = new Krypton.Toolkit.KryptonComboBox();
            this.lblFollowUpSubject = new Krypton.Toolkit.KryptonLabel();
            this.txtFollowUpSubject = new Krypton.Toolkit.KryptonTextBox();
            this.lblFollowUpDate = new Krypton.Toolkit.KryptonLabel();
            this.dtpFollowUpDate = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
            this.lblFollowUpContent = new Krypton.Toolkit.KryptonLabel();
            this.txtFollowUpContent = new Krypton.Toolkit.KryptonTextBox();
            this.cmbEmployee_ID = new Krypton.Toolkit.KryptonComboBox();
            this.klinklblDetail = new Krypton.Toolkit.KryptonLinkLabel();
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFollowUpMethod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmployee_ID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonLabel4
            // 
            this.kryptonLabel4.Location = new System.Drawing.Point(6, 34);
            this.kryptonLabel4.Name = "kryptonLabel4";
            this.kryptonLabel4.Size = new System.Drawing.Size(62, 20);
            this.kryptonLabel4.TabIndex = 200;
            this.kryptonLabel4.Values.Text = "跟进方式";
            // 
            // cmbFollowUpMethod
            // 
            this.cmbFollowUpMethod.DropDownWidth = 100;
            this.cmbFollowUpMethod.IntegralHeight = false;
            this.cmbFollowUpMethod.Location = new System.Drawing.Point(71, 34);
            this.cmbFollowUpMethod.Name = "cmbFollowUpMethod";
            this.cmbFollowUpMethod.Size = new System.Drawing.Size(115, 21);
            this.cmbFollowUpMethod.TabIndex = 199;
            // 
            // lblFollowUpSubject
            // 
            this.lblFollowUpSubject.Location = new System.Drawing.Point(6, 60);
            this.lblFollowUpSubject.Name = "lblFollowUpSubject";
            this.lblFollowUpSubject.Size = new System.Drawing.Size(62, 20);
            this.lblFollowUpSubject.TabIndex = 193;
            this.lblFollowUpSubject.Values.Text = "跟进主题";
            // 
            // txtFollowUpSubject
            // 
            this.txtFollowUpSubject.Location = new System.Drawing.Point(71, 59);
            this.txtFollowUpSubject.Name = "txtFollowUpSubject";
            this.txtFollowUpSubject.Size = new System.Drawing.Size(302, 23);
            this.txtFollowUpSubject.TabIndex = 194;
            // 
            // lblFollowUpDate
            // 
            this.lblFollowUpDate.Location = new System.Drawing.Point(9, 9);
            this.lblFollowUpDate.Name = "lblFollowUpDate";
            this.lblFollowUpDate.Size = new System.Drawing.Size(62, 20);
            this.lblFollowUpDate.TabIndex = 190;
            this.lblFollowUpDate.Values.Text = "跟进日期";
            // 
            // dtpFollowUpDate
            // 
            this.dtpFollowUpDate.CalendarTodayDate = new System.DateTime(2024, 12, 9, 0, 0, 0, 0);
            this.dtpFollowUpDate.Location = new System.Drawing.Point(71, 8);
            this.dtpFollowUpDate.Name = "dtpFollowUpDate";
            this.dtpFollowUpDate.Size = new System.Drawing.Size(115, 21);
            this.dtpFollowUpDate.TabIndex = 191;
            // 
            // lblEmployee_ID
            // 
            this.lblEmployee_ID.Location = new System.Drawing.Point(187, 36);
            this.lblEmployee_ID.Name = "lblEmployee_ID";
            this.lblEmployee_ID.Size = new System.Drawing.Size(53, 20);
            this.lblEmployee_ID.TabIndex = 187;
            this.lblEmployee_ID.Values.Text = " 跟进人";
            // 
            // lblFollowUpContent
            // 
            this.lblFollowUpContent.Location = new System.Drawing.Point(6, 88);
            this.lblFollowUpContent.Name = "lblFollowUpContent";
            this.lblFollowUpContent.Size = new System.Drawing.Size(62, 20);
            this.lblFollowUpContent.TabIndex = 185;
            this.lblFollowUpContent.Values.Text = "跟进内容";
            // 
            // txtFollowUpContent
            // 
            this.txtFollowUpContent.Location = new System.Drawing.Point(72, 88);
            this.txtFollowUpContent.Multiline = true;
            this.txtFollowUpContent.Name = "txtFollowUpContent";
            this.txtFollowUpContent.Size = new System.Drawing.Size(301, 36);
            this.txtFollowUpContent.TabIndex = 186;
            // 
            // cmbEmployee_ID
            // 
            this.cmbEmployee_ID.DropDownWidth = 100;
            this.cmbEmployee_ID.IntegralHeight = false;
            this.cmbEmployee_ID.Location = new System.Drawing.Point(243, 35);
            this.cmbEmployee_ID.Name = "cmbEmployee_ID";
            this.cmbEmployee_ID.Size = new System.Drawing.Size(130, 21);
            this.cmbEmployee_ID.TabIndex = 184;
            // 
            // klinklblDetail
            // 
            this.klinklblDetail.Location = new System.Drawing.Point(255, 9);
            this.klinklblDetail.Name = "klinklblDetail";
            this.klinklblDetail.Size = new System.Drawing.Size(36, 20);
            this.klinklblDetail.TabIndex = 201;
            this.klinklblDetail.Values.Text = "详情";
            this.klinklblDetail.Visible = false;
            this.klinklblDetail.LinkClicked += new System.EventHandler(this.klinklblDetail_LinkClicked);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.lblEmployee_ID);
            this.kryptonPanel1.Controls.Add(this.klinklblDetail);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel4);
            this.kryptonPanel1.Controls.Add(this.cmbFollowUpMethod);
            this.kryptonPanel1.Controls.Add(this.cmbEmployee_ID);
            this.kryptonPanel1.Controls.Add(this.lblFollowUpSubject);
            this.kryptonPanel1.Controls.Add(this.txtFollowUpContent);
            this.kryptonPanel1.Controls.Add(this.txtFollowUpSubject);
            this.kryptonPanel1.Controls.Add(this.lblFollowUpContent);
            this.kryptonPanel1.Controls.Add(this.lblFollowUpDate);
            this.kryptonPanel1.Controls.Add(this.dtpFollowUpDate);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(379, 128);
            this.kryptonPanel1.TabIndex = 202;
            // 
            // UCFollowUpRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonPanel1);
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "UCFollowUpRecord";
            this.Size = new System.Drawing.Size(379, 128);
            ((System.ComponentModel.ISupportInitialize)(this.cmbFollowUpMethod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmployee_ID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonLabel kryptonLabel4;
        private Krypton.Toolkit.KryptonComboBox cmbFollowUpMethod;
        private Krypton.Toolkit.KryptonLabel lblFollowUpSubject;
        private Krypton.Toolkit.KryptonTextBox txtFollowUpSubject;
        private Krypton.Toolkit.KryptonLabel lblFollowUpDate;
        private Krypton.Toolkit.KryptonDateTimePicker dtpFollowUpDate;
        private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
        private Krypton.Toolkit.KryptonLabel lblFollowUpContent;
        private Krypton.Toolkit.KryptonTextBox txtFollowUpContent;
        private Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;
        private Krypton.Toolkit.KryptonLinkLabel klinklblDetail;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
    }
}
