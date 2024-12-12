
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/12/2024 11:32:07
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
    /// 费用报销单数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_FM_ExpenseClaimEdit:UserControl
    {
     public tb_FM_ExpenseClaimEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_FM_ExpenseClaim UIToEntity()
        {
        tb_FM_ExpenseClaim entity = new tb_FM_ExpenseClaim();
                     entity.ClaimNo = txtClaimNo.Text ;
                       entity.Currency_ID = Int64.Parse(txtCurrency_ID.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.PayeeInfoID = Int64.Parse(txtPayeeInfoID.Text);
                        entity.DocumentDate = DateTime.Parse(txtDocumentDate.Text);
                        entity.ClaimAmount = Decimal.Parse(txtClaimAmount.Text);
                        entity.ApprovedAmount = Decimal.Parse(txtApprovedAmount.Text);
                        entity.IncludeTax = Boolean.Parse(txtIncludeTax.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.TaxAmount = Decimal.Parse(txtTaxAmount.Text);
                        entity.TaxRate = Decimal.Parse(txtTaxRate.Text);
                        entity.UntaxedAmount = Decimal.Parse(txtUntaxedAmount.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.ApprovalOpinions = txtApprovalOpinions.Text ;
                       entity.Approver_by = Int64.Parse(txtApprover_by.Text);
                        entity.Approver_at = DateTime.Parse(txtApprover_at.Text);
                        entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                        entity.ApprovalResults = Boolean.Parse(txtApprovalResults.Text);
                        entity.PrintStatus = Int32.Parse(txtPrintStatus.Text);
                        entity.CloseCaseImagePath = txtCloseCaseImagePath.Text ;
                       entity.CloseCaseOpinions = txtCloseCaseOpinions.Text ;
                               return entity;
}
        */

        
        private tb_FM_ExpenseClaim _EditEntity;
        public tb_FM_ExpenseClaim EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_FM_ExpenseClaim entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.ClaimNo, txtClaimNo, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_Currency>(entity, k => k.Currency_ID, v=>v.XXNAME, cmbCurrency_ID);
          // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
          // DataBindingHelper.BindData4Cmb<tb_FM_PayeeInfo>(entity, k => k.PayeeInfoID, v=>v.XXNAME, cmbPayeeInfoID);
           DataBindingHelper.BindData4DataTime<tb_FM_ExpenseClaim>(entity, t => t.DocumentDate, dtpDocumentDate,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.ClaimAmount.ToString(), txtClaimAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.ApprovedAmount.ToString(), txtApprovedAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_ExpenseClaim>(entity, t => t.IncludeTax, chkIncludeTax, false);
           DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.TaxAmount.ToString(), txtTaxAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.TaxRate.ToString(), txtTaxRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.UntaxedAmount.ToString(), txtUntaxedAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4DataTime<tb_FM_ExpenseClaim>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_FM_ExpenseClaim>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_ExpenseClaim>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_FM_ExpenseClaim>(entity, t => t.Approver_at, dtpApprover_at,false);
           //default  DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_ExpenseClaim>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.CloseCaseImagePath, txtCloseCaseImagePath, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaim>(entity, t => t.CloseCaseOpinions, txtCloseCaseOpinions, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



