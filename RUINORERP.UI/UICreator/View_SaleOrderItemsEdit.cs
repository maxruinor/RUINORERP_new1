
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
    /// 销售订单统计分析数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class View_SaleOrderItemsEdit:UserControl
    {
     public View_SaleOrderItemsEdit() {
     
             
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        View_SaleOrderItems UIToEntity()
        {
        View_SaleOrderItems entity = new View_SaleOrderItems();
                     entity.SOrder_ID = Int64.Parse(txtSOrder_ID.Text);
                        entity.SOrderNo = txtSOrderNo.Text ;
                       entity.SaleDate = DateTime.Parse(txtSaleDate.Text);
                        entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.SKU = txtSKU.Text ;
                       entity.Specifications = txtSpecifications.Text ;
                       entity.CNName = txtCNName.Text ;
                       entity.Model = txtModel.Text ;
                       entity.Type_ID = Int64.Parse(txtType_ID.Text);
                        entity.PayStatus = Int32.Parse(txtPayStatus.Text);
                        entity.Paytype_ID = Int64.Parse(txtPaytype_ID.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.ProjectGroup_ID = Int64.Parse(txtProjectGroup_ID.Text);
                        entity.PlatformOrderNo = txtPlatformOrderNo.Text ;
                       entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.UnitPrice = Decimal.Parse(txtUnitPrice.Text);
                        entity.Quantity = Int32.Parse(txtQuantity.Text);
                        entity.Discount = Decimal.Parse(txtDiscount.Text);
                        entity.TransactionPrice = Decimal.Parse(txtTransactionPrice.Text);
                        entity.SubtotalTransAmount = Decimal.Parse(txtSubtotalTransAmount.Text);
                        entity.Cost = Decimal.Parse(txtCost.Text);
                        entity.SubtotalCostAmount = Decimal.Parse(txtSubtotalCostAmount.Text);
                        entity.TotalDeliveredQty = Int32.Parse(txtTotalDeliveredQty.Text);
                        entity.CommissionAmount = Decimal.Parse(txtCommissionAmount.Text);
                        entity.Summary = txtSummary.Text ;
                       entity.CustomerPartNo = txtCustomerPartNo.Text ;
                       entity.Gift = Boolean.Parse(txtGift.Text);
                        entity.TotalReturnedQty = Int32.Parse(txtTotalReturnedQty.Text);
                        entity.property = txtproperty.Text ;
                       entity.ProductNo = txtProductNo.Text ;
                       entity.Category_ID = Int64.Parse(txtCategory_ID.Text);
                        entity.Unit_ID = Int64.Parse(txtUnit_ID.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                                return entity;
}
        */

        
        private View_SaleOrderItems _EditEntity;
        public View_SaleOrderItems EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(View_SaleOrderItems entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<View_SaleOrderItems>(entity, t => t.SOrder_ID, txtSOrder_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_SaleOrderItems>(entity, t => t.SOrderNo, txtSOrderNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<View_SaleOrderItems>(entity, t => t.SaleDate, dtpSaleDate,false);
           DataBindingHelper.BindData4TextBox<View_SaleOrderItems>(entity, t => t.CustomerVendor_ID, txtCustomerVendor_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_SaleOrderItems>(entity, t => t.SKU, txtSKU, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_SaleOrderItems>(entity, t => t.Specifications, txtSpecifications, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_SaleOrderItems>(entity, t => t.CNName, txtCNName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_SaleOrderItems>(entity, t => t.Model, txtModel, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_SaleOrderItems>(entity, t => t.Type_ID, txtType_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_SaleOrderItems>(entity, t => t.PayStatus, txtPayStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_SaleOrderItems>(entity, t => t.Paytype_ID, txtPaytype_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_SaleOrderItems>(entity, t => t.Employee_ID, txtEmployee_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_SaleOrderItems>(entity, t => t.ProjectGroup_ID, txtProjectGroup_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_SaleOrderItems>(entity, t => t.PlatformOrderNo, txtPlatformOrderNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_SaleOrderItems>(entity, t => t.ProdDetailID, txtProdDetailID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_SaleOrderItems>(entity, t => t.UnitPrice.ToString(), txtUnitPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_SaleOrderItems>(entity, t => t.Quantity, txtQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_SaleOrderItems>(entity, t => t.Discount.ToString(), txtDiscount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_SaleOrderItems>(entity, t => t.TransactionPrice.ToString(), txtTransactionPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_SaleOrderItems>(entity, t => t.SubtotalTransAmount.ToString(), txtSubtotalTransAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_SaleOrderItems>(entity, t => t.Cost.ToString(), txtCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_SaleOrderItems>(entity, t => t.SubtotalCostAmount.ToString(), txtSubtotalCostAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_SaleOrderItems>(entity, t => t.TotalDeliveredQty, txtTotalDeliveredQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_SaleOrderItems>(entity, t => t.CommissionAmount.ToString(), txtCommissionAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_SaleOrderItems>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_SaleOrderItems>(entity, t => t.CustomerPartNo, txtCustomerPartNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<View_SaleOrderItems>(entity, t => t.Gift, chkGift, false);
           DataBindingHelper.BindData4TextBox<View_SaleOrderItems>(entity, t => t.TotalReturnedQty, txtTotalReturnedQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_SaleOrderItems>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_SaleOrderItems>(entity, t => t.ProductNo, txtProductNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_SaleOrderItems>(entity, t => t.Category_ID, txtCategory_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_SaleOrderItems>(entity, t => t.Unit_ID, txtUnit_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_SaleOrderItems>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           //default  DataBindingHelper.BindData4TextBox<View_SaleOrderItems>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



