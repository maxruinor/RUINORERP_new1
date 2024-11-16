namespace RUINORERP.UI.BI
{
    partial class UCFMPayeeInfoEdit
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
            this.components = new System.ComponentModel.Container();
            RUINORERP.Global.Model.DataRowImage dataRowImage1 = new RUINORERP.Global.Model.DataRowImage();
            this.btnOk = new Krypton.Toolkit.KryptonButton();
            this.btnCancel = new Krypton.Toolkit.KryptonButton();
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            this.PicRowImage = new RUINOR.WinFormsUI.CustomPictureBox.MagicPictureBox();
            this.kryptonGroupBox1 = new Krypton.Toolkit.KryptonGroupBox();
            this.rdbis_enabledNo = new Krypton.Toolkit.KryptonRadioButton();
            this.rdbis_enabledYes = new Krypton.Toolkit.KryptonRadioButton();
            this.lblIs_enabled = new Krypton.Toolkit.KryptonLabel();
            this.groupboxIsDefault = new Krypton.Toolkit.KryptonGroupBox();
            this.rdbIsDefaultNo = new Krypton.Toolkit.KryptonRadioButton();
            this.rdbIsDefaultYes = new Krypton.Toolkit.KryptonRadioButton();
            this.lblIsDefault = new Krypton.Toolkit.KryptonLabel();
            this.cmbAccount_type = new Krypton.Toolkit.KryptonComboBox();
            this.lblaccount_type = new Krypton.Toolkit.KryptonLabel();
            this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
            this.cmbEmployee_ID = new Krypton.Toolkit.KryptonComboBox();
            this.lblCustomerVendor_ID = new Krypton.Toolkit.KryptonLabel();
            this.cmbCustomerVendor_ID = new Krypton.Toolkit.KryptonComboBox();
            this.lblAccount_name = new Krypton.Toolkit.KryptonLabel();
            this.txtAccount_name = new Krypton.Toolkit.KryptonTextBox();
            this.lblAccount_No = new Krypton.Toolkit.KryptonLabel();
            this.txtAccount_No = new Krypton.Toolkit.KryptonTextBox();
            this.lblPaymentCodeImagePath = new Krypton.Toolkit.KryptonLabel();
            this.lblBelongingBank = new Krypton.Toolkit.KryptonLabel();
            this.txtBelongingBank = new Krypton.Toolkit.KryptonTextBox();
            this.lblOpeningBank = new Krypton.Toolkit.KryptonLabel();
            this.txtOpeningBank = new Krypton.Toolkit.KryptonTextBox();
            this.lblNotes = new Krypton.Toolkit.KryptonLabel();
            this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicRowImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).BeginInit();
            this.kryptonGroupBox1.Panel.SuspendLayout();
            this.kryptonGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupboxIsDefault)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupboxIsDefault.Panel)).BeginInit();
            this.groupboxIsDefault.Panel.SuspendLayout();
            this.groupboxIsDefault.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAccount_type)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmployee_ID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCustomerVendor_ID)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(171, 499);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(289, 499);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.PicRowImage);
            this.kryptonPanel1.Controls.Add(this.kryptonGroupBox1);
            this.kryptonPanel1.Controls.Add(this.lblIs_enabled);
            this.kryptonPanel1.Controls.Add(this.groupboxIsDefault);
            this.kryptonPanel1.Controls.Add(this.lblIsDefault);
            this.kryptonPanel1.Controls.Add(this.cmbAccount_type);
            this.kryptonPanel1.Controls.Add(this.lblaccount_type);
            this.kryptonPanel1.Controls.Add(this.lblEmployee_ID);
            this.kryptonPanel1.Controls.Add(this.cmbEmployee_ID);
            this.kryptonPanel1.Controls.Add(this.lblCustomerVendor_ID);
            this.kryptonPanel1.Controls.Add(this.cmbCustomerVendor_ID);
            this.kryptonPanel1.Controls.Add(this.lblAccount_name);
            this.kryptonPanel1.Controls.Add(this.txtAccount_name);
            this.kryptonPanel1.Controls.Add(this.lblAccount_No);
            this.kryptonPanel1.Controls.Add(this.txtAccount_No);
            this.kryptonPanel1.Controls.Add(this.lblPaymentCodeImagePath);
            this.kryptonPanel1.Controls.Add(this.lblBelongingBank);
            this.kryptonPanel1.Controls.Add(this.txtBelongingBank);
            this.kryptonPanel1.Controls.Add(this.lblOpeningBank);
            this.kryptonPanel1.Controls.Add(this.txtOpeningBank);
            this.kryptonPanel1.Controls.Add(this.lblNotes);
            this.kryptonPanel1.Controls.Add(this.txtNotes);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(584, 536);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // PicRowImage
            // 
            this.PicRowImage.AllowDrop = true;
            this.PicRowImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PicRowImage.Location = new System.Drawing.Point(163, 204);
            this.PicRowImage.Name = "PicRowImage";
            dataRowImage1.Dir = null;
            dataRowImage1.image = null;
            dataRowImage1.ImageBytes = null;
            dataRowImage1.ImageFullName = null;
            dataRowImage1.newhash = null;
            dataRowImage1.oldhash = null;
            dataRowImage1.realName = null;
            this.PicRowImage.RowImage = dataRowImage1;
            this.PicRowImage.Size = new System.Drawing.Size(183, 176);
            this.PicRowImage.TabIndex = 70;
            this.PicRowImage.TabStop = false;
            // 
            // kryptonGroupBox1
            // 
            this.kryptonGroupBox1.CaptionVisible = false;
            this.kryptonGroupBox1.Location = new System.Drawing.Point(337, 441);
            this.kryptonGroupBox1.Name = "kryptonGroupBox1";
            // 
            // kryptonGroupBox1.Panel
            // 
            this.kryptonGroupBox1.Panel.Controls.Add(this.rdbis_enabledNo);
            this.kryptonGroupBox1.Panel.Controls.Add(this.rdbis_enabledYes);
            this.kryptonGroupBox1.Size = new System.Drawing.Size(81, 40);
            this.kryptonGroupBox1.TabIndex = 69;
            this.kryptonGroupBox1.Values.Heading = "是";
            // 
            // rdbis_enabledNo
            // 
            this.rdbis_enabledNo.Location = new System.Drawing.Point(44, 8);
            this.rdbis_enabledNo.Name = "rdbis_enabledNo";
            this.rdbis_enabledNo.Size = new System.Drawing.Size(35, 20);
            this.rdbis_enabledNo.TabIndex = 1;
            this.rdbis_enabledNo.Values.Text = "否";
            // 
            // rdbis_enabledYes
            // 
            this.rdbis_enabledYes.Location = new System.Drawing.Point(5, 8);
            this.rdbis_enabledYes.Name = "rdbis_enabledYes";
            this.rdbis_enabledYes.Size = new System.Drawing.Size(35, 20);
            this.rdbis_enabledYes.TabIndex = 0;
            this.rdbis_enabledYes.Values.Text = "是";
            // 
            // lblIs_enabled
            // 
            this.lblIs_enabled.Location = new System.Drawing.Point(271, 450);
            this.lblIs_enabled.Name = "lblIs_enabled";
            this.lblIs_enabled.Size = new System.Drawing.Size(62, 20);
            this.lblIs_enabled.TabIndex = 68;
            this.lblIs_enabled.Values.Text = "是否启用";
            // 
            // groupboxIsDefault
            // 
            this.groupboxIsDefault.CaptionVisible = false;
            this.groupboxIsDefault.Location = new System.Drawing.Point(160, 441);
            this.groupboxIsDefault.Name = "groupboxIsDefault";
            // 
            // groupboxIsDefault.Panel
            // 
            this.groupboxIsDefault.Panel.Controls.Add(this.rdbIsDefaultNo);
            this.groupboxIsDefault.Panel.Controls.Add(this.rdbIsDefaultYes);
            this.groupboxIsDefault.Size = new System.Drawing.Size(88, 40);
            this.groupboxIsDefault.TabIndex = 67;
            this.groupboxIsDefault.Values.Heading = "是";
            // 
            // rdbIsDefaultNo
            // 
            this.rdbIsDefaultNo.Location = new System.Drawing.Point(47, 8);
            this.rdbIsDefaultNo.Name = "rdbIsDefaultNo";
            this.rdbIsDefaultNo.Size = new System.Drawing.Size(35, 20);
            this.rdbIsDefaultNo.TabIndex = 1;
            this.rdbIsDefaultNo.Values.Text = "否";
            // 
            // rdbIsDefaultYes
            // 
            this.rdbIsDefaultYes.Location = new System.Drawing.Point(5, 8);
            this.rdbIsDefaultYes.Name = "rdbIsDefaultYes";
            this.rdbIsDefaultYes.Size = new System.Drawing.Size(35, 20);
            this.rdbIsDefaultYes.TabIndex = 0;
            this.rdbIsDefaultYes.Values.Text = "是";
            // 
            // lblIsDefault
            // 
            this.lblIsDefault.Location = new System.Drawing.Point(94, 451);
            this.lblIsDefault.Name = "lblIsDefault";
            this.lblIsDefault.Size = new System.Drawing.Size(62, 20);
            this.lblIsDefault.TabIndex = 66;
            this.lblIsDefault.Values.Text = "是否默认";
            // 
            // cmbAccount_type
            // 
            this.cmbAccount_type.DropDownWidth = 100;
            this.cmbAccount_type.IntegralHeight = false;
            this.cmbAccount_type.Location = new System.Drawing.Point(163, 66);
            this.cmbAccount_type.Name = "cmbAccount_type";
            this.cmbAccount_type.Size = new System.Drawing.Size(158, 21);
            this.cmbAccount_type.TabIndex = 30;
            // 
            // lblaccount_type
            // 
            this.lblaccount_type.Location = new System.Drawing.Point(98, 66);
            this.lblaccount_type.Name = "lblaccount_type";
            this.lblaccount_type.Size = new System.Drawing.Size(62, 20);
            this.lblaccount_type.TabIndex = 29;
            this.lblaccount_type.Values.Text = "账户类型";
            // 
            // lblEmployee_ID
            // 
            this.lblEmployee_ID.Location = new System.Drawing.Point(122, 41);
            this.lblEmployee_ID.Name = "lblEmployee_ID";
            this.lblEmployee_ID.Size = new System.Drawing.Size(36, 20);
            this.lblEmployee_ID.TabIndex = 10;
            this.lblEmployee_ID.Values.Text = "员工";
            // 
            // cmbEmployee_ID
            // 
            this.cmbEmployee_ID.DropDownWidth = 100;
            this.cmbEmployee_ID.IntegralHeight = false;
            this.cmbEmployee_ID.Location = new System.Drawing.Point(163, 39);
            this.cmbEmployee_ID.Name = "cmbEmployee_ID";
            this.cmbEmployee_ID.Size = new System.Drawing.Size(158, 21);
            this.cmbEmployee_ID.TabIndex = 11;
            // 
            // lblCustomerVendor_ID
            // 
            this.lblCustomerVendor_ID.Location = new System.Drawing.Point(95, 14);
            this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
            this.lblCustomerVendor_ID.Size = new System.Drawing.Size(62, 20);
            this.lblCustomerVendor_ID.TabIndex = 12;
            this.lblCustomerVendor_ID.Values.Text = "往来单位";
            // 
            // cmbCustomerVendor_ID
            // 
            this.cmbCustomerVendor_ID.DropDownWidth = 100;
            this.cmbCustomerVendor_ID.IntegralHeight = false;
            this.cmbCustomerVendor_ID.Location = new System.Drawing.Point(163, 12);
            this.cmbCustomerVendor_ID.Name = "cmbCustomerVendor_ID";
            this.cmbCustomerVendor_ID.Size = new System.Drawing.Size(158, 21);
            this.cmbCustomerVendor_ID.TabIndex = 13;
            // 
            // lblAccount_name
            // 
            this.lblAccount_name.Location = new System.Drawing.Point(96, 96);
            this.lblAccount_name.Name = "lblAccount_name";
            this.lblAccount_name.Size = new System.Drawing.Size(62, 20);
            this.lblAccount_name.TabIndex = 16;
            this.lblAccount_name.Values.Text = "账户名称";
            // 
            // txtAccount_name
            // 
            this.txtAccount_name.Location = new System.Drawing.Point(163, 93);
            this.txtAccount_name.Name = "txtAccount_name";
            this.txtAccount_name.Size = new System.Drawing.Size(256, 23);
            this.txtAccount_name.TabIndex = 17;
            // 
            // lblAccount_No
            // 
            this.lblAccount_No.Location = new System.Drawing.Point(122, 124);
            this.lblAccount_No.Name = "lblAccount_No";
            this.lblAccount_No.Size = new System.Drawing.Size(36, 20);
            this.lblAccount_No.TabIndex = 19;
            this.lblAccount_No.Values.Text = "账号";
            // 
            // txtAccount_No
            // 
            this.txtAccount_No.Location = new System.Drawing.Point(163, 122);
            this.txtAccount_No.Name = "txtAccount_No";
            this.txtAccount_No.Size = new System.Drawing.Size(256, 23);
            this.txtAccount_No.TabIndex = 18;
            // 
            // lblPaymentCodeImagePath
            // 
            this.lblPaymentCodeImagePath.Location = new System.Drawing.Point(107, 204);
            this.lblPaymentCodeImagePath.Name = "lblPaymentCodeImagePath";
            this.lblPaymentCodeImagePath.Size = new System.Drawing.Size(49, 20);
            this.lblPaymentCodeImagePath.TabIndex = 20;
            this.lblPaymentCodeImagePath.Values.Text = "收款码";
            // 
            // lblBelongingBank
            // 
            this.lblBelongingBank.Location = new System.Drawing.Point(96, 153);
            this.lblBelongingBank.Name = "lblBelongingBank";
            this.lblBelongingBank.Size = new System.Drawing.Size(62, 20);
            this.lblBelongingBank.TabIndex = 22;
            this.lblBelongingBank.Values.Text = "所属银行";
            // 
            // txtBelongingBank
            // 
            this.txtBelongingBank.Location = new System.Drawing.Point(163, 151);
            this.txtBelongingBank.Name = "txtBelongingBank";
            this.txtBelongingBank.Size = new System.Drawing.Size(256, 23);
            this.txtBelongingBank.TabIndex = 23;
            // 
            // lblOpeningBank
            // 
            this.lblOpeningBank.Location = new System.Drawing.Point(109, 178);
            this.lblOpeningBank.Name = "lblOpeningBank";
            this.lblOpeningBank.Size = new System.Drawing.Size(49, 20);
            this.lblOpeningBank.TabIndex = 24;
            this.lblOpeningBank.Values.Text = "开户行";
            // 
            // txtOpeningBank
            // 
            this.txtOpeningBank.Location = new System.Drawing.Point(163, 176);
            this.txtOpeningBank.Name = "txtOpeningBank";
            this.txtOpeningBank.Size = new System.Drawing.Size(256, 23);
            this.txtOpeningBank.TabIndex = 25;
            // 
            // lblNotes
            // 
            this.lblNotes.Location = new System.Drawing.Point(118, 387);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(36, 20);
            this.lblNotes.TabIndex = 26;
            this.lblNotes.Values.Text = "备注";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(163, 387);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(256, 48);
            this.txtNotes.TabIndex = 27;
            // 
            // UCFMPayeeInfoEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 536);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCFMPayeeInfoEdit";
            this.Load += new System.EventHandler(this.UCFMPayeeInfoEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicRowImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).EndInit();
            this.kryptonGroupBox1.Panel.ResumeLayout(false);
            this.kryptonGroupBox1.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).EndInit();
            this.kryptonGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupboxIsDefault.Panel)).EndInit();
            this.groupboxIsDefault.Panel.ResumeLayout(false);
            this.groupboxIsDefault.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupboxIsDefault)).EndInit();
            this.groupboxIsDefault.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbAccount_type)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmployee_ID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCustomerVendor_ID)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
        private Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;
        private Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
        private Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;
        private Krypton.Toolkit.KryptonLabel lblAccount_name;
        private Krypton.Toolkit.KryptonTextBox txtAccount_name;
        private Krypton.Toolkit.KryptonLabel lblAccount_No;
        private Krypton.Toolkit.KryptonTextBox txtAccount_No;
        private Krypton.Toolkit.KryptonLabel lblPaymentCodeImagePath;
        private Krypton.Toolkit.KryptonLabel lblBelongingBank;
        private Krypton.Toolkit.KryptonTextBox txtBelongingBank;
        private Krypton.Toolkit.KryptonLabel lblOpeningBank;
        private Krypton.Toolkit.KryptonTextBox txtOpeningBank;
        private Krypton.Toolkit.KryptonLabel lblNotes;
        private Krypton.Toolkit.KryptonTextBox txtNotes;
        private Krypton.Toolkit.KryptonComboBox cmbAccount_type;
        private Krypton.Toolkit.KryptonLabel lblaccount_type;
        private Krypton.Toolkit.KryptonGroupBox groupboxIsDefault;
        private Krypton.Toolkit.KryptonRadioButton rdbIsDefaultNo;
        private Krypton.Toolkit.KryptonRadioButton rdbIsDefaultYes;
        private Krypton.Toolkit.KryptonLabel lblIsDefault;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBox1;
        private Krypton.Toolkit.KryptonRadioButton rdbis_enabledNo;
        private Krypton.Toolkit.KryptonRadioButton rdbis_enabledYes;
        private Krypton.Toolkit.KryptonLabel lblIs_enabled;
        private RUINOR.WinFormsUI.CustomPictureBox.MagicPictureBox PicRowImage;
    }
}
