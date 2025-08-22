
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/20/2025 16:08:06
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
    /// 应收应付表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_FM_ReceivablePayableEdit:UserControl
    {
     public tb_FM_ReceivablePayableEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_FM_ReceivablePayable UIToEntity()
        {
        tb_FM_ReceivablePayable entity = new tb_FM_ReceivablePayable();
                     entity.ARAPNo = txtARAPNo.Text ;
                       entity.SourceBizType = Int32.Parse(txtSourceBizType.Text);
                        entity.SourceBillId = Int64.Parse(txtSourceBillId.Text);
                        entity.SourceBillNo = txtSourceBillNo.Text ;
                       entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.Currency_ID = Int64.Parse(txtCurrency_ID.Text);
                        entity.IsExpenseType = Boolean.Parse(txtIsExpenseType.Text);
                        entity.IsForCommission = Boolean.Parse(txtIsForCommission.Text);
                        entity.IsFromPlatform = Boolean.Parse(txtIsFromPlatform.Text);
                        entity.PlatformOrderNo = txtPlatformOrderNo.Text ;
                       entity.Account_id = Int64.Parse(txtAccount_id.Text);
                        entity.PayeeInfoID = Int64.Parse(txtPayeeInfoID.Text);
                        entity.PayeeAccountNo = txtPayeeAccountNo.Text ;
                       entity.ExchangeRate = Decimal.Parse(txtExchangeRate.Text);
                        entity.ReceivePaymentType = Int32.Parse(txtReceivePaymentType.Text);
                        entity.ShippingFee = Decimal.Parse(txtShippingFee.Text);
                        entity.TotalForeignPayableAmount = Decimal.Parse(txtTotalForeignPayableAmount.Text);
                        entity.TotalLocalPayableAmount = Decimal.Parse(txtTotalLocalPayableAmount.Text);
                        entity.ForeignPaidAmount = Decimal.Parse(txtForeignPaidAmount.Text);
                        entity.LocalPaidAmount = Decimal.Parse(txtLocalPaidAmount.Text);
                        entity.ForeignBalanceAmount = Decimal.Parse(txtForeignBalanceAmount.Text);
                        entity.LocalBalanceAmount = Decimal.Parse(txtLocalBalanceAmount.Text);
                        entity.DueDate = DateTime.Parse(txtDueDate.Text);
                        entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.ProjectGroup_ID = Int64.Parse(txtProjectGroup_ID.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.InvoiceId = Int64.Parse(txtInvoiceId.Text);
                        entity.Invoiced = Boolean.Parse(txtInvoiced.Text);
                        entity.IsIncludeTax = Boolean.Parse(txtIsIncludeTax.Text);
                        entity.TaxTotalAmount = Decimal.Parse(txtTaxTotalAmount.Text);
                        entity.UntaxedTotalAmont = Decimal.Parse(txtUntaxedTotalAmont.Text);
                        entity.ARAPStatus = Int32.Parse(txtARAPStatus.Text);
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

        
        private tb_FM_ReceivablePayable _EditEntity;
        public tb_FM_ReceivablePayable EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_FM_ReceivablePayable entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.ARAPNo, txtARAPNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.SourceBizType, txtSourceBizType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.SourceBillId, txtSourceBillId, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.SourceBillNo, txtSourceBillNo, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
          // DataBindingHelper.BindData4Cmb<tb_Currency>(entity, k => k.Currency_ID, v=>v.XXNAME, cmbCurrency_ID);
           DataBindingHelper.BindData4CheckBox<tb_FM_ReceivablePayable>(entity, t => t.IsExpenseType, chkIsExpenseType, false);
           DataBindingHelper.BindData4CheckBox<tb_FM_ReceivablePayable>(entity, t => t.IsForCommission, chkIsForCommission, false);
           DataBindingHelper.BindData4CheckBox<tb_FM_ReceivablePayable>(entity, t => t.IsFromPlatform, chkIsFromPlatform, false);
           DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.PlatformOrderNo, txtPlatformOrderNo, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_FM_Account>(entity, k => k.Account_id, v=>v.XXNAME, cmbAccount_id);
          // DataBindingHelper.BindData4Cmb<tb_FM_PayeeInfo>(entity, k => k.PayeeInfoID, v=>v.XXNAME, cmbPayeeInfoID);
           DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.PayeeAccountNo, txtPayeeAccountNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.ExchangeRate.ToString(), txtExchangeRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.ReceivePaymentType, txtReceivePaymentType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.ShippingFee.ToString(), txtShippingFee, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.TotalForeignPayableAmount.ToString(), txtTotalForeignPayableAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.TotalLocalPayableAmount.ToString(), txtTotalLocalPayableAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.ForeignPaidAmount.ToString(), txtForeignPaidAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.LocalPaidAmount.ToString(), txtLocalPaidAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.ForeignBalanceAmount.ToString(), txtForeignBalanceAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.LocalBalanceAmount.ToString(), txtLocalBalanceAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4DataTime<tb_FM_ReceivablePayable>(entity, t => t.DueDate, dtpDueDate,false);
          // DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v=>v.XXNAME, cmbDepartmentID);
          // DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v=>v.XXNAME, cmbProjectGroup_ID);
          // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
          // DataBindingHelper.BindData4Cmb<tb_FM_Invoice>(entity, k => k.InvoiceId, v=>v.XXNAME, cmbInvoiceId);
           DataBindingHelper.BindData4CheckBox<tb_FM_ReceivablePayable>(entity, t => t.Invoiced, chkInvoiced, false);
           DataBindingHelper.BindData4CheckBox<tb_FM_ReceivablePayable>(entity, t => t.IsIncludeTax, chkIsIncludeTax, false);
           DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.TaxTotalAmount.ToString(), txtTaxTotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.UntaxedTotalAmont.ToString(), txtUntaxedTotalAmont, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.ARAPStatus, txtARAPStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.Remark, txtRemark, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_FM_ReceivablePayable>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_FM_ReceivablePayable>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_ReceivablePayable>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_FM_ReceivablePayable>(entity, t => t.Approver_at, dtpApprover_at,false);
           //default  DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_ReceivablePayable>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayable>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



