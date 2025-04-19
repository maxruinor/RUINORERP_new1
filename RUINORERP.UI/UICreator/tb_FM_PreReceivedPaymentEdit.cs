
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/18/2025 13:55:13
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
    /// 预收付款单数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_FM_PreReceivedPaymentEdit:UserControl
    {
     public tb_FM_PreReceivedPaymentEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_FM_PreReceivedPayment UIToEntity()
        {
        tb_FM_PreReceivedPayment entity = new tb_FM_PreReceivedPayment();
                     entity.Account_id = Int64.Parse(txtAccount_id.Text);
                        entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.ProjectGroup_ID = Int64.Parse(txtProjectGroup_ID.Text);
                        entity.Paytype_ID = Int64.Parse(txtPaytype_ID.Text);
                        entity.Currency_ID = Int64.Parse(txtCurrency_ID.Text);
                        entity.ExchangeRate = Decimal.Parse(txtExchangeRate.Text);
                        entity.PrePayDate = DateTime.Parse(txtPrePayDate.Text);
                        entity.PrePaymentReason = txtPrePaymentReason.Text ;
                       entity.SourceBill_BizType = Int32.Parse(txtSourceBill_BizType.Text);
                        entity.SourceBill_ID = Int64.Parse(txtSourceBill_ID.Text);
                        entity.SourceBillNO = txtSourceBillNO.Text ;
                       entity.FMPaymentStatus = Int32.Parse(txtFMPaymentStatus.Text);
                        entity.ForeignPrepaidAmount = Decimal.Parse(txtForeignPrepaidAmount.Text);
                        entity.LocalPrepaidAmount = Decimal.Parse(txtLocalPrepaidAmount.Text);
                        entity.LocalPrepaidAmountInWords = txtLocalPrepaidAmountInWords.Text ;
                       entity.ForeignPaidAmount = Decimal.Parse(txtForeignPaidAmount.Text);
                        entity.LocalPaidAmount = Decimal.Parse(txtLocalPaidAmount.Text);
                        entity.ForeignBalanceAmount = Decimal.Parse(txtForeignBalanceAmount.Text);
                        entity.LocalBalanceAmount = Decimal.Parse(txtLocalBalanceAmount.Text);
                        entity.ReceivePaymentType = Int32.Parse(txtReceivePaymentType.Text);
                        entity.PaymentImagePath = txtPaymentImagePath.Text ;
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

        
        private tb_FM_PreReceivedPayment _EditEntity;
        public tb_FM_PreReceivedPayment EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_FM_PreReceivedPayment entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_FM_Account>(entity, k => k.Account_id, v=>v.XXNAME, cmbAccount_id);
          // DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
          // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
          // DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v=>v.XXNAME, cmbDepartmentID);
          // DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v=>v.XXNAME, cmbProjectGroup_ID);
          // DataBindingHelper.BindData4Cmb<tb_PaymentMethod>(entity, k => k.Paytype_ID, v=>v.XXNAME, cmbPaytype_ID);
          // DataBindingHelper.BindData4Cmb<tb_Currency>(entity, k => k.Currency_ID, v=>v.XXNAME, cmbCurrency_ID);
           DataBindingHelper.BindData4TextBox<tb_FM_PreReceivedPayment>(entity, t => t.ExchangeRate.ToString(), txtExchangeRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4DataTime<tb_FM_PreReceivedPayment>(entity, t => t.PrePayDate, dtpPrePayDate,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PreReceivedPayment>(entity, t => t.PrePaymentReason, txtPrePaymentReason, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PreReceivedPayment>(entity, t => t.SourceBill_BizType, txtSourceBill_BizType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PreReceivedPayment>(entity, t => t.SourceBill_ID, txtSourceBill_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PreReceivedPayment>(entity, t => t.SourceBillNO, txtSourceBillNO, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PreReceivedPayment>(entity, t => t.FMPaymentStatus, txtFMPaymentStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PreReceivedPayment>(entity, t => t.ForeignPrepaidAmount.ToString(), txtForeignPrepaidAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PreReceivedPayment>(entity, t => t.LocalPrepaidAmount.ToString(), txtLocalPrepaidAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PreReceivedPayment>(entity, t => t.LocalPrepaidAmountInWords, txtLocalPrepaidAmountInWords, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PreReceivedPayment>(entity, t => t.ForeignPaidAmount.ToString(), txtForeignPaidAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PreReceivedPayment>(entity, t => t.LocalPaidAmount.ToString(), txtLocalPaidAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PreReceivedPayment>(entity, t => t.ForeignBalanceAmount.ToString(), txtForeignBalanceAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PreReceivedPayment>(entity, t => t.LocalBalanceAmount.ToString(), txtLocalBalanceAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PreReceivedPayment>(entity, t => t.ReceivePaymentType, txtReceivePaymentType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PreReceivedPayment>(entity, t => t.PaymentImagePath, txtPaymentImagePath, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PreReceivedPayment>(entity, t => t.Remark, txtRemark, BindDataType4TextBox.Text,false);
         
           DataBindingHelper.BindData4CheckBox<tb_FM_PreReceivedPayment>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4TextBox<tb_FM_PreReceivedPayment>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PreReceivedPayment>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_FM_PreReceivedPayment>(entity, t => t.Approver_at, dtpApprover_at,false);
           //default  DataBindingHelper.BindData4TextBox<tb_FM_PreReceivedPayment>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_PreReceivedPayment>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4TextBox<tb_FM_PreReceivedPayment>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }

        private void tb_FM_PreReceivedPaymentEdit_Load(object sender, EventArgs e)
        {

        }
    }
}



