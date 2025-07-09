
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/09/2025 13:49:54
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
    /// 售后申请单 -登记，评估，清单，确认。目标是维修翻新数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_AS_AfterSaleApplyEdit:UserControl
    {
     public tb_AS_AfterSaleApplyEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_AS_AfterSaleApply UIToEntity()
        {
        tb_AS_AfterSaleApply entity = new tb_AS_AfterSaleApply();
                     entity.ASApplyNo = txtASApplyNo.Text ;
                       entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.CustomerSourceNo = txtCustomerSourceNo.Text ;
                       entity.Priority = Int32.Parse(txtPriority.Text);
                        entity.ASProcessStatus = Int32.Parse(txtASProcessStatus.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.ProjectGroup_ID = Int64.Parse(txtProjectGroup_ID.Text);
                        entity.TotalInitialQuantity = Int32.Parse(txtTotalInitialQuantity.Text);
                        entity.TotalConfirmedQuantity = Int32.Parse(txtTotalConfirmedQuantity.Text);
                        entity.ApplyDate = DateTime.Parse(txtApplyDate.Text);
                        entity.PreDeliveryDate = DateTime.Parse(txtPreDeliveryDate.Text);
                        entity.ShippingAddress = txtShippingAddress.Text ;
                       entity.ShippingWay = txtShippingWay.Text ;
                       entity.InWarrantyPeriod = Boolean.Parse(txtInWarrantyPeriod.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.RepairEvaluationOpinion = txtRepairEvaluationOpinion.Text ;
                       entity.ExpenseAllocationMode = Int32.Parse(txtExpenseAllocationMode.Text);
                        entity.ExpenseBearerType = Int32.Parse(txtExpenseBearerType.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.TotalDeliveredQty = Int32.Parse(txtTotalDeliveredQty.Text);
                        entity.MaterialFeeConfirmed = Boolean.Parse(txtMaterialFeeConfirmed.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                        entity.ApprovalOpinions = txtApprovalOpinions.Text ;
                       entity.Approver_by = Int64.Parse(txtApprover_by.Text);
                        entity.Approver_at = DateTime.Parse(txtApprover_at.Text);
                        entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                        entity.ApprovalResults = Boolean.Parse(txtApprovalResults.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.PrintStatus = Int32.Parse(txtPrintStatus.Text);
                                return entity;
}
        */

        
        private tb_AS_AfterSaleApply _EditEntity;
        public tb_AS_AfterSaleApply EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_AS_AfterSaleApply entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApply>(entity, t => t.ASApplyNo, txtASApplyNo, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApply>(entity, t => t.CustomerSourceNo, txtCustomerSourceNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApply>(entity, t => t.Priority, txtPriority, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApply>(entity, t => t.ASProcessStatus, txtASProcessStatus, BindDataType4TextBox.Qty,false);
          // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
          // DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v=>v.XXNAME, cmbProjectGroup_ID);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApply>(entity, t => t.TotalInitialQuantity, txtTotalInitialQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApply>(entity, t => t.TotalConfirmedQuantity, txtTotalConfirmedQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_AS_AfterSaleApply>(entity, t => t.ApplyDate, dtpApplyDate,false);
           DataBindingHelper.BindData4DataTime<tb_AS_AfterSaleApply>(entity, t => t.PreDeliveryDate, dtpPreDeliveryDate,false);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApply>(entity, t => t.ShippingAddress, txtShippingAddress, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApply>(entity, t => t.ShippingWay, txtShippingWay, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_AS_AfterSaleApply>(entity, t => t.InWarrantyPeriod, chkInWarrantyPeriod, false);
           DataBindingHelper.BindData4DataTime<tb_AS_AfterSaleApply>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApply>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_AS_AfterSaleApply>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApply>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApply>(entity, t => t.RepairEvaluationOpinion, txtRepairEvaluationOpinion, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApply>(entity, t => t.ExpenseAllocationMode, txtExpenseAllocationMode, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApply>(entity, t => t.ExpenseBearerType, txtExpenseBearerType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApply>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApply>(entity, t => t.TotalDeliveredQty, txtTotalDeliveredQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_AS_AfterSaleApply>(entity, t => t.MaterialFeeConfirmed, chkMaterialFeeConfirmed, false);
           DataBindingHelper.BindData4CheckBox<tb_AS_AfterSaleApply>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApply>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApply>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_AS_AfterSaleApply>(entity, t => t.Approver_at, dtpApprover_at,false);
           //default  DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApply>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_AS_AfterSaleApply>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApply>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApply>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



