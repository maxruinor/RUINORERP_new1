// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/24/2025 10:37:58
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 销售订单
    /// </summary>
    partial class tb_SaleOrderEdit
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
     this.lblSOrderNo = new Krypton.Toolkit.KryptonLabel();
this.txtSOrderNo = new Krypton.Toolkit.KryptonTextBox();

this.lblPayStatus = new Krypton.Toolkit.KryptonLabel();
this.txtPayStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblPaytype_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbPaytype_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblCustomerVendor_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbCustomerVendor_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblAccount_id = new Krypton.Toolkit.KryptonLabel();
this.cmbAccount_id = new Krypton.Toolkit.KryptonComboBox();

this.lblCurrency_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbCurrency_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblExchangeRate = new Krypton.Toolkit.KryptonLabel();
this.txtExchangeRate = new Krypton.Toolkit.KryptonTextBox();

this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblProjectGroup_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbProjectGroup_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblShipCost = new Krypton.Toolkit.KryptonLabel();
this.txtShipCost = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalQty = new Krypton.Toolkit.KryptonLabel();
this.txtTotalQty = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalCost = new Krypton.Toolkit.KryptonLabel();
this.txtTotalCost = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalAmount = new Krypton.Toolkit.KryptonLabel();
this.txtTotalAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalTaxAmount = new Krypton.Toolkit.KryptonLabel();
this.txtTotalTaxAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblPreDeliveryDate = new Krypton.Toolkit.KryptonLabel();
this.dtpPreDeliveryDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblSaleDate = new Krypton.Toolkit.KryptonLabel();
this.dtpSaleDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblDeliveryDate = new Krypton.Toolkit.KryptonLabel();
this.dtpDeliveryDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblShippingAddress = new Krypton.Toolkit.KryptonLabel();
this.txtShippingAddress = new Krypton.Toolkit.KryptonTextBox();
this.txtShippingAddress.Multiline = true;

this.lblShippingWay = new Krypton.Toolkit.KryptonLabel();
this.txtShippingWay = new Krypton.Toolkit.KryptonTextBox();

this.lblTrackNo = new Krypton.Toolkit.KryptonLabel();
this.txtTrackNo = new Krypton.Toolkit.KryptonTextBox();

this.lblForeignTotalAmount = new Krypton.Toolkit.KryptonLabel();
this.txtForeignTotalAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblCollectedMoney = new Krypton.Toolkit.KryptonLabel();
this.txtCollectedMoney = new Krypton.Toolkit.KryptonTextBox();

this.lblPrePayMoney = new Krypton.Toolkit.KryptonLabel();
this.txtPrePayMoney = new Krypton.Toolkit.KryptonTextBox();

this.lblForeignDeposit = new Krypton.Toolkit.KryptonLabel();
this.txtForeignDeposit = new Krypton.Toolkit.KryptonTextBox();

this.lblDeposit = new Krypton.Toolkit.KryptonLabel();
this.txtDeposit = new Krypton.Toolkit.KryptonTextBox();

this.lblTaxRate = new Krypton.Toolkit.KryptonLabel();
this.txtTaxRate = new Krypton.Toolkit.KryptonTextBox();

this.lblDeliveryDateConfirm = new Krypton.Toolkit.KryptonLabel();
this.chkDeliveryDateConfirm = new Krypton.Toolkit.KryptonCheckBox();
this.chkDeliveryDateConfirm.Values.Text ="";

this.lblTotalUntaxedAmount = new Krypton.Toolkit.KryptonLabel();
this.txtTotalUntaxedAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblCreated_at = new Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblCreated_by = new Krypton.Toolkit.KryptonLabel();
this.txtCreated_by = new Krypton.Toolkit.KryptonTextBox();

this.lblModified_at = new Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblModified_by = new Krypton.Toolkit.KryptonLabel();
this.txtModified_by = new Krypton.Toolkit.KryptonTextBox();

this.lblCloseCaseOpinions = new Krypton.Toolkit.KryptonLabel();
this.txtCloseCaseOpinions = new Krypton.Toolkit.KryptonTextBox();

this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblIsCustomizedOrder = new Krypton.Toolkit.KryptonLabel();
this.chkIsCustomizedOrder = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsCustomizedOrder.Values.Text ="";

this.lblisdeleted = new Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

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

this.lblDataStatus = new Krypton.Toolkit.KryptonLabel();
this.txtDataStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblKeepAccountsType = new Krypton.Toolkit.KryptonLabel();
this.txtKeepAccountsType = new Krypton.Toolkit.KryptonTextBox();

this.lblTaxDeductionType = new Krypton.Toolkit.KryptonLabel();
this.txtTaxDeductionType = new Krypton.Toolkit.KryptonTextBox();

this.lblOrderPriority = new Krypton.Toolkit.KryptonLabel();
this.txtOrderPriority = new Krypton.Toolkit.KryptonTextBox();

this.lblPlatformOrderNo = new Krypton.Toolkit.KryptonLabel();
this.txtPlatformOrderNo = new Krypton.Toolkit.KryptonTextBox();

this.lblPrintStatus = new Krypton.Toolkit.KryptonLabel();
this.txtPrintStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblIsFromPlatform = new Krypton.Toolkit.KryptonLabel();
this.chkIsFromPlatform = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsFromPlatform.Values.Text ="";

this.lblRefBillID = new Krypton.Toolkit.KryptonLabel();
this.txtRefBillID = new Krypton.Toolkit.KryptonTextBox();

this.lblRefNO = new Krypton.Toolkit.KryptonLabel();
this.txtRefNO = new Krypton.Toolkit.KryptonTextBox();

this.lblRefBizType = new Krypton.Toolkit.KryptonLabel();
this.txtRefBizType = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####50SOrderNo###String
this.lblSOrderNo.AutoSize = true;
this.lblSOrderNo.Location = new System.Drawing.Point(100,25);
this.lblSOrderNo.Name = "lblSOrderNo";
this.lblSOrderNo.Size = new System.Drawing.Size(41, 12);
this.lblSOrderNo.TabIndex = 1;
this.lblSOrderNo.Text = "订单编号";
this.txtSOrderNo.Location = new System.Drawing.Point(173,21);
this.txtSOrderNo.Name = "txtSOrderNo";
this.txtSOrderNo.Size = new System.Drawing.Size(100, 21);
this.txtSOrderNo.TabIndex = 1;
this.Controls.Add(this.lblSOrderNo);
this.Controls.Add(this.txtSOrderNo);

           //#####PayStatus###Int32
//属性测试50PayStatus
//属性测试50PayStatus
//属性测试50PayStatus
//属性测试50PayStatus
//属性测试50PayStatus
//属性测试50PayStatus
this.lblPayStatus.AutoSize = true;
this.lblPayStatus.Location = new System.Drawing.Point(100,50);
this.lblPayStatus.Name = "lblPayStatus";
this.lblPayStatus.Size = new System.Drawing.Size(41, 12);
this.lblPayStatus.TabIndex = 2;
this.lblPayStatus.Text = "付款状态";
this.txtPayStatus.Location = new System.Drawing.Point(173,46);
this.txtPayStatus.Name = "txtPayStatus";
this.txtPayStatus.Size = new System.Drawing.Size(100, 21);
this.txtPayStatus.TabIndex = 2;
this.Controls.Add(this.lblPayStatus);
this.Controls.Add(this.txtPayStatus);

           //#####Paytype_ID###Int64
//属性测试75Paytype_ID
//属性测试75Paytype_ID
//属性测试75Paytype_ID
//属性测试75Paytype_ID
//属性测试75Paytype_ID
//属性测试75Paytype_ID
this.lblPaytype_ID.AutoSize = true;
this.lblPaytype_ID.Location = new System.Drawing.Point(100,75);
this.lblPaytype_ID.Name = "lblPaytype_ID";
this.lblPaytype_ID.Size = new System.Drawing.Size(41, 12);
this.lblPaytype_ID.TabIndex = 3;
this.lblPaytype_ID.Text = "付款类型";
//111======75
this.cmbPaytype_ID.Location = new System.Drawing.Point(173,71);
this.cmbPaytype_ID.Name ="cmbPaytype_ID";
this.cmbPaytype_ID.Size = new System.Drawing.Size(100, 21);
this.cmbPaytype_ID.TabIndex = 3;
this.Controls.Add(this.lblPaytype_ID);
this.Controls.Add(this.cmbPaytype_ID);

           //#####CustomerVendor_ID###Int64
//属性测试100CustomerVendor_ID
//属性测试100CustomerVendor_ID
//属性测试100CustomerVendor_ID
//属性测试100CustomerVendor_ID
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,100);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 4;
this.lblCustomerVendor_ID.Text = "客户";
//111======100
this.cmbCustomerVendor_ID.Location = new System.Drawing.Point(173,96);
this.cmbCustomerVendor_ID.Name ="cmbCustomerVendor_ID";
this.cmbCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCustomerVendor_ID.TabIndex = 4;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.cmbCustomerVendor_ID);

           //#####Account_id###Int64
//属性测试125Account_id
//属性测试125Account_id
this.lblAccount_id.AutoSize = true;
this.lblAccount_id.Location = new System.Drawing.Point(100,125);
this.lblAccount_id.Name = "lblAccount_id";
this.lblAccount_id.Size = new System.Drawing.Size(41, 12);
this.lblAccount_id.TabIndex = 5;
this.lblAccount_id.Text = "收款账户";
//111======125
this.cmbAccount_id.Location = new System.Drawing.Point(173,121);
this.cmbAccount_id.Name ="cmbAccount_id";
this.cmbAccount_id.Size = new System.Drawing.Size(100, 21);
this.cmbAccount_id.TabIndex = 5;
this.Controls.Add(this.lblAccount_id);
this.Controls.Add(this.cmbAccount_id);

           //#####Currency_ID###Int64
//属性测试150Currency_ID
this.lblCurrency_ID.AutoSize = true;
this.lblCurrency_ID.Location = new System.Drawing.Point(100,150);
this.lblCurrency_ID.Name = "lblCurrency_ID";
this.lblCurrency_ID.Size = new System.Drawing.Size(41, 12);
this.lblCurrency_ID.TabIndex = 6;
this.lblCurrency_ID.Text = "币别";
//111======150
this.cmbCurrency_ID.Location = new System.Drawing.Point(173,146);
this.cmbCurrency_ID.Name ="cmbCurrency_ID";
this.cmbCurrency_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCurrency_ID.TabIndex = 6;
this.Controls.Add(this.lblCurrency_ID);
this.Controls.Add(this.cmbCurrency_ID);

           //#####ExchangeRate###Decimal
this.lblExchangeRate.AutoSize = true;
this.lblExchangeRate.Location = new System.Drawing.Point(100,175);
this.lblExchangeRate.Name = "lblExchangeRate";
this.lblExchangeRate.Size = new System.Drawing.Size(41, 12);
this.lblExchangeRate.TabIndex = 7;
this.lblExchangeRate.Text = "汇率";
//111======175
this.txtExchangeRate.Location = new System.Drawing.Point(173,171);
this.txtExchangeRate.Name ="txtExchangeRate";
this.txtExchangeRate.Size = new System.Drawing.Size(100, 21);
this.txtExchangeRate.TabIndex = 7;
this.Controls.Add(this.lblExchangeRate);
this.Controls.Add(this.txtExchangeRate);

           //#####Employee_ID###Int64
//属性测试200Employee_ID
//属性测试200Employee_ID
//属性测试200Employee_ID
//属性测试200Employee_ID
//属性测试200Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,200);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 8;
this.lblEmployee_ID.Text = "业务员";
//111======200
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,196);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 8;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####ProjectGroup_ID###Int64
//属性测试225ProjectGroup_ID
//属性测试225ProjectGroup_ID
//属性测试225ProjectGroup_ID
this.lblProjectGroup_ID.AutoSize = true;
this.lblProjectGroup_ID.Location = new System.Drawing.Point(100,225);
this.lblProjectGroup_ID.Name = "lblProjectGroup_ID";
this.lblProjectGroup_ID.Size = new System.Drawing.Size(41, 12);
this.lblProjectGroup_ID.TabIndex = 9;
this.lblProjectGroup_ID.Text = "项目组";
//111======225
this.cmbProjectGroup_ID.Location = new System.Drawing.Point(173,221);
this.cmbProjectGroup_ID.Name ="cmbProjectGroup_ID";
this.cmbProjectGroup_ID.Size = new System.Drawing.Size(100, 21);
this.cmbProjectGroup_ID.TabIndex = 9;
this.Controls.Add(this.lblProjectGroup_ID);
this.Controls.Add(this.cmbProjectGroup_ID);

           //#####ShipCost###Decimal
this.lblShipCost.AutoSize = true;
this.lblShipCost.Location = new System.Drawing.Point(100,250);
this.lblShipCost.Name = "lblShipCost";
this.lblShipCost.Size = new System.Drawing.Size(41, 12);
this.lblShipCost.TabIndex = 10;
this.lblShipCost.Text = "运费收入";
//111======250
this.txtShipCost.Location = new System.Drawing.Point(173,246);
this.txtShipCost.Name ="txtShipCost";
this.txtShipCost.Size = new System.Drawing.Size(100, 21);
this.txtShipCost.TabIndex = 10;
this.Controls.Add(this.lblShipCost);
this.Controls.Add(this.txtShipCost);

           //#####TotalQty###Int32
//属性测试275TotalQty
//属性测试275TotalQty
//属性测试275TotalQty
//属性测试275TotalQty
//属性测试275TotalQty
//属性测试275TotalQty
this.lblTotalQty.AutoSize = true;
this.lblTotalQty.Location = new System.Drawing.Point(100,275);
this.lblTotalQty.Name = "lblTotalQty";
this.lblTotalQty.Size = new System.Drawing.Size(41, 12);
this.lblTotalQty.TabIndex = 11;
this.lblTotalQty.Text = "总数量";
this.txtTotalQty.Location = new System.Drawing.Point(173,271);
this.txtTotalQty.Name = "txtTotalQty";
this.txtTotalQty.Size = new System.Drawing.Size(100, 21);
this.txtTotalQty.TabIndex = 11;
this.Controls.Add(this.lblTotalQty);
this.Controls.Add(this.txtTotalQty);

           //#####TotalCost###Decimal
this.lblTotalCost.AutoSize = true;
this.lblTotalCost.Location = new System.Drawing.Point(100,300);
this.lblTotalCost.Name = "lblTotalCost";
this.lblTotalCost.Size = new System.Drawing.Size(41, 12);
this.lblTotalCost.TabIndex = 12;
this.lblTotalCost.Text = "总成本";
//111======300
this.txtTotalCost.Location = new System.Drawing.Point(173,296);
this.txtTotalCost.Name ="txtTotalCost";
this.txtTotalCost.Size = new System.Drawing.Size(100, 21);
this.txtTotalCost.TabIndex = 12;
this.Controls.Add(this.lblTotalCost);
this.Controls.Add(this.txtTotalCost);

           //#####TotalAmount###Decimal
this.lblTotalAmount.AutoSize = true;
this.lblTotalAmount.Location = new System.Drawing.Point(100,325);
this.lblTotalAmount.Name = "lblTotalAmount";
this.lblTotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalAmount.TabIndex = 13;
this.lblTotalAmount.Text = "总金额";
//111======325
this.txtTotalAmount.Location = new System.Drawing.Point(173,321);
this.txtTotalAmount.Name ="txtTotalAmount";
this.txtTotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalAmount.TabIndex = 13;
this.Controls.Add(this.lblTotalAmount);
this.Controls.Add(this.txtTotalAmount);

           //#####TotalTaxAmount###Decimal
this.lblTotalTaxAmount.AutoSize = true;
this.lblTotalTaxAmount.Location = new System.Drawing.Point(100,350);
this.lblTotalTaxAmount.Name = "lblTotalTaxAmount";
this.lblTotalTaxAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalTaxAmount.TabIndex = 14;
this.lblTotalTaxAmount.Text = "总税额";
//111======350
this.txtTotalTaxAmount.Location = new System.Drawing.Point(173,346);
this.txtTotalTaxAmount.Name ="txtTotalTaxAmount";
this.txtTotalTaxAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalTaxAmount.TabIndex = 14;
this.Controls.Add(this.lblTotalTaxAmount);
this.Controls.Add(this.txtTotalTaxAmount);

           //#####PreDeliveryDate###DateTime
this.lblPreDeliveryDate.AutoSize = true;
this.lblPreDeliveryDate.Location = new System.Drawing.Point(100,375);
this.lblPreDeliveryDate.Name = "lblPreDeliveryDate";
this.lblPreDeliveryDate.Size = new System.Drawing.Size(41, 12);
this.lblPreDeliveryDate.TabIndex = 15;
this.lblPreDeliveryDate.Text = "预交日期";
//111======375
this.dtpPreDeliveryDate.Location = new System.Drawing.Point(173,371);
this.dtpPreDeliveryDate.Name ="dtpPreDeliveryDate";
this.dtpPreDeliveryDate.ShowCheckBox =true;
this.dtpPreDeliveryDate.Size = new System.Drawing.Size(100, 21);
this.dtpPreDeliveryDate.TabIndex = 15;
this.Controls.Add(this.lblPreDeliveryDate);
this.Controls.Add(this.dtpPreDeliveryDate);

           //#####SaleDate###DateTime
this.lblSaleDate.AutoSize = true;
this.lblSaleDate.Location = new System.Drawing.Point(100,400);
this.lblSaleDate.Name = "lblSaleDate";
this.lblSaleDate.Size = new System.Drawing.Size(41, 12);
this.lblSaleDate.TabIndex = 16;
this.lblSaleDate.Text = "订单日期";
//111======400
this.dtpSaleDate.Location = new System.Drawing.Point(173,396);
this.dtpSaleDate.Name ="dtpSaleDate";
this.dtpSaleDate.Size = new System.Drawing.Size(100, 21);
this.dtpSaleDate.TabIndex = 16;
this.Controls.Add(this.lblSaleDate);
this.Controls.Add(this.dtpSaleDate);

           //#####DeliveryDate###DateTime
this.lblDeliveryDate.AutoSize = true;
this.lblDeliveryDate.Location = new System.Drawing.Point(100,425);
this.lblDeliveryDate.Name = "lblDeliveryDate";
this.lblDeliveryDate.Size = new System.Drawing.Size(41, 12);
this.lblDeliveryDate.TabIndex = 17;
this.lblDeliveryDate.Text = "发货日期";
//111======425
this.dtpDeliveryDate.Location = new System.Drawing.Point(173,421);
this.dtpDeliveryDate.Name ="dtpDeliveryDate";
this.dtpDeliveryDate.ShowCheckBox =true;
this.dtpDeliveryDate.Size = new System.Drawing.Size(100, 21);
this.dtpDeliveryDate.TabIndex = 17;
this.Controls.Add(this.lblDeliveryDate);
this.Controls.Add(this.dtpDeliveryDate);

           //#####500ShippingAddress###String
this.lblShippingAddress.AutoSize = true;
this.lblShippingAddress.Location = new System.Drawing.Point(100,450);
this.lblShippingAddress.Name = "lblShippingAddress";
this.lblShippingAddress.Size = new System.Drawing.Size(41, 12);
this.lblShippingAddress.TabIndex = 18;
this.lblShippingAddress.Text = "收货地址";
this.txtShippingAddress.Location = new System.Drawing.Point(173,446);
this.txtShippingAddress.Name = "txtShippingAddress";
this.txtShippingAddress.Size = new System.Drawing.Size(100, 21);
this.txtShippingAddress.TabIndex = 18;
this.Controls.Add(this.lblShippingAddress);
this.Controls.Add(this.txtShippingAddress);

           //#####50ShippingWay###String
this.lblShippingWay.AutoSize = true;
this.lblShippingWay.Location = new System.Drawing.Point(100,475);
this.lblShippingWay.Name = "lblShippingWay";
this.lblShippingWay.Size = new System.Drawing.Size(41, 12);
this.lblShippingWay.TabIndex = 19;
this.lblShippingWay.Text = "发货方式";
this.txtShippingWay.Location = new System.Drawing.Point(173,471);
this.txtShippingWay.Name = "txtShippingWay";
this.txtShippingWay.Size = new System.Drawing.Size(100, 21);
this.txtShippingWay.TabIndex = 19;
this.Controls.Add(this.lblShippingWay);
this.Controls.Add(this.txtShippingWay);

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

           //#####ForeignTotalAmount###Decimal
this.lblForeignTotalAmount.AutoSize = true;
this.lblForeignTotalAmount.Location = new System.Drawing.Point(100,525);
this.lblForeignTotalAmount.Name = "lblForeignTotalAmount";
this.lblForeignTotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblForeignTotalAmount.TabIndex = 21;
this.lblForeignTotalAmount.Text = "金额外币";
//111======525
this.txtForeignTotalAmount.Location = new System.Drawing.Point(173,521);
this.txtForeignTotalAmount.Name ="txtForeignTotalAmount";
this.txtForeignTotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtForeignTotalAmount.TabIndex = 21;
this.Controls.Add(this.lblForeignTotalAmount);
this.Controls.Add(this.txtForeignTotalAmount);

           //#####CollectedMoney###Decimal
this.lblCollectedMoney.AutoSize = true;
this.lblCollectedMoney.Location = new System.Drawing.Point(100,550);
this.lblCollectedMoney.Name = "lblCollectedMoney";
this.lblCollectedMoney.Size = new System.Drawing.Size(41, 12);
this.lblCollectedMoney.TabIndex = 22;
this.lblCollectedMoney.Text = "实收金额";
//111======550
this.txtCollectedMoney.Location = new System.Drawing.Point(173,546);
this.txtCollectedMoney.Name ="txtCollectedMoney";
this.txtCollectedMoney.Size = new System.Drawing.Size(100, 21);
this.txtCollectedMoney.TabIndex = 22;
this.Controls.Add(this.lblCollectedMoney);
this.Controls.Add(this.txtCollectedMoney);

           //#####PrePayMoney###Decimal
this.lblPrePayMoney.AutoSize = true;
this.lblPrePayMoney.Location = new System.Drawing.Point(100,575);
this.lblPrePayMoney.Name = "lblPrePayMoney";
this.lblPrePayMoney.Size = new System.Drawing.Size(41, 12);
this.lblPrePayMoney.TabIndex = 23;
this.lblPrePayMoney.Text = "预收款";
//111======575
this.txtPrePayMoney.Location = new System.Drawing.Point(173,571);
this.txtPrePayMoney.Name ="txtPrePayMoney";
this.txtPrePayMoney.Size = new System.Drawing.Size(100, 21);
this.txtPrePayMoney.TabIndex = 23;
this.Controls.Add(this.lblPrePayMoney);
this.Controls.Add(this.txtPrePayMoney);

           //#####ForeignDeposit###Decimal
this.lblForeignDeposit.AutoSize = true;
this.lblForeignDeposit.Location = new System.Drawing.Point(100,600);
this.lblForeignDeposit.Name = "lblForeignDeposit";
this.lblForeignDeposit.Size = new System.Drawing.Size(41, 12);
this.lblForeignDeposit.TabIndex = 24;
this.lblForeignDeposit.Text = "订金外币";
//111======600
this.txtForeignDeposit.Location = new System.Drawing.Point(173,596);
this.txtForeignDeposit.Name ="txtForeignDeposit";
this.txtForeignDeposit.Size = new System.Drawing.Size(100, 21);
this.txtForeignDeposit.TabIndex = 24;
this.Controls.Add(this.lblForeignDeposit);
this.Controls.Add(this.txtForeignDeposit);

           //#####Deposit###Decimal
this.lblDeposit.AutoSize = true;
this.lblDeposit.Location = new System.Drawing.Point(100,625);
this.lblDeposit.Name = "lblDeposit";
this.lblDeposit.Size = new System.Drawing.Size(41, 12);
this.lblDeposit.TabIndex = 25;
this.lblDeposit.Text = "订金";
//111======625
this.txtDeposit.Location = new System.Drawing.Point(173,621);
this.txtDeposit.Name ="txtDeposit";
this.txtDeposit.Size = new System.Drawing.Size(100, 21);
this.txtDeposit.TabIndex = 25;
this.Controls.Add(this.lblDeposit);
this.Controls.Add(this.txtDeposit);

           //#####TaxRate###Decimal
this.lblTaxRate.AutoSize = true;
this.lblTaxRate.Location = new System.Drawing.Point(100,650);
this.lblTaxRate.Name = "lblTaxRate";
this.lblTaxRate.Size = new System.Drawing.Size(41, 12);
this.lblTaxRate.TabIndex = 26;
this.lblTaxRate.Text = "税率";
//111======650
this.txtTaxRate.Location = new System.Drawing.Point(173,646);
this.txtTaxRate.Name ="txtTaxRate";
this.txtTaxRate.Size = new System.Drawing.Size(100, 21);
this.txtTaxRate.TabIndex = 26;
this.Controls.Add(this.lblTaxRate);
this.Controls.Add(this.txtTaxRate);

           //#####DeliveryDateConfirm###Boolean
this.lblDeliveryDateConfirm.AutoSize = true;
this.lblDeliveryDateConfirm.Location = new System.Drawing.Point(100,675);
this.lblDeliveryDateConfirm.Name = "lblDeliveryDateConfirm";
this.lblDeliveryDateConfirm.Size = new System.Drawing.Size(41, 12);
this.lblDeliveryDateConfirm.TabIndex = 27;
this.lblDeliveryDateConfirm.Text = "交期确认";
this.chkDeliveryDateConfirm.Location = new System.Drawing.Point(173,671);
this.chkDeliveryDateConfirm.Name = "chkDeliveryDateConfirm";
this.chkDeliveryDateConfirm.Size = new System.Drawing.Size(100, 21);
this.chkDeliveryDateConfirm.TabIndex = 27;
this.Controls.Add(this.lblDeliveryDateConfirm);
this.Controls.Add(this.chkDeliveryDateConfirm);

           //#####TotalUntaxedAmount###Decimal
this.lblTotalUntaxedAmount.AutoSize = true;
this.lblTotalUntaxedAmount.Location = new System.Drawing.Point(100,700);
this.lblTotalUntaxedAmount.Name = "lblTotalUntaxedAmount";
this.lblTotalUntaxedAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalUntaxedAmount.TabIndex = 28;
this.lblTotalUntaxedAmount.Text = "未税本位币";
//111======700
this.txtTotalUntaxedAmount.Location = new System.Drawing.Point(173,696);
this.txtTotalUntaxedAmount.Name ="txtTotalUntaxedAmount";
this.txtTotalUntaxedAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalUntaxedAmount.TabIndex = 28;
this.Controls.Add(this.lblTotalUntaxedAmount);
this.Controls.Add(this.txtTotalUntaxedAmount);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,725);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 29;
this.lblCreated_at.Text = "创建时间";
//111======725
this.dtpCreated_at.Location = new System.Drawing.Point(173,721);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 29;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试750Created_by
//属性测试750Created_by
//属性测试750Created_by
//属性测试750Created_by
//属性测试750Created_by
//属性测试750Created_by
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,750);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 30;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,746);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 30;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,775);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 31;
this.lblModified_at.Text = "修改时间";
//111======775
this.dtpModified_at.Location = new System.Drawing.Point(173,771);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 31;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试800Modified_by
//属性测试800Modified_by
//属性测试800Modified_by
//属性测试800Modified_by
//属性测试800Modified_by
//属性测试800Modified_by
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,800);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 32;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,796);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 32;
this.Controls.Add(this.lblModified_by);
this.Controls.Add(this.txtModified_by);

           //#####200CloseCaseOpinions###String
this.lblCloseCaseOpinions.AutoSize = true;
this.lblCloseCaseOpinions.Location = new System.Drawing.Point(100,825);
this.lblCloseCaseOpinions.Name = "lblCloseCaseOpinions";
this.lblCloseCaseOpinions.Size = new System.Drawing.Size(41, 12);
this.lblCloseCaseOpinions.TabIndex = 33;
this.lblCloseCaseOpinions.Text = "结案意见";
this.txtCloseCaseOpinions.Location = new System.Drawing.Point(173,821);
this.txtCloseCaseOpinions.Name = "txtCloseCaseOpinions";
this.txtCloseCaseOpinions.Size = new System.Drawing.Size(100, 21);
this.txtCloseCaseOpinions.TabIndex = 33;
this.Controls.Add(this.lblCloseCaseOpinions);
this.Controls.Add(this.txtCloseCaseOpinions);

           //#####1500Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,850);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 34;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,846);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 34;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####IsCustomizedOrder###Boolean
this.lblIsCustomizedOrder.AutoSize = true;
this.lblIsCustomizedOrder.Location = new System.Drawing.Point(100,875);
this.lblIsCustomizedOrder.Name = "lblIsCustomizedOrder";
this.lblIsCustomizedOrder.Size = new System.Drawing.Size(41, 12);
this.lblIsCustomizedOrder.TabIndex = 35;
this.lblIsCustomizedOrder.Text = "定制单";
this.chkIsCustomizedOrder.Location = new System.Drawing.Point(173,871);
this.chkIsCustomizedOrder.Name = "chkIsCustomizedOrder";
this.chkIsCustomizedOrder.Size = new System.Drawing.Size(100, 21);
this.chkIsCustomizedOrder.TabIndex = 35;
this.Controls.Add(this.lblIsCustomizedOrder);
this.Controls.Add(this.chkIsCustomizedOrder);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,900);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 36;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,896);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 36;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####255ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,925);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 37;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,921);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 37;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####Approver_by###Int64
//属性测试950Approver_by
//属性测试950Approver_by
//属性测试950Approver_by
//属性测试950Approver_by
//属性测试950Approver_by
//属性测试950Approver_by
this.lblApprover_by.AutoSize = true;
this.lblApprover_by.Location = new System.Drawing.Point(100,950);
this.lblApprover_by.Name = "lblApprover_by";
this.lblApprover_by.Size = new System.Drawing.Size(41, 12);
this.lblApprover_by.TabIndex = 38;
this.lblApprover_by.Text = "审批人";
this.txtApprover_by.Location = new System.Drawing.Point(173,946);
this.txtApprover_by.Name = "txtApprover_by";
this.txtApprover_by.Size = new System.Drawing.Size(100, 21);
this.txtApprover_by.TabIndex = 38;
this.Controls.Add(this.lblApprover_by);
this.Controls.Add(this.txtApprover_by);

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,975);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 39;
this.lblApprover_at.Text = "审批时间";
//111======975
this.dtpApprover_at.Location = new System.Drawing.Point(173,971);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 39;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,1025);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 41;
this.lblApprovalResults.Text = "审批结果";
this.chkApprovalResults.Location = new System.Drawing.Point(173,1021);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 41;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####DataStatus###Int32
//属性测试1050DataStatus
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

           //#####KeepAccountsType###Int32
//属性测试1075KeepAccountsType
//属性测试1075KeepAccountsType
//属性测试1075KeepAccountsType
//属性测试1075KeepAccountsType
//属性测试1075KeepAccountsType
//属性测试1075KeepAccountsType
this.lblKeepAccountsType.AutoSize = true;
this.lblKeepAccountsType.Location = new System.Drawing.Point(100,1075);
this.lblKeepAccountsType.Name = "lblKeepAccountsType";
this.lblKeepAccountsType.Size = new System.Drawing.Size(41, 12);
this.lblKeepAccountsType.TabIndex = 43;
this.lblKeepAccountsType.Text = "立帐类型";
this.txtKeepAccountsType.Location = new System.Drawing.Point(173,1071);
this.txtKeepAccountsType.Name = "txtKeepAccountsType";
this.txtKeepAccountsType.Size = new System.Drawing.Size(100, 21);
this.txtKeepAccountsType.TabIndex = 43;
this.Controls.Add(this.lblKeepAccountsType);
this.Controls.Add(this.txtKeepAccountsType);

           //#####TaxDeductionType###Int32
//属性测试1100TaxDeductionType
//属性测试1100TaxDeductionType
//属性测试1100TaxDeductionType
//属性测试1100TaxDeductionType
//属性测试1100TaxDeductionType
//属性测试1100TaxDeductionType
this.lblTaxDeductionType.AutoSize = true;
this.lblTaxDeductionType.Location = new System.Drawing.Point(100,1100);
this.lblTaxDeductionType.Name = "lblTaxDeductionType";
this.lblTaxDeductionType.Size = new System.Drawing.Size(41, 12);
this.lblTaxDeductionType.TabIndex = 44;
this.lblTaxDeductionType.Text = "扣税类型";
this.txtTaxDeductionType.Location = new System.Drawing.Point(173,1096);
this.txtTaxDeductionType.Name = "txtTaxDeductionType";
this.txtTaxDeductionType.Size = new System.Drawing.Size(100, 21);
this.txtTaxDeductionType.TabIndex = 44;
this.Controls.Add(this.lblTaxDeductionType);
this.Controls.Add(this.txtTaxDeductionType);

           //#####OrderPriority###Int32
//属性测试1125OrderPriority
//属性测试1125OrderPriority
//属性测试1125OrderPriority
//属性测试1125OrderPriority
//属性测试1125OrderPriority
//属性测试1125OrderPriority
this.lblOrderPriority.AutoSize = true;
this.lblOrderPriority.Location = new System.Drawing.Point(100,1125);
this.lblOrderPriority.Name = "lblOrderPriority";
this.lblOrderPriority.Size = new System.Drawing.Size(41, 12);
this.lblOrderPriority.TabIndex = 45;
this.lblOrderPriority.Text = "紧急程度";
this.txtOrderPriority.Location = new System.Drawing.Point(173,1121);
this.txtOrderPriority.Name = "txtOrderPriority";
this.txtOrderPriority.Size = new System.Drawing.Size(100, 21);
this.txtOrderPriority.TabIndex = 45;
this.Controls.Add(this.lblOrderPriority);
this.Controls.Add(this.txtOrderPriority);

           //#####100PlatformOrderNo###String
this.lblPlatformOrderNo.AutoSize = true;
this.lblPlatformOrderNo.Location = new System.Drawing.Point(100,1150);
this.lblPlatformOrderNo.Name = "lblPlatformOrderNo";
this.lblPlatformOrderNo.Size = new System.Drawing.Size(41, 12);
this.lblPlatformOrderNo.TabIndex = 46;
this.lblPlatformOrderNo.Text = "平台单号";
this.txtPlatformOrderNo.Location = new System.Drawing.Point(173,1146);
this.txtPlatformOrderNo.Name = "txtPlatformOrderNo";
this.txtPlatformOrderNo.Size = new System.Drawing.Size(100, 21);
this.txtPlatformOrderNo.TabIndex = 46;
this.Controls.Add(this.lblPlatformOrderNo);
this.Controls.Add(this.txtPlatformOrderNo);

           //#####PrintStatus###Int32
//属性测试1175PrintStatus
//属性测试1175PrintStatus
//属性测试1175PrintStatus
//属性测试1175PrintStatus
//属性测试1175PrintStatus
//属性测试1175PrintStatus
this.lblPrintStatus.AutoSize = true;
this.lblPrintStatus.Location = new System.Drawing.Point(100,1175);
this.lblPrintStatus.Name = "lblPrintStatus";
this.lblPrintStatus.Size = new System.Drawing.Size(41, 12);
this.lblPrintStatus.TabIndex = 47;
this.lblPrintStatus.Text = "打印状态";
this.txtPrintStatus.Location = new System.Drawing.Point(173,1171);
this.txtPrintStatus.Name = "txtPrintStatus";
this.txtPrintStatus.Size = new System.Drawing.Size(100, 21);
this.txtPrintStatus.TabIndex = 47;
this.Controls.Add(this.lblPrintStatus);
this.Controls.Add(this.txtPrintStatus);

           //#####IsFromPlatform###Boolean
this.lblIsFromPlatform.AutoSize = true;
this.lblIsFromPlatform.Location = new System.Drawing.Point(100,1200);
this.lblIsFromPlatform.Name = "lblIsFromPlatform";
this.lblIsFromPlatform.Size = new System.Drawing.Size(41, 12);
this.lblIsFromPlatform.TabIndex = 48;
this.lblIsFromPlatform.Text = "平台单";
this.chkIsFromPlatform.Location = new System.Drawing.Point(173,1196);
this.chkIsFromPlatform.Name = "chkIsFromPlatform";
this.chkIsFromPlatform.Size = new System.Drawing.Size(100, 21);
this.chkIsFromPlatform.TabIndex = 48;
this.Controls.Add(this.lblIsFromPlatform);
this.Controls.Add(this.chkIsFromPlatform);

           //#####RefBillID###Int64
//属性测试1225RefBillID
//属性测试1225RefBillID
//属性测试1225RefBillID
//属性测试1225RefBillID
//属性测试1225RefBillID
//属性测试1225RefBillID
this.lblRefBillID.AutoSize = true;
this.lblRefBillID.Location = new System.Drawing.Point(100,1225);
this.lblRefBillID.Name = "lblRefBillID";
this.lblRefBillID.Size = new System.Drawing.Size(41, 12);
this.lblRefBillID.TabIndex = 49;
this.lblRefBillID.Text = "引用单据";
this.txtRefBillID.Location = new System.Drawing.Point(173,1221);
this.txtRefBillID.Name = "txtRefBillID";
this.txtRefBillID.Size = new System.Drawing.Size(100, 21);
this.txtRefBillID.TabIndex = 49;
this.Controls.Add(this.lblRefBillID);
this.Controls.Add(this.txtRefBillID);

           //#####50RefNO###String
this.lblRefNO.AutoSize = true;
this.lblRefNO.Location = new System.Drawing.Point(100,1250);
this.lblRefNO.Name = "lblRefNO";
this.lblRefNO.Size = new System.Drawing.Size(41, 12);
this.lblRefNO.TabIndex = 50;
this.lblRefNO.Text = "引用单号";
this.txtRefNO.Location = new System.Drawing.Point(173,1246);
this.txtRefNO.Name = "txtRefNO";
this.txtRefNO.Size = new System.Drawing.Size(100, 21);
this.txtRefNO.TabIndex = 50;
this.Controls.Add(this.lblRefNO);
this.Controls.Add(this.txtRefNO);

           //#####RefBizType###Int32
//属性测试1275RefBizType
//属性测试1275RefBizType
//属性测试1275RefBizType
//属性测试1275RefBizType
//属性测试1275RefBizType
//属性测试1275RefBizType
this.lblRefBizType.AutoSize = true;
this.lblRefBizType.Location = new System.Drawing.Point(100,1275);
this.lblRefBizType.Name = "lblRefBizType";
this.lblRefBizType.Size = new System.Drawing.Size(41, 12);
this.lblRefBizType.TabIndex = 51;
this.lblRefBizType.Text = "引用单据类型";
this.txtRefBizType.Location = new System.Drawing.Point(173,1271);
this.txtRefBizType.Name = "txtRefBizType";
this.txtRefBizType.Size = new System.Drawing.Size(100, 21);
this.txtRefBizType.TabIndex = 51;
this.Controls.Add(this.lblRefBizType);
this.Controls.Add(this.txtRefBizType);

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
           // this.kryptonPanel1.TabIndex = 51;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblSOrderNo );
this.Controls.Add(this.txtSOrderNo );

                this.Controls.Add(this.lblPayStatus );
this.Controls.Add(this.txtPayStatus );

                this.Controls.Add(this.lblPaytype_ID );
this.Controls.Add(this.cmbPaytype_ID );

                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.cmbCustomerVendor_ID );

                this.Controls.Add(this.lblAccount_id );
this.Controls.Add(this.cmbAccount_id );

                this.Controls.Add(this.lblCurrency_ID );
this.Controls.Add(this.cmbCurrency_ID );

                this.Controls.Add(this.lblExchangeRate );
this.Controls.Add(this.txtExchangeRate );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblProjectGroup_ID );
this.Controls.Add(this.cmbProjectGroup_ID );

                this.Controls.Add(this.lblShipCost );
this.Controls.Add(this.txtShipCost );

                this.Controls.Add(this.lblTotalQty );
this.Controls.Add(this.txtTotalQty );

                this.Controls.Add(this.lblTotalCost );
this.Controls.Add(this.txtTotalCost );

                this.Controls.Add(this.lblTotalAmount );
this.Controls.Add(this.txtTotalAmount );

                this.Controls.Add(this.lblTotalTaxAmount );
this.Controls.Add(this.txtTotalTaxAmount );

                this.Controls.Add(this.lblPreDeliveryDate );
this.Controls.Add(this.dtpPreDeliveryDate );

                this.Controls.Add(this.lblSaleDate );
this.Controls.Add(this.dtpSaleDate );

                this.Controls.Add(this.lblDeliveryDate );
this.Controls.Add(this.dtpDeliveryDate );

                this.Controls.Add(this.lblShippingAddress );
this.Controls.Add(this.txtShippingAddress );

                this.Controls.Add(this.lblShippingWay );
this.Controls.Add(this.txtShippingWay );

                this.Controls.Add(this.lblTrackNo );
this.Controls.Add(this.txtTrackNo );

                this.Controls.Add(this.lblForeignTotalAmount );
this.Controls.Add(this.txtForeignTotalAmount );

                this.Controls.Add(this.lblCollectedMoney );
this.Controls.Add(this.txtCollectedMoney );

                this.Controls.Add(this.lblPrePayMoney );
this.Controls.Add(this.txtPrePayMoney );

                this.Controls.Add(this.lblForeignDeposit );
this.Controls.Add(this.txtForeignDeposit );

                this.Controls.Add(this.lblDeposit );
this.Controls.Add(this.txtDeposit );

                this.Controls.Add(this.lblTaxRate );
this.Controls.Add(this.txtTaxRate );

                this.Controls.Add(this.lblDeliveryDateConfirm );
this.Controls.Add(this.chkDeliveryDateConfirm );

                this.Controls.Add(this.lblTotalUntaxedAmount );
this.Controls.Add(this.txtTotalUntaxedAmount );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                this.Controls.Add(this.lblModified_by );
this.Controls.Add(this.txtModified_by );

                this.Controls.Add(this.lblCloseCaseOpinions );
this.Controls.Add(this.txtCloseCaseOpinions );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblIsCustomizedOrder );
this.Controls.Add(this.chkIsCustomizedOrder );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                this.Controls.Add(this.lblApprover_by );
this.Controls.Add(this.txtApprover_by );

                this.Controls.Add(this.lblApprover_at );
this.Controls.Add(this.dtpApprover_at );

                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                this.Controls.Add(this.lblDataStatus );
this.Controls.Add(this.txtDataStatus );

                this.Controls.Add(this.lblKeepAccountsType );
this.Controls.Add(this.txtKeepAccountsType );

                this.Controls.Add(this.lblTaxDeductionType );
this.Controls.Add(this.txtTaxDeductionType );

                this.Controls.Add(this.lblOrderPriority );
this.Controls.Add(this.txtOrderPriority );

                this.Controls.Add(this.lblPlatformOrderNo );
this.Controls.Add(this.txtPlatformOrderNo );

                this.Controls.Add(this.lblPrintStatus );
this.Controls.Add(this.txtPrintStatus );

                this.Controls.Add(this.lblIsFromPlatform );
this.Controls.Add(this.chkIsFromPlatform );

                this.Controls.Add(this.lblRefBillID );
this.Controls.Add(this.txtRefBillID );

                this.Controls.Add(this.lblRefNO );
this.Controls.Add(this.txtRefNO );

                this.Controls.Add(this.lblRefBizType );
this.Controls.Add(this.txtRefBizType );

                            // 
            // "tb_SaleOrderEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_SaleOrderEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblSOrderNo;
private Krypton.Toolkit.KryptonTextBox txtSOrderNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblPayStatus;
private Krypton.Toolkit.KryptonTextBox txtPayStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblPaytype_ID;
private Krypton.Toolkit.KryptonComboBox cmbPaytype_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblAccount_id;
private Krypton.Toolkit.KryptonComboBox cmbAccount_id;

    
        
              private Krypton.Toolkit.KryptonLabel lblCurrency_ID;
private Krypton.Toolkit.KryptonComboBox cmbCurrency_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblExchangeRate;
private Krypton.Toolkit.KryptonTextBox txtExchangeRate;

    
        
              private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblProjectGroup_ID;
private Krypton.Toolkit.KryptonComboBox cmbProjectGroup_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblShipCost;
private Krypton.Toolkit.KryptonTextBox txtShipCost;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalQty;
private Krypton.Toolkit.KryptonTextBox txtTotalQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalCost;
private Krypton.Toolkit.KryptonTextBox txtTotalCost;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalAmount;
private Krypton.Toolkit.KryptonTextBox txtTotalAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalTaxAmount;
private Krypton.Toolkit.KryptonTextBox txtTotalTaxAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblPreDeliveryDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpPreDeliveryDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblSaleDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpSaleDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblDeliveryDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpDeliveryDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblShippingAddress;
private Krypton.Toolkit.KryptonTextBox txtShippingAddress;

    
        
              private Krypton.Toolkit.KryptonLabel lblShippingWay;
private Krypton.Toolkit.KryptonTextBox txtShippingWay;

    
        
              private Krypton.Toolkit.KryptonLabel lblTrackNo;
private Krypton.Toolkit.KryptonTextBox txtTrackNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblForeignTotalAmount;
private Krypton.Toolkit.KryptonTextBox txtForeignTotalAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblCollectedMoney;
private Krypton.Toolkit.KryptonTextBox txtCollectedMoney;

    
        
              private Krypton.Toolkit.KryptonLabel lblPrePayMoney;
private Krypton.Toolkit.KryptonTextBox txtPrePayMoney;

    
        
              private Krypton.Toolkit.KryptonLabel lblForeignDeposit;
private Krypton.Toolkit.KryptonTextBox txtForeignDeposit;

    
        
              private Krypton.Toolkit.KryptonLabel lblDeposit;
private Krypton.Toolkit.KryptonTextBox txtDeposit;

    
        
              private Krypton.Toolkit.KryptonLabel lblTaxRate;
private Krypton.Toolkit.KryptonTextBox txtTaxRate;

    
        
              private Krypton.Toolkit.KryptonLabel lblDeliveryDateConfirm;
private Krypton.Toolkit.KryptonCheckBox chkDeliveryDateConfirm;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalUntaxedAmount;
private Krypton.Toolkit.KryptonTextBox txtTotalUntaxedAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_by;
private Krypton.Toolkit.KryptonTextBox txtCreated_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_by;
private Krypton.Toolkit.KryptonTextBox txtModified_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblCloseCaseOpinions;
private Krypton.Toolkit.KryptonTextBox txtCloseCaseOpinions;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsCustomizedOrder;
private Krypton.Toolkit.KryptonCheckBox chkIsCustomizedOrder;

    
        
              private Krypton.Toolkit.KryptonLabel lblisdeleted;
private Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprover_by;
private Krypton.Toolkit.KryptonTextBox txtApprover_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprover_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;

    
        
              
    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalResults;
private Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              private Krypton.Toolkit.KryptonLabel lblDataStatus;
private Krypton.Toolkit.KryptonTextBox txtDataStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblKeepAccountsType;
private Krypton.Toolkit.KryptonTextBox txtKeepAccountsType;

    
        
              private Krypton.Toolkit.KryptonLabel lblTaxDeductionType;
private Krypton.Toolkit.KryptonTextBox txtTaxDeductionType;

    
        
              private Krypton.Toolkit.KryptonLabel lblOrderPriority;
private Krypton.Toolkit.KryptonTextBox txtOrderPriority;

    
        
              private Krypton.Toolkit.KryptonLabel lblPlatformOrderNo;
private Krypton.Toolkit.KryptonTextBox txtPlatformOrderNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblPrintStatus;
private Krypton.Toolkit.KryptonTextBox txtPrintStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsFromPlatform;
private Krypton.Toolkit.KryptonCheckBox chkIsFromPlatform;

    
        
              private Krypton.Toolkit.KryptonLabel lblRefBillID;
private Krypton.Toolkit.KryptonTextBox txtRefBillID;

    
        
              private Krypton.Toolkit.KryptonLabel lblRefNO;
private Krypton.Toolkit.KryptonTextBox txtRefNO;

    
        
              private Krypton.Toolkit.KryptonLabel lblRefBizType;
private Krypton.Toolkit.KryptonTextBox txtRefBizType;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

