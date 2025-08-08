
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:13
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
    /// 销售出库单数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_SaleOutEdit:UserControl
    {
     public tb_SaleOutEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_SaleOut UIToEntity()
        {
        tb_SaleOut entity = new tb_SaleOut();
                     entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.SOrder_ID = Int64.Parse(txtSOrder_ID.Text);
                        entity.SaleOrderNo = txtSaleOrderNo.Text ;
                       entity.SaleOutNo = txtSaleOutNo.Text ;
                       entity.ProjectGroup_ID = Int64.Parse(txtProjectGroup_ID.Text);
                        entity.PayStatus = Int32.Parse(txtPayStatus.Text);
                        entity.Currency_ID = Int64.Parse(txtCurrency_ID.Text);
                        entity.ExchangeRate = Decimal.Parse(txtExchangeRate.Text);
                        entity.Paytype_ID = Int64.Parse(txtPaytype_ID.Text);
                        entity.RefundStatus = Int32.Parse(txtRefundStatus.Text);
                        entity.ForeignFreightIncome = Decimal.Parse(txtForeignFreightIncome.Text);
                        entity.FreightIncome = Decimal.Parse(txtFreightIncome.Text);
                        entity.TotalQty = Int32.Parse(txtTotalQty.Text);
                        entity.TotalAmount = Decimal.Parse(txtTotalAmount.Text);
                        entity.OutDate = DateTime.Parse(txtOutDate.Text);
                        entity.DeliveryDate = DateTime.Parse(txtDeliveryDate.Text);
                        entity.DueDate = DateTime.Parse(txtDueDate.Text);
                        entity.ShippingAddress = txtShippingAddress.Text ;
                       entity.ShippingWay = txtShippingWay.Text ;
                       entity.PlatformOrderNo = txtPlatformOrderNo.Text ;
                       entity.IsCustomizedOrder = Boolean.Parse(txtIsCustomizedOrder.Text);
                        entity.IsFromPlatform = Boolean.Parse(txtIsFromPlatform.Text);
                        entity.TrackNo = txtTrackNo.Text ;
                       entity.CustomerPONo = txtCustomerPONo.Text ;
                       entity.ForeignTotalAmount = Decimal.Parse(txtForeignTotalAmount.Text);
                        entity.CollectedMoney = Decimal.Parse(txtCollectedMoney.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.ApprovalOpinions = txtApprovalOpinions.Text ;
                       entity.Approver_by = Int64.Parse(txtApprover_by.Text);
                        entity.Approver_at = DateTime.Parse(txtApprover_at.Text);
                        entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                        entity.ApprovalResults = Boolean.Parse(txtApprovalResults.Text);
                        entity.KeepAccountsType = Int32.Parse(txtKeepAccountsType.Text);
                        entity.ForeignDeposit = Decimal.Parse(txtForeignDeposit.Text);
                        entity.Deposit = Decimal.Parse(txtDeposit.Text);
                        entity.FreightCost = Decimal.Parse(txtFreightCost.Text);
                        entity.TaxDeductionType = Int32.Parse(txtTaxDeductionType.Text);
                        entity.PrintStatus = Int32.Parse(txtPrintStatus.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.TotalCommissionAmount = Decimal.Parse(txtTotalCommissionAmount.Text);
                        entity.TotalCost = Decimal.Parse(txtTotalCost.Text);
                        entity.TaxRate = Decimal.Parse(txtTaxRate.Text);
                        entity.TotalTaxAmount = Decimal.Parse(txtTotalTaxAmount.Text);
                        entity.TotalUntaxedAmount = Decimal.Parse(txtTotalUntaxedAmount.Text);
                        entity.GenerateVouchers = Boolean.Parse(txtGenerateVouchers.Text);
                        entity.DiscountAmount = Decimal.Parse(txtDiscountAmount.Text);
                        entity.PrePayMoney = Decimal.Parse(txtPrePayMoney.Text);
                        entity.ReplaceOut = Boolean.Parse(txtReplaceOut.Text);
                                return entity;
}
        */

        
        private tb_SaleOut _EditEntity;
        public tb_SaleOut EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_SaleOut entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.Employee_ID, txtEmployee_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.CustomerVendor_ID, txtCustomerVendor_ID, BindDataType4TextBox.Qty,false);
          // DataBindingHelper.BindData4Cmb<tb_SaleOrder>(entity, k => k.SOrder_ID, v=>v.XXNAME, cmbSOrder_ID);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.SaleOrderNo, txtSaleOrderNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.SaleOutNo, txtSaleOutNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.ProjectGroup_ID, txtProjectGroup_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.PayStatus, txtPayStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.Currency_ID, txtCurrency_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.ExchangeRate.ToString(), txtExchangeRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.Paytype_ID, txtPaytype_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.RefundStatus, txtRefundStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.ForeignFreightIncome.ToString(), txtForeignFreightIncome, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.FreightIncome.ToString(), txtFreightIncome, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.TotalQty, txtTotalQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4DataTime<tb_SaleOut>(entity, t => t.OutDate, dtpOutDate,false);
           DataBindingHelper.BindData4DataTime<tb_SaleOut>(entity, t => t.DeliveryDate, dtpDeliveryDate,false);
           DataBindingHelper.BindData4DataTime<tb_SaleOut>(entity, t => t.DueDate, dtpDueDate,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.ShippingAddress, txtShippingAddress, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.ShippingWay, txtShippingWay, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.PlatformOrderNo, txtPlatformOrderNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_SaleOut>(entity, t => t.IsCustomizedOrder, chkIsCustomizedOrder, false);
           DataBindingHelper.BindData4CheckBox<tb_SaleOut>(entity, t => t.IsFromPlatform, chkIsFromPlatform, false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.TrackNo, txtTrackNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.CustomerPONo, txtCustomerPONo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.ForeignTotalAmount.ToString(), txtForeignTotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.CollectedMoney.ToString(), txtCollectedMoney, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_SaleOut>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4DataTime<tb_SaleOut>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_SaleOut>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_SaleOut>(entity, t => t.Approver_at, dtpApprover_at,false);
           //default  DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_SaleOut>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.KeepAccountsType, txtKeepAccountsType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.ForeignDeposit.ToString(), txtForeignDeposit, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.Deposit.ToString(), txtDeposit, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.FreightCost.ToString(), txtFreightCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.TaxDeductionType, txtTaxDeductionType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.TotalCommissionAmount.ToString(), txtTotalCommissionAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.TotalCost.ToString(), txtTotalCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.TaxRate.ToString(), txtTaxRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.TotalTaxAmount.ToString(), txtTotalTaxAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.TotalUntaxedAmount.ToString(), txtTotalUntaxedAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_SaleOut>(entity, t => t.GenerateVouchers, chkGenerateVouchers, false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.DiscountAmount.ToString(), txtDiscountAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOut>(entity, t => t.PrePayMoney.ToString(), txtPrePayMoney, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_SaleOut>(entity, t => t.ReplaceOut, chkReplaceOut, false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



