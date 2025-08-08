
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
    /// 采购退货统计
    /// </summary>
    partial class View_PurEntryReItemsQuery
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
     
     this.lblPurEntryReNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPurEntryReNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPurEntryNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPurEntryNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();





this.lblReturnDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpReturnDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

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

this.lblTaxRate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTaxRate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalTrPriceAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalTrPriceAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

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




this.lblUnitName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUnitName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtUnitName.Multiline = true;

this.lblIsIncludeTax = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsIncludeTax = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsIncludeTax.Values.Text ="";

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####50PurEntryReNo###String
this.lblPurEntryReNo.AutoSize = true;
this.lblPurEntryReNo.Location = new System.Drawing.Point(100,25);
this.lblPurEntryReNo.Name = "lblPurEntryReNo";
this.lblPurEntryReNo.Size = new System.Drawing.Size(41, 12);
this.lblPurEntryReNo.TabIndex = 1;
this.lblPurEntryReNo.Text = "";
this.txtPurEntryReNo.Location = new System.Drawing.Point(173,21);
this.txtPurEntryReNo.Name = "txtPurEntryReNo";
this.txtPurEntryReNo.Size = new System.Drawing.Size(100, 21);
this.txtPurEntryReNo.TabIndex = 1;
this.Controls.Add(this.lblPurEntryReNo);
this.Controls.Add(this.txtPurEntryReNo);

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

           //#####255Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,200);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 8;
this.lblNotes.Text = "";
this.txtNotes.Location = new System.Drawing.Point(173,196);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 8;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####TaxDeductionType###Int32

           //#####ProdDetailID###Int64

           //#####80SKU###String
this.lblSKU.AutoSize = true;
this.lblSKU.Location = new System.Drawing.Point(100,275);
this.lblSKU.Name = "lblSKU";
this.lblSKU.Size = new System.Drawing.Size(41, 12);
this.lblSKU.TabIndex = 11;
this.lblSKU.Text = "";
this.txtSKU.Location = new System.Drawing.Point(173,271);
this.txtSKU.Name = "txtSKU";
this.txtSKU.Size = new System.Drawing.Size(100, 21);
this.txtSKU.TabIndex = 11;
this.Controls.Add(this.lblSKU);
this.Controls.Add(this.txtSKU);

           //#####1000Specifications###String
this.lblSpecifications.AutoSize = true;
this.lblSpecifications.Location = new System.Drawing.Point(100,300);
this.lblSpecifications.Name = "lblSpecifications";
this.lblSpecifications.Size = new System.Drawing.Size(41, 12);
this.lblSpecifications.TabIndex = 12;
this.lblSpecifications.Text = "";
this.txtSpecifications.Location = new System.Drawing.Point(173,296);
this.txtSpecifications.Name = "txtSpecifications";
this.txtSpecifications.Size = new System.Drawing.Size(100, 21);
this.txtSpecifications.TabIndex = 12;
this.Controls.Add(this.lblSpecifications);
this.Controls.Add(this.txtSpecifications);

           //#####255CNName###String
this.lblCNName.AutoSize = true;
this.lblCNName.Location = new System.Drawing.Point(100,325);
this.lblCNName.Name = "lblCNName";
this.lblCNName.Size = new System.Drawing.Size(41, 12);
this.lblCNName.TabIndex = 13;
this.lblCNName.Text = "";
this.txtCNName.Location = new System.Drawing.Point(173,321);
this.txtCNName.Name = "txtCNName";
this.txtCNName.Size = new System.Drawing.Size(100, 21);
this.txtCNName.TabIndex = 13;
this.Controls.Add(this.lblCNName);
this.Controls.Add(this.txtCNName);

           //#####50Model###String
this.lblModel.AutoSize = true;
this.lblModel.Location = new System.Drawing.Point(100,350);
this.lblModel.Name = "lblModel";
this.lblModel.Size = new System.Drawing.Size(41, 12);
this.lblModel.TabIndex = 14;
this.lblModel.Text = "";
this.txtModel.Location = new System.Drawing.Point(173,346);
this.txtModel.Name = "txtModel";
this.txtModel.Size = new System.Drawing.Size(100, 21);
this.txtModel.TabIndex = 14;
this.Controls.Add(this.lblModel);
this.Controls.Add(this.txtModel);

           //#####Type_ID###Int64

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,400);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 16;
this.lblproperty.Text = "";
this.txtproperty.Location = new System.Drawing.Point(173,396);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 16;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

           //#####Location_ID###Int64

           //#####Quantity###Int32

           //#####UnitPrice###Decimal
this.lblUnitPrice.AutoSize = true;
this.lblUnitPrice.Location = new System.Drawing.Point(100,475);
this.lblUnitPrice.Name = "lblUnitPrice";
this.lblUnitPrice.Size = new System.Drawing.Size(41, 12);
this.lblUnitPrice.TabIndex = 19;
this.lblUnitPrice.Text = "";
//111======475
this.txtUnitPrice.Location = new System.Drawing.Point(173,471);
this.txtUnitPrice.Name ="txtUnitPrice";
this.txtUnitPrice.Size = new System.Drawing.Size(100, 21);
this.txtUnitPrice.TabIndex = 19;
this.Controls.Add(this.lblUnitPrice);
this.Controls.Add(this.txtUnitPrice);

           //#####TaxRate###Decimal
this.lblTaxRate.AutoSize = true;
this.lblTaxRate.Location = new System.Drawing.Point(100,500);
this.lblTaxRate.Name = "lblTaxRate";
this.lblTaxRate.Size = new System.Drawing.Size(41, 12);
this.lblTaxRate.TabIndex = 20;
this.lblTaxRate.Text = "";
//111======500
this.txtTaxRate.Location = new System.Drawing.Point(173,496);
this.txtTaxRate.Name ="txtTaxRate";
this.txtTaxRate.Size = new System.Drawing.Size(100, 21);
this.txtTaxRate.TabIndex = 20;
this.Controls.Add(this.lblTaxRate);
this.Controls.Add(this.txtTaxRate);

           //#####TaxAmount###Decimal
this.lblTaxAmount.AutoSize = true;
this.lblTaxAmount.Location = new System.Drawing.Point(100,525);
this.lblTaxAmount.Name = "lblTaxAmount";
this.lblTaxAmount.Size = new System.Drawing.Size(41, 12);
this.lblTaxAmount.TabIndex = 21;
this.lblTaxAmount.Text = "";
//111======525
this.txtTaxAmount.Location = new System.Drawing.Point(173,521);
this.txtTaxAmount.Name ="txtTaxAmount";
this.txtTaxAmount.Size = new System.Drawing.Size(100, 21);
this.txtTaxAmount.TabIndex = 21;
this.Controls.Add(this.lblTaxAmount);
this.Controls.Add(this.txtTaxAmount);

           //#####SubtotalTrPriceAmount###Decimal
this.lblSubtotalTrPriceAmount.AutoSize = true;
this.lblSubtotalTrPriceAmount.Location = new System.Drawing.Point(100,550);
this.lblSubtotalTrPriceAmount.Name = "lblSubtotalTrPriceAmount";
this.lblSubtotalTrPriceAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalTrPriceAmount.TabIndex = 22;
this.lblSubtotalTrPriceAmount.Text = "";
//111======550
this.txtSubtotalTrPriceAmount.Location = new System.Drawing.Point(173,546);
this.txtSubtotalTrPriceAmount.Name ="txtSubtotalTrPriceAmount";
this.txtSubtotalTrPriceAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalTrPriceAmount.TabIndex = 22;
this.Controls.Add(this.lblSubtotalTrPriceAmount);
this.Controls.Add(this.txtSubtotalTrPriceAmount);

           //#####IsGift###Boolean
this.lblIsGift.AutoSize = true;
this.lblIsGift.Location = new System.Drawing.Point(100,575);
this.lblIsGift.Name = "lblIsGift";
this.lblIsGift.Size = new System.Drawing.Size(41, 12);
this.lblIsGift.TabIndex = 23;
this.lblIsGift.Text = "";
this.chkIsGift.Location = new System.Drawing.Point(173,571);
this.chkIsGift.Name = "chkIsGift";
this.chkIsGift.Size = new System.Drawing.Size(100, 21);
this.chkIsGift.TabIndex = 23;
this.Controls.Add(this.lblIsGift);
this.Controls.Add(this.chkIsGift);

           //#####50CustomertModel###String
this.lblCustomertModel.AutoSize = true;
this.lblCustomertModel.Location = new System.Drawing.Point(100,600);
this.lblCustomertModel.Name = "lblCustomertModel";
this.lblCustomertModel.Size = new System.Drawing.Size(41, 12);
this.lblCustomertModel.TabIndex = 24;
this.lblCustomertModel.Text = "";
this.txtCustomertModel.Location = new System.Drawing.Point(173,596);
this.txtCustomertModel.Name = "txtCustomertModel";
this.txtCustomertModel.Size = new System.Drawing.Size(100, 21);
this.txtCustomertModel.TabIndex = 24;
this.Controls.Add(this.lblCustomertModel);
this.Controls.Add(this.txtCustomertModel);

           //#####255Summary###String
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

           //#####PrintStatus###Int32

           //#####DataStatus###Int32

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,725);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 29;
this.lblApprovalResults.Text = "";
this.chkApprovalResults.Location = new System.Drawing.Point(173,721);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 29;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####200ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,750);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 30;
this.lblApprovalOpinions.Text = "";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,746);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 30;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####TotalAmount###Decimal
this.lblTotalAmount.AutoSize = true;
this.lblTotalAmount.Location = new System.Drawing.Point(100,775);
this.lblTotalAmount.Name = "lblTotalAmount";
this.lblTotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalAmount.TabIndex = 31;
this.lblTotalAmount.Text = "";
//111======775
this.txtTotalAmount.Location = new System.Drawing.Point(173,771);
this.txtTotalAmount.Name ="txtTotalAmount";
this.txtTotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalAmount.TabIndex = 31;
this.Controls.Add(this.lblTotalAmount);
this.Controls.Add(this.txtTotalAmount);

           //#####TotalTaxAmount###Decimal
this.lblTotalTaxAmount.AutoSize = true;
this.lblTotalTaxAmount.Location = new System.Drawing.Point(100,800);
this.lblTotalTaxAmount.Name = "lblTotalTaxAmount";
this.lblTotalTaxAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalTaxAmount.TabIndex = 32;
this.lblTotalTaxAmount.Text = "";
//111======800
this.txtTotalTaxAmount.Location = new System.Drawing.Point(173,796);
this.txtTotalTaxAmount.Name ="txtTotalTaxAmount";
this.txtTotalTaxAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalTaxAmount.TabIndex = 32;
this.Controls.Add(this.lblTotalTaxAmount);
this.Controls.Add(this.txtTotalTaxAmount);

           //#####TotalQty###Int32

           //#####Category_ID###Int64

           //#####Rack_ID###Int64

           //#####255UnitName###String
this.lblUnitName.AutoSize = true;
this.lblUnitName.Location = new System.Drawing.Point(100,900);
this.lblUnitName.Name = "lblUnitName";
this.lblUnitName.Size = new System.Drawing.Size(41, 12);
this.lblUnitName.TabIndex = 36;
this.lblUnitName.Text = "";
this.txtUnitName.Location = new System.Drawing.Point(173,896);
this.txtUnitName.Name = "txtUnitName";
this.txtUnitName.Size = new System.Drawing.Size(100, 21);
this.txtUnitName.TabIndex = 36;
this.Controls.Add(this.lblUnitName);
this.Controls.Add(this.txtUnitName);

           //#####IsIncludeTax###Boolean
this.lblIsIncludeTax.AutoSize = true;
this.lblIsIncludeTax.Location = new System.Drawing.Point(100,925);
this.lblIsIncludeTax.Name = "lblIsIncludeTax";
this.lblIsIncludeTax.Size = new System.Drawing.Size(41, 12);
this.lblIsIncludeTax.TabIndex = 37;
this.lblIsIncludeTax.Text = "";
this.chkIsIncludeTax.Location = new System.Drawing.Point(173,921);
this.chkIsIncludeTax.Name = "chkIsIncludeTax";
this.chkIsIncludeTax.Size = new System.Drawing.Size(100, 21);
this.chkIsIncludeTax.TabIndex = 37;
this.Controls.Add(this.lblIsIncludeTax);
this.Controls.Add(this.chkIsIncludeTax);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblPurEntryReNo );
this.Controls.Add(this.txtPurEntryReNo );

                this.Controls.Add(this.lblPurEntryNo );
this.Controls.Add(this.txtPurEntryNo );

                
                
                
                
                this.Controls.Add(this.lblReturnDate );
this.Controls.Add(this.dtpReturnDate );

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

                this.Controls.Add(this.lblTaxRate );
this.Controls.Add(this.txtTaxRate );

                this.Controls.Add(this.lblTaxAmount );
this.Controls.Add(this.txtTaxAmount );

                this.Controls.Add(this.lblSubtotalTrPriceAmount );
this.Controls.Add(this.txtSubtotalTrPriceAmount );

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

                
                
                
                this.Controls.Add(this.lblUnitName );
this.Controls.Add(this.txtUnitName );

                this.Controls.Add(this.lblIsIncludeTax );
this.Controls.Add(this.chkIsIncludeTax );

                    
            this.Name = "View_PurEntryReItemsQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPurEntryReNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPurEntryReNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPurEntryNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPurEntryNo;

    
        
              
    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblReturnDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpReturnDate;

    
        
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

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTaxRate;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTaxRate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTaxAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTaxAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalTrPriceAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalTrPriceAmount;

    
        
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

    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUnitName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUnitName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsIncludeTax;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsIncludeTax;

    
    
   
 





    }
}


