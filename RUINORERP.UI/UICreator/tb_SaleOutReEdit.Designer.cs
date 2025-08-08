// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:15
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 销售出库退回单
    /// </summary>
    partial class tb_SaleOutReEdit
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
     this.lblReturnNo = new Krypton.Toolkit.KryptonLabel();
this.txtReturnNo = new Krypton.Toolkit.KryptonTextBox();

this.lblProjectGroup_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbProjectGroup_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblPayStatus = new Krypton.Toolkit.KryptonLabel();
this.txtPayStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblPaytype_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbPaytype_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblCustomerVendor_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbCustomerVendor_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblSaleOut_MainID = new Krypton.Toolkit.KryptonLabel();
this.cmbSaleOut_MainID = new Krypton.Toolkit.KryptonComboBox();

this.lblSaleOut_NO = new Krypton.Toolkit.KryptonLabel();
this.txtSaleOut_NO = new Krypton.Toolkit.KryptonTextBox();

this.lblCurrency_ID = new Krypton.Toolkit.KryptonLabel();
this.txtCurrency_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblExchangeRate = new Krypton.Toolkit.KryptonLabel();
this.txtExchangeRate = new Krypton.Toolkit.KryptonTextBox();

this.lblForeignTotalAmount = new Krypton.Toolkit.KryptonLabel();
this.txtForeignTotalAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblRefundStatus = new Krypton.Toolkit.KryptonLabel();
this.txtRefundStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalQty = new Krypton.Toolkit.KryptonLabel();
this.txtTotalQty = new Krypton.Toolkit.KryptonTextBox();

this.lblActualRefundAmount = new Krypton.Toolkit.KryptonLabel();
this.txtActualRefundAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalAmount = new Krypton.Toolkit.KryptonLabel();
this.txtTotalAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblReturnDate = new Krypton.Toolkit.KryptonLabel();
this.dtpReturnDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblForeignFreightIncome = new Krypton.Toolkit.KryptonLabel();
this.txtForeignFreightIncome = new Krypton.Toolkit.KryptonTextBox();

this.lblFreightIncome = new Krypton.Toolkit.KryptonLabel();
this.txtFreightIncome = new Krypton.Toolkit.KryptonTextBox();

this.lblFreightCost = new Krypton.Toolkit.KryptonLabel();
this.txtFreightCost = new Krypton.Toolkit.KryptonTextBox();

this.lblTrackNo = new Krypton.Toolkit.KryptonLabel();
this.txtTrackNo = new Krypton.Toolkit.KryptonTextBox();

this.lblOfflineRefund = new Krypton.Toolkit.KryptonLabel();
this.chkOfflineRefund = new Krypton.Toolkit.KryptonCheckBox();
this.chkOfflineRefund.Values.Text ="";

this.lblIsFromPlatform = new Krypton.Toolkit.KryptonLabel();
this.chkIsFromPlatform = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsFromPlatform.Values.Text ="";

this.lblPlatformOrderNo = new Krypton.Toolkit.KryptonLabel();
this.txtPlatformOrderNo = new Krypton.Toolkit.KryptonTextBox();

this.lblIsCustomizedOrder = new Krypton.Toolkit.KryptonLabel();
this.chkIsCustomizedOrder = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsCustomizedOrder.Values.Text ="";

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

this.lblReturnReason = new Krypton.Toolkit.KryptonLabel();
this.txtReturnReason = new Krypton.Toolkit.KryptonTextBox();
this.txtReturnReason.Multiline = true;

this.lblTotalCommissionAmount = new Krypton.Toolkit.KryptonLabel();
this.txtTotalCommissionAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblApprovalOpinions = new Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new Krypton.Toolkit.KryptonTextBox();

this.lblApprover_by = new Krypton.Toolkit.KryptonLabel();
this.txtApprover_by = new Krypton.Toolkit.KryptonTextBox();

this.lblApprover_at = new Krypton.Toolkit.KryptonLabel();
this.dtpApprover_at = new Krypton.Toolkit.KryptonDateTimePicker();


this.lblApprovalResults = new Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";

this.lblRefundOnly = new Krypton.Toolkit.KryptonLabel();
this.chkRefundOnly = new Krypton.Toolkit.KryptonCheckBox();
this.chkRefundOnly.Values.Text ="";

this.lblIsIncludeTax = new Krypton.Toolkit.KryptonLabel();
this.chkIsIncludeTax = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsIncludeTax.Values.Text ="";

this.lblKeepAccountsType = new Krypton.Toolkit.KryptonLabel();
this.txtKeepAccountsType = new Krypton.Toolkit.KryptonTextBox();

this.lblTaxDeductionType = new Krypton.Toolkit.KryptonLabel();
this.txtTaxDeductionType = new Krypton.Toolkit.KryptonTextBox();

this.lblDataStatus = new Krypton.Toolkit.KryptonLabel();
this.txtDataStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblPrintStatus = new Krypton.Toolkit.KryptonLabel();
this.txtPrintStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblGenerateVouchers = new Krypton.Toolkit.KryptonLabel();
this.chkGenerateVouchers = new Krypton.Toolkit.KryptonCheckBox();
this.chkGenerateVouchers.Values.Text ="";

    
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
     
            //#####50ReturnNo###String
this.lblReturnNo.AutoSize = true;
this.lblReturnNo.Location = new System.Drawing.Point(100,25);
this.lblReturnNo.Name = "lblReturnNo";
this.lblReturnNo.Size = new System.Drawing.Size(41, 12);
this.lblReturnNo.TabIndex = 1;
this.lblReturnNo.Text = "退回单号";
this.txtReturnNo.Location = new System.Drawing.Point(173,21);
this.txtReturnNo.Name = "txtReturnNo";
this.txtReturnNo.Size = new System.Drawing.Size(100, 21);
this.txtReturnNo.TabIndex = 1;
this.Controls.Add(this.lblReturnNo);
this.Controls.Add(this.txtReturnNo);

           //#####ProjectGroup_ID###Int64
//属性测试50ProjectGroup_ID
//属性测试50ProjectGroup_ID
this.lblProjectGroup_ID.AutoSize = true;
this.lblProjectGroup_ID.Location = new System.Drawing.Point(100,50);
this.lblProjectGroup_ID.Name = "lblProjectGroup_ID";
this.lblProjectGroup_ID.Size = new System.Drawing.Size(41, 12);
this.lblProjectGroup_ID.TabIndex = 2;
this.lblProjectGroup_ID.Text = "项目组";
//111======50
this.cmbProjectGroup_ID.Location = new System.Drawing.Point(173,46);
this.cmbProjectGroup_ID.Name ="cmbProjectGroup_ID";
this.cmbProjectGroup_ID.Size = new System.Drawing.Size(100, 21);
this.cmbProjectGroup_ID.TabIndex = 2;
this.Controls.Add(this.lblProjectGroup_ID);
this.Controls.Add(this.cmbProjectGroup_ID);

           //#####PayStatus###Int32
//属性测试75PayStatus
//属性测试75PayStatus
//属性测试75PayStatus
//属性测试75PayStatus
//属性测试75PayStatus
this.lblPayStatus.AutoSize = true;
this.lblPayStatus.Location = new System.Drawing.Point(100,75);
this.lblPayStatus.Name = "lblPayStatus";
this.lblPayStatus.Size = new System.Drawing.Size(41, 12);
this.lblPayStatus.TabIndex = 3;
this.lblPayStatus.Text = "退款状态";
this.txtPayStatus.Location = new System.Drawing.Point(173,71);
this.txtPayStatus.Name = "txtPayStatus";
this.txtPayStatus.Size = new System.Drawing.Size(100, 21);
this.txtPayStatus.TabIndex = 3;
this.Controls.Add(this.lblPayStatus);
this.Controls.Add(this.txtPayStatus);

           //#####Paytype_ID###Int64
//属性测试100Paytype_ID
this.lblPaytype_ID.AutoSize = true;
this.lblPaytype_ID.Location = new System.Drawing.Point(100,100);
this.lblPaytype_ID.Name = "lblPaytype_ID";
this.lblPaytype_ID.Size = new System.Drawing.Size(41, 12);
this.lblPaytype_ID.TabIndex = 4;
this.lblPaytype_ID.Text = "退款类型";
//111======100
this.cmbPaytype_ID.Location = new System.Drawing.Point(173,96);
this.cmbPaytype_ID.Name ="cmbPaytype_ID";
this.cmbPaytype_ID.Size = new System.Drawing.Size(100, 21);
this.cmbPaytype_ID.TabIndex = 4;
this.Controls.Add(this.lblPaytype_ID);
this.Controls.Add(this.cmbPaytype_ID);

           //#####Employee_ID###Int64
//属性测试125Employee_ID
//属性测试125Employee_ID
//属性测试125Employee_ID
//属性测试125Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,125);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 5;
this.lblEmployee_ID.Text = "业务员";
//111======125
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,121);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 5;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####CustomerVendor_ID###Int64
//属性测试150CustomerVendor_ID
//属性测试150CustomerVendor_ID
//属性测试150CustomerVendor_ID
//属性测试150CustomerVendor_ID
//属性测试150CustomerVendor_ID
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,150);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 6;
this.lblCustomerVendor_ID.Text = "退货客户";
//111======150
this.cmbCustomerVendor_ID.Location = new System.Drawing.Point(173,146);
this.cmbCustomerVendor_ID.Name ="cmbCustomerVendor_ID";
this.cmbCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCustomerVendor_ID.TabIndex = 6;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.cmbCustomerVendor_ID);

           //#####SaleOut_MainID###Int64
//属性测试175SaleOut_MainID
//属性测试175SaleOut_MainID
//属性测试175SaleOut_MainID
this.lblSaleOut_MainID.AutoSize = true;
this.lblSaleOut_MainID.Location = new System.Drawing.Point(100,175);
this.lblSaleOut_MainID.Name = "lblSaleOut_MainID";
this.lblSaleOut_MainID.Size = new System.Drawing.Size(41, 12);
this.lblSaleOut_MainID.TabIndex = 7;
this.lblSaleOut_MainID.Text = "销售出库单";
//111======175
this.cmbSaleOut_MainID.Location = new System.Drawing.Point(173,171);
this.cmbSaleOut_MainID.Name ="cmbSaleOut_MainID";
this.cmbSaleOut_MainID.Size = new System.Drawing.Size(100, 21);
this.cmbSaleOut_MainID.TabIndex = 7;
this.Controls.Add(this.lblSaleOut_MainID);
this.Controls.Add(this.cmbSaleOut_MainID);

           //#####50SaleOut_NO###String
this.lblSaleOut_NO.AutoSize = true;
this.lblSaleOut_NO.Location = new System.Drawing.Point(100,200);
this.lblSaleOut_NO.Name = "lblSaleOut_NO";
this.lblSaleOut_NO.Size = new System.Drawing.Size(41, 12);
this.lblSaleOut_NO.TabIndex = 8;
this.lblSaleOut_NO.Text = "销售出库单号";
this.txtSaleOut_NO.Location = new System.Drawing.Point(173,196);
this.txtSaleOut_NO.Name = "txtSaleOut_NO";
this.txtSaleOut_NO.Size = new System.Drawing.Size(100, 21);
this.txtSaleOut_NO.TabIndex = 8;
this.Controls.Add(this.lblSaleOut_NO);
this.Controls.Add(this.txtSaleOut_NO);

           //#####Currency_ID###Int64
//属性测试225Currency_ID
//属性测试225Currency_ID
//属性测试225Currency_ID
//属性测试225Currency_ID
//属性测试225Currency_ID
this.lblCurrency_ID.AutoSize = true;
this.lblCurrency_ID.Location = new System.Drawing.Point(100,225);
this.lblCurrency_ID.Name = "lblCurrency_ID";
this.lblCurrency_ID.Size = new System.Drawing.Size(41, 12);
this.lblCurrency_ID.TabIndex = 9;
this.lblCurrency_ID.Text = "币别";
this.txtCurrency_ID.Location = new System.Drawing.Point(173,221);
this.txtCurrency_ID.Name = "txtCurrency_ID";
this.txtCurrency_ID.Size = new System.Drawing.Size(100, 21);
this.txtCurrency_ID.TabIndex = 9;
this.Controls.Add(this.lblCurrency_ID);
this.Controls.Add(this.txtCurrency_ID);

           //#####ExchangeRate###Decimal
this.lblExchangeRate.AutoSize = true;
this.lblExchangeRate.Location = new System.Drawing.Point(100,250);
this.lblExchangeRate.Name = "lblExchangeRate";
this.lblExchangeRate.Size = new System.Drawing.Size(41, 12);
this.lblExchangeRate.TabIndex = 10;
this.lblExchangeRate.Text = "汇率";
//111======250
this.txtExchangeRate.Location = new System.Drawing.Point(173,246);
this.txtExchangeRate.Name ="txtExchangeRate";
this.txtExchangeRate.Size = new System.Drawing.Size(100, 21);
this.txtExchangeRate.TabIndex = 10;
this.Controls.Add(this.lblExchangeRate);
this.Controls.Add(this.txtExchangeRate);

           //#####ForeignTotalAmount###Decimal
this.lblForeignTotalAmount.AutoSize = true;
this.lblForeignTotalAmount.Location = new System.Drawing.Point(100,275);
this.lblForeignTotalAmount.Name = "lblForeignTotalAmount";
this.lblForeignTotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblForeignTotalAmount.TabIndex = 11;
this.lblForeignTotalAmount.Text = "金额外币";
//111======275
this.txtForeignTotalAmount.Location = new System.Drawing.Point(173,271);
this.txtForeignTotalAmount.Name ="txtForeignTotalAmount";
this.txtForeignTotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtForeignTotalAmount.TabIndex = 11;
this.Controls.Add(this.lblForeignTotalAmount);
this.Controls.Add(this.txtForeignTotalAmount);

           //#####RefundStatus###Int32
//属性测试300RefundStatus
//属性测试300RefundStatus
//属性测试300RefundStatus
//属性测试300RefundStatus
//属性测试300RefundStatus
this.lblRefundStatus.AutoSize = true;
this.lblRefundStatus.Location = new System.Drawing.Point(100,300);
this.lblRefundStatus.Name = "lblRefundStatus";
this.lblRefundStatus.Size = new System.Drawing.Size(41, 12);
this.lblRefundStatus.TabIndex = 12;
this.lblRefundStatus.Text = "退货退款状态";
this.txtRefundStatus.Location = new System.Drawing.Point(173,296);
this.txtRefundStatus.Name = "txtRefundStatus";
this.txtRefundStatus.Size = new System.Drawing.Size(100, 21);
this.txtRefundStatus.TabIndex = 12;
this.Controls.Add(this.lblRefundStatus);
this.Controls.Add(this.txtRefundStatus);

           //#####TotalQty###Int32
//属性测试325TotalQty
//属性测试325TotalQty
//属性测试325TotalQty
//属性测试325TotalQty
//属性测试325TotalQty
this.lblTotalQty.AutoSize = true;
this.lblTotalQty.Location = new System.Drawing.Point(100,325);
this.lblTotalQty.Name = "lblTotalQty";
this.lblTotalQty.Size = new System.Drawing.Size(41, 12);
this.lblTotalQty.TabIndex = 13;
this.lblTotalQty.Text = "退回总数量";
this.txtTotalQty.Location = new System.Drawing.Point(173,321);
this.txtTotalQty.Name = "txtTotalQty";
this.txtTotalQty.Size = new System.Drawing.Size(100, 21);
this.txtTotalQty.TabIndex = 13;
this.Controls.Add(this.lblTotalQty);
this.Controls.Add(this.txtTotalQty);

           //#####ActualRefundAmount###Decimal
this.lblActualRefundAmount.AutoSize = true;
this.lblActualRefundAmount.Location = new System.Drawing.Point(100,350);
this.lblActualRefundAmount.Name = "lblActualRefundAmount";
this.lblActualRefundAmount.Size = new System.Drawing.Size(41, 12);
this.lblActualRefundAmount.TabIndex = 14;
this.lblActualRefundAmount.Text = "实际退款金额";
//111======350
this.txtActualRefundAmount.Location = new System.Drawing.Point(173,346);
this.txtActualRefundAmount.Name ="txtActualRefundAmount";
this.txtActualRefundAmount.Size = new System.Drawing.Size(100, 21);
this.txtActualRefundAmount.TabIndex = 14;
this.Controls.Add(this.lblActualRefundAmount);
this.Controls.Add(this.txtActualRefundAmount);

           //#####TotalAmount###Decimal
this.lblTotalAmount.AutoSize = true;
this.lblTotalAmount.Location = new System.Drawing.Point(100,375);
this.lblTotalAmount.Name = "lblTotalAmount";
this.lblTotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalAmount.TabIndex = 15;
this.lblTotalAmount.Text = "退款金额合计";
//111======375
this.txtTotalAmount.Location = new System.Drawing.Point(173,371);
this.txtTotalAmount.Name ="txtTotalAmount";
this.txtTotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalAmount.TabIndex = 15;
this.Controls.Add(this.lblTotalAmount);
this.Controls.Add(this.txtTotalAmount);

           //#####ReturnDate###DateTime
this.lblReturnDate.AutoSize = true;
this.lblReturnDate.Location = new System.Drawing.Point(100,400);
this.lblReturnDate.Name = "lblReturnDate";
this.lblReturnDate.Size = new System.Drawing.Size(41, 12);
this.lblReturnDate.TabIndex = 16;
this.lblReturnDate.Text = "发货日期";
//111======400
this.dtpReturnDate.Location = new System.Drawing.Point(173,396);
this.dtpReturnDate.Name ="dtpReturnDate";
this.dtpReturnDate.ShowCheckBox =true;
this.dtpReturnDate.Size = new System.Drawing.Size(100, 21);
this.dtpReturnDate.TabIndex = 16;
this.Controls.Add(this.lblReturnDate);
this.Controls.Add(this.dtpReturnDate);

           //#####ForeignFreightIncome###Decimal
this.lblForeignFreightIncome.AutoSize = true;
this.lblForeignFreightIncome.Location = new System.Drawing.Point(100,425);
this.lblForeignFreightIncome.Name = "lblForeignFreightIncome";
this.lblForeignFreightIncome.Size = new System.Drawing.Size(41, 12);
this.lblForeignFreightIncome.TabIndex = 17;
this.lblForeignFreightIncome.Text = "需退运费外币";
//111======425
this.txtForeignFreightIncome.Location = new System.Drawing.Point(173,421);
this.txtForeignFreightIncome.Name ="txtForeignFreightIncome";
this.txtForeignFreightIncome.Size = new System.Drawing.Size(100, 21);
this.txtForeignFreightIncome.TabIndex = 17;
this.Controls.Add(this.lblForeignFreightIncome);
this.Controls.Add(this.txtForeignFreightIncome);

           //#####FreightIncome###Decimal
this.lblFreightIncome.AutoSize = true;
this.lblFreightIncome.Location = new System.Drawing.Point(100,450);
this.lblFreightIncome.Name = "lblFreightIncome";
this.lblFreightIncome.Size = new System.Drawing.Size(41, 12);
this.lblFreightIncome.TabIndex = 18;
this.lblFreightIncome.Text = "需退运费";
//111======450
this.txtFreightIncome.Location = new System.Drawing.Point(173,446);
this.txtFreightIncome.Name ="txtFreightIncome";
this.txtFreightIncome.Size = new System.Drawing.Size(100, 21);
this.txtFreightIncome.TabIndex = 18;
this.Controls.Add(this.lblFreightIncome);
this.Controls.Add(this.txtFreightIncome);

           //#####FreightCost###Decimal
this.lblFreightCost.AutoSize = true;
this.lblFreightCost.Location = new System.Drawing.Point(100,475);
this.lblFreightCost.Name = "lblFreightCost";
this.lblFreightCost.Size = new System.Drawing.Size(41, 12);
this.lblFreightCost.TabIndex = 19;
this.lblFreightCost.Text = "运费成本";
//111======475
this.txtFreightCost.Location = new System.Drawing.Point(173,471);
this.txtFreightCost.Name ="txtFreightCost";
this.txtFreightCost.Size = new System.Drawing.Size(100, 21);
this.txtFreightCost.TabIndex = 19;
this.Controls.Add(this.lblFreightCost);
this.Controls.Add(this.txtFreightCost);

           //#####50TrackNo###String
this.lblTrackNo.AutoSize = true;
this.lblTrackNo.Location = new System.Drawing.Point(100,500);
this.lblTrackNo.Name = "lblTrackNo";
this.lblTrackNo.Size = new System.Drawing.Size(41, 12);
this.lblTrackNo.TabIndex = 20;
this.lblTrackNo.Text = "物流单号";
this.txtTrackNo.Location = new System.Drawing.Point(173,496);
this.txtTrackNo.Name = "txtTrackNo";
this.txtTrackNo.Size = new System.Drawing.Size(100, 21);
this.txtTrackNo.TabIndex = 20;
this.Controls.Add(this.lblTrackNo);
this.Controls.Add(this.txtTrackNo);

           //#####OfflineRefund###Boolean
this.lblOfflineRefund.AutoSize = true;
this.lblOfflineRefund.Location = new System.Drawing.Point(100,525);
this.lblOfflineRefund.Name = "lblOfflineRefund";
this.lblOfflineRefund.Size = new System.Drawing.Size(41, 12);
this.lblOfflineRefund.TabIndex = 21;
this.lblOfflineRefund.Text = "线下退款";
this.chkOfflineRefund.Location = new System.Drawing.Point(173,521);
this.chkOfflineRefund.Name = "chkOfflineRefund";
this.chkOfflineRefund.Size = new System.Drawing.Size(100, 21);
this.chkOfflineRefund.TabIndex = 21;
this.Controls.Add(this.lblOfflineRefund);
this.Controls.Add(this.chkOfflineRefund);

           //#####IsFromPlatform###Boolean
this.lblIsFromPlatform.AutoSize = true;
this.lblIsFromPlatform.Location = new System.Drawing.Point(100,550);
this.lblIsFromPlatform.Name = "lblIsFromPlatform";
this.lblIsFromPlatform.Size = new System.Drawing.Size(41, 12);
this.lblIsFromPlatform.TabIndex = 22;
this.lblIsFromPlatform.Text = "平台单";
this.chkIsFromPlatform.Location = new System.Drawing.Point(173,546);
this.chkIsFromPlatform.Name = "chkIsFromPlatform";
this.chkIsFromPlatform.Size = new System.Drawing.Size(100, 21);
this.chkIsFromPlatform.TabIndex = 22;
this.Controls.Add(this.lblIsFromPlatform);
this.Controls.Add(this.chkIsFromPlatform);

           //#####100PlatformOrderNo###String
this.lblPlatformOrderNo.AutoSize = true;
this.lblPlatformOrderNo.Location = new System.Drawing.Point(100,575);
this.lblPlatformOrderNo.Name = "lblPlatformOrderNo";
this.lblPlatformOrderNo.Size = new System.Drawing.Size(41, 12);
this.lblPlatformOrderNo.TabIndex = 23;
this.lblPlatformOrderNo.Text = "平台单号";
this.txtPlatformOrderNo.Location = new System.Drawing.Point(173,571);
this.txtPlatformOrderNo.Name = "txtPlatformOrderNo";
this.txtPlatformOrderNo.Size = new System.Drawing.Size(100, 21);
this.txtPlatformOrderNo.TabIndex = 23;
this.Controls.Add(this.lblPlatformOrderNo);
this.Controls.Add(this.txtPlatformOrderNo);

           //#####IsCustomizedOrder###Boolean
this.lblIsCustomizedOrder.AutoSize = true;
this.lblIsCustomizedOrder.Location = new System.Drawing.Point(100,600);
this.lblIsCustomizedOrder.Name = "lblIsCustomizedOrder";
this.lblIsCustomizedOrder.Size = new System.Drawing.Size(41, 12);
this.lblIsCustomizedOrder.TabIndex = 24;
this.lblIsCustomizedOrder.Text = "定制单";
this.chkIsCustomizedOrder.Location = new System.Drawing.Point(173,596);
this.chkIsCustomizedOrder.Name = "chkIsCustomizedOrder";
this.chkIsCustomizedOrder.Size = new System.Drawing.Size(100, 21);
this.chkIsCustomizedOrder.TabIndex = 24;
this.Controls.Add(this.lblIsCustomizedOrder);
this.Controls.Add(this.chkIsCustomizedOrder);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,625);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 25;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,621);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 25;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,650);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 26;
this.lblCreated_at.Text = "创建时间";
//111======650
this.dtpCreated_at.Location = new System.Drawing.Point(173,646);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 26;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试675Created_by
//属性测试675Created_by
//属性测试675Created_by
//属性测试675Created_by
//属性测试675Created_by
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,675);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 27;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,671);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 27;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,700);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 28;
this.lblModified_at.Text = "修改时间";
//111======700
this.dtpModified_at.Location = new System.Drawing.Point(173,696);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 28;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试725Modified_by
//属性测试725Modified_by
//属性测试725Modified_by
//属性测试725Modified_by
//属性测试725Modified_by
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,725);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 29;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,721);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 29;
this.Controls.Add(this.lblModified_by);
this.Controls.Add(this.txtModified_by);

           //#####1000ReturnReason###String
this.lblReturnReason.AutoSize = true;
this.lblReturnReason.Location = new System.Drawing.Point(100,750);
this.lblReturnReason.Name = "lblReturnReason";
this.lblReturnReason.Size = new System.Drawing.Size(41, 12);
this.lblReturnReason.TabIndex = 30;
this.lblReturnReason.Text = "退货原因";
this.txtReturnReason.Location = new System.Drawing.Point(173,746);
this.txtReturnReason.Name = "txtReturnReason";
this.txtReturnReason.Size = new System.Drawing.Size(100, 21);
this.txtReturnReason.TabIndex = 30;
this.Controls.Add(this.lblReturnReason);
this.Controls.Add(this.txtReturnReason);

           //#####TotalCommissionAmount###Decimal
this.lblTotalCommissionAmount.AutoSize = true;
this.lblTotalCommissionAmount.Location = new System.Drawing.Point(100,775);
this.lblTotalCommissionAmount.Name = "lblTotalCommissionAmount";
this.lblTotalCommissionAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalCommissionAmount.TabIndex = 31;
this.lblTotalCommissionAmount.Text = "返还佣金金额";
//111======775
this.txtTotalCommissionAmount.Location = new System.Drawing.Point(173,771);
this.txtTotalCommissionAmount.Name ="txtTotalCommissionAmount";
this.txtTotalCommissionAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalCommissionAmount.TabIndex = 31;
this.Controls.Add(this.lblTotalCommissionAmount);
this.Controls.Add(this.txtTotalCommissionAmount);

           //#####1500Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,800);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 32;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,796);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 32;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####200ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,825);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 33;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,821);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 33;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####Approver_by###Int64
//属性测试850Approver_by
//属性测试850Approver_by
//属性测试850Approver_by
//属性测试850Approver_by
//属性测试850Approver_by
this.lblApprover_by.AutoSize = true;
this.lblApprover_by.Location = new System.Drawing.Point(100,850);
this.lblApprover_by.Name = "lblApprover_by";
this.lblApprover_by.Size = new System.Drawing.Size(41, 12);
this.lblApprover_by.TabIndex = 34;
this.lblApprover_by.Text = "审批人";
this.txtApprover_by.Location = new System.Drawing.Point(173,846);
this.txtApprover_by.Name = "txtApprover_by";
this.txtApprover_by.Size = new System.Drawing.Size(100, 21);
this.txtApprover_by.TabIndex = 34;
this.Controls.Add(this.lblApprover_by);
this.Controls.Add(this.txtApprover_by);

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,875);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 35;
this.lblApprover_at.Text = "审批时间";
//111======875
this.dtpApprover_at.Location = new System.Drawing.Point(173,871);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 35;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,925);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 37;
this.lblApprovalResults.Text = "审批结果";
this.chkApprovalResults.Location = new System.Drawing.Point(173,921);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 37;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####RefundOnly###Boolean
this.lblRefundOnly.AutoSize = true;
this.lblRefundOnly.Location = new System.Drawing.Point(100,950);
this.lblRefundOnly.Name = "lblRefundOnly";
this.lblRefundOnly.Size = new System.Drawing.Size(41, 12);
this.lblRefundOnly.TabIndex = 38;
this.lblRefundOnly.Text = "含税";
this.chkRefundOnly.Location = new System.Drawing.Point(173,946);
this.chkRefundOnly.Name = "chkRefundOnly";
this.chkRefundOnly.Size = new System.Drawing.Size(100, 21);
this.chkRefundOnly.TabIndex = 38;
this.Controls.Add(this.lblRefundOnly);
this.Controls.Add(this.chkRefundOnly);

           //#####IsIncludeTax###Boolean
this.lblIsIncludeTax.AutoSize = true;
this.lblIsIncludeTax.Location = new System.Drawing.Point(100,975);
this.lblIsIncludeTax.Name = "lblIsIncludeTax";
this.lblIsIncludeTax.Size = new System.Drawing.Size(41, 12);
this.lblIsIncludeTax.TabIndex = 39;
this.lblIsIncludeTax.Text = "仅退款";
this.chkIsIncludeTax.Location = new System.Drawing.Point(173,971);
this.chkIsIncludeTax.Name = "chkIsIncludeTax";
this.chkIsIncludeTax.Size = new System.Drawing.Size(100, 21);
this.chkIsIncludeTax.TabIndex = 39;
this.Controls.Add(this.lblIsIncludeTax);
this.Controls.Add(this.chkIsIncludeTax);

           //#####KeepAccountsType###Int32
//属性测试1000KeepAccountsType
//属性测试1000KeepAccountsType
//属性测试1000KeepAccountsType
//属性测试1000KeepAccountsType
//属性测试1000KeepAccountsType
this.lblKeepAccountsType.AutoSize = true;
this.lblKeepAccountsType.Location = new System.Drawing.Point(100,1000);
this.lblKeepAccountsType.Name = "lblKeepAccountsType";
this.lblKeepAccountsType.Size = new System.Drawing.Size(41, 12);
this.lblKeepAccountsType.TabIndex = 40;
this.lblKeepAccountsType.Text = "立帐类型";
this.txtKeepAccountsType.Location = new System.Drawing.Point(173,996);
this.txtKeepAccountsType.Name = "txtKeepAccountsType";
this.txtKeepAccountsType.Size = new System.Drawing.Size(100, 21);
this.txtKeepAccountsType.TabIndex = 40;
this.Controls.Add(this.lblKeepAccountsType);
this.Controls.Add(this.txtKeepAccountsType);

           //#####TaxDeductionType###Int32
//属性测试1025TaxDeductionType
//属性测试1025TaxDeductionType
//属性测试1025TaxDeductionType
//属性测试1025TaxDeductionType
//属性测试1025TaxDeductionType
this.lblTaxDeductionType.AutoSize = true;
this.lblTaxDeductionType.Location = new System.Drawing.Point(100,1025);
this.lblTaxDeductionType.Name = "lblTaxDeductionType";
this.lblTaxDeductionType.Size = new System.Drawing.Size(41, 12);
this.lblTaxDeductionType.TabIndex = 41;
this.lblTaxDeductionType.Text = "扣税类型";
this.txtTaxDeductionType.Location = new System.Drawing.Point(173,1021);
this.txtTaxDeductionType.Name = "txtTaxDeductionType";
this.txtTaxDeductionType.Size = new System.Drawing.Size(100, 21);
this.txtTaxDeductionType.TabIndex = 41;
this.Controls.Add(this.lblTaxDeductionType);
this.Controls.Add(this.txtTaxDeductionType);

           //#####DataStatus###Int32
//属性测试1050DataStatus
//属性测试1050DataStatus
//属性测试1050DataStatus
//属性测试1050DataStatus
//属性测试1050DataStatus
this.lblDataStatus.AutoSize = true;
this.lblDataStatus.Location = new System.Drawing.Point(100,1050);
this.lblDataStatus.Name = "lblDataStatus";
this.lblDataStatus.Size = new System.Drawing.Size(41, 12);
this.lblDataStatus.TabIndex = 42;
this.lblDataStatus.Text = "数据状态";
this.txtDataStatus.Location = new System.Drawing.Point(173,1046);
this.txtDataStatus.Name = "txtDataStatus";
this.txtDataStatus.Size = new System.Drawing.Size(100, 21);
this.txtDataStatus.TabIndex = 42;
this.Controls.Add(this.lblDataStatus);
this.Controls.Add(this.txtDataStatus);

           //#####PrintStatus###Int32
//属性测试1075PrintStatus
//属性测试1075PrintStatus
//属性测试1075PrintStatus
//属性测试1075PrintStatus
//属性测试1075PrintStatus
this.lblPrintStatus.AutoSize = true;
this.lblPrintStatus.Location = new System.Drawing.Point(100,1075);
this.lblPrintStatus.Name = "lblPrintStatus";
this.lblPrintStatus.Size = new System.Drawing.Size(41, 12);
this.lblPrintStatus.TabIndex = 43;
this.lblPrintStatus.Text = "打印状态";
this.txtPrintStatus.Location = new System.Drawing.Point(173,1071);
this.txtPrintStatus.Name = "txtPrintStatus";
this.txtPrintStatus.Size = new System.Drawing.Size(100, 21);
this.txtPrintStatus.TabIndex = 43;
this.Controls.Add(this.lblPrintStatus);
this.Controls.Add(this.txtPrintStatus);

           //#####GenerateVouchers###Boolean
this.lblGenerateVouchers.AutoSize = true;
this.lblGenerateVouchers.Location = new System.Drawing.Point(100,1100);
this.lblGenerateVouchers.Name = "lblGenerateVouchers";
this.lblGenerateVouchers.Size = new System.Drawing.Size(41, 12);
this.lblGenerateVouchers.TabIndex = 44;
this.lblGenerateVouchers.Text = "生成凭证";
this.chkGenerateVouchers.Location = new System.Drawing.Point(173,1096);
this.chkGenerateVouchers.Name = "chkGenerateVouchers";
this.chkGenerateVouchers.Size = new System.Drawing.Size(100, 21);
this.chkGenerateVouchers.TabIndex = 44;
this.Controls.Add(this.lblGenerateVouchers);
this.Controls.Add(this.chkGenerateVouchers);

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
           // this.kryptonPanel1.TabIndex = 44;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblReturnNo );
this.Controls.Add(this.txtReturnNo );

                this.Controls.Add(this.lblProjectGroup_ID );
this.Controls.Add(this.cmbProjectGroup_ID );

                this.Controls.Add(this.lblPayStatus );
this.Controls.Add(this.txtPayStatus );

                this.Controls.Add(this.lblPaytype_ID );
this.Controls.Add(this.cmbPaytype_ID );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.cmbCustomerVendor_ID );

                this.Controls.Add(this.lblSaleOut_MainID );
this.Controls.Add(this.cmbSaleOut_MainID );

                this.Controls.Add(this.lblSaleOut_NO );
this.Controls.Add(this.txtSaleOut_NO );

                this.Controls.Add(this.lblCurrency_ID );
this.Controls.Add(this.txtCurrency_ID );

                this.Controls.Add(this.lblExchangeRate );
this.Controls.Add(this.txtExchangeRate );

                this.Controls.Add(this.lblForeignTotalAmount );
this.Controls.Add(this.txtForeignTotalAmount );

                this.Controls.Add(this.lblRefundStatus );
this.Controls.Add(this.txtRefundStatus );

                this.Controls.Add(this.lblTotalQty );
this.Controls.Add(this.txtTotalQty );

                this.Controls.Add(this.lblActualRefundAmount );
this.Controls.Add(this.txtActualRefundAmount );

                this.Controls.Add(this.lblTotalAmount );
this.Controls.Add(this.txtTotalAmount );

                this.Controls.Add(this.lblReturnDate );
this.Controls.Add(this.dtpReturnDate );

                this.Controls.Add(this.lblForeignFreightIncome );
this.Controls.Add(this.txtForeignFreightIncome );

                this.Controls.Add(this.lblFreightIncome );
this.Controls.Add(this.txtFreightIncome );

                this.Controls.Add(this.lblFreightCost );
this.Controls.Add(this.txtFreightCost );

                this.Controls.Add(this.lblTrackNo );
this.Controls.Add(this.txtTrackNo );

                this.Controls.Add(this.lblOfflineRefund );
this.Controls.Add(this.chkOfflineRefund );

                this.Controls.Add(this.lblIsFromPlatform );
this.Controls.Add(this.chkIsFromPlatform );

                this.Controls.Add(this.lblPlatformOrderNo );
this.Controls.Add(this.txtPlatformOrderNo );

                this.Controls.Add(this.lblIsCustomizedOrder );
this.Controls.Add(this.chkIsCustomizedOrder );

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

                this.Controls.Add(this.lblReturnReason );
this.Controls.Add(this.txtReturnReason );

                this.Controls.Add(this.lblTotalCommissionAmount );
this.Controls.Add(this.txtTotalCommissionAmount );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                this.Controls.Add(this.lblApprover_by );
this.Controls.Add(this.txtApprover_by );

                this.Controls.Add(this.lblApprover_at );
this.Controls.Add(this.dtpApprover_at );

                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                this.Controls.Add(this.lblRefundOnly );
this.Controls.Add(this.chkRefundOnly );

                this.Controls.Add(this.lblIsIncludeTax );
this.Controls.Add(this.chkIsIncludeTax );

                this.Controls.Add(this.lblKeepAccountsType );
this.Controls.Add(this.txtKeepAccountsType );

                this.Controls.Add(this.lblTaxDeductionType );
this.Controls.Add(this.txtTaxDeductionType );

                this.Controls.Add(this.lblDataStatus );
this.Controls.Add(this.txtDataStatus );

                this.Controls.Add(this.lblPrintStatus );
this.Controls.Add(this.txtPrintStatus );

                this.Controls.Add(this.lblGenerateVouchers );
this.Controls.Add(this.chkGenerateVouchers );

                            // 
            // "tb_SaleOutReEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_SaleOutReEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblReturnNo;
private Krypton.Toolkit.KryptonTextBox txtReturnNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblProjectGroup_ID;
private Krypton.Toolkit.KryptonComboBox cmbProjectGroup_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblPayStatus;
private Krypton.Toolkit.KryptonTextBox txtPayStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblPaytype_ID;
private Krypton.Toolkit.KryptonComboBox cmbPaytype_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblSaleOut_MainID;
private Krypton.Toolkit.KryptonComboBox cmbSaleOut_MainID;

    
        
              private Krypton.Toolkit.KryptonLabel lblSaleOut_NO;
private Krypton.Toolkit.KryptonTextBox txtSaleOut_NO;

    
        
              private Krypton.Toolkit.KryptonLabel lblCurrency_ID;
private Krypton.Toolkit.KryptonTextBox txtCurrency_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblExchangeRate;
private Krypton.Toolkit.KryptonTextBox txtExchangeRate;

    
        
              private Krypton.Toolkit.KryptonLabel lblForeignTotalAmount;
private Krypton.Toolkit.KryptonTextBox txtForeignTotalAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblRefundStatus;
private Krypton.Toolkit.KryptonTextBox txtRefundStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalQty;
private Krypton.Toolkit.KryptonTextBox txtTotalQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblActualRefundAmount;
private Krypton.Toolkit.KryptonTextBox txtActualRefundAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalAmount;
private Krypton.Toolkit.KryptonTextBox txtTotalAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblReturnDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpReturnDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblForeignFreightIncome;
private Krypton.Toolkit.KryptonTextBox txtForeignFreightIncome;

    
        
              private Krypton.Toolkit.KryptonLabel lblFreightIncome;
private Krypton.Toolkit.KryptonTextBox txtFreightIncome;

    
        
              private Krypton.Toolkit.KryptonLabel lblFreightCost;
private Krypton.Toolkit.KryptonTextBox txtFreightCost;

    
        
              private Krypton.Toolkit.KryptonLabel lblTrackNo;
private Krypton.Toolkit.KryptonTextBox txtTrackNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblOfflineRefund;
private Krypton.Toolkit.KryptonCheckBox chkOfflineRefund;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsFromPlatform;
private Krypton.Toolkit.KryptonCheckBox chkIsFromPlatform;

    
        
              private Krypton.Toolkit.KryptonLabel lblPlatformOrderNo;
private Krypton.Toolkit.KryptonTextBox txtPlatformOrderNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsCustomizedOrder;
private Krypton.Toolkit.KryptonCheckBox chkIsCustomizedOrder;

    
        
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

    
        
              private Krypton.Toolkit.KryptonLabel lblReturnReason;
private Krypton.Toolkit.KryptonTextBox txtReturnReason;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalCommissionAmount;
private Krypton.Toolkit.KryptonTextBox txtTotalCommissionAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprover_by;
private Krypton.Toolkit.KryptonTextBox txtApprover_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprover_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;

    
        
              
    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalResults;
private Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              private Krypton.Toolkit.KryptonLabel lblRefundOnly;
private Krypton.Toolkit.KryptonCheckBox chkRefundOnly;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsIncludeTax;
private Krypton.Toolkit.KryptonCheckBox chkIsIncludeTax;

    
        
              private Krypton.Toolkit.KryptonLabel lblKeepAccountsType;
private Krypton.Toolkit.KryptonTextBox txtKeepAccountsType;

    
        
              private Krypton.Toolkit.KryptonLabel lblTaxDeductionType;
private Krypton.Toolkit.KryptonTextBox txtTaxDeductionType;

    
        
              private Krypton.Toolkit.KryptonLabel lblDataStatus;
private Krypton.Toolkit.KryptonTextBox txtDataStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblPrintStatus;
private Krypton.Toolkit.KryptonTextBox txtPrintStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblGenerateVouchers;
private Krypton.Toolkit.KryptonCheckBox chkGenerateVouchers;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

