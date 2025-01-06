
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/06/2025 12:06:57
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
    /// 返工入库统计数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class View_MRP_ReworkEntryEdit:UserControl
    {
     public View_MRP_ReworkEntryEdit() {
     
             
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        View_MRP_ReworkEntry UIToEntity()
        {
        View_MRP_ReworkEntry entity = new View_MRP_ReworkEntry();
                     entity.ReworkEntryID = Int64.Parse(txtReworkEntryID.Text);
                        entity.ReworkEntryNo = txtReworkEntryNo.Text ;
                       entity.ReworkReturnID = Int64.Parse(txtReworkReturnID.Text);
                        entity.EntryDate = DateTime.Parse(txtEntryDate.Text);
                        entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.ApprovalOpinions = txtApprovalOpinions.Text ;
                       entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.SKU = txtSKU.Text ;
                       entity.Specifications = txtSpecifications.Text ;
                       entity.CNName = txtCNName.Text ;
                       entity.Model = txtModel.Text ;
                       entity.Type_ID = Int64.Parse(txtType_ID.Text);
                        entity.property = txtproperty.Text ;
                       entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.Summary = txtSummary.Text ;
                       entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.TotalQty = Int32.Parse(txtTotalQty.Text);
                        entity.TotalReworkFee = Decimal.Parse(txtTotalReworkFee.Text);
                        entity.TotalCost = Decimal.Parse(txtTotalCost.Text);
                        entity.Approver_at = DateTime.Parse(txtApprover_at.Text);
                        entity.Approver_by = Int64.Parse(txtApprover_by.Text);
                        entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                        entity.ApprovalResults = Boolean.Parse(txtApprovalResults.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.Quantity = Int32.Parse(txtQuantity.Text);
                        entity.ReworkFee = Decimal.Parse(txtReworkFee.Text);
                        entity.SubtotalReworkFee = Decimal.Parse(txtSubtotalReworkFee.Text);
                        entity.UnitCost = Decimal.Parse(txtUnitCost.Text);
                        entity.SubtotalCostAmount = Decimal.Parse(txtSubtotalCostAmount.Text);
                        entity.CustomertModel = txtCustomertModel.Text ;
                       entity.ProductNo = txtProductNo.Text ;
                               return entity;
}
        */

        
        private View_MRP_ReworkEntry _EditEntity;
        public View_MRP_ReworkEntry EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(View_MRP_ReworkEntry entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<View_MRP_ReworkEntry>(entity, t => t.ReworkEntryID, txtReworkEntryID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkEntry>(entity, t => t.ReworkEntryNo, txtReworkEntryNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkEntry>(entity, t => t.ReworkReturnID, txtReworkReturnID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<View_MRP_ReworkEntry>(entity, t => t.EntryDate, dtpEntryDate,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkEntry>(entity, t => t.CustomerVendor_ID, txtCustomerVendor_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkEntry>(entity, t => t.Employee_ID, txtEmployee_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkEntry>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkEntry>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkEntry>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkEntry>(entity, t => t.ProdDetailID, txtProdDetailID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkEntry>(entity, t => t.SKU, txtSKU, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkEntry>(entity, t => t.Specifications, txtSpecifications, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkEntry>(entity, t => t.CNName, txtCNName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkEntry>(entity, t => t.Model, txtModel, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkEntry>(entity, t => t.Type_ID, txtType_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkEntry>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkEntry>(entity, t => t.Location_ID, txtLocation_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkEntry>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkEntry>(entity, t => t.DepartmentID, txtDepartmentID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkEntry>(entity, t => t.TotalQty, txtTotalQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkEntry>(entity, t => t.TotalReworkFee.ToString(), txtTotalReworkFee, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkEntry>(entity, t => t.TotalCost.ToString(), txtTotalCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4DataTime<View_MRP_ReworkEntry>(entity, t => t.Approver_at, dtpApprover_at,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkEntry>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           //default  DataBindingHelper.BindData4TextBox<View_MRP_ReworkEntry>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<View_MRP_ReworkEntry>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4DataTime<View_MRP_ReworkEntry>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkEntry>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<View_MRP_ReworkEntry>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkEntry>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkEntry>(entity, t => t.Quantity, txtQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkEntry>(entity, t => t.ReworkFee.ToString(), txtReworkFee, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkEntry>(entity, t => t.SubtotalReworkFee.ToString(), txtSubtotalReworkFee, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkEntry>(entity, t => t.UnitCost.ToString(), txtUnitCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkEntry>(entity, t => t.SubtotalCostAmount.ToString(), txtSubtotalCostAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkEntry>(entity, t => t.CustomertModel, txtCustomertModel, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkEntry>(entity, t => t.ProductNo, txtProductNo, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



