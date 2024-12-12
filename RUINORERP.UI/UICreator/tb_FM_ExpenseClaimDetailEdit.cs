
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/12/2024 11:32:10
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
    /// 费用报销单明细数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_FM_ExpenseClaimDetailEdit:UserControl
    {
     public tb_FM_ExpenseClaimDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_FM_ExpenseClaimDetail UIToEntity()
        {
        tb_FM_ExpenseClaimDetail entity = new tb_FM_ExpenseClaimDetail();
                     entity.ClaimMainID = Int64.Parse(txtClaimMainID.Text);
                        entity.ClaimName = txtClaimName.Text ;
                       entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.ExpenseType_id = Int64.Parse(txtExpenseType_id.Text);
                        entity.Account_id = Int64.Parse(txtAccount_id.Text);
                        entity.Subject_id = Int64.Parse(txtSubject_id.Text);
                        entity.ProjectGroup_ID = Int64.Parse(txtProjectGroup_ID.Text);
                        entity.TranDate = DateTime.Parse(txtTranDate.Text);
                        entity.TotalAmount = Decimal.Parse(txtTotalAmount.Text);
                        entity.IncludeTax = Boolean.Parse(txtIncludeTax.Text);
                        entity.Summary = txtSummary.Text ;
                       entity.TaxRate = Decimal.Parse(txtTaxRate.Text);
                        entity.TaxAmount = Decimal.Parse(txtTaxAmount.Text);
                        entity.UntaxedAmount = Decimal.Parse(txtUntaxedAmount.Text);
                        entity.EvidenceImagePath = txtEvidenceImagePath.Text ;
                               return entity;
}
        */

        
        private tb_FM_ExpenseClaimDetail _EditEntity;
        public tb_FM_ExpenseClaimDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_FM_ExpenseClaimDetail entity)
        {
        _EditEntity = entity;
                       Subject_id主外字段不一致。// DataBindingHelper.BindData4Cmb<tb_FM_ExpenseClaim>(entity, k => k.ClaimMainID, v=>v.XXNAME, cmbClaimMainID);
           DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaimDetail>(entity, t => t.ClaimName, txtClaimName, BindDataType4TextBox.Text,false);
          Subject_id主外字段不一致。// DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v=>v.XXNAME, cmbDepartmentID);
          // DataBindingHelper.BindData4Cmb<tb_FM_ExpenseType>(entity, k => k.ExpenseType_id, v=>v.XXNAME, cmbExpenseType_id);
Subject_id主外字段不一致。          // DataBindingHelper.BindData4Cmb<tb_FM_Account>(entity, k => k.Account_id, v=>v.XXNAME, cmbAccount_id);
Subject_id主外字段不一致。          Subject_id主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaimDetail>(entity, t => t.Subject_id, txtSubject_id, BindDataType4TextBox.Qty,false);
          // DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v=>v.XXNAME, cmbProjectGroup_ID);
Subject_id主外字段不一致。           DataBindingHelper.BindData4DataTime<tb_FM_ExpenseClaimDetail>(entity, t => t.TranDate, dtpTranDate,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaimDetail>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_ExpenseClaimDetail>(entity, t => t.IncludeTax, chkIncludeTax, false);
           DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaimDetail>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaimDetail>(entity, t => t.TaxRate.ToString(), txtTaxRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaimDetail>(entity, t => t.TaxAmount.ToString(), txtTaxAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaimDetail>(entity, t => t.UntaxedAmount.ToString(), txtUntaxedAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ExpenseClaimDetail>(entity, t => t.EvidenceImagePath, txtEvidenceImagePath, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



