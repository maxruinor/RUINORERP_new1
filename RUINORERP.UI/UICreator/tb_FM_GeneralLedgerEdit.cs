
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:27
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
    /// 总账表来源于凭证分类汇总是财务报表的基础数据数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_FM_GeneralLedgerEdit:UserControl
    {
     public tb_FM_GeneralLedgerEdit() {
     
             
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_FM_GeneralLedger UIToEntity()
        {
        tb_FM_GeneralLedger entity = new tb_FM_GeneralLedger();
                     entity.GeneralLedgerID = Int64.Parse(txtGeneralLedgerID.Text);
                        entity.Subject_id = Int64.Parse(txtSubject_id.Text);
                        entity.Currency_ID = Int64.Parse(txtCurrency_ID.Text);
                        entity.TransactionDate = DateTime.Parse(txtTransactionDate.Text);
                        entity.TransactionType = txtTransactionType.Text ;
                       entity.Description = txtDescription.Text ;
                       entity.Amount = Decimal.Parse(txtAmount.Text);
                        entity.SourceBizType = Int32.Parse(txtSourceBizType.Text);
                        entity.SourceBillId = Int64.Parse(txtSourceBillId.Text);
                        entity.SourceBillNo = txtSourceBillNo.Text ;
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
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.PrintStatus = Int32.Parse(txtPrintStatus.Text);
                        entity.TransactionDirection = Int32.Parse(txtTransactionDirection.Text);
                                return entity;
}
        */

        
        private tb_FM_GeneralLedger _EditEntity;
        public tb_FM_GeneralLedger EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_FM_GeneralLedger entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_FM_GeneralLedger>(entity, t => t.GeneralLedgerID, txtGeneralLedgerID, BindDataType4TextBox.Qty,false);
          // DataBindingHelper.BindData4Cmb<tb_FM_Subject>(entity, k => k.Subject_id, v=>v.XXNAME, cmbSubject_id);
          // DataBindingHelper.BindData4Cmb<tb_Currency>(entity, k => k.Currency_ID, v=>v.XXNAME, cmbCurrency_ID);
           DataBindingHelper.BindData4DataTime<tb_FM_GeneralLedger>(entity, t => t.TransactionDate, dtpTransactionDate,false);
           DataBindingHelper.BindData4TextBox<tb_FM_GeneralLedger>(entity, t => t.TransactionType, txtTransactionType, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_GeneralLedger>(entity, t => t.Description, txtDescription, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_GeneralLedger>(entity, t => t.Amount.ToString(), txtAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_GeneralLedger>(entity, t => t.SourceBizType, txtSourceBizType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_GeneralLedger>(entity, t => t.SourceBillId, txtSourceBillId, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_GeneralLedger>(entity, t => t.SourceBillNo, txtSourceBillNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_FM_GeneralLedger>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_FM_GeneralLedger>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_FM_GeneralLedger>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_FM_GeneralLedger>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_GeneralLedger>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4TextBox<tb_FM_GeneralLedger>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_GeneralLedger>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_FM_GeneralLedger>(entity, t => t.Approver_at, dtpApprover_at,false);
           //default  DataBindingHelper.BindData4TextBox<tb_FM_GeneralLedger>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_GeneralLedger>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4TextBox<tb_FM_GeneralLedger>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_GeneralLedger>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_GeneralLedger>(entity, t => t.TransactionDirection, txtTransactionDirection, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



