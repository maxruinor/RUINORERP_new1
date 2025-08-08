
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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common;
using RUINORERP.Model;
using RUINORERP.UI.Common;

namespace RUINORERP.UI
{
    /// <summary>
    /// 产品详情视图数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class View_ProdDetailEdit:UserControl
    {
     public View_ProdDetailEdit() {
     
             
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        View_ProdDetail UIToEntity()
        {
        View_ProdDetail entity = new View_ProdDetail();
                     entity.ProdBaseID = Int64.Parse(txtProdBaseID.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.CNName = txtCNName.Text ;
                       entity.SKU = txtSKU.Text ;
                       entity.Specifications = txtSpecifications.Text ;
                       entity.Quantity = Int32.Parse(txtQuantity.Text);
                        entity.prop = txtprop.Text ;
                       entity.ProductNo = txtProductNo.Text ;
                       entity.Unit_ID = Int64.Parse(txtUnit_ID.Text);
                        entity.Model = txtModel.Text ;
                       entity.Category_ID = Int64.Parse(txtCategory_ID.Text);
                        entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.ENName = txtENName.Text ;
                       entity.Brand = txtBrand.Text ;
                       entity.VendorModelCode = txtVendorModelCode.Text ;
                       entity.Images = Binary.Parse(txtImages.Text);
                        entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.Rack_ID = Int64.Parse(txtRack_ID.Text);
                        entity.On_the_way_Qty = Int32.Parse(txtOn_the_way_Qty.Text);
                        entity.Sale_Qty = Int32.Parse(txtSale_Qty.Text);
                        entity.Alert_Quantity = Int32.Parse(txtAlert_Quantity.Text);
                        entity.MakingQty = Int32.Parse(txtMakingQty.Text);
                        entity.NotOutQty = Int32.Parse(txtNotOutQty.Text);
                        entity.Is_available = Boolean.Parse(txtIs_available.Text);
                        entity.Is_enabled = Boolean.Parse(txtIs_enabled.Text);
                        entity.产品可用 = Boolean.Parse(txt产品可用.Text);
                        entity.产品启用 = Boolean.Parse(txt产品启用.Text);
                        entity.SKU可用 = Boolean.Parse(txtSKU可用.Text);
                        entity.SKU启用 = Boolean.Parse(txtSKU启用.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.Type_ID = Int64.Parse(txtType_ID.Text);
                        entity.SalePublish = Boolean.Parse(txtSalePublish.Text);
                        entity.ShortCode = txtShortCode.Text ;
                       entity.SourceType = Int32.Parse(txtSourceType.Text);
                        entity.BarCode = txtBarCode.Text ;
                       entity.Inv_Cost = Decimal.Parse(txtInv_Cost.Text);
                        entity.Standard_Price = Decimal.Parse(txtStandard_Price.Text);
                        entity.Discount_price = Decimal.Parse(txtDiscount_price.Text);
                        entity.Market_price = Decimal.Parse(txtMarket_price.Text);
                        entity.Wholesale_Price = Decimal.Parse(txtWholesale_Price.Text);
                        entity.Transfer_price = Decimal.Parse(txtTransfer_price.Text);
                        entity.Weight = Decimal.Parse(txtWeight.Text);
                        entity.BOM_ID = Int64.Parse(txtBOM_ID.Text);
                        entity.LatestStorageTime = DateTime.Parse(txtLatestStorageTime.Text);
                        entity.LatestOutboundTime = DateTime.Parse(txtLatestOutboundTime.Text);
                        entity.LastInventoryDate = DateTime.Parse(txtLastInventoryDate.Text);
                                return entity;
}
        */

        
        private View_ProdDetail _EditEntity;
        public View_ProdDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(View_ProdDetail entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.ProdBaseID, txtProdBaseID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.ProdDetailID, txtProdDetailID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.CNName, txtCNName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.SKU, txtSKU, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.Specifications, txtSpecifications, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.Quantity, txtQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.prop, txtprop, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.ProductNo, txtProductNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.Unit_ID, txtUnit_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.Model, txtModel, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.Category_ID, txtCategory_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.CustomerVendor_ID, txtCustomerVendor_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.DepartmentID, txtDepartmentID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.ENName, txtENName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.Brand, txtBrand, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.VendorModelCode, txtVendorModelCode, BindDataType4TextBox.Text,false);
           //default  DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.Images.ToString(), txtImages, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.Location_ID, txtLocation_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.Rack_ID, txtRack_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.On_the_way_Qty, txtOn_the_way_Qty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.Sale_Qty, txtSale_Qty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.Alert_Quantity, txtAlert_Quantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.MakingQty, txtMakingQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.NotOutQty, txtNotOutQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<View_ProdDetail>(entity, t => t.Is_available, chkIs_available, false);
           DataBindingHelper.BindData4CheckBox<View_ProdDetail>(entity, t => t.Is_enabled, chkIs_enabled, false);
           DataBindingHelper.BindData4CheckBox<View_ProdDetail>(entity, t => t.产品可用, chk产品可用, false);
           DataBindingHelper.BindData4CheckBox<View_ProdDetail>(entity, t => t.产品启用, chk产品启用, false);
           DataBindingHelper.BindData4CheckBox<View_ProdDetail>(entity, t => t.SKU可用, chkSKU可用, false);
           DataBindingHelper.BindData4CheckBox<View_ProdDetail>(entity, t => t.SKU启用, chkSKU启用, false);
           DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.Type_ID, txtType_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<View_ProdDetail>(entity, t => t.SalePublish, chkSalePublish, false);
           DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.ShortCode, txtShortCode, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.SourceType, txtSourceType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.BarCode, txtBarCode, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.Inv_Cost.ToString(), txtInv_Cost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.Standard_Price.ToString(), txtStandard_Price, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.Discount_price.ToString(), txtDiscount_price, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.Market_price.ToString(), txtMarket_price, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.Wholesale_Price.ToString(), txtWholesale_Price, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.Transfer_price.ToString(), txtTransfer_price, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.Weight.ToString(), txtWeight, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_ProdDetail>(entity, t => t.BOM_ID, txtBOM_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<View_ProdDetail>(entity, t => t.LatestStorageTime, dtpLatestStorageTime,false);
           DataBindingHelper.BindData4DataTime<View_ProdDetail>(entity, t => t.LatestOutboundTime, dtpLatestOutboundTime,false);
           DataBindingHelper.BindData4DataTime<View_ProdDetail>(entity, t => t.LastInventoryDate, dtpLastInventoryDate,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



