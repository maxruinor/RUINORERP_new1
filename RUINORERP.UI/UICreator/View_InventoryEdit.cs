
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:29
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
    /// 库存视图-得从表取不能视图套视图数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class View_InventoryEdit:UserControl
    {
     public View_InventoryEdit() {
     
             
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        View_Inventory UIToEntity()
        {
        View_Inventory entity = new View_Inventory();
                     entity.Inventory_ID = Int64.Parse(txtInventory_ID.Text);
                        entity.ProductNo = txtProductNo.Text ;
                       entity.SKU = txtSKU.Text ;
                       entity.CNName = txtCNName.Text ;
                       entity.prop = txtprop.Text ;
                       entity.Specifications = txtSpecifications.Text ;
                       entity.Model = txtModel.Text ;
                       entity.Quantity = Int32.Parse(txtQuantity.Text);
                        entity.Type_ID = Int64.Parse(txtType_ID.Text);
                        entity.Unit_ID = Int64.Parse(txtUnit_ID.Text);
                        entity.Category_ID = Int64.Parse(txtCategory_ID.Text);
                        entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.SourceType = Int32.Parse(txtSourceType.Text);
                        entity.Brand = txtBrand.Text ;
                       entity.Rack_ID = Int64.Parse(txtRack_ID.Text);
                        entity.Alert_Quantity = Int32.Parse(txtAlert_Quantity.Text);
                        entity.On_the_way_Qty = Int32.Parse(txtOn_the_way_Qty.Text);
                        entity.Sale_Qty = Int32.Parse(txtSale_Qty.Text);
                        entity.MakingQty = Int32.Parse(txtMakingQty.Text);
                        entity.NotOutQty = Int32.Parse(txtNotOutQty.Text);
                        entity.Inv_Cost = Decimal.Parse(txtInv_Cost.Text);
                        entity.Inv_SubtotalCostMoney = Decimal.Parse(txtInv_SubtotalCostMoney.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.BOM_ID = Int64.Parse(txtBOM_ID.Text);
                        entity.LatestStorageTime = DateTime.Parse(txtLatestStorageTime.Text);
                        entity.LatestOutboundTime = DateTime.Parse(txtLatestOutboundTime.Text);
                        entity.LastInventoryDate = DateTime.Parse(txtLastInventoryDate.Text);
                        entity.Notes = txtNotes.Text ;
                               return entity;
}
        */

        
        private View_Inventory _EditEntity;
        public View_Inventory EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(View_Inventory entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<View_Inventory>(entity, t => t.Inventory_ID, txtInventory_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_Inventory>(entity, t => t.ProductNo, txtProductNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_Inventory>(entity, t => t.SKU, txtSKU, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_Inventory>(entity, t => t.CNName, txtCNName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_Inventory>(entity, t => t.prop, txtprop, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_Inventory>(entity, t => t.Specifications, txtSpecifications, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_Inventory>(entity, t => t.Model, txtModel, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_Inventory>(entity, t => t.Quantity, txtQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_Inventory>(entity, t => t.Type_ID, txtType_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_Inventory>(entity, t => t.Unit_ID, txtUnit_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_Inventory>(entity, t => t.Category_ID, txtCategory_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_Inventory>(entity, t => t.CustomerVendor_ID, txtCustomerVendor_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_Inventory>(entity, t => t.DepartmentID, txtDepartmentID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_Inventory>(entity, t => t.SourceType, txtSourceType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_Inventory>(entity, t => t.Brand, txtBrand, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_Inventory>(entity, t => t.Rack_ID, txtRack_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_Inventory>(entity, t => t.Alert_Quantity, txtAlert_Quantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_Inventory>(entity, t => t.On_the_way_Qty, txtOn_the_way_Qty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_Inventory>(entity, t => t.Sale_Qty, txtSale_Qty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_Inventory>(entity, t => t.MakingQty, txtMakingQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_Inventory>(entity, t => t.NotOutQty, txtNotOutQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_Inventory>(entity, t => t.Inv_Cost.ToString(), txtInv_Cost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_Inventory>(entity, t => t.Inv_SubtotalCostMoney.ToString(), txtInv_SubtotalCostMoney, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_Inventory>(entity, t => t.ProdDetailID, txtProdDetailID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_Inventory>(entity, t => t.Location_ID, txtLocation_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_Inventory>(entity, t => t.BOM_ID, txtBOM_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<View_Inventory>(entity, t => t.LatestStorageTime, dtpLatestStorageTime,false);
           DataBindingHelper.BindData4DataTime<View_Inventory>(entity, t => t.LatestOutboundTime, dtpLatestOutboundTime,false);
           DataBindingHelper.BindData4DataTime<View_Inventory>(entity, t => t.LastInventoryDate, dtpLastInventoryDate,false);
           DataBindingHelper.BindData4TextBox<View_Inventory>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



