// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:39
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 销售退货统计分析
    /// </summary>
    partial class View_SaleOutReItemsEdit
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
     this.lblSKU = new Krypton.Toolkit.KryptonLabel();
this.txtSKU = new Krypton.Toolkit.KryptonTextBox();

this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
this.txtEmployee_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblCustomerVendor_ID = new Krypton.Toolkit.KryptonLabel();
this.txtCustomerVendor_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblReturnNo = new Krypton.Toolkit.KryptonLabel();
this.txtReturnNo = new Krypton.Toolkit.KryptonTextBox();

this.lblPayStatus = new Krypton.Toolkit.KryptonLabel();
this.txtPayStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblPaytype_ID = new Krypton.Toolkit.KryptonLabel();
this.txtPaytype_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblReturnDate = new Krypton.Toolkit.KryptonLabel();
this.dtpReturnDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblSaleOut_NO = new Krypton.Toolkit.KryptonLabel();
this.txtSaleOut_NO = new Krypton.Toolkit.KryptonTextBox();

this.lblProjectGroup_ID = new Krypton.Toolkit.KryptonLabel();
this.txtProjectGroup_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblProdDetailID = new Krypton.Toolkit.KryptonLabel();
this.txtProdDetailID = new Krypton.Toolkit.KryptonTextBox();

this.lblproperty = new Krypton.Toolkit.KryptonLabel();
this.txtproperty = new Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

this.lblLocation_ID = new Krypton.Toolkit.KryptonLabel();
this.txtLocation_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblRack_ID = new Krypton.Toolkit.KryptonLabel();
this.txtRack_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtQuantity = new Krypton.Toolkit.KryptonTextBox();

this.lblTransactionPrice = new Krypton.Toolkit.KryptonLabel();
this.txtTransactionPrice = new Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalTransAmount = new Krypton.Toolkit.KryptonLabel();
this.txtSubtotalTransAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblSummary = new Krypton.Toolkit.KryptonLabel();
this.txtSummary = new Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

this.lblCustomerPartNo = new Krypton.Toolkit.KryptonLabel();
this.txtCustomerPartNo = new Krypton.Toolkit.KryptonTextBox();

this.lblCost = new Krypton.Toolkit.KryptonLabel();
this.txtCost = new Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalCostAmount = new Krypton.Toolkit.KryptonLabel();
this.txtSubtotalCostAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblTaxRate = new Krypton.Toolkit.KryptonLabel();
this.txtTaxRate = new Krypton.Toolkit.KryptonTextBox();

this.lblGift = new Krypton.Toolkit.KryptonLabel();
this.chkGift = new Krypton.Toolkit.KryptonCheckBox();
this.chkGift.Values.Text ="";

this.lblSubtotalUntaxedAmount = new Krypton.Toolkit.KryptonLabel();
this.txtSubtotalUntaxedAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalTaxAmount = new Krypton.Toolkit.KryptonLabel();
this.txtSubtotalTaxAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblCNName = new Krypton.Toolkit.KryptonLabel();
this.txtCNName = new Krypton.Toolkit.KryptonTextBox();
this.txtCNName.Multiline = true;

this.lblSpecifications = new Krypton.Toolkit.KryptonLabel();
this.txtSpecifications = new Krypton.Toolkit.KryptonTextBox();
this.txtSpecifications.Multiline = true;

this.lblProductNo = new Krypton.Toolkit.KryptonLabel();
this.txtProductNo = new Krypton.Toolkit.KryptonTextBox();

this.lblUnit_ID = new Krypton.Toolkit.KryptonLabel();
this.txtUnit_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblModel = new Krypton.Toolkit.KryptonLabel();
this.txtModel = new Krypton.Toolkit.KryptonTextBox();

this.lblCategory_ID = new Krypton.Toolkit.KryptonLabel();
this.txtCategory_ID = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####80SKU###String
this.lblSKU.AutoSize = true;
this.lblSKU.Location = new System.Drawing.Point(100,25);
this.lblSKU.Name = "lblSKU";
this.lblSKU.Size = new System.Drawing.Size(41, 12);
this.lblSKU.TabIndex = 1;
this.lblSKU.Text = "";
this.txtSKU.Location = new System.Drawing.Point(173,21);
this.txtSKU.Name = "txtSKU";
this.txtSKU.Size = new System.Drawing.Size(100, 21);
this.txtSKU.TabIndex = 1;
this.Controls.Add(this.lblSKU);
this.Controls.Add(this.txtSKU);

           //#####Employee_ID###Int64
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,50);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 2;
this.lblEmployee_ID.Text = "";
this.txtEmployee_ID.Location = new System.Drawing.Point(173,46);
this.txtEmployee_ID.Name = "txtEmployee_ID";
this.txtEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.txtEmployee_ID.TabIndex = 2;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.txtEmployee_ID);

           //#####CustomerVendor_ID###Int64
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,75);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 3;
this.lblCustomerVendor_ID.Text = "";
this.txtCustomerVendor_ID.Location = new System.Drawing.Point(173,71);
this.txtCustomerVendor_ID.Name = "txtCustomerVendor_ID";
this.txtCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.txtCustomerVendor_ID.TabIndex = 3;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.txtCustomerVendor_ID);

           //#####50ReturnNo###String
this.lblReturnNo.AutoSize = true;
this.lblReturnNo.Location = new System.Drawing.Point(100,100);
this.lblReturnNo.Name = "lblReturnNo";
this.lblReturnNo.Size = new System.Drawing.Size(41, 12);
this.lblReturnNo.TabIndex = 4;
this.lblReturnNo.Text = "";
this.txtReturnNo.Location = new System.Drawing.Point(173,96);
this.txtReturnNo.Name = "txtReturnNo";
this.txtReturnNo.Size = new System.Drawing.Size(100, 21);
this.txtReturnNo.TabIndex = 4;
this.Controls.Add(this.lblReturnNo);
this.Controls.Add(this.txtReturnNo);

           //#####PayStatus###Int32
this.lblPayStatus.AutoSize = true;
this.lblPayStatus.Location = new System.Drawing.Point(100,125);
this.lblPayStatus.Name = "lblPayStatus";
this.lblPayStatus.Size = new System.Drawing.Size(41, 12);
this.lblPayStatus.TabIndex = 5;
this.lblPayStatus.Text = "";
this.txtPayStatus.Location = new System.Drawing.Point(173,121);
this.txtPayStatus.Name = "txtPayStatus";
this.txtPayStatus.Size = new System.Drawing.Size(100, 21);
this.txtPayStatus.TabIndex = 5;
this.Controls.Add(this.lblPayStatus);
this.Controls.Add(this.txtPayStatus);

           //#####Paytype_ID###Int64
this.lblPaytype_ID.AutoSize = true;
this.lblPaytype_ID.Location = new System.Drawing.Point(100,150);
this.lblPaytype_ID.Name = "lblPaytype_ID";
this.lblPaytype_ID.Size = new System.Drawing.Size(41, 12);
this.lblPaytype_ID.TabIndex = 6;
this.lblPaytype_ID.Text = "";
this.txtPaytype_ID.Location = new System.Drawing.Point(173,146);
this.txtPaytype_ID.Name = "txtPaytype_ID";
this.txtPaytype_ID.Size = new System.Drawing.Size(100, 21);
this.txtPaytype_ID.TabIndex = 6;
this.Controls.Add(this.lblPaytype_ID);
this.Controls.Add(this.txtPaytype_ID);

           //#####ReturnDate###DateTime
this.lblReturnDate.AutoSize = true;
this.lblReturnDate.Location = new System.Drawing.Point(100,175);
this.lblReturnDate.Name = "lblReturnDate";
this.lblReturnDate.Size = new System.Drawing.Size(41, 12);
this.lblReturnDate.TabIndex = 7;
this.lblReturnDate.Text = "";
//111======175
this.dtpReturnDate.Location = new System.Drawing.Point(173,171);
this.dtpReturnDate.Name ="dtpReturnDate";
this.dtpReturnDate.ShowCheckBox =true;
this.dtpReturnDate.Size = new System.Drawing.Size(100, 21);
this.dtpReturnDate.TabIndex = 7;
this.Controls.Add(this.lblReturnDate);
this.Controls.Add(this.dtpReturnDate);

           //#####50SaleOut_NO###String
this.lblSaleOut_NO.AutoSize = true;
this.lblSaleOut_NO.Location = new System.Drawing.Point(100,200);
this.lblSaleOut_NO.Name = "lblSaleOut_NO";
this.lblSaleOut_NO.Size = new System.Drawing.Size(41, 12);
this.lblSaleOut_NO.TabIndex = 8;
this.lblSaleOut_NO.Text = "";
this.txtSaleOut_NO.Location = new System.Drawing.Point(173,196);
this.txtSaleOut_NO.Name = "txtSaleOut_NO";
this.txtSaleOut_NO.Size = new System.Drawing.Size(100, 21);
this.txtSaleOut_NO.TabIndex = 8;
this.Controls.Add(this.lblSaleOut_NO);
this.Controls.Add(this.txtSaleOut_NO);

           //#####ProjectGroup_ID###Int64
this.lblProjectGroup_ID.AutoSize = true;
this.lblProjectGroup_ID.Location = new System.Drawing.Point(100,225);
this.lblProjectGroup_ID.Name = "lblProjectGroup_ID";
this.lblProjectGroup_ID.Size = new System.Drawing.Size(41, 12);
this.lblProjectGroup_ID.TabIndex = 9;
this.lblProjectGroup_ID.Text = "";
this.txtProjectGroup_ID.Location = new System.Drawing.Point(173,221);
this.txtProjectGroup_ID.Name = "txtProjectGroup_ID";
this.txtProjectGroup_ID.Size = new System.Drawing.Size(100, 21);
this.txtProjectGroup_ID.TabIndex = 9;
this.Controls.Add(this.lblProjectGroup_ID);
this.Controls.Add(this.txtProjectGroup_ID);

           //#####ProdDetailID###Int64
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,250);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 10;
this.lblProdDetailID.Text = "";
this.txtProdDetailID.Location = new System.Drawing.Point(173,246);
this.txtProdDetailID.Name = "txtProdDetailID";
this.txtProdDetailID.Size = new System.Drawing.Size(100, 21);
this.txtProdDetailID.TabIndex = 10;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.txtProdDetailID);

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,275);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 11;
this.lblproperty.Text = "";
this.txtproperty.Location = new System.Drawing.Point(173,271);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 11;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

           //#####Location_ID###Int64
this.lblLocation_ID.AutoSize = true;
this.lblLocation_ID.Location = new System.Drawing.Point(100,300);
this.lblLocation_ID.Name = "lblLocation_ID";
this.lblLocation_ID.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID.TabIndex = 12;
this.lblLocation_ID.Text = "";
this.txtLocation_ID.Location = new System.Drawing.Point(173,296);
this.txtLocation_ID.Name = "txtLocation_ID";
this.txtLocation_ID.Size = new System.Drawing.Size(100, 21);
this.txtLocation_ID.TabIndex = 12;
this.Controls.Add(this.lblLocation_ID);
this.Controls.Add(this.txtLocation_ID);

           //#####Rack_ID###Int64
this.lblRack_ID.AutoSize = true;
this.lblRack_ID.Location = new System.Drawing.Point(100,325);
this.lblRack_ID.Name = "lblRack_ID";
this.lblRack_ID.Size = new System.Drawing.Size(41, 12);
this.lblRack_ID.TabIndex = 13;
this.lblRack_ID.Text = "";
this.txtRack_ID.Location = new System.Drawing.Point(173,321);
this.txtRack_ID.Name = "txtRack_ID";
this.txtRack_ID.Size = new System.Drawing.Size(100, 21);
this.txtRack_ID.TabIndex = 13;
this.Controls.Add(this.lblRack_ID);
this.Controls.Add(this.txtRack_ID);

           //#####Quantity###Int32
this.lblQuantity.AutoSize = true;
this.lblQuantity.Location = new System.Drawing.Point(100,350);
this.lblQuantity.Name = "lblQuantity";
this.lblQuantity.Size = new System.Drawing.Size(41, 12);
this.lblQuantity.TabIndex = 14;
this.lblQuantity.Text = "";
this.txtQuantity.Location = new System.Drawing.Point(173,346);
this.txtQuantity.Name = "txtQuantity";
this.txtQuantity.Size = new System.Drawing.Size(100, 21);
this.txtQuantity.TabIndex = 14;
this.Controls.Add(this.lblQuantity);
this.Controls.Add(this.txtQuantity);

           //#####TransactionPrice###Decimal
this.lblTransactionPrice.AutoSize = true;
this.lblTransactionPrice.Location = new System.Drawing.Point(100,375);
this.lblTransactionPrice.Name = "lblTransactionPrice";
this.lblTransactionPrice.Size = new System.Drawing.Size(41, 12);
this.lblTransactionPrice.TabIndex = 15;
this.lblTransactionPrice.Text = "";
//111======375
this.txtTransactionPrice.Location = new System.Drawing.Point(173,371);
this.txtTransactionPrice.Name ="txtTransactionPrice";
this.txtTransactionPrice.Size = new System.Drawing.Size(100, 21);
this.txtTransactionPrice.TabIndex = 15;
this.Controls.Add(this.lblTransactionPrice);
this.Controls.Add(this.txtTransactionPrice);

           //#####SubtotalTransAmount###Decimal
this.lblSubtotalTransAmount.AutoSize = true;
this.lblSubtotalTransAmount.Location = new System.Drawing.Point(100,400);
this.lblSubtotalTransAmount.Name = "lblSubtotalTransAmount";
this.lblSubtotalTransAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalTransAmount.TabIndex = 16;
this.lblSubtotalTransAmount.Text = "";
//111======400
this.txtSubtotalTransAmount.Location = new System.Drawing.Point(173,396);
this.txtSubtotalTransAmount.Name ="txtSubtotalTransAmount";
this.txtSubtotalTransAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalTransAmount.TabIndex = 16;
this.Controls.Add(this.lblSubtotalTransAmount);
this.Controls.Add(this.txtSubtotalTransAmount);

           //#####255Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,425);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 17;
this.lblSummary.Text = "";
this.txtSummary.Location = new System.Drawing.Point(173,421);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 17;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####50CustomerPartNo###String
this.lblCustomerPartNo.AutoSize = true;
this.lblCustomerPartNo.Location = new System.Drawing.Point(100,450);
this.lblCustomerPartNo.Name = "lblCustomerPartNo";
this.lblCustomerPartNo.Size = new System.Drawing.Size(41, 12);
this.lblCustomerPartNo.TabIndex = 18;
this.lblCustomerPartNo.Text = "";
this.txtCustomerPartNo.Location = new System.Drawing.Point(173,446);
this.txtCustomerPartNo.Name = "txtCustomerPartNo";
this.txtCustomerPartNo.Size = new System.Drawing.Size(100, 21);
this.txtCustomerPartNo.TabIndex = 18;
this.Controls.Add(this.lblCustomerPartNo);
this.Controls.Add(this.txtCustomerPartNo);

           //#####Cost###Decimal
this.lblCost.AutoSize = true;
this.lblCost.Location = new System.Drawing.Point(100,475);
this.lblCost.Name = "lblCost";
this.lblCost.Size = new System.Drawing.Size(41, 12);
this.lblCost.TabIndex = 19;
this.lblCost.Text = "";
//111======475
this.txtCost.Location = new System.Drawing.Point(173,471);
this.txtCost.Name ="txtCost";
this.txtCost.Size = new System.Drawing.Size(100, 21);
this.txtCost.TabIndex = 19;
this.Controls.Add(this.lblCost);
this.Controls.Add(this.txtCost);

           //#####SubtotalCostAmount###Decimal
this.lblSubtotalCostAmount.AutoSize = true;
this.lblSubtotalCostAmount.Location = new System.Drawing.Point(100,500);
this.lblSubtotalCostAmount.Name = "lblSubtotalCostAmount";
this.lblSubtotalCostAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalCostAmount.TabIndex = 20;
this.lblSubtotalCostAmount.Text = "";
//111======500
this.txtSubtotalCostAmount.Location = new System.Drawing.Point(173,496);
this.txtSubtotalCostAmount.Name ="txtSubtotalCostAmount";
this.txtSubtotalCostAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalCostAmount.TabIndex = 20;
this.Controls.Add(this.lblSubtotalCostAmount);
this.Controls.Add(this.txtSubtotalCostAmount);

           //#####TaxRate###Decimal
this.lblTaxRate.AutoSize = true;
this.lblTaxRate.Location = new System.Drawing.Point(100,525);
this.lblTaxRate.Name = "lblTaxRate";
this.lblTaxRate.Size = new System.Drawing.Size(41, 12);
this.lblTaxRate.TabIndex = 21;
this.lblTaxRate.Text = "";
//111======525
this.txtTaxRate.Location = new System.Drawing.Point(173,521);
this.txtTaxRate.Name ="txtTaxRate";
this.txtTaxRate.Size = new System.Drawing.Size(100, 21);
this.txtTaxRate.TabIndex = 21;
this.Controls.Add(this.lblTaxRate);
this.Controls.Add(this.txtTaxRate);

           //#####Gift###Boolean
this.lblGift.AutoSize = true;
this.lblGift.Location = new System.Drawing.Point(100,550);
this.lblGift.Name = "lblGift";
this.lblGift.Size = new System.Drawing.Size(41, 12);
this.lblGift.TabIndex = 22;
this.lblGift.Text = "";
this.chkGift.Location = new System.Drawing.Point(173,546);
this.chkGift.Name = "chkGift";
this.chkGift.Size = new System.Drawing.Size(100, 21);
this.chkGift.TabIndex = 22;
this.Controls.Add(this.lblGift);
this.Controls.Add(this.chkGift);

           //#####SubtotalUntaxedAmount###Decimal
this.lblSubtotalUntaxedAmount.AutoSize = true;
this.lblSubtotalUntaxedAmount.Location = new System.Drawing.Point(100,575);
this.lblSubtotalUntaxedAmount.Name = "lblSubtotalUntaxedAmount";
this.lblSubtotalUntaxedAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalUntaxedAmount.TabIndex = 23;
this.lblSubtotalUntaxedAmount.Text = "";
//111======575
this.txtSubtotalUntaxedAmount.Location = new System.Drawing.Point(173,571);
this.txtSubtotalUntaxedAmount.Name ="txtSubtotalUntaxedAmount";
this.txtSubtotalUntaxedAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalUntaxedAmount.TabIndex = 23;
this.Controls.Add(this.lblSubtotalUntaxedAmount);
this.Controls.Add(this.txtSubtotalUntaxedAmount);

           //#####SubtotalTaxAmount###Decimal
this.lblSubtotalTaxAmount.AutoSize = true;
this.lblSubtotalTaxAmount.Location = new System.Drawing.Point(100,600);
this.lblSubtotalTaxAmount.Name = "lblSubtotalTaxAmount";
this.lblSubtotalTaxAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalTaxAmount.TabIndex = 24;
this.lblSubtotalTaxAmount.Text = "";
//111======600
this.txtSubtotalTaxAmount.Location = new System.Drawing.Point(173,596);
this.txtSubtotalTaxAmount.Name ="txtSubtotalTaxAmount";
this.txtSubtotalTaxAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalTaxAmount.TabIndex = 24;
this.Controls.Add(this.lblSubtotalTaxAmount);
this.Controls.Add(this.txtSubtotalTaxAmount);

           //#####255CNName###String
this.lblCNName.AutoSize = true;
this.lblCNName.Location = new System.Drawing.Point(100,625);
this.lblCNName.Name = "lblCNName";
this.lblCNName.Size = new System.Drawing.Size(41, 12);
this.lblCNName.TabIndex = 25;
this.lblCNName.Text = "";
this.txtCNName.Location = new System.Drawing.Point(173,621);
this.txtCNName.Name = "txtCNName";
this.txtCNName.Size = new System.Drawing.Size(100, 21);
this.txtCNName.TabIndex = 25;
this.Controls.Add(this.lblCNName);
this.Controls.Add(this.txtCNName);

           //#####1000Specifications###String
this.lblSpecifications.AutoSize = true;
this.lblSpecifications.Location = new System.Drawing.Point(100,650);
this.lblSpecifications.Name = "lblSpecifications";
this.lblSpecifications.Size = new System.Drawing.Size(41, 12);
this.lblSpecifications.TabIndex = 26;
this.lblSpecifications.Text = "";
this.txtSpecifications.Location = new System.Drawing.Point(173,646);
this.txtSpecifications.Name = "txtSpecifications";
this.txtSpecifications.Size = new System.Drawing.Size(100, 21);
this.txtSpecifications.TabIndex = 26;
this.Controls.Add(this.lblSpecifications);
this.Controls.Add(this.txtSpecifications);

           //#####40ProductNo###String
this.lblProductNo.AutoSize = true;
this.lblProductNo.Location = new System.Drawing.Point(100,675);
this.lblProductNo.Name = "lblProductNo";
this.lblProductNo.Size = new System.Drawing.Size(41, 12);
this.lblProductNo.TabIndex = 27;
this.lblProductNo.Text = "";
this.txtProductNo.Location = new System.Drawing.Point(173,671);
this.txtProductNo.Name = "txtProductNo";
this.txtProductNo.Size = new System.Drawing.Size(100, 21);
this.txtProductNo.TabIndex = 27;
this.Controls.Add(this.lblProductNo);
this.Controls.Add(this.txtProductNo);

           //#####Unit_ID###Int64
this.lblUnit_ID.AutoSize = true;
this.lblUnit_ID.Location = new System.Drawing.Point(100,700);
this.lblUnit_ID.Name = "lblUnit_ID";
this.lblUnit_ID.Size = new System.Drawing.Size(41, 12);
this.lblUnit_ID.TabIndex = 28;
this.lblUnit_ID.Text = "";
this.txtUnit_ID.Location = new System.Drawing.Point(173,696);
this.txtUnit_ID.Name = "txtUnit_ID";
this.txtUnit_ID.Size = new System.Drawing.Size(100, 21);
this.txtUnit_ID.TabIndex = 28;
this.Controls.Add(this.lblUnit_ID);
this.Controls.Add(this.txtUnit_ID);

           //#####50Model###String
this.lblModel.AutoSize = true;
this.lblModel.Location = new System.Drawing.Point(100,725);
this.lblModel.Name = "lblModel";
this.lblModel.Size = new System.Drawing.Size(41, 12);
this.lblModel.TabIndex = 29;
this.lblModel.Text = "";
this.txtModel.Location = new System.Drawing.Point(173,721);
this.txtModel.Name = "txtModel";
this.txtModel.Size = new System.Drawing.Size(100, 21);
this.txtModel.TabIndex = 29;
this.Controls.Add(this.lblModel);
this.Controls.Add(this.txtModel);

           //#####Category_ID###Int64
this.lblCategory_ID.AutoSize = true;
this.lblCategory_ID.Location = new System.Drawing.Point(100,750);
this.lblCategory_ID.Name = "lblCategory_ID";
this.lblCategory_ID.Size = new System.Drawing.Size(41, 12);
this.lblCategory_ID.TabIndex = 30;
this.lblCategory_ID.Text = "";
this.txtCategory_ID.Location = new System.Drawing.Point(173,746);
this.txtCategory_ID.Name = "txtCategory_ID";
this.txtCategory_ID.Size = new System.Drawing.Size(100, 21);
this.txtCategory_ID.TabIndex = 30;
this.Controls.Add(this.lblCategory_ID);
this.Controls.Add(this.txtCategory_ID);

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
           // this.kryptonPanel1.TabIndex = 30;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblSKU );
this.Controls.Add(this.txtSKU );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.txtEmployee_ID );

                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.txtCustomerVendor_ID );

                this.Controls.Add(this.lblReturnNo );
this.Controls.Add(this.txtReturnNo );

                this.Controls.Add(this.lblPayStatus );
this.Controls.Add(this.txtPayStatus );

                this.Controls.Add(this.lblPaytype_ID );
this.Controls.Add(this.txtPaytype_ID );

                this.Controls.Add(this.lblReturnDate );
this.Controls.Add(this.dtpReturnDate );

                this.Controls.Add(this.lblSaleOut_NO );
this.Controls.Add(this.txtSaleOut_NO );

                this.Controls.Add(this.lblProjectGroup_ID );
this.Controls.Add(this.txtProjectGroup_ID );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.txtProdDetailID );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.txtLocation_ID );

                this.Controls.Add(this.lblRack_ID );
this.Controls.Add(this.txtRack_ID );

                this.Controls.Add(this.lblQuantity );
this.Controls.Add(this.txtQuantity );

                this.Controls.Add(this.lblTransactionPrice );
this.Controls.Add(this.txtTransactionPrice );

                this.Controls.Add(this.lblSubtotalTransAmount );
this.Controls.Add(this.txtSubtotalTransAmount );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                this.Controls.Add(this.lblCustomerPartNo );
this.Controls.Add(this.txtCustomerPartNo );

                this.Controls.Add(this.lblCost );
this.Controls.Add(this.txtCost );

                this.Controls.Add(this.lblSubtotalCostAmount );
this.Controls.Add(this.txtSubtotalCostAmount );

                this.Controls.Add(this.lblTaxRate );
this.Controls.Add(this.txtTaxRate );

                this.Controls.Add(this.lblGift );
this.Controls.Add(this.chkGift );

                this.Controls.Add(this.lblSubtotalUntaxedAmount );
this.Controls.Add(this.txtSubtotalUntaxedAmount );

                this.Controls.Add(this.lblSubtotalTaxAmount );
this.Controls.Add(this.txtSubtotalTaxAmount );

                this.Controls.Add(this.lblCNName );
this.Controls.Add(this.txtCNName );

                this.Controls.Add(this.lblSpecifications );
this.Controls.Add(this.txtSpecifications );

                this.Controls.Add(this.lblProductNo );
this.Controls.Add(this.txtProductNo );

                this.Controls.Add(this.lblUnit_ID );
this.Controls.Add(this.txtUnit_ID );

                this.Controls.Add(this.lblModel );
this.Controls.Add(this.txtModel );

                this.Controls.Add(this.lblCategory_ID );
this.Controls.Add(this.txtCategory_ID );

                            // 
            // "View_SaleOutReItemsEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "View_SaleOutReItemsEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblSKU;
private Krypton.Toolkit.KryptonTextBox txtSKU;

    
        
              private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonTextBox txtEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private Krypton.Toolkit.KryptonTextBox txtCustomerVendor_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblReturnNo;
private Krypton.Toolkit.KryptonTextBox txtReturnNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblPayStatus;
private Krypton.Toolkit.KryptonTextBox txtPayStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblPaytype_ID;
private Krypton.Toolkit.KryptonTextBox txtPaytype_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblReturnDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpReturnDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblSaleOut_NO;
private Krypton.Toolkit.KryptonTextBox txtSaleOut_NO;

    
        
              private Krypton.Toolkit.KryptonLabel lblProjectGroup_ID;
private Krypton.Toolkit.KryptonTextBox txtProjectGroup_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblProdDetailID;
private Krypton.Toolkit.KryptonTextBox txtProdDetailID;

    
        
              private Krypton.Toolkit.KryptonLabel lblproperty;
private Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private Krypton.Toolkit.KryptonLabel lblLocation_ID;
private Krypton.Toolkit.KryptonTextBox txtLocation_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblRack_ID;
private Krypton.Toolkit.KryptonTextBox txtRack_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblQuantity;
private Krypton.Toolkit.KryptonTextBox txtQuantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblTransactionPrice;
private Krypton.Toolkit.KryptonTextBox txtTransactionPrice;

    
        
              private Krypton.Toolkit.KryptonLabel lblSubtotalTransAmount;
private Krypton.Toolkit.KryptonTextBox txtSubtotalTransAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblSummary;
private Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerPartNo;
private Krypton.Toolkit.KryptonTextBox txtCustomerPartNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblCost;
private Krypton.Toolkit.KryptonTextBox txtCost;

    
        
              private Krypton.Toolkit.KryptonLabel lblSubtotalCostAmount;
private Krypton.Toolkit.KryptonTextBox txtSubtotalCostAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblTaxRate;
private Krypton.Toolkit.KryptonTextBox txtTaxRate;

    
        
              private Krypton.Toolkit.KryptonLabel lblGift;
private Krypton.Toolkit.KryptonCheckBox chkGift;

    
        
              private Krypton.Toolkit.KryptonLabel lblSubtotalUntaxedAmount;
private Krypton.Toolkit.KryptonTextBox txtSubtotalUntaxedAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblSubtotalTaxAmount;
private Krypton.Toolkit.KryptonTextBox txtSubtotalTaxAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblCNName;
private Krypton.Toolkit.KryptonTextBox txtCNName;

    
        
              private Krypton.Toolkit.KryptonLabel lblSpecifications;
private Krypton.Toolkit.KryptonTextBox txtSpecifications;

    
        
              private Krypton.Toolkit.KryptonLabel lblProductNo;
private Krypton.Toolkit.KryptonTextBox txtProductNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblUnit_ID;
private Krypton.Toolkit.KryptonTextBox txtUnit_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblModel;
private Krypton.Toolkit.KryptonTextBox txtModel;

    
        
              private Krypton.Toolkit.KryptonLabel lblCategory_ID;
private Krypton.Toolkit.KryptonTextBox txtCategory_ID;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

