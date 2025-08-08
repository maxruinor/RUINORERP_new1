
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:26
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
    /// 售后申请明细统计数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class View_AS_AfterSaleApplyItemsEdit:UserControl
    {
     public View_AS_AfterSaleApplyItemsEdit() {
     
             
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        View_AS_AfterSaleApplyItems UIToEntity()
        {
        View_AS_AfterSaleApplyItems entity = new View_AS_AfterSaleApplyItems();
                     entity.ASApplyNo = txtASApplyNo.Text ;
                       entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.ProjectGroup_ID = Int64.Parse(txtProjectGroup_ID.Text);
                        entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.ApplyDate = DateTime.Parse(txtApplyDate.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.CustomerSourceNo = txtCustomerSourceNo.Text ;
                       entity.Priority = Int32.Parse(txtPriority.Text);
                        entity.ASProcessStatus = Int32.Parse(txtASProcessStatus.Text);
                        entity.TotalConfirmedQuantity = Int32.Parse(txtTotalConfirmedQuantity.Text);
                        entity.RepairEvaluationOpinion = txtRepairEvaluationOpinion.Text ;
                       entity.ExpenseAllocationMode = Int32.Parse(txtExpenseAllocationMode.Text);
                        entity.ExpenseBearerType = Int32.Parse(txtExpenseBearerType.Text);
                        entity.TotalDeliveredQty = Int32.Parse(txtTotalDeliveredQty.Text);
                        entity.FaultDescription = txtFaultDescription.Text ;
                       entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.CustomerPartNo = txtCustomerPartNo.Text ;
                       entity.ConfirmedQuantity = Int32.Parse(txtConfirmedQuantity.Text);
                        entity.DeliveredQty = Int32.Parse(txtDeliveredQty.Text);
                        entity.InitialQuantity = Int32.Parse(txtInitialQuantity.Text);
                        entity.Summary = txtSummary.Text ;
                       entity.property = txtproperty.Text ;
                       entity.ProdBaseID = Int64.Parse(txtProdBaseID.Text);
                        entity.SKU = txtSKU.Text ;
                       entity.CNName = txtCNName.Text ;
                       entity.Unit_ID = Int64.Parse(txtUnit_ID.Text);
                        entity.Specifications = txtSpecifications.Text ;
                       entity.Quantity = Int32.Parse(txtQuantity.Text);
                        entity.prop = txtprop.Text ;
                       entity.ProductNo = txtProductNo.Text ;
                       entity.Model = txtModel.Text ;
                       entity.Category_ID = Int64.Parse(txtCategory_ID.Text);
                        entity.Type_ID = Int64.Parse(txtType_ID.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                        entity.ApprovalResults = Boolean.Parse(txtApprovalResults.Text);
                        entity.ApprovalOpinions = txtApprovalOpinions.Text ;
                       entity.Notes = txtNotes.Text ;
                               return entity;
}
        */

        
        private View_AS_AfterSaleApplyItems _EditEntity;
        public View_AS_AfterSaleApplyItems EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(View_AS_AfterSaleApplyItems entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.ASApplyNo, txtASApplyNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.Employee_ID, txtEmployee_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.ProjectGroup_ID, txtProjectGroup_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.CustomerVendor_ID, txtCustomerVendor_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<View_AS_AfterSaleApplyItems>(entity, t => t.ApplyDate, dtpApplyDate,false);
           DataBindingHelper.BindData4DataTime<View_AS_AfterSaleApplyItems>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.CustomerSourceNo, txtCustomerSourceNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.Priority, txtPriority, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.ASProcessStatus, txtASProcessStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.TotalConfirmedQuantity, txtTotalConfirmedQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.RepairEvaluationOpinion, txtRepairEvaluationOpinion, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.ExpenseAllocationMode, txtExpenseAllocationMode, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.ExpenseBearerType, txtExpenseBearerType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.TotalDeliveredQty, txtTotalDeliveredQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.FaultDescription, txtFaultDescription, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.ProdDetailID, txtProdDetailID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.Location_ID, txtLocation_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.CustomerPartNo, txtCustomerPartNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.ConfirmedQuantity, txtConfirmedQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.DeliveredQty, txtDeliveredQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.InitialQuantity, txtInitialQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.ProdBaseID, txtProdBaseID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.SKU, txtSKU, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.CNName, txtCNName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.Unit_ID, txtUnit_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.Specifications, txtSpecifications, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.Quantity, txtQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.prop, txtprop, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.ProductNo, txtProductNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.Model, txtModel, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.Category_ID, txtCategory_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.Type_ID, txtType_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           //default  DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<View_AS_AfterSaleApplyItems>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_AS_AfterSaleApplyItems>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



