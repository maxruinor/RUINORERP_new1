// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:13
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 销售订单明细
    /// </summary>
    partial class tb_SaleOrderDetailEdit
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
this.cmbSOrder_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblProdDetailID = new Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new Krypton.Toolkit.KryptonComboBox();

this.lblproperty = new Krypton.Toolkit.KryptonLabel();
this.txtproperty = new Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

this.lblLocation_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new Krypton.Toolkit.KryptonComboBox();

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

this.lblCustomizedCost = new Krypton.Toolkit.KryptonLabel();
this.txtCustomizedCost = new Krypton.Toolkit.KryptonTextBox();

this.lblCost = new Krypton.Toolkit.KryptonLabel();
this.txtCost = new Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalCostAmount = new Krypton.Toolkit.KryptonLabel();
this.txtSubtotalCostAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalDeliveredQty = new Krypton.Toolkit.KryptonLabel();
this.txtTotalDeliveredQty = new Krypton.Toolkit.KryptonTextBox();

this.lblUnitCommissionAmount = new Krypton.Toolkit.KryptonLabel();
this.txtUnitCommissionAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblCommissionAmount = new Krypton.Toolkit.KryptonLabel();
this.txtCommissionAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblTaxRate = new Krypton.Toolkit.KryptonLabel();
this.txtTaxRate = new Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalTaxAmount = new Krypton.Toolkit.KryptonLabel();
this.txtSubtotalTaxAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalUntaxedAmount = new Krypton.Toolkit.KryptonLabel();
this.txtSubtotalUntaxedAmount = new Krypton.Toolkit.KryptonTextBox();

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

this.lblSaleFlagCode = new Krypton.Toolkit.KryptonLabel();
this.txtSaleFlagCode = new Krypton.Toolkit.KryptonTextBox();

    
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
//属性测试25SOrder_ID
//属性测试25SOrder_ID
//属性测试25SOrder_ID
this.lblSOrder_ID.AutoSize = true;
this.lblSOrder_ID.Location = new System.Drawing.Point(100,25);
this.lblSOrder_ID.Name = "lblSOrder_ID";
this.lblSOrder_ID.Size = new System.Drawing.Size(41, 12);
this.lblSOrder_ID.TabIndex = 1;
this.lblSOrder_ID.Text = "";
//111======25
this.cmbSOrder_ID.Location = new System.Drawing.Point(173,21);
this.cmbSOrder_ID.Name ="cmbSOrder_ID";
this.cmbSOrder_ID.Size = new System.Drawing.Size(100, 21);
this.cmbSOrder_ID.TabIndex = 1;
this.Controls.Add(this.lblSOrder_ID);
this.Controls.Add(this.cmbSOrder_ID);

           //#####ProdDetailID###Int64
//属性测试50ProdDetailID
//属性测试50ProdDetailID
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,50);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 2;
this.lblProdDetailID.Text = "货品";
//111======50
this.cmbProdDetailID.Location = new System.Drawing.Point(173,46);
this.cmbProdDetailID.Name ="cmbProdDetailID";
this.cmbProdDetailID.Size = new System.Drawing.Size(100, 21);
this.cmbProdDetailID.TabIndex = 2;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.cmbProdDetailID);

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,75);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 3;
this.lblproperty.Text = "属性";
this.txtproperty.Location = new System.Drawing.Point(173,71);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 3;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

           //#####Location_ID###Int64
//属性测试100Location_ID
this.lblLocation_ID.AutoSize = true;
this.lblLocation_ID.Location = new System.Drawing.Point(100,100);
this.lblLocation_ID.Name = "lblLocation_ID";
this.lblLocation_ID.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID.TabIndex = 4;
this.lblLocation_ID.Text = "库位";
//111======100
this.cmbLocation_ID.Location = new System.Drawing.Point(173,96);
this.cmbLocation_ID.Name ="cmbLocation_ID";
this.cmbLocation_ID.Size = new System.Drawing.Size(100, 21);
this.cmbLocation_ID.TabIndex = 4;
this.Controls.Add(this.lblLocation_ID);
this.Controls.Add(this.cmbLocation_ID);

           //#####UnitPrice###Decimal
this.lblUnitPrice.AutoSize = true;
this.lblUnitPrice.Location = new System.Drawing.Point(100,125);
this.lblUnitPrice.Name = "lblUnitPrice";
this.lblUnitPrice.Size = new System.Drawing.Size(41, 12);
this.lblUnitPrice.TabIndex = 5;
this.lblUnitPrice.Text = "单价";
//111======125
this.txtUnitPrice.Location = new System.Drawing.Point(173,121);
this.txtUnitPrice.Name ="txtUnitPrice";
this.txtUnitPrice.Size = new System.Drawing.Size(100, 21);
this.txtUnitPrice.TabIndex = 5;
this.Controls.Add(this.lblUnitPrice);
this.Controls.Add(this.txtUnitPrice);

           //#####Quantity###Int32
//属性测试150Quantity
//属性测试150Quantity
//属性测试150Quantity
this.lblQuantity.AutoSize = true;
this.lblQuantity.Location = new System.Drawing.Point(100,150);
this.lblQuantity.Name = "lblQuantity";
this.lblQuantity.Size = new System.Drawing.Size(41, 12);
this.lblQuantity.TabIndex = 6;
this.lblQuantity.Text = "数量";
this.txtQuantity.Location = new System.Drawing.Point(173,146);
this.txtQuantity.Name = "txtQuantity";
this.txtQuantity.Size = new System.Drawing.Size(100, 21);
this.txtQuantity.TabIndex = 6;
this.Controls.Add(this.lblQuantity);
this.Controls.Add(this.txtQuantity);

           //#####Discount###Decimal
this.lblDiscount.AutoSize = true;
this.lblDiscount.Location = new System.Drawing.Point(100,175);
this.lblDiscount.Name = "lblDiscount";
this.lblDiscount.Size = new System.Drawing.Size(41, 12);
this.lblDiscount.TabIndex = 7;
this.lblDiscount.Text = "折扣";
//111======175
this.txtDiscount.Location = new System.Drawing.Point(173,171);
this.txtDiscount.Name ="txtDiscount";
this.txtDiscount.Size = new System.Drawing.Size(100, 21);
this.txtDiscount.TabIndex = 7;
this.Controls.Add(this.lblDiscount);
this.Controls.Add(this.txtDiscount);

           //#####TransactionPrice###Decimal
this.lblTransactionPrice.AutoSize = true;
this.lblTransactionPrice.Location = new System.Drawing.Point(100,200);
this.lblTransactionPrice.Name = "lblTransactionPrice";
this.lblTransactionPrice.Size = new System.Drawing.Size(41, 12);
this.lblTransactionPrice.TabIndex = 8;
this.lblTransactionPrice.Text = "成交价";
//111======200
this.txtTransactionPrice.Location = new System.Drawing.Point(173,196);
this.txtTransactionPrice.Name ="txtTransactionPrice";
this.txtTransactionPrice.Size = new System.Drawing.Size(100, 21);
this.txtTransactionPrice.TabIndex = 8;
this.Controls.Add(this.lblTransactionPrice);
this.Controls.Add(this.txtTransactionPrice);

           //#####SubtotalTransAmount###Decimal
this.lblSubtotalTransAmount.AutoSize = true;
this.lblSubtotalTransAmount.Location = new System.Drawing.Point(100,225);
this.lblSubtotalTransAmount.Name = "lblSubtotalTransAmount";
this.lblSubtotalTransAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalTransAmount.TabIndex = 9;
this.lblSubtotalTransAmount.Text = "成交小计";
//111======225
this.txtSubtotalTransAmount.Location = new System.Drawing.Point(173,221);
this.txtSubtotalTransAmount.Name ="txtSubtotalTransAmount";
this.txtSubtotalTransAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalTransAmount.TabIndex = 9;
this.Controls.Add(this.lblSubtotalTransAmount);
this.Controls.Add(this.txtSubtotalTransAmount);

           //#####CustomizedCost###Decimal
this.lblCustomizedCost.AutoSize = true;
this.lblCustomizedCost.Location = new System.Drawing.Point(100,250);
this.lblCustomizedCost.Name = "lblCustomizedCost";
this.lblCustomizedCost.Size = new System.Drawing.Size(41, 12);
this.lblCustomizedCost.TabIndex = 10;
this.lblCustomizedCost.Text = "定制成本";
//111======250
this.txtCustomizedCost.Location = new System.Drawing.Point(173,246);
this.txtCustomizedCost.Name ="txtCustomizedCost";
this.txtCustomizedCost.Size = new System.Drawing.Size(100, 21);
this.txtCustomizedCost.TabIndex = 10;
this.Controls.Add(this.lblCustomizedCost);
this.Controls.Add(this.txtCustomizedCost);

           //#####Cost###Decimal
this.lblCost.AutoSize = true;
this.lblCost.Location = new System.Drawing.Point(100,275);
this.lblCost.Name = "lblCost";
this.lblCost.Size = new System.Drawing.Size(41, 12);
this.lblCost.TabIndex = 11;
this.lblCost.Text = "成本";
//111======275
this.txtCost.Location = new System.Drawing.Point(173,271);
this.txtCost.Name ="txtCost";
this.txtCost.Size = new System.Drawing.Size(100, 21);
this.txtCost.TabIndex = 11;
this.Controls.Add(this.lblCost);
this.Controls.Add(this.txtCost);

           //#####SubtotalCostAmount###Decimal
this.lblSubtotalCostAmount.AutoSize = true;
this.lblSubtotalCostAmount.Location = new System.Drawing.Point(100,300);
this.lblSubtotalCostAmount.Name = "lblSubtotalCostAmount";
this.lblSubtotalCostAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalCostAmount.TabIndex = 12;
this.lblSubtotalCostAmount.Text = "成本小计";
//111======300
this.txtSubtotalCostAmount.Location = new System.Drawing.Point(173,296);
this.txtSubtotalCostAmount.Name ="txtSubtotalCostAmount";
this.txtSubtotalCostAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalCostAmount.TabIndex = 12;
this.Controls.Add(this.lblSubtotalCostAmount);
this.Controls.Add(this.txtSubtotalCostAmount);

           //#####TotalDeliveredQty###Int32
//属性测试325TotalDeliveredQty
//属性测试325TotalDeliveredQty
//属性测试325TotalDeliveredQty
this.lblTotalDeliveredQty.AutoSize = true;
this.lblTotalDeliveredQty.Location = new System.Drawing.Point(100,325);
this.lblTotalDeliveredQty.Name = "lblTotalDeliveredQty";
this.lblTotalDeliveredQty.Size = new System.Drawing.Size(41, 12);
this.lblTotalDeliveredQty.TabIndex = 13;
this.lblTotalDeliveredQty.Text = "订单出库数";
this.txtTotalDeliveredQty.Location = new System.Drawing.Point(173,321);
this.txtTotalDeliveredQty.Name = "txtTotalDeliveredQty";
this.txtTotalDeliveredQty.Size = new System.Drawing.Size(100, 21);
this.txtTotalDeliveredQty.TabIndex = 13;
this.Controls.Add(this.lblTotalDeliveredQty);
this.Controls.Add(this.txtTotalDeliveredQty);

           //#####UnitCommissionAmount###Decimal
this.lblUnitCommissionAmount.AutoSize = true;
this.lblUnitCommissionAmount.Location = new System.Drawing.Point(100,350);
this.lblUnitCommissionAmount.Name = "lblUnitCommissionAmount";
this.lblUnitCommissionAmount.Size = new System.Drawing.Size(41, 12);
this.lblUnitCommissionAmount.TabIndex = 14;
this.lblUnitCommissionAmount.Text = "单品佣金";
//111======350
this.txtUnitCommissionAmount.Location = new System.Drawing.Point(173,346);
this.txtUnitCommissionAmount.Name ="txtUnitCommissionAmount";
this.txtUnitCommissionAmount.Size = new System.Drawing.Size(100, 21);
this.txtUnitCommissionAmount.TabIndex = 14;
this.Controls.Add(this.lblUnitCommissionAmount);
this.Controls.Add(this.txtUnitCommissionAmount);

           //#####CommissionAmount###Decimal
this.lblCommissionAmount.AutoSize = true;
this.lblCommissionAmount.Location = new System.Drawing.Point(100,375);
this.lblCommissionAmount.Name = "lblCommissionAmount";
this.lblCommissionAmount.Size = new System.Drawing.Size(41, 12);
this.lblCommissionAmount.TabIndex = 15;
this.lblCommissionAmount.Text = "佣金小计";
//111======375
this.txtCommissionAmount.Location = new System.Drawing.Point(173,371);
this.txtCommissionAmount.Name ="txtCommissionAmount";
this.txtCommissionAmount.Size = new System.Drawing.Size(100, 21);
this.txtCommissionAmount.TabIndex = 15;
this.Controls.Add(this.lblCommissionAmount);
this.Controls.Add(this.txtCommissionAmount);

           //#####TaxRate###Decimal
this.lblTaxRate.AutoSize = true;
this.lblTaxRate.Location = new System.Drawing.Point(100,400);
this.lblTaxRate.Name = "lblTaxRate";
this.lblTaxRate.Size = new System.Drawing.Size(41, 12);
this.lblTaxRate.TabIndex = 16;
this.lblTaxRate.Text = "税率";
//111======400
this.txtTaxRate.Location = new System.Drawing.Point(173,396);
this.txtTaxRate.Name ="txtTaxRate";
this.txtTaxRate.Size = new System.Drawing.Size(100, 21);
this.txtTaxRate.TabIndex = 16;
this.Controls.Add(this.lblTaxRate);
this.Controls.Add(this.txtTaxRate);

           //#####SubtotalTaxAmount###Decimal
this.lblSubtotalTaxAmount.AutoSize = true;
this.lblSubtotalTaxAmount.Location = new System.Drawing.Point(100,425);
this.lblSubtotalTaxAmount.Name = "lblSubtotalTaxAmount";
this.lblSubtotalTaxAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalTaxAmount.TabIndex = 17;
this.lblSubtotalTaxAmount.Text = "税额";
//111======425
this.txtSubtotalTaxAmount.Location = new System.Drawing.Point(173,421);
this.txtSubtotalTaxAmount.Name ="txtSubtotalTaxAmount";
this.txtSubtotalTaxAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalTaxAmount.TabIndex = 17;
this.Controls.Add(this.lblSubtotalTaxAmount);
this.Controls.Add(this.txtSubtotalTaxAmount);

           //#####SubtotalUntaxedAmount###Decimal
this.lblSubtotalUntaxedAmount.AutoSize = true;
this.lblSubtotalUntaxedAmount.Location = new System.Drawing.Point(100,450);
this.lblSubtotalUntaxedAmount.Name = "lblSubtotalUntaxedAmount";
this.lblSubtotalUntaxedAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalUntaxedAmount.TabIndex = 18;
this.lblSubtotalUntaxedAmount.Text = "未税本位币";
//111======450
this.txtSubtotalUntaxedAmount.Location = new System.Drawing.Point(173,446);
this.txtSubtotalUntaxedAmount.Name ="txtSubtotalUntaxedAmount";
this.txtSubtotalUntaxedAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalUntaxedAmount.TabIndex = 18;
this.Controls.Add(this.lblSubtotalUntaxedAmount);
this.Controls.Add(this.txtSubtotalUntaxedAmount);

           //#####1000Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,475);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 19;
this.lblSummary.Text = "摘要";
this.txtSummary.Location = new System.Drawing.Point(173,471);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 19;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####100CustomerPartNo###String
this.lblCustomerPartNo.AutoSize = true;
this.lblCustomerPartNo.Location = new System.Drawing.Point(100,500);
this.lblCustomerPartNo.Name = "lblCustomerPartNo";
this.lblCustomerPartNo.Size = new System.Drawing.Size(41, 12);
this.lblCustomerPartNo.TabIndex = 20;
this.lblCustomerPartNo.Text = "客户型号";
this.txtCustomerPartNo.Location = new System.Drawing.Point(173,496);
this.txtCustomerPartNo.Name = "txtCustomerPartNo";
this.txtCustomerPartNo.Size = new System.Drawing.Size(100, 21);
this.txtCustomerPartNo.TabIndex = 20;
this.Controls.Add(this.lblCustomerPartNo);
this.Controls.Add(this.txtCustomerPartNo);

           //#####Gift###Boolean
this.lblGift.AutoSize = true;
this.lblGift.Location = new System.Drawing.Point(100,525);
this.lblGift.Name = "lblGift";
this.lblGift.Size = new System.Drawing.Size(41, 12);
this.lblGift.TabIndex = 21;
this.lblGift.Text = "赠品";
this.chkGift.Location = new System.Drawing.Point(173,521);
this.chkGift.Name = "chkGift";
this.chkGift.Size = new System.Drawing.Size(100, 21);
this.chkGift.TabIndex = 21;
this.Controls.Add(this.lblGift);
this.Controls.Add(this.chkGift);

           //#####TotalReturnedQty###Int32
//属性测试550TotalReturnedQty
//属性测试550TotalReturnedQty
//属性测试550TotalReturnedQty
this.lblTotalReturnedQty.AutoSize = true;
this.lblTotalReturnedQty.Location = new System.Drawing.Point(100,550);
this.lblTotalReturnedQty.Name = "lblTotalReturnedQty";
this.lblTotalReturnedQty.Size = new System.Drawing.Size(41, 12);
this.lblTotalReturnedQty.TabIndex = 22;
this.lblTotalReturnedQty.Text = "订单退回数";
this.txtTotalReturnedQty.Location = new System.Drawing.Point(173,546);
this.txtTotalReturnedQty.Name = "txtTotalReturnedQty";
this.txtTotalReturnedQty.Size = new System.Drawing.Size(100, 21);
this.txtTotalReturnedQty.TabIndex = 22;
this.Controls.Add(this.lblTotalReturnedQty);
this.Controls.Add(this.txtTotalReturnedQty);

           //#####200SaleFlagCode###String
this.lblSaleFlagCode.AutoSize = true;
this.lblSaleFlagCode.Location = new System.Drawing.Point(100,575);
this.lblSaleFlagCode.Name = "lblSaleFlagCode";
this.lblSaleFlagCode.Size = new System.Drawing.Size(41, 12);
this.lblSaleFlagCode.TabIndex = 23;
this.lblSaleFlagCode.Text = "标识代码";
this.txtSaleFlagCode.Location = new System.Drawing.Point(173,571);
this.txtSaleFlagCode.Name = "txtSaleFlagCode";
this.txtSaleFlagCode.Size = new System.Drawing.Size(100, 21);
this.txtSaleFlagCode.TabIndex = 23;
this.Controls.Add(this.lblSaleFlagCode);
this.Controls.Add(this.txtSaleFlagCode);

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
           // this.kryptonPanel1.TabIndex = 23;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblSOrder_ID );
this.Controls.Add(this.cmbSOrder_ID );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.cmbLocation_ID );

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

                this.Controls.Add(this.lblCustomizedCost );
this.Controls.Add(this.txtCustomizedCost );

                this.Controls.Add(this.lblCost );
this.Controls.Add(this.txtCost );

                this.Controls.Add(this.lblSubtotalCostAmount );
this.Controls.Add(this.txtSubtotalCostAmount );

                this.Controls.Add(this.lblTotalDeliveredQty );
this.Controls.Add(this.txtTotalDeliveredQty );

                this.Controls.Add(this.lblUnitCommissionAmount );
this.Controls.Add(this.txtUnitCommissionAmount );

                this.Controls.Add(this.lblCommissionAmount );
this.Controls.Add(this.txtCommissionAmount );

                this.Controls.Add(this.lblTaxRate );
this.Controls.Add(this.txtTaxRate );

                this.Controls.Add(this.lblSubtotalTaxAmount );
this.Controls.Add(this.txtSubtotalTaxAmount );

                this.Controls.Add(this.lblSubtotalUntaxedAmount );
this.Controls.Add(this.txtSubtotalUntaxedAmount );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                this.Controls.Add(this.lblCustomerPartNo );
this.Controls.Add(this.txtCustomerPartNo );

                this.Controls.Add(this.lblGift );
this.Controls.Add(this.chkGift );

                this.Controls.Add(this.lblTotalReturnedQty );
this.Controls.Add(this.txtTotalReturnedQty );

                this.Controls.Add(this.lblSaleFlagCode );
this.Controls.Add(this.txtSaleFlagCode );

                            // 
            // "tb_SaleOrderDetailEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_SaleOrderDetailEdit";
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
private Krypton.Toolkit.KryptonComboBox cmbSOrder_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblProdDetailID;
private Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private Krypton.Toolkit.KryptonLabel lblproperty;
private Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private Krypton.Toolkit.KryptonLabel lblLocation_ID;
private Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
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

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomizedCost;
private Krypton.Toolkit.KryptonTextBox txtCustomizedCost;

    
        
              private Krypton.Toolkit.KryptonLabel lblCost;
private Krypton.Toolkit.KryptonTextBox txtCost;

    
        
              private Krypton.Toolkit.KryptonLabel lblSubtotalCostAmount;
private Krypton.Toolkit.KryptonTextBox txtSubtotalCostAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalDeliveredQty;
private Krypton.Toolkit.KryptonTextBox txtTotalDeliveredQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblUnitCommissionAmount;
private Krypton.Toolkit.KryptonTextBox txtUnitCommissionAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblCommissionAmount;
private Krypton.Toolkit.KryptonTextBox txtCommissionAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblTaxRate;
private Krypton.Toolkit.KryptonTextBox txtTaxRate;

    
        
              private Krypton.Toolkit.KryptonLabel lblSubtotalTaxAmount;
private Krypton.Toolkit.KryptonTextBox txtSubtotalTaxAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblSubtotalUntaxedAmount;
private Krypton.Toolkit.KryptonTextBox txtSubtotalUntaxedAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblSummary;
private Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerPartNo;
private Krypton.Toolkit.KryptonTextBox txtCustomerPartNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblGift;
private Krypton.Toolkit.KryptonCheckBox chkGift;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalReturnedQty;
private Krypton.Toolkit.KryptonTextBox txtTotalReturnedQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblSaleFlagCode;
private Krypton.Toolkit.KryptonTextBox txtSaleFlagCode;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

