
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:10
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 采购退货入库单明细
    /// </summary>
    partial class tb_PurReturnEntryDetailQuery
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
     
     this.lblPurReEntry_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbPurReEntry_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();


this.lblCustomertModel = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCustomertModel = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblRack_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbRack_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblproperty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtproperty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;


this.lblUnitPrice = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUnitPrice = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblDiscount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDiscount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTransactionPrice = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTransactionPrice = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblIsGift = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsGift = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsGift.Values.Text ="";

this.lblTaxRate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTaxRate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalTrPriceAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalTrPriceAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblVendorModelCode = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtVendorModelCode = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSummary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSummary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

this.lblDiscountAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDiscountAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblIsIncludeTax = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsIncludeTax = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsIncludeTax.Values.Text ="";

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####PurReEntry_ID###Int64
//属性测试25PurReEntry_ID
//属性测试25PurReEntry_ID
//属性测试25PurReEntry_ID
//属性测试25PurReEntry_ID
this.lblPurReEntry_ID.AutoSize = true;
this.lblPurReEntry_ID.Location = new System.Drawing.Point(100,25);
this.lblPurReEntry_ID.Name = "lblPurReEntry_ID";
this.lblPurReEntry_ID.Size = new System.Drawing.Size(41, 12);
this.lblPurReEntry_ID.TabIndex = 1;
this.lblPurReEntry_ID.Text = "";
//111======25
this.cmbPurReEntry_ID.Location = new System.Drawing.Point(173,21);
this.cmbPurReEntry_ID.Name ="cmbPurReEntry_ID";
this.cmbPurReEntry_ID.Size = new System.Drawing.Size(100, 21);
this.cmbPurReEntry_ID.TabIndex = 1;
this.Controls.Add(this.lblPurReEntry_ID);
this.Controls.Add(this.cmbPurReEntry_ID);

           //#####ProdDetailID###Int64
//属性测试50ProdDetailID
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,50);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 2;
this.lblProdDetailID.Text = "产品";
//111======50
this.cmbProdDetailID.Location = new System.Drawing.Point(173,46);
this.cmbProdDetailID.Name ="cmbProdDetailID";
this.cmbProdDetailID.Size = new System.Drawing.Size(100, 21);
this.cmbProdDetailID.TabIndex = 2;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.cmbProdDetailID);

           //#####PurEntryRe_CID###Int64
//属性测试75PurEntryRe_CID
//属性测试75PurEntryRe_CID
//属性测试75PurEntryRe_CID
//属性测试75PurEntryRe_CID

           //#####50CustomertModel###String
this.lblCustomertModel.AutoSize = true;
this.lblCustomertModel.Location = new System.Drawing.Point(100,100);
this.lblCustomertModel.Name = "lblCustomertModel";
this.lblCustomertModel.Size = new System.Drawing.Size(41, 12);
this.lblCustomertModel.TabIndex = 4;
this.lblCustomertModel.Text = "客户型号";
this.txtCustomertModel.Location = new System.Drawing.Point(173,96);
this.txtCustomertModel.Name = "txtCustomertModel";
this.txtCustomertModel.Size = new System.Drawing.Size(100, 21);
this.txtCustomertModel.TabIndex = 4;
this.Controls.Add(this.lblCustomertModel);
this.Controls.Add(this.txtCustomertModel);

           //#####Location_ID###Int64
//属性测试125Location_ID
//属性测试125Location_ID
this.lblLocation_ID.AutoSize = true;
this.lblLocation_ID.Location = new System.Drawing.Point(100,125);
this.lblLocation_ID.Name = "lblLocation_ID";
this.lblLocation_ID.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID.TabIndex = 5;
this.lblLocation_ID.Text = "库位";
//111======125
this.cmbLocation_ID.Location = new System.Drawing.Point(173,121);
this.cmbLocation_ID.Name ="cmbLocation_ID";
this.cmbLocation_ID.Size = new System.Drawing.Size(100, 21);
this.cmbLocation_ID.TabIndex = 5;
this.Controls.Add(this.lblLocation_ID);
this.Controls.Add(this.cmbLocation_ID);

           //#####Rack_ID###Int64
//属性测试150Rack_ID
//属性测试150Rack_ID
//属性测试150Rack_ID
this.lblRack_ID.AutoSize = true;
this.lblRack_ID.Location = new System.Drawing.Point(100,150);
this.lblRack_ID.Name = "lblRack_ID";
this.lblRack_ID.Size = new System.Drawing.Size(41, 12);
this.lblRack_ID.TabIndex = 6;
this.lblRack_ID.Text = "货架";
//111======150
this.cmbRack_ID.Location = new System.Drawing.Point(173,146);
this.cmbRack_ID.Name ="cmbRack_ID";
this.cmbRack_ID.Size = new System.Drawing.Size(100, 21);
this.cmbRack_ID.TabIndex = 6;
this.Controls.Add(this.lblRack_ID);
this.Controls.Add(this.cmbRack_ID);

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,175);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 7;
this.lblproperty.Text = "属性";
this.txtproperty.Location = new System.Drawing.Point(173,171);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 7;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

           //#####Quantity###Int32
//属性测试200Quantity
//属性测试200Quantity
//属性测试200Quantity
//属性测试200Quantity

           //#####UnitPrice###Decimal
this.lblUnitPrice.AutoSize = true;
this.lblUnitPrice.Location = new System.Drawing.Point(100,225);
this.lblUnitPrice.Name = "lblUnitPrice";
this.lblUnitPrice.Size = new System.Drawing.Size(41, 12);
this.lblUnitPrice.TabIndex = 9;
this.lblUnitPrice.Text = "单价";
//111======225
this.txtUnitPrice.Location = new System.Drawing.Point(173,221);
this.txtUnitPrice.Name ="txtUnitPrice";
this.txtUnitPrice.Size = new System.Drawing.Size(100, 21);
this.txtUnitPrice.TabIndex = 9;
this.Controls.Add(this.lblUnitPrice);
this.Controls.Add(this.txtUnitPrice);

           //#####Discount###Decimal
this.lblDiscount.AutoSize = true;
this.lblDiscount.Location = new System.Drawing.Point(100,250);
this.lblDiscount.Name = "lblDiscount";
this.lblDiscount.Size = new System.Drawing.Size(41, 12);
this.lblDiscount.TabIndex = 10;
this.lblDiscount.Text = "折扣";
//111======250
this.txtDiscount.Location = new System.Drawing.Point(173,246);
this.txtDiscount.Name ="txtDiscount";
this.txtDiscount.Size = new System.Drawing.Size(100, 21);
this.txtDiscount.TabIndex = 10;
this.Controls.Add(this.lblDiscount);
this.Controls.Add(this.txtDiscount);

           //#####TransactionPrice###Decimal
this.lblTransactionPrice.AutoSize = true;
this.lblTransactionPrice.Location = new System.Drawing.Point(100,275);
this.lblTransactionPrice.Name = "lblTransactionPrice";
this.lblTransactionPrice.Size = new System.Drawing.Size(41, 12);
this.lblTransactionPrice.TabIndex = 11;
this.lblTransactionPrice.Text = "成交单价";
//111======275
this.txtTransactionPrice.Location = new System.Drawing.Point(173,271);
this.txtTransactionPrice.Name ="txtTransactionPrice";
this.txtTransactionPrice.Size = new System.Drawing.Size(100, 21);
this.txtTransactionPrice.TabIndex = 11;
this.Controls.Add(this.lblTransactionPrice);
this.Controls.Add(this.txtTransactionPrice);

           //#####IsGift###Boolean
this.lblIsGift.AutoSize = true;
this.lblIsGift.Location = new System.Drawing.Point(100,300);
this.lblIsGift.Name = "lblIsGift";
this.lblIsGift.Size = new System.Drawing.Size(41, 12);
this.lblIsGift.TabIndex = 12;
this.lblIsGift.Text = "赠品";
this.chkIsGift.Location = new System.Drawing.Point(173,296);
this.chkIsGift.Name = "chkIsGift";
this.chkIsGift.Size = new System.Drawing.Size(100, 21);
this.chkIsGift.TabIndex = 12;
this.Controls.Add(this.lblIsGift);
this.Controls.Add(this.chkIsGift);

           //#####TaxRate###Decimal
this.lblTaxRate.AutoSize = true;
this.lblTaxRate.Location = new System.Drawing.Point(100,325);
this.lblTaxRate.Name = "lblTaxRate";
this.lblTaxRate.Size = new System.Drawing.Size(41, 12);
this.lblTaxRate.TabIndex = 13;
this.lblTaxRate.Text = "税率";
//111======325
this.txtTaxRate.Location = new System.Drawing.Point(173,321);
this.txtTaxRate.Name ="txtTaxRate";
this.txtTaxRate.Size = new System.Drawing.Size(100, 21);
this.txtTaxRate.TabIndex = 13;
this.Controls.Add(this.lblTaxRate);
this.Controls.Add(this.txtTaxRate);

           //#####TaxAmount###Decimal
this.lblTaxAmount.AutoSize = true;
this.lblTaxAmount.Location = new System.Drawing.Point(100,350);
this.lblTaxAmount.Name = "lblTaxAmount";
this.lblTaxAmount.Size = new System.Drawing.Size(41, 12);
this.lblTaxAmount.TabIndex = 14;
this.lblTaxAmount.Text = "税额";
//111======350
this.txtTaxAmount.Location = new System.Drawing.Point(173,346);
this.txtTaxAmount.Name ="txtTaxAmount";
this.txtTaxAmount.Size = new System.Drawing.Size(100, 21);
this.txtTaxAmount.TabIndex = 14;
this.Controls.Add(this.lblTaxAmount);
this.Controls.Add(this.txtTaxAmount);

           //#####SubtotalTrPriceAmount###Decimal
this.lblSubtotalTrPriceAmount.AutoSize = true;
this.lblSubtotalTrPriceAmount.Location = new System.Drawing.Point(100,375);
this.lblSubtotalTrPriceAmount.Name = "lblSubtotalTrPriceAmount";
this.lblSubtotalTrPriceAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalTrPriceAmount.TabIndex = 15;
this.lblSubtotalTrPriceAmount.Text = "小计";
//111======375
this.txtSubtotalTrPriceAmount.Location = new System.Drawing.Point(173,371);
this.txtSubtotalTrPriceAmount.Name ="txtSubtotalTrPriceAmount";
this.txtSubtotalTrPriceAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalTrPriceAmount.TabIndex = 15;
this.Controls.Add(this.lblSubtotalTrPriceAmount);
this.Controls.Add(this.txtSubtotalTrPriceAmount);

           //#####50VendorModelCode###String
this.lblVendorModelCode.AutoSize = true;
this.lblVendorModelCode.Location = new System.Drawing.Point(100,400);
this.lblVendorModelCode.Name = "lblVendorModelCode";
this.lblVendorModelCode.Size = new System.Drawing.Size(41, 12);
this.lblVendorModelCode.TabIndex = 16;
this.lblVendorModelCode.Text = "厂商型号";
this.txtVendorModelCode.Location = new System.Drawing.Point(173,396);
this.txtVendorModelCode.Name = "txtVendorModelCode";
this.txtVendorModelCode.Size = new System.Drawing.Size(100, 21);
this.txtVendorModelCode.TabIndex = 16;
this.Controls.Add(this.lblVendorModelCode);
this.Controls.Add(this.txtVendorModelCode);

           //#####1000Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,425);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 17;
this.lblSummary.Text = "摘要";
this.txtSummary.Location = new System.Drawing.Point(173,421);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 17;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####DiscountAmount###Decimal
this.lblDiscountAmount.AutoSize = true;
this.lblDiscountAmount.Location = new System.Drawing.Point(100,450);
this.lblDiscountAmount.Name = "lblDiscountAmount";
this.lblDiscountAmount.Size = new System.Drawing.Size(41, 12);
this.lblDiscountAmount.TabIndex = 18;
this.lblDiscountAmount.Text = "优惠金额";
//111======450
this.txtDiscountAmount.Location = new System.Drawing.Point(173,446);
this.txtDiscountAmount.Name ="txtDiscountAmount";
this.txtDiscountAmount.Size = new System.Drawing.Size(100, 21);
this.txtDiscountAmount.TabIndex = 18;
this.Controls.Add(this.lblDiscountAmount);
this.Controls.Add(this.txtDiscountAmount);

           //#####IsIncludeTax###Boolean
this.lblIsIncludeTax.AutoSize = true;
this.lblIsIncludeTax.Location = new System.Drawing.Point(100,475);
this.lblIsIncludeTax.Name = "lblIsIncludeTax";
this.lblIsIncludeTax.Size = new System.Drawing.Size(41, 12);
this.lblIsIncludeTax.TabIndex = 19;
this.lblIsIncludeTax.Text = "含税";
this.chkIsIncludeTax.Location = new System.Drawing.Point(173,471);
this.chkIsIncludeTax.Name = "chkIsIncludeTax";
this.chkIsIncludeTax.Size = new System.Drawing.Size(100, 21);
this.chkIsIncludeTax.TabIndex = 19;
this.Controls.Add(this.lblIsIncludeTax);
this.Controls.Add(this.chkIsIncludeTax);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblPurReEntry_ID );
this.Controls.Add(this.cmbPurReEntry_ID );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                
                this.Controls.Add(this.lblCustomertModel );
this.Controls.Add(this.txtCustomertModel );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.cmbLocation_ID );

                this.Controls.Add(this.lblRack_ID );
this.Controls.Add(this.cmbRack_ID );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                
                this.Controls.Add(this.lblUnitPrice );
this.Controls.Add(this.txtUnitPrice );

                this.Controls.Add(this.lblDiscount );
this.Controls.Add(this.txtDiscount );

                this.Controls.Add(this.lblTransactionPrice );
this.Controls.Add(this.txtTransactionPrice );

                this.Controls.Add(this.lblIsGift );
this.Controls.Add(this.chkIsGift );

                this.Controls.Add(this.lblTaxRate );
this.Controls.Add(this.txtTaxRate );

                this.Controls.Add(this.lblTaxAmount );
this.Controls.Add(this.txtTaxAmount );

                this.Controls.Add(this.lblSubtotalTrPriceAmount );
this.Controls.Add(this.txtSubtotalTrPriceAmount );

                this.Controls.Add(this.lblVendorModelCode );
this.Controls.Add(this.txtVendorModelCode );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                this.Controls.Add(this.lblDiscountAmount );
this.Controls.Add(this.txtDiscountAmount );

                this.Controls.Add(this.lblIsIncludeTax );
this.Controls.Add(this.chkIsIncludeTax );

                    
            this.Name = "tb_PurReturnEntryDetailQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPurReEntry_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbPurReEntry_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProdDetailID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomertModel;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCustomertModel;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLocation_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRack_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbRack_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblproperty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUnitPrice;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUnitPrice;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDiscount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDiscount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTransactionPrice;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTransactionPrice;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsGift;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsGift;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTaxRate;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTaxRate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTaxAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTaxAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalTrPriceAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalTrPriceAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblVendorModelCode;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtVendorModelCode;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDiscountAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDiscountAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsIncludeTax;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsIncludeTax;

    
    
   
 





    }
}


