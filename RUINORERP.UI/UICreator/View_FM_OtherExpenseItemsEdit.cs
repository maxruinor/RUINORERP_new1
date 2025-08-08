
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:28
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
    /// 其他费用统计分析数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class View_FM_OtherExpenseItemsEdit:UserControl
    {
     public View_FM_OtherExpenseItemsEdit() {
     
             
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        View_FM_OtherExpenseItems UIToEntity()
        {
        View_FM_OtherExpenseItems entity = new View_FM_OtherExpenseItems();
                     entity.ExpenseNo = txtExpenseNo.Text ;
                       entity.DocumentDate = DateTime.Parse(txtDocumentDate.Text);
                        entity.TotalAmount = Decimal.Parse(txtTotalAmount.Text);
                        entity.EXPOrINC = Boolean.Parse(txtEXPOrINC.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                        entity.ApprovalResults = Boolean.Parse(txtApprovalResults.Text);
                        entity.Currency_ID = Int64.Parse(txtCurrency_ID.Text);
                        entity.ExpenseName = txtExpenseName.Text ;
                       entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.ExpenseType_id = Int64.Parse(txtExpenseType_id.Text);
                        entity.Account_id = Int64.Parse(txtAccount_id.Text);
                        entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.SingleTotalAmount = Decimal.Parse(txtSingleTotalAmount.Text);
                        entity.Subject_id = Int64.Parse(txtSubject_id.Text);
                        entity.CheckOutDate = DateTime.Parse(txtCheckOutDate.Text);
                        entity.IncludeTax = Boolean.Parse(txtIncludeTax.Text);
                        entity.Summary = txtSummary.Text ;
                       entity.TaxAmount = Decimal.Parse(txtTaxAmount.Text);
                        entity.UntaxedAmount = Decimal.Parse(txtUntaxedAmount.Text);
                        entity.ProjectGroup_ID = Int64.Parse(txtProjectGroup_ID.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                                return entity;
}
        */

        
        private View_FM_OtherExpenseItems _EditEntity;
        public View_FM_OtherExpenseItems EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(View_FM_OtherExpenseItems entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<View_FM_OtherExpenseItems>(entity, t => t.ExpenseNo, txtExpenseNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<View_FM_OtherExpenseItems>(entity, t => t.DocumentDate, dtpDocumentDate,false);
           DataBindingHelper.BindData4TextBox<View_FM_OtherExpenseItems>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<View_FM_OtherExpenseItems>(entity, t => t.EXPOrINC, chkEXPOrINC, false);
           DataBindingHelper.BindData4TextBox<View_FM_OtherExpenseItems>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<View_FM_OtherExpenseItems>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4CheckBox<View_FM_OtherExpenseItems>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4TextBox<View_FM_OtherExpenseItems>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           //default  DataBindingHelper.BindData4TextBox<View_FM_OtherExpenseItems>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<View_FM_OtherExpenseItems>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4TextBox<View_FM_OtherExpenseItems>(entity, t => t.Currency_ID, txtCurrency_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FM_OtherExpenseItems>(entity, t => t.ExpenseName, txtExpenseName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_FM_OtherExpenseItems>(entity, t => t.Employee_ID, txtEmployee_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FM_OtherExpenseItems>(entity, t => t.DepartmentID, txtDepartmentID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FM_OtherExpenseItems>(entity, t => t.ExpenseType_id, txtExpenseType_id, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FM_OtherExpenseItems>(entity, t => t.Account_id, txtAccount_id, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FM_OtherExpenseItems>(entity, t => t.CustomerVendor_ID, txtCustomerVendor_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FM_OtherExpenseItems>(entity, t => t.SingleTotalAmount.ToString(), txtSingleTotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_FM_OtherExpenseItems>(entity, t => t.Subject_id, txtSubject_id, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<View_FM_OtherExpenseItems>(entity, t => t.CheckOutDate, dtpCheckOutDate,false);
           DataBindingHelper.BindData4CheckBox<View_FM_OtherExpenseItems>(entity, t => t.IncludeTax, chkIncludeTax, false);
           DataBindingHelper.BindData4TextBox<View_FM_OtherExpenseItems>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_FM_OtherExpenseItems>(entity, t => t.TaxAmount.ToString(), txtTaxAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_FM_OtherExpenseItems>(entity, t => t.UntaxedAmount.ToString(), txtUntaxedAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_FM_OtherExpenseItems>(entity, t => t.ProjectGroup_ID, txtProjectGroup_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FM_OtherExpenseItems>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



