
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
    partial class tb_SaleOrderDetailQuery
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

this.lblProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblproperty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtproperty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

this.lblLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblUnitPrice = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUnitPrice = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblDiscount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDiscount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTransactionPrice = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTransactionPrice = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalTransAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalTransAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCustomizedCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCustomizedCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalCostAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalCostAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblUnitCommissionAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUnitCommissionAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCommissionAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCommissionAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTaxRate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTaxRate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalUntaxedAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalUntaxedAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSummary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSummary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

this.lblCustomerPartNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCustomerPartNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblGift = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkGift = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkGift.Values.Text ="";


this.lblSaleFlagCode = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSaleFlagCode = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
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

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
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

                
                this.Controls.Add(this.lblSaleFlagCode );
this.Controls.Add(this.txtSaleFlagCode );

                    
            this.Name = "tb_SaleOrderDetailQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSOrder_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbSOrder_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProdDetailID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblproperty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLocation_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUnitPrice;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUnitPrice;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDiscount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDiscount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTransactionPrice;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTransactionPrice;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalTransAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalTransAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomizedCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCustomizedCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalCostAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalCostAmount;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUnitCommissionAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUnitCommissionAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCommissionAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCommissionAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTaxRate;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTaxRate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalTaxAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalTaxAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalUntaxedAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalUntaxedAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerPartNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCustomerPartNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblGift;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkGift;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSaleFlagCode;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSaleFlagCode;

    
    
   
 





    }
}


