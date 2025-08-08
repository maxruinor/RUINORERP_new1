
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:15
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
    /// 销售出库退回单数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_SaleOutReEdit:UserControl
    {
     public tb_SaleOutReEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_SaleOutRe UIToEntity()
        {
        tb_SaleOutRe entity = new tb_SaleOutRe();
                     entity.ReturnNo = txtReturnNo.Text ;
                       entity.ProjectGroup_ID = Int64.Parse(txtProjectGroup_ID.Text);
                        entity.PayStatus = Int32.Parse(txtPayStatus.Text);
                        entity.Paytype_ID = Int64.Parse(txtPaytype_ID.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.SaleOut_MainID = Int64.Parse(txtSaleOut_MainID.Text);
                        entity.SaleOut_NO = txtSaleOut_NO.Text ;
                       entity.Currency_ID = Int64.Parse(txtCurrency_ID.Text);
                        entity.ExchangeRate = Decimal.Parse(txtExchangeRate.Text);
                        entity.ForeignTotalAmount = Decimal.Parse(txtForeignTotalAmount.Text);
                        entity.RefundStatus = Int32.Parse(txtRefundStatus.Text);
                        entity.TotalQty = Int32.Parse(txtTotalQty.Text);
                        entity.ActualRefundAmount = Decimal.Parse(txtActualRefundAmount.Text);
                        entity.TotalAmount = Decimal.Parse(txtTotalAmount.Text);
                        entity.ReturnDate = DateTime.Parse(txtReturnDate.Text);
                        entity.ForeignFreightIncome = Decimal.Parse(txtForeignFreightIncome.Text);
                        entity.FreightIncome = Decimal.Parse(txtFreightIncome.Text);
                        entity.FreightCost = Decimal.Parse(txtFreightCost.Text);
                        entity.TrackNo = txtTrackNo.Text ;
                       entity.OfflineRefund = Boolean.Parse(txtOfflineRefund.Text);
                        entity.IsFromPlatform = Boolean.Parse(txtIsFromPlatform.Text);
                        entity.PlatformOrderNo = txtPlatformOrderNo.Text ;
                       entity.IsCustomizedOrder = Boolean.Parse(txtIsCustomizedOrder.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.ReturnReason = txtReturnReason.Text ;
                       entity.TotalCommissionAmount = Decimal.Parse(txtTotalCommissionAmount.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.ApprovalOpinions = txtApprovalOpinions.Text ;
                       entity.Approver_by = Int64.Parse(txtApprover_by.Text);
                        entity.Approver_at = DateTime.Parse(txtApprover_at.Text);
                        entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                        entity.ApprovalResults = Boolean.Parse(txtApprovalResults.Text);
                        entity.RefundOnly = Boolean.Parse(txtRefundOnly.Text);
                        entity.IsIncludeTax = Boolean.Parse(txtIsIncludeTax.Text);
                        entity.KeepAccountsType = Int32.Parse(txtKeepAccountsType.Text);
                        entity.TaxDeductionType = Int32.Parse(txtTaxDeductionType.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.PrintStatus = Int32.Parse(txtPrintStatus.Text);
                        entity.GenerateVouchers = Boolean.Parse(txtGenerateVouchers.Text);
                                return entity;
}
        */

        
        private tb_SaleOutRe _EditEntity;
        public tb_SaleOutRe EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_SaleOutRe entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.ReturnNo, txtReturnNo, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v=>v.XXNAME, cmbProjectGroup_ID);
           DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.PayStatus, txtPayStatus, BindDataType4TextBox.Qty,false);
          // DataBindingHelper.BindData4Cmb<tb_PaymentMethod>(entity, k => k.Paytype_ID, v=>v.XXNAME, cmbPaytype_ID);
          // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
          // DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
          // DataBindingHelper.BindData4Cmb<tb_SaleOut>(entity, k => k.SaleOut_MainID, v=>v.XXNAME, cmbSaleOut_MainID);
           DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.SaleOut_NO, txtSaleOut_NO, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.Currency_ID, txtCurrency_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.ExchangeRate.ToString(), txtExchangeRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.ForeignTotalAmount.ToString(), txtForeignTotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.RefundStatus, txtRefundStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.TotalQty, txtTotalQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.ActualRefundAmount.ToString(), txtActualRefundAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4DataTime<tb_SaleOutRe>(entity, t => t.ReturnDate, dtpReturnDate,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.ForeignFreightIncome.ToString(), txtForeignFreightIncome, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.FreightIncome.ToString(), txtFreightIncome, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.FreightCost.ToString(), txtFreightCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.TrackNo, txtTrackNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_SaleOutRe>(entity, t => t.OfflineRefund, chkOfflineRefund, false);
           DataBindingHelper.BindData4CheckBox<tb_SaleOutRe>(entity, t => t.IsFromPlatform, chkIsFromPlatform, false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.PlatformOrderNo, txtPlatformOrderNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_SaleOutRe>(entity, t => t.IsCustomizedOrder, chkIsCustomizedOrder, false);
           DataBindingHelper.BindData4CheckBox<tb_SaleOutRe>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4DataTime<tb_SaleOutRe>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_SaleOutRe>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.ReturnReason, txtReturnReason, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.TotalCommissionAmount.ToString(), txtTotalCommissionAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_SaleOutRe>(entity, t => t.Approver_at, dtpApprover_at,false);
           //default  DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_SaleOutRe>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4CheckBox<tb_SaleOutRe>(entity, t => t.RefundOnly, chkRefundOnly, false);
           DataBindingHelper.BindData4CheckBox<tb_SaleOutRe>(entity, t => t.IsIncludeTax, chkIsIncludeTax, false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.KeepAccountsType, txtKeepAccountsType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.TaxDeductionType, txtTaxDeductionType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutRe>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_SaleOutRe>(entity, t => t.GenerateVouchers, chkGenerateVouchers, false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



