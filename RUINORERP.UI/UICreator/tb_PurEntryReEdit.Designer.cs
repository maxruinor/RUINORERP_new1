// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:05
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 采购入库单 供应商接到采购订单后，向企业发货，用户在收到货物时，可以先检验，对合格品进行入库，也可以直接入库，形成采购入库单。为了保证清楚地记录进货情况，对进货的管理就很重要，而在我们的系统中，凭证、收付款是根据进货单自动一环扣一环地切制，故详细输入进货单资料后，存货的数量、成本会随着改变，收付帐款也会跟着你的立帐方式变化；凭证亦会随着“您是否立即产生凭证”变化。采购入库单可以由采购订单、借入单、在途物资单转入，也可以手动录入新增单据。
    /// </summary>
    partial class tb_PurEntryReEdit
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
     this.lblPurEntryReNo = new Krypton.Toolkit.KryptonLabel();
this.txtPurEntryReNo = new Krypton.Toolkit.KryptonTextBox();

this.lblCustomerVendor_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbCustomerVendor_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblDepartmentID = new Krypton.Toolkit.KryptonLabel();
this.cmbDepartmentID = new Krypton.Toolkit.KryptonComboBox();

this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblPaytype_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbPaytype_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblPurEntryID = new Krypton.Toolkit.KryptonLabel();
this.cmbPurEntryID = new Krypton.Toolkit.KryptonComboBox();

this.lblPurEntryNo = new Krypton.Toolkit.KryptonLabel();
this.txtPurEntryNo = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalQty = new Krypton.Toolkit.KryptonLabel();
this.txtTotalQty = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalTaxAmount = new Krypton.Toolkit.KryptonLabel();
this.txtTotalTaxAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalAmount = new Krypton.Toolkit.KryptonLabel();
this.txtTotalAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblActualAmount = new Krypton.Toolkit.KryptonLabel();
this.txtActualAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblProcessWay = new Krypton.Toolkit.KryptonLabel();
this.txtProcessWay = new Krypton.Toolkit.KryptonTextBox();

this.lblReturnDate = new Krypton.Toolkit.KryptonLabel();
this.dtpReturnDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblShippingWay = new Krypton.Toolkit.KryptonLabel();
this.txtShippingWay = new Krypton.Toolkit.KryptonTextBox();

this.lblTrackNo = new Krypton.Toolkit.KryptonLabel();
this.txtTrackNo = new Krypton.Toolkit.KryptonTextBox();

this.lblIsCustomizedOrder = new Krypton.Toolkit.KryptonLabel();
this.chkIsCustomizedOrder = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsCustomizedOrder.Values.Text ="";

this.lblPayStatus = new Krypton.Toolkit.KryptonLabel();
this.txtPayStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblCurrency_ID = new Krypton.Toolkit.KryptonLabel();
this.txtCurrency_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblExchangeRate = new Krypton.Toolkit.KryptonLabel();
this.txtExchangeRate = new Krypton.Toolkit.KryptonTextBox();

this.lblForeignTotalAmount = new Krypton.Toolkit.KryptonLabel();
this.txtForeignTotalAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblisdeleted = new Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

this.lblCreated_at = new Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblCreated_by = new Krypton.Toolkit.KryptonLabel();
this.txtCreated_by = new Krypton.Toolkit.KryptonTextBox();

this.lblModified_at = new Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblModified_by = new Krypton.Toolkit.KryptonLabel();
this.txtModified_by = new Krypton.Toolkit.KryptonTextBox();

this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblApprovalOpinions = new Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new Krypton.Toolkit.KryptonTextBox();


this.lblApprovalResults = new Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";

this.lblIsIncludeTax = new Krypton.Toolkit.KryptonLabel();
this.chkIsIncludeTax = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsIncludeTax.Values.Text ="";

this.lblKeepAccountsType = new Krypton.Toolkit.KryptonLabel();
this.txtKeepAccountsType = new Krypton.Toolkit.KryptonTextBox();

this.lblDeposit = new Krypton.Toolkit.KryptonLabel();
this.txtDeposit = new Krypton.Toolkit.KryptonTextBox();

this.lblTaxDeductionType = new Krypton.Toolkit.KryptonLabel();
this.txtTaxDeductionType = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalDiscountAmount = new Krypton.Toolkit.KryptonLabel();
this.txtTotalDiscountAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblReceiptInvoiceClosed = new Krypton.Toolkit.KryptonLabel();
this.chkReceiptInvoiceClosed = new Krypton.Toolkit.KryptonCheckBox();
this.chkReceiptInvoiceClosed.Values.Text ="";

this.lblDataStatus = new Krypton.Toolkit.KryptonLabel();
this.txtDataStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblApprover_by = new Krypton.Toolkit.KryptonLabel();
this.txtApprover_by = new Krypton.Toolkit.KryptonTextBox();

this.lblApprover_at = new Krypton.Toolkit.KryptonLabel();
this.dtpApprover_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblPrintStatus = new Krypton.Toolkit.KryptonLabel();
this.txtPrintStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblGenerateVouchers = new Krypton.Toolkit.KryptonLabel();
this.chkGenerateVouchers = new Krypton.Toolkit.KryptonCheckBox();
this.chkGenerateVouchers.Values.Text ="";

this.lblVoucherNO = new Krypton.Toolkit.KryptonLabel();
this.txtVoucherNO = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####50PurEntryReNo###String
this.lblPurEntryReNo.AutoSize = true;
this.lblPurEntryReNo.Location = new System.Drawing.Point(100,25);
this.lblPurEntryReNo.Name = "lblPurEntryReNo";
this.lblPurEntryReNo.Size = new System.Drawing.Size(41, 12);
this.lblPurEntryReNo.TabIndex = 1;
this.lblPurEntryReNo.Text = "退回单号";
this.txtPurEntryReNo.Location = new System.Drawing.Point(173,21);
this.txtPurEntryReNo.Name = "txtPurEntryReNo";
this.txtPurEntryReNo.Size = new System.Drawing.Size(100, 21);
this.txtPurEntryReNo.TabIndex = 1;
this.Controls.Add(this.lblPurEntryReNo);
this.Controls.Add(this.txtPurEntryReNo);

           //#####CustomerVendor_ID###Int64
//属性测试50CustomerVendor_ID
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,50);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 2;
this.lblCustomerVendor_ID.Text = "供应商";
//111======50
this.cmbCustomerVendor_ID.Location = new System.Drawing.Point(173,46);
this.cmbCustomerVendor_ID.Name ="cmbCustomerVendor_ID";
this.cmbCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCustomerVendor_ID.TabIndex = 2;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.cmbCustomerVendor_ID);

           //#####DepartmentID###Int64
//属性测试75DepartmentID
//属性测试75DepartmentID
//属性测试75DepartmentID
//属性测试75DepartmentID
this.lblDepartmentID.AutoSize = true;
this.lblDepartmentID.Location = new System.Drawing.Point(100,75);
this.lblDepartmentID.Name = "lblDepartmentID";
this.lblDepartmentID.Size = new System.Drawing.Size(41, 12);
this.lblDepartmentID.TabIndex = 3;
this.lblDepartmentID.Text = "部门";
//111======75
this.cmbDepartmentID.Location = new System.Drawing.Point(173,71);
this.cmbDepartmentID.Name ="cmbDepartmentID";
this.cmbDepartmentID.Size = new System.Drawing.Size(100, 21);
this.cmbDepartmentID.TabIndex = 3;
this.Controls.Add(this.lblDepartmentID);
this.Controls.Add(this.cmbDepartmentID);

           //#####Employee_ID###Int64
//属性测试100Employee_ID
//属性测试100Employee_ID
//属性测试100Employee_ID
//属性测试100Employee_ID
//属性测试100Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,100);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 4;
this.lblEmployee_ID.Text = "经办人";
//111======100
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,96);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 4;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####Paytype_ID###Int64
//属性测试125Paytype_ID
//属性测试125Paytype_ID
//属性测试125Paytype_ID
this.lblPaytype_ID.AutoSize = true;
this.lblPaytype_ID.Location = new System.Drawing.Point(100,125);
this.lblPaytype_ID.Name = "lblPaytype_ID";
this.lblPaytype_ID.Size = new System.Drawing.Size(41, 12);
this.lblPaytype_ID.TabIndex = 5;
this.lblPaytype_ID.Text = "付款类型";
//111======125
this.cmbPaytype_ID.Location = new System.Drawing.Point(173,121);
this.cmbPaytype_ID.Name ="cmbPaytype_ID";
this.cmbPaytype_ID.Size = new System.Drawing.Size(100, 21);
this.cmbPaytype_ID.TabIndex = 5;
this.Controls.Add(this.lblPaytype_ID);
this.Controls.Add(this.cmbPaytype_ID);

           //#####PurEntryID###Int64
//属性测试150PurEntryID
//属性测试150PurEntryID
this.lblPurEntryID.AutoSize = true;
this.lblPurEntryID.Location = new System.Drawing.Point(100,150);
this.lblPurEntryID.Name = "lblPurEntryID";
this.lblPurEntryID.Size = new System.Drawing.Size(41, 12);
this.lblPurEntryID.TabIndex = 6;
this.lblPurEntryID.Text = "采购入库单";
//111======150
this.cmbPurEntryID.Location = new System.Drawing.Point(173,146);
this.cmbPurEntryID.Name ="cmbPurEntryID";
this.cmbPurEntryID.Size = new System.Drawing.Size(100, 21);
this.cmbPurEntryID.TabIndex = 6;
this.Controls.Add(this.lblPurEntryID);
this.Controls.Add(this.cmbPurEntryID);

           //#####50PurEntryNo###String
this.lblPurEntryNo.AutoSize = true;
this.lblPurEntryNo.Location = new System.Drawing.Point(100,175);
this.lblPurEntryNo.Name = "lblPurEntryNo";
this.lblPurEntryNo.Size = new System.Drawing.Size(41, 12);
this.lblPurEntryNo.TabIndex = 7;
this.lblPurEntryNo.Text = "入库单号";
this.txtPurEntryNo.Location = new System.Drawing.Point(173,171);
this.txtPurEntryNo.Name = "txtPurEntryNo";
this.txtPurEntryNo.Size = new System.Drawing.Size(100, 21);
this.txtPurEntryNo.TabIndex = 7;
this.Controls.Add(this.lblPurEntryNo);
this.Controls.Add(this.txtPurEntryNo);

           //#####TotalQty###Int32
//属性测试200TotalQty
//属性测试200TotalQty
//属性测试200TotalQty
//属性测试200TotalQty
//属性测试200TotalQty
this.lblTotalQty.AutoSize = true;
this.lblTotalQty.Location = new System.Drawing.Point(100,200);
this.lblTotalQty.Name = "lblTotalQty";
this.lblTotalQty.Size = new System.Drawing.Size(41, 12);
this.lblTotalQty.TabIndex = 8;
this.lblTotalQty.Text = "合计数量";
this.txtTotalQty.Location = new System.Drawing.Point(173,196);
this.txtTotalQty.Name = "txtTotalQty";
this.txtTotalQty.Size = new System.Drawing.Size(100, 21);
this.txtTotalQty.TabIndex = 8;
this.Controls.Add(this.lblTotalQty);
this.Controls.Add(this.txtTotalQty);

           //#####TotalTaxAmount###Decimal
this.lblTotalTaxAmount.AutoSize = true;
this.lblTotalTaxAmount.Location = new System.Drawing.Point(100,225);
this.lblTotalTaxAmount.Name = "lblTotalTaxAmount";
this.lblTotalTaxAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalTaxAmount.TabIndex = 9;
this.lblTotalTaxAmount.Text = "合计税额";
//111======225
this.txtTotalTaxAmount.Location = new System.Drawing.Point(173,221);
this.txtTotalTaxAmount.Name ="txtTotalTaxAmount";
this.txtTotalTaxAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalTaxAmount.TabIndex = 9;
this.Controls.Add(this.lblTotalTaxAmount);
this.Controls.Add(this.txtTotalTaxAmount);

           //#####TotalAmount###Decimal
this.lblTotalAmount.AutoSize = true;
this.lblTotalAmount.Location = new System.Drawing.Point(100,250);
this.lblTotalAmount.Name = "lblTotalAmount";
this.lblTotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalAmount.TabIndex = 10;
this.lblTotalAmount.Text = "合计金额";
//111======250
this.txtTotalAmount.Location = new System.Drawing.Point(173,246);
this.txtTotalAmount.Name ="txtTotalAmount";
this.txtTotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalAmount.TabIndex = 10;
this.Controls.Add(this.lblTotalAmount);
this.Controls.Add(this.txtTotalAmount);

           //#####ActualAmount###Decimal
this.lblActualAmount.AutoSize = true;
this.lblActualAmount.Location = new System.Drawing.Point(100,275);
this.lblActualAmount.Name = "lblActualAmount";
this.lblActualAmount.Size = new System.Drawing.Size(41, 12);
this.lblActualAmount.TabIndex = 11;
this.lblActualAmount.Text = "实退金额";
//111======275
this.txtActualAmount.Location = new System.Drawing.Point(173,271);
this.txtActualAmount.Name ="txtActualAmount";
this.txtActualAmount.Size = new System.Drawing.Size(100, 21);
this.txtActualAmount.TabIndex = 11;
this.Controls.Add(this.lblActualAmount);
this.Controls.Add(this.txtActualAmount);

           //#####ProcessWay###Int32
//属性测试300ProcessWay
//属性测试300ProcessWay
//属性测试300ProcessWay
//属性测试300ProcessWay
//属性测试300ProcessWay
this.lblProcessWay.AutoSize = true;
this.lblProcessWay.Location = new System.Drawing.Point(100,300);
this.lblProcessWay.Name = "lblProcessWay";
this.lblProcessWay.Size = new System.Drawing.Size(41, 12);
this.lblProcessWay.TabIndex = 12;
this.lblProcessWay.Text = "处理方式";
this.txtProcessWay.Location = new System.Drawing.Point(173,296);
this.txtProcessWay.Name = "txtProcessWay";
this.txtProcessWay.Size = new System.Drawing.Size(100, 21);
this.txtProcessWay.TabIndex = 12;
this.Controls.Add(this.lblProcessWay);
this.Controls.Add(this.txtProcessWay);

           //#####ReturnDate###DateTime
this.lblReturnDate.AutoSize = true;
this.lblReturnDate.Location = new System.Drawing.Point(100,325);
this.lblReturnDate.Name = "lblReturnDate";
this.lblReturnDate.Size = new System.Drawing.Size(41, 12);
this.lblReturnDate.TabIndex = 13;
this.lblReturnDate.Text = "退回日期";
//111======325
this.dtpReturnDate.Location = new System.Drawing.Point(173,321);
this.dtpReturnDate.Name ="dtpReturnDate";
this.dtpReturnDate.Size = new System.Drawing.Size(100, 21);
this.dtpReturnDate.TabIndex = 13;
this.Controls.Add(this.lblReturnDate);
this.Controls.Add(this.dtpReturnDate);

           //#####50ShippingWay###String
this.lblShippingWay.AutoSize = true;
this.lblShippingWay.Location = new System.Drawing.Point(100,350);
this.lblShippingWay.Name = "lblShippingWay";
this.lblShippingWay.Size = new System.Drawing.Size(41, 12);
this.lblShippingWay.TabIndex = 14;
this.lblShippingWay.Text = "发货方式";
this.txtShippingWay.Location = new System.Drawing.Point(173,346);
this.txtShippingWay.Name = "txtShippingWay";
this.txtShippingWay.Size = new System.Drawing.Size(100, 21);
this.txtShippingWay.TabIndex = 14;
this.Controls.Add(this.lblShippingWay);
this.Controls.Add(this.txtShippingWay);

           //#####50TrackNo###String
this.lblTrackNo.AutoSize = true;
this.lblTrackNo.Location = new System.Drawing.Point(100,375);
this.lblTrackNo.Name = "lblTrackNo";
this.lblTrackNo.Size = new System.Drawing.Size(41, 12);
this.lblTrackNo.TabIndex = 15;
this.lblTrackNo.Text = "物流单号";
this.txtTrackNo.Location = new System.Drawing.Point(173,371);
this.txtTrackNo.Name = "txtTrackNo";
this.txtTrackNo.Size = new System.Drawing.Size(100, 21);
this.txtTrackNo.TabIndex = 15;
this.Controls.Add(this.lblTrackNo);
this.Controls.Add(this.txtTrackNo);

           //#####IsCustomizedOrder###Boolean
this.lblIsCustomizedOrder.AutoSize = true;
this.lblIsCustomizedOrder.Location = new System.Drawing.Point(100,400);
this.lblIsCustomizedOrder.Name = "lblIsCustomizedOrder";
this.lblIsCustomizedOrder.Size = new System.Drawing.Size(41, 12);
this.lblIsCustomizedOrder.TabIndex = 16;
this.lblIsCustomizedOrder.Text = "定制单";
this.chkIsCustomizedOrder.Location = new System.Drawing.Point(173,396);
this.chkIsCustomizedOrder.Name = "chkIsCustomizedOrder";
this.chkIsCustomizedOrder.Size = new System.Drawing.Size(100, 21);
this.chkIsCustomizedOrder.TabIndex = 16;
this.Controls.Add(this.lblIsCustomizedOrder);
this.Controls.Add(this.chkIsCustomizedOrder);

           //#####PayStatus###Int32
//属性测试425PayStatus
//属性测试425PayStatus
//属性测试425PayStatus
//属性测试425PayStatus
//属性测试425PayStatus
this.lblPayStatus.AutoSize = true;
this.lblPayStatus.Location = new System.Drawing.Point(100,425);
this.lblPayStatus.Name = "lblPayStatus";
this.lblPayStatus.Size = new System.Drawing.Size(41, 12);
this.lblPayStatus.TabIndex = 17;
this.lblPayStatus.Text = "退款状态";
this.txtPayStatus.Location = new System.Drawing.Point(173,421);
this.txtPayStatus.Name = "txtPayStatus";
this.txtPayStatus.Size = new System.Drawing.Size(100, 21);
this.txtPayStatus.TabIndex = 17;
this.Controls.Add(this.lblPayStatus);
this.Controls.Add(this.txtPayStatus);

           //#####Currency_ID###Int64
//属性测试450Currency_ID
//属性测试450Currency_ID
//属性测试450Currency_ID
//属性测试450Currency_ID
//属性测试450Currency_ID
this.lblCurrency_ID.AutoSize = true;
this.lblCurrency_ID.Location = new System.Drawing.Point(100,450);
this.lblCurrency_ID.Name = "lblCurrency_ID";
this.lblCurrency_ID.Size = new System.Drawing.Size(41, 12);
this.lblCurrency_ID.TabIndex = 18;
this.lblCurrency_ID.Text = "币别";
this.txtCurrency_ID.Location = new System.Drawing.Point(173,446);
this.txtCurrency_ID.Name = "txtCurrency_ID";
this.txtCurrency_ID.Size = new System.Drawing.Size(100, 21);
this.txtCurrency_ID.TabIndex = 18;
this.Controls.Add(this.lblCurrency_ID);
this.Controls.Add(this.txtCurrency_ID);

           //#####ExchangeRate###Decimal
this.lblExchangeRate.AutoSize = true;
this.lblExchangeRate.Location = new System.Drawing.Point(100,475);
this.lblExchangeRate.Name = "lblExchangeRate";
this.lblExchangeRate.Size = new System.Drawing.Size(41, 12);
this.lblExchangeRate.TabIndex = 19;
this.lblExchangeRate.Text = "汇率";
//111======475
this.txtExchangeRate.Location = new System.Drawing.Point(173,471);
this.txtExchangeRate.Name ="txtExchangeRate";
this.txtExchangeRate.Size = new System.Drawing.Size(100, 21);
this.txtExchangeRate.TabIndex = 19;
this.Controls.Add(this.lblExchangeRate);
this.Controls.Add(this.txtExchangeRate);

           //#####ForeignTotalAmount###Decimal
this.lblForeignTotalAmount.AutoSize = true;
this.lblForeignTotalAmount.Location = new System.Drawing.Point(100,500);
this.lblForeignTotalAmount.Name = "lblForeignTotalAmount";
this.lblForeignTotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblForeignTotalAmount.TabIndex = 20;
this.lblForeignTotalAmount.Text = "金额外币";
//111======500
this.txtForeignTotalAmount.Location = new System.Drawing.Point(173,496);
this.txtForeignTotalAmount.Name ="txtForeignTotalAmount";
this.txtForeignTotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtForeignTotalAmount.TabIndex = 20;
this.Controls.Add(this.lblForeignTotalAmount);
this.Controls.Add(this.txtForeignTotalAmount);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,525);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 21;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,521);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 21;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,550);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 22;
this.lblCreated_at.Text = "创建时间";
//111======550
this.dtpCreated_at.Location = new System.Drawing.Point(173,546);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 22;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试575Created_by
//属性测试575Created_by
//属性测试575Created_by
//属性测试575Created_by
//属性测试575Created_by
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,575);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 23;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,571);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 23;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,600);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 24;
this.lblModified_at.Text = "修改时间";
//111======600
this.dtpModified_at.Location = new System.Drawing.Point(173,596);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 24;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试625Modified_by
//属性测试625Modified_by
//属性测试625Modified_by
//属性测试625Modified_by
//属性测试625Modified_by
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,625);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 25;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,621);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 25;
this.Controls.Add(this.lblModified_by);
this.Controls.Add(this.txtModified_by);

           //#####255Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,650);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 26;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,646);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 26;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####200ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,675);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 27;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,671);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 27;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,725);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 29;
this.lblApprovalResults.Text = "审批结果";
this.chkApprovalResults.Location = new System.Drawing.Point(173,721);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 29;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####IsIncludeTax###Boolean
this.lblIsIncludeTax.AutoSize = true;
this.lblIsIncludeTax.Location = new System.Drawing.Point(100,750);
this.lblIsIncludeTax.Name = "lblIsIncludeTax";
this.lblIsIncludeTax.Size = new System.Drawing.Size(41, 12);
this.lblIsIncludeTax.TabIndex = 30;
this.lblIsIncludeTax.Text = "含税";
this.chkIsIncludeTax.Location = new System.Drawing.Point(173,746);
this.chkIsIncludeTax.Name = "chkIsIncludeTax";
this.chkIsIncludeTax.Size = new System.Drawing.Size(100, 21);
this.chkIsIncludeTax.TabIndex = 30;
this.Controls.Add(this.lblIsIncludeTax);
this.Controls.Add(this.chkIsIncludeTax);

           //#####KeepAccountsType###Int32
//属性测试775KeepAccountsType
//属性测试775KeepAccountsType
//属性测试775KeepAccountsType
//属性测试775KeepAccountsType
//属性测试775KeepAccountsType
this.lblKeepAccountsType.AutoSize = true;
this.lblKeepAccountsType.Location = new System.Drawing.Point(100,775);
this.lblKeepAccountsType.Name = "lblKeepAccountsType";
this.lblKeepAccountsType.Size = new System.Drawing.Size(41, 12);
this.lblKeepAccountsType.TabIndex = 31;
this.lblKeepAccountsType.Text = "立帐类型";
this.txtKeepAccountsType.Location = new System.Drawing.Point(173,771);
this.txtKeepAccountsType.Name = "txtKeepAccountsType";
this.txtKeepAccountsType.Size = new System.Drawing.Size(100, 21);
this.txtKeepAccountsType.TabIndex = 31;
this.Controls.Add(this.lblKeepAccountsType);
this.Controls.Add(this.txtKeepAccountsType);

           //#####Deposit###Decimal
this.lblDeposit.AutoSize = true;
this.lblDeposit.Location = new System.Drawing.Point(100,800);
this.lblDeposit.Name = "lblDeposit";
this.lblDeposit.Size = new System.Drawing.Size(41, 12);
this.lblDeposit.TabIndex = 32;
this.lblDeposit.Text = "订金";
//111======800
this.txtDeposit.Location = new System.Drawing.Point(173,796);
this.txtDeposit.Name ="txtDeposit";
this.txtDeposit.Size = new System.Drawing.Size(100, 21);
this.txtDeposit.TabIndex = 32;
this.Controls.Add(this.lblDeposit);
this.Controls.Add(this.txtDeposit);

           //#####TaxDeductionType###Int32
//属性测试825TaxDeductionType
//属性测试825TaxDeductionType
//属性测试825TaxDeductionType
//属性测试825TaxDeductionType
//属性测试825TaxDeductionType
this.lblTaxDeductionType.AutoSize = true;
this.lblTaxDeductionType.Location = new System.Drawing.Point(100,825);
this.lblTaxDeductionType.Name = "lblTaxDeductionType";
this.lblTaxDeductionType.Size = new System.Drawing.Size(41, 12);
this.lblTaxDeductionType.TabIndex = 33;
this.lblTaxDeductionType.Text = "扣税类型";
this.txtTaxDeductionType.Location = new System.Drawing.Point(173,821);
this.txtTaxDeductionType.Name = "txtTaxDeductionType";
this.txtTaxDeductionType.Size = new System.Drawing.Size(100, 21);
this.txtTaxDeductionType.TabIndex = 33;
this.Controls.Add(this.lblTaxDeductionType);
this.Controls.Add(this.txtTaxDeductionType);

           //#####TotalDiscountAmount###Decimal
this.lblTotalDiscountAmount.AutoSize = true;
this.lblTotalDiscountAmount.Location = new System.Drawing.Point(100,850);
this.lblTotalDiscountAmount.Name = "lblTotalDiscountAmount";
this.lblTotalDiscountAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalDiscountAmount.TabIndex = 34;
this.lblTotalDiscountAmount.Text = "折扣金额总计";
//111======850
this.txtTotalDiscountAmount.Location = new System.Drawing.Point(173,846);
this.txtTotalDiscountAmount.Name ="txtTotalDiscountAmount";
this.txtTotalDiscountAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalDiscountAmount.TabIndex = 34;
this.Controls.Add(this.lblTotalDiscountAmount);
this.Controls.Add(this.txtTotalDiscountAmount);

           //#####ReceiptInvoiceClosed###Boolean
this.lblReceiptInvoiceClosed.AutoSize = true;
this.lblReceiptInvoiceClosed.Location = new System.Drawing.Point(100,875);
this.lblReceiptInvoiceClosed.Name = "lblReceiptInvoiceClosed";
this.lblReceiptInvoiceClosed.Size = new System.Drawing.Size(41, 12);
this.lblReceiptInvoiceClosed.TabIndex = 35;
this.lblReceiptInvoiceClosed.Text = "立帐结案";
this.chkReceiptInvoiceClosed.Location = new System.Drawing.Point(173,871);
this.chkReceiptInvoiceClosed.Name = "chkReceiptInvoiceClosed";
this.chkReceiptInvoiceClosed.Size = new System.Drawing.Size(100, 21);
this.chkReceiptInvoiceClosed.TabIndex = 35;
this.Controls.Add(this.lblReceiptInvoiceClosed);
this.Controls.Add(this.chkReceiptInvoiceClosed);

           //#####DataStatus###Int32
//属性测试900DataStatus
//属性测试900DataStatus
//属性测试900DataStatus
//属性测试900DataStatus
//属性测试900DataStatus
this.lblDataStatus.AutoSize = true;
this.lblDataStatus.Location = new System.Drawing.Point(100,900);
this.lblDataStatus.Name = "lblDataStatus";
this.lblDataStatus.Size = new System.Drawing.Size(41, 12);
this.lblDataStatus.TabIndex = 36;
this.lblDataStatus.Text = "数据状态";
this.txtDataStatus.Location = new System.Drawing.Point(173,896);
this.txtDataStatus.Name = "txtDataStatus";
this.txtDataStatus.Size = new System.Drawing.Size(100, 21);
this.txtDataStatus.TabIndex = 36;
this.Controls.Add(this.lblDataStatus);
this.Controls.Add(this.txtDataStatus);

           //#####Approver_by###Int64
//属性测试925Approver_by
//属性测试925Approver_by
//属性测试925Approver_by
//属性测试925Approver_by
//属性测试925Approver_by
this.lblApprover_by.AutoSize = true;
this.lblApprover_by.Location = new System.Drawing.Point(100,925);
this.lblApprover_by.Name = "lblApprover_by";
this.lblApprover_by.Size = new System.Drawing.Size(41, 12);
this.lblApprover_by.TabIndex = 37;
this.lblApprover_by.Text = "审批人";
this.txtApprover_by.Location = new System.Drawing.Point(173,921);
this.txtApprover_by.Name = "txtApprover_by";
this.txtApprover_by.Size = new System.Drawing.Size(100, 21);
this.txtApprover_by.TabIndex = 37;
this.Controls.Add(this.lblApprover_by);
this.Controls.Add(this.txtApprover_by);

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,950);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 38;
this.lblApprover_at.Text = "审批时间";
//111======950
this.dtpApprover_at.Location = new System.Drawing.Point(173,946);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 38;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####PrintStatus###Int32
//属性测试975PrintStatus
//属性测试975PrintStatus
//属性测试975PrintStatus
//属性测试975PrintStatus
//属性测试975PrintStatus
this.lblPrintStatus.AutoSize = true;
this.lblPrintStatus.Location = new System.Drawing.Point(100,975);
this.lblPrintStatus.Name = "lblPrintStatus";
this.lblPrintStatus.Size = new System.Drawing.Size(41, 12);
this.lblPrintStatus.TabIndex = 39;
this.lblPrintStatus.Text = "打印状态";
this.txtPrintStatus.Location = new System.Drawing.Point(173,971);
this.txtPrintStatus.Name = "txtPrintStatus";
this.txtPrintStatus.Size = new System.Drawing.Size(100, 21);
this.txtPrintStatus.TabIndex = 39;
this.Controls.Add(this.lblPrintStatus);
this.Controls.Add(this.txtPrintStatus);

           //#####GenerateVouchers###Boolean
this.lblGenerateVouchers.AutoSize = true;
this.lblGenerateVouchers.Location = new System.Drawing.Point(100,1000);
this.lblGenerateVouchers.Name = "lblGenerateVouchers";
this.lblGenerateVouchers.Size = new System.Drawing.Size(41, 12);
this.lblGenerateVouchers.TabIndex = 40;
this.lblGenerateVouchers.Text = "生成凭证";
this.chkGenerateVouchers.Location = new System.Drawing.Point(173,996);
this.chkGenerateVouchers.Name = "chkGenerateVouchers";
this.chkGenerateVouchers.Size = new System.Drawing.Size(100, 21);
this.chkGenerateVouchers.TabIndex = 40;
this.Controls.Add(this.lblGenerateVouchers);
this.Controls.Add(this.chkGenerateVouchers);

           //#####50VoucherNO###String
this.lblVoucherNO.AutoSize = true;
this.lblVoucherNO.Location = new System.Drawing.Point(100,1025);
this.lblVoucherNO.Name = "lblVoucherNO";
this.lblVoucherNO.Size = new System.Drawing.Size(41, 12);
this.lblVoucherNO.TabIndex = 41;
this.lblVoucherNO.Text = "凭证号码";
this.txtVoucherNO.Location = new System.Drawing.Point(173,1021);
this.txtVoucherNO.Name = "txtVoucherNO";
this.txtVoucherNO.Size = new System.Drawing.Size(100, 21);
this.txtVoucherNO.TabIndex = 41;
this.Controls.Add(this.lblVoucherNO);
this.Controls.Add(this.txtVoucherNO);

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
           // this.kryptonPanel1.TabIndex = 41;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblPurEntryReNo );
this.Controls.Add(this.txtPurEntryReNo );

                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.cmbCustomerVendor_ID );

                this.Controls.Add(this.lblDepartmentID );
this.Controls.Add(this.cmbDepartmentID );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblPaytype_ID );
this.Controls.Add(this.cmbPaytype_ID );

                this.Controls.Add(this.lblPurEntryID );
this.Controls.Add(this.cmbPurEntryID );

                this.Controls.Add(this.lblPurEntryNo );
this.Controls.Add(this.txtPurEntryNo );

                this.Controls.Add(this.lblTotalQty );
this.Controls.Add(this.txtTotalQty );

                this.Controls.Add(this.lblTotalTaxAmount );
this.Controls.Add(this.txtTotalTaxAmount );

                this.Controls.Add(this.lblTotalAmount );
this.Controls.Add(this.txtTotalAmount );

                this.Controls.Add(this.lblActualAmount );
this.Controls.Add(this.txtActualAmount );

                this.Controls.Add(this.lblProcessWay );
this.Controls.Add(this.txtProcessWay );

                this.Controls.Add(this.lblReturnDate );
this.Controls.Add(this.dtpReturnDate );

                this.Controls.Add(this.lblShippingWay );
this.Controls.Add(this.txtShippingWay );

                this.Controls.Add(this.lblTrackNo );
this.Controls.Add(this.txtTrackNo );

                this.Controls.Add(this.lblIsCustomizedOrder );
this.Controls.Add(this.chkIsCustomizedOrder );

                this.Controls.Add(this.lblPayStatus );
this.Controls.Add(this.txtPayStatus );

                this.Controls.Add(this.lblCurrency_ID );
this.Controls.Add(this.txtCurrency_ID );

                this.Controls.Add(this.lblExchangeRate );
this.Controls.Add(this.txtExchangeRate );

                this.Controls.Add(this.lblForeignTotalAmount );
this.Controls.Add(this.txtForeignTotalAmount );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                this.Controls.Add(this.lblModified_by );
this.Controls.Add(this.txtModified_by );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                this.Controls.Add(this.lblIsIncludeTax );
this.Controls.Add(this.chkIsIncludeTax );

                this.Controls.Add(this.lblKeepAccountsType );
this.Controls.Add(this.txtKeepAccountsType );

                this.Controls.Add(this.lblDeposit );
this.Controls.Add(this.txtDeposit );

                this.Controls.Add(this.lblTaxDeductionType );
this.Controls.Add(this.txtTaxDeductionType );

                this.Controls.Add(this.lblTotalDiscountAmount );
this.Controls.Add(this.txtTotalDiscountAmount );

                this.Controls.Add(this.lblReceiptInvoiceClosed );
this.Controls.Add(this.chkReceiptInvoiceClosed );

                this.Controls.Add(this.lblDataStatus );
this.Controls.Add(this.txtDataStatus );

                this.Controls.Add(this.lblApprover_by );
this.Controls.Add(this.txtApprover_by );

                this.Controls.Add(this.lblApprover_at );
this.Controls.Add(this.dtpApprover_at );

                this.Controls.Add(this.lblPrintStatus );
this.Controls.Add(this.txtPrintStatus );

                this.Controls.Add(this.lblGenerateVouchers );
this.Controls.Add(this.chkGenerateVouchers );

                this.Controls.Add(this.lblVoucherNO );
this.Controls.Add(this.txtVoucherNO );

                            // 
            // "tb_PurEntryReEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_PurEntryReEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblPurEntryReNo;
private Krypton.Toolkit.KryptonTextBox txtPurEntryReNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblDepartmentID;
private Krypton.Toolkit.KryptonComboBox cmbDepartmentID;

    
        
              private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblPaytype_ID;
private Krypton.Toolkit.KryptonComboBox cmbPaytype_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblPurEntryID;
private Krypton.Toolkit.KryptonComboBox cmbPurEntryID;

    
        
              private Krypton.Toolkit.KryptonLabel lblPurEntryNo;
private Krypton.Toolkit.KryptonTextBox txtPurEntryNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalQty;
private Krypton.Toolkit.KryptonTextBox txtTotalQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalTaxAmount;
private Krypton.Toolkit.KryptonTextBox txtTotalTaxAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalAmount;
private Krypton.Toolkit.KryptonTextBox txtTotalAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblActualAmount;
private Krypton.Toolkit.KryptonTextBox txtActualAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblProcessWay;
private Krypton.Toolkit.KryptonTextBox txtProcessWay;

    
        
              private Krypton.Toolkit.KryptonLabel lblReturnDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpReturnDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblShippingWay;
private Krypton.Toolkit.KryptonTextBox txtShippingWay;

    
        
              private Krypton.Toolkit.KryptonLabel lblTrackNo;
private Krypton.Toolkit.KryptonTextBox txtTrackNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsCustomizedOrder;
private Krypton.Toolkit.KryptonCheckBox chkIsCustomizedOrder;

    
        
              private Krypton.Toolkit.KryptonLabel lblPayStatus;
private Krypton.Toolkit.KryptonTextBox txtPayStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblCurrency_ID;
private Krypton.Toolkit.KryptonTextBox txtCurrency_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblExchangeRate;
private Krypton.Toolkit.KryptonTextBox txtExchangeRate;

    
        
              private Krypton.Toolkit.KryptonLabel lblForeignTotalAmount;
private Krypton.Toolkit.KryptonTextBox txtForeignTotalAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblisdeleted;
private Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_by;
private Krypton.Toolkit.KryptonTextBox txtCreated_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_by;
private Krypton.Toolkit.KryptonTextBox txtModified_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              
    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalResults;
private Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsIncludeTax;
private Krypton.Toolkit.KryptonCheckBox chkIsIncludeTax;

    
        
              private Krypton.Toolkit.KryptonLabel lblKeepAccountsType;
private Krypton.Toolkit.KryptonTextBox txtKeepAccountsType;

    
        
              private Krypton.Toolkit.KryptonLabel lblDeposit;
private Krypton.Toolkit.KryptonTextBox txtDeposit;

    
        
              private Krypton.Toolkit.KryptonLabel lblTaxDeductionType;
private Krypton.Toolkit.KryptonTextBox txtTaxDeductionType;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalDiscountAmount;
private Krypton.Toolkit.KryptonTextBox txtTotalDiscountAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblReceiptInvoiceClosed;
private Krypton.Toolkit.KryptonCheckBox chkReceiptInvoiceClosed;

    
        
              private Krypton.Toolkit.KryptonLabel lblDataStatus;
private Krypton.Toolkit.KryptonTextBox txtDataStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprover_by;
private Krypton.Toolkit.KryptonTextBox txtApprover_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprover_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblPrintStatus;
private Krypton.Toolkit.KryptonTextBox txtPrintStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblGenerateVouchers;
private Krypton.Toolkit.KryptonCheckBox chkGenerateVouchers;

    
        
              private Krypton.Toolkit.KryptonLabel lblVoucherNO;
private Krypton.Toolkit.KryptonTextBox txtVoucherNO;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

