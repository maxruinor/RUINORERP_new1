
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/19/2024 00:34:12
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
    public partial class tb_FM_OtherExpenseDetailEdit:UserControl
    {
     public tb_FM_OtherExpenseDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_FM_OtherExpenseDetail UIToEntity()
        {
        tb_FM_OtherExpenseDetail entity = new tb_FM_OtherExpenseDetail();
                     entity.ExpenseMainID = Int64.Parse(txtExpenseMainID.Text);
                        entity.ExpenseName = txtExpenseName.Text ;
                       entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.ExpenseType_id = Int64.Parse(txtExpenseType_id.Text);
                        entity.Account_id = Int64.Parse(txtAccount_id.Text);
                        entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.Subject_id = Int64.Parse(txtSubject_id.Text);
                        entity.CheckOutDate = DateTime.Parse(txtCheckOutDate.Text);
                        entity.TotalAmount = Decimal.Parse(txtTotalAmount.Text);
                        entity.IncludeTax = Boolean.Parse(txtIncludeTax.Text);
                        entity.Summary = txtSummary.Text ;
                       entity.TaxAmount = Decimal.Parse(txtTaxAmount.Text);
                        entity.TaxRate = Decimal.Parse(txtTaxRate.Text);
                        entity.UntaxedAmount = Decimal.Parse(txtUntaxedAmount.Text);
                        entity.ProjectGroup_ID = Int64.Parse(txtProjectGroup_ID.Text);
                        entity.EvidenceImagePath = txtEvidenceImagePath.Text ;
                               return entity;
}
        */

        
        private tb_FM_OtherExpenseDetail _EditEntity;
        public tb_FM_OtherExpenseDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_FM_OtherExpenseDetail entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_FM_OtherExpense>(entity, k => k.ExpenseMainID, v=>v.XXNAME, cmbExpenseMainID);
Account_id主外字段不一致。Subject_id主外字段不一致。           DataBindingHelper.BindData4TextBox<tb_FM_OtherExpenseDetail>(entity, t => t.ExpenseName, txtExpenseName, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
Account_id主外字段不一致。Subject_id主外字段不一致。          // DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v=>v.XXNAME, cmbDepartmentID);
Account_id主外字段不一致。Subject_id主外字段不一致。          Account_id主外字段不一致。// DataBindingHelper.BindData4Cmb<tb_FM_ExpenseType>(entity, k => k.ExpenseType_id, v=>v.XXNAME, cmbExpenseType_id);
Subject_id主外字段不一致。          Account_id主外字段不一致。Subject_id主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_OtherExpenseDetail>(entity, t => t.Account_id, txtAccount_id, BindDataType4TextBox.Qty,false);
          // DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
Account_id主外字段不一致。Subject_id主外字段不一致。          Account_id主外字段不一致。Subject_id主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_OtherExpenseDetail>(entity, t => t.Subject_id, txtSubject_id, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_FM_OtherExpenseDetail>(entity, t => t.CheckOutDate, dtpCheckOutDate,false);
           DataBindingHelper.BindData4TextBox<tb_FM_OtherExpenseDetail>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CehckBox<tb_FM_OtherExpenseDetail>(entity, t => t.IncludeTax, chkIncludeTax, false);
           DataBindingHelper.BindData4TextBox<tb_FM_OtherExpenseDetail>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_OtherExpenseDetail>(entity, t => t.TaxAmount.ToString(), txtTaxAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_OtherExpenseDetail>(entity, t => t.TaxRate.ToString(), txtTaxRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_OtherExpenseDetail>(entity, t => t.UntaxedAmount.ToString(), txtUntaxedAmount, BindDataType4TextBox.Money,false);
          // DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v=>v.XXNAME, cmbProjectGroup_ID);
Account_id主外字段不一致。Subject_id主外字段不一致。           DataBindingHelper.BindData4TextBox<tb_FM_OtherExpenseDetail>(entity, t => t.EvidenceImagePath, txtEvidenceImagePath, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



