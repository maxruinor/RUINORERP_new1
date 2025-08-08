
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:05
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
    /// 采购入库单 供应商接到采购订单后，向企业发货，用户在收到货物时，可以先检验，对合格品进行入库，也可以直接入库，形成采购入库单。为了保证清楚地记录进货情况，对进货的管理就很重要，而在我们的系统中，凭证、收付款是根据进货单自动一环扣一环地切制，故详细输入进货单资料后，存货的数量、成本会随着改变，收付帐款也会跟着你的立帐方式变化；凭证亦会随着“您是否立即产生凭证”变化。采购入库单可以由采购订单、借入单、在途物资单转入，也可以手动录入新增单据。数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_PurEntryReEdit:UserControl
    {
     public tb_PurEntryReEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_PurEntryRe UIToEntity()
        {
        tb_PurEntryRe entity = new tb_PurEntryRe();
                     entity.PurEntryReNo = txtPurEntryReNo.Text ;
                       entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.Paytype_ID = Int64.Parse(txtPaytype_ID.Text);
                        entity.PurEntryID = Int64.Parse(txtPurEntryID.Text);
                        entity.PurEntryNo = txtPurEntryNo.Text ;
                       entity.TotalQty = Int32.Parse(txtTotalQty.Text);
                        entity.TotalTaxAmount = Decimal.Parse(txtTotalTaxAmount.Text);
                        entity.TotalAmount = Decimal.Parse(txtTotalAmount.Text);
                        entity.ActualAmount = Decimal.Parse(txtActualAmount.Text);
                        entity.ProcessWay = Int32.Parse(txtProcessWay.Text);
                        entity.ReturnDate = DateTime.Parse(txtReturnDate.Text);
                        entity.ShippingWay = txtShippingWay.Text ;
                       entity.TrackNo = txtTrackNo.Text ;
                       entity.IsCustomizedOrder = Boolean.Parse(txtIsCustomizedOrder.Text);
                        entity.PayStatus = Int32.Parse(txtPayStatus.Text);
                        entity.Currency_ID = Int64.Parse(txtCurrency_ID.Text);
                        entity.ExchangeRate = Decimal.Parse(txtExchangeRate.Text);
                        entity.ForeignTotalAmount = Decimal.Parse(txtForeignTotalAmount.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.ApprovalOpinions = txtApprovalOpinions.Text ;
                       entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                        entity.ApprovalResults = Boolean.Parse(txtApprovalResults.Text);
                        entity.IsIncludeTax = Boolean.Parse(txtIsIncludeTax.Text);
                        entity.KeepAccountsType = Int32.Parse(txtKeepAccountsType.Text);
                        entity.Deposit = Decimal.Parse(txtDeposit.Text);
                        entity.TaxDeductionType = Int32.Parse(txtTaxDeductionType.Text);
                        entity.TotalDiscountAmount = Decimal.Parse(txtTotalDiscountAmount.Text);
                        entity.ReceiptInvoiceClosed = Boolean.Parse(txtReceiptInvoiceClosed.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.Approver_by = Int64.Parse(txtApprover_by.Text);
                        entity.Approver_at = DateTime.Parse(txtApprover_at.Text);
                        entity.PrintStatus = Int32.Parse(txtPrintStatus.Text);
                        entity.GenerateVouchers = Boolean.Parse(txtGenerateVouchers.Text);
                        entity.VoucherNO = txtVoucherNO.Text ;
                               return entity;
}
        */

        
        private tb_PurEntryRe _EditEntity;
        public tb_PurEntryRe EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_PurEntryRe entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, t => t.PurEntryReNo, txtPurEntryReNo, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
          // DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v=>v.XXNAME, cmbDepartmentID);
          // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
          // DataBindingHelper.BindData4Cmb<tb_PaymentMethod>(entity, k => k.Paytype_ID, v=>v.XXNAME, cmbPaytype_ID);
          // DataBindingHelper.BindData4Cmb<tb_PurEntry>(entity, k => k.PurEntryID, v=>v.XXNAME, cmbPurEntryID);
           DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, t => t.PurEntryNo, txtPurEntryNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, t => t.TotalQty, txtTotalQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, t => t.TotalTaxAmount.ToString(), txtTotalTaxAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, t => t.ActualAmount.ToString(), txtActualAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, t => t.ProcessWay, txtProcessWay, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_PurEntryRe>(entity, t => t.ReturnDate, dtpReturnDate,false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, t => t.ShippingWay, txtShippingWay, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, t => t.TrackNo, txtTrackNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_PurEntryRe>(entity, t => t.IsCustomizedOrder, chkIsCustomizedOrder, false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, t => t.PayStatus, txtPayStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, t => t.Currency_ID, txtCurrency_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, t => t.ExchangeRate.ToString(), txtExchangeRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, t => t.ForeignTotalAmount.ToString(), txtForeignTotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_PurEntryRe>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4DataTime<tb_PurEntryRe>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_PurEntryRe>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           //default  DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_PurEntryRe>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4CheckBox<tb_PurEntryRe>(entity, t => t.IsIncludeTax, chkIsIncludeTax, false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, t => t.KeepAccountsType, txtKeepAccountsType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, t => t.Deposit.ToString(), txtDeposit, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, t => t.TaxDeductionType, txtTaxDeductionType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, t => t.TotalDiscountAmount.ToString(), txtTotalDiscountAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_PurEntryRe>(entity, t => t.ReceiptInvoiceClosed, chkReceiptInvoiceClosed, false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_PurEntryRe>(entity, t => t.Approver_at, dtpApprover_at,false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_PurEntryRe>(entity, t => t.GenerateVouchers, chkGenerateVouchers, false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryRe>(entity, t => t.VoucherNO, txtVoucherNO, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



