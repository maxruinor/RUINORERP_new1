
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:27
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
    /// 费用报销统计分析数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class View_FM_ExpenseClaimItemsEdit:UserControl
    {
     public View_FM_ExpenseClaimItemsEdit() {
     
             
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        View_FM_ExpenseClaimItems UIToEntity()
        {
        View_FM_ExpenseClaimItems entity = new View_FM_ExpenseClaimItems();
                     entity.ClaimNo = txtClaimNo.Text ;
                       entity.DocumentDate = DateTime.Parse(txtDocumentDate.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.PayeeInfoID = Int64.Parse(txtPayeeInfoID.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.ApprovalOpinions = txtApprovalOpinions.Text ;
                       entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                        entity.ApprovalResults = Boolean.Parse(txtApprovalResults.Text);
                        entity.Approver_by = Int64.Parse(txtApprover_by.Text);
                        entity.Approver_at = DateTime.Parse(txtApprover_at.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.ClaimName = txtClaimName.Text ;
                       entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.ExpenseType_id = Int64.Parse(txtExpenseType_id.Text);
                        entity.Account_id = Int64.Parse(txtAccount_id.Text);
                        entity.Subject_id = Int64.Parse(txtSubject_id.Text);
                        entity.ProjectGroup_ID = Int64.Parse(txtProjectGroup_ID.Text);
                        entity.TranDate = DateTime.Parse(txtTranDate.Text);
                        entity.SingleAmount = Decimal.Parse(txtSingleAmount.Text);
                        entity.EvidenceImagePath = txtEvidenceImagePath.Text ;
                       entity.Summary = txtSummary.Text ;
                               return entity;
}
        */

        
        private View_FM_ExpenseClaimItems _EditEntity;
        public View_FM_ExpenseClaimItems EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(View_FM_ExpenseClaimItems entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<View_FM_ExpenseClaimItems>(entity, t => t.ClaimNo, txtClaimNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<View_FM_ExpenseClaimItems>(entity, t => t.DocumentDate, dtpDocumentDate,false);
           DataBindingHelper.BindData4TextBox<View_FM_ExpenseClaimItems>(entity, t => t.Employee_ID, txtEmployee_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FM_ExpenseClaimItems>(entity, t => t.PayeeInfoID, txtPayeeInfoID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FM_ExpenseClaimItems>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FM_ExpenseClaimItems>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FM_ExpenseClaimItems>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           //default  DataBindingHelper.BindData4TextBox<View_FM_ExpenseClaimItems>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<View_FM_ExpenseClaimItems>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4TextBox<View_FM_ExpenseClaimItems>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<View_FM_ExpenseClaimItems>(entity, t => t.Approver_at, dtpApprover_at,false);
           DataBindingHelper.BindData4TextBox<View_FM_ExpenseClaimItems>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_FM_ExpenseClaimItems>(entity, t => t.ClaimName, txtClaimName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_FM_ExpenseClaimItems>(entity, t => t.DepartmentID, txtDepartmentID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FM_ExpenseClaimItems>(entity, t => t.ExpenseType_id, txtExpenseType_id, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FM_ExpenseClaimItems>(entity, t => t.Account_id, txtAccount_id, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FM_ExpenseClaimItems>(entity, t => t.Subject_id, txtSubject_id, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FM_ExpenseClaimItems>(entity, t => t.ProjectGroup_ID, txtProjectGroup_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<View_FM_ExpenseClaimItems>(entity, t => t.TranDate, dtpTranDate,false);
           DataBindingHelper.BindData4TextBox<View_FM_ExpenseClaimItems>(entity, t => t.SingleAmount.ToString(), txtSingleAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_FM_ExpenseClaimItems>(entity, t => t.EvidenceImagePath, txtEvidenceImagePath, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_FM_ExpenseClaimItems>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



