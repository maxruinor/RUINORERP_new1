namespace RUINORERP.Server.Controls
{
    partial class RegistrationManagementControl
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
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            tsbtnSaveRegInfo = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            tsbtnCreateRegInfo = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            tsbtnRenewRegInfo = new System.Windows.Forms.ToolStripButton();
            groupBox1 = new System.Windows.Forms.GroupBox();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            lblCompanyName = new System.Windows.Forms.Label();
            txtCompanyName = new System.Windows.Forms.TextBox();
            lblContactName = new System.Windows.Forms.Label();
            txtContactName = new System.Windows.Forms.TextBox();
            lblPhoneNumber = new System.Windows.Forms.Label();
            txtPhoneNumber = new System.Windows.Forms.TextBox();
            lblLicenseType = new System.Windows.Forms.Label();
            cmbLicenseType = new System.Windows.Forms.ComboBox();
            lblConcurrentUsers = new System.Windows.Forms.Label();
            txtConcurrentUsers = new System.Windows.Forms.TextBox();
            lblProductVersion = new System.Windows.Forms.Label();
            txtProductVersion = new System.Windows.Forms.TextBox();
            lblMachineCode = new System.Windows.Forms.Label();
            txtMachineCode = new System.Windows.Forms.TextBox();
            chkIsRegistered = new System.Windows.Forms.CheckBox();
            lblRegistrationCode = new System.Windows.Forms.Label();
            txtRegistrationCode = new System.Windows.Forms.TextBox();
            btnGenerateMachineCode = new System.Windows.Forms.Button();
            lblRemarks = new System.Windows.Forms.Label();
            txtRemarks = new System.Windows.Forms.TextBox();
            lblDays = new System.Windows.Forms.Label();
            cmbDays = new System.Windows.Forms.ComboBox();
            dtpRegistrationDate = new System.Windows.Forms.DateTimePicker();
            dtpExpirationDate = new System.Windows.Forms.DateTimePicker();
            dtpPurchaseDate = new System.Windows.Forms.DateTimePicker();
            lblPurchaseDate = new System.Windows.Forms.Label();
            lblRegistrationDate = new System.Windows.Forms.Label();
            lblExpirationDate = new System.Windows.Forms.Label();
            groupBox2 = new System.Windows.Forms.GroupBox();
            checkedListBoxMod = new System.Windows.Forms.CheckedListBox();
            toolStrip1.SuspendLayout();
            groupBox1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tsbtnSaveRegInfo, toolStripSeparator1, tsbtnCreateRegInfo, toolStripSeparator2, tsbtnRenewRegInfo });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(933, 25);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // tsbtnSaveRegInfo
            // 
            tsbtnSaveRegInfo.Image = Properties.Resources.save;
            tsbtnSaveRegInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbtnSaveRegInfo.Name = "tsbtnSaveRegInfo";
            tsbtnSaveRegInfo.Size = new System.Drawing.Size(52, 22);
            tsbtnSaveRegInfo.Text = "保存";
            tsbtnSaveRegInfo.Click += tsbtnSaveRegInfo_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbtnCreateRegInfo
            // 
            tsbtnCreateRegInfo.Image = Properties.Resources.add;
            tsbtnCreateRegInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbtnCreateRegInfo.Name = "tsbtnCreateRegInfo";
            tsbtnCreateRegInfo.Size = new System.Drawing.Size(76, 22);
            tsbtnCreateRegInfo.Text = "生成注册";
            tsbtnCreateRegInfo.Click += btnCreateRegInfo_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbtnRenewRegInfo
            // 
            tsbtnRenewRegInfo.Image = Properties.Resources.foward2;
            tsbtnRenewRegInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbtnRenewRegInfo.Name = "tsbtnRenewRegInfo";
            tsbtnRenewRegInfo.Size = new System.Drawing.Size(52, 22);
            tsbtnRenewRegInfo.Text = "续期";
            tsbtnRenewRegInfo.Click += btnRenewRegInfo_Click;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            groupBox1.Controls.Add(tableLayoutPanel1);
            groupBox1.Location = new System.Drawing.Point(4, 40);
            groupBox1.Margin = new System.Windows.Forms.Padding(4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4);
            groupBox1.Size = new System.Drawing.Size(926, 397);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "基本信息";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 117F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 117F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(lblCompanyName, 0, 0);
            tableLayoutPanel1.Controls.Add(txtCompanyName, 1, 0);
            tableLayoutPanel1.Controls.Add(lblContactName, 2, 0);
            tableLayoutPanel1.Controls.Add(txtContactName, 3, 0);
            tableLayoutPanel1.Controls.Add(lblPhoneNumber, 0, 1);
            tableLayoutPanel1.Controls.Add(txtPhoneNumber, 1, 1);
            tableLayoutPanel1.Controls.Add(lblLicenseType, 2, 1);
            tableLayoutPanel1.Controls.Add(cmbLicenseType, 3, 1);
            tableLayoutPanel1.Controls.Add(lblConcurrentUsers, 2, 2);
            tableLayoutPanel1.Controls.Add(txtConcurrentUsers, 3, 2);
            tableLayoutPanel1.Controls.Add(lblProductVersion, 0, 3);
            tableLayoutPanel1.Controls.Add(txtProductVersion, 1, 3);
            tableLayoutPanel1.Controls.Add(lblMachineCode, 2, 3);
            tableLayoutPanel1.Controls.Add(txtMachineCode, 3, 3);
            tableLayoutPanel1.Controls.Add(chkIsRegistered, 0, 5);
            tableLayoutPanel1.Controls.Add(lblRegistrationCode, 0, 6);
            tableLayoutPanel1.Controls.Add(txtRegistrationCode, 1, 6);
            tableLayoutPanel1.Controls.Add(btnGenerateMachineCode, 3, 6);
            tableLayoutPanel1.Controls.Add(lblRemarks, 0, 7);
            tableLayoutPanel1.Controls.Add(txtRemarks, 1, 7);
            tableLayoutPanel1.Controls.Add(lblDays, 2, 7);
            tableLayoutPanel1.Controls.Add(cmbDays, 3, 7);
            tableLayoutPanel1.Controls.Add(dtpRegistrationDate, 1, 4);
            tableLayoutPanel1.Controls.Add(dtpExpirationDate, 3, 4);
            tableLayoutPanel1.Controls.Add(dtpPurchaseDate, 1, 2);
            tableLayoutPanel1.Controls.Add(lblPurchaseDate, 0, 2);
            tableLayoutPanel1.Controls.Add(lblRegistrationDate, 0, 4);
            tableLayoutPanel1.Controls.Add(lblExpirationDate, 2, 4);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(4, 20);
            tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 8;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            tableLayoutPanel1.Size = new System.Drawing.Size(918, 373);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // lblCompanyName
            // 
            lblCompanyName.AutoSize = true;
            lblCompanyName.Dock = System.Windows.Forms.DockStyle.Fill;
            lblCompanyName.Location = new System.Drawing.Point(4, 0);
            lblCompanyName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblCompanyName.Name = "lblCompanyName";
            lblCompanyName.Size = new System.Drawing.Size(109, 42);
            lblCompanyName.TabIndex = 0;
            lblCompanyName.Text = "公司名称:";
            lblCompanyName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCompanyName
            // 
            txtCompanyName.Dock = System.Windows.Forms.DockStyle.Fill;
            txtCompanyName.Location = new System.Drawing.Point(121, 4);
            txtCompanyName.Margin = new System.Windows.Forms.Padding(4);
            txtCompanyName.Name = "txtCompanyName";
            txtCompanyName.Size = new System.Drawing.Size(334, 23);
            txtCompanyName.TabIndex = 1;
            // 
            // lblContactName
            // 
            lblContactName.AutoSize = true;
            lblContactName.Dock = System.Windows.Forms.DockStyle.Fill;
            lblContactName.Location = new System.Drawing.Point(463, 0);
            lblContactName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblContactName.Name = "lblContactName";
            lblContactName.Size = new System.Drawing.Size(109, 42);
            lblContactName.TabIndex = 2;
            lblContactName.Text = "联系人:";
            lblContactName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtContactName
            // 
            txtContactName.Dock = System.Windows.Forms.DockStyle.Fill;
            txtContactName.Location = new System.Drawing.Point(580, 4);
            txtContactName.Margin = new System.Windows.Forms.Padding(4);
            txtContactName.Name = "txtContactName";
            txtContactName.Size = new System.Drawing.Size(334, 23);
            txtContactName.TabIndex = 3;
            // 
            // lblPhoneNumber
            // 
            lblPhoneNumber.AutoSize = true;
            lblPhoneNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            lblPhoneNumber.Location = new System.Drawing.Point(4, 42);
            lblPhoneNumber.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblPhoneNumber.Name = "lblPhoneNumber";
            lblPhoneNumber.Size = new System.Drawing.Size(109, 42);
            lblPhoneNumber.TabIndex = 4;
            lblPhoneNumber.Text = "电话:";
            lblPhoneNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPhoneNumber
            // 
            txtPhoneNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            txtPhoneNumber.Location = new System.Drawing.Point(121, 46);
            txtPhoneNumber.Margin = new System.Windows.Forms.Padding(4);
            txtPhoneNumber.Name = "txtPhoneNumber";
            txtPhoneNumber.Size = new System.Drawing.Size(334, 23);
            txtPhoneNumber.TabIndex = 5;
            // 
            // lblLicenseType
            // 
            lblLicenseType.AutoSize = true;
            lblLicenseType.Dock = System.Windows.Forms.DockStyle.Fill;
            lblLicenseType.Location = new System.Drawing.Point(463, 42);
            lblLicenseType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblLicenseType.Name = "lblLicenseType";
            lblLicenseType.Size = new System.Drawing.Size(109, 42);
            lblLicenseType.TabIndex = 6;
            lblLicenseType.Text = "授权类型:";
            lblLicenseType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbLicenseType
            // 
            cmbLicenseType.Dock = System.Windows.Forms.DockStyle.Fill;
            cmbLicenseType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbLicenseType.FormattingEnabled = true;
            cmbLicenseType.Items.AddRange(new object[] { "试用版", "正式版" });
            cmbLicenseType.Location = new System.Drawing.Point(580, 46);
            cmbLicenseType.Margin = new System.Windows.Forms.Padding(4);
            cmbLicenseType.Name = "cmbLicenseType";
            cmbLicenseType.Size = new System.Drawing.Size(334, 25);
            cmbLicenseType.TabIndex = 7;
            cmbLicenseType.SelectedIndexChanged += cmbLicenseType_SelectedIndexChanged;
            // 
            // lblConcurrentUsers
            // 
            lblConcurrentUsers.AutoSize = true;
            lblConcurrentUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            lblConcurrentUsers.Location = new System.Drawing.Point(463, 84);
            lblConcurrentUsers.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblConcurrentUsers.Name = "lblConcurrentUsers";
            lblConcurrentUsers.Size = new System.Drawing.Size(109, 42);
            lblConcurrentUsers.TabIndex = 10;
            lblConcurrentUsers.Text = "并发用户数:";
            lblConcurrentUsers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtConcurrentUsers
            // 
            txtConcurrentUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            txtConcurrentUsers.Location = new System.Drawing.Point(580, 88);
            txtConcurrentUsers.Margin = new System.Windows.Forms.Padding(4);
            txtConcurrentUsers.Name = "txtConcurrentUsers";
            txtConcurrentUsers.Size = new System.Drawing.Size(334, 23);
            txtConcurrentUsers.TabIndex = 11;
            // 
            // lblProductVersion
            // 
            lblProductVersion.AutoSize = true;
            lblProductVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            lblProductVersion.Location = new System.Drawing.Point(4, 126);
            lblProductVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblProductVersion.Name = "lblProductVersion";
            lblProductVersion.Size = new System.Drawing.Size(109, 42);
            lblProductVersion.TabIndex = 12;
            lblProductVersion.Text = "产品版本:";
            lblProductVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtProductVersion
            // 
            txtProductVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            txtProductVersion.Location = new System.Drawing.Point(121, 130);
            txtProductVersion.Margin = new System.Windows.Forms.Padding(4);
            txtProductVersion.Name = "txtProductVersion";
            txtProductVersion.ReadOnly = true;
            txtProductVersion.Size = new System.Drawing.Size(334, 23);
            txtProductVersion.TabIndex = 13;
            // 
            // lblMachineCode
            // 
            lblMachineCode.AutoSize = true;
            lblMachineCode.Dock = System.Windows.Forms.DockStyle.Fill;
            lblMachineCode.Location = new System.Drawing.Point(463, 126);
            lblMachineCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblMachineCode.Name = "lblMachineCode";
            lblMachineCode.Size = new System.Drawing.Size(109, 42);
            lblMachineCode.TabIndex = 14;
            lblMachineCode.Text = "机器码:";
            lblMachineCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMachineCode
            // 
            txtMachineCode.Dock = System.Windows.Forms.DockStyle.Fill;
            txtMachineCode.Location = new System.Drawing.Point(580, 130);
            txtMachineCode.Margin = new System.Windows.Forms.Padding(4);
            txtMachineCode.Name = "txtMachineCode";
            txtMachineCode.ReadOnly = true;
            txtMachineCode.Size = new System.Drawing.Size(334, 23);
            txtMachineCode.TabIndex = 15;
            // 
            // chkIsRegistered
            // 
            chkIsRegistered.AutoSize = true;
            chkIsRegistered.Dock = System.Windows.Forms.DockStyle.Fill;
            chkIsRegistered.Enabled = false;
            chkIsRegistered.Location = new System.Drawing.Point(4, 214);
            chkIsRegistered.Margin = new System.Windows.Forms.Padding(4);
            chkIsRegistered.Name = "chkIsRegistered";
            chkIsRegistered.Size = new System.Drawing.Size(109, 34);
            chkIsRegistered.TabIndex = 20;
            chkIsRegistered.Text = "已注册";
            chkIsRegistered.UseVisualStyleBackColor = true;
            // 
            // lblRegistrationCode
            // 
            lblRegistrationCode.AutoSize = true;
            lblRegistrationCode.Dock = System.Windows.Forms.DockStyle.Fill;
            lblRegistrationCode.Location = new System.Drawing.Point(4, 252);
            lblRegistrationCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblRegistrationCode.Name = "lblRegistrationCode";
            lblRegistrationCode.Size = new System.Drawing.Size(109, 42);
            lblRegistrationCode.TabIndex = 21;
            lblRegistrationCode.Text = "注册码:";
            lblRegistrationCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRegistrationCode
            // 
            txtRegistrationCode.Dock = System.Windows.Forms.DockStyle.Fill;
            txtRegistrationCode.Location = new System.Drawing.Point(121, 256);
            txtRegistrationCode.Margin = new System.Windows.Forms.Padding(4);
            txtRegistrationCode.Name = "txtRegistrationCode";
            txtRegistrationCode.Size = new System.Drawing.Size(334, 23);
            txtRegistrationCode.TabIndex = 22;
            // 
            // btnGenerateMachineCode
            // 
            btnGenerateMachineCode.Dock = System.Windows.Forms.DockStyle.Fill;
            btnGenerateMachineCode.Location = new System.Drawing.Point(580, 256);
            btnGenerateMachineCode.Margin = new System.Windows.Forms.Padding(4);
            btnGenerateMachineCode.Name = "btnGenerateMachineCode";
            btnGenerateMachineCode.Size = new System.Drawing.Size(334, 34);
            btnGenerateMachineCode.TabIndex = 23;
            btnGenerateMachineCode.Text = "生成机器码";
            btnGenerateMachineCode.UseVisualStyleBackColor = true;
            btnGenerateMachineCode.Click += btnGenerateMachineCode_Click;
            // 
            // lblRemarks
            // 
            lblRemarks.AutoSize = true;
            lblRemarks.Dock = System.Windows.Forms.DockStyle.Fill;
            lblRemarks.Location = new System.Drawing.Point(4, 294);
            lblRemarks.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblRemarks.Name = "lblRemarks";
            lblRemarks.Size = new System.Drawing.Size(109, 79);
            lblRemarks.TabIndex = 24;
            lblRemarks.Text = "备注:";
            lblRemarks.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRemarks
            // 
            txtRemarks.Dock = System.Windows.Forms.DockStyle.Fill;
            txtRemarks.Location = new System.Drawing.Point(121, 298);
            txtRemarks.Margin = new System.Windows.Forms.Padding(4);
            txtRemarks.Multiline = true;
            txtRemarks.Name = "txtRemarks";
            txtRemarks.Size = new System.Drawing.Size(334, 71);
            txtRemarks.TabIndex = 25;
            // 
            // lblDays
            // 
            lblDays.AutoSize = true;
            lblDays.Dock = System.Windows.Forms.DockStyle.Fill;
            lblDays.Location = new System.Drawing.Point(463, 294);
            lblDays.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblDays.Name = "lblDays";
            lblDays.Size = new System.Drawing.Size(109, 79);
            lblDays.TabIndex = 26;
            lblDays.Text = "授权期限:";
            lblDays.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbDays
            // 
            cmbDays.Dock = System.Windows.Forms.DockStyle.Fill;
            cmbDays.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbDays.FormattingEnabled = true;
            cmbDays.Items.AddRange(new object[] { "一个月", "三个月", "六个月", "一年", "两年", "三年", "五年", "十年" });
            cmbDays.Location = new System.Drawing.Point(580, 298);
            cmbDays.Margin = new System.Windows.Forms.Padding(4);
            cmbDays.Name = "cmbDays";
            cmbDays.Size = new System.Drawing.Size(334, 25);
            cmbDays.TabIndex = 27;
            cmbDays.SelectedIndexChanged += cmbDays_SelectedIndexChanged;
            // 
            // dtpRegistrationDate
            // 
            dtpRegistrationDate.Dock = System.Windows.Forms.DockStyle.Fill;
            dtpRegistrationDate.Location = new System.Drawing.Point(121, 172);
            dtpRegistrationDate.Margin = new System.Windows.Forms.Padding(4);
            dtpRegistrationDate.Name = "dtpRegistrationDate";
            dtpRegistrationDate.Size = new System.Drawing.Size(334, 23);
            dtpRegistrationDate.TabIndex = 19;
            // 
            // dtpExpirationDate
            // 
            dtpExpirationDate.Location = new System.Drawing.Point(580, 172);
            dtpExpirationDate.Margin = new System.Windows.Forms.Padding(4);
            dtpExpirationDate.Name = "dtpExpirationDate";
            dtpExpirationDate.Size = new System.Drawing.Size(334, 23);
            dtpExpirationDate.TabIndex = 9;
            // 
            // dtpPurchaseDate
            // 
            dtpPurchaseDate.Dock = System.Windows.Forms.DockStyle.Fill;
            dtpPurchaseDate.Location = new System.Drawing.Point(121, 88);
            dtpPurchaseDate.Margin = new System.Windows.Forms.Padding(4);
            dtpPurchaseDate.Name = "dtpPurchaseDate";
            dtpPurchaseDate.Size = new System.Drawing.Size(334, 23);
            dtpPurchaseDate.TabIndex = 17;
            // 
            // lblPurchaseDate
            // 
            lblPurchaseDate.AutoSize = true;
            lblPurchaseDate.Dock = System.Windows.Forms.DockStyle.Fill;
            lblPurchaseDate.Location = new System.Drawing.Point(4, 84);
            lblPurchaseDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblPurchaseDate.Name = "lblPurchaseDate";
            lblPurchaseDate.Size = new System.Drawing.Size(109, 42);
            lblPurchaseDate.TabIndex = 16;
            lblPurchaseDate.Text = "购买日期:";
            lblPurchaseDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRegistrationDate
            // 
            lblRegistrationDate.AutoSize = true;
            lblRegistrationDate.Dock = System.Windows.Forms.DockStyle.Fill;
            lblRegistrationDate.Location = new System.Drawing.Point(4, 168);
            lblRegistrationDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblRegistrationDate.Name = "lblRegistrationDate";
            lblRegistrationDate.Size = new System.Drawing.Size(109, 42);
            lblRegistrationDate.TabIndex = 18;
            lblRegistrationDate.Text = "注册日期:";
            lblRegistrationDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblExpirationDate
            // 
            lblExpirationDate.AutoSize = true;
            lblExpirationDate.Dock = System.Windows.Forms.DockStyle.Fill;
            lblExpirationDate.Location = new System.Drawing.Point(463, 168);
            lblExpirationDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblExpirationDate.Name = "lblExpirationDate";
            lblExpirationDate.Size = new System.Drawing.Size(109, 42);
            lblExpirationDate.TabIndex = 8;
            lblExpirationDate.Text = "授权到期:";
            lblExpirationDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox2
            // 
            groupBox2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            groupBox2.Controls.Add(checkedListBoxMod);
            groupBox2.Location = new System.Drawing.Point(4, 445);
            groupBox2.Margin = new System.Windows.Forms.Padding(4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new System.Windows.Forms.Padding(4);
            groupBox2.Size = new System.Drawing.Size(926, 330);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "功能模块";
            // 
            // checkedListBoxMod
            // 
            checkedListBoxMod.Dock = System.Windows.Forms.DockStyle.Fill;
            checkedListBoxMod.FormattingEnabled = true;
            checkedListBoxMod.Location = new System.Drawing.Point(4, 20);
            checkedListBoxMod.Margin = new System.Windows.Forms.Padding(4);
            checkedListBoxMod.Name = "checkedListBoxMod";
            checkedListBoxMod.Size = new System.Drawing.Size(918, 306);
            checkedListBoxMod.TabIndex = 0;
            // 
            // RegistrationManagementControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(toolStrip1);
            Margin = new System.Windows.Forms.Padding(4);
            Name = "RegistrationManagementControl";
            Size = new System.Drawing.Size(933, 779);
            Load += RegistrationManagementControl_Load;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            groupBox1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            groupBox2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbtnSaveRegInfo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbtnCreateRegInfo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbtnRenewRegInfo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblCompanyName;
        private System.Windows.Forms.TextBox txtCompanyName;
        private System.Windows.Forms.Label lblContactName;
        private System.Windows.Forms.TextBox txtContactName;
        private System.Windows.Forms.Label lblPhoneNumber;
        private System.Windows.Forms.TextBox txtPhoneNumber;
        private System.Windows.Forms.Label lblLicenseType;
        private System.Windows.Forms.ComboBox cmbLicenseType;
        private System.Windows.Forms.Label lblExpirationDate;
        private System.Windows.Forms.DateTimePicker dtpExpirationDate;
        private System.Windows.Forms.Label lblConcurrentUsers;
        private System.Windows.Forms.TextBox txtConcurrentUsers;
        private System.Windows.Forms.Label lblProductVersion;
        private System.Windows.Forms.TextBox txtProductVersion;
        private System.Windows.Forms.Label lblMachineCode;
        private System.Windows.Forms.TextBox txtMachineCode;
        private System.Windows.Forms.Label lblPurchaseDate;
        private System.Windows.Forms.DateTimePicker dtpPurchaseDate;
        private System.Windows.Forms.Label lblRegistrationDate;
        private System.Windows.Forms.DateTimePicker dtpRegistrationDate;
        private System.Windows.Forms.CheckBox chkIsRegistered;
        private System.Windows.Forms.Label lblRegistrationCode;
        private System.Windows.Forms.TextBox txtRegistrationCode;
        private System.Windows.Forms.Button btnGenerateMachineCode;
        private System.Windows.Forms.Label lblRemarks;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Label lblDays;
        private System.Windows.Forms.ComboBox cmbDays;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckedListBox checkedListBoxMod;
    }
}