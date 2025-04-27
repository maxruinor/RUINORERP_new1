namespace RUINORERP.UI.BI
{
    partial class UCCustomerVendorEdit
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
            this.lblCustomerCreditLimit = new Krypton.Toolkit.KryptonLabel();
            this.txtCustomerCreditLimit = new Krypton.Toolkit.KryptonTextBox();
            this.lblCustomerCreditDays = new Krypton.Toolkit.KryptonLabel();
            this.txtCustomerCreditDays = new Krypton.Toolkit.KryptonTextBox();
            this.btnAddBillingInformation = new Krypton.Toolkit.KryptonButton();
            this.chkNoNeedSource = new Krypton.Toolkit.KryptonCheckBox();
            this.lblCustomer_id = new Krypton.Toolkit.KryptonLabel();
            this.cmbCustomer_id = new Krypton.Toolkit.KryptonComboBox();
            this.btnAddPayeeInfo = new Krypton.Toolkit.KryptonButton();
            this.chkOther = new Krypton.Toolkit.KryptonCheckBox();
            this.kryptonLabel3 = new Krypton.Toolkit.KryptonLabel();
            this.kryptonGroupBox1 = new Krypton.Toolkit.KryptonGroupBox();
            this.rdbis_enabledNo = new Krypton.Toolkit.KryptonRadioButton();
            this.rdbis_enabledYes = new Krypton.Toolkit.KryptonRadioButton();
            this.lblIs_enabled = new Krypton.Toolkit.KryptonLabel();
            this.chk责任人专属 = new Krypton.Toolkit.KryptonCheckBox();
            this.lbl责任人专属 = new Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel2 = new Krypton.Toolkit.KryptonLabel();
            this.txtShortName = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            this.txtCVCode = new Krypton.Toolkit.KryptonTextBox();
            this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
            this.cmbEmployee_ID = new Krypton.Toolkit.KryptonComboBox();
            this.txtIsVendor = new Krypton.Toolkit.KryptonCheckBox();
            this.txtIsCustomer = new Krypton.Toolkit.KryptonCheckBox();
            this.txtType_ID = new Krypton.Toolkit.KryptonComboBox();
            this.lblType_ID = new Krypton.Toolkit.KryptonLabel();
            this.lblCVName = new Krypton.Toolkit.KryptonLabel();
            this.txtCVName = new Krypton.Toolkit.KryptonTextBox();
            this.lblContact = new Krypton.Toolkit.KryptonLabel();
            this.txtContact = new Krypton.Toolkit.KryptonTextBox();
            this.lblPhone = new Krypton.Toolkit.KryptonLabel();
            this.txtPhone = new Krypton.Toolkit.KryptonTextBox();
            this.lblAddress = new Krypton.Toolkit.KryptonLabel();
            this.txtAddress = new Krypton.Toolkit.KryptonTextBox();
            this.lblWebsite = new Krypton.Toolkit.KryptonLabel();
            this.txtWebsite = new Krypton.Toolkit.KryptonTextBox();
            this.lblIsCustomer = new Krypton.Toolkit.KryptonLabel();
            this.lblIsVendor = new Krypton.Toolkit.KryptonLabel();
            this.lblNotes = new Krypton.Toolkit.KryptonLabel();
            this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
            this.lblSupplierCreditLimit = new Krypton.Toolkit.KryptonLabel();
            this.txtSupplierCreditLimit = new Krypton.Toolkit.KryptonTextBox();
            this.lblSupplierCreditDays = new Krypton.Toolkit.KryptonLabel();
            this.txtSupplierCreditDays = new Krypton.Toolkit.KryptonTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCustomer_id)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).BeginInit();
            this.kryptonGroupBox1.Panel.SuspendLayout();
            this.kryptonGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmployee_ID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtType_ID)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(212, 606);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(330, 606);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.lblSupplierCreditLimit);
            this.kryptonPanel1.Controls.Add(this.txtSupplierCreditLimit);
            this.kryptonPanel1.Controls.Add(this.lblSupplierCreditDays);
            this.kryptonPanel1.Controls.Add(this.txtSupplierCreditDays);
            this.kryptonPanel1.Controls.Add(this.lblCustomerCreditLimit);
            this.kryptonPanel1.Controls.Add(this.txtCustomerCreditLimit);
            this.kryptonPanel1.Controls.Add(this.lblCustomerCreditDays);
            this.kryptonPanel1.Controls.Add(this.txtCustomerCreditDays);
            this.kryptonPanel1.Controls.Add(this.btnAddBillingInformation);
            this.kryptonPanel1.Controls.Add(this.chkNoNeedSource);
            this.kryptonPanel1.Controls.Add(this.lblCustomer_id);
            this.kryptonPanel1.Controls.Add(this.cmbCustomer_id);
            this.kryptonPanel1.Controls.Add(this.btnAddPayeeInfo);
            this.kryptonPanel1.Controls.Add(this.chkOther);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel3);
            this.kryptonPanel1.Controls.Add(this.kryptonGroupBox1);
            this.kryptonPanel1.Controls.Add(this.lblIs_enabled);
            this.kryptonPanel1.Controls.Add(this.chk责任人专属);
            this.kryptonPanel1.Controls.Add(this.lbl责任人专属);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel2);
            this.kryptonPanel1.Controls.Add(this.txtShortName);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel1);
            this.kryptonPanel1.Controls.Add(this.txtCVCode);
            this.kryptonPanel1.Controls.Add(this.lblEmployee_ID);
            this.kryptonPanel1.Controls.Add(this.cmbEmployee_ID);
            this.kryptonPanel1.Controls.Add(this.txtIsVendor);
            this.kryptonPanel1.Controls.Add(this.txtIsCustomer);
            this.kryptonPanel1.Controls.Add(this.txtType_ID);
            this.kryptonPanel1.Controls.Add(this.lblType_ID);
            this.kryptonPanel1.Controls.Add(this.lblCVName);
            this.kryptonPanel1.Controls.Add(this.txtCVName);
            this.kryptonPanel1.Controls.Add(this.lblContact);
            this.kryptonPanel1.Controls.Add(this.txtContact);
            this.kryptonPanel1.Controls.Add(this.lblPhone);
            this.kryptonPanel1.Controls.Add(this.txtPhone);
            this.kryptonPanel1.Controls.Add(this.lblAddress);
            this.kryptonPanel1.Controls.Add(this.txtAddress);
            this.kryptonPanel1.Controls.Add(this.lblWebsite);
            this.kryptonPanel1.Controls.Add(this.txtWebsite);
            this.kryptonPanel1.Controls.Add(this.lblIsCustomer);
            this.kryptonPanel1.Controls.Add(this.lblIsVendor);
            this.kryptonPanel1.Controls.Add(this.lblNotes);
            this.kryptonPanel1.Controls.Add(this.txtNotes);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.PaletteMode = Krypton.Toolkit.PaletteMode.Office2007Blue;
            this.kryptonPanel1.Size = new System.Drawing.Size(731, 657);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // lblCustomerCreditLimit
            // 
            this.lblCustomerCreditLimit.Location = new System.Drawing.Point(104, 440);
            this.lblCustomerCreditLimit.Name = "lblCustomerCreditLimit";
            this.lblCustomerCreditLimit.Size = new System.Drawing.Size(62, 20);
            this.lblCustomerCreditLimit.TabIndex = 84;
            this.lblCustomerCreditLimit.Values.Text = "信用额度";
            // 
            // txtCustomerCreditLimit
            // 
            this.txtCustomerCreditLimit.Location = new System.Drawing.Point(177, 436);
            this.txtCustomerCreditLimit.Name = "txtCustomerCreditLimit";
            this.txtCustomerCreditLimit.Size = new System.Drawing.Size(100, 23);
            this.txtCustomerCreditLimit.TabIndex = 85;
            // 
            // lblCustomerCreditDays
            // 
            this.lblCustomerCreditDays.Location = new System.Drawing.Point(358, 441);
            this.lblCustomerCreditDays.Name = "lblCustomerCreditDays";
            this.lblCustomerCreditDays.Size = new System.Drawing.Size(62, 20);
            this.lblCustomerCreditDays.TabIndex = 86;
            this.lblCustomerCreditDays.Values.Text = "账期天数";
            // 
            // txtCustomerCreditDays
            // 
            this.txtCustomerCreditDays.Location = new System.Drawing.Point(431, 437);
            this.txtCustomerCreditDays.Name = "txtCustomerCreditDays";
            this.txtCustomerCreditDays.Size = new System.Drawing.Size(100, 23);
            this.txtCustomerCreditDays.TabIndex = 87;
            // 
            // btnAddBillingInformation
            // 
            this.btnAddBillingInformation.Location = new System.Drawing.Point(594, 606);
            this.btnAddBillingInformation.Name = "btnAddBillingInformation";
            this.btnAddBillingInformation.Size = new System.Drawing.Size(90, 25);
            this.btnAddBillingInformation.TabIndex = 83;
            this.btnAddBillingInformation.Values.Text = "添加开票信息";
            this.btnAddBillingInformation.Click += new System.EventHandler(this.btnAddBillingInformation_Click);
            // 
            // chkNoNeedSource
            // 
            this.chkNoNeedSource.Location = new System.Drawing.Point(542, 53);
            this.chkNoNeedSource.Name = "chkNoNeedSource";
            this.chkNoNeedSource.Size = new System.Drawing.Size(62, 20);
            this.chkNoNeedSource.TabIndex = 82;
            this.chkNoNeedSource.Values.Text = "无来源";
            this.chkNoNeedSource.CheckedChanged += new System.EventHandler(this.chkNoNeedSource_CheckedChanged);
            // 
            // lblCustomer_id
            // 
            this.lblCustomer_id.Location = new System.Drawing.Point(196, 50);
            this.lblCustomer_id.Name = "lblCustomer_id";
            this.lblCustomer_id.Size = new System.Drawing.Size(62, 20);
            this.lblCustomer_id.TabIndex = 80;
            this.lblCustomer_id.Values.Text = "客户来源";
            // 
            // cmbCustomer_id
            // 
            this.cmbCustomer_id.DropDownWidth = 100;
            this.cmbCustomer_id.IntegralHeight = false;
            this.cmbCustomer_id.Location = new System.Drawing.Point(262, 49);
            this.cmbCustomer_id.Name = "cmbCustomer_id";
            this.cmbCustomer_id.Size = new System.Drawing.Size(274, 21);
            this.cmbCustomer_id.TabIndex = 81;
            // 
            // btnAddPayeeInfo
            // 
            this.btnAddPayeeInfo.Location = new System.Drawing.Point(475, 606);
            this.btnAddPayeeInfo.Name = "btnAddPayeeInfo";
            this.btnAddPayeeInfo.Size = new System.Drawing.Size(90, 25);
            this.btnAddPayeeInfo.TabIndex = 79;
            this.btnAddPayeeInfo.Values.Text = "添加收款信息";
            this.btnAddPayeeInfo.Click += new System.EventHandler(this.btnAddPayeeInfo_Click);
            // 
            // chkOther
            // 
            this.chkOther.Location = new System.Drawing.Point(454, 222);
            this.chkOther.Name = "chkOther";
            this.chkOther.Size = new System.Drawing.Size(19, 13);
            this.chkOther.TabIndex = 70;
            this.chkOther.Values.Text = "";
            // 
            // kryptonLabel3
            // 
            this.kryptonLabel3.Location = new System.Drawing.Point(375, 219);
            this.kryptonLabel3.Name = "kryptonLabel3";
            this.kryptonLabel3.Size = new System.Drawing.Size(75, 20);
            this.kryptonLabel3.TabIndex = 69;
            this.kryptonLabel3.Values.Text = "是否为其他";
            // 
            // kryptonGroupBox1
            // 
            this.kryptonGroupBox1.CaptionVisible = false;
            this.kryptonGroupBox1.Location = new System.Drawing.Point(177, 534);
            this.kryptonGroupBox1.Name = "kryptonGroupBox1";
            // 
            // kryptonGroupBox1.Panel
            // 
            this.kryptonGroupBox1.Panel.Controls.Add(this.rdbis_enabledNo);
            this.kryptonGroupBox1.Panel.Controls.Add(this.rdbis_enabledYes);
            this.kryptonGroupBox1.Size = new System.Drawing.Size(105, 40);
            this.kryptonGroupBox1.TabIndex = 67;
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
            // lblIs_enabled
            // 
            this.lblIs_enabled.Location = new System.Drawing.Point(111, 544);
            this.lblIs_enabled.Name = "lblIs_enabled";
            this.lblIs_enabled.Size = new System.Drawing.Size(62, 20);
            this.lblIs_enabled.TabIndex = 65;
            this.lblIs_enabled.Values.Text = "是否启用";
            // 
            // chk责任人专属
            // 
            this.chk责任人专属.Location = new System.Drawing.Point(454, 187);
            this.chk责任人专属.Name = "chk责任人专属";
            this.chk责任人专属.Size = new System.Drawing.Size(19, 13);
            this.chk责任人专属.TabIndex = 43;
            this.chk责任人专属.Values.Text = "";
            // 
            // lbl责任人专属
            // 
            this.lbl责任人专属.Location = new System.Drawing.Point(377, 183);
            this.lbl责任人专属.Name = "lbl责任人专属";
            this.lbl责任人专属.PaletteMode = Krypton.Toolkit.PaletteMode.Office2007Blue;
            this.lbl责任人专属.Size = new System.Drawing.Size(75, 20);
            this.lbl责任人专属.TabIndex = 42;
            this.lbl责任人专属.Values.Text = "责任人专属";
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(137, 153);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.PaletteMode = Krypton.Toolkit.PaletteMode.Office2007Blue;
            this.kryptonLabel2.Size = new System.Drawing.Size(36, 20);
            this.kryptonLabel2.TabIndex = 40;
            this.kryptonLabel2.Values.Text = "简称";
            // 
            // txtShortName
            // 
            this.txtShortName.Location = new System.Drawing.Point(177, 153);
            this.txtShortName.Name = "txtShortName";
            this.txtShortName.Size = new System.Drawing.Size(355, 23);
            this.txtShortName.TabIndex = 41;
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(137, 12);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.PaletteMode = Krypton.Toolkit.PaletteMode.Office2007Blue;
            this.kryptonLabel1.Size = new System.Drawing.Size(36, 20);
            this.kryptonLabel1.TabIndex = 38;
            this.kryptonLabel1.Values.Text = "编号";
            // 
            // txtCVCode
            // 
            this.txtCVCode.Location = new System.Drawing.Point(177, 12);
            this.txtCVCode.Name = "txtCVCode";
            this.txtCVCode.Size = new System.Drawing.Size(355, 23);
            this.txtCVCode.TabIndex = 39;
            // 
            // lblEmployee_ID
            // 
            this.lblEmployee_ID.Location = new System.Drawing.Point(124, 183);
            this.lblEmployee_ID.Name = "lblEmployee_ID";
            this.lblEmployee_ID.PaletteMode = Krypton.Toolkit.PaletteMode.Office2007Blue;
            this.lblEmployee_ID.Size = new System.Drawing.Size(49, 20);
            this.lblEmployee_ID.TabIndex = 36;
            this.lblEmployee_ID.Values.Text = "责任人";
            // 
            // cmbEmployee_ID
            // 
            this.cmbEmployee_ID.DropDownWidth = 100;
            this.cmbEmployee_ID.IntegralHeight = false;
            this.cmbEmployee_ID.Location = new System.Drawing.Point(177, 183);
            this.cmbEmployee_ID.Name = "cmbEmployee_ID";
            this.cmbEmployee_ID.Size = new System.Drawing.Size(158, 21);
            this.cmbEmployee_ID.TabIndex = 37;
            // 
            // txtIsVendor
            // 
            this.txtIsVendor.Location = new System.Drawing.Point(177, 219);
            this.txtIsVendor.Name = "txtIsVendor";
            this.txtIsVendor.Size = new System.Drawing.Size(19, 13);
            this.txtIsVendor.TabIndex = 35;
            this.txtIsVendor.Values.Text = "";
            // 
            // txtIsCustomer
            // 
            this.txtIsCustomer.Location = new System.Drawing.Point(177, 53);
            this.txtIsCustomer.Name = "txtIsCustomer";
            this.txtIsCustomer.Size = new System.Drawing.Size(19, 13);
            this.txtIsCustomer.TabIndex = 34;
            this.txtIsCustomer.Values.Text = "";
            this.txtIsCustomer.CheckedChanged += new System.EventHandler(this.txtIsCustomer_CheckedChanged);
            // 
            // txtType_ID
            // 
            this.txtType_ID.DropDownWidth = 296;
            this.txtType_ID.IntegralHeight = false;
            this.txtType_ID.Location = new System.Drawing.Point(177, 91);
            this.txtType_ID.Name = "txtType_ID";
            this.txtType_ID.Size = new System.Drawing.Size(355, 21);
            this.txtType_ID.TabIndex = 33;
            // 
            // lblType_ID
            // 
            this.lblType_ID.Location = new System.Drawing.Point(111, 91);
            this.lblType_ID.Name = "lblType_ID";
            this.lblType_ID.PaletteMode = Krypton.Toolkit.PaletteMode.Office2007Blue;
            this.lblType_ID.Size = new System.Drawing.Size(62, 20);
            this.lblType_ID.TabIndex = 15;
            this.lblType_ID.Values.Text = "类型等级";
            // 
            // lblCVName
            // 
            this.lblCVName.Location = new System.Drawing.Point(137, 122);
            this.lblCVName.Name = "lblCVName";
            this.lblCVName.PaletteMode = Krypton.Toolkit.PaletteMode.Office2007Blue;
            this.lblCVName.Size = new System.Drawing.Size(36, 20);
            this.lblCVName.TabIndex = 17;
            this.lblCVName.Values.Text = "全称";
            // 
            // txtCVName
            // 
            this.txtCVName.Location = new System.Drawing.Point(177, 122);
            this.txtCVName.Name = "txtCVName";
            this.txtCVName.Size = new System.Drawing.Size(355, 23);
            this.txtCVName.TabIndex = 18;
            // 
            // lblContact
            // 
            this.lblContact.Location = new System.Drawing.Point(124, 251);
            this.lblContact.Name = "lblContact";
            this.lblContact.PaletteMode = Krypton.Toolkit.PaletteMode.Office2007Blue;
            this.lblContact.Size = new System.Drawing.Size(49, 20);
            this.lblContact.TabIndex = 19;
            this.lblContact.Values.Text = "联系人";
            // 
            // txtContact
            // 
            this.txtContact.Location = new System.Drawing.Point(177, 247);
            this.txtContact.Name = "txtContact";
            this.txtContact.Size = new System.Drawing.Size(355, 23);
            this.txtContact.TabIndex = 20;
            // 
            // lblPhone
            // 
            this.lblPhone.Location = new System.Drawing.Point(137, 276);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.PaletteMode = Krypton.Toolkit.PaletteMode.Office2007Blue;
            this.lblPhone.Size = new System.Drawing.Size(36, 20);
            this.lblPhone.TabIndex = 21;
            this.lblPhone.Values.Text = "电话";
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(177, 272);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(355, 23);
            this.txtPhone.TabIndex = 22;
            // 
            // lblAddress
            // 
            this.lblAddress.Location = new System.Drawing.Point(137, 301);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.PaletteMode = Krypton.Toolkit.PaletteMode.Office2007Blue;
            this.lblAddress.Size = new System.Drawing.Size(36, 20);
            this.lblAddress.TabIndex = 23;
            this.lblAddress.Values.Text = "地址";
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(177, 297);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(355, 23);
            this.txtAddress.TabIndex = 24;
            // 
            // lblWebsite
            // 
            this.lblWebsite.Location = new System.Drawing.Point(137, 326);
            this.lblWebsite.Name = "lblWebsite";
            this.lblWebsite.PaletteMode = Krypton.Toolkit.PaletteMode.Office2007Blue;
            this.lblWebsite.Size = new System.Drawing.Size(36, 20);
            this.lblWebsite.TabIndex = 25;
            this.lblWebsite.Values.Text = "网址";
            // 
            // txtWebsite
            // 
            this.txtWebsite.Location = new System.Drawing.Point(177, 322);
            this.txtWebsite.Name = "txtWebsite";
            this.txtWebsite.Size = new System.Drawing.Size(355, 23);
            this.txtWebsite.TabIndex = 26;
            // 
            // lblIsCustomer
            // 
            this.lblIsCustomer.Location = new System.Drawing.Point(98, 49);
            this.lblIsCustomer.Name = "lblIsCustomer";
            this.lblIsCustomer.PaletteMode = Krypton.Toolkit.PaletteMode.Office2007Blue;
            this.lblIsCustomer.Size = new System.Drawing.Size(75, 20);
            this.lblIsCustomer.TabIndex = 28;
            this.lblIsCustomer.Values.Text = "是否为客户";
            // 
            // lblIsVendor
            // 
            this.lblIsVendor.Location = new System.Drawing.Point(85, 215);
            this.lblIsVendor.Name = "lblIsVendor";
            this.lblIsVendor.Size = new System.Drawing.Size(88, 20);
            this.lblIsVendor.TabIndex = 29;
            this.lblIsVendor.Values.Text = "是否为供应商";
            // 
            // lblNotes
            // 
            this.lblNotes.Location = new System.Drawing.Point(137, 351);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.PaletteMode = Krypton.Toolkit.PaletteMode.Office2007Blue;
            this.lblNotes.Size = new System.Drawing.Size(36, 20);
            this.lblNotes.TabIndex = 31;
            this.lblNotes.Values.Text = "备注";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(177, 351);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(355, 72);
            this.txtNotes.TabIndex = 32;
            // 
            // lblSupplierCreditLimit
            // 
            this.lblSupplierCreditLimit.Location = new System.Drawing.Point(104, 469);
            this.lblSupplierCreditLimit.Name = "lblSupplierCreditLimit";
            this.lblSupplierCreditLimit.Size = new System.Drawing.Size(62, 20);
            this.lblSupplierCreditLimit.TabIndex = 88;
            this.lblSupplierCreditLimit.Values.Text = "信用额度";
            // 
            // txtSupplierCreditLimit
            // 
            this.txtSupplierCreditLimit.Location = new System.Drawing.Point(177, 465);
            this.txtSupplierCreditLimit.Name = "txtSupplierCreditLimit";
            this.txtSupplierCreditLimit.Size = new System.Drawing.Size(100, 23);
            this.txtSupplierCreditLimit.TabIndex = 89;
            // 
            // lblSupplierCreditDays
            // 
            this.lblSupplierCreditDays.Location = new System.Drawing.Point(358, 473);
            this.lblSupplierCreditDays.Name = "lblSupplierCreditDays";
            this.lblSupplierCreditDays.Size = new System.Drawing.Size(62, 20);
            this.lblSupplierCreditDays.TabIndex = 90;
            this.lblSupplierCreditDays.Values.Text = "账期天数";
            // 
            // txtSupplierCreditDays
            // 
            this.txtSupplierCreditDays.Location = new System.Drawing.Point(431, 469);
            this.txtSupplierCreditDays.Name = "txtSupplierCreditDays";
            this.txtSupplierCreditDays.Size = new System.Drawing.Size(100, 23);
            this.txtSupplierCreditDays.TabIndex = 91;
            // 
            // UCCustomerVendorEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(731, 657);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCCustomerVendorEdit";
            this.Load += new System.EventHandler(this.UCCustomerVendorEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCustomer_id)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).EndInit();
            this.kryptonGroupBox1.Panel.ResumeLayout(false);
            this.kryptonGroupBox1.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).EndInit();
            this.kryptonGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmployee_ID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtType_ID)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonLabel lblType_ID;
        private Krypton.Toolkit.KryptonLabel lblCVName;
        private Krypton.Toolkit.KryptonTextBox txtCVName;
        private Krypton.Toolkit.KryptonLabel lblContact;
        private Krypton.Toolkit.KryptonTextBox txtContact;
        private Krypton.Toolkit.KryptonLabel lblPhone;
        private Krypton.Toolkit.KryptonTextBox txtPhone;
        private Krypton.Toolkit.KryptonLabel lblAddress;
        private Krypton.Toolkit.KryptonTextBox txtAddress;
        private Krypton.Toolkit.KryptonLabel lblWebsite;
        private Krypton.Toolkit.KryptonTextBox txtWebsite;
        private Krypton.Toolkit.KryptonLabel lblIsCustomer;
        private Krypton.Toolkit.KryptonLabel lblIsVendor;
        private Krypton.Toolkit.KryptonLabel lblNotes;
        private Krypton.Toolkit.KryptonTextBox txtNotes;
        private Krypton.Toolkit.KryptonComboBox txtType_ID;
        private Krypton.Toolkit.KryptonCheckBox txtIsVendor;
        private Krypton.Toolkit.KryptonCheckBox txtIsCustomer;
        private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
        private Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private Krypton.Toolkit.KryptonTextBox txtCVCode;
        private Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private Krypton.Toolkit.KryptonTextBox txtShortName;
        private Krypton.Toolkit.KryptonCheckBox chk责任人专属;
        private Krypton.Toolkit.KryptonLabel lbl责任人专属;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBox1;
        private Krypton.Toolkit.KryptonRadioButton rdbis_enabledNo;
        private Krypton.Toolkit.KryptonRadioButton rdbis_enabledYes;
        private Krypton.Toolkit.KryptonLabel lblIs_enabled;
        private Krypton.Toolkit.KryptonCheckBox chkOther;
        private Krypton.Toolkit.KryptonLabel kryptonLabel3;
        private Krypton.Toolkit.KryptonButton btnAddPayeeInfo;
        private Krypton.Toolkit.KryptonLabel lblCustomer_id;
        private Krypton.Toolkit.KryptonComboBox cmbCustomer_id;
        private Krypton.Toolkit.KryptonCheckBox chkNoNeedSource;
        private Krypton.Toolkit.KryptonButton btnAddBillingInformation;
        private Krypton.Toolkit.KryptonLabel lblCustomerCreditLimit;
        private Krypton.Toolkit.KryptonTextBox txtCustomerCreditLimit;
        private Krypton.Toolkit.KryptonLabel lblCustomerCreditDays;
        private Krypton.Toolkit.KryptonTextBox txtCustomerCreditDays;
        private Krypton.Toolkit.KryptonLabel lblSupplierCreditLimit;
        private Krypton.Toolkit.KryptonTextBox txtSupplierCreditLimit;
        private Krypton.Toolkit.KryptonLabel lblSupplierCreditDays;
        private Krypton.Toolkit.KryptonTextBox txtSupplierCreditDays;
    }
}
