
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/20/2025 16:08:04
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
    /// 损益费用单数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_FM_ProfitLossEdit:UserControl
    {
     public tb_FM_ProfitLossEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_FM_ProfitLoss UIToEntity()
        {
        tb_FM_ProfitLoss entity = new tb_FM_ProfitLoss();
                     entity.ProfitLossNo = txtProfitLossNo.Text ;
                       entity.SourceBizType = Int32.Parse(txtSourceBizType.Text);
                        entity.SourceBillId = Int64.Parse(txtSourceBillId.Text);
                        entity.SourceBillNo = txtSourceBillNo.Text ;
                       entity.ProfitLossDirection = Boolean.Parse(txtProfitLossDirection.Text);
                        entity.PostTime = DateTime.Parse(txtPostTime.Text);
                        entity.IsExpenseType = Boolean.Parse(txtIsExpenseType.Text);
                        entity.ProfitLossType = Int32.Parse(txtProfitLossType.Text);
                        entity.TotalAmount = Decimal.Parse(txtTotalAmount.Text);
                        entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.ProjectGroup_ID = Int64.Parse(txtProjectGroup_ID.Text);
                        entity.IsIncludeTax = Boolean.Parse(txtIsIncludeTax.Text);
                        entity.TaxTotalAmount = Decimal.Parse(txtTaxTotalAmount.Text);
                        entity.UntaxedTotalAmont = Decimal.Parse(txtUntaxedTotalAmont.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.Remark = txtRemark.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                        entity.ApprovalOpinions = txtApprovalOpinions.Text ;
                       entity.Approver_by = Int64.Parse(txtApprover_by.Text);
                        entity.Approver_at = DateTime.Parse(txtApprover_at.Text);
                        entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                        entity.ApprovalResults = Boolean.Parse(txtApprovalResults.Text);
                        entity.PrintStatus = Int32.Parse(txtPrintStatus.Text);
                                return entity;
}
        */

        
        private tb_FM_ProfitLoss _EditEntity;
        public tb_FM_ProfitLoss EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_FM_ProfitLoss entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_FM_ProfitLoss>(entity, t => t.ProfitLossNo, txtProfitLossNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ProfitLoss>(entity, t => t.SourceBizType, txtSourceBizType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ProfitLoss>(entity, t => t.SourceBillId, txtSourceBillId, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ProfitLoss>(entity, t => t.SourceBillNo, txtSourceBillNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_ProfitLoss>(entity, t => t.ProfitLossDirection, chkProfitLossDirection, false);
           DataBindingHelper.BindData4DataTime<tb_FM_ProfitLoss>(entity, t => t.PostTime, dtpPostTime,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_ProfitLoss>(entity, t => t.IsExpenseType, chkIsExpenseType, false);
           DataBindingHelper.BindData4TextBox<tb_FM_ProfitLoss>(entity, t => t.ProfitLossType, txtProfitLossType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ProfitLoss>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money,false);
          // DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v=>v.XXNAME, cmbDepartmentID);
          // DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v=>v.XXNAME, cmbProjectGroup_ID);
           DataBindingHelper.BindData4CheckBox<tb_FM_ProfitLoss>(entity, t => t.IsIncludeTax, chkIsIncludeTax, false);
           DataBindingHelper.BindData4TextBox<tb_FM_ProfitLoss>(entity, t => t.TaxTotalAmount.ToString(), txtTaxTotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ProfitLoss>(entity, t => t.UntaxedTotalAmont.ToString(), txtUntaxedTotalAmont, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ProfitLoss>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ProfitLoss>(entity, t => t.Remark, txtRemark, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_FM_ProfitLoss>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ProfitLoss>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_FM_ProfitLoss>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ProfitLoss>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_ProfitLoss>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4TextBox<tb_FM_ProfitLoss>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ProfitLoss>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_FM_ProfitLoss>(entity, t => t.Approver_at, dtpApprover_at,false);
           //default  DataBindingHelper.BindData4TextBox<tb_FM_ProfitLoss>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_ProfitLoss>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4TextBox<tb_FM_ProfitLoss>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



