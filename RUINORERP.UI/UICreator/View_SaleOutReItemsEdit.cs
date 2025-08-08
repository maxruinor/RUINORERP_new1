
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:38
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
    /// 销售退货统计分析数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class View_SaleOutReItemsEdit:UserControl
    {
     public View_SaleOutReItemsEdit() {
     
             
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        View_SaleOutReItems UIToEntity()
        {
        View_SaleOutReItems entity = new View_SaleOutReItems();
                     entity.SKU = txtSKU.Text ;
                       entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.ReturnNo = txtReturnNo.Text ;
                       entity.PayStatus = Int32.Parse(txtPayStatus.Text);
                        entity.Paytype_ID = Int64.Parse(txtPaytype_ID.Text);
                        entity.ReturnDate = DateTime.Parse(txtReturnDate.Text);
                        entity.SaleOut_NO = txtSaleOut_NO.Text ;
                       entity.ProjectGroup_ID = Int64.Parse(txtProjectGroup_ID.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.property = txtproperty.Text ;
                       entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.Rack_ID = Int64.Parse(txtRack_ID.Text);
                        entity.Quantity = Int32.Parse(txtQuantity.Text);
                        entity.TransactionPrice = Decimal.Parse(txtTransactionPrice.Text);
                        entity.SubtotalTransAmount = Decimal.Parse(txtSubtotalTransAmount.Text);
                        entity.Summary = txtSummary.Text ;
                       entity.CustomerPartNo = txtCustomerPartNo.Text ;
                       entity.Cost = Decimal.Parse(txtCost.Text);
                        entity.SubtotalCostAmount = Decimal.Parse(txtSubtotalCostAmount.Text);
                        entity.TaxRate = Decimal.Parse(txtTaxRate.Text);
                        entity.Gift = Boolean.Parse(txtGift.Text);
                        entity.SubtotalUntaxedAmount = Decimal.Parse(txtSubtotalUntaxedAmount.Text);
                        entity.SubtotalTaxAmount = Decimal.Parse(txtSubtotalTaxAmount.Text);
                        entity.CNName = txtCNName.Text ;
                       entity.Specifications = txtSpecifications.Text ;
                       entity.ProductNo = txtProductNo.Text ;
                       entity.Unit_ID = Int64.Parse(txtUnit_ID.Text);
                        entity.Model = txtModel.Text ;
                       entity.Category_ID = Int64.Parse(txtCategory_ID.Text);
                                return entity;
}
        */

        
        private View_SaleOutReItems _EditEntity;
        public View_SaleOutReItems EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(View_SaleOutReItems entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<View_SaleOutReItems>(entity, t => t.SKU, txtSKU, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_SaleOutReItems>(entity, t => t.Employee_ID, txtEmployee_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_SaleOutReItems>(entity, t => t.CustomerVendor_ID, txtCustomerVendor_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_SaleOutReItems>(entity, t => t.ReturnNo, txtReturnNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_SaleOutReItems>(entity, t => t.PayStatus, txtPayStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_SaleOutReItems>(entity, t => t.Paytype_ID, txtPaytype_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<View_SaleOutReItems>(entity, t => t.ReturnDate, dtpReturnDate,false);
           DataBindingHelper.BindData4TextBox<View_SaleOutReItems>(entity, t => t.SaleOut_NO, txtSaleOut_NO, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_SaleOutReItems>(entity, t => t.ProjectGroup_ID, txtProjectGroup_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_SaleOutReItems>(entity, t => t.ProdDetailID, txtProdDetailID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_SaleOutReItems>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_SaleOutReItems>(entity, t => t.Location_ID, txtLocation_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_SaleOutReItems>(entity, t => t.Rack_ID, txtRack_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_SaleOutReItems>(entity, t => t.Quantity, txtQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_SaleOutReItems>(entity, t => t.TransactionPrice.ToString(), txtTransactionPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_SaleOutReItems>(entity, t => t.SubtotalTransAmount.ToString(), txtSubtotalTransAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_SaleOutReItems>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_SaleOutReItems>(entity, t => t.CustomerPartNo, txtCustomerPartNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_SaleOutReItems>(entity, t => t.Cost.ToString(), txtCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_SaleOutReItems>(entity, t => t.SubtotalCostAmount.ToString(), txtSubtotalCostAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_SaleOutReItems>(entity, t => t.TaxRate.ToString(), txtTaxRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<View_SaleOutReItems>(entity, t => t.Gift, chkGift, false);
           DataBindingHelper.BindData4TextBox<View_SaleOutReItems>(entity, t => t.SubtotalUntaxedAmount.ToString(), txtSubtotalUntaxedAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_SaleOutReItems>(entity, t => t.SubtotalTaxAmount.ToString(), txtSubtotalTaxAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_SaleOutReItems>(entity, t => t.CNName, txtCNName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_SaleOutReItems>(entity, t => t.Specifications, txtSpecifications, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_SaleOutReItems>(entity, t => t.ProductNo, txtProductNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_SaleOutReItems>(entity, t => t.Unit_ID, txtUnit_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_SaleOutReItems>(entity, t => t.Model, txtModel, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_SaleOutReItems>(entity, t => t.Category_ID, txtCategory_ID, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



