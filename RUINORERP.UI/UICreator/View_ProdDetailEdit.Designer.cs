// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:33
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 产品详情视图
    /// </summary>
    partial class View_ProdDetailEdit
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
     this.lblProdBaseID = new Krypton.Toolkit.KryptonLabel();
this.txtProdBaseID = new Krypton.Toolkit.KryptonTextBox();

this.lblProdDetailID = new Krypton.Toolkit.KryptonLabel();
this.txtProdDetailID = new Krypton.Toolkit.KryptonTextBox();

this.lblCNName = new Krypton.Toolkit.KryptonLabel();
this.txtCNName = new Krypton.Toolkit.KryptonTextBox();
this.txtCNName.Multiline = true;

this.lblSKU = new Krypton.Toolkit.KryptonLabel();
this.txtSKU = new Krypton.Toolkit.KryptonTextBox();

this.lblSpecifications = new Krypton.Toolkit.KryptonLabel();
this.txtSpecifications = new Krypton.Toolkit.KryptonTextBox();
this.txtSpecifications.Multiline = true;

this.lblQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtQuantity = new Krypton.Toolkit.KryptonTextBox();

this.lblprop = new Krypton.Toolkit.KryptonLabel();
this.txtprop = new Krypton.Toolkit.KryptonTextBox();

this.lblProductNo = new Krypton.Toolkit.KryptonLabel();
this.txtProductNo = new Krypton.Toolkit.KryptonTextBox();

this.lblUnit_ID = new Krypton.Toolkit.KryptonLabel();
this.txtUnit_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblModel = new Krypton.Toolkit.KryptonLabel();
this.txtModel = new Krypton.Toolkit.KryptonTextBox();

this.lblCategory_ID = new Krypton.Toolkit.KryptonLabel();
this.txtCategory_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblCustomerVendor_ID = new Krypton.Toolkit.KryptonLabel();
this.txtCustomerVendor_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblDepartmentID = new Krypton.Toolkit.KryptonLabel();
this.txtDepartmentID = new Krypton.Toolkit.KryptonTextBox();

this.lblENName = new Krypton.Toolkit.KryptonLabel();
this.txtENName = new Krypton.Toolkit.KryptonTextBox();
this.txtENName.Multiline = true;

this.lblBrand = new Krypton.Toolkit.KryptonLabel();
this.txtBrand = new Krypton.Toolkit.KryptonTextBox();

this.lblVendorModelCode = new Krypton.Toolkit.KryptonLabel();
this.txtVendorModelCode = new Krypton.Toolkit.KryptonTextBox();


this.lblLocation_ID = new Krypton.Toolkit.KryptonLabel();
this.txtLocation_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblRack_ID = new Krypton.Toolkit.KryptonLabel();
this.txtRack_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblOn_the_way_Qty = new Krypton.Toolkit.KryptonLabel();
this.txtOn_the_way_Qty = new Krypton.Toolkit.KryptonTextBox();

this.lblSale_Qty = new Krypton.Toolkit.KryptonLabel();
this.txtSale_Qty = new Krypton.Toolkit.KryptonTextBox();

this.lblAlert_Quantity = new Krypton.Toolkit.KryptonLabel();
this.txtAlert_Quantity = new Krypton.Toolkit.KryptonTextBox();

this.lblMakingQty = new Krypton.Toolkit.KryptonLabel();
this.txtMakingQty = new Krypton.Toolkit.KryptonTextBox();

this.lblNotOutQty = new Krypton.Toolkit.KryptonLabel();
this.txtNotOutQty = new Krypton.Toolkit.KryptonTextBox();

this.lblIs_available = new Krypton.Toolkit.KryptonLabel();
this.chkIs_available = new Krypton.Toolkit.KryptonCheckBox();
this.chkIs_available.Values.Text ="";

this.lblIs_enabled = new Krypton.Toolkit.KryptonLabel();
this.chkIs_enabled = new Krypton.Toolkit.KryptonCheckBox();
this.chkIs_enabled.Values.Text ="";

this.lbl产品可用 = new Krypton.Toolkit.KryptonLabel();
this.chk产品可用 = new Krypton.Toolkit.KryptonCheckBox();
this.chk产品可用.Values.Text ="";

this.lbl产品启用 = new Krypton.Toolkit.KryptonLabel();
this.chk产品启用 = new Krypton.Toolkit.KryptonCheckBox();
this.chk产品启用.Values.Text ="";

this.lblSKU可用 = new Krypton.Toolkit.KryptonLabel();
this.chkSKU可用 = new Krypton.Toolkit.KryptonCheckBox();
this.chkSKU可用.Values.Text ="";

this.lblSKU启用 = new Krypton.Toolkit.KryptonLabel();
this.chkSKU启用 = new Krypton.Toolkit.KryptonCheckBox();
this.chkSKU启用.Values.Text ="";

this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblType_ID = new Krypton.Toolkit.KryptonLabel();
this.txtType_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblSalePublish = new Krypton.Toolkit.KryptonLabel();
this.chkSalePublish = new Krypton.Toolkit.KryptonCheckBox();
this.chkSalePublish.Values.Text ="";

this.lblShortCode = new Krypton.Toolkit.KryptonLabel();
this.txtShortCode = new Krypton.Toolkit.KryptonTextBox();

this.lblSourceType = new Krypton.Toolkit.KryptonLabel();
this.txtSourceType = new Krypton.Toolkit.KryptonTextBox();

this.lblBarCode = new Krypton.Toolkit.KryptonLabel();
this.txtBarCode = new Krypton.Toolkit.KryptonTextBox();

this.lblInv_Cost = new Krypton.Toolkit.KryptonLabel();
this.txtInv_Cost = new Krypton.Toolkit.KryptonTextBox();

this.lblStandard_Price = new Krypton.Toolkit.KryptonLabel();
this.txtStandard_Price = new Krypton.Toolkit.KryptonTextBox();

this.lblDiscount_price = new Krypton.Toolkit.KryptonLabel();
this.txtDiscount_price = new Krypton.Toolkit.KryptonTextBox();

this.lblMarket_price = new Krypton.Toolkit.KryptonLabel();
this.txtMarket_price = new Krypton.Toolkit.KryptonTextBox();

this.lblWholesale_Price = new Krypton.Toolkit.KryptonLabel();
this.txtWholesale_Price = new Krypton.Toolkit.KryptonTextBox();

this.lblTransfer_price = new Krypton.Toolkit.KryptonLabel();
this.txtTransfer_price = new Krypton.Toolkit.KryptonTextBox();

this.lblWeight = new Krypton.Toolkit.KryptonLabel();
this.txtWeight = new Krypton.Toolkit.KryptonTextBox();

this.lblBOM_ID = new Krypton.Toolkit.KryptonLabel();
this.txtBOM_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblLatestStorageTime = new Krypton.Toolkit.KryptonLabel();
this.dtpLatestStorageTime = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblLatestOutboundTime = new Krypton.Toolkit.KryptonLabel();
this.dtpLatestOutboundTime = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblLastInventoryDate = new Krypton.Toolkit.KryptonLabel();
this.dtpLastInventoryDate = new Krypton.Toolkit.KryptonDateTimePicker();

    
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
     
            //#####ProdBaseID###Int64
this.lblProdBaseID.AutoSize = true;
this.lblProdBaseID.Location = new System.Drawing.Point(100,25);
this.lblProdBaseID.Name = "lblProdBaseID";
this.lblProdBaseID.Size = new System.Drawing.Size(41, 12);
this.lblProdBaseID.TabIndex = 1;
this.lblProdBaseID.Text = "";
this.txtProdBaseID.Location = new System.Drawing.Point(173,21);
this.txtProdBaseID.Name = "txtProdBaseID";
this.txtProdBaseID.Size = new System.Drawing.Size(100, 21);
this.txtProdBaseID.TabIndex = 1;
this.Controls.Add(this.lblProdBaseID);
this.Controls.Add(this.txtProdBaseID);

           //#####ProdDetailID###Int64
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,50);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 2;
this.lblProdDetailID.Text = "";
this.txtProdDetailID.Location = new System.Drawing.Point(173,46);
this.txtProdDetailID.Name = "txtProdDetailID";
this.txtProdDetailID.Size = new System.Drawing.Size(100, 21);
this.txtProdDetailID.TabIndex = 2;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.txtProdDetailID);

           //#####255CNName###String
this.lblCNName.AutoSize = true;
this.lblCNName.Location = new System.Drawing.Point(100,75);
this.lblCNName.Name = "lblCNName";
this.lblCNName.Size = new System.Drawing.Size(41, 12);
this.lblCNName.TabIndex = 3;
this.lblCNName.Text = "";
this.txtCNName.Location = new System.Drawing.Point(173,71);
this.txtCNName.Name = "txtCNName";
this.txtCNName.Size = new System.Drawing.Size(100, 21);
this.txtCNName.TabIndex = 3;
this.Controls.Add(this.lblCNName);
this.Controls.Add(this.txtCNName);

           //#####80SKU###String
this.lblSKU.AutoSize = true;
this.lblSKU.Location = new System.Drawing.Point(100,100);
this.lblSKU.Name = "lblSKU";
this.lblSKU.Size = new System.Drawing.Size(41, 12);
this.lblSKU.TabIndex = 4;
this.lblSKU.Text = "";
this.txtSKU.Location = new System.Drawing.Point(173,96);
this.txtSKU.Name = "txtSKU";
this.txtSKU.Size = new System.Drawing.Size(100, 21);
this.txtSKU.TabIndex = 4;
this.Controls.Add(this.lblSKU);
this.Controls.Add(this.txtSKU);

           //#####1000Specifications###String
this.lblSpecifications.AutoSize = true;
this.lblSpecifications.Location = new System.Drawing.Point(100,125);
this.lblSpecifications.Name = "lblSpecifications";
this.lblSpecifications.Size = new System.Drawing.Size(41, 12);
this.lblSpecifications.TabIndex = 5;
this.lblSpecifications.Text = "";
this.txtSpecifications.Location = new System.Drawing.Point(173,121);
this.txtSpecifications.Name = "txtSpecifications";
this.txtSpecifications.Size = new System.Drawing.Size(100, 21);
this.txtSpecifications.TabIndex = 5;
this.Controls.Add(this.lblSpecifications);
this.Controls.Add(this.txtSpecifications);

           //#####Quantity###Int32
this.lblQuantity.AutoSize = true;
this.lblQuantity.Location = new System.Drawing.Point(100,150);
this.lblQuantity.Name = "lblQuantity";
this.lblQuantity.Size = new System.Drawing.Size(41, 12);
this.lblQuantity.TabIndex = 6;
this.lblQuantity.Text = "";
this.txtQuantity.Location = new System.Drawing.Point(173,146);
this.txtQuantity.Name = "txtQuantity";
this.txtQuantity.Size = new System.Drawing.Size(100, 21);
this.txtQuantity.TabIndex = 6;
this.Controls.Add(this.lblQuantity);
this.Controls.Add(this.txtQuantity);

           //#####-1prop###String
this.lblprop.AutoSize = true;
this.lblprop.Location = new System.Drawing.Point(100,175);
this.lblprop.Name = "lblprop";
this.lblprop.Size = new System.Drawing.Size(41, 12);
this.lblprop.TabIndex = 7;
this.lblprop.Text = "";
this.txtprop.Location = new System.Drawing.Point(173,171);
this.txtprop.Name = "txtprop";
this.txtprop.Size = new System.Drawing.Size(100, 21);
this.txtprop.TabIndex = 7;
this.Controls.Add(this.lblprop);
this.Controls.Add(this.txtprop);

           //#####40ProductNo###String
this.lblProductNo.AutoSize = true;
this.lblProductNo.Location = new System.Drawing.Point(100,200);
this.lblProductNo.Name = "lblProductNo";
this.lblProductNo.Size = new System.Drawing.Size(41, 12);
this.lblProductNo.TabIndex = 8;
this.lblProductNo.Text = "";
this.txtProductNo.Location = new System.Drawing.Point(173,196);
this.txtProductNo.Name = "txtProductNo";
this.txtProductNo.Size = new System.Drawing.Size(100, 21);
this.txtProductNo.TabIndex = 8;
this.Controls.Add(this.lblProductNo);
this.Controls.Add(this.txtProductNo);

           //#####Unit_ID###Int64
this.lblUnit_ID.AutoSize = true;
this.lblUnit_ID.Location = new System.Drawing.Point(100,225);
this.lblUnit_ID.Name = "lblUnit_ID";
this.lblUnit_ID.Size = new System.Drawing.Size(41, 12);
this.lblUnit_ID.TabIndex = 9;
this.lblUnit_ID.Text = "";
this.txtUnit_ID.Location = new System.Drawing.Point(173,221);
this.txtUnit_ID.Name = "txtUnit_ID";
this.txtUnit_ID.Size = new System.Drawing.Size(100, 21);
this.txtUnit_ID.TabIndex = 9;
this.Controls.Add(this.lblUnit_ID);
this.Controls.Add(this.txtUnit_ID);

           //#####50Model###String
this.lblModel.AutoSize = true;
this.lblModel.Location = new System.Drawing.Point(100,250);
this.lblModel.Name = "lblModel";
this.lblModel.Size = new System.Drawing.Size(41, 12);
this.lblModel.TabIndex = 10;
this.lblModel.Text = "";
this.txtModel.Location = new System.Drawing.Point(173,246);
this.txtModel.Name = "txtModel";
this.txtModel.Size = new System.Drawing.Size(100, 21);
this.txtModel.TabIndex = 10;
this.Controls.Add(this.lblModel);
this.Controls.Add(this.txtModel);

           //#####Category_ID###Int64
this.lblCategory_ID.AutoSize = true;
this.lblCategory_ID.Location = new System.Drawing.Point(100,275);
this.lblCategory_ID.Name = "lblCategory_ID";
this.lblCategory_ID.Size = new System.Drawing.Size(41, 12);
this.lblCategory_ID.TabIndex = 11;
this.lblCategory_ID.Text = "";
this.txtCategory_ID.Location = new System.Drawing.Point(173,271);
this.txtCategory_ID.Name = "txtCategory_ID";
this.txtCategory_ID.Size = new System.Drawing.Size(100, 21);
this.txtCategory_ID.TabIndex = 11;
this.Controls.Add(this.lblCategory_ID);
this.Controls.Add(this.txtCategory_ID);

           //#####CustomerVendor_ID###Int64
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,300);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 12;
this.lblCustomerVendor_ID.Text = "";
this.txtCustomerVendor_ID.Location = new System.Drawing.Point(173,296);
this.txtCustomerVendor_ID.Name = "txtCustomerVendor_ID";
this.txtCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.txtCustomerVendor_ID.TabIndex = 12;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.txtCustomerVendor_ID);

           //#####DepartmentID###Int64
this.lblDepartmentID.AutoSize = true;
this.lblDepartmentID.Location = new System.Drawing.Point(100,325);
this.lblDepartmentID.Name = "lblDepartmentID";
this.lblDepartmentID.Size = new System.Drawing.Size(41, 12);
this.lblDepartmentID.TabIndex = 13;
this.lblDepartmentID.Text = "";
this.txtDepartmentID.Location = new System.Drawing.Point(173,321);
this.txtDepartmentID.Name = "txtDepartmentID";
this.txtDepartmentID.Size = new System.Drawing.Size(100, 21);
this.txtDepartmentID.TabIndex = 13;
this.Controls.Add(this.lblDepartmentID);
this.Controls.Add(this.txtDepartmentID);

           //#####255ENName###String
this.lblENName.AutoSize = true;
this.lblENName.Location = new System.Drawing.Point(100,350);
this.lblENName.Name = "lblENName";
this.lblENName.Size = new System.Drawing.Size(41, 12);
this.lblENName.TabIndex = 14;
this.lblENName.Text = "";
this.txtENName.Location = new System.Drawing.Point(173,346);
this.txtENName.Name = "txtENName";
this.txtENName.Size = new System.Drawing.Size(100, 21);
this.txtENName.TabIndex = 14;
this.Controls.Add(this.lblENName);
this.Controls.Add(this.txtENName);

           //#####50Brand###String
this.lblBrand.AutoSize = true;
this.lblBrand.Location = new System.Drawing.Point(100,375);
this.lblBrand.Name = "lblBrand";
this.lblBrand.Size = new System.Drawing.Size(41, 12);
this.lblBrand.TabIndex = 15;
this.lblBrand.Text = "";
this.txtBrand.Location = new System.Drawing.Point(173,371);
this.txtBrand.Name = "txtBrand";
this.txtBrand.Size = new System.Drawing.Size(100, 21);
this.txtBrand.TabIndex = 15;
this.Controls.Add(this.lblBrand);
this.Controls.Add(this.txtBrand);

           //#####50VendorModelCode###String
this.lblVendorModelCode.AutoSize = true;
this.lblVendorModelCode.Location = new System.Drawing.Point(100,400);
this.lblVendorModelCode.Name = "lblVendorModelCode";
this.lblVendorModelCode.Size = new System.Drawing.Size(41, 12);
this.lblVendorModelCode.TabIndex = 16;
this.lblVendorModelCode.Text = "";
this.txtVendorModelCode.Location = new System.Drawing.Point(173,396);
this.txtVendorModelCode.Name = "txtVendorModelCode";
this.txtVendorModelCode.Size = new System.Drawing.Size(100, 21);
this.txtVendorModelCode.TabIndex = 16;
this.Controls.Add(this.lblVendorModelCode);
this.Controls.Add(this.txtVendorModelCode);

           //#####2147483647Images###Binary

           //#####Location_ID###Int64
this.lblLocation_ID.AutoSize = true;
this.lblLocation_ID.Location = new System.Drawing.Point(100,450);
this.lblLocation_ID.Name = "lblLocation_ID";
this.lblLocation_ID.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID.TabIndex = 18;
this.lblLocation_ID.Text = "";
this.txtLocation_ID.Location = new System.Drawing.Point(173,446);
this.txtLocation_ID.Name = "txtLocation_ID";
this.txtLocation_ID.Size = new System.Drawing.Size(100, 21);
this.txtLocation_ID.TabIndex = 18;
this.Controls.Add(this.lblLocation_ID);
this.Controls.Add(this.txtLocation_ID);

           //#####Rack_ID###Int64
this.lblRack_ID.AutoSize = true;
this.lblRack_ID.Location = new System.Drawing.Point(100,475);
this.lblRack_ID.Name = "lblRack_ID";
this.lblRack_ID.Size = new System.Drawing.Size(41, 12);
this.lblRack_ID.TabIndex = 19;
this.lblRack_ID.Text = "";
this.txtRack_ID.Location = new System.Drawing.Point(173,471);
this.txtRack_ID.Name = "txtRack_ID";
this.txtRack_ID.Size = new System.Drawing.Size(100, 21);
this.txtRack_ID.TabIndex = 19;
this.Controls.Add(this.lblRack_ID);
this.Controls.Add(this.txtRack_ID);

           //#####On_the_way_Qty###Int32
this.lblOn_the_way_Qty.AutoSize = true;
this.lblOn_the_way_Qty.Location = new System.Drawing.Point(100,500);
this.lblOn_the_way_Qty.Name = "lblOn_the_way_Qty";
this.lblOn_the_way_Qty.Size = new System.Drawing.Size(41, 12);
this.lblOn_the_way_Qty.TabIndex = 20;
this.lblOn_the_way_Qty.Text = "";
this.txtOn_the_way_Qty.Location = new System.Drawing.Point(173,496);
this.txtOn_the_way_Qty.Name = "txtOn_the_way_Qty";
this.txtOn_the_way_Qty.Size = new System.Drawing.Size(100, 21);
this.txtOn_the_way_Qty.TabIndex = 20;
this.Controls.Add(this.lblOn_the_way_Qty);
this.Controls.Add(this.txtOn_the_way_Qty);

           //#####Sale_Qty###Int32
this.lblSale_Qty.AutoSize = true;
this.lblSale_Qty.Location = new System.Drawing.Point(100,525);
this.lblSale_Qty.Name = "lblSale_Qty";
this.lblSale_Qty.Size = new System.Drawing.Size(41, 12);
this.lblSale_Qty.TabIndex = 21;
this.lblSale_Qty.Text = "";
this.txtSale_Qty.Location = new System.Drawing.Point(173,521);
this.txtSale_Qty.Name = "txtSale_Qty";
this.txtSale_Qty.Size = new System.Drawing.Size(100, 21);
this.txtSale_Qty.TabIndex = 21;
this.Controls.Add(this.lblSale_Qty);
this.Controls.Add(this.txtSale_Qty);

           //#####Alert_Quantity###Int32
this.lblAlert_Quantity.AutoSize = true;
this.lblAlert_Quantity.Location = new System.Drawing.Point(100,550);
this.lblAlert_Quantity.Name = "lblAlert_Quantity";
this.lblAlert_Quantity.Size = new System.Drawing.Size(41, 12);
this.lblAlert_Quantity.TabIndex = 22;
this.lblAlert_Quantity.Text = "";
this.txtAlert_Quantity.Location = new System.Drawing.Point(173,546);
this.txtAlert_Quantity.Name = "txtAlert_Quantity";
this.txtAlert_Quantity.Size = new System.Drawing.Size(100, 21);
this.txtAlert_Quantity.TabIndex = 22;
this.Controls.Add(this.lblAlert_Quantity);
this.Controls.Add(this.txtAlert_Quantity);

           //#####MakingQty###Int32
this.lblMakingQty.AutoSize = true;
this.lblMakingQty.Location = new System.Drawing.Point(100,575);
this.lblMakingQty.Name = "lblMakingQty";
this.lblMakingQty.Size = new System.Drawing.Size(41, 12);
this.lblMakingQty.TabIndex = 23;
this.lblMakingQty.Text = "";
this.txtMakingQty.Location = new System.Drawing.Point(173,571);
this.txtMakingQty.Name = "txtMakingQty";
this.txtMakingQty.Size = new System.Drawing.Size(100, 21);
this.txtMakingQty.TabIndex = 23;
this.Controls.Add(this.lblMakingQty);
this.Controls.Add(this.txtMakingQty);

           //#####NotOutQty###Int32
this.lblNotOutQty.AutoSize = true;
this.lblNotOutQty.Location = new System.Drawing.Point(100,600);
this.lblNotOutQty.Name = "lblNotOutQty";
this.lblNotOutQty.Size = new System.Drawing.Size(41, 12);
this.lblNotOutQty.TabIndex = 24;
this.lblNotOutQty.Text = "";
this.txtNotOutQty.Location = new System.Drawing.Point(173,596);
this.txtNotOutQty.Name = "txtNotOutQty";
this.txtNotOutQty.Size = new System.Drawing.Size(100, 21);
this.txtNotOutQty.TabIndex = 24;
this.Controls.Add(this.lblNotOutQty);
this.Controls.Add(this.txtNotOutQty);

           //#####Is_available###Boolean
this.lblIs_available.AutoSize = true;
this.lblIs_available.Location = new System.Drawing.Point(100,625);
this.lblIs_available.Name = "lblIs_available";
this.lblIs_available.Size = new System.Drawing.Size(41, 12);
this.lblIs_available.TabIndex = 25;
this.lblIs_available.Text = "";
this.chkIs_available.Location = new System.Drawing.Point(173,621);
this.chkIs_available.Name = "chkIs_available";
this.chkIs_available.Size = new System.Drawing.Size(100, 21);
this.chkIs_available.TabIndex = 25;
this.Controls.Add(this.lblIs_available);
this.Controls.Add(this.chkIs_available);

           //#####Is_enabled###Boolean
this.lblIs_enabled.AutoSize = true;
this.lblIs_enabled.Location = new System.Drawing.Point(100,650);
this.lblIs_enabled.Name = "lblIs_enabled";
this.lblIs_enabled.Size = new System.Drawing.Size(41, 12);
this.lblIs_enabled.TabIndex = 26;
this.lblIs_enabled.Text = "";
this.chkIs_enabled.Location = new System.Drawing.Point(173,646);
this.chkIs_enabled.Name = "chkIs_enabled";
this.chkIs_enabled.Size = new System.Drawing.Size(100, 21);
this.chkIs_enabled.TabIndex = 26;
this.Controls.Add(this.lblIs_enabled);
this.Controls.Add(this.chkIs_enabled);

           //#####产品可用###Boolean
this.lbl产品可用.AutoSize = true;
this.lbl产品可用.Location = new System.Drawing.Point(100,675);
this.lbl产品可用.Name = "lbl产品可用";
this.lbl产品可用.Size = new System.Drawing.Size(41, 12);
this.lbl产品可用.TabIndex = 27;
this.lbl产品可用.Text = "";
this.chk产品可用.Location = new System.Drawing.Point(173,671);
this.chk产品可用.Name = "chk产品可用";
this.chk产品可用.Size = new System.Drawing.Size(100, 21);
this.chk产品可用.TabIndex = 27;
this.Controls.Add(this.lbl产品可用);
this.Controls.Add(this.chk产品可用);

           //#####产品启用###Boolean
this.lbl产品启用.AutoSize = true;
this.lbl产品启用.Location = new System.Drawing.Point(100,700);
this.lbl产品启用.Name = "lbl产品启用";
this.lbl产品启用.Size = new System.Drawing.Size(41, 12);
this.lbl产品启用.TabIndex = 28;
this.lbl产品启用.Text = "";
this.chk产品启用.Location = new System.Drawing.Point(173,696);
this.chk产品启用.Name = "chk产品启用";
this.chk产品启用.Size = new System.Drawing.Size(100, 21);
this.chk产品启用.TabIndex = 28;
this.Controls.Add(this.lbl产品启用);
this.Controls.Add(this.chk产品启用);

           //#####SKU可用###Boolean
this.lblSKU可用.AutoSize = true;
this.lblSKU可用.Location = new System.Drawing.Point(100,725);
this.lblSKU可用.Name = "lblSKU可用";
this.lblSKU可用.Size = new System.Drawing.Size(41, 12);
this.lblSKU可用.TabIndex = 29;
this.lblSKU可用.Text = "";
this.chkSKU可用.Location = new System.Drawing.Point(173,721);
this.chkSKU可用.Name = "chkSKU可用";
this.chkSKU可用.Size = new System.Drawing.Size(100, 21);
this.chkSKU可用.TabIndex = 29;
this.Controls.Add(this.lblSKU可用);
this.Controls.Add(this.chkSKU可用);

           //#####SKU启用###Boolean
this.lblSKU启用.AutoSize = true;
this.lblSKU启用.Location = new System.Drawing.Point(100,750);
this.lblSKU启用.Name = "lblSKU启用";
this.lblSKU启用.Size = new System.Drawing.Size(41, 12);
this.lblSKU启用.TabIndex = 30;
this.lblSKU启用.Text = "";
this.chkSKU启用.Location = new System.Drawing.Point(173,746);
this.chkSKU启用.Name = "chkSKU启用";
this.chkSKU启用.Size = new System.Drawing.Size(100, 21);
this.chkSKU启用.TabIndex = 30;
this.Controls.Add(this.lblSKU启用);
this.Controls.Add(this.chkSKU启用);

           //#####255Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,775);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 31;
this.lblNotes.Text = "";
this.txtNotes.Location = new System.Drawing.Point(173,771);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 31;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####Type_ID###Int64
this.lblType_ID.AutoSize = true;
this.lblType_ID.Location = new System.Drawing.Point(100,800);
this.lblType_ID.Name = "lblType_ID";
this.lblType_ID.Size = new System.Drawing.Size(41, 12);
this.lblType_ID.TabIndex = 32;
this.lblType_ID.Text = "";
this.txtType_ID.Location = new System.Drawing.Point(173,796);
this.txtType_ID.Name = "txtType_ID";
this.txtType_ID.Size = new System.Drawing.Size(100, 21);
this.txtType_ID.TabIndex = 32;
this.Controls.Add(this.lblType_ID);
this.Controls.Add(this.txtType_ID);

           //#####SalePublish###Boolean
this.lblSalePublish.AutoSize = true;
this.lblSalePublish.Location = new System.Drawing.Point(100,825);
this.lblSalePublish.Name = "lblSalePublish";
this.lblSalePublish.Size = new System.Drawing.Size(41, 12);
this.lblSalePublish.TabIndex = 33;
this.lblSalePublish.Text = "";
this.chkSalePublish.Location = new System.Drawing.Point(173,821);
this.chkSalePublish.Name = "chkSalePublish";
this.chkSalePublish.Size = new System.Drawing.Size(100, 21);
this.chkSalePublish.TabIndex = 33;
this.Controls.Add(this.lblSalePublish);
this.Controls.Add(this.chkSalePublish);

           //#####50ShortCode###String
this.lblShortCode.AutoSize = true;
this.lblShortCode.Location = new System.Drawing.Point(100,850);
this.lblShortCode.Name = "lblShortCode";
this.lblShortCode.Size = new System.Drawing.Size(41, 12);
this.lblShortCode.TabIndex = 34;
this.lblShortCode.Text = "";
this.txtShortCode.Location = new System.Drawing.Point(173,846);
this.txtShortCode.Name = "txtShortCode";
this.txtShortCode.Size = new System.Drawing.Size(100, 21);
this.txtShortCode.TabIndex = 34;
this.Controls.Add(this.lblShortCode);
this.Controls.Add(this.txtShortCode);

           //#####SourceType###Int32
this.lblSourceType.AutoSize = true;
this.lblSourceType.Location = new System.Drawing.Point(100,875);
this.lblSourceType.Name = "lblSourceType";
this.lblSourceType.Size = new System.Drawing.Size(41, 12);
this.lblSourceType.TabIndex = 35;
this.lblSourceType.Text = "";
this.txtSourceType.Location = new System.Drawing.Point(173,871);
this.txtSourceType.Name = "txtSourceType";
this.txtSourceType.Size = new System.Drawing.Size(100, 21);
this.txtSourceType.TabIndex = 35;
this.Controls.Add(this.lblSourceType);
this.Controls.Add(this.txtSourceType);

           //#####50BarCode###String
this.lblBarCode.AutoSize = true;
this.lblBarCode.Location = new System.Drawing.Point(100,900);
this.lblBarCode.Name = "lblBarCode";
this.lblBarCode.Size = new System.Drawing.Size(41, 12);
this.lblBarCode.TabIndex = 36;
this.lblBarCode.Text = "";
this.txtBarCode.Location = new System.Drawing.Point(173,896);
this.txtBarCode.Name = "txtBarCode";
this.txtBarCode.Size = new System.Drawing.Size(100, 21);
this.txtBarCode.TabIndex = 36;
this.Controls.Add(this.lblBarCode);
this.Controls.Add(this.txtBarCode);

           //#####Inv_Cost###Decimal
this.lblInv_Cost.AutoSize = true;
this.lblInv_Cost.Location = new System.Drawing.Point(100,925);
this.lblInv_Cost.Name = "lblInv_Cost";
this.lblInv_Cost.Size = new System.Drawing.Size(41, 12);
this.lblInv_Cost.TabIndex = 37;
this.lblInv_Cost.Text = "";
//111======925
this.txtInv_Cost.Location = new System.Drawing.Point(173,921);
this.txtInv_Cost.Name ="txtInv_Cost";
this.txtInv_Cost.Size = new System.Drawing.Size(100, 21);
this.txtInv_Cost.TabIndex = 37;
this.Controls.Add(this.lblInv_Cost);
this.Controls.Add(this.txtInv_Cost);

           //#####Standard_Price###Decimal
this.lblStandard_Price.AutoSize = true;
this.lblStandard_Price.Location = new System.Drawing.Point(100,950);
this.lblStandard_Price.Name = "lblStandard_Price";
this.lblStandard_Price.Size = new System.Drawing.Size(41, 12);
this.lblStandard_Price.TabIndex = 38;
this.lblStandard_Price.Text = "";
//111======950
this.txtStandard_Price.Location = new System.Drawing.Point(173,946);
this.txtStandard_Price.Name ="txtStandard_Price";
this.txtStandard_Price.Size = new System.Drawing.Size(100, 21);
this.txtStandard_Price.TabIndex = 38;
this.Controls.Add(this.lblStandard_Price);
this.Controls.Add(this.txtStandard_Price);

           //#####Discount_price###Decimal
this.lblDiscount_price.AutoSize = true;
this.lblDiscount_price.Location = new System.Drawing.Point(100,975);
this.lblDiscount_price.Name = "lblDiscount_price";
this.lblDiscount_price.Size = new System.Drawing.Size(41, 12);
this.lblDiscount_price.TabIndex = 39;
this.lblDiscount_price.Text = "";
//111======975
this.txtDiscount_price.Location = new System.Drawing.Point(173,971);
this.txtDiscount_price.Name ="txtDiscount_price";
this.txtDiscount_price.Size = new System.Drawing.Size(100, 21);
this.txtDiscount_price.TabIndex = 39;
this.Controls.Add(this.lblDiscount_price);
this.Controls.Add(this.txtDiscount_price);

           //#####Market_price###Decimal
this.lblMarket_price.AutoSize = true;
this.lblMarket_price.Location = new System.Drawing.Point(100,1000);
this.lblMarket_price.Name = "lblMarket_price";
this.lblMarket_price.Size = new System.Drawing.Size(41, 12);
this.lblMarket_price.TabIndex = 40;
this.lblMarket_price.Text = "";
//111======1000
this.txtMarket_price.Location = new System.Drawing.Point(173,996);
this.txtMarket_price.Name ="txtMarket_price";
this.txtMarket_price.Size = new System.Drawing.Size(100, 21);
this.txtMarket_price.TabIndex = 40;
this.Controls.Add(this.lblMarket_price);
this.Controls.Add(this.txtMarket_price);

           //#####Wholesale_Price###Decimal
this.lblWholesale_Price.AutoSize = true;
this.lblWholesale_Price.Location = new System.Drawing.Point(100,1025);
this.lblWholesale_Price.Name = "lblWholesale_Price";
this.lblWholesale_Price.Size = new System.Drawing.Size(41, 12);
this.lblWholesale_Price.TabIndex = 41;
this.lblWholesale_Price.Text = "";
//111======1025
this.txtWholesale_Price.Location = new System.Drawing.Point(173,1021);
this.txtWholesale_Price.Name ="txtWholesale_Price";
this.txtWholesale_Price.Size = new System.Drawing.Size(100, 21);
this.txtWholesale_Price.TabIndex = 41;
this.Controls.Add(this.lblWholesale_Price);
this.Controls.Add(this.txtWholesale_Price);

           //#####Transfer_price###Decimal
this.lblTransfer_price.AutoSize = true;
this.lblTransfer_price.Location = new System.Drawing.Point(100,1050);
this.lblTransfer_price.Name = "lblTransfer_price";
this.lblTransfer_price.Size = new System.Drawing.Size(41, 12);
this.lblTransfer_price.TabIndex = 42;
this.lblTransfer_price.Text = "";
//111======1050
this.txtTransfer_price.Location = new System.Drawing.Point(173,1046);
this.txtTransfer_price.Name ="txtTransfer_price";
this.txtTransfer_price.Size = new System.Drawing.Size(100, 21);
this.txtTransfer_price.TabIndex = 42;
this.Controls.Add(this.lblTransfer_price);
this.Controls.Add(this.txtTransfer_price);

           //#####Weight###Decimal
this.lblWeight.AutoSize = true;
this.lblWeight.Location = new System.Drawing.Point(100,1075);
this.lblWeight.Name = "lblWeight";
this.lblWeight.Size = new System.Drawing.Size(41, 12);
this.lblWeight.TabIndex = 43;
this.lblWeight.Text = "";
//111======1075
this.txtWeight.Location = new System.Drawing.Point(173,1071);
this.txtWeight.Name ="txtWeight";
this.txtWeight.Size = new System.Drawing.Size(100, 21);
this.txtWeight.TabIndex = 43;
this.Controls.Add(this.lblWeight);
this.Controls.Add(this.txtWeight);

           //#####BOM_ID###Int64
this.lblBOM_ID.AutoSize = true;
this.lblBOM_ID.Location = new System.Drawing.Point(100,1100);
this.lblBOM_ID.Name = "lblBOM_ID";
this.lblBOM_ID.Size = new System.Drawing.Size(41, 12);
this.lblBOM_ID.TabIndex = 44;
this.lblBOM_ID.Text = "";
this.txtBOM_ID.Location = new System.Drawing.Point(173,1096);
this.txtBOM_ID.Name = "txtBOM_ID";
this.txtBOM_ID.Size = new System.Drawing.Size(100, 21);
this.txtBOM_ID.TabIndex = 44;
this.Controls.Add(this.lblBOM_ID);
this.Controls.Add(this.txtBOM_ID);

           //#####LatestStorageTime###DateTime
this.lblLatestStorageTime.AutoSize = true;
this.lblLatestStorageTime.Location = new System.Drawing.Point(100,1125);
this.lblLatestStorageTime.Name = "lblLatestStorageTime";
this.lblLatestStorageTime.Size = new System.Drawing.Size(41, 12);
this.lblLatestStorageTime.TabIndex = 45;
this.lblLatestStorageTime.Text = "";
//111======1125
this.dtpLatestStorageTime.Location = new System.Drawing.Point(173,1121);
this.dtpLatestStorageTime.Name ="dtpLatestStorageTime";
this.dtpLatestStorageTime.ShowCheckBox =true;
this.dtpLatestStorageTime.Size = new System.Drawing.Size(100, 21);
this.dtpLatestStorageTime.TabIndex = 45;
this.Controls.Add(this.lblLatestStorageTime);
this.Controls.Add(this.dtpLatestStorageTime);

           //#####LatestOutboundTime###DateTime
this.lblLatestOutboundTime.AutoSize = true;
this.lblLatestOutboundTime.Location = new System.Drawing.Point(100,1150);
this.lblLatestOutboundTime.Name = "lblLatestOutboundTime";
this.lblLatestOutboundTime.Size = new System.Drawing.Size(41, 12);
this.lblLatestOutboundTime.TabIndex = 46;
this.lblLatestOutboundTime.Text = "";
//111======1150
this.dtpLatestOutboundTime.Location = new System.Drawing.Point(173,1146);
this.dtpLatestOutboundTime.Name ="dtpLatestOutboundTime";
this.dtpLatestOutboundTime.ShowCheckBox =true;
this.dtpLatestOutboundTime.Size = new System.Drawing.Size(100, 21);
this.dtpLatestOutboundTime.TabIndex = 46;
this.Controls.Add(this.lblLatestOutboundTime);
this.Controls.Add(this.dtpLatestOutboundTime);

           //#####LastInventoryDate###DateTime
this.lblLastInventoryDate.AutoSize = true;
this.lblLastInventoryDate.Location = new System.Drawing.Point(100,1175);
this.lblLastInventoryDate.Name = "lblLastInventoryDate";
this.lblLastInventoryDate.Size = new System.Drawing.Size(41, 12);
this.lblLastInventoryDate.TabIndex = 47;
this.lblLastInventoryDate.Text = "";
//111======1175
this.dtpLastInventoryDate.Location = new System.Drawing.Point(173,1171);
this.dtpLastInventoryDate.Name ="dtpLastInventoryDate";
this.dtpLastInventoryDate.ShowCheckBox =true;
this.dtpLastInventoryDate.Size = new System.Drawing.Size(100, 21);
this.dtpLastInventoryDate.TabIndex = 47;
this.Controls.Add(this.lblLastInventoryDate);
this.Controls.Add(this.dtpLastInventoryDate);

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
           // this.kryptonPanel1.TabIndex = 47;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblProdBaseID );
this.Controls.Add(this.txtProdBaseID );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.txtProdDetailID );

                this.Controls.Add(this.lblCNName );
this.Controls.Add(this.txtCNName );

                this.Controls.Add(this.lblSKU );
this.Controls.Add(this.txtSKU );

                this.Controls.Add(this.lblSpecifications );
this.Controls.Add(this.txtSpecifications );

                this.Controls.Add(this.lblQuantity );
this.Controls.Add(this.txtQuantity );

                this.Controls.Add(this.lblprop );
this.Controls.Add(this.txtprop );

                this.Controls.Add(this.lblProductNo );
this.Controls.Add(this.txtProductNo );

                this.Controls.Add(this.lblUnit_ID );
this.Controls.Add(this.txtUnit_ID );

                this.Controls.Add(this.lblModel );
this.Controls.Add(this.txtModel );

                this.Controls.Add(this.lblCategory_ID );
this.Controls.Add(this.txtCategory_ID );

                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.txtCustomerVendor_ID );

                this.Controls.Add(this.lblDepartmentID );
this.Controls.Add(this.txtDepartmentID );

                this.Controls.Add(this.lblENName );
this.Controls.Add(this.txtENName );

                this.Controls.Add(this.lblBrand );
this.Controls.Add(this.txtBrand );

                this.Controls.Add(this.lblVendorModelCode );
this.Controls.Add(this.txtVendorModelCode );

                
                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.txtLocation_ID );

                this.Controls.Add(this.lblRack_ID );
this.Controls.Add(this.txtRack_ID );

                this.Controls.Add(this.lblOn_the_way_Qty );
this.Controls.Add(this.txtOn_the_way_Qty );

                this.Controls.Add(this.lblSale_Qty );
this.Controls.Add(this.txtSale_Qty );

                this.Controls.Add(this.lblAlert_Quantity );
this.Controls.Add(this.txtAlert_Quantity );

                this.Controls.Add(this.lblMakingQty );
this.Controls.Add(this.txtMakingQty );

                this.Controls.Add(this.lblNotOutQty );
this.Controls.Add(this.txtNotOutQty );

                this.Controls.Add(this.lblIs_available );
this.Controls.Add(this.chkIs_available );

                this.Controls.Add(this.lblIs_enabled );
this.Controls.Add(this.chkIs_enabled );

                this.Controls.Add(this.lbl产品可用 );
this.Controls.Add(this.chk产品可用 );

                this.Controls.Add(this.lbl产品启用 );
this.Controls.Add(this.chk产品启用 );

                this.Controls.Add(this.lblSKU可用 );
this.Controls.Add(this.chkSKU可用 );

                this.Controls.Add(this.lblSKU启用 );
this.Controls.Add(this.chkSKU启用 );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblType_ID );
this.Controls.Add(this.txtType_ID );

                this.Controls.Add(this.lblSalePublish );
this.Controls.Add(this.chkSalePublish );

                this.Controls.Add(this.lblShortCode );
this.Controls.Add(this.txtShortCode );

                this.Controls.Add(this.lblSourceType );
this.Controls.Add(this.txtSourceType );

                this.Controls.Add(this.lblBarCode );
this.Controls.Add(this.txtBarCode );

                this.Controls.Add(this.lblInv_Cost );
this.Controls.Add(this.txtInv_Cost );

                this.Controls.Add(this.lblStandard_Price );
this.Controls.Add(this.txtStandard_Price );

                this.Controls.Add(this.lblDiscount_price );
this.Controls.Add(this.txtDiscount_price );

                this.Controls.Add(this.lblMarket_price );
this.Controls.Add(this.txtMarket_price );

                this.Controls.Add(this.lblWholesale_Price );
this.Controls.Add(this.txtWholesale_Price );

                this.Controls.Add(this.lblTransfer_price );
this.Controls.Add(this.txtTransfer_price );

                this.Controls.Add(this.lblWeight );
this.Controls.Add(this.txtWeight );

                this.Controls.Add(this.lblBOM_ID );
this.Controls.Add(this.txtBOM_ID );

                this.Controls.Add(this.lblLatestStorageTime );
this.Controls.Add(this.dtpLatestStorageTime );

                this.Controls.Add(this.lblLatestOutboundTime );
this.Controls.Add(this.dtpLatestOutboundTime );

                this.Controls.Add(this.lblLastInventoryDate );
this.Controls.Add(this.dtpLastInventoryDate );

                            // 
            // "View_ProdDetailEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "View_ProdDetailEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblProdBaseID;
private Krypton.Toolkit.KryptonTextBox txtProdBaseID;

    
        
              private Krypton.Toolkit.KryptonLabel lblProdDetailID;
private Krypton.Toolkit.KryptonTextBox txtProdDetailID;

    
        
              private Krypton.Toolkit.KryptonLabel lblCNName;
private Krypton.Toolkit.KryptonTextBox txtCNName;

    
        
              private Krypton.Toolkit.KryptonLabel lblSKU;
private Krypton.Toolkit.KryptonTextBox txtSKU;

    
        
              private Krypton.Toolkit.KryptonLabel lblSpecifications;
private Krypton.Toolkit.KryptonTextBox txtSpecifications;

    
        
              private Krypton.Toolkit.KryptonLabel lblQuantity;
private Krypton.Toolkit.KryptonTextBox txtQuantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblprop;
private Krypton.Toolkit.KryptonTextBox txtprop;

    
        
              private Krypton.Toolkit.KryptonLabel lblProductNo;
private Krypton.Toolkit.KryptonTextBox txtProductNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblUnit_ID;
private Krypton.Toolkit.KryptonTextBox txtUnit_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblModel;
private Krypton.Toolkit.KryptonTextBox txtModel;

    
        
              private Krypton.Toolkit.KryptonLabel lblCategory_ID;
private Krypton.Toolkit.KryptonTextBox txtCategory_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private Krypton.Toolkit.KryptonTextBox txtCustomerVendor_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblDepartmentID;
private Krypton.Toolkit.KryptonTextBox txtDepartmentID;

    
        
              private Krypton.Toolkit.KryptonLabel lblENName;
private Krypton.Toolkit.KryptonTextBox txtENName;

    
        
              private Krypton.Toolkit.KryptonLabel lblBrand;
private Krypton.Toolkit.KryptonTextBox txtBrand;

    
        
              private Krypton.Toolkit.KryptonLabel lblVendorModelCode;
private Krypton.Toolkit.KryptonTextBox txtVendorModelCode;

    
        
              
    
        
              private Krypton.Toolkit.KryptonLabel lblLocation_ID;
private Krypton.Toolkit.KryptonTextBox txtLocation_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblRack_ID;
private Krypton.Toolkit.KryptonTextBox txtRack_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblOn_the_way_Qty;
private Krypton.Toolkit.KryptonTextBox txtOn_the_way_Qty;

    
        
              private Krypton.Toolkit.KryptonLabel lblSale_Qty;
private Krypton.Toolkit.KryptonTextBox txtSale_Qty;

    
        
              private Krypton.Toolkit.KryptonLabel lblAlert_Quantity;
private Krypton.Toolkit.KryptonTextBox txtAlert_Quantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblMakingQty;
private Krypton.Toolkit.KryptonTextBox txtMakingQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotOutQty;
private Krypton.Toolkit.KryptonTextBox txtNotOutQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblIs_available;
private Krypton.Toolkit.KryptonCheckBox chkIs_available;

    
        
              private Krypton.Toolkit.KryptonLabel lblIs_enabled;
private Krypton.Toolkit.KryptonCheckBox chkIs_enabled;

    
        
              private Krypton.Toolkit.KryptonLabel lbl产品可用;
private Krypton.Toolkit.KryptonCheckBox chk产品可用;

    
        
              private Krypton.Toolkit.KryptonLabel lbl产品启用;
private Krypton.Toolkit.KryptonCheckBox chk产品启用;

    
        
              private Krypton.Toolkit.KryptonLabel lblSKU可用;
private Krypton.Toolkit.KryptonCheckBox chkSKU可用;

    
        
              private Krypton.Toolkit.KryptonLabel lblSKU启用;
private Krypton.Toolkit.KryptonCheckBox chkSKU启用;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private Krypton.Toolkit.KryptonLabel lblType_ID;
private Krypton.Toolkit.KryptonTextBox txtType_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblSalePublish;
private Krypton.Toolkit.KryptonCheckBox chkSalePublish;

    
        
              private Krypton.Toolkit.KryptonLabel lblShortCode;
private Krypton.Toolkit.KryptonTextBox txtShortCode;

    
        
              private Krypton.Toolkit.KryptonLabel lblSourceType;
private Krypton.Toolkit.KryptonTextBox txtSourceType;

    
        
              private Krypton.Toolkit.KryptonLabel lblBarCode;
private Krypton.Toolkit.KryptonTextBox txtBarCode;

    
        
              private Krypton.Toolkit.KryptonLabel lblInv_Cost;
private Krypton.Toolkit.KryptonTextBox txtInv_Cost;

    
        
              private Krypton.Toolkit.KryptonLabel lblStandard_Price;
private Krypton.Toolkit.KryptonTextBox txtStandard_Price;

    
        
              private Krypton.Toolkit.KryptonLabel lblDiscount_price;
private Krypton.Toolkit.KryptonTextBox txtDiscount_price;

    
        
              private Krypton.Toolkit.KryptonLabel lblMarket_price;
private Krypton.Toolkit.KryptonTextBox txtMarket_price;

    
        
              private Krypton.Toolkit.KryptonLabel lblWholesale_Price;
private Krypton.Toolkit.KryptonTextBox txtWholesale_Price;

    
        
              private Krypton.Toolkit.KryptonLabel lblTransfer_price;
private Krypton.Toolkit.KryptonTextBox txtTransfer_price;

    
        
              private Krypton.Toolkit.KryptonLabel lblWeight;
private Krypton.Toolkit.KryptonTextBox txtWeight;

    
        
              private Krypton.Toolkit.KryptonLabel lblBOM_ID;
private Krypton.Toolkit.KryptonTextBox txtBOM_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblLatestStorageTime;
private Krypton.Toolkit.KryptonDateTimePicker dtpLatestStorageTime;

    
        
              private Krypton.Toolkit.KryptonLabel lblLatestOutboundTime;
private Krypton.Toolkit.KryptonDateTimePicker dtpLatestOutboundTime;

    
        
              private Krypton.Toolkit.KryptonLabel lblLastInventoryDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpLastInventoryDate;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

