
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:37
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 采购订单统计
    /// </summary>
    partial class View_PurOrderItemsQuery
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
     
     
this.lblPurOrderNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPurOrderNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();







this.lblPurDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpPurDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblIsIncludeTax = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsIncludeTax = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsIncludeTax.Values.Text ="";

this.lblShipCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtShipCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblOrderPreDeliveryDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpOrderPreDeliveryDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblArrival_date = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpArrival_date = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblDeposit = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDeposit = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();



this.lblSKU = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSKU = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSpecifications = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSpecifications = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSpecifications.Multiline = true;

this.lblCNName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCNName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtCNName.Multiline = true;

this.lblModel = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtModel = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblproperty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtproperty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;



this.lblUnitPrice = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUnitPrice = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTaxRate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTaxRate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblIsGift = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsGift = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsGift.Values.Text ="";

this.lblItemPreDeliveryDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpItemPreDeliveryDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblCustomertModel = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCustomertModel = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblSummary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSummary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####PurOrder_ID###Int64

           //#####100PurOrderNo###String
this.lblPurOrderNo.AutoSize = true;
this.lblPurOrderNo.Location = new System.Drawing.Point(100,50);
this.lblPurOrderNo.Name = "lblPurOrderNo";
this.lblPurOrderNo.Size = new System.Drawing.Size(41, 12);
this.lblPurOrderNo.TabIndex = 2;
this.lblPurOrderNo.Text = "";
this.txtPurOrderNo.Location = new System.Drawing.Point(173,46);
this.txtPurOrderNo.Name = "txtPurOrderNo";
this.txtPurOrderNo.Size = new System.Drawing.Size(100, 21);
this.txtPurOrderNo.TabIndex = 2;
this.Controls.Add(this.lblPurOrderNo);
this.Controls.Add(this.txtPurOrderNo);

           //#####PDID###Int64

           //#####CustomerVendor_ID###Int64

           //#####Employee_ID###Int64

           //#####DepartmentID###Int64

           //#####Paytype_ID###Int64

           //#####SOrder_ID###Int64

           //#####PurDate###DateTime
this.lblPurDate.AutoSize = true;
this.lblPurDate.Location = new System.Drawing.Point(100,225);
this.lblPurDate.Name = "lblPurDate";
this.lblPurDate.Size = new System.Drawing.Size(41, 12);
this.lblPurDate.TabIndex = 9;
this.lblPurDate.Text = "";
//111======225
this.dtpPurDate.Location = new System.Drawing.Point(173,221);
this.dtpPurDate.Name ="dtpPurDate";
this.dtpPurDate.ShowCheckBox =true;
this.dtpPurDate.Size = new System.Drawing.Size(100, 21);
this.dtpPurDate.TabIndex = 9;
this.Controls.Add(this.lblPurDate);
this.Controls.Add(this.dtpPurDate);

           //#####IsIncludeTax###Boolean
this.lblIsIncludeTax.AutoSize = true;
this.lblIsIncludeTax.Location = new System.Drawing.Point(100,250);
this.lblIsIncludeTax.Name = "lblIsIncludeTax";
this.lblIsIncludeTax.Size = new System.Drawing.Size(41, 12);
this.lblIsIncludeTax.TabIndex = 10;
this.lblIsIncludeTax.Text = "";
this.chkIsIncludeTax.Location = new System.Drawing.Point(173,246);
this.chkIsIncludeTax.Name = "chkIsIncludeTax";
this.chkIsIncludeTax.Size = new System.Drawing.Size(100, 21);
this.chkIsIncludeTax.TabIndex = 10;
this.Controls.Add(this.lblIsIncludeTax);
this.Controls.Add(this.chkIsIncludeTax);

           //#####ShipCost###Decimal
this.lblShipCost.AutoSize = true;
this.lblShipCost.Location = new System.Drawing.Point(100,275);
this.lblShipCost.Name = "lblShipCost";
this.lblShipCost.Size = new System.Drawing.Size(41, 12);
this.lblShipCost.TabIndex = 11;
this.lblShipCost.Text = "";
//111======275
this.txtShipCost.Location = new System.Drawing.Point(173,271);
this.txtShipCost.Name ="txtShipCost";
this.txtShipCost.Size = new System.Drawing.Size(100, 21);
this.txtShipCost.TabIndex = 11;
this.Controls.Add(this.lblShipCost);
this.Controls.Add(this.txtShipCost);

           //#####OrderPreDeliveryDate###DateTime
this.lblOrderPreDeliveryDate.AutoSize = true;
this.lblOrderPreDeliveryDate.Location = new System.Drawing.Point(100,300);
this.lblOrderPreDeliveryDate.Name = "lblOrderPreDeliveryDate";
this.lblOrderPreDeliveryDate.Size = new System.Drawing.Size(41, 12);
this.lblOrderPreDeliveryDate.TabIndex = 12;
this.lblOrderPreDeliveryDate.Text = "";
//111======300
this.dtpOrderPreDeliveryDate.Location = new System.Drawing.Point(173,296);
this.dtpOrderPreDeliveryDate.Name ="dtpOrderPreDeliveryDate";
this.dtpOrderPreDeliveryDate.ShowCheckBox =true;
this.dtpOrderPreDeliveryDate.Size = new System.Drawing.Size(100, 21);
this.dtpOrderPreDeliveryDate.TabIndex = 12;
this.Controls.Add(this.lblOrderPreDeliveryDate);
this.Controls.Add(this.dtpOrderPreDeliveryDate);

           //#####1500Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,325);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 13;
this.lblNotes.Text = "";
this.txtNotes.Location = new System.Drawing.Point(173,321);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 13;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####Arrival_date###DateTime
this.lblArrival_date.AutoSize = true;
this.lblArrival_date.Location = new System.Drawing.Point(100,350);
this.lblArrival_date.Name = "lblArrival_date";
this.lblArrival_date.Size = new System.Drawing.Size(41, 12);
this.lblArrival_date.TabIndex = 14;
this.lblArrival_date.Text = "";
//111======350
this.dtpArrival_date.Location = new System.Drawing.Point(173,346);
this.dtpArrival_date.Name ="dtpArrival_date";
this.dtpArrival_date.ShowCheckBox =true;
this.dtpArrival_date.Size = new System.Drawing.Size(100, 21);
this.dtpArrival_date.TabIndex = 14;
this.Controls.Add(this.lblArrival_date);
this.Controls.Add(this.dtpArrival_date);

           //#####Deposit###Decimal
this.lblDeposit.AutoSize = true;
this.lblDeposit.Location = new System.Drawing.Point(100,375);
this.lblDeposit.Name = "lblDeposit";
this.lblDeposit.Size = new System.Drawing.Size(41, 12);
this.lblDeposit.TabIndex = 15;
this.lblDeposit.Text = "";
//111======375
this.txtDeposit.Location = new System.Drawing.Point(173,371);
this.txtDeposit.Name ="txtDeposit";
this.txtDeposit.Size = new System.Drawing.Size(100, 21);
this.txtDeposit.TabIndex = 15;
this.Controls.Add(this.lblDeposit);
this.Controls.Add(this.txtDeposit);

           //#####TaxDeductionType###Int32

           //#####ProdDetailID###Int64

           //#####80SKU###String
this.lblSKU.AutoSize = true;
this.lblSKU.Location = new System.Drawing.Point(100,450);
this.lblSKU.Name = "lblSKU";
this.lblSKU.Size = new System.Drawing.Size(41, 12);
this.lblSKU.TabIndex = 18;
this.lblSKU.Text = "";
this.txtSKU.Location = new System.Drawing.Point(173,446);
this.txtSKU.Name = "txtSKU";
this.txtSKU.Size = new System.Drawing.Size(100, 21);
this.txtSKU.TabIndex = 18;
this.Controls.Add(this.lblSKU);
this.Controls.Add(this.txtSKU);

           //#####1000Specifications###String
this.lblSpecifications.AutoSize = true;
this.lblSpecifications.Location = new System.Drawing.Point(100,475);
this.lblSpecifications.Name = "lblSpecifications";
this.lblSpecifications.Size = new System.Drawing.Size(41, 12);
this.lblSpecifications.TabIndex = 19;
this.lblSpecifications.Text = "";
this.txtSpecifications.Location = new System.Drawing.Point(173,471);
this.txtSpecifications.Name = "txtSpecifications";
this.txtSpecifications.Size = new System.Drawing.Size(100, 21);
this.txtSpecifications.TabIndex = 19;
this.Controls.Add(this.lblSpecifications);
this.Controls.Add(this.txtSpecifications);

           //#####255CNName###String
this.lblCNName.AutoSize = true;
this.lblCNName.Location = new System.Drawing.Point(100,500);
this.lblCNName.Name = "lblCNName";
this.lblCNName.Size = new System.Drawing.Size(41, 12);
this.lblCNName.TabIndex = 20;
this.lblCNName.Text = "";
this.txtCNName.Location = new System.Drawing.Point(173,496);
this.txtCNName.Name = "txtCNName";
this.txtCNName.Size = new System.Drawing.Size(100, 21);
this.txtCNName.TabIndex = 20;
this.Controls.Add(this.lblCNName);
this.Controls.Add(this.txtCNName);

           //#####50Model###String
this.lblModel.AutoSize = true;
this.lblModel.Location = new System.Drawing.Point(100,525);
this.lblModel.Name = "lblModel";
this.lblModel.Size = new System.Drawing.Size(41, 12);
this.lblModel.TabIndex = 21;
this.lblModel.Text = "";
this.txtModel.Location = new System.Drawing.Point(173,521);
this.txtModel.Name = "txtModel";
this.txtModel.Size = new System.Drawing.Size(100, 21);
this.txtModel.TabIndex = 21;
this.Controls.Add(this.lblModel);
this.Controls.Add(this.txtModel);

           //#####Type_ID###Int64

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,575);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 23;
this.lblproperty.Text = "";
this.txtproperty.Location = new System.Drawing.Point(173,571);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 23;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

           //#####Location_ID###Int64

           //#####Quantity###Int32

           //#####UnitPrice###Decimal
this.lblUnitPrice.AutoSize = true;
this.lblUnitPrice.Location = new System.Drawing.Point(100,650);
this.lblUnitPrice.Name = "lblUnitPrice";
this.lblUnitPrice.Size = new System.Drawing.Size(41, 12);
this.lblUnitPrice.TabIndex = 26;
this.lblUnitPrice.Text = "";
//111======650
this.txtUnitPrice.Location = new System.Drawing.Point(173,646);
this.txtUnitPrice.Name ="txtUnitPrice";
this.txtUnitPrice.Size = new System.Drawing.Size(100, 21);
this.txtUnitPrice.TabIndex = 26;
this.Controls.Add(this.lblUnitPrice);
this.Controls.Add(this.txtUnitPrice);

           //#####TaxRate###Decimal
this.lblTaxRate.AutoSize = true;
this.lblTaxRate.Location = new System.Drawing.Point(100,675);
this.lblTaxRate.Name = "lblTaxRate";
this.lblTaxRate.Size = new System.Drawing.Size(41, 12);
this.lblTaxRate.TabIndex = 27;
this.lblTaxRate.Text = "";
//111======675
this.txtTaxRate.Location = new System.Drawing.Point(173,671);
this.txtTaxRate.Name ="txtTaxRate";
this.txtTaxRate.Size = new System.Drawing.Size(100, 21);
this.txtTaxRate.TabIndex = 27;
this.Controls.Add(this.lblTaxRate);
this.Controls.Add(this.txtTaxRate);

           //#####TaxAmount###Decimal
this.lblTaxAmount.AutoSize = true;
this.lblTaxAmount.Location = new System.Drawing.Point(100,700);
this.lblTaxAmount.Name = "lblTaxAmount";
this.lblTaxAmount.Size = new System.Drawing.Size(41, 12);
this.lblTaxAmount.TabIndex = 28;
this.lblTaxAmount.Text = "";
//111======700
this.txtTaxAmount.Location = new System.Drawing.Point(173,696);
this.txtTaxAmount.Name ="txtTaxAmount";
this.txtTaxAmount.Size = new System.Drawing.Size(100, 21);
this.txtTaxAmount.TabIndex = 28;
this.Controls.Add(this.lblTaxAmount);
this.Controls.Add(this.txtTaxAmount);

           //#####SubtotalAmount###Decimal
this.lblSubtotalAmount.AutoSize = true;
this.lblSubtotalAmount.Location = new System.Drawing.Point(100,725);
this.lblSubtotalAmount.Name = "lblSubtotalAmount";
this.lblSubtotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalAmount.TabIndex = 29;
this.lblSubtotalAmount.Text = "";
//111======725
this.txtSubtotalAmount.Location = new System.Drawing.Point(173,721);
this.txtSubtotalAmount.Name ="txtSubtotalAmount";
this.txtSubtotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalAmount.TabIndex = 29;
this.Controls.Add(this.lblSubtotalAmount);
this.Controls.Add(this.txtSubtotalAmount);

           //#####IsGift###Boolean
this.lblIsGift.AutoSize = true;
this.lblIsGift.Location = new System.Drawing.Point(100,750);
this.lblIsGift.Name = "lblIsGift";
this.lblIsGift.Size = new System.Drawing.Size(41, 12);
this.lblIsGift.TabIndex = 30;
this.lblIsGift.Text = "";
this.chkIsGift.Location = new System.Drawing.Point(173,746);
this.chkIsGift.Name = "chkIsGift";
this.chkIsGift.Size = new System.Drawing.Size(100, 21);
this.chkIsGift.TabIndex = 30;
this.Controls.Add(this.lblIsGift);
this.Controls.Add(this.chkIsGift);

           //#####ItemPreDeliveryDate###DateTime
this.lblItemPreDeliveryDate.AutoSize = true;
this.lblItemPreDeliveryDate.Location = new System.Drawing.Point(100,775);
this.lblItemPreDeliveryDate.Name = "lblItemPreDeliveryDate";
this.lblItemPreDeliveryDate.Size = new System.Drawing.Size(41, 12);
this.lblItemPreDeliveryDate.TabIndex = 31;
this.lblItemPreDeliveryDate.Text = "";
//111======775
this.dtpItemPreDeliveryDate.Location = new System.Drawing.Point(173,771);
this.dtpItemPreDeliveryDate.Name ="dtpItemPreDeliveryDate";
this.dtpItemPreDeliveryDate.Size = new System.Drawing.Size(100, 21);
this.dtpItemPreDeliveryDate.TabIndex = 31;
this.Controls.Add(this.lblItemPreDeliveryDate);
this.Controls.Add(this.dtpItemPreDeliveryDate);

           //#####50CustomertModel###String
this.lblCustomertModel.AutoSize = true;
this.lblCustomertModel.Location = new System.Drawing.Point(100,800);
this.lblCustomertModel.Name = "lblCustomertModel";
this.lblCustomertModel.Size = new System.Drawing.Size(41, 12);
this.lblCustomertModel.TabIndex = 32;
this.lblCustomertModel.Text = "";
this.txtCustomertModel.Location = new System.Drawing.Point(173,796);
this.txtCustomertModel.Name = "txtCustomertModel";
this.txtCustomertModel.Size = new System.Drawing.Size(100, 21);
this.txtCustomertModel.TabIndex = 32;
this.Controls.Add(this.lblCustomertModel);
this.Controls.Add(this.txtCustomertModel);

           //#####DeliveredQuantity###Int32

           //#####1000Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,850);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 34;
this.lblSummary.Text = "";
this.txtSummary.Location = new System.Drawing.Point(173,846);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 34;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                
                this.Controls.Add(this.lblPurOrderNo );
this.Controls.Add(this.txtPurOrderNo );

                
                
                
                
                
                
                this.Controls.Add(this.lblPurDate );
this.Controls.Add(this.dtpPurDate );

                this.Controls.Add(this.lblIsIncludeTax );
this.Controls.Add(this.chkIsIncludeTax );

                this.Controls.Add(this.lblShipCost );
this.Controls.Add(this.txtShipCost );

                this.Controls.Add(this.lblOrderPreDeliveryDate );
this.Controls.Add(this.dtpOrderPreDeliveryDate );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblArrival_date );
this.Controls.Add(this.dtpArrival_date );

                this.Controls.Add(this.lblDeposit );
this.Controls.Add(this.txtDeposit );

                
                
                this.Controls.Add(this.lblSKU );
this.Controls.Add(this.txtSKU );

                this.Controls.Add(this.lblSpecifications );
this.Controls.Add(this.txtSpecifications );

                this.Controls.Add(this.lblCNName );
this.Controls.Add(this.txtCNName );

                this.Controls.Add(this.lblModel );
this.Controls.Add(this.txtModel );

                
                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                
                
                this.Controls.Add(this.lblUnitPrice );
this.Controls.Add(this.txtUnitPrice );

                this.Controls.Add(this.lblTaxRate );
this.Controls.Add(this.txtTaxRate );

                this.Controls.Add(this.lblTaxAmount );
this.Controls.Add(this.txtTaxAmount );

                this.Controls.Add(this.lblSubtotalAmount );
this.Controls.Add(this.txtSubtotalAmount );

                this.Controls.Add(this.lblIsGift );
this.Controls.Add(this.chkIsGift );

                this.Controls.Add(this.lblItemPreDeliveryDate );
this.Controls.Add(this.dtpItemPreDeliveryDate );

                this.Controls.Add(this.lblCustomertModel );
this.Controls.Add(this.txtCustomertModel );

                
                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                    
            this.Name = "View_PurOrderItemsQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPurOrderNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPurOrderNo;

    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPurDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpPurDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsIncludeTax;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsIncludeTax;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblShipCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtShipCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblOrderPreDeliveryDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpOrderPreDeliveryDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblArrival_date;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpArrival_date;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDeposit;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDeposit;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSKU;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSKU;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSpecifications;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSpecifications;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCNName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCNName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModel;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtModel;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblproperty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUnitPrice;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUnitPrice;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTaxRate;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTaxRate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTaxAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTaxAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsGift;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsGift;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblItemPreDeliveryDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpItemPreDeliveryDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomertModel;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCustomertModel;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
    
   
 





    }
}


