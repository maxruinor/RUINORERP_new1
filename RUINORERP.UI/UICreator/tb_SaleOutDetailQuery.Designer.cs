
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
    /// 销售出库明细
    /// </summary>
    partial class tb_SaleOutDetailQuery
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
     
     this.lblProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblproperty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtproperty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

this.lblLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblRack_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbRack_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblSaleOut_MainID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbSaleOut_MainID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();


this.lblUnitPrice = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUnitPrice = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblDiscount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDiscount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTransactionPrice = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTransactionPrice = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalTransAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalTransAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSummary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSummary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

this.lblCustomerPartNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCustomerPartNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCustomizedCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCustomizedCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalCostAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalCostAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTaxRate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTaxRate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblGift = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkGift = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkGift.Values.Text ="";

this.lblIncludingTax = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIncludingTax = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIncludingTax.Values.Text ="";

this.lblSubtotalUntaxedAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalUntaxedAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblUnitCommissionAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUnitCommissionAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCommissionAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCommissionAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSaleFlagCode = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSaleFlagCode = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblAllocatedFreightIncome = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtAllocatedFreightIncome = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblAllocatedFreightCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtAllocatedFreightCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


    //for end
    this.SuspendLayout();
    
         //for start
                 //#####ProdDetailID###Int64
//属性测试25ProdDetailID
//属性测试25ProdDetailID
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,25);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 1;
this.lblProdDetailID.Text = "货品详情";
//111======25
this.cmbProdDetailID.Location = new System.Drawing.Point(173,21);
this.cmbProdDetailID.Name ="cmbProdDetailID";
this.cmbProdDetailID.Size = new System.Drawing.Size(100, 21);
this.cmbProdDetailID.TabIndex = 1;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.cmbProdDetailID);

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,50);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 2;
this.lblproperty.Text = "属性";
this.txtproperty.Location = new System.Drawing.Point(173,46);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 2;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

           //#####Location_ID###Int64
//属性测试75Location_ID
//属性测试75Location_ID
//属性测试75Location_ID
//属性测试75Location_ID
this.lblLocation_ID.AutoSize = true;
this.lblLocation_ID.Location = new System.Drawing.Point(100,75);
this.lblLocation_ID.Name = "lblLocation_ID";
this.lblLocation_ID.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID.TabIndex = 3;
this.lblLocation_ID.Text = "库位";
//111======75
this.cmbLocation_ID.Location = new System.Drawing.Point(173,71);
this.cmbLocation_ID.Name ="cmbLocation_ID";
this.cmbLocation_ID.Size = new System.Drawing.Size(100, 21);
this.cmbLocation_ID.TabIndex = 3;
this.Controls.Add(this.lblLocation_ID);
this.Controls.Add(this.cmbLocation_ID);

           //#####Rack_ID###Int64
//属性测试100Rack_ID
//属性测试100Rack_ID
//属性测试100Rack_ID
this.lblRack_ID.AutoSize = true;
this.lblRack_ID.Location = new System.Drawing.Point(100,100);
this.lblRack_ID.Name = "lblRack_ID";
this.lblRack_ID.Size = new System.Drawing.Size(41, 12);
this.lblRack_ID.TabIndex = 4;
this.lblRack_ID.Text = "货架";
//111======100
this.cmbRack_ID.Location = new System.Drawing.Point(173,96);
this.cmbRack_ID.Name ="cmbRack_ID";
this.cmbRack_ID.Size = new System.Drawing.Size(100, 21);
this.cmbRack_ID.TabIndex = 4;
this.Controls.Add(this.lblRack_ID);
this.Controls.Add(this.cmbRack_ID);

           //#####SaleOut_MainID###Int64
//属性测试125SaleOut_MainID
this.lblSaleOut_MainID.AutoSize = true;
this.lblSaleOut_MainID.Location = new System.Drawing.Point(100,125);
this.lblSaleOut_MainID.Name = "lblSaleOut_MainID";
this.lblSaleOut_MainID.Size = new System.Drawing.Size(41, 12);
this.lblSaleOut_MainID.TabIndex = 5;
this.lblSaleOut_MainID.Text = "";
//111======125
this.cmbSaleOut_MainID.Location = new System.Drawing.Point(173,121);
this.cmbSaleOut_MainID.Name ="cmbSaleOut_MainID";
this.cmbSaleOut_MainID.Size = new System.Drawing.Size(100, 21);
this.cmbSaleOut_MainID.TabIndex = 5;
this.Controls.Add(this.lblSaleOut_MainID);
this.Controls.Add(this.cmbSaleOut_MainID);

           //#####Quantity###Int32
//属性测试150Quantity
//属性测试150Quantity
//属性测试150Quantity
//属性测试150Quantity

           //#####UnitPrice###Decimal
this.lblUnitPrice.AutoSize = true;
this.lblUnitPrice.Location = new System.Drawing.Point(100,175);
this.lblUnitPrice.Name = "lblUnitPrice";
this.lblUnitPrice.Size = new System.Drawing.Size(41, 12);
this.lblUnitPrice.TabIndex = 7;
this.lblUnitPrice.Text = "单价";
//111======175
this.txtUnitPrice.Location = new System.Drawing.Point(173,171);
this.txtUnitPrice.Name ="txtUnitPrice";
this.txtUnitPrice.Size = new System.Drawing.Size(100, 21);
this.txtUnitPrice.TabIndex = 7;
this.Controls.Add(this.lblUnitPrice);
this.Controls.Add(this.txtUnitPrice);

           //#####Discount###Decimal
this.lblDiscount.AutoSize = true;
this.lblDiscount.Location = new System.Drawing.Point(100,200);
this.lblDiscount.Name = "lblDiscount";
this.lblDiscount.Size = new System.Drawing.Size(41, 12);
this.lblDiscount.TabIndex = 8;
this.lblDiscount.Text = "折扣";
//111======200
this.txtDiscount.Location = new System.Drawing.Point(173,196);
this.txtDiscount.Name ="txtDiscount";
this.txtDiscount.Size = new System.Drawing.Size(100, 21);
this.txtDiscount.TabIndex = 8;
this.Controls.Add(this.lblDiscount);
this.Controls.Add(this.txtDiscount);

           //#####TransactionPrice###Decimal
this.lblTransactionPrice.AutoSize = true;
this.lblTransactionPrice.Location = new System.Drawing.Point(100,225);
this.lblTransactionPrice.Name = "lblTransactionPrice";
this.lblTransactionPrice.Size = new System.Drawing.Size(41, 12);
this.lblTransactionPrice.TabIndex = 9;
this.lblTransactionPrice.Text = "成交单价";
//111======225
this.txtTransactionPrice.Location = new System.Drawing.Point(173,221);
this.txtTransactionPrice.Name ="txtTransactionPrice";
this.txtTransactionPrice.Size = new System.Drawing.Size(100, 21);
this.txtTransactionPrice.TabIndex = 9;
this.Controls.Add(this.lblTransactionPrice);
this.Controls.Add(this.txtTransactionPrice);

           //#####SubtotalTransAmount###Decimal
this.lblSubtotalTransAmount.AutoSize = true;
this.lblSubtotalTransAmount.Location = new System.Drawing.Point(100,250);
this.lblSubtotalTransAmount.Name = "lblSubtotalTransAmount";
this.lblSubtotalTransAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalTransAmount.TabIndex = 10;
this.lblSubtotalTransAmount.Text = "成交小计";
//111======250
this.txtSubtotalTransAmount.Location = new System.Drawing.Point(173,246);
this.txtSubtotalTransAmount.Name ="txtSubtotalTransAmount";
this.txtSubtotalTransAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalTransAmount.TabIndex = 10;
this.Controls.Add(this.lblSubtotalTransAmount);
this.Controls.Add(this.txtSubtotalTransAmount);

           //#####1000Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,275);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 11;
this.lblSummary.Text = "摘要";
this.txtSummary.Location = new System.Drawing.Point(173,271);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 11;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####50CustomerPartNo###String
this.lblCustomerPartNo.AutoSize = true;
this.lblCustomerPartNo.Location = new System.Drawing.Point(100,300);
this.lblCustomerPartNo.Name = "lblCustomerPartNo";
this.lblCustomerPartNo.Size = new System.Drawing.Size(41, 12);
this.lblCustomerPartNo.TabIndex = 12;
this.lblCustomerPartNo.Text = "客户型号";
this.txtCustomerPartNo.Location = new System.Drawing.Point(173,296);
this.txtCustomerPartNo.Name = "txtCustomerPartNo";
this.txtCustomerPartNo.Size = new System.Drawing.Size(100, 21);
this.txtCustomerPartNo.TabIndex = 12;
this.Controls.Add(this.lblCustomerPartNo);
this.Controls.Add(this.txtCustomerPartNo);

           //#####CustomizedCost###Decimal
this.lblCustomizedCost.AutoSize = true;
this.lblCustomizedCost.Location = new System.Drawing.Point(100,325);
this.lblCustomizedCost.Name = "lblCustomizedCost";
this.lblCustomizedCost.Size = new System.Drawing.Size(41, 12);
this.lblCustomizedCost.TabIndex = 13;
this.lblCustomizedCost.Text = "定制成本";
//111======325
this.txtCustomizedCost.Location = new System.Drawing.Point(173,321);
this.txtCustomizedCost.Name ="txtCustomizedCost";
this.txtCustomizedCost.Size = new System.Drawing.Size(100, 21);
this.txtCustomizedCost.TabIndex = 13;
this.Controls.Add(this.lblCustomizedCost);
this.Controls.Add(this.txtCustomizedCost);

           //#####Cost###Decimal
this.lblCost.AutoSize = true;
this.lblCost.Location = new System.Drawing.Point(100,350);
this.lblCost.Name = "lblCost";
this.lblCost.Size = new System.Drawing.Size(41, 12);
this.lblCost.TabIndex = 14;
this.lblCost.Text = "成本";
//111======350
this.txtCost.Location = new System.Drawing.Point(173,346);
this.txtCost.Name ="txtCost";
this.txtCost.Size = new System.Drawing.Size(100, 21);
this.txtCost.TabIndex = 14;
this.Controls.Add(this.lblCost);
this.Controls.Add(this.txtCost);

           //#####SubtotalCostAmount###Decimal
this.lblSubtotalCostAmount.AutoSize = true;
this.lblSubtotalCostAmount.Location = new System.Drawing.Point(100,375);
this.lblSubtotalCostAmount.Name = "lblSubtotalCostAmount";
this.lblSubtotalCostAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalCostAmount.TabIndex = 15;
this.lblSubtotalCostAmount.Text = "成本小计";
//111======375
this.txtSubtotalCostAmount.Location = new System.Drawing.Point(173,371);
this.txtSubtotalCostAmount.Name ="txtSubtotalCostAmount";
this.txtSubtotalCostAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalCostAmount.TabIndex = 15;
this.Controls.Add(this.lblSubtotalCostAmount);
this.Controls.Add(this.txtSubtotalCostAmount);

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

           //#####TotalReturnedQty###Int32
//属性测试425TotalReturnedQty
//属性测试425TotalReturnedQty
//属性测试425TotalReturnedQty
//属性测试425TotalReturnedQty

           //#####Gift###Boolean
this.lblGift.AutoSize = true;
this.lblGift.Location = new System.Drawing.Point(100,450);
this.lblGift.Name = "lblGift";
this.lblGift.Size = new System.Drawing.Size(41, 12);
this.lblGift.TabIndex = 18;
this.lblGift.Text = "赠品";
this.chkGift.Location = new System.Drawing.Point(173,446);
this.chkGift.Name = "chkGift";
this.chkGift.Size = new System.Drawing.Size(100, 21);
this.chkGift.TabIndex = 18;
this.Controls.Add(this.lblGift);
this.Controls.Add(this.chkGift);

           //#####IncludingTax###Boolean
this.lblIncludingTax.AutoSize = true;
this.lblIncludingTax.Location = new System.Drawing.Point(100,475);
this.lblIncludingTax.Name = "lblIncludingTax";
this.lblIncludingTax.Size = new System.Drawing.Size(41, 12);
this.lblIncludingTax.TabIndex = 19;
this.lblIncludingTax.Text = "含税";
this.chkIncludingTax.Location = new System.Drawing.Point(173,471);
this.chkIncludingTax.Name = "chkIncludingTax";
this.chkIncludingTax.Size = new System.Drawing.Size(100, 21);
this.chkIncludingTax.TabIndex = 19;
this.Controls.Add(this.lblIncludingTax);
this.Controls.Add(this.chkIncludingTax);

           //#####SubtotalUntaxedAmount###Decimal
this.lblSubtotalUntaxedAmount.AutoSize = true;
this.lblSubtotalUntaxedAmount.Location = new System.Drawing.Point(100,500);
this.lblSubtotalUntaxedAmount.Name = "lblSubtotalUntaxedAmount";
this.lblSubtotalUntaxedAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalUntaxedAmount.TabIndex = 20;
this.lblSubtotalUntaxedAmount.Text = "未税本位币";
//111======500
this.txtSubtotalUntaxedAmount.Location = new System.Drawing.Point(173,496);
this.txtSubtotalUntaxedAmount.Name ="txtSubtotalUntaxedAmount";
this.txtSubtotalUntaxedAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalUntaxedAmount.TabIndex = 20;
this.Controls.Add(this.lblSubtotalUntaxedAmount);
this.Controls.Add(this.txtSubtotalUntaxedAmount);

           //#####UnitCommissionAmount###Decimal
this.lblUnitCommissionAmount.AutoSize = true;
this.lblUnitCommissionAmount.Location = new System.Drawing.Point(100,525);
this.lblUnitCommissionAmount.Name = "lblUnitCommissionAmount";
this.lblUnitCommissionAmount.Size = new System.Drawing.Size(41, 12);
this.lblUnitCommissionAmount.TabIndex = 21;
this.lblUnitCommissionAmount.Text = "单品佣金";
//111======525
this.txtUnitCommissionAmount.Location = new System.Drawing.Point(173,521);
this.txtUnitCommissionAmount.Name ="txtUnitCommissionAmount";
this.txtUnitCommissionAmount.Size = new System.Drawing.Size(100, 21);
this.txtUnitCommissionAmount.TabIndex = 21;
this.Controls.Add(this.lblUnitCommissionAmount);
this.Controls.Add(this.txtUnitCommissionAmount);

           //#####CommissionAmount###Decimal
this.lblCommissionAmount.AutoSize = true;
this.lblCommissionAmount.Location = new System.Drawing.Point(100,550);
this.lblCommissionAmount.Name = "lblCommissionAmount";
this.lblCommissionAmount.Size = new System.Drawing.Size(41, 12);
this.lblCommissionAmount.TabIndex = 22;
this.lblCommissionAmount.Text = "佣金小计";
//111======550
this.txtCommissionAmount.Location = new System.Drawing.Point(173,546);
this.txtCommissionAmount.Name ="txtCommissionAmount";
this.txtCommissionAmount.Size = new System.Drawing.Size(100, 21);
this.txtCommissionAmount.TabIndex = 22;
this.Controls.Add(this.lblCommissionAmount);
this.Controls.Add(this.txtCommissionAmount);

           //#####SubtotalTaxAmount###Decimal
this.lblSubtotalTaxAmount.AutoSize = true;
this.lblSubtotalTaxAmount.Location = new System.Drawing.Point(100,575);
this.lblSubtotalTaxAmount.Name = "lblSubtotalTaxAmount";
this.lblSubtotalTaxAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalTaxAmount.TabIndex = 23;
this.lblSubtotalTaxAmount.Text = "税额";
//111======575
this.txtSubtotalTaxAmount.Location = new System.Drawing.Point(173,571);
this.txtSubtotalTaxAmount.Name ="txtSubtotalTaxAmount";
this.txtSubtotalTaxAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalTaxAmount.TabIndex = 23;
this.Controls.Add(this.lblSubtotalTaxAmount);
this.Controls.Add(this.txtSubtotalTaxAmount);

           //#####200SaleFlagCode###String
this.lblSaleFlagCode.AutoSize = true;
this.lblSaleFlagCode.Location = new System.Drawing.Point(100,600);
this.lblSaleFlagCode.Name = "lblSaleFlagCode";
this.lblSaleFlagCode.Size = new System.Drawing.Size(41, 12);
this.lblSaleFlagCode.TabIndex = 24;
this.lblSaleFlagCode.Text = "标识代码";
this.txtSaleFlagCode.Location = new System.Drawing.Point(173,596);
this.txtSaleFlagCode.Name = "txtSaleFlagCode";
this.txtSaleFlagCode.Size = new System.Drawing.Size(100, 21);
this.txtSaleFlagCode.TabIndex = 24;
this.Controls.Add(this.lblSaleFlagCode);
this.Controls.Add(this.txtSaleFlagCode);

           //#####SaleOrderDetail_ID###Int64
//属性测试625SaleOrderDetail_ID
//属性测试625SaleOrderDetail_ID
//属性测试625SaleOrderDetail_ID
//属性测试625SaleOrderDetail_ID

           //#####AllocatedFreightIncome###Decimal
this.lblAllocatedFreightIncome.AutoSize = true;
this.lblAllocatedFreightIncome.Location = new System.Drawing.Point(100,650);
this.lblAllocatedFreightIncome.Name = "lblAllocatedFreightIncome";
this.lblAllocatedFreightIncome.Size = new System.Drawing.Size(41, 12);
this.lblAllocatedFreightIncome.TabIndex = 26;
this.lblAllocatedFreightIncome.Text = "运费收入分摊";
//111======650
this.txtAllocatedFreightIncome.Location = new System.Drawing.Point(173,646);
this.txtAllocatedFreightIncome.Name ="txtAllocatedFreightIncome";
this.txtAllocatedFreightIncome.Size = new System.Drawing.Size(100, 21);
this.txtAllocatedFreightIncome.TabIndex = 26;
this.Controls.Add(this.lblAllocatedFreightIncome);
this.Controls.Add(this.txtAllocatedFreightIncome);

           //#####AllocatedFreightCost###Decimal
this.lblAllocatedFreightCost.AutoSize = true;
this.lblAllocatedFreightCost.Location = new System.Drawing.Point(100,675);
this.lblAllocatedFreightCost.Name = "lblAllocatedFreightCost";
this.lblAllocatedFreightCost.Size = new System.Drawing.Size(41, 12);
this.lblAllocatedFreightCost.TabIndex = 27;
this.lblAllocatedFreightCost.Text = "运费成本分摊";
//111======675
this.txtAllocatedFreightCost.Location = new System.Drawing.Point(173,671);
this.txtAllocatedFreightCost.Name ="txtAllocatedFreightCost";
this.txtAllocatedFreightCost.Size = new System.Drawing.Size(100, 21);
this.txtAllocatedFreightCost.TabIndex = 27;
this.Controls.Add(this.lblAllocatedFreightCost);
this.Controls.Add(this.txtAllocatedFreightCost);

           //#####FreightAllocationRules###Int32
//属性测试700FreightAllocationRules
//属性测试700FreightAllocationRules
//属性测试700FreightAllocationRules
//属性测试700FreightAllocationRules

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.cmbLocation_ID );

                this.Controls.Add(this.lblRack_ID );
this.Controls.Add(this.cmbRack_ID );

                this.Controls.Add(this.lblSaleOut_MainID );
this.Controls.Add(this.cmbSaleOut_MainID );

                
                this.Controls.Add(this.lblUnitPrice );
this.Controls.Add(this.txtUnitPrice );

                this.Controls.Add(this.lblDiscount );
this.Controls.Add(this.txtDiscount );

                this.Controls.Add(this.lblTransactionPrice );
this.Controls.Add(this.txtTransactionPrice );

                this.Controls.Add(this.lblSubtotalTransAmount );
this.Controls.Add(this.txtSubtotalTransAmount );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                this.Controls.Add(this.lblCustomerPartNo );
this.Controls.Add(this.txtCustomerPartNo );

                this.Controls.Add(this.lblCustomizedCost );
this.Controls.Add(this.txtCustomizedCost );

                this.Controls.Add(this.lblCost );
this.Controls.Add(this.txtCost );

                this.Controls.Add(this.lblSubtotalCostAmount );
this.Controls.Add(this.txtSubtotalCostAmount );

                this.Controls.Add(this.lblTaxRate );
this.Controls.Add(this.txtTaxRate );

                
                this.Controls.Add(this.lblGift );
this.Controls.Add(this.chkGift );

                this.Controls.Add(this.lblIncludingTax );
this.Controls.Add(this.chkIncludingTax );

                this.Controls.Add(this.lblSubtotalUntaxedAmount );
this.Controls.Add(this.txtSubtotalUntaxedAmount );

                this.Controls.Add(this.lblUnitCommissionAmount );
this.Controls.Add(this.txtUnitCommissionAmount );

                this.Controls.Add(this.lblCommissionAmount );
this.Controls.Add(this.txtCommissionAmount );

                this.Controls.Add(this.lblSubtotalTaxAmount );
this.Controls.Add(this.txtSubtotalTaxAmount );

                this.Controls.Add(this.lblSaleFlagCode );
this.Controls.Add(this.txtSaleFlagCode );

                
                this.Controls.Add(this.lblAllocatedFreightIncome );
this.Controls.Add(this.txtAllocatedFreightIncome );

                this.Controls.Add(this.lblAllocatedFreightCost );
this.Controls.Add(this.txtAllocatedFreightCost );

                
                    
            this.Name = "tb_SaleOutDetailQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProdDetailID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblproperty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLocation_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRack_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbRack_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSaleOut_MainID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbSaleOut_MainID;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUnitPrice;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUnitPrice;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDiscount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDiscount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTransactionPrice;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTransactionPrice;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalTransAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalTransAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerPartNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCustomerPartNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomizedCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCustomizedCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalCostAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalCostAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTaxRate;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTaxRate;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblGift;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkGift;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIncludingTax;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIncludingTax;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalUntaxedAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalUntaxedAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUnitCommissionAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUnitCommissionAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCommissionAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCommissionAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalTaxAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalTaxAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSaleFlagCode;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSaleFlagCode;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblAllocatedFreightIncome;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtAllocatedFreightIncome;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblAllocatedFreightCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtAllocatedFreightCost;

    
        
              
    
    
   
 





    }
}


