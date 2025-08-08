// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:38
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 销售订单统计分析
    /// </summary>
    partial class View_SaleOrderItemsEdit
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
     this.lblSOrder_ID = new Krypton.Toolkit.KryptonLabel();
this.txtSOrder_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblSOrderNo = new Krypton.Toolkit.KryptonLabel();
this.txtSOrderNo = new Krypton.Toolkit.KryptonTextBox();

this.lblSaleDate = new Krypton.Toolkit.KryptonLabel();
this.dtpSaleDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblCustomerVendor_ID = new Krypton.Toolkit.KryptonLabel();
this.txtCustomerVendor_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblSKU = new Krypton.Toolkit.KryptonLabel();
this.txtSKU = new Krypton.Toolkit.KryptonTextBox();

this.lblSpecifications = new Krypton.Toolkit.KryptonLabel();
this.txtSpecifications = new Krypton.Toolkit.KryptonTextBox();
this.txtSpecifications.Multiline = true;

this.lblCNName = new Krypton.Toolkit.KryptonLabel();
this.txtCNName = new Krypton.Toolkit.KryptonTextBox();
this.txtCNName.Multiline = true;

this.lblModel = new Krypton.Toolkit.KryptonLabel();
this.txtModel = new Krypton.Toolkit.KryptonTextBox();

this.lblType_ID = new Krypton.Toolkit.KryptonLabel();
this.txtType_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblPayStatus = new Krypton.Toolkit.KryptonLabel();
this.txtPayStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblPaytype_ID = new Krypton.Toolkit.KryptonLabel();
this.txtPaytype_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
this.txtEmployee_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblProjectGroup_ID = new Krypton.Toolkit.KryptonLabel();
this.txtProjectGroup_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblPlatformOrderNo = new Krypton.Toolkit.KryptonLabel();
this.txtPlatformOrderNo = new Krypton.Toolkit.KryptonTextBox();

this.lblProdDetailID = new Krypton.Toolkit.KryptonLabel();
this.txtProdDetailID = new Krypton.Toolkit.KryptonTextBox();

this.lblUnitPrice = new Krypton.Toolkit.KryptonLabel();
this.txtUnitPrice = new Krypton.Toolkit.KryptonTextBox();

this.lblQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtQuantity = new Krypton.Toolkit.KryptonTextBox();

this.lblDiscount = new Krypton.Toolkit.KryptonLabel();
this.txtDiscount = new Krypton.Toolkit.KryptonTextBox();

this.lblTransactionPrice = new Krypton.Toolkit.KryptonLabel();
this.txtTransactionPrice = new Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalTransAmount = new Krypton.Toolkit.KryptonLabel();
this.txtSubtotalTransAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblCost = new Krypton.Toolkit.KryptonLabel();
this.txtCost = new Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalCostAmount = new Krypton.Toolkit.KryptonLabel();
this.txtSubtotalCostAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalDeliveredQty = new Krypton.Toolkit.KryptonLabel();
this.txtTotalDeliveredQty = new Krypton.Toolkit.KryptonTextBox();

this.lblCommissionAmount = new Krypton.Toolkit.KryptonLabel();
this.txtCommissionAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblSummary = new Krypton.Toolkit.KryptonLabel();
this.txtSummary = new Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

this.lblCustomerPartNo = new Krypton.Toolkit.KryptonLabel();
this.txtCustomerPartNo = new Krypton.Toolkit.KryptonTextBox();

this.lblGift = new Krypton.Toolkit.KryptonLabel();
this.chkGift = new Krypton.Toolkit.KryptonCheckBox();
this.chkGift.Values.Text ="";

this.lblTotalReturnedQty = new Krypton.Toolkit.KryptonLabel();
this.txtTotalReturnedQty = new Krypton.Toolkit.KryptonTextBox();

this.lblproperty = new Krypton.Toolkit.KryptonLabel();
this.txtproperty = new Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

this.lblProductNo = new Krypton.Toolkit.KryptonLabel();
this.txtProductNo = new Krypton.Toolkit.KryptonTextBox();

this.lblCategory_ID = new Krypton.Toolkit.KryptonLabel();
this.txtCategory_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblUnit_ID = new Krypton.Toolkit.KryptonLabel();
this.txtUnit_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblDataStatus = new Krypton.Toolkit.KryptonLabel();
this.txtDataStatus = new Krypton.Toolkit.KryptonTextBox();


    
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
     
            //#####SOrder_ID###Int64
this.lblSOrder_ID.AutoSize = true;
this.lblSOrder_ID.Location = new System.Drawing.Point(100,25);
this.lblSOrder_ID.Name = "lblSOrder_ID";
this.lblSOrder_ID.Size = new System.Drawing.Size(41, 12);
this.lblSOrder_ID.TabIndex = 1;
this.lblSOrder_ID.Text = "";
this.txtSOrder_ID.Location = new System.Drawing.Point(173,21);
this.txtSOrder_ID.Name = "txtSOrder_ID";
this.txtSOrder_ID.Size = new System.Drawing.Size(100, 21);
this.txtSOrder_ID.TabIndex = 1;
this.Controls.Add(this.lblSOrder_ID);
this.Controls.Add(this.txtSOrder_ID);

           //#####50SOrderNo###String
this.lblSOrderNo.AutoSize = true;
this.lblSOrderNo.Location = new System.Drawing.Point(100,50);
this.lblSOrderNo.Name = "lblSOrderNo";
this.lblSOrderNo.Size = new System.Drawing.Size(41, 12);
this.lblSOrderNo.TabIndex = 2;
this.lblSOrderNo.Text = "";
this.txtSOrderNo.Location = new System.Drawing.Point(173,46);
this.txtSOrderNo.Name = "txtSOrderNo";
this.txtSOrderNo.Size = new System.Drawing.Size(100, 21);
this.txtSOrderNo.TabIndex = 2;
this.Controls.Add(this.lblSOrderNo);
this.Controls.Add(this.txtSOrderNo);

           //#####SaleDate###DateTime
this.lblSaleDate.AutoSize = true;
this.lblSaleDate.Location = new System.Drawing.Point(100,75);
this.lblSaleDate.Name = "lblSaleDate";
this.lblSaleDate.Size = new System.Drawing.Size(41, 12);
this.lblSaleDate.TabIndex = 3;
this.lblSaleDate.Text = "";
//111======75
this.dtpSaleDate.Location = new System.Drawing.Point(173,71);
this.dtpSaleDate.Name ="dtpSaleDate";
this.dtpSaleDate.ShowCheckBox =true;
this.dtpSaleDate.Size = new System.Drawing.Size(100, 21);
this.dtpSaleDate.TabIndex = 3;
this.Controls.Add(this.lblSaleDate);
this.Controls.Add(this.dtpSaleDate);

           //#####CustomerVendor_ID###Int64
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,100);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 4;
this.lblCustomerVendor_ID.Text = "";
this.txtCustomerVendor_ID.Location = new System.Drawing.Point(173,96);
this.txtCustomerVendor_ID.Name = "txtCustomerVendor_ID";
this.txtCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.txtCustomerVendor_ID.TabIndex = 4;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.txtCustomerVendor_ID);

           //#####80SKU###String
this.lblSKU.AutoSize = true;
this.lblSKU.Location = new System.Drawing.Point(100,125);
this.lblSKU.Name = "lblSKU";
this.lblSKU.Size = new System.Drawing.Size(41, 12);
this.lblSKU.TabIndex = 5;
this.lblSKU.Text = "";
this.txtSKU.Location = new System.Drawing.Point(173,121);
this.txtSKU.Name = "txtSKU";
this.txtSKU.Size = new System.Drawing.Size(100, 21);
this.txtSKU.TabIndex = 5;
this.Controls.Add(this.lblSKU);
this.Controls.Add(this.txtSKU);

           //#####1000Specifications###String
this.lblSpecifications.AutoSize = true;
this.lblSpecifications.Location = new System.Drawing.Point(100,150);
this.lblSpecifications.Name = "lblSpecifications";
this.lblSpecifications.Size = new System.Drawing.Size(41, 12);
this.lblSpecifications.TabIndex = 6;
this.lblSpecifications.Text = "";
this.txtSpecifications.Location = new System.Drawing.Point(173,146);
this.txtSpecifications.Name = "txtSpecifications";
this.txtSpecifications.Size = new System.Drawing.Size(100, 21);
this.txtSpecifications.TabIndex = 6;
this.Controls.Add(this.lblSpecifications);
this.Controls.Add(this.txtSpecifications);

           //#####255CNName###String
this.lblCNName.AutoSize = true;
this.lblCNName.Location = new System.Drawing.Point(100,175);
this.lblCNName.Name = "lblCNName";
this.lblCNName.Size = new System.Drawing.Size(41, 12);
this.lblCNName.TabIndex = 7;
this.lblCNName.Text = "";
this.txtCNName.Location = new System.Drawing.Point(173,171);
this.txtCNName.Name = "txtCNName";
this.txtCNName.Size = new System.Drawing.Size(100, 21);
this.txtCNName.TabIndex = 7;
this.Controls.Add(this.lblCNName);
this.Controls.Add(this.txtCNName);

           //#####50Model###String
this.lblModel.AutoSize = true;
this.lblModel.Location = new System.Drawing.Point(100,200);
this.lblModel.Name = "lblModel";
this.lblModel.Size = new System.Drawing.Size(41, 12);
this.lblModel.TabIndex = 8;
this.lblModel.Text = "";
this.txtModel.Location = new System.Drawing.Point(173,196);
this.txtModel.Name = "txtModel";
this.txtModel.Size = new System.Drawing.Size(100, 21);
this.txtModel.TabIndex = 8;
this.Controls.Add(this.lblModel);
this.Controls.Add(this.txtModel);

           //#####Type_ID###Int64
this.lblType_ID.AutoSize = true;
this.lblType_ID.Location = new System.Drawing.Point(100,225);
this.lblType_ID.Name = "lblType_ID";
this.lblType_ID.Size = new System.Drawing.Size(41, 12);
this.lblType_ID.TabIndex = 9;
this.lblType_ID.Text = "";
this.txtType_ID.Location = new System.Drawing.Point(173,221);
this.txtType_ID.Name = "txtType_ID";
this.txtType_ID.Size = new System.Drawing.Size(100, 21);
this.txtType_ID.TabIndex = 9;
this.Controls.Add(this.lblType_ID);
this.Controls.Add(this.txtType_ID);

           //#####PayStatus###Int32
this.lblPayStatus.AutoSize = true;
this.lblPayStatus.Location = new System.Drawing.Point(100,250);
this.lblPayStatus.Name = "lblPayStatus";
this.lblPayStatus.Size = new System.Drawing.Size(41, 12);
this.lblPayStatus.TabIndex = 10;
this.lblPayStatus.Text = "";
this.txtPayStatus.Location = new System.Drawing.Point(173,246);
this.txtPayStatus.Name = "txtPayStatus";
this.txtPayStatus.Size = new System.Drawing.Size(100, 21);
this.txtPayStatus.TabIndex = 10;
this.Controls.Add(this.lblPayStatus);
this.Controls.Add(this.txtPayStatus);

           //#####Paytype_ID###Int64
this.lblPaytype_ID.AutoSize = true;
this.lblPaytype_ID.Location = new System.Drawing.Point(100,275);
this.lblPaytype_ID.Name = "lblPaytype_ID";
this.lblPaytype_ID.Size = new System.Drawing.Size(41, 12);
this.lblPaytype_ID.TabIndex = 11;
this.lblPaytype_ID.Text = "";
this.txtPaytype_ID.Location = new System.Drawing.Point(173,271);
this.txtPaytype_ID.Name = "txtPaytype_ID";
this.txtPaytype_ID.Size = new System.Drawing.Size(100, 21);
this.txtPaytype_ID.TabIndex = 11;
this.Controls.Add(this.lblPaytype_ID);
this.Controls.Add(this.txtPaytype_ID);

           //#####Employee_ID###Int64
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,300);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 12;
this.lblEmployee_ID.Text = "";
this.txtEmployee_ID.Location = new System.Drawing.Point(173,296);
this.txtEmployee_ID.Name = "txtEmployee_ID";
this.txtEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.txtEmployee_ID.TabIndex = 12;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.txtEmployee_ID);

           //#####ProjectGroup_ID###Int64
this.lblProjectGroup_ID.AutoSize = true;
this.lblProjectGroup_ID.Location = new System.Drawing.Point(100,325);
this.lblProjectGroup_ID.Name = "lblProjectGroup_ID";
this.lblProjectGroup_ID.Size = new System.Drawing.Size(41, 12);
this.lblProjectGroup_ID.TabIndex = 13;
this.lblProjectGroup_ID.Text = "";
this.txtProjectGroup_ID.Location = new System.Drawing.Point(173,321);
this.txtProjectGroup_ID.Name = "txtProjectGroup_ID";
this.txtProjectGroup_ID.Size = new System.Drawing.Size(100, 21);
this.txtProjectGroup_ID.TabIndex = 13;
this.Controls.Add(this.lblProjectGroup_ID);
this.Controls.Add(this.txtProjectGroup_ID);

           //#####100PlatformOrderNo###String
this.lblPlatformOrderNo.AutoSize = true;
this.lblPlatformOrderNo.Location = new System.Drawing.Point(100,350);
this.lblPlatformOrderNo.Name = "lblPlatformOrderNo";
this.lblPlatformOrderNo.Size = new System.Drawing.Size(41, 12);
this.lblPlatformOrderNo.TabIndex = 14;
this.lblPlatformOrderNo.Text = "";
this.txtPlatformOrderNo.Location = new System.Drawing.Point(173,346);
this.txtPlatformOrderNo.Name = "txtPlatformOrderNo";
this.txtPlatformOrderNo.Size = new System.Drawing.Size(100, 21);
this.txtPlatformOrderNo.TabIndex = 14;
this.Controls.Add(this.lblPlatformOrderNo);
this.Controls.Add(this.txtPlatformOrderNo);

           //#####ProdDetailID###Int64
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,375);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 15;
this.lblProdDetailID.Text = "";
this.txtProdDetailID.Location = new System.Drawing.Point(173,371);
this.txtProdDetailID.Name = "txtProdDetailID";
this.txtProdDetailID.Size = new System.Drawing.Size(100, 21);
this.txtProdDetailID.TabIndex = 15;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.txtProdDetailID);

           //#####UnitPrice###Decimal
this.lblUnitPrice.AutoSize = true;
this.lblUnitPrice.Location = new System.Drawing.Point(100,400);
this.lblUnitPrice.Name = "lblUnitPrice";
this.lblUnitPrice.Size = new System.Drawing.Size(41, 12);
this.lblUnitPrice.TabIndex = 16;
this.lblUnitPrice.Text = "";
//111======400
this.txtUnitPrice.Location = new System.Drawing.Point(173,396);
this.txtUnitPrice.Name ="txtUnitPrice";
this.txtUnitPrice.Size = new System.Drawing.Size(100, 21);
this.txtUnitPrice.TabIndex = 16;
this.Controls.Add(this.lblUnitPrice);
this.Controls.Add(this.txtUnitPrice);

           //#####Quantity###Int32
this.lblQuantity.AutoSize = true;
this.lblQuantity.Location = new System.Drawing.Point(100,425);
this.lblQuantity.Name = "lblQuantity";
this.lblQuantity.Size = new System.Drawing.Size(41, 12);
this.lblQuantity.TabIndex = 17;
this.lblQuantity.Text = "";
this.txtQuantity.Location = new System.Drawing.Point(173,421);
this.txtQuantity.Name = "txtQuantity";
this.txtQuantity.Size = new System.Drawing.Size(100, 21);
this.txtQuantity.TabIndex = 17;
this.Controls.Add(this.lblQuantity);
this.Controls.Add(this.txtQuantity);

           //#####Discount###Decimal
this.lblDiscount.AutoSize = true;
this.lblDiscount.Location = new System.Drawing.Point(100,450);
this.lblDiscount.Name = "lblDiscount";
this.lblDiscount.Size = new System.Drawing.Size(41, 12);
this.lblDiscount.TabIndex = 18;
this.lblDiscount.Text = "";
//111======450
this.txtDiscount.Location = new System.Drawing.Point(173,446);
this.txtDiscount.Name ="txtDiscount";
this.txtDiscount.Size = new System.Drawing.Size(100, 21);
this.txtDiscount.TabIndex = 18;
this.Controls.Add(this.lblDiscount);
this.Controls.Add(this.txtDiscount);

           //#####TransactionPrice###Decimal
this.lblTransactionPrice.AutoSize = true;
this.lblTransactionPrice.Location = new System.Drawing.Point(100,475);
this.lblTransactionPrice.Name = "lblTransactionPrice";
this.lblTransactionPrice.Size = new System.Drawing.Size(41, 12);
this.lblTransactionPrice.TabIndex = 19;
this.lblTransactionPrice.Text = "";
//111======475
this.txtTransactionPrice.Location = new System.Drawing.Point(173,471);
this.txtTransactionPrice.Name ="txtTransactionPrice";
this.txtTransactionPrice.Size = new System.Drawing.Size(100, 21);
this.txtTransactionPrice.TabIndex = 19;
this.Controls.Add(this.lblTransactionPrice);
this.Controls.Add(this.txtTransactionPrice);

           //#####SubtotalTransAmount###Decimal
this.lblSubtotalTransAmount.AutoSize = true;
this.lblSubtotalTransAmount.Location = new System.Drawing.Point(100,500);
this.lblSubtotalTransAmount.Name = "lblSubtotalTransAmount";
this.lblSubtotalTransAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalTransAmount.TabIndex = 20;
this.lblSubtotalTransAmount.Text = "";
//111======500
this.txtSubtotalTransAmount.Location = new System.Drawing.Point(173,496);
this.txtSubtotalTransAmount.Name ="txtSubtotalTransAmount";
this.txtSubtotalTransAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalTransAmount.TabIndex = 20;
this.Controls.Add(this.lblSubtotalTransAmount);
this.Controls.Add(this.txtSubtotalTransAmount);

           //#####Cost###Decimal
this.lblCost.AutoSize = true;
this.lblCost.Location = new System.Drawing.Point(100,525);
this.lblCost.Name = "lblCost";
this.lblCost.Size = new System.Drawing.Size(41, 12);
this.lblCost.TabIndex = 21;
this.lblCost.Text = "";
//111======525
this.txtCost.Location = new System.Drawing.Point(173,521);
this.txtCost.Name ="txtCost";
this.txtCost.Size = new System.Drawing.Size(100, 21);
this.txtCost.TabIndex = 21;
this.Controls.Add(this.lblCost);
this.Controls.Add(this.txtCost);

           //#####SubtotalCostAmount###Decimal
this.lblSubtotalCostAmount.AutoSize = true;
this.lblSubtotalCostAmount.Location = new System.Drawing.Point(100,550);
this.lblSubtotalCostAmount.Name = "lblSubtotalCostAmount";
this.lblSubtotalCostAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalCostAmount.TabIndex = 22;
this.lblSubtotalCostAmount.Text = "";
//111======550
this.txtSubtotalCostAmount.Location = new System.Drawing.Point(173,546);
this.txtSubtotalCostAmount.Name ="txtSubtotalCostAmount";
this.txtSubtotalCostAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalCostAmount.TabIndex = 22;
this.Controls.Add(this.lblSubtotalCostAmount);
this.Controls.Add(this.txtSubtotalCostAmount);

           //#####TotalDeliveredQty###Int32
this.lblTotalDeliveredQty.AutoSize = true;
this.lblTotalDeliveredQty.Location = new System.Drawing.Point(100,575);
this.lblTotalDeliveredQty.Name = "lblTotalDeliveredQty";
this.lblTotalDeliveredQty.Size = new System.Drawing.Size(41, 12);
this.lblTotalDeliveredQty.TabIndex = 23;
this.lblTotalDeliveredQty.Text = "";
this.txtTotalDeliveredQty.Location = new System.Drawing.Point(173,571);
this.txtTotalDeliveredQty.Name = "txtTotalDeliveredQty";
this.txtTotalDeliveredQty.Size = new System.Drawing.Size(100, 21);
this.txtTotalDeliveredQty.TabIndex = 23;
this.Controls.Add(this.lblTotalDeliveredQty);
this.Controls.Add(this.txtTotalDeliveredQty);

           //#####CommissionAmount###Decimal
this.lblCommissionAmount.AutoSize = true;
this.lblCommissionAmount.Location = new System.Drawing.Point(100,600);
this.lblCommissionAmount.Name = "lblCommissionAmount";
this.lblCommissionAmount.Size = new System.Drawing.Size(41, 12);
this.lblCommissionAmount.TabIndex = 24;
this.lblCommissionAmount.Text = "";
//111======600
this.txtCommissionAmount.Location = new System.Drawing.Point(173,596);
this.txtCommissionAmount.Name ="txtCommissionAmount";
this.txtCommissionAmount.Size = new System.Drawing.Size(100, 21);
this.txtCommissionAmount.TabIndex = 24;
this.Controls.Add(this.lblCommissionAmount);
this.Controls.Add(this.txtCommissionAmount);

           //#####1000Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,625);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 25;
this.lblSummary.Text = "";
this.txtSummary.Location = new System.Drawing.Point(173,621);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 25;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####100CustomerPartNo###String
this.lblCustomerPartNo.AutoSize = true;
this.lblCustomerPartNo.Location = new System.Drawing.Point(100,650);
this.lblCustomerPartNo.Name = "lblCustomerPartNo";
this.lblCustomerPartNo.Size = new System.Drawing.Size(41, 12);
this.lblCustomerPartNo.TabIndex = 26;
this.lblCustomerPartNo.Text = "";
this.txtCustomerPartNo.Location = new System.Drawing.Point(173,646);
this.txtCustomerPartNo.Name = "txtCustomerPartNo";
this.txtCustomerPartNo.Size = new System.Drawing.Size(100, 21);
this.txtCustomerPartNo.TabIndex = 26;
this.Controls.Add(this.lblCustomerPartNo);
this.Controls.Add(this.txtCustomerPartNo);

           //#####Gift###Boolean
this.lblGift.AutoSize = true;
this.lblGift.Location = new System.Drawing.Point(100,675);
this.lblGift.Name = "lblGift";
this.lblGift.Size = new System.Drawing.Size(41, 12);
this.lblGift.TabIndex = 27;
this.lblGift.Text = "";
this.chkGift.Location = new System.Drawing.Point(173,671);
this.chkGift.Name = "chkGift";
this.chkGift.Size = new System.Drawing.Size(100, 21);
this.chkGift.TabIndex = 27;
this.Controls.Add(this.lblGift);
this.Controls.Add(this.chkGift);

           //#####TotalReturnedQty###Int32
this.lblTotalReturnedQty.AutoSize = true;
this.lblTotalReturnedQty.Location = new System.Drawing.Point(100,700);
this.lblTotalReturnedQty.Name = "lblTotalReturnedQty";
this.lblTotalReturnedQty.Size = new System.Drawing.Size(41, 12);
this.lblTotalReturnedQty.TabIndex = 28;
this.lblTotalReturnedQty.Text = "";
this.txtTotalReturnedQty.Location = new System.Drawing.Point(173,696);
this.txtTotalReturnedQty.Name = "txtTotalReturnedQty";
this.txtTotalReturnedQty.Size = new System.Drawing.Size(100, 21);
this.txtTotalReturnedQty.TabIndex = 28;
this.Controls.Add(this.lblTotalReturnedQty);
this.Controls.Add(this.txtTotalReturnedQty);

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,725);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 29;
this.lblproperty.Text = "";
this.txtproperty.Location = new System.Drawing.Point(173,721);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 29;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

           //#####40ProductNo###String
this.lblProductNo.AutoSize = true;
this.lblProductNo.Location = new System.Drawing.Point(100,750);
this.lblProductNo.Name = "lblProductNo";
this.lblProductNo.Size = new System.Drawing.Size(41, 12);
this.lblProductNo.TabIndex = 30;
this.lblProductNo.Text = "";
this.txtProductNo.Location = new System.Drawing.Point(173,746);
this.txtProductNo.Name = "txtProductNo";
this.txtProductNo.Size = new System.Drawing.Size(100, 21);
this.txtProductNo.TabIndex = 30;
this.Controls.Add(this.lblProductNo);
this.Controls.Add(this.txtProductNo);

           //#####Category_ID###Int64
this.lblCategory_ID.AutoSize = true;
this.lblCategory_ID.Location = new System.Drawing.Point(100,775);
this.lblCategory_ID.Name = "lblCategory_ID";
this.lblCategory_ID.Size = new System.Drawing.Size(41, 12);
this.lblCategory_ID.TabIndex = 31;
this.lblCategory_ID.Text = "";
this.txtCategory_ID.Location = new System.Drawing.Point(173,771);
this.txtCategory_ID.Name = "txtCategory_ID";
this.txtCategory_ID.Size = new System.Drawing.Size(100, 21);
this.txtCategory_ID.TabIndex = 31;
this.Controls.Add(this.lblCategory_ID);
this.Controls.Add(this.txtCategory_ID);

           //#####Unit_ID###Int64
this.lblUnit_ID.AutoSize = true;
this.lblUnit_ID.Location = new System.Drawing.Point(100,800);
this.lblUnit_ID.Name = "lblUnit_ID";
this.lblUnit_ID.Size = new System.Drawing.Size(41, 12);
this.lblUnit_ID.TabIndex = 32;
this.lblUnit_ID.Text = "";
this.txtUnit_ID.Location = new System.Drawing.Point(173,796);
this.txtUnit_ID.Name = "txtUnit_ID";
this.txtUnit_ID.Size = new System.Drawing.Size(100, 21);
this.txtUnit_ID.TabIndex = 32;
this.Controls.Add(this.lblUnit_ID);
this.Controls.Add(this.txtUnit_ID);

           //#####DataStatus###Int32
this.lblDataStatus.AutoSize = true;
this.lblDataStatus.Location = new System.Drawing.Point(100,825);
this.lblDataStatus.Name = "lblDataStatus";
this.lblDataStatus.Size = new System.Drawing.Size(41, 12);
this.lblDataStatus.TabIndex = 33;
this.lblDataStatus.Text = "";
this.txtDataStatus.Location = new System.Drawing.Point(173,821);
this.txtDataStatus.Name = "txtDataStatus";
this.txtDataStatus.Size = new System.Drawing.Size(100, 21);
this.txtDataStatus.TabIndex = 33;
this.Controls.Add(this.lblDataStatus);
this.Controls.Add(this.txtDataStatus);

           //#####ApprovalStatus###SByte

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
           // this.kryptonPanel1.TabIndex = 34;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblSOrder_ID );
this.Controls.Add(this.txtSOrder_ID );

                this.Controls.Add(this.lblSOrderNo );
this.Controls.Add(this.txtSOrderNo );

                this.Controls.Add(this.lblSaleDate );
this.Controls.Add(this.dtpSaleDate );

                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.txtCustomerVendor_ID );

                this.Controls.Add(this.lblSKU );
this.Controls.Add(this.txtSKU );

                this.Controls.Add(this.lblSpecifications );
this.Controls.Add(this.txtSpecifications );

                this.Controls.Add(this.lblCNName );
this.Controls.Add(this.txtCNName );

                this.Controls.Add(this.lblModel );
this.Controls.Add(this.txtModel );

                this.Controls.Add(this.lblType_ID );
this.Controls.Add(this.txtType_ID );

                this.Controls.Add(this.lblPayStatus );
this.Controls.Add(this.txtPayStatus );

                this.Controls.Add(this.lblPaytype_ID );
this.Controls.Add(this.txtPaytype_ID );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.txtEmployee_ID );

                this.Controls.Add(this.lblProjectGroup_ID );
this.Controls.Add(this.txtProjectGroup_ID );

                this.Controls.Add(this.lblPlatformOrderNo );
this.Controls.Add(this.txtPlatformOrderNo );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.txtProdDetailID );

                this.Controls.Add(this.lblUnitPrice );
this.Controls.Add(this.txtUnitPrice );

                this.Controls.Add(this.lblQuantity );
this.Controls.Add(this.txtQuantity );

                this.Controls.Add(this.lblDiscount );
this.Controls.Add(this.txtDiscount );

                this.Controls.Add(this.lblTransactionPrice );
this.Controls.Add(this.txtTransactionPrice );

                this.Controls.Add(this.lblSubtotalTransAmount );
this.Controls.Add(this.txtSubtotalTransAmount );

                this.Controls.Add(this.lblCost );
this.Controls.Add(this.txtCost );

                this.Controls.Add(this.lblSubtotalCostAmount );
this.Controls.Add(this.txtSubtotalCostAmount );

                this.Controls.Add(this.lblTotalDeliveredQty );
this.Controls.Add(this.txtTotalDeliveredQty );

                this.Controls.Add(this.lblCommissionAmount );
this.Controls.Add(this.txtCommissionAmount );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                this.Controls.Add(this.lblCustomerPartNo );
this.Controls.Add(this.txtCustomerPartNo );

                this.Controls.Add(this.lblGift );
this.Controls.Add(this.chkGift );

                this.Controls.Add(this.lblTotalReturnedQty );
this.Controls.Add(this.txtTotalReturnedQty );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                this.Controls.Add(this.lblProductNo );
this.Controls.Add(this.txtProductNo );

                this.Controls.Add(this.lblCategory_ID );
this.Controls.Add(this.txtCategory_ID );

                this.Controls.Add(this.lblUnit_ID );
this.Controls.Add(this.txtUnit_ID );

                this.Controls.Add(this.lblDataStatus );
this.Controls.Add(this.txtDataStatus );

                
                            // 
            // "View_SaleOrderItemsEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "View_SaleOrderItemsEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblSOrder_ID;
private Krypton.Toolkit.KryptonTextBox txtSOrder_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblSOrderNo;
private Krypton.Toolkit.KryptonTextBox txtSOrderNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblSaleDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpSaleDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private Krypton.Toolkit.KryptonTextBox txtCustomerVendor_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblSKU;
private Krypton.Toolkit.KryptonTextBox txtSKU;

    
        
              private Krypton.Toolkit.KryptonLabel lblSpecifications;
private Krypton.Toolkit.KryptonTextBox txtSpecifications;

    
        
              private Krypton.Toolkit.KryptonLabel lblCNName;
private Krypton.Toolkit.KryptonTextBox txtCNName;

    
        
              private Krypton.Toolkit.KryptonLabel lblModel;
private Krypton.Toolkit.KryptonTextBox txtModel;

    
        
              private Krypton.Toolkit.KryptonLabel lblType_ID;
private Krypton.Toolkit.KryptonTextBox txtType_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblPayStatus;
private Krypton.Toolkit.KryptonTextBox txtPayStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblPaytype_ID;
private Krypton.Toolkit.KryptonTextBox txtPaytype_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonTextBox txtEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblProjectGroup_ID;
private Krypton.Toolkit.KryptonTextBox txtProjectGroup_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblPlatformOrderNo;
private Krypton.Toolkit.KryptonTextBox txtPlatformOrderNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblProdDetailID;
private Krypton.Toolkit.KryptonTextBox txtProdDetailID;

    
        
              private Krypton.Toolkit.KryptonLabel lblUnitPrice;
private Krypton.Toolkit.KryptonTextBox txtUnitPrice;

    
        
              private Krypton.Toolkit.KryptonLabel lblQuantity;
private Krypton.Toolkit.KryptonTextBox txtQuantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblDiscount;
private Krypton.Toolkit.KryptonTextBox txtDiscount;

    
        
              private Krypton.Toolkit.KryptonLabel lblTransactionPrice;
private Krypton.Toolkit.KryptonTextBox txtTransactionPrice;

    
        
              private Krypton.Toolkit.KryptonLabel lblSubtotalTransAmount;
private Krypton.Toolkit.KryptonTextBox txtSubtotalTransAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblCost;
private Krypton.Toolkit.KryptonTextBox txtCost;

    
        
              private Krypton.Toolkit.KryptonLabel lblSubtotalCostAmount;
private Krypton.Toolkit.KryptonTextBox txtSubtotalCostAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalDeliveredQty;
private Krypton.Toolkit.KryptonTextBox txtTotalDeliveredQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblCommissionAmount;
private Krypton.Toolkit.KryptonTextBox txtCommissionAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblSummary;
private Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerPartNo;
private Krypton.Toolkit.KryptonTextBox txtCustomerPartNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblGift;
private Krypton.Toolkit.KryptonCheckBox chkGift;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalReturnedQty;
private Krypton.Toolkit.KryptonTextBox txtTotalReturnedQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblproperty;
private Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private Krypton.Toolkit.KryptonLabel lblProductNo;
private Krypton.Toolkit.KryptonTextBox txtProductNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblCategory_ID;
private Krypton.Toolkit.KryptonTextBox txtCategory_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblUnit_ID;
private Krypton.Toolkit.KryptonTextBox txtUnit_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblDataStatus;
private Krypton.Toolkit.KryptonTextBox txtDataStatus;

    
        
              
    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

