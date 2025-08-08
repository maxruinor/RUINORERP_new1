
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:14
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 销售出库单
    /// </summary>
    partial class tb_SaleOutQuery
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
        
     //for start
     
     

this.lblSOrder_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbSOrder_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblSaleOrderNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSaleOrderNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSaleOutNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSaleOutNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();




this.lblExchangeRate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtExchangeRate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();



this.lblForeignFreightIncome = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtForeignFreightIncome = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblFreightIncome = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtFreightIncome = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblOutDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpOutDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblDeliveryDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpDeliveryDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblDueDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpDueDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblShippingAddress = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtShippingAddress = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtShippingAddress.Multiline = true;

this.lblShippingWay = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtShippingWay = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPlatformOrderNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPlatformOrderNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblIsCustomizedOrder = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsCustomizedOrder = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsCustomizedOrder.Values.Text ="";

this.lblIsFromPlatform = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsFromPlatform = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsFromPlatform.Values.Text ="";

this.lblTrackNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTrackNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCustomerPONo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCustomerPONo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblForeignTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtForeignTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCollectedMoney = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCollectedMoney = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";


this.lblForeignDeposit = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtForeignDeposit = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblDeposit = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDeposit = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblFreightCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtFreightCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();




this.lblTotalCommissionAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalCommissionAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTaxRate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTaxRate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalUntaxedAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalUntaxedAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblGenerateVouchers = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkGenerateVouchers = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkGenerateVouchers.Values.Text ="";

this.lblDiscountAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDiscountAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPrePayMoney = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPrePayMoney = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblReplaceOut = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkReplaceOut = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkReplaceOut.Values.Text ="";

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####Employee_ID###Int64
//属性测试25Employee_ID

           //#####CustomerVendor_ID###Int64
//属性测试50CustomerVendor_ID

           //#####SOrder_ID###Int64
//属性测试75SOrder_ID
this.lblSOrder_ID.AutoSize = true;
this.lblSOrder_ID.Location = new System.Drawing.Point(100,75);
this.lblSOrder_ID.Name = "lblSOrder_ID";
this.lblSOrder_ID.Size = new System.Drawing.Size(41, 12);
this.lblSOrder_ID.TabIndex = 3;
this.lblSOrder_ID.Text = "销售订单";
//111======75
this.cmbSOrder_ID.Location = new System.Drawing.Point(173,71);
this.cmbSOrder_ID.Name ="cmbSOrder_ID";
this.cmbSOrder_ID.Size = new System.Drawing.Size(100, 21);
this.cmbSOrder_ID.TabIndex = 3;
this.Controls.Add(this.lblSOrder_ID);
this.Controls.Add(this.cmbSOrder_ID);

           //#####50SaleOrderNo###String
this.lblSaleOrderNo.AutoSize = true;
this.lblSaleOrderNo.Location = new System.Drawing.Point(100,100);
this.lblSaleOrderNo.Name = "lblSaleOrderNo";
this.lblSaleOrderNo.Size = new System.Drawing.Size(41, 12);
this.lblSaleOrderNo.TabIndex = 4;
this.lblSaleOrderNo.Text = "销售订单编号";
this.txtSaleOrderNo.Location = new System.Drawing.Point(173,96);
this.txtSaleOrderNo.Name = "txtSaleOrderNo";
this.txtSaleOrderNo.Size = new System.Drawing.Size(100, 21);
this.txtSaleOrderNo.TabIndex = 4;
this.Controls.Add(this.lblSaleOrderNo);
this.Controls.Add(this.txtSaleOrderNo);

           //#####50SaleOutNo###String
this.lblSaleOutNo.AutoSize = true;
this.lblSaleOutNo.Location = new System.Drawing.Point(100,125);
this.lblSaleOutNo.Name = "lblSaleOutNo";
this.lblSaleOutNo.Size = new System.Drawing.Size(41, 12);
this.lblSaleOutNo.TabIndex = 5;
this.lblSaleOutNo.Text = "出库单号";
this.txtSaleOutNo.Location = new System.Drawing.Point(173,121);
this.txtSaleOutNo.Name = "txtSaleOutNo";
this.txtSaleOutNo.Size = new System.Drawing.Size(100, 21);
this.txtSaleOutNo.TabIndex = 5;
this.Controls.Add(this.lblSaleOutNo);
this.Controls.Add(this.txtSaleOutNo);

           //#####ProjectGroup_ID###Int64
//属性测试150ProjectGroup_ID

           //#####PayStatus###Int32
//属性测试175PayStatus

           //#####Currency_ID###Int64
//属性测试200Currency_ID

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

           //#####Paytype_ID###Int64
//属性测试250Paytype_ID

           //#####RefundStatus###Int32
//属性测试275RefundStatus

           //#####ForeignFreightIncome###Decimal
this.lblForeignFreightIncome.AutoSize = true;
this.lblForeignFreightIncome.Location = new System.Drawing.Point(100,300);
this.lblForeignFreightIncome.Name = "lblForeignFreightIncome";
this.lblForeignFreightIncome.Size = new System.Drawing.Size(41, 12);
this.lblForeignFreightIncome.TabIndex = 12;
this.lblForeignFreightIncome.Text = "运费收入外币";
//111======300
this.txtForeignFreightIncome.Location = new System.Drawing.Point(173,296);
this.txtForeignFreightIncome.Name ="txtForeignFreightIncome";
this.txtForeignFreightIncome.Size = new System.Drawing.Size(100, 21);
this.txtForeignFreightIncome.TabIndex = 12;
this.Controls.Add(this.lblForeignFreightIncome);
this.Controls.Add(this.txtForeignFreightIncome);

           //#####FreightIncome###Decimal
this.lblFreightIncome.AutoSize = true;
this.lblFreightIncome.Location = new System.Drawing.Point(100,325);
this.lblFreightIncome.Name = "lblFreightIncome";
this.lblFreightIncome.Size = new System.Drawing.Size(41, 12);
this.lblFreightIncome.TabIndex = 13;
this.lblFreightIncome.Text = "运费收入";
//111======325
this.txtFreightIncome.Location = new System.Drawing.Point(173,321);
this.txtFreightIncome.Name ="txtFreightIncome";
this.txtFreightIncome.Size = new System.Drawing.Size(100, 21);
this.txtFreightIncome.TabIndex = 13;
this.Controls.Add(this.lblFreightIncome);
this.Controls.Add(this.txtFreightIncome);

           //#####TotalQty###Int32
//属性测试350TotalQty

           //#####TotalAmount###Decimal
this.lblTotalAmount.AutoSize = true;
this.lblTotalAmount.Location = new System.Drawing.Point(100,375);
this.lblTotalAmount.Name = "lblTotalAmount";
this.lblTotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalAmount.TabIndex = 15;
this.lblTotalAmount.Text = "总金额";
//111======375
this.txtTotalAmount.Location = new System.Drawing.Point(173,371);
this.txtTotalAmount.Name ="txtTotalAmount";
this.txtTotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalAmount.TabIndex = 15;
this.Controls.Add(this.lblTotalAmount);
this.Controls.Add(this.txtTotalAmount);

           //#####OutDate###DateTime
this.lblOutDate.AutoSize = true;
this.lblOutDate.Location = new System.Drawing.Point(100,400);
this.lblOutDate.Name = "lblOutDate";
this.lblOutDate.Size = new System.Drawing.Size(41, 12);
this.lblOutDate.TabIndex = 16;
this.lblOutDate.Text = "出库日期";
//111======400
this.dtpOutDate.Location = new System.Drawing.Point(173,396);
this.dtpOutDate.Name ="dtpOutDate";
this.dtpOutDate.Size = new System.Drawing.Size(100, 21);
this.dtpOutDate.TabIndex = 16;
this.Controls.Add(this.lblOutDate);
this.Controls.Add(this.dtpOutDate);

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

           //#####DueDate###DateTime
this.lblDueDate.AutoSize = true;
this.lblDueDate.Location = new System.Drawing.Point(100,450);
this.lblDueDate.Name = "lblDueDate";
this.lblDueDate.Size = new System.Drawing.Size(41, 12);
this.lblDueDate.TabIndex = 18;
this.lblDueDate.Text = "账期届满日";
//111======450
this.dtpDueDate.Location = new System.Drawing.Point(173,446);
this.dtpDueDate.Name ="dtpDueDate";
this.dtpDueDate.ShowCheckBox =true;
this.dtpDueDate.Size = new System.Drawing.Size(100, 21);
this.dtpDueDate.TabIndex = 18;
this.Controls.Add(this.lblDueDate);
this.Controls.Add(this.dtpDueDate);

           //#####500ShippingAddress###String
this.lblShippingAddress.AutoSize = true;
this.lblShippingAddress.Location = new System.Drawing.Point(100,475);
this.lblShippingAddress.Name = "lblShippingAddress";
this.lblShippingAddress.Size = new System.Drawing.Size(41, 12);
this.lblShippingAddress.TabIndex = 19;
this.lblShippingAddress.Text = "收货地址";
this.txtShippingAddress.Location = new System.Drawing.Point(173,471);
this.txtShippingAddress.Name = "txtShippingAddress";
this.txtShippingAddress.Size = new System.Drawing.Size(100, 21);
this.txtShippingAddress.TabIndex = 19;
this.Controls.Add(this.lblShippingAddress);
this.Controls.Add(this.txtShippingAddress);

           //#####50ShippingWay###String
this.lblShippingWay.AutoSize = true;
this.lblShippingWay.Location = new System.Drawing.Point(100,500);
this.lblShippingWay.Name = "lblShippingWay";
this.lblShippingWay.Size = new System.Drawing.Size(41, 12);
this.lblShippingWay.TabIndex = 20;
this.lblShippingWay.Text = "发货方式";
this.txtShippingWay.Location = new System.Drawing.Point(173,496);
this.txtShippingWay.Name = "txtShippingWay";
this.txtShippingWay.Size = new System.Drawing.Size(100, 21);
this.txtShippingWay.TabIndex = 20;
this.Controls.Add(this.lblShippingWay);
this.Controls.Add(this.txtShippingWay);

           //#####100PlatformOrderNo###String
this.lblPlatformOrderNo.AutoSize = true;
this.lblPlatformOrderNo.Location = new System.Drawing.Point(100,525);
this.lblPlatformOrderNo.Name = "lblPlatformOrderNo";
this.lblPlatformOrderNo.Size = new System.Drawing.Size(41, 12);
this.lblPlatformOrderNo.TabIndex = 21;
this.lblPlatformOrderNo.Text = "平台单号";
this.txtPlatformOrderNo.Location = new System.Drawing.Point(173,521);
this.txtPlatformOrderNo.Name = "txtPlatformOrderNo";
this.txtPlatformOrderNo.Size = new System.Drawing.Size(100, 21);
this.txtPlatformOrderNo.TabIndex = 21;
this.Controls.Add(this.lblPlatformOrderNo);
this.Controls.Add(this.txtPlatformOrderNo);

           //#####IsCustomizedOrder###Boolean
this.lblIsCustomizedOrder.AutoSize = true;
this.lblIsCustomizedOrder.Location = new System.Drawing.Point(100,550);
this.lblIsCustomizedOrder.Name = "lblIsCustomizedOrder";
this.lblIsCustomizedOrder.Size = new System.Drawing.Size(41, 12);
this.lblIsCustomizedOrder.TabIndex = 22;
this.lblIsCustomizedOrder.Text = "定制单";
this.chkIsCustomizedOrder.Location = new System.Drawing.Point(173,546);
this.chkIsCustomizedOrder.Name = "chkIsCustomizedOrder";
this.chkIsCustomizedOrder.Size = new System.Drawing.Size(100, 21);
this.chkIsCustomizedOrder.TabIndex = 22;
this.Controls.Add(this.lblIsCustomizedOrder);
this.Controls.Add(this.chkIsCustomizedOrder);

           //#####IsFromPlatform###Boolean
this.lblIsFromPlatform.AutoSize = true;
this.lblIsFromPlatform.Location = new System.Drawing.Point(100,575);
this.lblIsFromPlatform.Name = "lblIsFromPlatform";
this.lblIsFromPlatform.Size = new System.Drawing.Size(41, 12);
this.lblIsFromPlatform.TabIndex = 23;
this.lblIsFromPlatform.Text = "平台单";
this.chkIsFromPlatform.Location = new System.Drawing.Point(173,571);
this.chkIsFromPlatform.Name = "chkIsFromPlatform";
this.chkIsFromPlatform.Size = new System.Drawing.Size(100, 21);
this.chkIsFromPlatform.TabIndex = 23;
this.Controls.Add(this.lblIsFromPlatform);
this.Controls.Add(this.chkIsFromPlatform);

           //#####50TrackNo###String
this.lblTrackNo.AutoSize = true;
this.lblTrackNo.Location = new System.Drawing.Point(100,600);
this.lblTrackNo.Name = "lblTrackNo";
this.lblTrackNo.Size = new System.Drawing.Size(41, 12);
this.lblTrackNo.TabIndex = 24;
this.lblTrackNo.Text = "物流单号";
this.txtTrackNo.Location = new System.Drawing.Point(173,596);
this.txtTrackNo.Name = "txtTrackNo";
this.txtTrackNo.Size = new System.Drawing.Size(100, 21);
this.txtTrackNo.TabIndex = 24;
this.Controls.Add(this.lblTrackNo);
this.Controls.Add(this.txtTrackNo);

           //#####50CustomerPONo###String
this.lblCustomerPONo.AutoSize = true;
this.lblCustomerPONo.Location = new System.Drawing.Point(100,625);
this.lblCustomerPONo.Name = "lblCustomerPONo";
this.lblCustomerPONo.Size = new System.Drawing.Size(41, 12);
this.lblCustomerPONo.TabIndex = 25;
this.lblCustomerPONo.Text = "客户订单号";
this.txtCustomerPONo.Location = new System.Drawing.Point(173,621);
this.txtCustomerPONo.Name = "txtCustomerPONo";
this.txtCustomerPONo.Size = new System.Drawing.Size(100, 21);
this.txtCustomerPONo.TabIndex = 25;
this.Controls.Add(this.lblCustomerPONo);
this.Controls.Add(this.txtCustomerPONo);

           //#####ForeignTotalAmount###Decimal
this.lblForeignTotalAmount.AutoSize = true;
this.lblForeignTotalAmount.Location = new System.Drawing.Point(100,650);
this.lblForeignTotalAmount.Name = "lblForeignTotalAmount";
this.lblForeignTotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblForeignTotalAmount.TabIndex = 26;
this.lblForeignTotalAmount.Text = "金额外币";
//111======650
this.txtForeignTotalAmount.Location = new System.Drawing.Point(173,646);
this.txtForeignTotalAmount.Name ="txtForeignTotalAmount";
this.txtForeignTotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtForeignTotalAmount.TabIndex = 26;
this.Controls.Add(this.lblForeignTotalAmount);
this.Controls.Add(this.txtForeignTotalAmount);

           //#####CollectedMoney###Decimal
this.lblCollectedMoney.AutoSize = true;
this.lblCollectedMoney.Location = new System.Drawing.Point(100,675);
this.lblCollectedMoney.Name = "lblCollectedMoney";
this.lblCollectedMoney.Size = new System.Drawing.Size(41, 12);
this.lblCollectedMoney.TabIndex = 27;
this.lblCollectedMoney.Text = "实收金额";
//111======675
this.txtCollectedMoney.Location = new System.Drawing.Point(173,671);
this.txtCollectedMoney.Name ="txtCollectedMoney";
this.txtCollectedMoney.Size = new System.Drawing.Size(100, 21);
this.txtCollectedMoney.TabIndex = 27;
this.Controls.Add(this.lblCollectedMoney);
this.Controls.Add(this.txtCollectedMoney);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,700);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 28;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,696);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 28;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

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

           //#####1500Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,825);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 33;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,821);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 33;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####200ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,850);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 34;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,846);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 34;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####Approver_by###Int64
//属性测试875Approver_by

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,900);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 36;
this.lblApprover_at.Text = "审批时间";
//111======900
this.dtpApprover_at.Location = new System.Drawing.Point(173,896);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 36;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,950);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 38;
this.lblApprovalResults.Text = "审批结果";
this.chkApprovalResults.Location = new System.Drawing.Point(173,946);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 38;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####KeepAccountsType###Int32
//属性测试975KeepAccountsType

           //#####ForeignDeposit###Decimal
this.lblForeignDeposit.AutoSize = true;
this.lblForeignDeposit.Location = new System.Drawing.Point(100,1000);
this.lblForeignDeposit.Name = "lblForeignDeposit";
this.lblForeignDeposit.Size = new System.Drawing.Size(41, 12);
this.lblForeignDeposit.TabIndex = 40;
this.lblForeignDeposit.Text = "订金外币";
//111======1000
this.txtForeignDeposit.Location = new System.Drawing.Point(173,996);
this.txtForeignDeposit.Name ="txtForeignDeposit";
this.txtForeignDeposit.Size = new System.Drawing.Size(100, 21);
this.txtForeignDeposit.TabIndex = 40;
this.Controls.Add(this.lblForeignDeposit);
this.Controls.Add(this.txtForeignDeposit);

           //#####Deposit###Decimal
this.lblDeposit.AutoSize = true;
this.lblDeposit.Location = new System.Drawing.Point(100,1025);
this.lblDeposit.Name = "lblDeposit";
this.lblDeposit.Size = new System.Drawing.Size(41, 12);
this.lblDeposit.TabIndex = 41;
this.lblDeposit.Text = "订金";
//111======1025
this.txtDeposit.Location = new System.Drawing.Point(173,1021);
this.txtDeposit.Name ="txtDeposit";
this.txtDeposit.Size = new System.Drawing.Size(100, 21);
this.txtDeposit.TabIndex = 41;
this.Controls.Add(this.lblDeposit);
this.Controls.Add(this.txtDeposit);

           //#####FreightCost###Decimal
this.lblFreightCost.AutoSize = true;
this.lblFreightCost.Location = new System.Drawing.Point(100,1050);
this.lblFreightCost.Name = "lblFreightCost";
this.lblFreightCost.Size = new System.Drawing.Size(41, 12);
this.lblFreightCost.TabIndex = 42;
this.lblFreightCost.Text = "运费成本";
//111======1050
this.txtFreightCost.Location = new System.Drawing.Point(173,1046);
this.txtFreightCost.Name ="txtFreightCost";
this.txtFreightCost.Size = new System.Drawing.Size(100, 21);
this.txtFreightCost.TabIndex = 42;
this.Controls.Add(this.lblFreightCost);
this.Controls.Add(this.txtFreightCost);

           //#####TaxDeductionType###Int32
//属性测试1075TaxDeductionType

           //#####PrintStatus###Int32
//属性测试1100PrintStatus

           //#####DataStatus###Int32
//属性测试1125DataStatus

           //#####TotalCommissionAmount###Decimal
this.lblTotalCommissionAmount.AutoSize = true;
this.lblTotalCommissionAmount.Location = new System.Drawing.Point(100,1150);
this.lblTotalCommissionAmount.Name = "lblTotalCommissionAmount";
this.lblTotalCommissionAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalCommissionAmount.TabIndex = 46;
this.lblTotalCommissionAmount.Text = "佣金金额";
//111======1150
this.txtTotalCommissionAmount.Location = new System.Drawing.Point(173,1146);
this.txtTotalCommissionAmount.Name ="txtTotalCommissionAmount";
this.txtTotalCommissionAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalCommissionAmount.TabIndex = 46;
this.Controls.Add(this.lblTotalCommissionAmount);
this.Controls.Add(this.txtTotalCommissionAmount);

           //#####TotalCost###Decimal
this.lblTotalCost.AutoSize = true;
this.lblTotalCost.Location = new System.Drawing.Point(100,1175);
this.lblTotalCost.Name = "lblTotalCost";
this.lblTotalCost.Size = new System.Drawing.Size(41, 12);
this.lblTotalCost.TabIndex = 47;
this.lblTotalCost.Text = "总成本";
//111======1175
this.txtTotalCost.Location = new System.Drawing.Point(173,1171);
this.txtTotalCost.Name ="txtTotalCost";
this.txtTotalCost.Size = new System.Drawing.Size(100, 21);
this.txtTotalCost.TabIndex = 47;
this.Controls.Add(this.lblTotalCost);
this.Controls.Add(this.txtTotalCost);

           //#####TaxRate###Decimal
this.lblTaxRate.AutoSize = true;
this.lblTaxRate.Location = new System.Drawing.Point(100,1200);
this.lblTaxRate.Name = "lblTaxRate";
this.lblTaxRate.Size = new System.Drawing.Size(41, 12);
this.lblTaxRate.TabIndex = 48;
this.lblTaxRate.Text = "税率";
//111======1200
this.txtTaxRate.Location = new System.Drawing.Point(173,1196);
this.txtTaxRate.Name ="txtTaxRate";
this.txtTaxRate.Size = new System.Drawing.Size(100, 21);
this.txtTaxRate.TabIndex = 48;
this.Controls.Add(this.lblTaxRate);
this.Controls.Add(this.txtTaxRate);

           //#####TotalTaxAmount###Decimal
this.lblTotalTaxAmount.AutoSize = true;
this.lblTotalTaxAmount.Location = new System.Drawing.Point(100,1225);
this.lblTotalTaxAmount.Name = "lblTotalTaxAmount";
this.lblTotalTaxAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalTaxAmount.TabIndex = 49;
this.lblTotalTaxAmount.Text = "总税额";
//111======1225
this.txtTotalTaxAmount.Location = new System.Drawing.Point(173,1221);
this.txtTotalTaxAmount.Name ="txtTotalTaxAmount";
this.txtTotalTaxAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalTaxAmount.TabIndex = 49;
this.Controls.Add(this.lblTotalTaxAmount);
this.Controls.Add(this.txtTotalTaxAmount);

           //#####TotalUntaxedAmount###Decimal
this.lblTotalUntaxedAmount.AutoSize = true;
this.lblTotalUntaxedAmount.Location = new System.Drawing.Point(100,1250);
this.lblTotalUntaxedAmount.Name = "lblTotalUntaxedAmount";
this.lblTotalUntaxedAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalUntaxedAmount.TabIndex = 50;
this.lblTotalUntaxedAmount.Text = "未税本位币";
//111======1250
this.txtTotalUntaxedAmount.Location = new System.Drawing.Point(173,1246);
this.txtTotalUntaxedAmount.Name ="txtTotalUntaxedAmount";
this.txtTotalUntaxedAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalUntaxedAmount.TabIndex = 50;
this.Controls.Add(this.lblTotalUntaxedAmount);
this.Controls.Add(this.txtTotalUntaxedAmount);

           //#####GenerateVouchers###Boolean
this.lblGenerateVouchers.AutoSize = true;
this.lblGenerateVouchers.Location = new System.Drawing.Point(100,1275);
this.lblGenerateVouchers.Name = "lblGenerateVouchers";
this.lblGenerateVouchers.Size = new System.Drawing.Size(41, 12);
this.lblGenerateVouchers.TabIndex = 51;
this.lblGenerateVouchers.Text = "生成凭证";
this.chkGenerateVouchers.Location = new System.Drawing.Point(173,1271);
this.chkGenerateVouchers.Name = "chkGenerateVouchers";
this.chkGenerateVouchers.Size = new System.Drawing.Size(100, 21);
this.chkGenerateVouchers.TabIndex = 51;
this.Controls.Add(this.lblGenerateVouchers);
this.Controls.Add(this.chkGenerateVouchers);

           //#####DiscountAmount###Decimal
this.lblDiscountAmount.AutoSize = true;
this.lblDiscountAmount.Location = new System.Drawing.Point(100,1300);
this.lblDiscountAmount.Name = "lblDiscountAmount";
this.lblDiscountAmount.Size = new System.Drawing.Size(41, 12);
this.lblDiscountAmount.TabIndex = 52;
this.lblDiscountAmount.Text = "优惠金额";
//111======1300
this.txtDiscountAmount.Location = new System.Drawing.Point(173,1296);
this.txtDiscountAmount.Name ="txtDiscountAmount";
this.txtDiscountAmount.Size = new System.Drawing.Size(100, 21);
this.txtDiscountAmount.TabIndex = 52;
this.Controls.Add(this.lblDiscountAmount);
this.Controls.Add(this.txtDiscountAmount);

           //#####PrePayMoney###Decimal
this.lblPrePayMoney.AutoSize = true;
this.lblPrePayMoney.Location = new System.Drawing.Point(100,1325);
this.lblPrePayMoney.Name = "lblPrePayMoney";
this.lblPrePayMoney.Size = new System.Drawing.Size(41, 12);
this.lblPrePayMoney.TabIndex = 53;
this.lblPrePayMoney.Text = "预收款";
//111======1325
this.txtPrePayMoney.Location = new System.Drawing.Point(173,1321);
this.txtPrePayMoney.Name ="txtPrePayMoney";
this.txtPrePayMoney.Size = new System.Drawing.Size(100, 21);
this.txtPrePayMoney.TabIndex = 53;
this.Controls.Add(this.lblPrePayMoney);
this.Controls.Add(this.txtPrePayMoney);

           //#####ReplaceOut###Boolean
this.lblReplaceOut.AutoSize = true;
this.lblReplaceOut.Location = new System.Drawing.Point(100,1350);
this.lblReplaceOut.Name = "lblReplaceOut";
this.lblReplaceOut.Size = new System.Drawing.Size(41, 12);
this.lblReplaceOut.TabIndex = 54;
this.lblReplaceOut.Text = "替代出库";
this.chkReplaceOut.Location = new System.Drawing.Point(173,1346);
this.chkReplaceOut.Name = "chkReplaceOut";
this.chkReplaceOut.Size = new System.Drawing.Size(100, 21);
this.chkReplaceOut.TabIndex = 54;
this.Controls.Add(this.lblReplaceOut);
this.Controls.Add(this.chkReplaceOut);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                
                
                this.Controls.Add(this.lblSOrder_ID );
this.Controls.Add(this.cmbSOrder_ID );

                this.Controls.Add(this.lblSaleOrderNo );
this.Controls.Add(this.txtSaleOrderNo );

                this.Controls.Add(this.lblSaleOutNo );
this.Controls.Add(this.txtSaleOutNo );

                
                
                
                this.Controls.Add(this.lblExchangeRate );
this.Controls.Add(this.txtExchangeRate );

                
                
                this.Controls.Add(this.lblForeignFreightIncome );
this.Controls.Add(this.txtForeignFreightIncome );

                this.Controls.Add(this.lblFreightIncome );
this.Controls.Add(this.txtFreightIncome );

                
                this.Controls.Add(this.lblTotalAmount );
this.Controls.Add(this.txtTotalAmount );

                this.Controls.Add(this.lblOutDate );
this.Controls.Add(this.dtpOutDate );

                this.Controls.Add(this.lblDeliveryDate );
this.Controls.Add(this.dtpDeliveryDate );

                this.Controls.Add(this.lblDueDate );
this.Controls.Add(this.dtpDueDate );

                this.Controls.Add(this.lblShippingAddress );
this.Controls.Add(this.txtShippingAddress );

                this.Controls.Add(this.lblShippingWay );
this.Controls.Add(this.txtShippingWay );

                this.Controls.Add(this.lblPlatformOrderNo );
this.Controls.Add(this.txtPlatformOrderNo );

                this.Controls.Add(this.lblIsCustomizedOrder );
this.Controls.Add(this.chkIsCustomizedOrder );

                this.Controls.Add(this.lblIsFromPlatform );
this.Controls.Add(this.chkIsFromPlatform );

                this.Controls.Add(this.lblTrackNo );
this.Controls.Add(this.txtTrackNo );

                this.Controls.Add(this.lblCustomerPONo );
this.Controls.Add(this.txtCustomerPONo );

                this.Controls.Add(this.lblForeignTotalAmount );
this.Controls.Add(this.txtForeignTotalAmount );

                this.Controls.Add(this.lblCollectedMoney );
this.Controls.Add(this.txtCollectedMoney );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                
                this.Controls.Add(this.lblApprover_at );
this.Controls.Add(this.dtpApprover_at );

                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                
                this.Controls.Add(this.lblForeignDeposit );
this.Controls.Add(this.txtForeignDeposit );

                this.Controls.Add(this.lblDeposit );
this.Controls.Add(this.txtDeposit );

                this.Controls.Add(this.lblFreightCost );
this.Controls.Add(this.txtFreightCost );

                
                
                
                this.Controls.Add(this.lblTotalCommissionAmount );
this.Controls.Add(this.txtTotalCommissionAmount );

                this.Controls.Add(this.lblTotalCost );
this.Controls.Add(this.txtTotalCost );

                this.Controls.Add(this.lblTaxRate );
this.Controls.Add(this.txtTaxRate );

                this.Controls.Add(this.lblTotalTaxAmount );
this.Controls.Add(this.txtTotalTaxAmount );

                this.Controls.Add(this.lblTotalUntaxedAmount );
this.Controls.Add(this.txtTotalUntaxedAmount );

                this.Controls.Add(this.lblGenerateVouchers );
this.Controls.Add(this.chkGenerateVouchers );

                this.Controls.Add(this.lblDiscountAmount );
this.Controls.Add(this.txtDiscountAmount );

                this.Controls.Add(this.lblPrePayMoney );
this.Controls.Add(this.txtPrePayMoney );

                this.Controls.Add(this.lblReplaceOut );
this.Controls.Add(this.chkReplaceOut );

                    
            this.Name = "tb_SaleOutQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSOrder_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbSOrder_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSaleOrderNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSaleOrderNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSaleOutNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSaleOutNo;

    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblExchangeRate;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtExchangeRate;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblForeignFreightIncome;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtForeignFreightIncome;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblFreightIncome;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtFreightIncome;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblOutDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpOutDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDeliveryDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpDeliveryDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDueDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpDueDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblShippingAddress;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtShippingAddress;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblShippingWay;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtShippingWay;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPlatformOrderNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPlatformOrderNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsCustomizedOrder;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsCustomizedOrder;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsFromPlatform;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsFromPlatform;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTrackNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTrackNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerPONo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCustomerPONo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblForeignTotalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtForeignTotalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCollectedMoney;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCollectedMoney;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprover_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalResults;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblForeignDeposit;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtForeignDeposit;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDeposit;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDeposit;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblFreightCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtFreightCost;

    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalCommissionAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalCommissionAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTaxRate;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTaxRate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalTaxAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalTaxAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalUntaxedAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalUntaxedAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblGenerateVouchers;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkGenerateVouchers;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDiscountAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDiscountAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPrePayMoney;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPrePayMoney;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblReplaceOut;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkReplaceOut;

    
    
   
 





    }
}


