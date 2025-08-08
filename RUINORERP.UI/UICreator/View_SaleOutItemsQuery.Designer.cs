
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
    /// 销售出库统计分析
    /// </summary>
    partial class View_SaleOutItemsQuery
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
     
     this.lblSKU = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSKU = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();



this.lblSaleOutNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSaleOutNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();




this.lblOutDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpOutDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblDeliveryDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpDeliveryDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblSaleOrderNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSaleOrderNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblproperty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtproperty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;




this.lblTransactionPrice = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTransactionPrice = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalTransAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalTransAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSummary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSummary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

this.lblCustomerPartNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCustomerPartNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalCostAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalCostAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTaxRate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTaxRate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblGift = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkGift = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkGift.Values.Text ="";

this.lblUnitPrice = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUnitPrice = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblDiscount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDiscount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCommissionAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCommissionAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCNName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCNName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtCNName.Multiline = true;

this.lblSpecifications = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSpecifications = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSpecifications.Multiline = true;

this.lblProductNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtProductNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblModel = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtModel = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();



this.lblGrossProfit = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtGrossProfit = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblGrossProfitRatio = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtGrossProfitRatio = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
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

           //#####CustomerVendor_ID###Int64

           //#####50SaleOutNo###String
this.lblSaleOutNo.AutoSize = true;
this.lblSaleOutNo.Location = new System.Drawing.Point(100,100);
this.lblSaleOutNo.Name = "lblSaleOutNo";
this.lblSaleOutNo.Size = new System.Drawing.Size(41, 12);
this.lblSaleOutNo.TabIndex = 4;
this.lblSaleOutNo.Text = "";
this.txtSaleOutNo.Location = new System.Drawing.Point(173,96);
this.txtSaleOutNo.Name = "txtSaleOutNo";
this.txtSaleOutNo.Size = new System.Drawing.Size(100, 21);
this.txtSaleOutNo.TabIndex = 4;
this.Controls.Add(this.lblSaleOutNo);
this.Controls.Add(this.txtSaleOutNo);

           //#####PayStatus###Int32

           //#####Paytype_ID###Int64

           //#####ProjectGroup_ID###Int64

           //#####OutDate###DateTime
this.lblOutDate.AutoSize = true;
this.lblOutDate.Location = new System.Drawing.Point(100,200);
this.lblOutDate.Name = "lblOutDate";
this.lblOutDate.Size = new System.Drawing.Size(41, 12);
this.lblOutDate.TabIndex = 8;
this.lblOutDate.Text = "";
//111======200
this.dtpOutDate.Location = new System.Drawing.Point(173,196);
this.dtpOutDate.Name ="dtpOutDate";
this.dtpOutDate.ShowCheckBox =true;
this.dtpOutDate.Size = new System.Drawing.Size(100, 21);
this.dtpOutDate.TabIndex = 8;
this.Controls.Add(this.lblOutDate);
this.Controls.Add(this.dtpOutDate);

           //#####DeliveryDate###DateTime
this.lblDeliveryDate.AutoSize = true;
this.lblDeliveryDate.Location = new System.Drawing.Point(100,225);
this.lblDeliveryDate.Name = "lblDeliveryDate";
this.lblDeliveryDate.Size = new System.Drawing.Size(41, 12);
this.lblDeliveryDate.TabIndex = 9;
this.lblDeliveryDate.Text = "";
//111======225
this.dtpDeliveryDate.Location = new System.Drawing.Point(173,221);
this.dtpDeliveryDate.Name ="dtpDeliveryDate";
this.dtpDeliveryDate.ShowCheckBox =true;
this.dtpDeliveryDate.Size = new System.Drawing.Size(100, 21);
this.dtpDeliveryDate.TabIndex = 9;
this.Controls.Add(this.lblDeliveryDate);
this.Controls.Add(this.dtpDeliveryDate);

           //#####50SaleOrderNo###String
this.lblSaleOrderNo.AutoSize = true;
this.lblSaleOrderNo.Location = new System.Drawing.Point(100,250);
this.lblSaleOrderNo.Name = "lblSaleOrderNo";
this.lblSaleOrderNo.Size = new System.Drawing.Size(41, 12);
this.lblSaleOrderNo.TabIndex = 10;
this.lblSaleOrderNo.Text = "";
this.txtSaleOrderNo.Location = new System.Drawing.Point(173,246);
this.txtSaleOrderNo.Name = "txtSaleOrderNo";
this.txtSaleOrderNo.Size = new System.Drawing.Size(100, 21);
this.txtSaleOrderNo.TabIndex = 10;
this.Controls.Add(this.lblSaleOrderNo);
this.Controls.Add(this.txtSaleOrderNo);

           //#####ProdDetailID###Int64

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,300);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 12;
this.lblproperty.Text = "";
this.txtproperty.Location = new System.Drawing.Point(173,296);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 12;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

           //#####Location_ID###Int64

           //#####Rack_ID###Int64

           //#####Quantity###Int32

           //#####TransactionPrice###Decimal
this.lblTransactionPrice.AutoSize = true;
this.lblTransactionPrice.Location = new System.Drawing.Point(100,400);
this.lblTransactionPrice.Name = "lblTransactionPrice";
this.lblTransactionPrice.Size = new System.Drawing.Size(41, 12);
this.lblTransactionPrice.TabIndex = 16;
this.lblTransactionPrice.Text = "";
//111======400
this.txtTransactionPrice.Location = new System.Drawing.Point(173,396);
this.txtTransactionPrice.Name ="txtTransactionPrice";
this.txtTransactionPrice.Size = new System.Drawing.Size(100, 21);
this.txtTransactionPrice.TabIndex = 16;
this.Controls.Add(this.lblTransactionPrice);
this.Controls.Add(this.txtTransactionPrice);

           //#####SubtotalTransAmount###Decimal
this.lblSubtotalTransAmount.AutoSize = true;
this.lblSubtotalTransAmount.Location = new System.Drawing.Point(100,425);
this.lblSubtotalTransAmount.Name = "lblSubtotalTransAmount";
this.lblSubtotalTransAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalTransAmount.TabIndex = 17;
this.lblSubtotalTransAmount.Text = "";
//111======425
this.txtSubtotalTransAmount.Location = new System.Drawing.Point(173,421);
this.txtSubtotalTransAmount.Name ="txtSubtotalTransAmount";
this.txtSubtotalTransAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalTransAmount.TabIndex = 17;
this.Controls.Add(this.lblSubtotalTransAmount);
this.Controls.Add(this.txtSubtotalTransAmount);

           //#####1000Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,450);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 18;
this.lblSummary.Text = "";
this.txtSummary.Location = new System.Drawing.Point(173,446);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 18;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####50CustomerPartNo###String
this.lblCustomerPartNo.AutoSize = true;
this.lblCustomerPartNo.Location = new System.Drawing.Point(100,475);
this.lblCustomerPartNo.Name = "lblCustomerPartNo";
this.lblCustomerPartNo.Size = new System.Drawing.Size(41, 12);
this.lblCustomerPartNo.TabIndex = 19;
this.lblCustomerPartNo.Text = "";
this.txtCustomerPartNo.Location = new System.Drawing.Point(173,471);
this.txtCustomerPartNo.Name = "txtCustomerPartNo";
this.txtCustomerPartNo.Size = new System.Drawing.Size(100, 21);
this.txtCustomerPartNo.TabIndex = 19;
this.Controls.Add(this.lblCustomerPartNo);
this.Controls.Add(this.txtCustomerPartNo);

           //#####Cost###Decimal
this.lblCost.AutoSize = true;
this.lblCost.Location = new System.Drawing.Point(100,500);
this.lblCost.Name = "lblCost";
this.lblCost.Size = new System.Drawing.Size(41, 12);
this.lblCost.TabIndex = 20;
this.lblCost.Text = "";
//111======500
this.txtCost.Location = new System.Drawing.Point(173,496);
this.txtCost.Name ="txtCost";
this.txtCost.Size = new System.Drawing.Size(100, 21);
this.txtCost.TabIndex = 20;
this.Controls.Add(this.lblCost);
this.Controls.Add(this.txtCost);

           //#####SubtotalCostAmount###Decimal
this.lblSubtotalCostAmount.AutoSize = true;
this.lblSubtotalCostAmount.Location = new System.Drawing.Point(100,525);
this.lblSubtotalCostAmount.Name = "lblSubtotalCostAmount";
this.lblSubtotalCostAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalCostAmount.TabIndex = 21;
this.lblSubtotalCostAmount.Text = "";
//111======525
this.txtSubtotalCostAmount.Location = new System.Drawing.Point(173,521);
this.txtSubtotalCostAmount.Name ="txtSubtotalCostAmount";
this.txtSubtotalCostAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalCostAmount.TabIndex = 21;
this.Controls.Add(this.lblSubtotalCostAmount);
this.Controls.Add(this.txtSubtotalCostAmount);

           //#####TaxRate###Decimal
this.lblTaxRate.AutoSize = true;
this.lblTaxRate.Location = new System.Drawing.Point(100,550);
this.lblTaxRate.Name = "lblTaxRate";
this.lblTaxRate.Size = new System.Drawing.Size(41, 12);
this.lblTaxRate.TabIndex = 22;
this.lblTaxRate.Text = "";
//111======550
this.txtTaxRate.Location = new System.Drawing.Point(173,546);
this.txtTaxRate.Name ="txtTaxRate";
this.txtTaxRate.Size = new System.Drawing.Size(100, 21);
this.txtTaxRate.TabIndex = 22;
this.Controls.Add(this.lblTaxRate);
this.Controls.Add(this.txtTaxRate);

           //#####TotalReturnedQty###Int32

           //#####Gift###Boolean
this.lblGift.AutoSize = true;
this.lblGift.Location = new System.Drawing.Point(100,600);
this.lblGift.Name = "lblGift";
this.lblGift.Size = new System.Drawing.Size(41, 12);
this.lblGift.TabIndex = 24;
this.lblGift.Text = "";
this.chkGift.Location = new System.Drawing.Point(173,596);
this.chkGift.Name = "chkGift";
this.chkGift.Size = new System.Drawing.Size(100, 21);
this.chkGift.TabIndex = 24;
this.Controls.Add(this.lblGift);
this.Controls.Add(this.chkGift);

           //#####UnitPrice###Decimal
this.lblUnitPrice.AutoSize = true;
this.lblUnitPrice.Location = new System.Drawing.Point(100,625);
this.lblUnitPrice.Name = "lblUnitPrice";
this.lblUnitPrice.Size = new System.Drawing.Size(41, 12);
this.lblUnitPrice.TabIndex = 25;
this.lblUnitPrice.Text = "";
//111======625
this.txtUnitPrice.Location = new System.Drawing.Point(173,621);
this.txtUnitPrice.Name ="txtUnitPrice";
this.txtUnitPrice.Size = new System.Drawing.Size(100, 21);
this.txtUnitPrice.TabIndex = 25;
this.Controls.Add(this.lblUnitPrice);
this.Controls.Add(this.txtUnitPrice);

           //#####Discount###Decimal
this.lblDiscount.AutoSize = true;
this.lblDiscount.Location = new System.Drawing.Point(100,650);
this.lblDiscount.Name = "lblDiscount";
this.lblDiscount.Size = new System.Drawing.Size(41, 12);
this.lblDiscount.TabIndex = 26;
this.lblDiscount.Text = "";
//111======650
this.txtDiscount.Location = new System.Drawing.Point(173,646);
this.txtDiscount.Name ="txtDiscount";
this.txtDiscount.Size = new System.Drawing.Size(100, 21);
this.txtDiscount.TabIndex = 26;
this.Controls.Add(this.lblDiscount);
this.Controls.Add(this.txtDiscount);

           //#####CommissionAmount###Decimal
this.lblCommissionAmount.AutoSize = true;
this.lblCommissionAmount.Location = new System.Drawing.Point(100,675);
this.lblCommissionAmount.Name = "lblCommissionAmount";
this.lblCommissionAmount.Size = new System.Drawing.Size(41, 12);
this.lblCommissionAmount.TabIndex = 27;
this.lblCommissionAmount.Text = "";
//111======675
this.txtCommissionAmount.Location = new System.Drawing.Point(173,671);
this.txtCommissionAmount.Name ="txtCommissionAmount";
this.txtCommissionAmount.Size = new System.Drawing.Size(100, 21);
this.txtCommissionAmount.TabIndex = 27;
this.Controls.Add(this.lblCommissionAmount);
this.Controls.Add(this.txtCommissionAmount);

           //#####SubtotalTaxAmount###Decimal
this.lblSubtotalTaxAmount.AutoSize = true;
this.lblSubtotalTaxAmount.Location = new System.Drawing.Point(100,700);
this.lblSubtotalTaxAmount.Name = "lblSubtotalTaxAmount";
this.lblSubtotalTaxAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalTaxAmount.TabIndex = 28;
this.lblSubtotalTaxAmount.Text = "";
//111======700
this.txtSubtotalTaxAmount.Location = new System.Drawing.Point(173,696);
this.txtSubtotalTaxAmount.Name ="txtSubtotalTaxAmount";
this.txtSubtotalTaxAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalTaxAmount.TabIndex = 28;
this.Controls.Add(this.lblSubtotalTaxAmount);
this.Controls.Add(this.txtSubtotalTaxAmount);

           //#####255CNName###String
this.lblCNName.AutoSize = true;
this.lblCNName.Location = new System.Drawing.Point(100,725);
this.lblCNName.Name = "lblCNName";
this.lblCNName.Size = new System.Drawing.Size(41, 12);
this.lblCNName.TabIndex = 29;
this.lblCNName.Text = "";
this.txtCNName.Location = new System.Drawing.Point(173,721);
this.txtCNName.Name = "txtCNName";
this.txtCNName.Size = new System.Drawing.Size(100, 21);
this.txtCNName.TabIndex = 29;
this.Controls.Add(this.lblCNName);
this.Controls.Add(this.txtCNName);

           //#####1000Specifications###String
this.lblSpecifications.AutoSize = true;
this.lblSpecifications.Location = new System.Drawing.Point(100,750);
this.lblSpecifications.Name = "lblSpecifications";
this.lblSpecifications.Size = new System.Drawing.Size(41, 12);
this.lblSpecifications.TabIndex = 30;
this.lblSpecifications.Text = "";
this.txtSpecifications.Location = new System.Drawing.Point(173,746);
this.txtSpecifications.Name = "txtSpecifications";
this.txtSpecifications.Size = new System.Drawing.Size(100, 21);
this.txtSpecifications.TabIndex = 30;
this.Controls.Add(this.lblSpecifications);
this.Controls.Add(this.txtSpecifications);

           //#####40ProductNo###String
this.lblProductNo.AutoSize = true;
this.lblProductNo.Location = new System.Drawing.Point(100,775);
this.lblProductNo.Name = "lblProductNo";
this.lblProductNo.Size = new System.Drawing.Size(41, 12);
this.lblProductNo.TabIndex = 31;
this.lblProductNo.Text = "";
this.txtProductNo.Location = new System.Drawing.Point(173,771);
this.txtProductNo.Name = "txtProductNo";
this.txtProductNo.Size = new System.Drawing.Size(100, 21);
this.txtProductNo.TabIndex = 31;
this.Controls.Add(this.lblProductNo);
this.Controls.Add(this.txtProductNo);

           //#####Unit_ID###Int64

           //#####50Model###String
this.lblModel.AutoSize = true;
this.lblModel.Location = new System.Drawing.Point(100,825);
this.lblModel.Name = "lblModel";
this.lblModel.Size = new System.Drawing.Size(41, 12);
this.lblModel.TabIndex = 33;
this.lblModel.Text = "";
this.txtModel.Location = new System.Drawing.Point(173,821);
this.txtModel.Name = "txtModel";
this.txtModel.Size = new System.Drawing.Size(100, 21);
this.txtModel.TabIndex = 33;
this.Controls.Add(this.lblModel);
this.Controls.Add(this.txtModel);

           //#####DepartmentID###Int64

           //#####Category_ID###Int64

           //#####GrossProfit###Decimal
this.lblGrossProfit.AutoSize = true;
this.lblGrossProfit.Location = new System.Drawing.Point(100,900);
this.lblGrossProfit.Name = "lblGrossProfit";
this.lblGrossProfit.Size = new System.Drawing.Size(41, 12);
this.lblGrossProfit.TabIndex = 36;
this.lblGrossProfit.Text = "";
//111======900
this.txtGrossProfit.Location = new System.Drawing.Point(173,896);
this.txtGrossProfit.Name ="txtGrossProfit";
this.txtGrossProfit.Size = new System.Drawing.Size(100, 21);
this.txtGrossProfit.TabIndex = 36;
this.Controls.Add(this.lblGrossProfit);
this.Controls.Add(this.txtGrossProfit);

           //#####GrossProfitRatio###Decimal
this.lblGrossProfitRatio.AutoSize = true;
this.lblGrossProfitRatio.Location = new System.Drawing.Point(100,925);
this.lblGrossProfitRatio.Name = "lblGrossProfitRatio";
this.lblGrossProfitRatio.Size = new System.Drawing.Size(41, 12);
this.lblGrossProfitRatio.TabIndex = 37;
this.lblGrossProfitRatio.Text = "";
//111======925
this.txtGrossProfitRatio.Location = new System.Drawing.Point(173,921);
this.txtGrossProfitRatio.Name ="txtGrossProfitRatio";
this.txtGrossProfitRatio.Size = new System.Drawing.Size(100, 21);
this.txtGrossProfitRatio.TabIndex = 37;
this.Controls.Add(this.lblGrossProfitRatio);
this.Controls.Add(this.txtGrossProfitRatio);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblSKU );
this.Controls.Add(this.txtSKU );

                
                
                this.Controls.Add(this.lblSaleOutNo );
this.Controls.Add(this.txtSaleOutNo );

                
                
                
                this.Controls.Add(this.lblOutDate );
this.Controls.Add(this.dtpOutDate );

                this.Controls.Add(this.lblDeliveryDate );
this.Controls.Add(this.dtpDeliveryDate );

                this.Controls.Add(this.lblSaleOrderNo );
this.Controls.Add(this.txtSaleOrderNo );

                
                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                
                
                
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

                this.Controls.Add(this.lblUnitPrice );
this.Controls.Add(this.txtUnitPrice );

                this.Controls.Add(this.lblDiscount );
this.Controls.Add(this.txtDiscount );

                this.Controls.Add(this.lblCommissionAmount );
this.Controls.Add(this.txtCommissionAmount );

                this.Controls.Add(this.lblSubtotalTaxAmount );
this.Controls.Add(this.txtSubtotalTaxAmount );

                this.Controls.Add(this.lblCNName );
this.Controls.Add(this.txtCNName );

                this.Controls.Add(this.lblSpecifications );
this.Controls.Add(this.txtSpecifications );

                this.Controls.Add(this.lblProductNo );
this.Controls.Add(this.txtProductNo );

                
                this.Controls.Add(this.lblModel );
this.Controls.Add(this.txtModel );

                
                
                this.Controls.Add(this.lblGrossProfit );
this.Controls.Add(this.txtGrossProfit );

                this.Controls.Add(this.lblGrossProfitRatio );
this.Controls.Add(this.txtGrossProfitRatio );

                    
            this.Name = "View_SaleOutItemsQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSKU;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSKU;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSaleOutNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSaleOutNo;

    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblOutDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpOutDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDeliveryDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpDeliveryDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSaleOrderNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSaleOrderNo;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblproperty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTransactionPrice;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTransactionPrice;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalTransAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalTransAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerPartNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCustomerPartNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalCostAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalCostAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTaxRate;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTaxRate;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblGift;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkGift;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUnitPrice;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUnitPrice;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDiscount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDiscount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCommissionAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCommissionAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalTaxAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalTaxAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCNName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCNName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSpecifications;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSpecifications;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProductNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtProductNo;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModel;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtModel;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblGrossProfit;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtGrossProfit;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblGrossProfitRatio;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtGrossProfitRatio;

    
    
   
 





    }
}


