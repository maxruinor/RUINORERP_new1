// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/24/2025 18:28:25
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 价格调整单
    /// </summary>
    partial class tb_FM_PriceAdjustmentEdit
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
        
     //for definition
     // this.btnOk = new ComponentFactory.Krypton.Toolkit.KryptonButton();
     // this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
      //this.kryptonPanel1 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
      //for definition
     this.lblAdjustNo = new Krypton.Toolkit.KryptonLabel();
this.txtAdjustNo = new Krypton.Toolkit.KryptonTextBox();

this.lblCustomerVendor_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbCustomerVendor_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblReceivePaymentType = new Krypton.Toolkit.KryptonLabel();
this.txtReceivePaymentType = new Krypton.Toolkit.KryptonTextBox();

this.lblSourceBizType = new Krypton.Toolkit.KryptonLabel();
this.txtSourceBizType = new Krypton.Toolkit.KryptonTextBox();

this.lblSourceBillId = new Krypton.Toolkit.KryptonLabel();
this.txtSourceBillId = new Krypton.Toolkit.KryptonTextBox();

this.lblSourceBillNo = new Krypton.Toolkit.KryptonLabel();
this.txtSourceBillNo = new Krypton.Toolkit.KryptonTextBox();

this.lblCurrency_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbCurrency_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblAdjustReason = new Krypton.Toolkit.KryptonLabel();
this.txtAdjustReason = new Krypton.Toolkit.KryptonTextBox();
this.txtAdjustReason.Multiline = true;

this.lblExchangeRate = new Krypton.Toolkit.KryptonLabel();
this.txtExchangeRate = new Krypton.Toolkit.KryptonTextBox();

this.lblAdjustDate = new Krypton.Toolkit.KryptonLabel();
this.dtpAdjustDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblDepartmentID = new Krypton.Toolkit.KryptonLabel();
this.cmbDepartmentID = new Krypton.Toolkit.KryptonComboBox();

this.lblProjectGroup_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbProjectGroup_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblInvoiceId = new Krypton.Toolkit.KryptonLabel();
this.txtInvoiceId = new Krypton.Toolkit.KryptonTextBox();

this.lblInvoiced = new Krypton.Toolkit.KryptonLabel();
this.chkInvoiced = new Krypton.Toolkit.KryptonCheckBox();
this.chkInvoiced.Values.Text ="";

this.lblTotalForeignDiffAmount = new Krypton.Toolkit.KryptonLabel();
this.txtTotalForeignDiffAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalLocalDiffAmount = new Krypton.Toolkit.KryptonLabel();
this.txtTotalLocalDiffAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblIsIncludeTax = new Krypton.Toolkit.KryptonLabel();
this.chkIsIncludeTax = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsIncludeTax.Values.Text ="";

this.lblTaxTotalDiffLocalAmount = new Krypton.Toolkit.KryptonLabel();
this.txtTaxTotalDiffLocalAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblDataStatus = new Krypton.Toolkit.KryptonLabel();
this.txtDataStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblRemark = new Krypton.Toolkit.KryptonLabel();
this.txtRemark = new Krypton.Toolkit.KryptonTextBox();
this.txtRemark.Multiline = true;

this.lblApprovalOpinions = new Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new Krypton.Toolkit.KryptonTextBox();
this.txtApprovalOpinions.Multiline = true;

this.lblApprover_by = new Krypton.Toolkit.KryptonLabel();
this.txtApprover_by = new Krypton.Toolkit.KryptonTextBox();

this.lblApprover_at = new Krypton.Toolkit.KryptonLabel();
this.dtpApprover_at = new Krypton.Toolkit.KryptonDateTimePicker();


this.lblApprovalResults = new Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";

this.lblPrintStatus = new Krypton.Toolkit.KryptonLabel();
this.txtPrintStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblCreated_at = new Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblCreated_by = new Krypton.Toolkit.KryptonLabel();
this.txtCreated_by = new Krypton.Toolkit.KryptonTextBox();

this.lblModified_at = new Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblModified_by = new Krypton.Toolkit.KryptonLabel();
this.txtModified_by = new Krypton.Toolkit.KryptonTextBox();

this.lblisdeleted = new Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

    
    //for end
   // ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
   // this.kryptonPanel1.SuspendLayout();
    this.SuspendLayout();
    
            // 
            // btnOk
            // 
            //this.btnOk.Location = new System.Drawing.Point(126, 355);
            //this.btnOk.Name = "btnOk";
            //this.btnOk.Size = new System.Drawing.Size(90, 25);
            //this.btnOk.TabIndex = 0;
           // this.btnOk.Values.Text = "确定";
            //this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
           // this.btnCancel.Location = new System.Drawing.Point(244, 355);
            //this.btnCancel.Name = "btnCancel";
            //this.btnCancel.Size = new System.Drawing.Size(90, 25);
            //this.btnCancel.TabIndex = 1;
            //this.btnCancel.Values.Text = "取消";
           // this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            
         //for size
     
            //#####30AdjustNo###String
this.lblAdjustNo.AutoSize = true;
this.lblAdjustNo.Location = new System.Drawing.Point(100,25);
this.lblAdjustNo.Name = "lblAdjustNo";
this.lblAdjustNo.Size = new System.Drawing.Size(41, 12);
this.lblAdjustNo.TabIndex = 1;
this.lblAdjustNo.Text = "调整编号";
this.txtAdjustNo.Location = new System.Drawing.Point(173,21);
this.txtAdjustNo.Name = "txtAdjustNo";
this.txtAdjustNo.Size = new System.Drawing.Size(100, 21);
this.txtAdjustNo.TabIndex = 1;
this.Controls.Add(this.lblAdjustNo);
this.Controls.Add(this.txtAdjustNo);

           //#####CustomerVendor_ID###Int64
//属性测试50CustomerVendor_ID
//属性测试50CustomerVendor_ID
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,50);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 2;
this.lblCustomerVendor_ID.Text = "往来单位";
//111======50
this.cmbCustomerVendor_ID.Location = new System.Drawing.Point(173,46);
this.cmbCustomerVendor_ID.Name ="cmbCustomerVendor_ID";
this.cmbCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCustomerVendor_ID.TabIndex = 2;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.cmbCustomerVendor_ID);

           //#####ReceivePaymentType###Int32
//属性测试75ReceivePaymentType
//属性测试75ReceivePaymentType
//属性测试75ReceivePaymentType
//属性测试75ReceivePaymentType
//属性测试75ReceivePaymentType
this.lblReceivePaymentType.AutoSize = true;
this.lblReceivePaymentType.Location = new System.Drawing.Point(100,75);
this.lblReceivePaymentType.Name = "lblReceivePaymentType";
this.lblReceivePaymentType.Size = new System.Drawing.Size(41, 12);
this.lblReceivePaymentType.TabIndex = 3;
this.lblReceivePaymentType.Text = "收付类型";
this.txtReceivePaymentType.Location = new System.Drawing.Point(173,71);
this.txtReceivePaymentType.Name = "txtReceivePaymentType";
this.txtReceivePaymentType.Size = new System.Drawing.Size(100, 21);
this.txtReceivePaymentType.TabIndex = 3;
this.Controls.Add(this.lblReceivePaymentType);
this.Controls.Add(this.txtReceivePaymentType);

           //#####SourceBizType###Int32
//属性测试100SourceBizType
//属性测试100SourceBizType
//属性测试100SourceBizType
//属性测试100SourceBizType
//属性测试100SourceBizType
this.lblSourceBizType.AutoSize = true;
this.lblSourceBizType.Location = new System.Drawing.Point(100,100);
this.lblSourceBizType.Name = "lblSourceBizType";
this.lblSourceBizType.Size = new System.Drawing.Size(41, 12);
this.lblSourceBizType.TabIndex = 4;
this.lblSourceBizType.Text = "来源业务";
this.txtSourceBizType.Location = new System.Drawing.Point(173,96);
this.txtSourceBizType.Name = "txtSourceBizType";
this.txtSourceBizType.Size = new System.Drawing.Size(100, 21);
this.txtSourceBizType.TabIndex = 4;
this.Controls.Add(this.lblSourceBizType);
this.Controls.Add(this.txtSourceBizType);

           //#####SourceBillId###Int64
//属性测试125SourceBillId
//属性测试125SourceBillId
//属性测试125SourceBillId
//属性测试125SourceBillId
//属性测试125SourceBillId
this.lblSourceBillId.AutoSize = true;
this.lblSourceBillId.Location = new System.Drawing.Point(100,125);
this.lblSourceBillId.Name = "lblSourceBillId";
this.lblSourceBillId.Size = new System.Drawing.Size(41, 12);
this.lblSourceBillId.TabIndex = 5;
this.lblSourceBillId.Text = "来源单据";
this.txtSourceBillId.Location = new System.Drawing.Point(173,121);
this.txtSourceBillId.Name = "txtSourceBillId";
this.txtSourceBillId.Size = new System.Drawing.Size(100, 21);
this.txtSourceBillId.TabIndex = 5;
this.Controls.Add(this.lblSourceBillId);
this.Controls.Add(this.txtSourceBillId);

           //#####30SourceBillNo###String
this.lblSourceBillNo.AutoSize = true;
this.lblSourceBillNo.Location = new System.Drawing.Point(100,150);
this.lblSourceBillNo.Name = "lblSourceBillNo";
this.lblSourceBillNo.Size = new System.Drawing.Size(41, 12);
this.lblSourceBillNo.TabIndex = 6;
this.lblSourceBillNo.Text = "来源单号";
this.txtSourceBillNo.Location = new System.Drawing.Point(173,146);
this.txtSourceBillNo.Name = "txtSourceBillNo";
this.txtSourceBillNo.Size = new System.Drawing.Size(100, 21);
this.txtSourceBillNo.TabIndex = 6;
this.Controls.Add(this.lblSourceBillNo);
this.Controls.Add(this.txtSourceBillNo);

           //#####Currency_ID###Int64
//属性测试175Currency_ID
this.lblCurrency_ID.AutoSize = true;
this.lblCurrency_ID.Location = new System.Drawing.Point(100,175);
this.lblCurrency_ID.Name = "lblCurrency_ID";
this.lblCurrency_ID.Size = new System.Drawing.Size(41, 12);
this.lblCurrency_ID.TabIndex = 7;
this.lblCurrency_ID.Text = "币别";
//111======175
this.cmbCurrency_ID.Location = new System.Drawing.Point(173,171);
this.cmbCurrency_ID.Name ="cmbCurrency_ID";
this.cmbCurrency_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCurrency_ID.TabIndex = 7;
this.Controls.Add(this.lblCurrency_ID);
this.Controls.Add(this.cmbCurrency_ID);

           //#####400AdjustReason###String
this.lblAdjustReason.AutoSize = true;
this.lblAdjustReason.Location = new System.Drawing.Point(100,200);
this.lblAdjustReason.Name = "lblAdjustReason";
this.lblAdjustReason.Size = new System.Drawing.Size(41, 12);
this.lblAdjustReason.TabIndex = 8;
this.lblAdjustReason.Text = "调整原因";
this.txtAdjustReason.Location = new System.Drawing.Point(173,196);
this.txtAdjustReason.Name = "txtAdjustReason";
this.txtAdjustReason.Size = new System.Drawing.Size(100, 21);
this.txtAdjustReason.TabIndex = 8;
this.Controls.Add(this.lblAdjustReason);
this.Controls.Add(this.txtAdjustReason);

           //#####ExchangeRate###Decimal
this.lblExchangeRate.AutoSize = true;
this.lblExchangeRate.Location = new System.Drawing.Point(100,225);
this.lblExchangeRate.Name = "lblExchangeRate";
this.lblExchangeRate.Size = new System.Drawing.Size(41, 12);
this.lblExchangeRate.TabIndex = 9;
this.lblExchangeRate.Text = "汇率";
//111======225
this.txtExchangeRate.Location = new System.Drawing.Point(173,221);
this.txtExchangeRate.Name ="txtExchangeRate";
this.txtExchangeRate.Size = new System.Drawing.Size(100, 21);
this.txtExchangeRate.TabIndex = 9;
this.Controls.Add(this.lblExchangeRate);
this.Controls.Add(this.txtExchangeRate);

           //#####AdjustDate###DateTime
this.lblAdjustDate.AutoSize = true;
this.lblAdjustDate.Location = new System.Drawing.Point(100,250);
this.lblAdjustDate.Name = "lblAdjustDate";
this.lblAdjustDate.Size = new System.Drawing.Size(41, 12);
this.lblAdjustDate.TabIndex = 10;
this.lblAdjustDate.Text = "调整日期";
//111======250
this.dtpAdjustDate.Location = new System.Drawing.Point(173,246);
this.dtpAdjustDate.Name ="dtpAdjustDate";
this.dtpAdjustDate.Size = new System.Drawing.Size(100, 21);
this.dtpAdjustDate.TabIndex = 10;
this.Controls.Add(this.lblAdjustDate);
this.Controls.Add(this.dtpAdjustDate);

           //#####DepartmentID###Int64
//属性测试275DepartmentID
//属性测试275DepartmentID
//属性测试275DepartmentID
this.lblDepartmentID.AutoSize = true;
this.lblDepartmentID.Location = new System.Drawing.Point(100,275);
this.lblDepartmentID.Name = "lblDepartmentID";
this.lblDepartmentID.Size = new System.Drawing.Size(41, 12);
this.lblDepartmentID.TabIndex = 11;
this.lblDepartmentID.Text = "部门";
//111======275
this.cmbDepartmentID.Location = new System.Drawing.Point(173,271);
this.cmbDepartmentID.Name ="cmbDepartmentID";
this.cmbDepartmentID.Size = new System.Drawing.Size(100, 21);
this.cmbDepartmentID.TabIndex = 11;
this.Controls.Add(this.lblDepartmentID);
this.Controls.Add(this.cmbDepartmentID);

           //#####ProjectGroup_ID###Int64
//属性测试300ProjectGroup_ID
//属性测试300ProjectGroup_ID
//属性测试300ProjectGroup_ID
//属性测试300ProjectGroup_ID
//属性测试300ProjectGroup_ID
this.lblProjectGroup_ID.AutoSize = true;
this.lblProjectGroup_ID.Location = new System.Drawing.Point(100,300);
this.lblProjectGroup_ID.Name = "lblProjectGroup_ID";
this.lblProjectGroup_ID.Size = new System.Drawing.Size(41, 12);
this.lblProjectGroup_ID.TabIndex = 12;
this.lblProjectGroup_ID.Text = "项目组";
//111======300
this.cmbProjectGroup_ID.Location = new System.Drawing.Point(173,296);
this.cmbProjectGroup_ID.Name ="cmbProjectGroup_ID";
this.cmbProjectGroup_ID.Size = new System.Drawing.Size(100, 21);
this.cmbProjectGroup_ID.TabIndex = 12;
this.Controls.Add(this.lblProjectGroup_ID);
this.Controls.Add(this.cmbProjectGroup_ID);

           //#####Employee_ID###Int64
//属性测试325Employee_ID
//属性测试325Employee_ID
//属性测试325Employee_ID
//属性测试325Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,325);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 13;
this.lblEmployee_ID.Text = "经办人";
//111======325
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,321);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 13;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####InvoiceId###Int64
//属性测试350InvoiceId
//属性测试350InvoiceId
//属性测试350InvoiceId
//属性测试350InvoiceId
//属性测试350InvoiceId
this.lblInvoiceId.AutoSize = true;
this.lblInvoiceId.Location = new System.Drawing.Point(100,350);
this.lblInvoiceId.Name = "lblInvoiceId";
this.lblInvoiceId.Size = new System.Drawing.Size(41, 12);
this.lblInvoiceId.TabIndex = 14;
this.lblInvoiceId.Text = "发票";
this.txtInvoiceId.Location = new System.Drawing.Point(173,346);
this.txtInvoiceId.Name = "txtInvoiceId";
this.txtInvoiceId.Size = new System.Drawing.Size(100, 21);
this.txtInvoiceId.TabIndex = 14;
this.Controls.Add(this.lblInvoiceId);
this.Controls.Add(this.txtInvoiceId);

           //#####Invoiced###Boolean
this.lblInvoiced.AutoSize = true;
this.lblInvoiced.Location = new System.Drawing.Point(100,375);
this.lblInvoiced.Name = "lblInvoiced";
this.lblInvoiced.Size = new System.Drawing.Size(41, 12);
this.lblInvoiced.TabIndex = 15;
this.lblInvoiced.Text = "已开票";
this.chkInvoiced.Location = new System.Drawing.Point(173,371);
this.chkInvoiced.Name = "chkInvoiced";
this.chkInvoiced.Size = new System.Drawing.Size(100, 21);
this.chkInvoiced.TabIndex = 15;
this.Controls.Add(this.lblInvoiced);
this.Controls.Add(this.chkInvoiced);

           //#####TotalForeignDiffAmount###Decimal
this.lblTotalForeignDiffAmount.AutoSize = true;
this.lblTotalForeignDiffAmount.Location = new System.Drawing.Point(100,400);
this.lblTotalForeignDiffAmount.Name = "lblTotalForeignDiffAmount";
this.lblTotalForeignDiffAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalForeignDiffAmount.TabIndex = 16;
this.lblTotalForeignDiffAmount.Text = "金额差总计外币";
//111======400
this.txtTotalForeignDiffAmount.Location = new System.Drawing.Point(173,396);
this.txtTotalForeignDiffAmount.Name ="txtTotalForeignDiffAmount";
this.txtTotalForeignDiffAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalForeignDiffAmount.TabIndex = 16;
this.Controls.Add(this.lblTotalForeignDiffAmount);
this.Controls.Add(this.txtTotalForeignDiffAmount);

           //#####TotalLocalDiffAmount###Decimal
this.lblTotalLocalDiffAmount.AutoSize = true;
this.lblTotalLocalDiffAmount.Location = new System.Drawing.Point(100,425);
this.lblTotalLocalDiffAmount.Name = "lblTotalLocalDiffAmount";
this.lblTotalLocalDiffAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalLocalDiffAmount.TabIndex = 17;
this.lblTotalLocalDiffAmount.Text = "金额差总计本币";
//111======425
this.txtTotalLocalDiffAmount.Location = new System.Drawing.Point(173,421);
this.txtTotalLocalDiffAmount.Name ="txtTotalLocalDiffAmount";
this.txtTotalLocalDiffAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalLocalDiffAmount.TabIndex = 17;
this.Controls.Add(this.lblTotalLocalDiffAmount);
this.Controls.Add(this.txtTotalLocalDiffAmount);

           //#####IsIncludeTax###Boolean
this.lblIsIncludeTax.AutoSize = true;
this.lblIsIncludeTax.Location = new System.Drawing.Point(100,450);
this.lblIsIncludeTax.Name = "lblIsIncludeTax";
this.lblIsIncludeTax.Size = new System.Drawing.Size(41, 12);
this.lblIsIncludeTax.TabIndex = 18;
this.lblIsIncludeTax.Text = "含税";
this.chkIsIncludeTax.Location = new System.Drawing.Point(173,446);
this.chkIsIncludeTax.Name = "chkIsIncludeTax";
this.chkIsIncludeTax.Size = new System.Drawing.Size(100, 21);
this.chkIsIncludeTax.TabIndex = 18;
this.Controls.Add(this.lblIsIncludeTax);
this.Controls.Add(this.chkIsIncludeTax);

           //#####TaxTotalDiffLocalAmount###Decimal
this.lblTaxTotalDiffLocalAmount.AutoSize = true;
this.lblTaxTotalDiffLocalAmount.Location = new System.Drawing.Point(100,475);
this.lblTaxTotalDiffLocalAmount.Name = "lblTaxTotalDiffLocalAmount";
this.lblTaxTotalDiffLocalAmount.Size = new System.Drawing.Size(41, 12);
this.lblTaxTotalDiffLocalAmount.TabIndex = 19;
this.lblTaxTotalDiffLocalAmount.Text = "税额差总计";
//111======475
this.txtTaxTotalDiffLocalAmount.Location = new System.Drawing.Point(173,471);
this.txtTaxTotalDiffLocalAmount.Name ="txtTaxTotalDiffLocalAmount";
this.txtTaxTotalDiffLocalAmount.Size = new System.Drawing.Size(100, 21);
this.txtTaxTotalDiffLocalAmount.TabIndex = 19;
this.Controls.Add(this.lblTaxTotalDiffLocalAmount);
this.Controls.Add(this.txtTaxTotalDiffLocalAmount);

           //#####DataStatus###Int32
//属性测试500DataStatus
//属性测试500DataStatus
//属性测试500DataStatus
//属性测试500DataStatus
//属性测试500DataStatus
this.lblDataStatus.AutoSize = true;
this.lblDataStatus.Location = new System.Drawing.Point(100,500);
this.lblDataStatus.Name = "lblDataStatus";
this.lblDataStatus.Size = new System.Drawing.Size(41, 12);
this.lblDataStatus.TabIndex = 20;
this.lblDataStatus.Text = "数据状态";
this.txtDataStatus.Location = new System.Drawing.Point(173,496);
this.txtDataStatus.Name = "txtDataStatus";
this.txtDataStatus.Size = new System.Drawing.Size(100, 21);
this.txtDataStatus.TabIndex = 20;
this.Controls.Add(this.lblDataStatus);
this.Controls.Add(this.txtDataStatus);

           //#####300Remark###String
this.lblRemark.AutoSize = true;
this.lblRemark.Location = new System.Drawing.Point(100,525);
this.lblRemark.Name = "lblRemark";
this.lblRemark.Size = new System.Drawing.Size(41, 12);
this.lblRemark.TabIndex = 21;
this.lblRemark.Text = "备注";
this.txtRemark.Location = new System.Drawing.Point(173,521);
this.txtRemark.Name = "txtRemark";
this.txtRemark.Size = new System.Drawing.Size(100, 21);
this.txtRemark.TabIndex = 21;
this.Controls.Add(this.lblRemark);
this.Controls.Add(this.txtRemark);

           //#####255ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,550);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 22;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,546);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 22;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####Approver_by###Int64
//属性测试575Approver_by
//属性测试575Approver_by
//属性测试575Approver_by
//属性测试575Approver_by
//属性测试575Approver_by
this.lblApprover_by.AutoSize = true;
this.lblApprover_by.Location = new System.Drawing.Point(100,575);
this.lblApprover_by.Name = "lblApprover_by";
this.lblApprover_by.Size = new System.Drawing.Size(41, 12);
this.lblApprover_by.TabIndex = 23;
this.lblApprover_by.Text = "审批人";
this.txtApprover_by.Location = new System.Drawing.Point(173,571);
this.txtApprover_by.Name = "txtApprover_by";
this.txtApprover_by.Size = new System.Drawing.Size(100, 21);
this.txtApprover_by.TabIndex = 23;
this.Controls.Add(this.lblApprover_by);
this.Controls.Add(this.txtApprover_by);

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,600);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 24;
this.lblApprover_at.Text = "审批时间";
//111======600
this.dtpApprover_at.Location = new System.Drawing.Point(173,596);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 24;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,650);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 26;
this.lblApprovalResults.Text = "审批结果";
this.chkApprovalResults.Location = new System.Drawing.Point(173,646);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 26;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####PrintStatus###Int32
//属性测试675PrintStatus
//属性测试675PrintStatus
//属性测试675PrintStatus
//属性测试675PrintStatus
//属性测试675PrintStatus
this.lblPrintStatus.AutoSize = true;
this.lblPrintStatus.Location = new System.Drawing.Point(100,675);
this.lblPrintStatus.Name = "lblPrintStatus";
this.lblPrintStatus.Size = new System.Drawing.Size(41, 12);
this.lblPrintStatus.TabIndex = 27;
this.lblPrintStatus.Text = "打印状态";
this.txtPrintStatus.Location = new System.Drawing.Point(173,671);
this.txtPrintStatus.Name = "txtPrintStatus";
this.txtPrintStatus.Size = new System.Drawing.Size(100, 21);
this.txtPrintStatus.TabIndex = 27;
this.Controls.Add(this.lblPrintStatus);
this.Controls.Add(this.txtPrintStatus);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,700);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 28;
this.lblCreated_at.Text = "创建时间";
//111======700
this.dtpCreated_at.Location = new System.Drawing.Point(173,696);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 28;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试725Created_by
//属性测试725Created_by
//属性测试725Created_by
//属性测试725Created_by
//属性测试725Created_by
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,725);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 29;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,721);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 29;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,750);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 30;
this.lblModified_at.Text = "修改时间";
//111======750
this.dtpModified_at.Location = new System.Drawing.Point(173,746);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 30;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试775Modified_by
//属性测试775Modified_by
//属性测试775Modified_by
//属性测试775Modified_by
//属性测试775Modified_by
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,775);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 31;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,771);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 31;
this.Controls.Add(this.lblModified_by);
this.Controls.Add(this.txtModified_by);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,800);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 32;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,796);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 32;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

        //for 加入到容器
            //components = new System.ComponentModel.Container();
           
            //this.Controls.Add(this.btnCancel);
            //this.Controls.Add(this.btnOk);
            // 
            // kryptonPanel1
            // 
          //  this.kryptonPanel1.Controls.Add(this.btnCancel);
         //   this.kryptonPanel1.Controls.Add(this.btnOk);
           // this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
           // this.kryptonPanel1.Name = "kryptonPanel1";
           // this.kryptonPanel1.Size = new System.Drawing.Size(404, 300);
           // this.kryptonPanel1.TabIndex = 32;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblAdjustNo );
this.Controls.Add(this.txtAdjustNo );

                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.cmbCustomerVendor_ID );

                this.Controls.Add(this.lblReceivePaymentType );
this.Controls.Add(this.txtReceivePaymentType );

                this.Controls.Add(this.lblSourceBizType );
this.Controls.Add(this.txtSourceBizType );

                this.Controls.Add(this.lblSourceBillId );
this.Controls.Add(this.txtSourceBillId );

                this.Controls.Add(this.lblSourceBillNo );
this.Controls.Add(this.txtSourceBillNo );

                this.Controls.Add(this.lblCurrency_ID );
this.Controls.Add(this.cmbCurrency_ID );

                this.Controls.Add(this.lblAdjustReason );
this.Controls.Add(this.txtAdjustReason );

                this.Controls.Add(this.lblExchangeRate );
this.Controls.Add(this.txtExchangeRate );

                this.Controls.Add(this.lblAdjustDate );
this.Controls.Add(this.dtpAdjustDate );

                this.Controls.Add(this.lblDepartmentID );
this.Controls.Add(this.cmbDepartmentID );

                this.Controls.Add(this.lblProjectGroup_ID );
this.Controls.Add(this.cmbProjectGroup_ID );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblInvoiceId );
this.Controls.Add(this.txtInvoiceId );

                this.Controls.Add(this.lblInvoiced );
this.Controls.Add(this.chkInvoiced );

                this.Controls.Add(this.lblTotalForeignDiffAmount );
this.Controls.Add(this.txtTotalForeignDiffAmount );

                this.Controls.Add(this.lblTotalLocalDiffAmount );
this.Controls.Add(this.txtTotalLocalDiffAmount );

                this.Controls.Add(this.lblIsIncludeTax );
this.Controls.Add(this.chkIsIncludeTax );

                this.Controls.Add(this.lblTaxTotalDiffLocalAmount );
this.Controls.Add(this.txtTaxTotalDiffLocalAmount );

                this.Controls.Add(this.lblDataStatus );
this.Controls.Add(this.txtDataStatus );

                this.Controls.Add(this.lblRemark );
this.Controls.Add(this.txtRemark );

                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                this.Controls.Add(this.lblApprover_by );
this.Controls.Add(this.txtApprover_by );

                this.Controls.Add(this.lblApprover_at );
this.Controls.Add(this.dtpApprover_at );

                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                this.Controls.Add(this.lblPrintStatus );
this.Controls.Add(this.txtPrintStatus );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                this.Controls.Add(this.lblModified_by );
this.Controls.Add(this.txtModified_by );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                            // 
            // "tb_FM_PriceAdjustmentEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_FM_PriceAdjustmentEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblAdjustNo;
private Krypton.Toolkit.KryptonTextBox txtAdjustNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblReceivePaymentType;
private Krypton.Toolkit.KryptonTextBox txtReceivePaymentType;

    
        
              private Krypton.Toolkit.KryptonLabel lblSourceBizType;
private Krypton.Toolkit.KryptonTextBox txtSourceBizType;

    
        
              private Krypton.Toolkit.KryptonLabel lblSourceBillId;
private Krypton.Toolkit.KryptonTextBox txtSourceBillId;

    
        
              private Krypton.Toolkit.KryptonLabel lblSourceBillNo;
private Krypton.Toolkit.KryptonTextBox txtSourceBillNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblCurrency_ID;
private Krypton.Toolkit.KryptonComboBox cmbCurrency_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblAdjustReason;
private Krypton.Toolkit.KryptonTextBox txtAdjustReason;

    
        
              private Krypton.Toolkit.KryptonLabel lblExchangeRate;
private Krypton.Toolkit.KryptonTextBox txtExchangeRate;

    
        
              private Krypton.Toolkit.KryptonLabel lblAdjustDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpAdjustDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblDepartmentID;
private Krypton.Toolkit.KryptonComboBox cmbDepartmentID;

    
        
              private Krypton.Toolkit.KryptonLabel lblProjectGroup_ID;
private Krypton.Toolkit.KryptonComboBox cmbProjectGroup_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblInvoiceId;
private Krypton.Toolkit.KryptonTextBox txtInvoiceId;

    
        
              private Krypton.Toolkit.KryptonLabel lblInvoiced;
private Krypton.Toolkit.KryptonCheckBox chkInvoiced;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalForeignDiffAmount;
private Krypton.Toolkit.KryptonTextBox txtTotalForeignDiffAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalLocalDiffAmount;
private Krypton.Toolkit.KryptonTextBox txtTotalLocalDiffAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsIncludeTax;
private Krypton.Toolkit.KryptonCheckBox chkIsIncludeTax;

    
        
              private Krypton.Toolkit.KryptonLabel lblTaxTotalDiffLocalAmount;
private Krypton.Toolkit.KryptonTextBox txtTaxTotalDiffLocalAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblDataStatus;
private Krypton.Toolkit.KryptonTextBox txtDataStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblRemark;
private Krypton.Toolkit.KryptonTextBox txtRemark;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprover_by;
private Krypton.Toolkit.KryptonTextBox txtApprover_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprover_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;

    
        
              
    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalResults;
private Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              private Krypton.Toolkit.KryptonLabel lblPrintStatus;
private Krypton.Toolkit.KryptonTextBox txtPrintStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_by;
private Krypton.Toolkit.KryptonTextBox txtCreated_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_by;
private Krypton.Toolkit.KryptonTextBox txtModified_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblisdeleted;
private Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

