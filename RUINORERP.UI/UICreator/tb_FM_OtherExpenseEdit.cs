
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/19/2024 00:34:11
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
    /// 其它费用记录表，账户管理，财务系统中使用,像基础资料一样单表操作简单数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_FM_OtherExpenseEdit:UserControl
    {
     public tb_FM_OtherExpenseEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_FM_OtherExpense UIToEntity()
        {
        tb_FM_OtherExpense entity = new tb_FM_OtherExpense();
                     entity.ExpenseNo = txtExpenseNo.Text ;
                       entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.DocumentDate = DateTime.Parse(txtDocumentDate.Text);
                        entity.TotalAmount = Decimal.Parse(txtTotalAmount.Text);
                        entity.EXPOrINC = Boolean.Parse(txtEXPOrINC.Text);
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
                        entity.ApprovedAmount = Decimal.Parse(txtApprovedAmount.Text);
                        entity.Currency_ID = Int64.Parse(txtCurrency_ID.Text);
                        entity.CloseCaseImagePath = txtCloseCaseImagePath.Text ;
                               return entity;
}
        */

        
        private tb_FM_OtherExpense _EditEntity;
        public tb_FM_OtherExpense EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_FM_OtherExpense entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_FM_OtherExpense>(entity, t => t.ExpenseNo, txtExpenseNo, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
           DataBindingHelper.BindData4DataTime<tb_FM_OtherExpense>(entity, t => t.DocumentDate, dtpDocumentDate,false);
           DataBindingHelper.BindData4TextBox<tb_FM_OtherExpense>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CehckBox<tb_FM_OtherExpense>(entity, t => t.EXPOrINC, chkEXPOrINC, false);
//有默认值
           DataBindingHelper.BindData4CehckBox<tb_FM_OtherExpense>(entity, t => t.IncludeTax, chkIncludeTax, false);
           DataBindingHelper.BindData4TextBox<tb_FM_OtherExpense>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_OtherExpense>(entity, t => t.TaxAmount.ToString(), txtTaxAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_OtherExpense>(entity, t => t.TaxRate.ToString(), txtTaxRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_OtherExpense>(entity, t => t.UntaxedAmount.ToString(), txtUntaxedAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4DataTime<tb_FM_OtherExpense>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_FM_OtherExpense>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_FM_OtherExpense>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_FM_OtherExpense>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CehckBox<tb_FM_OtherExpense>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4TextBox<tb_FM_OtherExpense>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_OtherExpense>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_OtherExpense>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_FM_OtherExpense>(entity, t => t.Approver_at, dtpApprover_at,false);
           //default  DataBindingHelper.BindData4TextBox<tb_FM_OtherExpense>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CehckBox<tb_FM_OtherExpense>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4TextBox<tb_FM_OtherExpense>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_OtherExpense>(entity, t => t.ApprovedAmount.ToString(), txtApprovedAmount, BindDataType4TextBox.Money,false);
          // DataBindingHelper.BindData4Cmb<tb_Currency>(entity, k => k.Currency_ID, v=>v.XXNAME, cmbCurrency_ID);
           DataBindingHelper.BindData4TextBox<tb_FM_OtherExpense>(entity, t => t.CloseCaseImagePath, txtCloseCaseImagePath, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



