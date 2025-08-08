
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:09
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
    /// 采购退货入库单数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_PurReturnEntryEdit:UserControl
    {
     public tb_PurReturnEntryEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_PurReturnEntry UIToEntity()
        {
        tb_PurReturnEntry entity = new tb_PurReturnEntry();
                     entity.PurReEntryNo = txtPurReEntryNo.Text ;
                       entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.Paytype_ID = Int64.Parse(txtPaytype_ID.Text);
                        entity.PurEntryRe_ID = Int64.Parse(txtPurEntryRe_ID.Text);
                        entity.PurEntryReNo = txtPurEntryReNo.Text ;
                       entity.TotalQty = Int32.Parse(txtTotalQty.Text);
                        entity.TotalTaxAmount = Decimal.Parse(txtTotalTaxAmount.Text);
                        entity.TotalAmount = Decimal.Parse(txtTotalAmount.Text);
                        entity.BillDate = DateTime.Parse(txtBillDate.Text);
                        entity.ShippingWay = txtShippingWay.Text ;
                       entity.TrackNo = txtTrackNo.Text ;
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

        
        private tb_PurReturnEntry _EditEntity;
        public tb_PurReturnEntry EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_PurReturnEntry entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_PurReturnEntry>(entity, t => t.PurReEntryNo, txtPurReEntryNo, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
          // DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v=>v.XXNAME, cmbDepartmentID);
          // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
          // DataBindingHelper.BindData4Cmb<tb_PaymentMethod>(entity, k => k.Paytype_ID, v=>v.XXNAME, cmbPaytype_ID);
          // DataBindingHelper.BindData4Cmb<tb_PurEntryRe>(entity, k => k.PurEntryRe_ID, v=>v.XXNAME, cmbPurEntryRe_ID);
           DataBindingHelper.BindData4TextBox<tb_PurReturnEntry>(entity, t => t.PurEntryReNo, txtPurEntryReNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PurReturnEntry>(entity, t => t.TotalQty, txtTotalQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_PurReturnEntry>(entity, t => t.TotalTaxAmount.ToString(), txtTotalTaxAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurReturnEntry>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4DataTime<tb_PurReturnEntry>(entity, t => t.BillDate, dtpBillDate,false);
           DataBindingHelper.BindData4TextBox<tb_PurReturnEntry>(entity, t => t.ShippingWay, txtShippingWay, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PurReturnEntry>(entity, t => t.TrackNo, txtTrackNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_PurReturnEntry>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4DataTime<tb_PurReturnEntry>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_PurReturnEntry>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_PurReturnEntry>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_PurReturnEntry>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_PurReturnEntry>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PurReturnEntry>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           //default  DataBindingHelper.BindData4TextBox<tb_PurReturnEntry>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_PurReturnEntry>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4CheckBox<tb_PurReturnEntry>(entity, t => t.IsIncludeTax, chkIsIncludeTax, false);
           DataBindingHelper.BindData4TextBox<tb_PurReturnEntry>(entity, t => t.KeepAccountsType, txtKeepAccountsType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_PurReturnEntry>(entity, t => t.Deposit.ToString(), txtDeposit, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurReturnEntry>(entity, t => t.TaxDeductionType, txtTaxDeductionType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_PurReturnEntry>(entity, t => t.TotalDiscountAmount.ToString(), txtTotalDiscountAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_PurReturnEntry>(entity, t => t.ReceiptInvoiceClosed, chkReceiptInvoiceClosed, false);
           DataBindingHelper.BindData4TextBox<tb_PurReturnEntry>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_PurReturnEntry>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_PurReturnEntry>(entity, t => t.Approver_at, dtpApprover_at,false);
           DataBindingHelper.BindData4TextBox<tb_PurReturnEntry>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_PurReturnEntry>(entity, t => t.GenerateVouchers, chkGenerateVouchers, false);
           DataBindingHelper.BindData4TextBox<tb_PurReturnEntry>(entity, t => t.VoucherNO, txtVoucherNO, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



