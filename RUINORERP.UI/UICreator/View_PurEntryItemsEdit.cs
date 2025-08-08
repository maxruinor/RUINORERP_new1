
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:36
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
    /// 采购入库统计数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class View_PurEntryItemsEdit:UserControl
    {
     public View_PurEntryItemsEdit() {
     
             
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        View_PurEntryItems UIToEntity()
        {
        View_PurEntryItems entity = new View_PurEntryItems();
                     entity.PurOrder_NO = txtPurOrder_NO.Text ;
                       entity.PurEntryNo = txtPurEntryNo.Text ;
                       entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.Paytype_ID = Int64.Parse(txtPaytype_ID.Text);
                        entity.EntryDate = DateTime.Parse(txtEntryDate.Text);
                        entity.ShipCost = Decimal.Parse(txtShipCost.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.TaxDeductionType = Int32.Parse(txtTaxDeductionType.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.SKU = txtSKU.Text ;
                       entity.Specifications = txtSpecifications.Text ;
                       entity.CNName = txtCNName.Text ;
                       entity.Model = txtModel.Text ;
                       entity.Type_ID = Int64.Parse(txtType_ID.Text);
                        entity.property = txtproperty.Text ;
                       entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.Quantity = Int32.Parse(txtQuantity.Text);
                        entity.UnitPrice = Decimal.Parse(txtUnitPrice.Text);
                        entity.UntaxedUnitPrice = Decimal.Parse(txtUntaxedUnitPrice.Text);
                        entity.TaxRate = Decimal.Parse(txtTaxRate.Text);
                        entity.TaxAmount = Decimal.Parse(txtTaxAmount.Text);
                        entity.SubtotalAmount = Decimal.Parse(txtSubtotalAmount.Text);
                        entity.IsGift = Boolean.Parse(txtIsGift.Text);
                        entity.CustomertModel = txtCustomertModel.Text ;
                       entity.ReturnedQty = Int32.Parse(txtReturnedQty.Text);
                        entity.Summary = txtSummary.Text ;
                       entity.PrintStatus = Int32.Parse(txtPrintStatus.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                        entity.ApprovalResults = Boolean.Parse(txtApprovalResults.Text);
                        entity.ApprovalOpinions = txtApprovalOpinions.Text ;
                       entity.TotalAmount = Decimal.Parse(txtTotalAmount.Text);
                        entity.TotalTaxAmount = Decimal.Parse(txtTotalTaxAmount.Text);
                        entity.TotalQty = Decimal.Parse(txtTotalQty.Text);
                        entity.Category_ID = Int64.Parse(txtCategory_ID.Text);
                        entity.Rack_ID = Int64.Parse(txtRack_ID.Text);
                        entity.UnitName = txtUnitName.Text ;
                       entity.IsIncludeTax = Boolean.Parse(txtIsIncludeTax.Text);
                                return entity;
}
        */

        
        private View_PurEntryItems _EditEntity;
        public View_PurEntryItems EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(View_PurEntryItems entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<View_PurEntryItems>(entity, t => t.PurOrder_NO, txtPurOrder_NO, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_PurEntryItems>(entity, t => t.PurEntryNo, txtPurEntryNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_PurEntryItems>(entity, t => t.CustomerVendor_ID, txtCustomerVendor_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_PurEntryItems>(entity, t => t.Employee_ID, txtEmployee_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_PurEntryItems>(entity, t => t.DepartmentID, txtDepartmentID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_PurEntryItems>(entity, t => t.Paytype_ID, txtPaytype_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<View_PurEntryItems>(entity, t => t.EntryDate, dtpEntryDate,false);
           DataBindingHelper.BindData4TextBox<View_PurEntryItems>(entity, t => t.ShipCost.ToString(), txtShipCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_PurEntryItems>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_PurEntryItems>(entity, t => t.TaxDeductionType, txtTaxDeductionType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_PurEntryItems>(entity, t => t.ProdDetailID, txtProdDetailID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_PurEntryItems>(entity, t => t.SKU, txtSKU, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_PurEntryItems>(entity, t => t.Specifications, txtSpecifications, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_PurEntryItems>(entity, t => t.CNName, txtCNName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_PurEntryItems>(entity, t => t.Model, txtModel, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_PurEntryItems>(entity, t => t.Type_ID, txtType_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_PurEntryItems>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_PurEntryItems>(entity, t => t.Location_ID, txtLocation_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_PurEntryItems>(entity, t => t.Quantity, txtQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_PurEntryItems>(entity, t => t.UnitPrice.ToString(), txtUnitPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_PurEntryItems>(entity, t => t.UntaxedUnitPrice.ToString(), txtUntaxedUnitPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_PurEntryItems>(entity, t => t.TaxRate.ToString(), txtTaxRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_PurEntryItems>(entity, t => t.TaxAmount.ToString(), txtTaxAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_PurEntryItems>(entity, t => t.SubtotalAmount.ToString(), txtSubtotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<View_PurEntryItems>(entity, t => t.IsGift, chkIsGift, false);
           DataBindingHelper.BindData4TextBox<View_PurEntryItems>(entity, t => t.CustomertModel, txtCustomertModel, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_PurEntryItems>(entity, t => t.ReturnedQty, txtReturnedQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_PurEntryItems>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_PurEntryItems>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_PurEntryItems>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           //default  DataBindingHelper.BindData4TextBox<View_PurEntryItems>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<View_PurEntryItems>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4TextBox<View_PurEntryItems>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_PurEntryItems>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_PurEntryItems>(entity, t => t.TotalTaxAmount.ToString(), txtTotalTaxAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_PurEntryItems>(entity, t => t.TotalQty.ToString(), txtTotalQty, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_PurEntryItems>(entity, t => t.Category_ID, txtCategory_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_PurEntryItems>(entity, t => t.Rack_ID, txtRack_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_PurEntryItems>(entity, t => t.UnitName, txtUnitName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<View_PurEntryItems>(entity, t => t.IsIncludeTax, chkIsIncludeTax, false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



