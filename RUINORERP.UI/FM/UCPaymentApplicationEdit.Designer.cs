namespace RUINORERP.UI.FM
{
    partial class UCPaymentApplicationEdit
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
            this.lbl盘点单 = new Krypton.Toolkit.KryptonLabel();
            this.lblDataStatus = new Krypton.Toolkit.KryptonLabel();
            this.lblPrintStatus = new Krypton.Toolkit.KryptonLabel();
            this.lblReview = new Krypton.Toolkit.KryptonLabel();
            this.lblApprovalOpinions = new Krypton.Toolkit.KryptonLabel();
            this.txtApprovalOpinions = new Krypton.Toolkit.KryptonTextBox();
            this.lblApprover_by = new Krypton.Toolkit.KryptonLabel();
            this.txtApprover_by = new Krypton.Toolkit.KryptonTextBox();
            this.lblApprover_at = new Krypton.Toolkit.KryptonLabel();
            this.dtpApprover_at = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lblCloseCaseImagePath = new Krypton.Toolkit.KryptonLabel();
            this.txtCloseCaseImagePath = new Krypton.Toolkit.KryptonTextBox();
            this.lblCloseCaseOpinions = new Krypton.Toolkit.KryptonLabel();
            this.txtCloseCaseOpinions = new Krypton.Toolkit.KryptonTextBox();
            this.lblTotalAmount = new Krypton.Toolkit.KryptonLabel();
            this.txtTotalAmount = new Krypton.Toolkit.KryptonTextBox();
            this.lblOverpaymentAmount = new Krypton.Toolkit.KryptonLabel();
            this.txtOverpaymentAmount = new Krypton.Toolkit.KryptonTextBox();
            this.lblCurrency_ID = new Krypton.Toolkit.KryptonLabel();
            this.cmbCurrency_ID = new Krypton.Toolkit.KryptonComboBox();
            this.lblAccount_id = new Krypton.Toolkit.KryptonLabel();
            this.cmbAccount_id = new Krypton.Toolkit.KryptonComboBox();
            this.lblIsAdvancePayment = new Krypton.Toolkit.KryptonLabel();
            this.chkIsAdvancePayment = new Krypton.Toolkit.KryptonCheckBox();
            this.lblPrePaymentBill_id = new Krypton.Toolkit.KryptonLabel();
            this.txtPrePaymentBill_id = new Krypton.Toolkit.KryptonTextBox();
            this.lblPayReasonItems = new Krypton.Toolkit.KryptonLabel();
            this.txtPayReasonItems = new Krypton.Toolkit.KryptonTextBox();
            this.lblInvoiceDate = new Krypton.Toolkit.KryptonLabel();
            this.dtpInvoiceDate = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lblPaymentDate = new Krypton.Toolkit.KryptonLabel();
            this.dtpPaymentDate = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lblApplicationNo = new Krypton.Toolkit.KryptonLabel();
            this.txtApplicationNo = new Krypton.Toolkit.KryptonTextBox();
            this.lblDepartmentID = new Krypton.Toolkit.KryptonLabel();
            this.cmbDepartmentID = new Krypton.Toolkit.KryptonComboBox();
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            this.cmbEmployee_ID = new Krypton.Toolkit.KryptonComboBox();
            this.lblCustomerVendor_ID = new Krypton.Toolkit.KryptonLabel();
            this.cmbCustomerVendor_ID = new Krypton.Toolkit.KryptonComboBox();
            this.lblPayeeInfoID = new Krypton.Toolkit.KryptonLabel();
            this.cmbPayeeInfoID = new Krypton.Toolkit.KryptonComboBox();
            this.lblPayeeAccountNo = new Krypton.Toolkit.KryptonLabel();
            this.txtPayeeAccountNo = new Krypton.Toolkit.KryptonTextBox();
            this.lblNotes = new Krypton.Toolkit.KryptonLabel();
            this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceSub)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCurrency_ID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAccount_id)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDepartmentID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmployee_ID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCustomerVendor_ID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPayeeInfoID)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(199, 588);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(317, 588);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.lbl盘点单);
            this.kryptonPanel1.Controls.Add(this.lblDataStatus);
            this.kryptonPanel1.Controls.Add(this.lblPrintStatus);
            this.kryptonPanel1.Controls.Add(this.lblReview);
            this.kryptonPanel1.Controls.Add(this.lblApprovalOpinions);
            this.kryptonPanel1.Controls.Add(this.txtApprovalOpinions);
            this.kryptonPanel1.Controls.Add(this.lblApprover_by);
            this.kryptonPanel1.Controls.Add(this.txtApprover_by);
            this.kryptonPanel1.Controls.Add(this.lblApprover_at);
            this.kryptonPanel1.Controls.Add(this.dtpApprover_at);
            this.kryptonPanel1.Controls.Add(this.lblCloseCaseImagePath);
            this.kryptonPanel1.Controls.Add(this.txtCloseCaseImagePath);
            this.kryptonPanel1.Controls.Add(this.lblCloseCaseOpinions);
            this.kryptonPanel1.Controls.Add(this.txtCloseCaseOpinions);
            this.kryptonPanel1.Controls.Add(this.lblTotalAmount);
            this.kryptonPanel1.Controls.Add(this.txtTotalAmount);
            this.kryptonPanel1.Controls.Add(this.lblOverpaymentAmount);
            this.kryptonPanel1.Controls.Add(this.txtOverpaymentAmount);
            this.kryptonPanel1.Controls.Add(this.lblCurrency_ID);
            this.kryptonPanel1.Controls.Add(this.cmbCurrency_ID);
            this.kryptonPanel1.Controls.Add(this.lblAccount_id);
            this.kryptonPanel1.Controls.Add(this.cmbAccount_id);
            this.kryptonPanel1.Controls.Add(this.lblIsAdvancePayment);
            this.kryptonPanel1.Controls.Add(this.chkIsAdvancePayment);
            this.kryptonPanel1.Controls.Add(this.lblPrePaymentBill_id);
            this.kryptonPanel1.Controls.Add(this.txtPrePaymentBill_id);
            this.kryptonPanel1.Controls.Add(this.lblPayReasonItems);
            this.kryptonPanel1.Controls.Add(this.txtPayReasonItems);
            this.kryptonPanel1.Controls.Add(this.lblInvoiceDate);
            this.kryptonPanel1.Controls.Add(this.dtpInvoiceDate);
            this.kryptonPanel1.Controls.Add(this.lblPaymentDate);
            this.kryptonPanel1.Controls.Add(this.dtpPaymentDate);
            this.kryptonPanel1.Controls.Add(this.lblApplicationNo);
            this.kryptonPanel1.Controls.Add(this.txtApplicationNo);
            this.kryptonPanel1.Controls.Add(this.lblDepartmentID);
            this.kryptonPanel1.Controls.Add(this.cmbDepartmentID);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel1);
            this.kryptonPanel1.Controls.Add(this.cmbEmployee_ID);
            this.kryptonPanel1.Controls.Add(this.lblCustomerVendor_ID);
            this.kryptonPanel1.Controls.Add(this.cmbCustomerVendor_ID);
            this.kryptonPanel1.Controls.Add(this.lblPayeeInfoID);
            this.kryptonPanel1.Controls.Add(this.cmbPayeeInfoID);
            this.kryptonPanel1.Controls.Add(this.lblPayeeAccountNo);
            this.kryptonPanel1.Controls.Add(this.txtPayeeAccountNo);
            this.kryptonPanel1.Controls.Add(this.lblNotes);
            this.kryptonPanel1.Controls.Add(this.txtNotes);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.PaletteMode = Krypton.Toolkit.PaletteMode.Office2007Blue;
            this.kryptonPanel1.Size = new System.Drawing.Size(1063, 669);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // lbl盘点单
            // 
            this.lbl盘点单.LabelStyle = Krypton.Toolkit.LabelStyle.TitlePanel;
            this.lbl盘点单.Location = new System.Drawing.Point(402, 25);
            this.lbl盘点单.Name = "lbl盘点单";
            this.lbl盘点单.Size = new System.Drawing.Size(110, 29);
            this.lbl盘点单.StateCommon.LongText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.lbl盘点单.StateCommon.LongText.Color2 = System.Drawing.Color.Lime;
            this.lbl盘点单.TabIndex = 131;
            this.lbl盘点单.Values.Text = "付款申请单";
            // 
            // lblDataStatus
            // 
            this.lblDataStatus.Location = new System.Drawing.Point(909, 40);
            this.lblDataStatus.Name = "lblDataStatus";
            this.lblDataStatus.Size = new System.Drawing.Size(125, 38);
            this.lblDataStatus.StateNormal.ShortText.Color1 = System.Drawing.Color.Red;
            this.lblDataStatus.StateNormal.ShortText.Font = new System.Drawing.Font("长城行楷体", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDataStatus.TabIndex = 130;
            this.lblDataStatus.Values.Text = "数据状态";
            // 
            // lblPrintStatus
            // 
            this.lblPrintStatus.Location = new System.Drawing.Point(661, 40);
            this.lblPrintStatus.Name = "lblPrintStatus";
            this.lblPrintStatus.Size = new System.Drawing.Size(125, 38);
            this.lblPrintStatus.StateNormal.ShortText.Color1 = System.Drawing.Color.Red;
            this.lblPrintStatus.StateNormal.ShortText.Font = new System.Drawing.Font("长城行楷体", 20F);
            this.lblPrintStatus.TabIndex = 129;
            this.lblPrintStatus.Values.Text = "打印状态";
            // 
            // lblReview
            // 
            this.lblReview.Location = new System.Drawing.Point(786, 40);
            this.lblReview.Name = "lblReview";
            this.lblReview.Size = new System.Drawing.Size(125, 38);
            this.lblReview.StateNormal.ShortText.Color1 = System.Drawing.Color.Red;
            this.lblReview.StateNormal.ShortText.Font = new System.Drawing.Font("长城行楷体", 20F);
            this.lblReview.TabIndex = 128;
            this.lblReview.Values.Text = "审核状态";
            // 
            // lblApprovalOpinions
            // 
            this.lblApprovalOpinions.Location = new System.Drawing.Point(613, 503);
            this.lblApprovalOpinions.Name = "lblApprovalOpinions";
            this.lblApprovalOpinions.Size = new System.Drawing.Size(62, 20);
            this.lblApprovalOpinions.TabIndex = 112;
            this.lblApprovalOpinions.Values.Text = "审批意见";
            // 
            // txtApprovalOpinions
            // 
            this.txtApprovalOpinions.Location = new System.Drawing.Point(686, 499);
            this.txtApprovalOpinions.Multiline = true;
            this.txtApprovalOpinions.Name = "txtApprovalOpinions";
            this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
            this.txtApprovalOpinions.TabIndex = 113;
            // 
            // lblApprover_by
            // 
            this.lblApprover_by.Location = new System.Drawing.Point(613, 530);
            this.lblApprover_by.Name = "lblApprover_by";
            this.lblApprover_by.Size = new System.Drawing.Size(49, 20);
            this.lblApprover_by.TabIndex = 114;
            this.lblApprover_by.Values.Text = "审批人";
            // 
            // txtApprover_by
            // 
            this.txtApprover_by.Location = new System.Drawing.Point(686, 526);
            this.txtApprover_by.Name = "txtApprover_by";
            this.txtApprover_by.Size = new System.Drawing.Size(100, 23);
            this.txtApprover_by.TabIndex = 115;
            // 
            // lblApprover_at
            // 
            this.lblApprover_at.Location = new System.Drawing.Point(613, 555);
            this.lblApprover_at.Name = "lblApprover_at";
            this.lblApprover_at.Size = new System.Drawing.Size(62, 20);
            this.lblApprover_at.TabIndex = 116;
            this.lblApprover_at.Values.Text = "审批时间";
            // 
            // dtpApprover_at
            // 
            this.dtpApprover_at.Location = new System.Drawing.Point(686, 551);
            this.dtpApprover_at.Name = "dtpApprover_at";
            this.dtpApprover_at.ShowCheckBox = true;
            this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
            this.dtpApprover_at.TabIndex = 117;
            // 
            // lblCloseCaseImagePath
            // 
            this.lblCloseCaseImagePath.Location = new System.Drawing.Point(861, 507);
            this.lblCloseCaseImagePath.Name = "lblCloseCaseImagePath";
            this.lblCloseCaseImagePath.Size = new System.Drawing.Size(62, 20);
            this.lblCloseCaseImagePath.TabIndex = 124;
            this.lblCloseCaseImagePath.Values.Text = "结案凭证";
            // 
            // txtCloseCaseImagePath
            // 
            this.txtCloseCaseImagePath.Location = new System.Drawing.Point(934, 503);
            this.txtCloseCaseImagePath.Multiline = true;
            this.txtCloseCaseImagePath.Name = "txtCloseCaseImagePath";
            this.txtCloseCaseImagePath.Size = new System.Drawing.Size(100, 21);
            this.txtCloseCaseImagePath.TabIndex = 125;
            // 
            // lblCloseCaseOpinions
            // 
            this.lblCloseCaseOpinions.Location = new System.Drawing.Point(861, 532);
            this.lblCloseCaseOpinions.Name = "lblCloseCaseOpinions";
            this.lblCloseCaseOpinions.Size = new System.Drawing.Size(62, 20);
            this.lblCloseCaseOpinions.TabIndex = 126;
            this.lblCloseCaseOpinions.Values.Text = "结案意见";
            // 
            // txtCloseCaseOpinions
            // 
            this.txtCloseCaseOpinions.Location = new System.Drawing.Point(934, 528);
            this.txtCloseCaseOpinions.Name = "txtCloseCaseOpinions";
            this.txtCloseCaseOpinions.Size = new System.Drawing.Size(100, 23);
            this.txtCloseCaseOpinions.TabIndex = 127;
            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.Location = new System.Drawing.Point(415, 171);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(62, 20);
            this.lblTotalAmount.TabIndex = 108;
            this.lblTotalAmount.Values.Text = "付款金额";
            // 
            // txtTotalAmount
            // 
            this.txtTotalAmount.Location = new System.Drawing.Point(488, 167);
            this.txtTotalAmount.Name = "txtTotalAmount";
            this.txtTotalAmount.Size = new System.Drawing.Size(100, 23);
            this.txtTotalAmount.TabIndex = 109;
            // 
            // lblOverpaymentAmount
            // 
            this.lblOverpaymentAmount.Location = new System.Drawing.Point(415, 196);
            this.lblOverpaymentAmount.Name = "lblOverpaymentAmount";
            this.lblOverpaymentAmount.Size = new System.Drawing.Size(62, 20);
            this.lblOverpaymentAmount.TabIndex = 110;
            this.lblOverpaymentAmount.Values.Text = "超付金额";
            // 
            // txtOverpaymentAmount
            // 
            this.txtOverpaymentAmount.Location = new System.Drawing.Point(488, 192);
            this.txtOverpaymentAmount.Name = "txtOverpaymentAmount";
            this.txtOverpaymentAmount.Size = new System.Drawing.Size(100, 23);
            this.txtOverpaymentAmount.TabIndex = 111;
            // 
            // lblCurrency_ID
            // 
            this.lblCurrency_ID.Location = new System.Drawing.Point(32, 144);
            this.lblCurrency_ID.Name = "lblCurrency_ID";
            this.lblCurrency_ID.Size = new System.Drawing.Size(36, 20);
            this.lblCurrency_ID.TabIndex = 94;
            this.lblCurrency_ID.Values.Text = "币别";
            // 
            // cmbCurrency_ID
            // 
            this.cmbCurrency_ID.DropDownWidth = 100;
            this.cmbCurrency_ID.IntegralHeight = false;
            this.cmbCurrency_ID.Location = new System.Drawing.Point(105, 140);
            this.cmbCurrency_ID.Name = "cmbCurrency_ID";
            this.cmbCurrency_ID.Size = new System.Drawing.Size(100, 21);
            this.cmbCurrency_ID.TabIndex = 95;
            // 
            // lblAccount_id
            // 
            this.lblAccount_id.Location = new System.Drawing.Point(342, 296);
            this.lblAccount_id.Name = "lblAccount_id";
            this.lblAccount_id.Size = new System.Drawing.Size(62, 20);
            this.lblAccount_id.TabIndex = 96;
            this.lblAccount_id.Values.Text = "付款账户";
            // 
            // cmbAccount_id
            // 
            this.cmbAccount_id.DropDownWidth = 100;
            this.cmbAccount_id.IntegralHeight = false;
            this.cmbAccount_id.Location = new System.Drawing.Point(415, 292);
            this.cmbAccount_id.Name = "cmbAccount_id";
            this.cmbAccount_id.Size = new System.Drawing.Size(100, 21);
            this.cmbAccount_id.TabIndex = 97;
            // 
            // lblIsAdvancePayment
            // 
            this.lblIsAdvancePayment.Location = new System.Drawing.Point(342, 277);
            this.lblIsAdvancePayment.Name = "lblIsAdvancePayment";
            this.lblIsAdvancePayment.Size = new System.Drawing.Size(62, 20);
            this.lblIsAdvancePayment.TabIndex = 98;
            this.lblIsAdvancePayment.Values.Text = "为预付款";
            // 
            // chkIsAdvancePayment
            // 
            this.chkIsAdvancePayment.Location = new System.Drawing.Point(415, 273);
            this.chkIsAdvancePayment.Name = "chkIsAdvancePayment";
            this.chkIsAdvancePayment.Size = new System.Drawing.Size(19, 13);
            this.chkIsAdvancePayment.TabIndex = 99;
            this.chkIsAdvancePayment.Values.Text = "";
            // 
            // lblPrePaymentBill_id
            // 
            this.lblPrePaymentBill_id.Location = new System.Drawing.Point(694, 251);
            this.lblPrePaymentBill_id.Name = "lblPrePaymentBill_id";
            this.lblPrePaymentBill_id.Size = new System.Drawing.Size(49, 20);
            this.lblPrePaymentBill_id.TabIndex = 100;
            this.lblPrePaymentBill_id.Values.Text = "预付单";
            // 
            // txtPrePaymentBill_id
            // 
            this.txtPrePaymentBill_id.Location = new System.Drawing.Point(767, 247);
            this.txtPrePaymentBill_id.Name = "txtPrePaymentBill_id";
            this.txtPrePaymentBill_id.Size = new System.Drawing.Size(100, 23);
            this.txtPrePaymentBill_id.TabIndex = 101;
            // 
            // lblPayReasonItems
            // 
            this.lblPayReasonItems.Location = new System.Drawing.Point(116, 431);
            this.lblPayReasonItems.Name = "lblPayReasonItems";
            this.lblPayReasonItems.Size = new System.Drawing.Size(62, 20);
            this.lblPayReasonItems.TabIndex = 102;
            this.lblPayReasonItems.Values.Text = "付款项目";
            // 
            // txtPayReasonItems
            // 
            this.txtPayReasonItems.Location = new System.Drawing.Point(189, 427);
            this.txtPayReasonItems.Multiline = true;
            this.txtPayReasonItems.Name = "txtPayReasonItems";
            this.txtPayReasonItems.Size = new System.Drawing.Size(100, 21);
            this.txtPayReasonItems.TabIndex = 103;
            // 
            // lblInvoiceDate
            // 
            this.lblInvoiceDate.Location = new System.Drawing.Point(290, 65);
            this.lblInvoiceDate.Name = "lblInvoiceDate";
            this.lblInvoiceDate.Size = new System.Drawing.Size(62, 20);
            this.lblInvoiceDate.TabIndex = 104;
            this.lblInvoiceDate.Values.Text = "对账日期";
            // 
            // dtpInvoiceDate
            // 
            this.dtpInvoiceDate.Location = new System.Drawing.Point(363, 61);
            this.dtpInvoiceDate.Name = "dtpInvoiceDate";
            this.dtpInvoiceDate.ShowCheckBox = true;
            this.dtpInvoiceDate.Size = new System.Drawing.Size(100, 21);
            this.dtpInvoiceDate.TabIndex = 105;
            // 
            // lblPaymentDate
            // 
            this.lblPaymentDate.Location = new System.Drawing.Point(486, 61);
            this.lblPaymentDate.Name = "lblPaymentDate";
            this.lblPaymentDate.Size = new System.Drawing.Size(62, 20);
            this.lblPaymentDate.TabIndex = 106;
            this.lblPaymentDate.Values.Text = "付款日期";
            // 
            // dtpPaymentDate
            // 
            this.dtpPaymentDate.Location = new System.Drawing.Point(559, 57);
            this.dtpPaymentDate.Name = "dtpPaymentDate";
            this.dtpPaymentDate.ShowCheckBox = true;
            this.dtpPaymentDate.Size = new System.Drawing.Size(100, 21);
            this.dtpPaymentDate.TabIndex = 107;
            // 
            // lblApplicationNo
            // 
            this.lblApplicationNo.Location = new System.Drawing.Point(32, 61);
            this.lblApplicationNo.Name = "lblApplicationNo";
            this.lblApplicationNo.Size = new System.Drawing.Size(62, 20);
            this.lblApplicationNo.TabIndex = 82;
            this.lblApplicationNo.Values.Text = "申请单号";
            // 
            // txtApplicationNo
            // 
            this.txtApplicationNo.Location = new System.Drawing.Point(105, 57);
            this.txtApplicationNo.Name = "txtApplicationNo";
            this.txtApplicationNo.Size = new System.Drawing.Size(100, 23);
            this.txtApplicationNo.TabIndex = 83;
            // 
            // lblDepartmentID
            // 
            this.lblDepartmentID.Location = new System.Drawing.Point(678, 96);
            this.lblDepartmentID.Name = "lblDepartmentID";
            this.lblDepartmentID.Size = new System.Drawing.Size(36, 20);
            this.lblDepartmentID.TabIndex = 84;
            this.lblDepartmentID.Values.Text = "部门";
            // 
            // cmbDepartmentID
            // 
            this.cmbDepartmentID.DropDownWidth = 100;
            this.cmbDepartmentID.IntegralHeight = false;
            this.cmbDepartmentID.Location = new System.Drawing.Point(751, 92);
            this.cmbDepartmentID.Name = "cmbDepartmentID";
            this.cmbDepartmentID.Size = new System.Drawing.Size(100, 21);
            this.cmbDepartmentID.TabIndex = 85;
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(290, 92);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(49, 20);
            this.kryptonLabel1.TabIndex = 86;
            this.kryptonLabel1.Values.Text = "申请人";
            // 
            // cmbEmployee_ID
            // 
            this.cmbEmployee_ID.DropDownWidth = 100;
            this.cmbEmployee_ID.IntegralHeight = false;
            this.cmbEmployee_ID.Location = new System.Drawing.Point(363, 88);
            this.cmbEmployee_ID.Name = "cmbEmployee_ID";
            this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
            this.cmbEmployee_ID.TabIndex = 87;
            // 
            // lblCustomerVendor_ID
            // 
            this.lblCustomerVendor_ID.Location = new System.Drawing.Point(32, 90);
            this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
            this.lblCustomerVendor_ID.Size = new System.Drawing.Size(62, 20);
            this.lblCustomerVendor_ID.TabIndex = 88;
            this.lblCustomerVendor_ID.Values.Text = "收款单位";
            // 
            // cmbCustomerVendor_ID
            // 
            this.cmbCustomerVendor_ID.DropDownWidth = 100;
            this.cmbCustomerVendor_ID.IntegralHeight = false;
            this.cmbCustomerVendor_ID.Location = new System.Drawing.Point(105, 86);
            this.cmbCustomerVendor_ID.Name = "cmbCustomerVendor_ID";
            this.cmbCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
            this.cmbCustomerVendor_ID.TabIndex = 89;
            // 
            // lblPayeeInfoID
            // 
            this.lblPayeeInfoID.Location = new System.Drawing.Point(32, 117);
            this.lblPayeeInfoID.Name = "lblPayeeInfoID";
            this.lblPayeeInfoID.Size = new System.Drawing.Size(62, 20);
            this.lblPayeeInfoID.TabIndex = 90;
            this.lblPayeeInfoID.Values.Text = "收款信息";
            // 
            // cmbPayeeInfoID
            // 
            this.cmbPayeeInfoID.DropDownWidth = 100;
            this.cmbPayeeInfoID.IntegralHeight = false;
            this.cmbPayeeInfoID.Location = new System.Drawing.Point(105, 113);
            this.cmbPayeeInfoID.Name = "cmbPayeeInfoID";
            this.cmbPayeeInfoID.Size = new System.Drawing.Size(100, 21);
            this.cmbPayeeInfoID.TabIndex = 91;
            // 
            // lblPayeeAccountNo
            // 
            this.lblPayeeAccountNo.Location = new System.Drawing.Point(290, 121);
            this.lblPayeeAccountNo.Name = "lblPayeeAccountNo";
            this.lblPayeeAccountNo.Size = new System.Drawing.Size(62, 20);
            this.lblPayeeAccountNo.TabIndex = 92;
            this.lblPayeeAccountNo.Values.Text = "收款账号";
            // 
            // txtPayeeAccountNo
            // 
            this.txtPayeeAccountNo.Location = new System.Drawing.Point(363, 117);
            this.txtPayeeAccountNo.Name = "txtPayeeAccountNo";
            this.txtPayeeAccountNo.Size = new System.Drawing.Size(263, 23);
            this.txtPayeeAccountNo.TabIndex = 93;
            // 
            // lblNotes
            // 
            this.lblNotes.Location = new System.Drawing.Point(48, 488);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.PaletteMode = Krypton.Toolkit.PaletteMode.Office2007Blue;
            this.lblNotes.Size = new System.Drawing.Size(36, 20);
            this.lblNotes.TabIndex = 31;
            this.lblNotes.Values.Text = "备注";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(90, 488);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(355, 72);
            this.txtNotes.TabIndex = 32;
            // 
            // UCPaymentApplicationEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCPaymentApplicationEdit";
            this.Size = new System.Drawing.Size(1063, 669);
            this.Load += new System.EventHandler(this.UCCustomerVendorEdit_Load);
            this.Controls.SetChildIndex(this.kryptonPanel1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceSub)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCurrency_ID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAccount_id)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDepartmentID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmployee_ID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCustomerVendor_ID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPayeeInfoID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonLabel lblNotes;
        private Krypton.Toolkit.KryptonTextBox txtNotes;
        private Krypton.Toolkit.KryptonLabel lblApplicationNo;
        private Krypton.Toolkit.KryptonTextBox txtApplicationNo;
        private Krypton.Toolkit.KryptonLabel lblDepartmentID;
        private Krypton.Toolkit.KryptonComboBox cmbDepartmentID;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;
        private Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
        private Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;
        private Krypton.Toolkit.KryptonLabel lblPayeeInfoID;
        private Krypton.Toolkit.KryptonComboBox cmbPayeeInfoID;
        private Krypton.Toolkit.KryptonLabel lblPayeeAccountNo;
        private Krypton.Toolkit.KryptonTextBox txtPayeeAccountNo;
        private Krypton.Toolkit.KryptonLabel lblCurrency_ID;
        private Krypton.Toolkit.KryptonComboBox cmbCurrency_ID;
        private Krypton.Toolkit.KryptonLabel lblAccount_id;
        private Krypton.Toolkit.KryptonComboBox cmbAccount_id;
        private Krypton.Toolkit.KryptonLabel lblIsAdvancePayment;
        private Krypton.Toolkit.KryptonCheckBox chkIsAdvancePayment;
        private Krypton.Toolkit.KryptonLabel lblPrePaymentBill_id;
        private Krypton.Toolkit.KryptonTextBox txtPrePaymentBill_id;
        private Krypton.Toolkit.KryptonLabel lblPayReasonItems;
        private Krypton.Toolkit.KryptonTextBox txtPayReasonItems;
        private Krypton.Toolkit.KryptonLabel lblInvoiceDate;
        private Krypton.Toolkit.KryptonDateTimePicker dtpInvoiceDate;
        private Krypton.Toolkit.KryptonLabel lblPaymentDate;
        private Krypton.Toolkit.KryptonDateTimePicker dtpPaymentDate;
        private Krypton.Toolkit.KryptonLabel lblTotalAmount;
        private Krypton.Toolkit.KryptonTextBox txtTotalAmount;
        private Krypton.Toolkit.KryptonLabel lblOverpaymentAmount;
        private Krypton.Toolkit.KryptonTextBox txtOverpaymentAmount;
        private Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
        private Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;
        private Krypton.Toolkit.KryptonLabel lblApprover_by;
        private Krypton.Toolkit.KryptonTextBox txtApprover_by;
        private Krypton.Toolkit.KryptonLabel lblApprover_at;
        private Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;
        private Krypton.Toolkit.KryptonLabel lblCloseCaseImagePath;
        private Krypton.Toolkit.KryptonTextBox txtCloseCaseImagePath;
        private Krypton.Toolkit.KryptonLabel lblCloseCaseOpinions;
        private Krypton.Toolkit.KryptonTextBox txtCloseCaseOpinions;
        private Krypton.Toolkit.KryptonLabel lblDataStatus;
        private Krypton.Toolkit.KryptonLabel lblPrintStatus;
        private Krypton.Toolkit.KryptonLabel lblReview;
        private Krypton.Toolkit.KryptonLabel lbl盘点单;
    }
}
