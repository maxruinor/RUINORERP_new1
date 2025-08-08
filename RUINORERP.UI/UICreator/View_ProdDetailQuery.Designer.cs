
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
    partial class View_ProdDetailQuery
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
     
     

this.lblCNName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCNName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtCNName.Multiline = true;

this.lblSKU = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSKU = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSpecifications = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSpecifications = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSpecifications.Multiline = true;


this.lblprop = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtprop = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblProductNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtProductNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblModel = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtModel = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();




this.lblENName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtENName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtENName.Multiline = true;

this.lblBrand = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBrand = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblVendorModelCode = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtVendorModelCode = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();









this.lblIs_available = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIs_available = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIs_available.Values.Text ="";

this.lblIs_enabled = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIs_enabled = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIs_enabled.Values.Text ="";

this.lbl产品可用 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chk产品可用 = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chk产品可用.Values.Text ="";

this.lbl产品启用 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chk产品启用 = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chk产品启用.Values.Text ="";

this.lblSKU可用 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkSKU可用 = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkSKU可用.Values.Text ="";

this.lblSKU启用 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkSKU启用 = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkSKU启用.Values.Text ="";

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;


this.lblSalePublish = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkSalePublish = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkSalePublish.Values.Text ="";

this.lblShortCode = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtShortCode = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblBarCode = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBarCode = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblInv_Cost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtInv_Cost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblStandard_Price = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtStandard_Price = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblDiscount_price = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDiscount_price = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblMarket_price = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtMarket_price = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblWholesale_Price = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtWholesale_Price = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTransfer_price = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTransfer_price = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblWeight = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtWeight = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblLatestStorageTime = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpLatestStorageTime = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblLatestOutboundTime = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpLatestOutboundTime = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblLastInventoryDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpLastInventoryDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####ProdBaseID###Int64

           //#####ProdDetailID###Int64

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

           //#####CustomerVendor_ID###Int64

           //#####DepartmentID###Int64

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

           //#####Rack_ID###Int64

           //#####On_the_way_Qty###Int32

           //#####Sale_Qty###Int32

           //#####Alert_Quantity###Int32

           //#####MakingQty###Int32

           //#####NotOutQty###Int32

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

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                
                
                this.Controls.Add(this.lblCNName );
this.Controls.Add(this.txtCNName );

                this.Controls.Add(this.lblSKU );
this.Controls.Add(this.txtSKU );

                this.Controls.Add(this.lblSpecifications );
this.Controls.Add(this.txtSpecifications );

                
                this.Controls.Add(this.lblprop );
this.Controls.Add(this.txtprop );

                this.Controls.Add(this.lblProductNo );
this.Controls.Add(this.txtProductNo );

                
                this.Controls.Add(this.lblModel );
this.Controls.Add(this.txtModel );

                
                
                
                this.Controls.Add(this.lblENName );
this.Controls.Add(this.txtENName );

                this.Controls.Add(this.lblBrand );
this.Controls.Add(this.txtBrand );

                this.Controls.Add(this.lblVendorModelCode );
this.Controls.Add(this.txtVendorModelCode );

                
                
                
                
                
                
                
                
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

                
                this.Controls.Add(this.lblSalePublish );
this.Controls.Add(this.chkSalePublish );

                this.Controls.Add(this.lblShortCode );
this.Controls.Add(this.txtShortCode );

                
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

                
                this.Controls.Add(this.lblLatestStorageTime );
this.Controls.Add(this.dtpLatestStorageTime );

                this.Controls.Add(this.lblLatestOutboundTime );
this.Controls.Add(this.dtpLatestOutboundTime );

                this.Controls.Add(this.lblLastInventoryDate );
this.Controls.Add(this.dtpLastInventoryDate );

                    
            this.Name = "View_ProdDetailQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCNName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCNName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSKU;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSKU;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSpecifications;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSpecifications;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblprop;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtprop;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProductNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtProductNo;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModel;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtModel;

    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblENName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtENName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBrand;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBrand;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblVendorModelCode;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtVendorModelCode;

    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIs_available;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIs_available;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIs_enabled;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIs_enabled;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lbl产品可用;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chk产品可用;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lbl产品启用;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chk产品启用;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSKU可用;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkSKU可用;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSKU启用;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkSKU启用;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSalePublish;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkSalePublish;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblShortCode;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtShortCode;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBarCode;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBarCode;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblInv_Cost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtInv_Cost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblStandard_Price;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtStandard_Price;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDiscount_price;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDiscount_price;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMarket_price;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtMarket_price;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblWholesale_Price;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtWholesale_Price;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTransfer_price;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTransfer_price;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblWeight;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtWeight;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLatestStorageTime;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpLatestStorageTime;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLatestOutboundTime;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpLatestOutboundTime;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLastInventoryDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpLastInventoryDate;

    
    
   
 





    }
}


