
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:31
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
    /// 返工退库统计数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class View_MRP_ReworkReturnEdit:UserControl
    {
     public View_MRP_ReworkReturnEdit() {
     
             
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        View_MRP_ReworkReturn UIToEntity()
        {
        View_MRP_ReworkReturn entity = new View_MRP_ReworkReturn();
                     entity.ReworkReturnID = Int64.Parse(txtReworkReturnID.Text);
                        entity.ReworkReturnNo = txtReworkReturnNo.Text ;
                       entity.DeliveryBillNo = txtDeliveryBillNo.Text ;
                       entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.CloseCaseOpinions = txtCloseCaseOpinions.Text ;
                       entity.ApprovalOpinions = txtApprovalOpinions.Text ;
                       entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.ReturnDate = DateTime.Parse(txtReturnDate.Text);
                        entity.ExpectedReturnDate = DateTime.Parse(txtExpectedReturnDate.Text);
                        entity.ReasonForRework = txtReasonForRework.Text ;
                       entity.Approver_at = DateTime.Parse(txtApprover_at.Text);
                        entity.Approver_by = Int64.Parse(txtApprover_by.Text);
                        entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                        entity.ApprovalResults = Boolean.Parse(txtApprovalResults.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.SKU = txtSKU.Text ;
                       entity.Specifications = txtSpecifications.Text ;
                       entity.CNName = txtCNName.Text ;
                       entity.Model = txtModel.Text ;
                       entity.Type_ID = Int64.Parse(txtType_ID.Text);
                        entity.property = txtproperty.Text ;
                       entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.Summary = txtSummary.Text ;
                       entity.Quantity = Int32.Parse(txtQuantity.Text);
                        entity.DeliveredQuantity = Int32.Parse(txtDeliveredQuantity.Text);
                        entity.ReworkFee = Decimal.Parse(txtReworkFee.Text);
                        entity.SubtotalReworkFee = Decimal.Parse(txtSubtotalReworkFee.Text);
                        entity.UnitCost = Decimal.Parse(txtUnitCost.Text);
                        entity.SubtotalCostAmount = Decimal.Parse(txtSubtotalCostAmount.Text);
                        entity.CustomertModel = txtCustomertModel.Text ;
                       entity.ProductNo = txtProductNo.Text ;
                       entity.Notes = txtNotes.Text ;
                               return entity;
}
        */

        
        private View_MRP_ReworkReturn _EditEntity;
        public View_MRP_ReworkReturn EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(View_MRP_ReworkReturn entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<View_MRP_ReworkReturn>(entity, t => t.ReworkReturnID, txtReworkReturnID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkReturn>(entity, t => t.ReworkReturnNo, txtReworkReturnNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkReturn>(entity, t => t.DeliveryBillNo, txtDeliveryBillNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkReturn>(entity, t => t.CustomerVendor_ID, txtCustomerVendor_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkReturn>(entity, t => t.Employee_ID, txtEmployee_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkReturn>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkReturn>(entity, t => t.CloseCaseOpinions, txtCloseCaseOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkReturn>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkReturn>(entity, t => t.DepartmentID, txtDepartmentID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<View_MRP_ReworkReturn>(entity, t => t.ReturnDate, dtpReturnDate,false);
           DataBindingHelper.BindData4DataTime<View_MRP_ReworkReturn>(entity, t => t.ExpectedReturnDate, dtpExpectedReturnDate,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkReturn>(entity, t => t.ReasonForRework, txtReasonForRework, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<View_MRP_ReworkReturn>(entity, t => t.Approver_at, dtpApprover_at,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkReturn>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           //default  DataBindingHelper.BindData4TextBox<View_MRP_ReworkReturn>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<View_MRP_ReworkReturn>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4DataTime<View_MRP_ReworkReturn>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkReturn>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<View_MRP_ReworkReturn>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkReturn>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkReturn>(entity, t => t.ProdDetailID, txtProdDetailID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkReturn>(entity, t => t.SKU, txtSKU, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkReturn>(entity, t => t.Specifications, txtSpecifications, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkReturn>(entity, t => t.CNName, txtCNName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkReturn>(entity, t => t.Model, txtModel, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkReturn>(entity, t => t.Type_ID, txtType_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkReturn>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkReturn>(entity, t => t.Location_ID, txtLocation_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkReturn>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkReturn>(entity, t => t.Quantity, txtQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkReturn>(entity, t => t.DeliveredQuantity, txtDeliveredQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkReturn>(entity, t => t.ReworkFee.ToString(), txtReworkFee, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkReturn>(entity, t => t.SubtotalReworkFee.ToString(), txtSubtotalReworkFee, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkReturn>(entity, t => t.UnitCost.ToString(), txtUnitCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkReturn>(entity, t => t.SubtotalCostAmount.ToString(), txtSubtotalCostAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkReturn>(entity, t => t.CustomertModel, txtCustomertModel, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkReturn>(entity, t => t.ProductNo, txtProductNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MRP_ReworkReturn>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



