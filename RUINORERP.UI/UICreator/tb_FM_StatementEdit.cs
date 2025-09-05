
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/04/2025 18:02:21
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
    /// 对账单数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_FM_StatementEdit:UserControl
    {
     public tb_FM_StatementEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_FM_Statement UIToEntity()
        {
        tb_FM_Statement entity = new tb_FM_Statement();
                     entity.StatementNo = txtStatementNo.Text ;
                       entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.ARAPNos = txtARAPNos.Text ;
                       entity.Account_id = Int64.Parse(txtAccount_id.Text);
                        entity.PayeeInfoID = Int64.Parse(txtPayeeInfoID.Text);
                        entity.PayeeAccountNo = txtPayeeAccountNo.Text ;
                       entity.ReceivePaymentType = Int32.Parse(txtReceivePaymentType.Text);
                        entity.StartDate = DateTime.Parse(txtStartDate.Text);
                        entity.EndDate = DateTime.Parse(txtEndDate.Text);
                        entity.OpeningBalanceForeignAmount = Decimal.Parse(txtOpeningBalanceForeignAmount.Text);
                        entity.OpeningBalanceLocalAmount = Decimal.Parse(txtOpeningBalanceLocalAmount.Text);
                        entity.TotalReceivableForeignAmount = Decimal.Parse(txtTotalReceivableForeignAmount.Text);
                        entity.TotalReceivableLocalAmount = Decimal.Parse(txtTotalReceivableLocalAmount.Text);
                        entity.TotalPayableForeignAmount = Decimal.Parse(txtTotalPayableForeignAmount.Text);
                        entity.TotalPayableLocalAmount = Decimal.Parse(txtTotalPayableLocalAmount.Text);
                        entity.TotalReceivedForeignAmount = Decimal.Parse(txtTotalReceivedForeignAmount.Text);
                        entity.TotalReceivedLocalAmount = Decimal.Parse(txtTotalReceivedLocalAmount.Text);
                        entity.TotalPaidForeignAmount = Decimal.Parse(txtTotalPaidForeignAmount.Text);
                        entity.TotalPaidLocalAmount = Decimal.Parse(txtTotalPaidLocalAmount.Text);
                        entity.ClosingBalanceForeignAmount = Decimal.Parse(txtClosingBalanceForeignAmount.Text);
                        entity.ClosingBalanceLocalAmount = Decimal.Parse(txtClosingBalanceLocalAmount.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.StatementStatus = Int32.Parse(txtStatementStatus.Text);
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
                        entity.Summary = txtSummary.Text ;
                       entity.PrintStatus = Int32.Parse(txtPrintStatus.Text);
                                return entity;
}
        */

        
        private tb_FM_Statement _EditEntity;
        public tb_FM_Statement EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_FM_Statement entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.StatementNo, txtStatementNo, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
           DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.ARAPNos, txtARAPNos, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_FM_Account>(entity, k => k.Account_id, v=>v.XXNAME, cmbAccount_id);
          // DataBindingHelper.BindData4Cmb<tb_FM_PayeeInfo>(entity, k => k.PayeeInfoID, v=>v.XXNAME, cmbPayeeInfoID);
           DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.PayeeAccountNo, txtPayeeAccountNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.ReceivePaymentType, txtReceivePaymentType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_FM_Statement>(entity, t => t.StartDate, dtpStartDate,false);
           DataBindingHelper.BindData4DataTime<tb_FM_Statement>(entity, t => t.EndDate, dtpEndDate,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.OpeningBalanceForeignAmount.ToString(), txtOpeningBalanceForeignAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.OpeningBalanceLocalAmount.ToString(), txtOpeningBalanceLocalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.TotalReceivableForeignAmount.ToString(), txtTotalReceivableForeignAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.TotalReceivableLocalAmount.ToString(), txtTotalReceivableLocalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.TotalPayableForeignAmount.ToString(), txtTotalPayableForeignAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.TotalPayableLocalAmount.ToString(), txtTotalPayableLocalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.TotalReceivedForeignAmount.ToString(), txtTotalReceivedForeignAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.TotalReceivedLocalAmount.ToString(), txtTotalReceivedLocalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.TotalPaidForeignAmount.ToString(), txtTotalPaidForeignAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.TotalPaidLocalAmount.ToString(), txtTotalPaidLocalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.ClosingBalanceForeignAmount.ToString(), txtClosingBalanceForeignAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.ClosingBalanceLocalAmount.ToString(), txtClosingBalanceLocalAmount, BindDataType4TextBox.Money,false);
          // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
           DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.StatementStatus, txtStatementStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_FM_Statement>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_FM_Statement>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_Statement>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_FM_Statement>(entity, t => t.Approver_at, dtpApprover_at,false);
           //default  DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_Statement>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Statement>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



