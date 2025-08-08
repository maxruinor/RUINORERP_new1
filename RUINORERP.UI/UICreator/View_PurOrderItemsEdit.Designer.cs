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
    partial class View_PurOrderItemsEdit
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
     this.lblPurOrder_ID = new Krypton.Toolkit.KryptonLabel();
this.txtPurOrder_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblPurOrderNo = new Krypton.Toolkit.KryptonLabel();
this.txtPurOrderNo = new Krypton.Toolkit.KryptonTextBox();

this.lblPDID = new Krypton.Toolkit.KryptonLabel();
this.txtPDID = new Krypton.Toolkit.KryptonTextBox();

this.lblCustomerVendor_ID = new Krypton.Toolkit.KryptonLabel();
this.txtCustomerVendor_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
this.txtEmployee_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblDepartmentID = new Krypton.Toolkit.KryptonLabel();
this.txtDepartmentID = new Krypton.Toolkit.KryptonTextBox();

this.lblPaytype_ID = new Krypton.Toolkit.KryptonLabel();
this.txtPaytype_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblSOrder_ID = new Krypton.Toolkit.KryptonLabel();
this.txtSOrder_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblPurDate = new Krypton.Toolkit.KryptonLabel();
this.dtpPurDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblIsIncludeTax = new Krypton.Toolkit.KryptonLabel();
this.chkIsIncludeTax = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsIncludeTax.Values.Text ="";

this.lblShipCost = new Krypton.Toolkit.KryptonLabel();
this.txtShipCost = new Krypton.Toolkit.KryptonTextBox();

this.lblOrderPreDeliveryDate = new Krypton.Toolkit.KryptonLabel();
this.dtpOrderPreDeliveryDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblArrival_date = new Krypton.Toolkit.KryptonLabel();
this.dtpArrival_date = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblDeposit = new Krypton.Toolkit.KryptonLabel();
this.txtDeposit = new Krypton.Toolkit.KryptonTextBox();

this.lblTaxDeductionType = new Krypton.Toolkit.KryptonLabel();
this.txtTaxDeductionType = new Krypton.Toolkit.KryptonTextBox();

this.lblProdDetailID = new Krypton.Toolkit.KryptonLabel();
this.txtProdDetailID = new Krypton.Toolkit.KryptonTextBox();

this.lblSKU = new Krypton.Toolkit.KryptonLabel();
this.txtSKU = new Krypton.Toolkit.KryptonTextBox();

this.lblSpecifications = new Krypton.Toolkit.KryptonLabel();
this.txtSpecifications = new Krypton.Toolkit.KryptonTextBox();
this.txtSpecifications.Multiline = true;

this.lblCNName = new Krypton.Toolkit.KryptonLabel();
this.txtCNName = new Krypton.Toolkit.KryptonTextBox();
this.txtCNName.Multiline = true;

this.lblModel = new Krypton.Toolkit.KryptonLabel();
this.txtModel = new Krypton.Toolkit.KryptonTextBox();

this.lblType_ID = new Krypton.Toolkit.KryptonLabel();
this.txtType_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblproperty = new Krypton.Toolkit.KryptonLabel();
this.txtproperty = new Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

this.lblLocation_ID = new Krypton.Toolkit.KryptonLabel();
this.txtLocation_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtQuantity = new Krypton.Toolkit.KryptonTextBox();

this.lblUnitPrice = new Krypton.Toolkit.KryptonLabel();
this.txtUnitPrice = new Krypton.Toolkit.KryptonTextBox();

this.lblTaxRate = new Krypton.Toolkit.KryptonLabel();
this.txtTaxRate = new Krypton.Toolkit.KryptonTextBox();

this.lblTaxAmount = new Krypton.Toolkit.KryptonLabel();
this.txtTaxAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalAmount = new Krypton.Toolkit.KryptonLabel();
this.txtSubtotalAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblIsGift = new Krypton.Toolkit.KryptonLabel();
this.chkIsGift = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsGift.Values.Text ="";

this.lblItemPreDeliveryDate = new Krypton.Toolkit.KryptonLabel();
this.dtpItemPreDeliveryDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblCustomertModel = new Krypton.Toolkit.KryptonLabel();
this.txtCustomertModel = new Krypton.Toolkit.KryptonTextBox();

this.lblDeliveredQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtDeliveredQuantity = new Krypton.Toolkit.KryptonTextBox();

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
     
            //#####PurOrder_ID###Int64
this.lblPurOrder_ID.AutoSize = true;
this.lblPurOrder_ID.Location = new System.Drawing.Point(100,25);
this.lblPurOrder_ID.Name = "lblPurOrder_ID";
this.lblPurOrder_ID.Size = new System.Drawing.Size(41, 12);
this.lblPurOrder_ID.TabIndex = 1;
this.lblPurOrder_ID.Text = "";
this.txtPurOrder_ID.Location = new System.Drawing.Point(173,21);
this.txtPurOrder_ID.Name = "txtPurOrder_ID";
this.txtPurOrder_ID.Size = new System.Drawing.Size(100, 21);
this.txtPurOrder_ID.TabIndex = 1;
this.Controls.Add(this.lblPurOrder_ID);
this.Controls.Add(this.txtPurOrder_ID);

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
this.lblPDID.AutoSize = true;
this.lblPDID.Location = new System.Drawing.Point(100,75);
this.lblPDID.Name = "lblPDID";
this.lblPDID.Size = new System.Drawing.Size(41, 12);
this.lblPDID.TabIndex = 3;
this.lblPDID.Text = "";
this.txtPDID.Location = new System.Drawing.Point(173,71);
this.txtPDID.Name = "txtPDID";
this.txtPDID.Size = new System.Drawing.Size(100, 21);
this.txtPDID.TabIndex = 3;
this.Controls.Add(this.lblPDID);
this.Controls.Add(this.txtPDID);

           //#####CustomerVendor_ID###Int64
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,100);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 4;
this.lblCustomerVendor_ID.Text = "";
this.txtCustomerVendor_ID.Location = new System.Drawing.Point(173,96);
this.txtCustomerVendor_ID.Name = "txtCustomerVendor_ID";
this.txtCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.txtCustomerVendor_ID.TabIndex = 4;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.txtCustomerVendor_ID);

           //#####Employee_ID###Int64
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,125);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 5;
this.lblEmployee_ID.Text = "";
this.txtEmployee_ID.Location = new System.Drawing.Point(173,121);
this.txtEmployee_ID.Name = "txtEmployee_ID";
this.txtEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.txtEmployee_ID.TabIndex = 5;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.txtEmployee_ID);

           //#####DepartmentID###Int64
this.lblDepartmentID.AutoSize = true;
this.lblDepartmentID.Location = new System.Drawing.Point(100,150);
this.lblDepartmentID.Name = "lblDepartmentID";
this.lblDepartmentID.Size = new System.Drawing.Size(41, 12);
this.lblDepartmentID.TabIndex = 6;
this.lblDepartmentID.Text = "";
this.txtDepartmentID.Location = new System.Drawing.Point(173,146);
this.txtDepartmentID.Name = "txtDepartmentID";
this.txtDepartmentID.Size = new System.Drawing.Size(100, 21);
this.txtDepartmentID.TabIndex = 6;
this.Controls.Add(this.lblDepartmentID);
this.Controls.Add(this.txtDepartmentID);

           //#####Paytype_ID###Int64
this.lblPaytype_ID.AutoSize = true;
this.lblPaytype_ID.Location = new System.Drawing.Point(100,175);
this.lblPaytype_ID.Name = "lblPaytype_ID";
this.lblPaytype_ID.Size = new System.Drawing.Size(41, 12);
this.lblPaytype_ID.TabIndex = 7;
this.lblPaytype_ID.Text = "";
this.txtPaytype_ID.Location = new System.Drawing.Point(173,171);
this.txtPaytype_ID.Name = "txtPaytype_ID";
this.txtPaytype_ID.Size = new System.Drawing.Size(100, 21);
this.txtPaytype_ID.TabIndex = 7;
this.Controls.Add(this.lblPaytype_ID);
this.Controls.Add(this.txtPaytype_ID);

           //#####SOrder_ID###Int64
this.lblSOrder_ID.AutoSize = true;
this.lblSOrder_ID.Location = new System.Drawing.Point(100,200);
this.lblSOrder_ID.Name = "lblSOrder_ID";
this.lblSOrder_ID.Size = new System.Drawing.Size(41, 12);
this.lblSOrder_ID.TabIndex = 8;
this.lblSOrder_ID.Text = "";
this.txtSOrder_ID.Location = new System.Drawing.Point(173,196);
this.txtSOrder_ID.Name = "txtSOrder_ID";
this.txtSOrder_ID.Size = new System.Drawing.Size(100, 21);
this.txtSOrder_ID.TabIndex = 8;
this.Controls.Add(this.lblSOrder_ID);
this.Controls.Add(this.txtSOrder_ID);

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
this.lblTaxDeductionType.AutoSize = true;
this.lblTaxDeductionType.Location = new System.Drawing.Point(100,400);
this.lblTaxDeductionType.Name = "lblTaxDeductionType";
this.lblTaxDeductionType.Size = new System.Drawing.Size(41, 12);
this.lblTaxDeductionType.TabIndex = 16;
this.lblTaxDeductionType.Text = "";
this.txtTaxDeductionType.Location = new System.Drawing.Point(173,396);
this.txtTaxDeductionType.Name = "txtTaxDeductionType";
this.txtTaxDeductionType.Size = new System.Drawing.Size(100, 21);
this.txtTaxDeductionType.TabIndex = 16;
this.Controls.Add(this.lblTaxDeductionType);
this.Controls.Add(this.txtTaxDeductionType);

           //#####ProdDetailID###Int64
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,425);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 17;
this.lblProdDetailID.Text = "";
this.txtProdDetailID.Location = new System.Drawing.Point(173,421);
this.txtProdDetailID.Name = "txtProdDetailID";
this.txtProdDetailID.Size = new System.Drawing.Size(100, 21);
this.txtProdDetailID.TabIndex = 17;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.txtProdDetailID);

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
this.lblType_ID.AutoSize = true;
this.lblType_ID.Location = new System.Drawing.Point(100,550);
this.lblType_ID.Name = "lblType_ID";
this.lblType_ID.Size = new System.Drawing.Size(41, 12);
this.lblType_ID.TabIndex = 22;
this.lblType_ID.Text = "";
this.txtType_ID.Location = new System.Drawing.Point(173,546);
this.txtType_ID.Name = "txtType_ID";
this.txtType_ID.Size = new System.Drawing.Size(100, 21);
this.txtType_ID.TabIndex = 22;
this.Controls.Add(this.lblType_ID);
this.Controls.Add(this.txtType_ID);

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
this.lblLocation_ID.AutoSize = true;
this.lblLocation_ID.Location = new System.Drawing.Point(100,600);
this.lblLocation_ID.Name = "lblLocation_ID";
this.lblLocation_ID.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID.TabIndex = 24;
this.lblLocation_ID.Text = "";
this.txtLocation_ID.Location = new System.Drawing.Point(173,596);
this.txtLocation_ID.Name = "txtLocation_ID";
this.txtLocation_ID.Size = new System.Drawing.Size(100, 21);
this.txtLocation_ID.TabIndex = 24;
this.Controls.Add(this.lblLocation_ID);
this.Controls.Add(this.txtLocation_ID);

           //#####Quantity###Int32
this.lblQuantity.AutoSize = true;
this.lblQuantity.Location = new System.Drawing.Point(100,625);
this.lblQuantity.Name = "lblQuantity";
this.lblQuantity.Size = new System.Drawing.Size(41, 12);
this.lblQuantity.TabIndex = 25;
this.lblQuantity.Text = "";
this.txtQuantity.Location = new System.Drawing.Point(173,621);
this.txtQuantity.Name = "txtQuantity";
this.txtQuantity.Size = new System.Drawing.Size(100, 21);
this.txtQuantity.TabIndex = 25;
this.Controls.Add(this.lblQuantity);
this.Controls.Add(this.txtQuantity);

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
this.lblDeliveredQuantity.AutoSize = true;
this.lblDeliveredQuantity.Location = new System.Drawing.Point(100,825);
this.lblDeliveredQuantity.Name = "lblDeliveredQuantity";
this.lblDeliveredQuantity.Size = new System.Drawing.Size(41, 12);
this.lblDeliveredQuantity.TabIndex = 33;
this.lblDeliveredQuantity.Text = "";
this.txtDeliveredQuantity.Location = new System.Drawing.Point(173,821);
this.txtDeliveredQuantity.Name = "txtDeliveredQuantity";
this.txtDeliveredQuantity.Size = new System.Drawing.Size(100, 21);
this.txtDeliveredQuantity.TabIndex = 33;
this.Controls.Add(this.lblDeliveredQuantity);
this.Controls.Add(this.txtDeliveredQuantity);

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
           // this.kryptonPanel1.TabIndex = 34;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblPurOrder_ID );
this.Controls.Add(this.txtPurOrder_ID );

                this.Controls.Add(this.lblPurOrderNo );
this.Controls.Add(this.txtPurOrderNo );

                this.Controls.Add(this.lblPDID );
this.Controls.Add(this.txtPDID );

                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.txtCustomerVendor_ID );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.txtEmployee_ID );

                this.Controls.Add(this.lblDepartmentID );
this.Controls.Add(this.txtDepartmentID );

                this.Controls.Add(this.lblPaytype_ID );
this.Controls.Add(this.txtPaytype_ID );

                this.Controls.Add(this.lblSOrder_ID );
this.Controls.Add(this.txtSOrder_ID );

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

                this.Controls.Add(this.lblTaxDeductionType );
this.Controls.Add(this.txtTaxDeductionType );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.txtProdDetailID );

                this.Controls.Add(this.lblSKU );
this.Controls.Add(this.txtSKU );

                this.Controls.Add(this.lblSpecifications );
this.Controls.Add(this.txtSpecifications );

                this.Controls.Add(this.lblCNName );
this.Controls.Add(this.txtCNName );

                this.Controls.Add(this.lblModel );
this.Controls.Add(this.txtModel );

                this.Controls.Add(this.lblType_ID );
this.Controls.Add(this.txtType_ID );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.txtLocation_ID );

                this.Controls.Add(this.lblQuantity );
this.Controls.Add(this.txtQuantity );

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

                this.Controls.Add(this.lblDeliveredQuantity );
this.Controls.Add(this.txtDeliveredQuantity );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                            // 
            // "View_PurOrderItemsEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "View_PurOrderItemsEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblPurOrder_ID;
private Krypton.Toolkit.KryptonTextBox txtPurOrder_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblPurOrderNo;
private Krypton.Toolkit.KryptonTextBox txtPurOrderNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblPDID;
private Krypton.Toolkit.KryptonTextBox txtPDID;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private Krypton.Toolkit.KryptonTextBox txtCustomerVendor_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonTextBox txtEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblDepartmentID;
private Krypton.Toolkit.KryptonTextBox txtDepartmentID;

    
        
              private Krypton.Toolkit.KryptonLabel lblPaytype_ID;
private Krypton.Toolkit.KryptonTextBox txtPaytype_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblSOrder_ID;
private Krypton.Toolkit.KryptonTextBox txtSOrder_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblPurDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpPurDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsIncludeTax;
private Krypton.Toolkit.KryptonCheckBox chkIsIncludeTax;

    
        
              private Krypton.Toolkit.KryptonLabel lblShipCost;
private Krypton.Toolkit.KryptonTextBox txtShipCost;

    
        
              private Krypton.Toolkit.KryptonLabel lblOrderPreDeliveryDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpOrderPreDeliveryDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private Krypton.Toolkit.KryptonLabel lblArrival_date;
private Krypton.Toolkit.KryptonDateTimePicker dtpArrival_date;

    
        
              private Krypton.Toolkit.KryptonLabel lblDeposit;
private Krypton.Toolkit.KryptonTextBox txtDeposit;

    
        
              private Krypton.Toolkit.KryptonLabel lblTaxDeductionType;
private Krypton.Toolkit.KryptonTextBox txtTaxDeductionType;

    
        
              private Krypton.Toolkit.KryptonLabel lblProdDetailID;
private Krypton.Toolkit.KryptonTextBox txtProdDetailID;

    
        
              private Krypton.Toolkit.KryptonLabel lblSKU;
private Krypton.Toolkit.KryptonTextBox txtSKU;

    
        
              private Krypton.Toolkit.KryptonLabel lblSpecifications;
private Krypton.Toolkit.KryptonTextBox txtSpecifications;

    
        
              private Krypton.Toolkit.KryptonLabel lblCNName;
private Krypton.Toolkit.KryptonTextBox txtCNName;

    
        
              private Krypton.Toolkit.KryptonLabel lblModel;
private Krypton.Toolkit.KryptonTextBox txtModel;

    
        
              private Krypton.Toolkit.KryptonLabel lblType_ID;
private Krypton.Toolkit.KryptonTextBox txtType_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblproperty;
private Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private Krypton.Toolkit.KryptonLabel lblLocation_ID;
private Krypton.Toolkit.KryptonTextBox txtLocation_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblQuantity;
private Krypton.Toolkit.KryptonTextBox txtQuantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblUnitPrice;
private Krypton.Toolkit.KryptonTextBox txtUnitPrice;

    
        
              private Krypton.Toolkit.KryptonLabel lblTaxRate;
private Krypton.Toolkit.KryptonTextBox txtTaxRate;

    
        
              private Krypton.Toolkit.KryptonLabel lblTaxAmount;
private Krypton.Toolkit.KryptonTextBox txtTaxAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblSubtotalAmount;
private Krypton.Toolkit.KryptonTextBox txtSubtotalAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsGift;
private Krypton.Toolkit.KryptonCheckBox chkIsGift;

    
        
              private Krypton.Toolkit.KryptonLabel lblItemPreDeliveryDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpItemPreDeliveryDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomertModel;
private Krypton.Toolkit.KryptonTextBox txtCustomertModel;

    
        
              private Krypton.Toolkit.KryptonLabel lblDeliveredQuantity;
private Krypton.Toolkit.KryptonTextBox txtDeliveredQuantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblSummary;
private Krypton.Toolkit.KryptonTextBox txtSummary;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

