// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/11/2025 15:24:56
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 价格调整单明细
    /// </summary>
    partial class tb_FM_PriceAdjustmentDetailEdit
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
     this.lblAdjustId = new Krypton.Toolkit.KryptonLabel();
this.cmbAdjustId = new Krypton.Toolkit.KryptonComboBox();

this.lblProdDetailID = new Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new Krypton.Toolkit.KryptonComboBox();

this.lblLocation_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblproperty = new Krypton.Toolkit.KryptonLabel();
this.txtproperty = new Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

this.lblSpecifications = new Krypton.Toolkit.KryptonLabel();
this.txtSpecifications = new Krypton.Toolkit.KryptonTextBox();
this.txtSpecifications.Multiline = true;

this.lblUnit_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbUnit_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblOriginal_UnitPrice_NoTax = new Krypton.Toolkit.KryptonLabel();
this.txtOriginal_UnitPrice_NoTax = new Krypton.Toolkit.KryptonTextBox();

this.lblCorrect_UnitPrice_NoTax = new Krypton.Toolkit.KryptonLabel();
this.txtCorrect_UnitPrice_NoTax = new Krypton.Toolkit.KryptonTextBox();

this.lblOriginal_TaxRate = new Krypton.Toolkit.KryptonLabel();
this.txtOriginal_TaxRate = new Krypton.Toolkit.KryptonTextBox();

this.lblCorrect_TaxRate = new Krypton.Toolkit.KryptonLabel();
this.txtCorrect_TaxRate = new Krypton.Toolkit.KryptonTextBox();

this.lblOriginal_UnitPrice_WithTax = new Krypton.Toolkit.KryptonLabel();
this.txtOriginal_UnitPrice_WithTax = new Krypton.Toolkit.KryptonTextBox();

this.lblCorrect_UnitPrice_WithTax = new Krypton.Toolkit.KryptonLabel();
this.txtCorrect_UnitPrice_WithTax = new Krypton.Toolkit.KryptonTextBox();

this.lblUnitPrice_NoTax_Diff = new Krypton.Toolkit.KryptonLabel();
this.txtUnitPrice_NoTax_Diff = new Krypton.Toolkit.KryptonTextBox();

this.lblUnitPrice_WithTax_Diff = new Krypton.Toolkit.KryptonLabel();
this.txtUnitPrice_WithTax_Diff = new Krypton.Toolkit.KryptonTextBox();

this.lblQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtQuantity = new Krypton.Toolkit.KryptonTextBox();

this.lblOriginal_TaxAmount = new Krypton.Toolkit.KryptonLabel();
this.txtOriginal_TaxAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblCorrect_TaxAmount = new Krypton.Toolkit.KryptonLabel();
this.txtCorrect_TaxAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblTaxAmount_Diff = new Krypton.Toolkit.KryptonLabel();
this.txtTaxAmount_Diff = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalAmount_Diff_NoTax = new Krypton.Toolkit.KryptonLabel();
this.txtTotalAmount_Diff_NoTax = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalAmount_Diff_WithTax = new Krypton.Toolkit.KryptonLabel();
this.txtTotalAmount_Diff_WithTax = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalAmount_Diff = new Krypton.Toolkit.KryptonLabel();
this.txtTotalAmount_Diff = new Krypton.Toolkit.KryptonTextBox();

this.lblCustomerPartNo = new Krypton.Toolkit.KryptonLabel();
this.txtCustomerPartNo = new Krypton.Toolkit.KryptonTextBox();

this.lblAdjustReason = new Krypton.Toolkit.KryptonLabel();
this.txtAdjustReason = new Krypton.Toolkit.KryptonTextBox();
this.txtAdjustReason.Multiline = true;

this.lblSummary = new Krypton.Toolkit.KryptonLabel();
this.txtSummary = new Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

    
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
     
            //#####AdjustId###Int64
//属性测试25AdjustId
//属性测试25AdjustId
//属性测试25AdjustId
this.lblAdjustId.AutoSize = true;
this.lblAdjustId.Location = new System.Drawing.Point(100,25);
this.lblAdjustId.Name = "lblAdjustId";
this.lblAdjustId.Size = new System.Drawing.Size(41, 12);
this.lblAdjustId.TabIndex = 1;
this.lblAdjustId.Text = "价格调整单";
//111======25
this.cmbAdjustId.Location = new System.Drawing.Point(173,21);
this.cmbAdjustId.Name ="cmbAdjustId";
this.cmbAdjustId.Size = new System.Drawing.Size(100, 21);
this.cmbAdjustId.TabIndex = 1;
this.Controls.Add(this.lblAdjustId);
this.Controls.Add(this.cmbAdjustId);

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

           //#####1000Specifications###String
this.lblSpecifications.AutoSize = true;
this.lblSpecifications.Location = new System.Drawing.Point(100,125);
this.lblSpecifications.Name = "lblSpecifications";
this.lblSpecifications.Size = new System.Drawing.Size(41, 12);
this.lblSpecifications.TabIndex = 5;
this.lblSpecifications.Text = "规格";
this.txtSpecifications.Location = new System.Drawing.Point(173,121);
this.txtSpecifications.Name = "txtSpecifications";
this.txtSpecifications.Size = new System.Drawing.Size(100, 21);
this.txtSpecifications.TabIndex = 5;
this.Controls.Add(this.lblSpecifications);
this.Controls.Add(this.txtSpecifications);

           //#####Unit_ID###Int64
//属性测试150Unit_ID
//属性测试150Unit_ID
this.lblUnit_ID.AutoSize = true;
this.lblUnit_ID.Location = new System.Drawing.Point(100,150);
this.lblUnit_ID.Name = "lblUnit_ID";
this.lblUnit_ID.Size = new System.Drawing.Size(41, 12);
this.lblUnit_ID.TabIndex = 6;
this.lblUnit_ID.Text = "单位";
//111======150
this.cmbUnit_ID.Location = new System.Drawing.Point(173,146);
this.cmbUnit_ID.Name ="cmbUnit_ID";
this.cmbUnit_ID.Size = new System.Drawing.Size(100, 21);
this.cmbUnit_ID.TabIndex = 6;
this.Controls.Add(this.lblUnit_ID);
this.Controls.Add(this.cmbUnit_ID);

           //#####Original_UnitPrice_NoTax###Decimal
this.lblOriginal_UnitPrice_NoTax.AutoSize = true;
this.lblOriginal_UnitPrice_NoTax.Location = new System.Drawing.Point(100,175);
this.lblOriginal_UnitPrice_NoTax.Name = "lblOriginal_UnitPrice_NoTax";
this.lblOriginal_UnitPrice_NoTax.Size = new System.Drawing.Size(41, 12);
this.lblOriginal_UnitPrice_NoTax.TabIndex = 7;
this.lblOriginal_UnitPrice_NoTax.Text = "原未税单价";
//111======175
this.txtOriginal_UnitPrice_NoTax.Location = new System.Drawing.Point(173,171);
this.txtOriginal_UnitPrice_NoTax.Name ="txtOriginal_UnitPrice_NoTax";
this.txtOriginal_UnitPrice_NoTax.Size = new System.Drawing.Size(100, 21);
this.txtOriginal_UnitPrice_NoTax.TabIndex = 7;
this.Controls.Add(this.lblOriginal_UnitPrice_NoTax);
this.Controls.Add(this.txtOriginal_UnitPrice_NoTax);

           //#####Correct_UnitPrice_NoTax###Decimal
this.lblCorrect_UnitPrice_NoTax.AutoSize = true;
this.lblCorrect_UnitPrice_NoTax.Location = new System.Drawing.Point(100,200);
this.lblCorrect_UnitPrice_NoTax.Name = "lblCorrect_UnitPrice_NoTax";
this.lblCorrect_UnitPrice_NoTax.Size = new System.Drawing.Size(41, 12);
this.lblCorrect_UnitPrice_NoTax.TabIndex = 8;
this.lblCorrect_UnitPrice_NoTax.Text = "新未税单价";
//111======200
this.txtCorrect_UnitPrice_NoTax.Location = new System.Drawing.Point(173,196);
this.txtCorrect_UnitPrice_NoTax.Name ="txtCorrect_UnitPrice_NoTax";
this.txtCorrect_UnitPrice_NoTax.Size = new System.Drawing.Size(100, 21);
this.txtCorrect_UnitPrice_NoTax.TabIndex = 8;
this.Controls.Add(this.lblCorrect_UnitPrice_NoTax);
this.Controls.Add(this.txtCorrect_UnitPrice_NoTax);

           //#####Original_TaxRate###Decimal
this.lblOriginal_TaxRate.AutoSize = true;
this.lblOriginal_TaxRate.Location = new System.Drawing.Point(100,225);
this.lblOriginal_TaxRate.Name = "lblOriginal_TaxRate";
this.lblOriginal_TaxRate.Size = new System.Drawing.Size(41, 12);
this.lblOriginal_TaxRate.TabIndex = 9;
this.lblOriginal_TaxRate.Text = "原税率";
//111======225
this.txtOriginal_TaxRate.Location = new System.Drawing.Point(173,221);
this.txtOriginal_TaxRate.Name ="txtOriginal_TaxRate";
this.txtOriginal_TaxRate.Size = new System.Drawing.Size(100, 21);
this.txtOriginal_TaxRate.TabIndex = 9;
this.Controls.Add(this.lblOriginal_TaxRate);
this.Controls.Add(this.txtOriginal_TaxRate);

           //#####Correct_TaxRate###Decimal
this.lblCorrect_TaxRate.AutoSize = true;
this.lblCorrect_TaxRate.Location = new System.Drawing.Point(100,250);
this.lblCorrect_TaxRate.Name = "lblCorrect_TaxRate";
this.lblCorrect_TaxRate.Size = new System.Drawing.Size(41, 12);
this.lblCorrect_TaxRate.TabIndex = 10;
this.lblCorrect_TaxRate.Text = "新税率";
//111======250
this.txtCorrect_TaxRate.Location = new System.Drawing.Point(173,246);
this.txtCorrect_TaxRate.Name ="txtCorrect_TaxRate";
this.txtCorrect_TaxRate.Size = new System.Drawing.Size(100, 21);
this.txtCorrect_TaxRate.TabIndex = 10;
this.Controls.Add(this.lblCorrect_TaxRate);
this.Controls.Add(this.txtCorrect_TaxRate);

           //#####Original_UnitPrice_WithTax###Decimal
this.lblOriginal_UnitPrice_WithTax.AutoSize = true;
this.lblOriginal_UnitPrice_WithTax.Location = new System.Drawing.Point(100,275);
this.lblOriginal_UnitPrice_WithTax.Name = "lblOriginal_UnitPrice_WithTax";
this.lblOriginal_UnitPrice_WithTax.Size = new System.Drawing.Size(41, 12);
this.lblOriginal_UnitPrice_WithTax.TabIndex = 11;
this.lblOriginal_UnitPrice_WithTax.Text = "原含税单价";
//111======275
this.txtOriginal_UnitPrice_WithTax.Location = new System.Drawing.Point(173,271);
this.txtOriginal_UnitPrice_WithTax.Name ="txtOriginal_UnitPrice_WithTax";
this.txtOriginal_UnitPrice_WithTax.Size = new System.Drawing.Size(100, 21);
this.txtOriginal_UnitPrice_WithTax.TabIndex = 11;
this.Controls.Add(this.lblOriginal_UnitPrice_WithTax);
this.Controls.Add(this.txtOriginal_UnitPrice_WithTax);

           //#####Correct_UnitPrice_WithTax###Decimal
this.lblCorrect_UnitPrice_WithTax.AutoSize = true;
this.lblCorrect_UnitPrice_WithTax.Location = new System.Drawing.Point(100,300);
this.lblCorrect_UnitPrice_WithTax.Name = "lblCorrect_UnitPrice_WithTax";
this.lblCorrect_UnitPrice_WithTax.Size = new System.Drawing.Size(41, 12);
this.lblCorrect_UnitPrice_WithTax.TabIndex = 12;
this.lblCorrect_UnitPrice_WithTax.Text = "新含税单价";
//111======300
this.txtCorrect_UnitPrice_WithTax.Location = new System.Drawing.Point(173,296);
this.txtCorrect_UnitPrice_WithTax.Name ="txtCorrect_UnitPrice_WithTax";
this.txtCorrect_UnitPrice_WithTax.Size = new System.Drawing.Size(100, 21);
this.txtCorrect_UnitPrice_WithTax.TabIndex = 12;
this.Controls.Add(this.lblCorrect_UnitPrice_WithTax);
this.Controls.Add(this.txtCorrect_UnitPrice_WithTax);

           //#####UnitPrice_NoTax_Diff###Decimal
this.lblUnitPrice_NoTax_Diff.AutoSize = true;
this.lblUnitPrice_NoTax_Diff.Location = new System.Drawing.Point(100,325);
this.lblUnitPrice_NoTax_Diff.Name = "lblUnitPrice_NoTax_Diff";
this.lblUnitPrice_NoTax_Diff.Size = new System.Drawing.Size(41, 12);
this.lblUnitPrice_NoTax_Diff.TabIndex = 13;
this.lblUnitPrice_NoTax_Diff.Text = "未税单价差异";
//111======325
this.txtUnitPrice_NoTax_Diff.Location = new System.Drawing.Point(173,321);
this.txtUnitPrice_NoTax_Diff.Name ="txtUnitPrice_NoTax_Diff";
this.txtUnitPrice_NoTax_Diff.Size = new System.Drawing.Size(100, 21);
this.txtUnitPrice_NoTax_Diff.TabIndex = 13;
this.Controls.Add(this.lblUnitPrice_NoTax_Diff);
this.Controls.Add(this.txtUnitPrice_NoTax_Diff);

           //#####UnitPrice_WithTax_Diff###Decimal
this.lblUnitPrice_WithTax_Diff.AutoSize = true;
this.lblUnitPrice_WithTax_Diff.Location = new System.Drawing.Point(100,350);
this.lblUnitPrice_WithTax_Diff.Name = "lblUnitPrice_WithTax_Diff";
this.lblUnitPrice_WithTax_Diff.Size = new System.Drawing.Size(41, 12);
this.lblUnitPrice_WithTax_Diff.TabIndex = 14;
this.lblUnitPrice_WithTax_Diff.Text = "含税单价差异";
//111======350
this.txtUnitPrice_WithTax_Diff.Location = new System.Drawing.Point(173,346);
this.txtUnitPrice_WithTax_Diff.Name ="txtUnitPrice_WithTax_Diff";
this.txtUnitPrice_WithTax_Diff.Size = new System.Drawing.Size(100, 21);
this.txtUnitPrice_WithTax_Diff.TabIndex = 14;
this.Controls.Add(this.lblUnitPrice_WithTax_Diff);
this.Controls.Add(this.txtUnitPrice_WithTax_Diff);

           //#####Quantity###Decimal
this.lblQuantity.AutoSize = true;
this.lblQuantity.Location = new System.Drawing.Point(100,375);
this.lblQuantity.Name = "lblQuantity";
this.lblQuantity.Size = new System.Drawing.Size(41, 12);
this.lblQuantity.TabIndex = 15;
this.lblQuantity.Text = "数量";
//111======375
this.txtQuantity.Location = new System.Drawing.Point(173,371);
this.txtQuantity.Name ="txtQuantity";
this.txtQuantity.Size = new System.Drawing.Size(100, 21);
this.txtQuantity.TabIndex = 15;
this.Controls.Add(this.lblQuantity);
this.Controls.Add(this.txtQuantity);

           //#####Original_TaxAmount###Decimal
this.lblOriginal_TaxAmount.AutoSize = true;
this.lblOriginal_TaxAmount.Location = new System.Drawing.Point(100,400);
this.lblOriginal_TaxAmount.Name = "lblOriginal_TaxAmount";
this.lblOriginal_TaxAmount.Size = new System.Drawing.Size(41, 12);
this.lblOriginal_TaxAmount.TabIndex = 16;
this.lblOriginal_TaxAmount.Text = "原始税额";
//111======400
this.txtOriginal_TaxAmount.Location = new System.Drawing.Point(173,396);
this.txtOriginal_TaxAmount.Name ="txtOriginal_TaxAmount";
this.txtOriginal_TaxAmount.Size = new System.Drawing.Size(100, 21);
this.txtOriginal_TaxAmount.TabIndex = 16;
this.Controls.Add(this.lblOriginal_TaxAmount);
this.Controls.Add(this.txtOriginal_TaxAmount);

           //#####Correct_TaxAmount###Decimal
this.lblCorrect_TaxAmount.AutoSize = true;
this.lblCorrect_TaxAmount.Location = new System.Drawing.Point(100,425);
this.lblCorrect_TaxAmount.Name = "lblCorrect_TaxAmount";
this.lblCorrect_TaxAmount.Size = new System.Drawing.Size(41, 12);
this.lblCorrect_TaxAmount.TabIndex = 17;
this.lblCorrect_TaxAmount.Text = "新调税额";
//111======425
this.txtCorrect_TaxAmount.Location = new System.Drawing.Point(173,421);
this.txtCorrect_TaxAmount.Name ="txtCorrect_TaxAmount";
this.txtCorrect_TaxAmount.Size = new System.Drawing.Size(100, 21);
this.txtCorrect_TaxAmount.TabIndex = 17;
this.Controls.Add(this.lblCorrect_TaxAmount);
this.Controls.Add(this.txtCorrect_TaxAmount);

           //#####TaxAmount_Diff###Decimal
this.lblTaxAmount_Diff.AutoSize = true;
this.lblTaxAmount_Diff.Location = new System.Drawing.Point(100,450);
this.lblTaxAmount_Diff.Name = "lblTaxAmount_Diff";
this.lblTaxAmount_Diff.Size = new System.Drawing.Size(41, 12);
this.lblTaxAmount_Diff.TabIndex = 18;
this.lblTaxAmount_Diff.Text = "税额差异";
//111======450
this.txtTaxAmount_Diff.Location = new System.Drawing.Point(173,446);
this.txtTaxAmount_Diff.Name ="txtTaxAmount_Diff";
this.txtTaxAmount_Diff.Size = new System.Drawing.Size(100, 21);
this.txtTaxAmount_Diff.TabIndex = 18;
this.Controls.Add(this.lblTaxAmount_Diff);
this.Controls.Add(this.txtTaxAmount_Diff);

           //#####TotalAmount_Diff_NoTax###Decimal
this.lblTotalAmount_Diff_NoTax.AutoSize = true;
this.lblTotalAmount_Diff_NoTax.Location = new System.Drawing.Point(100,475);
this.lblTotalAmount_Diff_NoTax.Name = "lblTotalAmount_Diff_NoTax";
this.lblTotalAmount_Diff_NoTax.Size = new System.Drawing.Size(41, 12);
this.lblTotalAmount_Diff_NoTax.TabIndex = 19;
this.lblTotalAmount_Diff_NoTax.Text = "总未税差异金额";
//111======475
this.txtTotalAmount_Diff_NoTax.Location = new System.Drawing.Point(173,471);
this.txtTotalAmount_Diff_NoTax.Name ="txtTotalAmount_Diff_NoTax";
this.txtTotalAmount_Diff_NoTax.Size = new System.Drawing.Size(100, 21);
this.txtTotalAmount_Diff_NoTax.TabIndex = 19;
this.Controls.Add(this.lblTotalAmount_Diff_NoTax);
this.Controls.Add(this.txtTotalAmount_Diff_NoTax);

           //#####TotalAmount_Diff_WithTax###Decimal
this.lblTotalAmount_Diff_WithTax.AutoSize = true;
this.lblTotalAmount_Diff_WithTax.Location = new System.Drawing.Point(100,500);
this.lblTotalAmount_Diff_WithTax.Name = "lblTotalAmount_Diff_WithTax";
this.lblTotalAmount_Diff_WithTax.Size = new System.Drawing.Size(41, 12);
this.lblTotalAmount_Diff_WithTax.TabIndex = 20;
this.lblTotalAmount_Diff_WithTax.Text = "总含税差异金额价";
//111======500
this.txtTotalAmount_Diff_WithTax.Location = new System.Drawing.Point(173,496);
this.txtTotalAmount_Diff_WithTax.Name ="txtTotalAmount_Diff_WithTax";
this.txtTotalAmount_Diff_WithTax.Size = new System.Drawing.Size(100, 21);
this.txtTotalAmount_Diff_WithTax.TabIndex = 20;
this.Controls.Add(this.lblTotalAmount_Diff_WithTax);
this.Controls.Add(this.txtTotalAmount_Diff_WithTax);

           //#####TotalAmount_Diff###Decimal
this.lblTotalAmount_Diff.AutoSize = true;
this.lblTotalAmount_Diff.Location = new System.Drawing.Point(100,525);
this.lblTotalAmount_Diff.Name = "lblTotalAmount_Diff";
this.lblTotalAmount_Diff.Size = new System.Drawing.Size(41, 12);
this.lblTotalAmount_Diff.TabIndex = 21;
this.lblTotalAmount_Diff.Text = "总差异金额";
//111======525
this.txtTotalAmount_Diff.Location = new System.Drawing.Point(173,521);
this.txtTotalAmount_Diff.Name ="txtTotalAmount_Diff";
this.txtTotalAmount_Diff.Size = new System.Drawing.Size(100, 21);
this.txtTotalAmount_Diff.TabIndex = 21;
this.Controls.Add(this.lblTotalAmount_Diff);
this.Controls.Add(this.txtTotalAmount_Diff);

           //#####100CustomerPartNo###String
this.lblCustomerPartNo.AutoSize = true;
this.lblCustomerPartNo.Location = new System.Drawing.Point(100,550);
this.lblCustomerPartNo.Name = "lblCustomerPartNo";
this.lblCustomerPartNo.Size = new System.Drawing.Size(41, 12);
this.lblCustomerPartNo.TabIndex = 22;
this.lblCustomerPartNo.Text = "往来单位料号";
this.txtCustomerPartNo.Location = new System.Drawing.Point(173,546);
this.txtCustomerPartNo.Name = "txtCustomerPartNo";
this.txtCustomerPartNo.Size = new System.Drawing.Size(100, 21);
this.txtCustomerPartNo.TabIndex = 22;
this.Controls.Add(this.lblCustomerPartNo);
this.Controls.Add(this.txtCustomerPartNo);

           //#####500AdjustReason###String
this.lblAdjustReason.AutoSize = true;
this.lblAdjustReason.Location = new System.Drawing.Point(100,575);
this.lblAdjustReason.Name = "lblAdjustReason";
this.lblAdjustReason.Size = new System.Drawing.Size(41, 12);
this.lblAdjustReason.TabIndex = 23;
this.lblAdjustReason.Text = "调整原因";
this.txtAdjustReason.Location = new System.Drawing.Point(173,571);
this.txtAdjustReason.Name = "txtAdjustReason";
this.txtAdjustReason.Size = new System.Drawing.Size(100, 21);
this.txtAdjustReason.TabIndex = 23;
this.Controls.Add(this.lblAdjustReason);
this.Controls.Add(this.txtAdjustReason);

           //#####300Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,600);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 24;
this.lblSummary.Text = "摘要";
this.txtSummary.Location = new System.Drawing.Point(173,596);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 24;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

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
           // this.kryptonPanel1.TabIndex = 24;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblAdjustId );
this.Controls.Add(this.cmbAdjustId );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.cmbLocation_ID );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                this.Controls.Add(this.lblSpecifications );
this.Controls.Add(this.txtSpecifications );

                this.Controls.Add(this.lblUnit_ID );
this.Controls.Add(this.cmbUnit_ID );

                this.Controls.Add(this.lblOriginal_UnitPrice_NoTax );
this.Controls.Add(this.txtOriginal_UnitPrice_NoTax );

                this.Controls.Add(this.lblCorrect_UnitPrice_NoTax );
this.Controls.Add(this.txtCorrect_UnitPrice_NoTax );

                this.Controls.Add(this.lblOriginal_TaxRate );
this.Controls.Add(this.txtOriginal_TaxRate );

                this.Controls.Add(this.lblCorrect_TaxRate );
this.Controls.Add(this.txtCorrect_TaxRate );

                this.Controls.Add(this.lblOriginal_UnitPrice_WithTax );
this.Controls.Add(this.txtOriginal_UnitPrice_WithTax );

                this.Controls.Add(this.lblCorrect_UnitPrice_WithTax );
this.Controls.Add(this.txtCorrect_UnitPrice_WithTax );

                this.Controls.Add(this.lblUnitPrice_NoTax_Diff );
this.Controls.Add(this.txtUnitPrice_NoTax_Diff );

                this.Controls.Add(this.lblUnitPrice_WithTax_Diff );
this.Controls.Add(this.txtUnitPrice_WithTax_Diff );

                this.Controls.Add(this.lblQuantity );
this.Controls.Add(this.txtQuantity );

                this.Controls.Add(this.lblOriginal_TaxAmount );
this.Controls.Add(this.txtOriginal_TaxAmount );

                this.Controls.Add(this.lblCorrect_TaxAmount );
this.Controls.Add(this.txtCorrect_TaxAmount );

                this.Controls.Add(this.lblTaxAmount_Diff );
this.Controls.Add(this.txtTaxAmount_Diff );

                this.Controls.Add(this.lblTotalAmount_Diff_NoTax );
this.Controls.Add(this.txtTotalAmount_Diff_NoTax );

                this.Controls.Add(this.lblTotalAmount_Diff_WithTax );
this.Controls.Add(this.txtTotalAmount_Diff_WithTax );

                this.Controls.Add(this.lblTotalAmount_Diff );
this.Controls.Add(this.txtTotalAmount_Diff );

                this.Controls.Add(this.lblCustomerPartNo );
this.Controls.Add(this.txtCustomerPartNo );

                this.Controls.Add(this.lblAdjustReason );
this.Controls.Add(this.txtAdjustReason );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                            // 
            // "tb_FM_PriceAdjustmentDetailEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_FM_PriceAdjustmentDetailEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblAdjustId;
private Krypton.Toolkit.KryptonComboBox cmbAdjustId;

    
        
              private Krypton.Toolkit.KryptonLabel lblProdDetailID;
private Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private Krypton.Toolkit.KryptonLabel lblLocation_ID;
private Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblproperty;
private Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private Krypton.Toolkit.KryptonLabel lblSpecifications;
private Krypton.Toolkit.KryptonTextBox txtSpecifications;

    
        
              private Krypton.Toolkit.KryptonLabel lblUnit_ID;
private Krypton.Toolkit.KryptonComboBox cmbUnit_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblOriginal_UnitPrice_NoTax;
private Krypton.Toolkit.KryptonTextBox txtOriginal_UnitPrice_NoTax;

    
        
              private Krypton.Toolkit.KryptonLabel lblCorrect_UnitPrice_NoTax;
private Krypton.Toolkit.KryptonTextBox txtCorrect_UnitPrice_NoTax;

    
        
              private Krypton.Toolkit.KryptonLabel lblOriginal_TaxRate;
private Krypton.Toolkit.KryptonTextBox txtOriginal_TaxRate;

    
        
              private Krypton.Toolkit.KryptonLabel lblCorrect_TaxRate;
private Krypton.Toolkit.KryptonTextBox txtCorrect_TaxRate;

    
        
              private Krypton.Toolkit.KryptonLabel lblOriginal_UnitPrice_WithTax;
private Krypton.Toolkit.KryptonTextBox txtOriginal_UnitPrice_WithTax;

    
        
              private Krypton.Toolkit.KryptonLabel lblCorrect_UnitPrice_WithTax;
private Krypton.Toolkit.KryptonTextBox txtCorrect_UnitPrice_WithTax;

    
        
              private Krypton.Toolkit.KryptonLabel lblUnitPrice_NoTax_Diff;
private Krypton.Toolkit.KryptonTextBox txtUnitPrice_NoTax_Diff;

    
        
              private Krypton.Toolkit.KryptonLabel lblUnitPrice_WithTax_Diff;
private Krypton.Toolkit.KryptonTextBox txtUnitPrice_WithTax_Diff;

    
        
              private Krypton.Toolkit.KryptonLabel lblQuantity;
private Krypton.Toolkit.KryptonTextBox txtQuantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblOriginal_TaxAmount;
private Krypton.Toolkit.KryptonTextBox txtOriginal_TaxAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblCorrect_TaxAmount;
private Krypton.Toolkit.KryptonTextBox txtCorrect_TaxAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblTaxAmount_Diff;
private Krypton.Toolkit.KryptonTextBox txtTaxAmount_Diff;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalAmount_Diff_NoTax;
private Krypton.Toolkit.KryptonTextBox txtTotalAmount_Diff_NoTax;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalAmount_Diff_WithTax;
private Krypton.Toolkit.KryptonTextBox txtTotalAmount_Diff_WithTax;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalAmount_Diff;
private Krypton.Toolkit.KryptonTextBox txtTotalAmount_Diff;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerPartNo;
private Krypton.Toolkit.KryptonTextBox txtCustomerPartNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblAdjustReason;
private Krypton.Toolkit.KryptonTextBox txtAdjustReason;

    
        
              private Krypton.Toolkit.KryptonLabel lblSummary;
private Krypton.Toolkit.KryptonTextBox txtSummary;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

