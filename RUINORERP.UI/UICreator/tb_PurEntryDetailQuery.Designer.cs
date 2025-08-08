
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
    /// 采购入库单
    /// </summary>
    partial class tb_PurEntryDetailQuery
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
     
     this.lblLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblPurEntryID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbPurEntryID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblproperty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtproperty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;


this.lblUnitPrice = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUnitPrice = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCustomizedCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCustomizedCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblUntaxedCustomizedCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUntaxedCustomizedCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblDiscount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDiscount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblIsGift = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsGift = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsGift.Values.Text ="";

this.lblTaxRate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTaxRate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblUntaxedUnitPrice = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUntaxedUnitPrice = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTransactionPrice = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTransactionPrice = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalUntaxedAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalUntaxedAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblVendorModelCode = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtVendorModelCode = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCustomertModel = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCustomertModel = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSummary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSummary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

this.lblIsIncludeTax = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsIncludeTax = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsIncludeTax.Values.Text ="";


this.lblRack_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbRack_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();


this.lblAllocatedFreightCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtAllocatedFreightCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


    //for end
    this.SuspendLayout();
    
         //for start
                 //#####Location_ID###Int64
//属性测试25Location_ID
//属性测试25Location_ID
this.lblLocation_ID.AutoSize = true;
this.lblLocation_ID.Location = new System.Drawing.Point(100,25);
this.lblLocation_ID.Name = "lblLocation_ID";
this.lblLocation_ID.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID.TabIndex = 1;
this.lblLocation_ID.Text = "库位";
//111======25
this.cmbLocation_ID.Location = new System.Drawing.Point(173,21);
this.cmbLocation_ID.Name ="cmbLocation_ID";
this.cmbLocation_ID.Size = new System.Drawing.Size(100, 21);
this.cmbLocation_ID.TabIndex = 1;
this.Controls.Add(this.lblLocation_ID);
this.Controls.Add(this.cmbLocation_ID);

           //#####ProdDetailID###Int64
//属性测试50ProdDetailID
//属性测试50ProdDetailID
//属性测试50ProdDetailID
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,50);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 2;
this.lblProdDetailID.Text = "货品详情";
//111======50
this.cmbProdDetailID.Location = new System.Drawing.Point(173,46);
this.cmbProdDetailID.Name ="cmbProdDetailID";
this.cmbProdDetailID.Size = new System.Drawing.Size(100, 21);
this.cmbProdDetailID.TabIndex = 2;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.cmbProdDetailID);

           //#####PurEntryID###Int64
//属性测试75PurEntryID
this.lblPurEntryID.AutoSize = true;
this.lblPurEntryID.Location = new System.Drawing.Point(100,75);
this.lblPurEntryID.Name = "lblPurEntryID";
this.lblPurEntryID.Size = new System.Drawing.Size(41, 12);
this.lblPurEntryID.TabIndex = 3;
this.lblPurEntryID.Text = "采购入库单";
//111======75
this.cmbPurEntryID.Location = new System.Drawing.Point(173,71);
this.cmbPurEntryID.Name ="cmbPurEntryID";
this.cmbPurEntryID.Size = new System.Drawing.Size(100, 21);
this.cmbPurEntryID.TabIndex = 3;
this.Controls.Add(this.lblPurEntryID);
this.Controls.Add(this.cmbPurEntryID);

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,100);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 4;
this.lblproperty.Text = "属性";
this.txtproperty.Location = new System.Drawing.Point(173,96);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 4;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

           //#####Quantity###Int32
//属性测试125Quantity
//属性测试125Quantity
//属性测试125Quantity
//属性测试125Quantity

           //#####UnitPrice###Decimal
this.lblUnitPrice.AutoSize = true;
this.lblUnitPrice.Location = new System.Drawing.Point(100,150);
this.lblUnitPrice.Name = "lblUnitPrice";
this.lblUnitPrice.Size = new System.Drawing.Size(41, 12);
this.lblUnitPrice.TabIndex = 6;
this.lblUnitPrice.Text = "单价";
//111======150
this.txtUnitPrice.Location = new System.Drawing.Point(173,146);
this.txtUnitPrice.Name ="txtUnitPrice";
this.txtUnitPrice.Size = new System.Drawing.Size(100, 21);
this.txtUnitPrice.TabIndex = 6;
this.Controls.Add(this.lblUnitPrice);
this.Controls.Add(this.txtUnitPrice);

           //#####CustomizedCost###Decimal
this.lblCustomizedCost.AutoSize = true;
this.lblCustomizedCost.Location = new System.Drawing.Point(100,175);
this.lblCustomizedCost.Name = "lblCustomizedCost";
this.lblCustomizedCost.Size = new System.Drawing.Size(41, 12);
this.lblCustomizedCost.TabIndex = 7;
this.lblCustomizedCost.Text = "定制成本";
//111======175
this.txtCustomizedCost.Location = new System.Drawing.Point(173,171);
this.txtCustomizedCost.Name ="txtCustomizedCost";
this.txtCustomizedCost.Size = new System.Drawing.Size(100, 21);
this.txtCustomizedCost.TabIndex = 7;
this.Controls.Add(this.lblCustomizedCost);
this.Controls.Add(this.txtCustomizedCost);

           //#####UntaxedCustomizedCost###Decimal
this.lblUntaxedCustomizedCost.AutoSize = true;
this.lblUntaxedCustomizedCost.Location = new System.Drawing.Point(100,200);
this.lblUntaxedCustomizedCost.Name = "lblUntaxedCustomizedCost";
this.lblUntaxedCustomizedCost.Size = new System.Drawing.Size(41, 12);
this.lblUntaxedCustomizedCost.TabIndex = 8;
this.lblUntaxedCustomizedCost.Text = "未税定制成本";
//111======200
this.txtUntaxedCustomizedCost.Location = new System.Drawing.Point(173,196);
this.txtUntaxedCustomizedCost.Name ="txtUntaxedCustomizedCost";
this.txtUntaxedCustomizedCost.Size = new System.Drawing.Size(100, 21);
this.txtUntaxedCustomizedCost.TabIndex = 8;
this.Controls.Add(this.lblUntaxedCustomizedCost);
this.Controls.Add(this.txtUntaxedCustomizedCost);

           //#####Discount###Decimal
this.lblDiscount.AutoSize = true;
this.lblDiscount.Location = new System.Drawing.Point(100,225);
this.lblDiscount.Name = "lblDiscount";
this.lblDiscount.Size = new System.Drawing.Size(41, 12);
this.lblDiscount.TabIndex = 9;
this.lblDiscount.Text = "折扣";
//111======225
this.txtDiscount.Location = new System.Drawing.Point(173,221);
this.txtDiscount.Name ="txtDiscount";
this.txtDiscount.Size = new System.Drawing.Size(100, 21);
this.txtDiscount.TabIndex = 9;
this.Controls.Add(this.lblDiscount);
this.Controls.Add(this.txtDiscount);

           //#####IsGift###Boolean
this.lblIsGift.AutoSize = true;
this.lblIsGift.Location = new System.Drawing.Point(100,250);
this.lblIsGift.Name = "lblIsGift";
this.lblIsGift.Size = new System.Drawing.Size(41, 12);
this.lblIsGift.TabIndex = 10;
this.lblIsGift.Text = "赠品";
this.chkIsGift.Location = new System.Drawing.Point(173,246);
this.chkIsGift.Name = "chkIsGift";
this.chkIsGift.Size = new System.Drawing.Size(100, 21);
this.chkIsGift.TabIndex = 10;
this.Controls.Add(this.lblIsGift);
this.Controls.Add(this.chkIsGift);

           //#####TaxRate###Decimal
this.lblTaxRate.AutoSize = true;
this.lblTaxRate.Location = new System.Drawing.Point(100,275);
this.lblTaxRate.Name = "lblTaxRate";
this.lblTaxRate.Size = new System.Drawing.Size(41, 12);
this.lblTaxRate.TabIndex = 11;
this.lblTaxRate.Text = "税率";
//111======275
this.txtTaxRate.Location = new System.Drawing.Point(173,271);
this.txtTaxRate.Name ="txtTaxRate";
this.txtTaxRate.Size = new System.Drawing.Size(100, 21);
this.txtTaxRate.TabIndex = 11;
this.Controls.Add(this.lblTaxRate);
this.Controls.Add(this.txtTaxRate);

           //#####TaxAmount###Decimal
this.lblTaxAmount.AutoSize = true;
this.lblTaxAmount.Location = new System.Drawing.Point(100,300);
this.lblTaxAmount.Name = "lblTaxAmount";
this.lblTaxAmount.Size = new System.Drawing.Size(41, 12);
this.lblTaxAmount.TabIndex = 12;
this.lblTaxAmount.Text = "税额";
//111======300
this.txtTaxAmount.Location = new System.Drawing.Point(173,296);
this.txtTaxAmount.Name ="txtTaxAmount";
this.txtTaxAmount.Size = new System.Drawing.Size(100, 21);
this.txtTaxAmount.TabIndex = 12;
this.Controls.Add(this.lblTaxAmount);
this.Controls.Add(this.txtTaxAmount);

           //#####UntaxedUnitPrice###Decimal
this.lblUntaxedUnitPrice.AutoSize = true;
this.lblUntaxedUnitPrice.Location = new System.Drawing.Point(100,325);
this.lblUntaxedUnitPrice.Name = "lblUntaxedUnitPrice";
this.lblUntaxedUnitPrice.Size = new System.Drawing.Size(41, 12);
this.lblUntaxedUnitPrice.TabIndex = 13;
this.lblUntaxedUnitPrice.Text = "未税单价";
//111======325
this.txtUntaxedUnitPrice.Location = new System.Drawing.Point(173,321);
this.txtUntaxedUnitPrice.Name ="txtUntaxedUnitPrice";
this.txtUntaxedUnitPrice.Size = new System.Drawing.Size(100, 21);
this.txtUntaxedUnitPrice.TabIndex = 13;
this.Controls.Add(this.lblUntaxedUnitPrice);
this.Controls.Add(this.txtUntaxedUnitPrice);

           //#####TransactionPrice###Decimal
this.lblTransactionPrice.AutoSize = true;
this.lblTransactionPrice.Location = new System.Drawing.Point(100,350);
this.lblTransactionPrice.Name = "lblTransactionPrice";
this.lblTransactionPrice.Size = new System.Drawing.Size(41, 12);
this.lblTransactionPrice.TabIndex = 14;
this.lblTransactionPrice.Text = "成交单价";
//111======350
this.txtTransactionPrice.Location = new System.Drawing.Point(173,346);
this.txtTransactionPrice.Name ="txtTransactionPrice";
this.txtTransactionPrice.Size = new System.Drawing.Size(100, 21);
this.txtTransactionPrice.TabIndex = 14;
this.Controls.Add(this.lblTransactionPrice);
this.Controls.Add(this.txtTransactionPrice);

           //#####SubtotalAmount###Decimal
this.lblSubtotalAmount.AutoSize = true;
this.lblSubtotalAmount.Location = new System.Drawing.Point(100,375);
this.lblSubtotalAmount.Name = "lblSubtotalAmount";
this.lblSubtotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalAmount.TabIndex = 15;
this.lblSubtotalAmount.Text = "成交小计";
//111======375
this.txtSubtotalAmount.Location = new System.Drawing.Point(173,371);
this.txtSubtotalAmount.Name ="txtSubtotalAmount";
this.txtSubtotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalAmount.TabIndex = 15;
this.Controls.Add(this.lblSubtotalAmount);
this.Controls.Add(this.txtSubtotalAmount);

           //#####SubtotalUntaxedAmount###Decimal
this.lblSubtotalUntaxedAmount.AutoSize = true;
this.lblSubtotalUntaxedAmount.Location = new System.Drawing.Point(100,400);
this.lblSubtotalUntaxedAmount.Name = "lblSubtotalUntaxedAmount";
this.lblSubtotalUntaxedAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalUntaxedAmount.TabIndex = 16;
this.lblSubtotalUntaxedAmount.Text = "未税金额小计";
//111======400
this.txtSubtotalUntaxedAmount.Location = new System.Drawing.Point(173,396);
this.txtSubtotalUntaxedAmount.Name ="txtSubtotalUntaxedAmount";
this.txtSubtotalUntaxedAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalUntaxedAmount.TabIndex = 16;
this.Controls.Add(this.lblSubtotalUntaxedAmount);
this.Controls.Add(this.txtSubtotalUntaxedAmount);

           //#####50VendorModelCode###String
this.lblVendorModelCode.AutoSize = true;
this.lblVendorModelCode.Location = new System.Drawing.Point(100,425);
this.lblVendorModelCode.Name = "lblVendorModelCode";
this.lblVendorModelCode.Size = new System.Drawing.Size(41, 12);
this.lblVendorModelCode.TabIndex = 17;
this.lblVendorModelCode.Text = "厂商型号";
this.txtVendorModelCode.Location = new System.Drawing.Point(173,421);
this.txtVendorModelCode.Name = "txtVendorModelCode";
this.txtVendorModelCode.Size = new System.Drawing.Size(100, 21);
this.txtVendorModelCode.TabIndex = 17;
this.Controls.Add(this.lblVendorModelCode);
this.Controls.Add(this.txtVendorModelCode);

           //#####50CustomertModel###String
this.lblCustomertModel.AutoSize = true;
this.lblCustomertModel.Location = new System.Drawing.Point(100,450);
this.lblCustomertModel.Name = "lblCustomertModel";
this.lblCustomertModel.Size = new System.Drawing.Size(41, 12);
this.lblCustomertModel.TabIndex = 18;
this.lblCustomertModel.Text = "客户型号";
this.txtCustomertModel.Location = new System.Drawing.Point(173,446);
this.txtCustomertModel.Name = "txtCustomertModel";
this.txtCustomertModel.Size = new System.Drawing.Size(100, 21);
this.txtCustomertModel.TabIndex = 18;
this.Controls.Add(this.lblCustomertModel);
this.Controls.Add(this.txtCustomertModel);

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

           //#####IsIncludeTax###Boolean
this.lblIsIncludeTax.AutoSize = true;
this.lblIsIncludeTax.Location = new System.Drawing.Point(100,500);
this.lblIsIncludeTax.Name = "lblIsIncludeTax";
this.lblIsIncludeTax.Size = new System.Drawing.Size(41, 12);
this.lblIsIncludeTax.TabIndex = 20;
this.lblIsIncludeTax.Text = "含税";
this.chkIsIncludeTax.Location = new System.Drawing.Point(173,496);
this.chkIsIncludeTax.Name = "chkIsIncludeTax";
this.chkIsIncludeTax.Size = new System.Drawing.Size(100, 21);
this.chkIsIncludeTax.TabIndex = 20;
this.Controls.Add(this.lblIsIncludeTax);
this.Controls.Add(this.chkIsIncludeTax);

           //#####ReturnedQty###Int32
//属性测试525ReturnedQty
//属性测试525ReturnedQty
//属性测试525ReturnedQty
//属性测试525ReturnedQty

           //#####Rack_ID###Int64
//属性测试550Rack_ID
//属性测试550Rack_ID
//属性测试550Rack_ID
//属性测试550Rack_ID
this.lblRack_ID.AutoSize = true;
this.lblRack_ID.Location = new System.Drawing.Point(100,550);
this.lblRack_ID.Name = "lblRack_ID";
this.lblRack_ID.Size = new System.Drawing.Size(41, 12);
this.lblRack_ID.TabIndex = 22;
this.lblRack_ID.Text = "货架";
//111======550
this.cmbRack_ID.Location = new System.Drawing.Point(173,546);
this.cmbRack_ID.Name ="cmbRack_ID";
this.cmbRack_ID.Size = new System.Drawing.Size(100, 21);
this.cmbRack_ID.TabIndex = 22;
this.Controls.Add(this.lblRack_ID);
this.Controls.Add(this.cmbRack_ID);

           //#####PurOrder_ChildID###Int64
//属性测试575PurOrder_ChildID
//属性测试575PurOrder_ChildID
//属性测试575PurOrder_ChildID
//属性测试575PurOrder_ChildID

           //#####AllocatedFreightCost###Decimal
this.lblAllocatedFreightCost.AutoSize = true;
this.lblAllocatedFreightCost.Location = new System.Drawing.Point(100,600);
this.lblAllocatedFreightCost.Name = "lblAllocatedFreightCost";
this.lblAllocatedFreightCost.Size = new System.Drawing.Size(41, 12);
this.lblAllocatedFreightCost.TabIndex = 24;
this.lblAllocatedFreightCost.Text = "运费成本分摊";
//111======600
this.txtAllocatedFreightCost.Location = new System.Drawing.Point(173,596);
this.txtAllocatedFreightCost.Name ="txtAllocatedFreightCost";
this.txtAllocatedFreightCost.Size = new System.Drawing.Size(100, 21);
this.txtAllocatedFreightCost.TabIndex = 24;
this.Controls.Add(this.lblAllocatedFreightCost);
this.Controls.Add(this.txtAllocatedFreightCost);

           //#####FreightAllocationRules###Int32
//属性测试625FreightAllocationRules
//属性测试625FreightAllocationRules
//属性测试625FreightAllocationRules
//属性测试625FreightAllocationRules

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.cmbLocation_ID );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblPurEntryID );
this.Controls.Add(this.cmbPurEntryID );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                
                this.Controls.Add(this.lblUnitPrice );
this.Controls.Add(this.txtUnitPrice );

                this.Controls.Add(this.lblCustomizedCost );
this.Controls.Add(this.txtCustomizedCost );

                this.Controls.Add(this.lblUntaxedCustomizedCost );
this.Controls.Add(this.txtUntaxedCustomizedCost );

                this.Controls.Add(this.lblDiscount );
this.Controls.Add(this.txtDiscount );

                this.Controls.Add(this.lblIsGift );
this.Controls.Add(this.chkIsGift );

                this.Controls.Add(this.lblTaxRate );
this.Controls.Add(this.txtTaxRate );

                this.Controls.Add(this.lblTaxAmount );
this.Controls.Add(this.txtTaxAmount );

                this.Controls.Add(this.lblUntaxedUnitPrice );
this.Controls.Add(this.txtUntaxedUnitPrice );

                this.Controls.Add(this.lblTransactionPrice );
this.Controls.Add(this.txtTransactionPrice );

                this.Controls.Add(this.lblSubtotalAmount );
this.Controls.Add(this.txtSubtotalAmount );

                this.Controls.Add(this.lblSubtotalUntaxedAmount );
this.Controls.Add(this.txtSubtotalUntaxedAmount );

                this.Controls.Add(this.lblVendorModelCode );
this.Controls.Add(this.txtVendorModelCode );

                this.Controls.Add(this.lblCustomertModel );
this.Controls.Add(this.txtCustomertModel );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                this.Controls.Add(this.lblIsIncludeTax );
this.Controls.Add(this.chkIsIncludeTax );

                
                this.Controls.Add(this.lblRack_ID );
this.Controls.Add(this.cmbRack_ID );

                
                this.Controls.Add(this.lblAllocatedFreightCost );
this.Controls.Add(this.txtAllocatedFreightCost );

                
                    
            this.Name = "tb_PurEntryDetailQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLocation_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProdDetailID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPurEntryID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbPurEntryID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblproperty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUnitPrice;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUnitPrice;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomizedCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCustomizedCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUntaxedCustomizedCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUntaxedCustomizedCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDiscount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDiscount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsGift;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsGift;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTaxRate;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTaxRate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTaxAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTaxAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUntaxedUnitPrice;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUntaxedUnitPrice;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTransactionPrice;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTransactionPrice;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalUntaxedAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalUntaxedAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblVendorModelCode;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtVendorModelCode;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomertModel;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCustomertModel;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsIncludeTax;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsIncludeTax;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRack_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbRack_ID;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblAllocatedFreightCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtAllocatedFreightCost;

    
        
              
    
    
   
 





    }
}


