
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/11/2025 15:43:10
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
    /// 维修工单  工时费 材料费数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_AS_RepairOrderEdit:UserControl
    {
     public tb_AS_RepairOrderEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_AS_RepairOrder UIToEntity()
        {
        tb_AS_RepairOrder entity = new tb_AS_RepairOrder();
                     entity.RepairOrderNo = txtRepairOrderNo.Text ;
                       entity.ASApplyID = Int64.Parse(txtASApplyID.Text);
                        entity.ASApplyNo = txtASApplyNo.Text ;
                       entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.ProjectGroup_ID = Int64.Parse(txtProjectGroup_ID.Text);
                        entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.RepairStatus = Int32.Parse(txtRepairStatus.Text);
                        entity.PayStatus = Int32.Parse(txtPayStatus.Text);
                        entity.Paytype_ID = Int64.Parse(txtPaytype_ID.Text);
                        entity.TotalQty = Int32.Parse(txtTotalQty.Text);
                        entity.LaborCost = Decimal.Parse(txtLaborCost.Text);
                        entity.TotalMaterialAmount = Decimal.Parse(txtTotalMaterialAmount.Text);
                        entity.TotalAmount = Decimal.Parse(txtTotalAmount.Text);
                        entity.CustomerPaidAmount = Decimal.Parse(txtCustomerPaidAmount.Text);
                        entity.ExpenseAllocationMode = Int32.Parse(txtExpenseAllocationMode.Text);
                        entity.ExpenseBearerType = Int32.Parse(txtExpenseBearerType.Text);
                        entity.RepairStartDate = DateTime.Parse(txtRepairStartDate.Text);
                        entity.PreDeliveryDate = DateTime.Parse(txtPreDeliveryDate.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.ApprovalOpinions = txtApprovalOpinions.Text ;
                       entity.Approver_by = Int64.Parse(txtApprover_by.Text);
                        entity.Approver_at = DateTime.Parse(txtApprover_at.Text);
                        entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                        entity.ApprovalResults = Boolean.Parse(txtApprovalResults.Text);
                        entity.PrintStatus = Int32.Parse(txtPrintStatus.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.TotalMaterialCost = Decimal.Parse(txtTotalMaterialCost.Text);
                                return entity;
}
        */

        
        private tb_AS_RepairOrder _EditEntity;
        public tb_AS_RepairOrder EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_AS_RepairOrder entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_AS_RepairOrder>(entity, t => t.RepairOrderNo, txtRepairOrderNo, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_AS_AfterSaleApply>(entity, k => k.ASApplyID, v=>v.XXNAME, cmbASApplyID);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrder>(entity, t => t.ASApplyNo, txtASApplyNo, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
          // DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v=>v.XXNAME, cmbProjectGroup_ID);
          // DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrder>(entity, t => t.RepairStatus, txtRepairStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrder>(entity, t => t.PayStatus, txtPayStatus, BindDataType4TextBox.Qty,false);
          // DataBindingHelper.BindData4Cmb<tb_PaymentMethod>(entity, k => k.Paytype_ID, v=>v.XXNAME, cmbPaytype_ID);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrder>(entity, t => t.TotalQty, txtTotalQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrder>(entity, t => t.LaborCost.ToString(), txtLaborCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrder>(entity, t => t.TotalMaterialAmount.ToString(), txtTotalMaterialAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrder>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrder>(entity, t => t.CustomerPaidAmount.ToString(), txtCustomerPaidAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrder>(entity, t => t.ExpenseAllocationMode, txtExpenseAllocationMode, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrder>(entity, t => t.ExpenseBearerType, txtExpenseBearerType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_AS_RepairOrder>(entity, t => t.RepairStartDate, dtpRepairStartDate,false);
           DataBindingHelper.BindData4DataTime<tb_AS_RepairOrder>(entity, t => t.PreDeliveryDate, dtpPreDeliveryDate,false);
           DataBindingHelper.BindData4CheckBox<tb_AS_RepairOrder>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4DataTime<tb_AS_RepairOrder>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrder>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_AS_RepairOrder>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrder>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrder>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrder>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrder>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_AS_RepairOrder>(entity, t => t.Approver_at, dtpApprover_at,false);
           //default  DataBindingHelper.BindData4TextBox<tb_AS_RepairOrder>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_AS_RepairOrder>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrder>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrder>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrder>(entity, t => t.TotalMaterialCost.ToString(), txtTotalMaterialCost, BindDataType4TextBox.Money,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



