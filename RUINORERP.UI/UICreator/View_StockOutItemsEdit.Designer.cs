// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:40
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 其它出库统计
    /// </summary>
    partial class View_StockOutItemsEdit
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
     this.lblType_ID = new Krypton.Toolkit.KryptonLabel();
this.txtType_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblCustomerVendor_ID = new Krypton.Toolkit.KryptonLabel();
this.txtCustomerVendor_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
this.txtEmployee_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblBillNo = new Krypton.Toolkit.KryptonLabel();
this.txtBillNo = new Krypton.Toolkit.KryptonTextBox();

this.lblBill_Date = new Krypton.Toolkit.KryptonLabel();
this.dtpBill_Date = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblOut_date = new Krypton.Toolkit.KryptonLabel();
this.dtpOut_date = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblCreated_at = new Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblCreated_by = new Krypton.Toolkit.KryptonLabel();
this.txtCreated_by = new Krypton.Toolkit.KryptonTextBox();

this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblApprovalResults = new Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";

this.lblRefNO = new Krypton.Toolkit.KryptonLabel();
this.txtRefNO = new Krypton.Toolkit.KryptonTextBox();

this.lblRefBizType = new Krypton.Toolkit.KryptonLabel();
this.txtRefBizType = new Krypton.Toolkit.KryptonTextBox();

this.lblLocation_ID = new Krypton.Toolkit.KryptonLabel();
this.txtLocation_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblProdDetailID = new Krypton.Toolkit.KryptonLabel();
this.txtProdDetailID = new Krypton.Toolkit.KryptonTextBox();

this.lblRack_ID = new Krypton.Toolkit.KryptonLabel();
this.txtRack_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblQty = new Krypton.Toolkit.KryptonLabel();
this.txtQty = new Krypton.Toolkit.KryptonTextBox();

this.lblPrice = new Krypton.Toolkit.KryptonLabel();
this.txtPrice = new Krypton.Toolkit.KryptonTextBox();

this.lblCost = new Krypton.Toolkit.KryptonLabel();
this.txtCost = new Krypton.Toolkit.KryptonTextBox();

this.lblSummary = new Krypton.Toolkit.KryptonLabel();
this.txtSummary = new Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

this.lblSubtotalCostAmount = new Krypton.Toolkit.KryptonLabel();
this.txtSubtotalCostAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalPirceAmount = new Krypton.Toolkit.KryptonLabel();
this.txtSubtotalPirceAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblproperty = new Krypton.Toolkit.KryptonLabel();
this.txtproperty = new Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

this.lblSKU = new Krypton.Toolkit.KryptonLabel();
this.txtSKU = new Krypton.Toolkit.KryptonTextBox();

this.lblCNName = new Krypton.Toolkit.KryptonLabel();
this.txtCNName = new Krypton.Toolkit.KryptonTextBox();
this.txtCNName.Multiline = true;

this.lblSpecifications = new Krypton.Toolkit.KryptonLabel();
this.txtSpecifications = new Krypton.Toolkit.KryptonTextBox();
this.txtSpecifications.Multiline = true;

this.lblProductNo = new Krypton.Toolkit.KryptonLabel();
this.txtProductNo = new Krypton.Toolkit.KryptonTextBox();

this.lblModel = new Krypton.Toolkit.KryptonLabel();
this.txtModel = new Krypton.Toolkit.KryptonTextBox();

this.lblCategory_ID = new Krypton.Toolkit.KryptonLabel();
this.txtCategory_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblDataStatus = new Krypton.Toolkit.KryptonLabel();
this.txtDataStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblApprovalOpinions = new Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new Krypton.Toolkit.KryptonTextBox();
this.txtApprovalOpinions.Multiline = true;


this.lblRefBillID = new Krypton.Toolkit.KryptonLabel();
this.txtRefBillID = new Krypton.Toolkit.KryptonTextBox();

this.lblUnit_ID = new Krypton.Toolkit.KryptonLabel();
this.txtUnit_ID = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####Type_ID###Int64
this.lblType_ID.AutoSize = true;
this.lblType_ID.Location = new System.Drawing.Point(100,25);
this.lblType_ID.Name = "lblType_ID";
this.lblType_ID.Size = new System.Drawing.Size(41, 12);
this.lblType_ID.TabIndex = 1;
this.lblType_ID.Text = "";
this.txtType_ID.Location = new System.Drawing.Point(173,21);
this.txtType_ID.Name = "txtType_ID";
this.txtType_ID.Size = new System.Drawing.Size(100, 21);
this.txtType_ID.TabIndex = 1;
this.Controls.Add(this.lblType_ID);
this.Controls.Add(this.txtType_ID);

           //#####CustomerVendor_ID###Int64
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,50);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 2;
this.lblCustomerVendor_ID.Text = "";
this.txtCustomerVendor_ID.Location = new System.Drawing.Point(173,46);
this.txtCustomerVendor_ID.Name = "txtCustomerVendor_ID";
this.txtCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.txtCustomerVendor_ID.TabIndex = 2;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.txtCustomerVendor_ID);

           //#####Employee_ID###Int64
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,75);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 3;
this.lblEmployee_ID.Text = "";
this.txtEmployee_ID.Location = new System.Drawing.Point(173,71);
this.txtEmployee_ID.Name = "txtEmployee_ID";
this.txtEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.txtEmployee_ID.TabIndex = 3;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.txtEmployee_ID);

           //#####50BillNo###String
this.lblBillNo.AutoSize = true;
this.lblBillNo.Location = new System.Drawing.Point(100,100);
this.lblBillNo.Name = "lblBillNo";
this.lblBillNo.Size = new System.Drawing.Size(41, 12);
this.lblBillNo.TabIndex = 4;
this.lblBillNo.Text = "";
this.txtBillNo.Location = new System.Drawing.Point(173,96);
this.txtBillNo.Name = "txtBillNo";
this.txtBillNo.Size = new System.Drawing.Size(100, 21);
this.txtBillNo.TabIndex = 4;
this.Controls.Add(this.lblBillNo);
this.Controls.Add(this.txtBillNo);

           //#####Bill_Date###DateTime
this.lblBill_Date.AutoSize = true;
this.lblBill_Date.Location = new System.Drawing.Point(100,125);
this.lblBill_Date.Name = "lblBill_Date";
this.lblBill_Date.Size = new System.Drawing.Size(41, 12);
this.lblBill_Date.TabIndex = 5;
this.lblBill_Date.Text = "";
//111======125
this.dtpBill_Date.Location = new System.Drawing.Point(173,121);
this.dtpBill_Date.Name ="dtpBill_Date";
this.dtpBill_Date.ShowCheckBox =true;
this.dtpBill_Date.Size = new System.Drawing.Size(100, 21);
this.dtpBill_Date.TabIndex = 5;
this.Controls.Add(this.lblBill_Date);
this.Controls.Add(this.dtpBill_Date);

           //#####Out_date###DateTime
this.lblOut_date.AutoSize = true;
this.lblOut_date.Location = new System.Drawing.Point(100,150);
this.lblOut_date.Name = "lblOut_date";
this.lblOut_date.Size = new System.Drawing.Size(41, 12);
this.lblOut_date.TabIndex = 6;
this.lblOut_date.Text = "";
//111======150
this.dtpOut_date.Location = new System.Drawing.Point(173,146);
this.dtpOut_date.Name ="dtpOut_date";
this.dtpOut_date.ShowCheckBox =true;
this.dtpOut_date.Size = new System.Drawing.Size(100, 21);
this.dtpOut_date.TabIndex = 6;
this.Controls.Add(this.lblOut_date);
this.Controls.Add(this.dtpOut_date);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,175);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 7;
this.lblCreated_at.Text = "";
//111======175
this.dtpCreated_at.Location = new System.Drawing.Point(173,171);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 7;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,200);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 8;
this.lblCreated_by.Text = "";
this.txtCreated_by.Location = new System.Drawing.Point(173,196);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 8;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

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

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,250);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 10;
this.lblApprovalResults.Text = "";
this.chkApprovalResults.Location = new System.Drawing.Point(173,246);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 10;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####50RefNO###String
this.lblRefNO.AutoSize = true;
this.lblRefNO.Location = new System.Drawing.Point(100,275);
this.lblRefNO.Name = "lblRefNO";
this.lblRefNO.Size = new System.Drawing.Size(41, 12);
this.lblRefNO.TabIndex = 11;
this.lblRefNO.Text = "";
this.txtRefNO.Location = new System.Drawing.Point(173,271);
this.txtRefNO.Name = "txtRefNO";
this.txtRefNO.Size = new System.Drawing.Size(100, 21);
this.txtRefNO.TabIndex = 11;
this.Controls.Add(this.lblRefNO);
this.Controls.Add(this.txtRefNO);

           //#####RefBizType###Int32
this.lblRefBizType.AutoSize = true;
this.lblRefBizType.Location = new System.Drawing.Point(100,300);
this.lblRefBizType.Name = "lblRefBizType";
this.lblRefBizType.Size = new System.Drawing.Size(41, 12);
this.lblRefBizType.TabIndex = 12;
this.lblRefBizType.Text = "";
this.txtRefBizType.Location = new System.Drawing.Point(173,296);
this.txtRefBizType.Name = "txtRefBizType";
this.txtRefBizType.Size = new System.Drawing.Size(100, 21);
this.txtRefBizType.TabIndex = 12;
this.Controls.Add(this.lblRefBizType);
this.Controls.Add(this.txtRefBizType);

           //#####Location_ID###Int64
this.lblLocation_ID.AutoSize = true;
this.lblLocation_ID.Location = new System.Drawing.Point(100,325);
this.lblLocation_ID.Name = "lblLocation_ID";
this.lblLocation_ID.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID.TabIndex = 13;
this.lblLocation_ID.Text = "";
this.txtLocation_ID.Location = new System.Drawing.Point(173,321);
this.txtLocation_ID.Name = "txtLocation_ID";
this.txtLocation_ID.Size = new System.Drawing.Size(100, 21);
this.txtLocation_ID.TabIndex = 13;
this.Controls.Add(this.lblLocation_ID);
this.Controls.Add(this.txtLocation_ID);

           //#####ProdDetailID###Int64
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,350);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 14;
this.lblProdDetailID.Text = "";
this.txtProdDetailID.Location = new System.Drawing.Point(173,346);
this.txtProdDetailID.Name = "txtProdDetailID";
this.txtProdDetailID.Size = new System.Drawing.Size(100, 21);
this.txtProdDetailID.TabIndex = 14;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.txtProdDetailID);

           //#####Rack_ID###Int64
this.lblRack_ID.AutoSize = true;
this.lblRack_ID.Location = new System.Drawing.Point(100,375);
this.lblRack_ID.Name = "lblRack_ID";
this.lblRack_ID.Size = new System.Drawing.Size(41, 12);
this.lblRack_ID.TabIndex = 15;
this.lblRack_ID.Text = "";
this.txtRack_ID.Location = new System.Drawing.Point(173,371);
this.txtRack_ID.Name = "txtRack_ID";
this.txtRack_ID.Size = new System.Drawing.Size(100, 21);
this.txtRack_ID.TabIndex = 15;
this.Controls.Add(this.lblRack_ID);
this.Controls.Add(this.txtRack_ID);

           //#####Qty###Int32
this.lblQty.AutoSize = true;
this.lblQty.Location = new System.Drawing.Point(100,400);
this.lblQty.Name = "lblQty";
this.lblQty.Size = new System.Drawing.Size(41, 12);
this.lblQty.TabIndex = 16;
this.lblQty.Text = "";
this.txtQty.Location = new System.Drawing.Point(173,396);
this.txtQty.Name = "txtQty";
this.txtQty.Size = new System.Drawing.Size(100, 21);
this.txtQty.TabIndex = 16;
this.Controls.Add(this.lblQty);
this.Controls.Add(this.txtQty);

           //#####Price###Decimal
this.lblPrice.AutoSize = true;
this.lblPrice.Location = new System.Drawing.Point(100,425);
this.lblPrice.Name = "lblPrice";
this.lblPrice.Size = new System.Drawing.Size(41, 12);
this.lblPrice.TabIndex = 17;
this.lblPrice.Text = "";
//111======425
this.txtPrice.Location = new System.Drawing.Point(173,421);
this.txtPrice.Name ="txtPrice";
this.txtPrice.Size = new System.Drawing.Size(100, 21);
this.txtPrice.TabIndex = 17;
this.Controls.Add(this.lblPrice);
this.Controls.Add(this.txtPrice);

           //#####Cost###Decimal
this.lblCost.AutoSize = true;
this.lblCost.Location = new System.Drawing.Point(100,450);
this.lblCost.Name = "lblCost";
this.lblCost.Size = new System.Drawing.Size(41, 12);
this.lblCost.TabIndex = 18;
this.lblCost.Text = "";
//111======450
this.txtCost.Location = new System.Drawing.Point(173,446);
this.txtCost.Name ="txtCost";
this.txtCost.Size = new System.Drawing.Size(100, 21);
this.txtCost.TabIndex = 18;
this.Controls.Add(this.lblCost);
this.Controls.Add(this.txtCost);

           //#####500Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,475);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 19;
this.lblSummary.Text = "";
this.txtSummary.Location = new System.Drawing.Point(173,471);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 19;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####SubtotalCostAmount###Decimal
this.lblSubtotalCostAmount.AutoSize = true;
this.lblSubtotalCostAmount.Location = new System.Drawing.Point(100,500);
this.lblSubtotalCostAmount.Name = "lblSubtotalCostAmount";
this.lblSubtotalCostAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalCostAmount.TabIndex = 20;
this.lblSubtotalCostAmount.Text = "";
//111======500
this.txtSubtotalCostAmount.Location = new System.Drawing.Point(173,496);
this.txtSubtotalCostAmount.Name ="txtSubtotalCostAmount";
this.txtSubtotalCostAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalCostAmount.TabIndex = 20;
this.Controls.Add(this.lblSubtotalCostAmount);
this.Controls.Add(this.txtSubtotalCostAmount);

           //#####SubtotalPirceAmount###Decimal
this.lblSubtotalPirceAmount.AutoSize = true;
this.lblSubtotalPirceAmount.Location = new System.Drawing.Point(100,525);
this.lblSubtotalPirceAmount.Name = "lblSubtotalPirceAmount";
this.lblSubtotalPirceAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalPirceAmount.TabIndex = 21;
this.lblSubtotalPirceAmount.Text = "";
//111======525
this.txtSubtotalPirceAmount.Location = new System.Drawing.Point(173,521);
this.txtSubtotalPirceAmount.Name ="txtSubtotalPirceAmount";
this.txtSubtotalPirceAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalPirceAmount.TabIndex = 21;
this.Controls.Add(this.lblSubtotalPirceAmount);
this.Controls.Add(this.txtSubtotalPirceAmount);

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,550);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 22;
this.lblproperty.Text = "";
this.txtproperty.Location = new System.Drawing.Point(173,546);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 22;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

           //#####80SKU###String
this.lblSKU.AutoSize = true;
this.lblSKU.Location = new System.Drawing.Point(100,575);
this.lblSKU.Name = "lblSKU";
this.lblSKU.Size = new System.Drawing.Size(41, 12);
this.lblSKU.TabIndex = 23;
this.lblSKU.Text = "";
this.txtSKU.Location = new System.Drawing.Point(173,571);
this.txtSKU.Name = "txtSKU";
this.txtSKU.Size = new System.Drawing.Size(100, 21);
this.txtSKU.TabIndex = 23;
this.Controls.Add(this.lblSKU);
this.Controls.Add(this.txtSKU);

           //#####255CNName###String
this.lblCNName.AutoSize = true;
this.lblCNName.Location = new System.Drawing.Point(100,600);
this.lblCNName.Name = "lblCNName";
this.lblCNName.Size = new System.Drawing.Size(41, 12);
this.lblCNName.TabIndex = 24;
this.lblCNName.Text = "";
this.txtCNName.Location = new System.Drawing.Point(173,596);
this.txtCNName.Name = "txtCNName";
this.txtCNName.Size = new System.Drawing.Size(100, 21);
this.txtCNName.TabIndex = 24;
this.Controls.Add(this.lblCNName);
this.Controls.Add(this.txtCNName);

           //#####1000Specifications###String
this.lblSpecifications.AutoSize = true;
this.lblSpecifications.Location = new System.Drawing.Point(100,625);
this.lblSpecifications.Name = "lblSpecifications";
this.lblSpecifications.Size = new System.Drawing.Size(41, 12);
this.lblSpecifications.TabIndex = 25;
this.lblSpecifications.Text = "";
this.txtSpecifications.Location = new System.Drawing.Point(173,621);
this.txtSpecifications.Name = "txtSpecifications";
this.txtSpecifications.Size = new System.Drawing.Size(100, 21);
this.txtSpecifications.TabIndex = 25;
this.Controls.Add(this.lblSpecifications);
this.Controls.Add(this.txtSpecifications);

           //#####40ProductNo###String
this.lblProductNo.AutoSize = true;
this.lblProductNo.Location = new System.Drawing.Point(100,650);
this.lblProductNo.Name = "lblProductNo";
this.lblProductNo.Size = new System.Drawing.Size(41, 12);
this.lblProductNo.TabIndex = 26;
this.lblProductNo.Text = "";
this.txtProductNo.Location = new System.Drawing.Point(173,646);
this.txtProductNo.Name = "txtProductNo";
this.txtProductNo.Size = new System.Drawing.Size(100, 21);
this.txtProductNo.TabIndex = 26;
this.Controls.Add(this.lblProductNo);
this.Controls.Add(this.txtProductNo);

           //#####50Model###String
this.lblModel.AutoSize = true;
this.lblModel.Location = new System.Drawing.Point(100,675);
this.lblModel.Name = "lblModel";
this.lblModel.Size = new System.Drawing.Size(41, 12);
this.lblModel.TabIndex = 27;
this.lblModel.Text = "";
this.txtModel.Location = new System.Drawing.Point(173,671);
this.txtModel.Name = "txtModel";
this.txtModel.Size = new System.Drawing.Size(100, 21);
this.txtModel.TabIndex = 27;
this.Controls.Add(this.lblModel);
this.Controls.Add(this.txtModel);

           //#####Category_ID###Int64
this.lblCategory_ID.AutoSize = true;
this.lblCategory_ID.Location = new System.Drawing.Point(100,700);
this.lblCategory_ID.Name = "lblCategory_ID";
this.lblCategory_ID.Size = new System.Drawing.Size(41, 12);
this.lblCategory_ID.TabIndex = 28;
this.lblCategory_ID.Text = "";
this.txtCategory_ID.Location = new System.Drawing.Point(173,696);
this.txtCategory_ID.Name = "txtCategory_ID";
this.txtCategory_ID.Size = new System.Drawing.Size(100, 21);
this.txtCategory_ID.TabIndex = 28;
this.Controls.Add(this.lblCategory_ID);
this.Controls.Add(this.txtCategory_ID);

           //#####DataStatus###Int32
this.lblDataStatus.AutoSize = true;
this.lblDataStatus.Location = new System.Drawing.Point(100,725);
this.lblDataStatus.Name = "lblDataStatus";
this.lblDataStatus.Size = new System.Drawing.Size(41, 12);
this.lblDataStatus.TabIndex = 29;
this.lblDataStatus.Text = "";
this.txtDataStatus.Location = new System.Drawing.Point(173,721);
this.txtDataStatus.Name = "txtDataStatus";
this.txtDataStatus.Size = new System.Drawing.Size(100, 21);
this.txtDataStatus.TabIndex = 29;
this.Controls.Add(this.lblDataStatus);
this.Controls.Add(this.txtDataStatus);

           //#####500ApprovalOpinions###String
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

           //#####ApprovalStatus###SByte

           //#####RefBillID###Int64
this.lblRefBillID.AutoSize = true;
this.lblRefBillID.Location = new System.Drawing.Point(100,800);
this.lblRefBillID.Name = "lblRefBillID";
this.lblRefBillID.Size = new System.Drawing.Size(41, 12);
this.lblRefBillID.TabIndex = 32;
this.lblRefBillID.Text = "";
this.txtRefBillID.Location = new System.Drawing.Point(173,796);
this.txtRefBillID.Name = "txtRefBillID";
this.txtRefBillID.Size = new System.Drawing.Size(100, 21);
this.txtRefBillID.TabIndex = 32;
this.Controls.Add(this.lblRefBillID);
this.Controls.Add(this.txtRefBillID);

           //#####Unit_ID###Int64
this.lblUnit_ID.AutoSize = true;
this.lblUnit_ID.Location = new System.Drawing.Point(100,825);
this.lblUnit_ID.Name = "lblUnit_ID";
this.lblUnit_ID.Size = new System.Drawing.Size(41, 12);
this.lblUnit_ID.TabIndex = 33;
this.lblUnit_ID.Text = "";
this.txtUnit_ID.Location = new System.Drawing.Point(173,821);
this.txtUnit_ID.Name = "txtUnit_ID";
this.txtUnit_ID.Size = new System.Drawing.Size(100, 21);
this.txtUnit_ID.TabIndex = 33;
this.Controls.Add(this.lblUnit_ID);
this.Controls.Add(this.txtUnit_ID);

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
           // this.kryptonPanel1.TabIndex = 33;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblType_ID );
this.Controls.Add(this.txtType_ID );

                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.txtCustomerVendor_ID );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.txtEmployee_ID );

                this.Controls.Add(this.lblBillNo );
this.Controls.Add(this.txtBillNo );

                this.Controls.Add(this.lblBill_Date );
this.Controls.Add(this.dtpBill_Date );

                this.Controls.Add(this.lblOut_date );
this.Controls.Add(this.dtpOut_date );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                this.Controls.Add(this.lblRefNO );
this.Controls.Add(this.txtRefNO );

                this.Controls.Add(this.lblRefBizType );
this.Controls.Add(this.txtRefBizType );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.txtLocation_ID );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.txtProdDetailID );

                this.Controls.Add(this.lblRack_ID );
this.Controls.Add(this.txtRack_ID );

                this.Controls.Add(this.lblQty );
this.Controls.Add(this.txtQty );

                this.Controls.Add(this.lblPrice );
this.Controls.Add(this.txtPrice );

                this.Controls.Add(this.lblCost );
this.Controls.Add(this.txtCost );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                this.Controls.Add(this.lblSubtotalCostAmount );
this.Controls.Add(this.txtSubtotalCostAmount );

                this.Controls.Add(this.lblSubtotalPirceAmount );
this.Controls.Add(this.txtSubtotalPirceAmount );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                this.Controls.Add(this.lblSKU );
this.Controls.Add(this.txtSKU );

                this.Controls.Add(this.lblCNName );
this.Controls.Add(this.txtCNName );

                this.Controls.Add(this.lblSpecifications );
this.Controls.Add(this.txtSpecifications );

                this.Controls.Add(this.lblProductNo );
this.Controls.Add(this.txtProductNo );

                this.Controls.Add(this.lblModel );
this.Controls.Add(this.txtModel );

                this.Controls.Add(this.lblCategory_ID );
this.Controls.Add(this.txtCategory_ID );

                this.Controls.Add(this.lblDataStatus );
this.Controls.Add(this.txtDataStatus );

                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                
                this.Controls.Add(this.lblRefBillID );
this.Controls.Add(this.txtRefBillID );

                this.Controls.Add(this.lblUnit_ID );
this.Controls.Add(this.txtUnit_ID );

                            // 
            // "View_StockOutItemsEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "View_StockOutItemsEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblType_ID;
private Krypton.Toolkit.KryptonTextBox txtType_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private Krypton.Toolkit.KryptonTextBox txtCustomerVendor_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonTextBox txtEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblBillNo;
private Krypton.Toolkit.KryptonTextBox txtBillNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblBill_Date;
private Krypton.Toolkit.KryptonDateTimePicker dtpBill_Date;

    
        
              private Krypton.Toolkit.KryptonLabel lblOut_date;
private Krypton.Toolkit.KryptonDateTimePicker dtpOut_date;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_by;
private Krypton.Toolkit.KryptonTextBox txtCreated_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalResults;
private Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              private Krypton.Toolkit.KryptonLabel lblRefNO;
private Krypton.Toolkit.KryptonTextBox txtRefNO;

    
        
              private Krypton.Toolkit.KryptonLabel lblRefBizType;
private Krypton.Toolkit.KryptonTextBox txtRefBizType;

    
        
              private Krypton.Toolkit.KryptonLabel lblLocation_ID;
private Krypton.Toolkit.KryptonTextBox txtLocation_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblProdDetailID;
private Krypton.Toolkit.KryptonTextBox txtProdDetailID;

    
        
              private Krypton.Toolkit.KryptonLabel lblRack_ID;
private Krypton.Toolkit.KryptonTextBox txtRack_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblQty;
private Krypton.Toolkit.KryptonTextBox txtQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblPrice;
private Krypton.Toolkit.KryptonTextBox txtPrice;

    
        
              private Krypton.Toolkit.KryptonLabel lblCost;
private Krypton.Toolkit.KryptonTextBox txtCost;

    
        
              private Krypton.Toolkit.KryptonLabel lblSummary;
private Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private Krypton.Toolkit.KryptonLabel lblSubtotalCostAmount;
private Krypton.Toolkit.KryptonTextBox txtSubtotalCostAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblSubtotalPirceAmount;
private Krypton.Toolkit.KryptonTextBox txtSubtotalPirceAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblproperty;
private Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private Krypton.Toolkit.KryptonLabel lblSKU;
private Krypton.Toolkit.KryptonTextBox txtSKU;

    
        
              private Krypton.Toolkit.KryptonLabel lblCNName;
private Krypton.Toolkit.KryptonTextBox txtCNName;

    
        
              private Krypton.Toolkit.KryptonLabel lblSpecifications;
private Krypton.Toolkit.KryptonTextBox txtSpecifications;

    
        
              private Krypton.Toolkit.KryptonLabel lblProductNo;
private Krypton.Toolkit.KryptonTextBox txtProductNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblModel;
private Krypton.Toolkit.KryptonTextBox txtModel;

    
        
              private Krypton.Toolkit.KryptonLabel lblCategory_ID;
private Krypton.Toolkit.KryptonTextBox txtCategory_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblDataStatus;
private Krypton.Toolkit.KryptonTextBox txtDataStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              
    
        
              private Krypton.Toolkit.KryptonLabel lblRefBillID;
private Krypton.Toolkit.KryptonTextBox txtRefBillID;

    
        
              private Krypton.Toolkit.KryptonLabel lblUnit_ID;
private Krypton.Toolkit.KryptonTextBox txtUnit_ID;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

