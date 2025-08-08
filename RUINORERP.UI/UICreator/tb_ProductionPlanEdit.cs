
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:01
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
    /// 生产计划表 应该是分析来的。可能来自于生产需求单，比方系统根据库存情况分析销售情况等也也可以手动。也可以程序分析数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_ProductionPlanEdit:UserControl
    {
     public tb_ProductionPlanEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_ProductionPlan UIToEntity()
        {
        tb_ProductionPlan entity = new tb_ProductionPlan();
                     entity.SOrder_ID = Int64.Parse(txtSOrder_ID.Text);
                        entity.SaleOrderNo = txtSaleOrderNo.Text ;
                       entity.PPNo = txtPPNo.Text ;
                       entity.ProjectGroup_ID = Int64.Parse(txtProjectGroup_ID.Text);
                        entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.Priority = Int32.Parse(txtPriority.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.RequirementDate = DateTime.Parse(txtRequirementDate.Text);
                        entity.PlanDate = DateTime.Parse(txtPlanDate.Text);
                        entity.TotalCompletedQuantity = Int32.Parse(txtTotalCompletedQuantity.Text);
                        entity.TotalQuantity = Int32.Parse(txtTotalQuantity.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.ApprovalOpinions = txtApprovalOpinions.Text ;
                       entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                        entity.Analyzed = Boolean.Parse(txtAnalyzed.Text);
                        entity.ApprovalResults = Boolean.Parse(txtApprovalResults.Text);
                        entity.PrintStatus = Int32.Parse(txtPrintStatus.Text);
                        entity.Approver_by = Int64.Parse(txtApprover_by.Text);
                        entity.Approver_at = DateTime.Parse(txtApprover_at.Text);
                        entity.CloseCaseOpinions = txtCloseCaseOpinions.Text ;
                               return entity;
}
        */

        
        private tb_ProductionPlan _EditEntity;
        public tb_ProductionPlan EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_ProductionPlan entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_SaleOrder>(entity, k => k.SOrder_ID, v=>v.XXNAME, cmbSOrder_ID);
           DataBindingHelper.BindData4TextBox<tb_ProductionPlan>(entity, t => t.SaleOrderNo, txtSaleOrderNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProductionPlan>(entity, t => t.PPNo, txtPPNo, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v=>v.XXNAME, cmbProjectGroup_ID);
          // DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v=>v.XXNAME, cmbDepartmentID);
           DataBindingHelper.BindData4TextBox<tb_ProductionPlan>(entity, t => t.Priority, txtPriority, BindDataType4TextBox.Qty,false);
          // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
           DataBindingHelper.BindData4DataTime<tb_ProductionPlan>(entity, t => t.RequirementDate, dtpRequirementDate,false);
           DataBindingHelper.BindData4DataTime<tb_ProductionPlan>(entity, t => t.PlanDate, dtpPlanDate,false);
           DataBindingHelper.BindData4TextBox<tb_ProductionPlan>(entity, t => t.TotalCompletedQuantity, txtTotalCompletedQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ProductionPlan>(entity, t => t.TotalQuantity, txtTotalQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_ProductionPlan>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4TextBox<tb_ProductionPlan>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_ProductionPlan>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_ProductionPlan>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_ProductionPlan>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_ProductionPlan>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ProductionPlan>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProductionPlan>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           //default  DataBindingHelper.BindData4TextBox<tb_ProductionPlan>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_ProductionPlan>(entity, t => t.Analyzed, chkAnalyzed, false);
           DataBindingHelper.BindData4CheckBox<tb_ProductionPlan>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4TextBox<tb_ProductionPlan>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ProductionPlan>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_ProductionPlan>(entity, t => t.Approver_at, dtpApprover_at,false);
           DataBindingHelper.BindData4TextBox<tb_ProductionPlan>(entity, t => t.CloseCaseOpinions, txtCloseCaseOpinions, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



