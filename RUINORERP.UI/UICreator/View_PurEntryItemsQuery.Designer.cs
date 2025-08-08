
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:36
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 采购入库统计
    /// </summary>
    partial class View_PurEntryItemsQuery
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
     
     this.lblPurOrder_NO = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPurOrder_NO = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPurEntryNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPurEntryNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();





this.lblEntryDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpEntryDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblShipCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtShipCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;



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

this.lblUntaxedUnitPrice = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUntaxedUnitPrice = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTaxRate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTaxRate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblIsGift = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsGift = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsGift.Values.Text ="";

this.lblCustomertModel = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCustomertModel = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblSummary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSummary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;




this.lblApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";

this.lblApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalQty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalQty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();



this.lblUnitName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUnitName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtUnitName.Multiline = true;

this.lblIsIncludeTax = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsIncludeTax = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsIncludeTax.Values.Text ="";

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####50PurOrder_NO###String
this.lblPurOrder_NO.AutoSize = true;
this.lblPurOrder_NO.Location = new System.Drawing.Point(100,25);
this.lblPurOrder_NO.Name = "lblPurOrder_NO";
this.lblPurOrder_NO.Size = new System.Drawing.Size(41, 12);
this.lblPurOrder_NO.TabIndex = 1;
this.lblPurOrder_NO.Text = "";
this.txtPurOrder_NO.Location = new System.Drawing.Point(173,21);
this.txtPurOrder_NO.Name = "txtPurOrder_NO";
this.txtPurOrder_NO.Size = new System.Drawing.Size(100, 21);
this.txtPurOrder_NO.TabIndex = 1;
this.Controls.Add(this.lblPurOrder_NO);
this.Controls.Add(this.txtPurOrder_NO);

           //#####50PurEntryNo###String
this.lblPurEntryNo.AutoSize = true;
this.lblPurEntryNo.Location = new System.Drawing.Point(100,50);
this.lblPurEntryNo.Name = "lblPurEntryNo";
this.lblPurEntryNo.Size = new System.Drawing.Size(41, 12);
this.lblPurEntryNo.TabIndex = 2;
this.lblPurEntryNo.Text = "";
this.txtPurEntryNo.Location = new System.Drawing.Point(173,46);
this.txtPurEntryNo.Name = "txtPurEntryNo";
this.txtPurEntryNo.Size = new System.Drawing.Size(100, 21);
this.txtPurEntryNo.TabIndex = 2;
this.Controls.Add(this.lblPurEntryNo);
this.Controls.Add(this.txtPurEntryNo);

           //#####CustomerVendor_ID###Int64

           //#####Employee_ID###Int64

           //#####DepartmentID###Int64

           //#####Paytype_ID###Int64

           //#####EntryDate###DateTime
this.lblEntryDate.AutoSize = true;
this.lblEntryDate.Location = new System.Drawing.Point(100,175);
this.lblEntryDate.Name = "lblEntryDate";
this.lblEntryDate.Size = new System.Drawing.Size(41, 12);
this.lblEntryDate.TabIndex = 7;
this.lblEntryDate.Text = "";
//111======175
this.dtpEntryDate.Location = new System.Drawing.Point(173,171);
this.dtpEntryDate.Name ="dtpEntryDate";
this.dtpEntryDate.ShowCheckBox =true;
this.dtpEntryDate.Size = new System.Drawing.Size(100, 21);
this.dtpEntryDate.TabIndex = 7;
this.Controls.Add(this.lblEntryDate);
this.Controls.Add(this.dtpEntryDate);

           //#####ShipCost###Decimal
this.lblShipCost.AutoSize = true;
this.lblShipCost.Location = new System.Drawing.Point(100,200);
this.lblShipCost.Name = "lblShipCost";
this.lblShipCost.Size = new System.Drawing.Size(41, 12);
this.lblShipCost.TabIndex = 8;
this.lblShipCost.Text = "";
//111======200
this.txtShipCost.Location = new System.Drawing.Point(173,196);
this.txtShipCost.Name ="txtShipCost";
this.txtShipCost.Size = new System.Drawing.Size(100, 21);
this.txtShipCost.TabIndex = 8;
this.Controls.Add(this.lblShipCost);
this.Controls.Add(this.txtShipCost);

           //#####1500Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,225);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 9;
this.lblNotes.Text = "";
this.txtNotes.Location = new System.Drawing.Point(173,221);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 9;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####TaxDeductionType###Int32

           //#####ProdDetailID###Int64

           //#####80SKU###String
this.lblSKU.AutoSize = true;
this.lblSKU.Location = new System.Drawing.Point(100,300);
this.lblSKU.Name = "lblSKU";
this.lblSKU.Size = new System.Drawing.Size(41, 12);
this.lblSKU.TabIndex = 12;
this.lblSKU.Text = "";
this.txtSKU.Location = new System.Drawing.Point(173,296);
this.txtSKU.Name = "txtSKU";
this.txtSKU.Size = new System.Drawing.Size(100, 21);
this.txtSKU.TabIndex = 12;
this.Controls.Add(this.lblSKU);
this.Controls.Add(this.txtSKU);

           //#####1000Specifications###String
this.lblSpecifications.AutoSize = true;
this.lblSpecifications.Location = new System.Drawing.Point(100,325);
this.lblSpecifications.Name = "lblSpecifications";
this.lblSpecifications.Size = new System.Drawing.Size(41, 12);
this.lblSpecifications.TabIndex = 13;
this.lblSpecifications.Text = "";
this.txtSpecifications.Location = new System.Drawing.Point(173,321);
this.txtSpecifications.Name = "txtSpecifications";
this.txtSpecifications.Size = new System.Drawing.Size(100, 21);
this.txtSpecifications.TabIndex = 13;
this.Controls.Add(this.lblSpecifications);
this.Controls.Add(this.txtSpecifications);

           //#####255CNName###String
this.lblCNName.AutoSize = true;
this.lblCNName.Location = new System.Drawing.Point(100,350);
this.lblCNName.Name = "lblCNName";
this.lblCNName.Size = new System.Drawing.Size(41, 12);
this.lblCNName.TabIndex = 14;
this.lblCNName.Text = "";
this.txtCNName.Location = new System.Drawing.Point(173,346);
this.txtCNName.Name = "txtCNName";
this.txtCNName.Size = new System.Drawing.Size(100, 21);
this.txtCNName.TabIndex = 14;
this.Controls.Add(this.lblCNName);
this.Controls.Add(this.txtCNName);

           //#####50Model###String
this.lblModel.AutoSize = true;
this.lblModel.Location = new System.Drawing.Point(100,375);
this.lblModel.Name = "lblModel";
this.lblModel.Size = new System.Drawing.Size(41, 12);
this.lblModel.TabIndex = 15;
this.lblModel.Text = "";
this.txtModel.Location = new System.Drawing.Point(173,371);
this.txtModel.Name = "txtModel";
this.txtModel.Size = new System.Drawing.Size(100, 21);
this.txtModel.TabIndex = 15;
this.Controls.Add(this.lblModel);
this.Controls.Add(this.txtModel);

           //#####Type_ID###Int64

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,425);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 17;
this.lblproperty.Text = "";
this.txtproperty.Location = new System.Drawing.Point(173,421);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 17;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

           //#####Location_ID###Int64

           //#####Quantity###Int32

           //#####UnitPrice###Decimal
this.lblUnitPrice.AutoSize = true;
this.lblUnitPrice.Location = new System.Drawing.Point(100,500);
this.lblUnitPrice.Name = "lblUnitPrice";
this.lblUnitPrice.Size = new System.Drawing.Size(41, 12);
this.lblUnitPrice.TabIndex = 20;
this.lblUnitPrice.Text = "";
//111======500
this.txtUnitPrice.Location = new System.Drawing.Point(173,496);
this.txtUnitPrice.Name ="txtUnitPrice";
this.txtUnitPrice.Size = new System.Drawing.Size(100, 21);
this.txtUnitPrice.TabIndex = 20;
this.Controls.Add(this.lblUnitPrice);
this.Controls.Add(this.txtUnitPrice);

           //#####UntaxedUnitPrice###Decimal
this.lblUntaxedUnitPrice.AutoSize = true;
this.lblUntaxedUnitPrice.Location = new System.Drawing.Point(100,525);
this.lblUntaxedUnitPrice.Name = "lblUntaxedUnitPrice";
this.lblUntaxedUnitPrice.Size = new System.Drawing.Size(41, 12);
this.lblUntaxedUnitPrice.TabIndex = 21;
this.lblUntaxedUnitPrice.Text = "";
//111======525
this.txtUntaxedUnitPrice.Location = new System.Drawing.Point(173,521);
this.txtUntaxedUnitPrice.Name ="txtUntaxedUnitPrice";
this.txtUntaxedUnitPrice.Size = new System.Drawing.Size(100, 21);
this.txtUntaxedUnitPrice.TabIndex = 21;
this.Controls.Add(this.lblUntaxedUnitPrice);
this.Controls.Add(this.txtUntaxedUnitPrice);

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

           //#####TaxAmount###Decimal
this.lblTaxAmount.AutoSize = true;
this.lblTaxAmount.Location = new System.Drawing.Point(100,575);
this.lblTaxAmount.Name = "lblTaxAmount";
this.lblTaxAmount.Size = new System.Drawing.Size(41, 12);
this.lblTaxAmount.TabIndex = 23;
this.lblTaxAmount.Text = "";
//111======575
this.txtTaxAmount.Location = new System.Drawing.Point(173,571);
this.txtTaxAmount.Name ="txtTaxAmount";
this.txtTaxAmount.Size = new System.Drawing.Size(100, 21);
this.txtTaxAmount.TabIndex = 23;
this.Controls.Add(this.lblTaxAmount);
this.Controls.Add(this.txtTaxAmount);

           //#####SubtotalAmount###Decimal
this.lblSubtotalAmount.AutoSize = true;
this.lblSubtotalAmount.Location = new System.Drawing.Point(100,600);
this.lblSubtotalAmount.Name = "lblSubtotalAmount";
this.lblSubtotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalAmount.TabIndex = 24;
this.lblSubtotalAmount.Text = "";
//111======600
this.txtSubtotalAmount.Location = new System.Drawing.Point(173,596);
this.txtSubtotalAmount.Name ="txtSubtotalAmount";
this.txtSubtotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalAmount.TabIndex = 24;
this.Controls.Add(this.lblSubtotalAmount);
this.Controls.Add(this.txtSubtotalAmount);

           //#####IsGift###Boolean
this.lblIsGift.AutoSize = true;
this.lblIsGift.Location = new System.Drawing.Point(100,625);
this.lblIsGift.Name = "lblIsGift";
this.lblIsGift.Size = new System.Drawing.Size(41, 12);
this.lblIsGift.TabIndex = 25;
this.lblIsGift.Text = "";
this.chkIsGift.Location = new System.Drawing.Point(173,621);
this.chkIsGift.Name = "chkIsGift";
this.chkIsGift.Size = new System.Drawing.Size(100, 21);
this.chkIsGift.TabIndex = 25;
this.Controls.Add(this.lblIsGift);
this.Controls.Add(this.chkIsGift);

           //#####50CustomertModel###String
this.lblCustomertModel.AutoSize = true;
this.lblCustomertModel.Location = new System.Drawing.Point(100,650);
this.lblCustomertModel.Name = "lblCustomertModel";
this.lblCustomertModel.Size = new System.Drawing.Size(41, 12);
this.lblCustomertModel.TabIndex = 26;
this.lblCustomertModel.Text = "";
this.txtCustomertModel.Location = new System.Drawing.Point(173,646);
this.txtCustomertModel.Name = "txtCustomertModel";
this.txtCustomertModel.Size = new System.Drawing.Size(100, 21);
this.txtCustomertModel.TabIndex = 26;
this.Controls.Add(this.lblCustomertModel);
this.Controls.Add(this.txtCustomertModel);

           //#####ReturnedQty###Int32

           //#####1000Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,700);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 28;
this.lblSummary.Text = "";
this.txtSummary.Location = new System.Drawing.Point(173,696);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 28;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####PrintStatus###Int32

           //#####DataStatus###Int32

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,800);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 32;
this.lblApprovalResults.Text = "";
this.chkApprovalResults.Location = new System.Drawing.Point(173,796);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 32;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####200ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,825);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 33;
this.lblApprovalOpinions.Text = "";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,821);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 33;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####TotalAmount###Decimal
this.lblTotalAmount.AutoSize = true;
this.lblTotalAmount.Location = new System.Drawing.Point(100,850);
this.lblTotalAmount.Name = "lblTotalAmount";
this.lblTotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalAmount.TabIndex = 34;
this.lblTotalAmount.Text = "";
//111======850
this.txtTotalAmount.Location = new System.Drawing.Point(173,846);
this.txtTotalAmount.Name ="txtTotalAmount";
this.txtTotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalAmount.TabIndex = 34;
this.Controls.Add(this.lblTotalAmount);
this.Controls.Add(this.txtTotalAmount);

           //#####TotalTaxAmount###Decimal
this.lblTotalTaxAmount.AutoSize = true;
this.lblTotalTaxAmount.Location = new System.Drawing.Point(100,875);
this.lblTotalTaxAmount.Name = "lblTotalTaxAmount";
this.lblTotalTaxAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalTaxAmount.TabIndex = 35;
this.lblTotalTaxAmount.Text = "";
//111======875
this.txtTotalTaxAmount.Location = new System.Drawing.Point(173,871);
this.txtTotalTaxAmount.Name ="txtTotalTaxAmount";
this.txtTotalTaxAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalTaxAmount.TabIndex = 35;
this.Controls.Add(this.lblTotalTaxAmount);
this.Controls.Add(this.txtTotalTaxAmount);

           //#####TotalQty###Decimal
this.lblTotalQty.AutoSize = true;
this.lblTotalQty.Location = new System.Drawing.Point(100,900);
this.lblTotalQty.Name = "lblTotalQty";
this.lblTotalQty.Size = new System.Drawing.Size(41, 12);
this.lblTotalQty.TabIndex = 36;
this.lblTotalQty.Text = "";
//111======900
this.txtTotalQty.Location = new System.Drawing.Point(173,896);
this.txtTotalQty.Name ="txtTotalQty";
this.txtTotalQty.Size = new System.Drawing.Size(100, 21);
this.txtTotalQty.TabIndex = 36;
this.Controls.Add(this.lblTotalQty);
this.Controls.Add(this.txtTotalQty);

           //#####Category_ID###Int64

           //#####Rack_ID###Int64

           //#####255UnitName###String
this.lblUnitName.AutoSize = true;
this.lblUnitName.Location = new System.Drawing.Point(100,975);
this.lblUnitName.Name = "lblUnitName";
this.lblUnitName.Size = new System.Drawing.Size(41, 12);
this.lblUnitName.TabIndex = 39;
this.lblUnitName.Text = "";
this.txtUnitName.Location = new System.Drawing.Point(173,971);
this.txtUnitName.Name = "txtUnitName";
this.txtUnitName.Size = new System.Drawing.Size(100, 21);
this.txtUnitName.TabIndex = 39;
this.Controls.Add(this.lblUnitName);
this.Controls.Add(this.txtUnitName);

           //#####IsIncludeTax###Boolean
this.lblIsIncludeTax.AutoSize = true;
this.lblIsIncludeTax.Location = new System.Drawing.Point(100,1000);
this.lblIsIncludeTax.Name = "lblIsIncludeTax";
this.lblIsIncludeTax.Size = new System.Drawing.Size(41, 12);
this.lblIsIncludeTax.TabIndex = 40;
this.lblIsIncludeTax.Text = "";
this.chkIsIncludeTax.Location = new System.Drawing.Point(173,996);
this.chkIsIncludeTax.Name = "chkIsIncludeTax";
this.chkIsIncludeTax.Size = new System.Drawing.Size(100, 21);
this.chkIsIncludeTax.TabIndex = 40;
this.Controls.Add(this.lblIsIncludeTax);
this.Controls.Add(this.chkIsIncludeTax);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblPurOrder_NO );
this.Controls.Add(this.txtPurOrder_NO );

                this.Controls.Add(this.lblPurEntryNo );
this.Controls.Add(this.txtPurEntryNo );

                
                
                
                
                this.Controls.Add(this.lblEntryDate );
this.Controls.Add(this.dtpEntryDate );

                this.Controls.Add(this.lblShipCost );
this.Controls.Add(this.txtShipCost );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                
                
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

                this.Controls.Add(this.lblUntaxedUnitPrice );
this.Controls.Add(this.txtUntaxedUnitPrice );

                this.Controls.Add(this.lblTaxRate );
this.Controls.Add(this.txtTaxRate );

                this.Controls.Add(this.lblTaxAmount );
this.Controls.Add(this.txtTaxAmount );

                this.Controls.Add(this.lblSubtotalAmount );
this.Controls.Add(this.txtSubtotalAmount );

                this.Controls.Add(this.lblIsGift );
this.Controls.Add(this.chkIsGift );

                this.Controls.Add(this.lblCustomertModel );
this.Controls.Add(this.txtCustomertModel );

                
                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                
                
                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                this.Controls.Add(this.lblTotalAmount );
this.Controls.Add(this.txtTotalAmount );

                this.Controls.Add(this.lblTotalTaxAmount );
this.Controls.Add(this.txtTotalTaxAmount );

                this.Controls.Add(this.lblTotalQty );
this.Controls.Add(this.txtTotalQty );

                
                
                this.Controls.Add(this.lblUnitName );
this.Controls.Add(this.txtUnitName );

                this.Controls.Add(this.lblIsIncludeTax );
this.Controls.Add(this.chkIsIncludeTax );

                    
            this.Name = "View_PurEntryItemsQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPurOrder_NO;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPurOrder_NO;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPurEntryNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPurEntryNo;

    
        
              
    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEntryDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpEntryDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblShipCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtShipCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              
    
        
              
    
        
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

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUntaxedUnitPrice;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUntaxedUnitPrice;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTaxRate;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTaxRate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTaxAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTaxAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsGift;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsGift;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomertModel;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCustomertModel;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalResults;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalTaxAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalTaxAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalQty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalQty;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUnitName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUnitName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsIncludeTax;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsIncludeTax;

    
    
   
 





    }
}


