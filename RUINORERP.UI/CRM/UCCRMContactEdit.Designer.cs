namespace RUINORERP.UI.CRM
{
    partial class UCCRMContactEdit
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
            this.lblCustomer_id = new Krypton.Toolkit.KryptonLabel();
            this.cmbCustomer_id = new Krypton.Toolkit.KryptonComboBox();
            this.lblSocialTools = new Krypton.Toolkit.KryptonLabel();
            this.txtSocialTools = new Krypton.Toolkit.KryptonTextBox();
            this.lblContact_Name = new Krypton.Toolkit.KryptonLabel();
            this.txtContact_Name = new Krypton.Toolkit.KryptonTextBox();
            this.lblContact_Email = new Krypton.Toolkit.KryptonLabel();
            this.txtContact_Email = new Krypton.Toolkit.KryptonTextBox();
            this.lblContact_Phone = new Krypton.Toolkit.KryptonLabel();
            this.txtContact_Phone = new Krypton.Toolkit.KryptonTextBox();
            this.lblPosition = new Krypton.Toolkit.KryptonLabel();
            this.txtPosition = new Krypton.Toolkit.KryptonTextBox();
            this.lblPreferences = new Krypton.Toolkit.KryptonLabel();
            this.txtPreferences = new Krypton.Toolkit.KryptonTextBox();
            this.lblAddress = new Krypton.Toolkit.KryptonLabel();
            this.txtAddress = new Krypton.Toolkit.KryptonTextBox();
            this.lblNotes = new Krypton.Toolkit.KryptonLabel();
            this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCustomer_id)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(192, 382);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定(&S)";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(310, 382);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消(&C)";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.lblCustomer_id);
            this.kryptonPanel1.Controls.Add(this.cmbCustomer_id);
            this.kryptonPanel1.Controls.Add(this.lblSocialTools);
            this.kryptonPanel1.Controls.Add(this.txtSocialTools);
            this.kryptonPanel1.Controls.Add(this.lblContact_Name);
            this.kryptonPanel1.Controls.Add(this.txtContact_Name);
            this.kryptonPanel1.Controls.Add(this.lblContact_Email);
            this.kryptonPanel1.Controls.Add(this.txtContact_Email);
            this.kryptonPanel1.Controls.Add(this.lblContact_Phone);
            this.kryptonPanel1.Controls.Add(this.txtContact_Phone);
            this.kryptonPanel1.Controls.Add(this.lblPosition);
            this.kryptonPanel1.Controls.Add(this.txtPosition);
            this.kryptonPanel1.Controls.Add(this.lblPreferences);
            this.kryptonPanel1.Controls.Add(this.txtPreferences);
            this.kryptonPanel1.Controls.Add(this.lblAddress);
            this.kryptonPanel1.Controls.Add(this.txtAddress);
            this.kryptonPanel1.Controls.Add(this.lblNotes);
            this.kryptonPanel1.Controls.Add(this.txtNotes);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(609, 424);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // lblCustomer_id
            // 
            this.lblCustomer_id.Location = new System.Drawing.Point(79, 38);
            this.lblCustomer_id.Name = "lblCustomer_id";
            this.lblCustomer_id.Size = new System.Drawing.Size(62, 20);
            this.lblCustomer_id.TabIndex = 53;
            this.lblCustomer_id.Values.Text = "目标客户";
            // 
            // cmbCustomer_id
            // 
            this.cmbCustomer_id.DropDownWidth = 100;
            this.cmbCustomer_id.IntegralHeight = false;
            this.cmbCustomer_id.Location = new System.Drawing.Point(152, 34);
            this.cmbCustomer_id.Name = "cmbCustomer_id";
            this.cmbCustomer_id.Size = new System.Drawing.Size(345, 21);
            this.cmbCustomer_id.TabIndex = 54;
            // 
            // lblSocialTools
            // 
            this.lblSocialTools.Location = new System.Drawing.Point(79, 63);
            this.lblSocialTools.Name = "lblSocialTools";
            this.lblSocialTools.Size = new System.Drawing.Size(62, 20);
            this.lblSocialTools.TabIndex = 55;
            this.lblSocialTools.Values.Text = "社交工具";
            // 
            // txtSocialTools
            // 
            this.txtSocialTools.Location = new System.Drawing.Point(152, 59);
            this.txtSocialTools.Name = "txtSocialTools";
            this.txtSocialTools.Size = new System.Drawing.Size(345, 23);
            this.txtSocialTools.TabIndex = 56;
            // 
            // lblContact_Name
            // 
            this.lblContact_Name.Location = new System.Drawing.Point(105, 88);
            this.lblContact_Name.Name = "lblContact_Name";
            this.lblContact_Name.Size = new System.Drawing.Size(36, 20);
            this.lblContact_Name.TabIndex = 57;
            this.lblContact_Name.Values.Text = "姓名";
            // 
            // txtContact_Name
            // 
            this.txtContact_Name.Location = new System.Drawing.Point(152, 84);
            this.txtContact_Name.Name = "txtContact_Name";
            this.txtContact_Name.Size = new System.Drawing.Size(345, 23);
            this.txtContact_Name.TabIndex = 58;
            // 
            // lblContact_Email
            // 
            this.lblContact_Email.Location = new System.Drawing.Point(105, 113);
            this.lblContact_Email.Name = "lblContact_Email";
            this.lblContact_Email.Size = new System.Drawing.Size(36, 20);
            this.lblContact_Email.TabIndex = 59;
            this.lblContact_Email.Values.Text = "邮箱";
            // 
            // txtContact_Email
            // 
            this.txtContact_Email.Location = new System.Drawing.Point(152, 109);
            this.txtContact_Email.Name = "txtContact_Email";
            this.txtContact_Email.Size = new System.Drawing.Size(345, 23);
            this.txtContact_Email.TabIndex = 60;
            // 
            // lblContact_Phone
            // 
            this.lblContact_Phone.Location = new System.Drawing.Point(105, 138);
            this.lblContact_Phone.Name = "lblContact_Phone";
            this.lblContact_Phone.Size = new System.Drawing.Size(36, 20);
            this.lblContact_Phone.TabIndex = 61;
            this.lblContact_Phone.Values.Text = "电话";
            // 
            // txtContact_Phone
            // 
            this.txtContact_Phone.Location = new System.Drawing.Point(152, 134);
            this.txtContact_Phone.Name = "txtContact_Phone";
            this.txtContact_Phone.Size = new System.Drawing.Size(345, 23);
            this.txtContact_Phone.TabIndex = 62;
            // 
            // lblPosition
            // 
            this.lblPosition.Location = new System.Drawing.Point(105, 163);
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(36, 20);
            this.lblPosition.TabIndex = 63;
            this.lblPosition.Values.Text = "职位";
            // 
            // txtPosition
            // 
            this.txtPosition.Location = new System.Drawing.Point(152, 159);
            this.txtPosition.Name = "txtPosition";
            this.txtPosition.Size = new System.Drawing.Size(345, 23);
            this.txtPosition.TabIndex = 64;
            // 
            // lblPreferences
            // 
            this.lblPreferences.Location = new System.Drawing.Point(105, 188);
            this.lblPreferences.Name = "lblPreferences";
            this.lblPreferences.Size = new System.Drawing.Size(36, 20);
            this.lblPreferences.TabIndex = 65;
            this.lblPreferences.Values.Text = "爱好";
            // 
            // txtPreferences
            // 
            this.txtPreferences.Location = new System.Drawing.Point(152, 184);
            this.txtPreferences.Name = "txtPreferences";
            this.txtPreferences.Size = new System.Drawing.Size(345, 23);
            this.txtPreferences.TabIndex = 66;
            // 
            // lblAddress
            // 
            this.lblAddress.Location = new System.Drawing.Point(79, 213);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(62, 20);
            this.lblAddress.TabIndex = 67;
            this.lblAddress.Values.Text = "联系地址";
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(152, 209);
            this.txtAddress.Multiline = true;
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(345, 21);
            this.txtAddress.TabIndex = 68;
            // 
            // lblNotes
            // 
            this.lblNotes.Location = new System.Drawing.Point(105, 243);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(36, 20);
            this.lblNotes.TabIndex = 51;
            this.lblNotes.Values.Text = "备注";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(152, 242);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(345, 96);
            this.txtNotes.TabIndex = 52;
            // 
            // UCCRMContactEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 424);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCCRMContactEdit";
            this.Load += new System.EventHandler(this.UCLeadsEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCustomer_id)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonLabel lblNotes;
        private Krypton.Toolkit.KryptonTextBox txtNotes;
        private Krypton.Toolkit.KryptonLabel lblCustomer_id;
        private Krypton.Toolkit.KryptonComboBox cmbCustomer_id;
        private Krypton.Toolkit.KryptonLabel lblSocialTools;
        private Krypton.Toolkit.KryptonTextBox txtSocialTools;
        private Krypton.Toolkit.KryptonLabel lblContact_Name;
        private Krypton.Toolkit.KryptonTextBox txtContact_Name;
        private Krypton.Toolkit.KryptonLabel lblContact_Email;
        private Krypton.Toolkit.KryptonTextBox txtContact_Email;
        private Krypton.Toolkit.KryptonLabel lblContact_Phone;
        private Krypton.Toolkit.KryptonTextBox txtContact_Phone;
        private Krypton.Toolkit.KryptonLabel lblPosition;
        private Krypton.Toolkit.KryptonTextBox txtPosition;
        private Krypton.Toolkit.KryptonLabel lblPreferences;
        private Krypton.Toolkit.KryptonTextBox txtPreferences;
        private Krypton.Toolkit.KryptonLabel lblAddress;
        private Krypton.Toolkit.KryptonTextBox txtAddress;
    }
}
