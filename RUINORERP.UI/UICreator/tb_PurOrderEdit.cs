
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:07
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
    /// 采购订单，可能来自销售订单也可能来自生产需求也可以直接录数据数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_PurOrderEdit:UserControl
    {
     public tb_PurOrderEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_PurOrder UIToEntity()
        {
        tb_PurOrder entity = new tb_PurOrder();
                     entity.PurOrderNo = txtPurOrderNo.Text ;
                       entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.ProjectGroup_ID = Int64.Parse(txtProjectGroup_ID.Text);
                        entity.PayStatus = Int32.Parse(txtPayStatus.Text);
                        entity.Paytype_ID = Int64.Parse(txtPaytype_ID.Text);
                        entity.PayeeInfoID = Int64.Parse(txtPayeeInfoID.Text);
                        entity.SOrder_ID = Int64.Parse(txtSOrder_ID.Text);
                        entity.SOrderNo = txtSOrderNo.Text ;
                       entity.PDID = Int64.Parse(txtPDID.Text);
                        entity.PurDate = DateTime.Parse(txtPurDate.Text);
                        entity.PreDeliveryDate = DateTime.Parse(txtPreDeliveryDate.Text);
                        entity.TotalQty = Int32.Parse(txtTotalQty.Text);
                        entity.TotalUndeliveredQty = Int32.Parse(txtTotalUndeliveredQty.Text);
                        entity.ForeignShipCost = Decimal.Parse(txtForeignShipCost.Text);
                        entity.ShipCost = Decimal.Parse(txtShipCost.Text);
                        entity.TotalTaxAmount = Decimal.Parse(txtTotalTaxAmount.Text);
                        entity.TotalAmount = Decimal.Parse(txtTotalAmount.Text);
                        entity.ActualAmount = Decimal.Parse(txtActualAmount.Text);
                        entity.TotalUntaxedAmount = Decimal.Parse(txtTotalUntaxedAmount.Text);
                        entity.Arrival_date = DateTime.Parse(txtArrival_date.Text);
                        entity.IsIncludeTax = Boolean.Parse(txtIsIncludeTax.Text);
                        entity.KeepAccountsType = Int32.Parse(txtKeepAccountsType.Text);
                        entity.PrePay = Boolean.Parse(txtPrePay.Text);
                        entity.PrePayMoney = Decimal.Parse(txtPrePayMoney.Text);
                        entity.IsCustomizedOrder = Boolean.Parse(txtIsCustomizedOrder.Text);
                        entity.Account_id = Int64.Parse(txtAccount_id.Text);
                        entity.Currency_ID = Int64.Parse(txtCurrency_ID.Text);
                        entity.ExchangeRate = Decimal.Parse(txtExchangeRate.Text);
                        entity.ForeignTotalAmount = Decimal.Parse(txtForeignTotalAmount.Text);
                        entity.ForeignDeposit = Decimal.Parse(txtForeignDeposit.Text);
                        entity.Deposit = Decimal.Parse(txtDeposit.Text);
                        entity.TaxDeductionType = Int32.Parse(txtTaxDeductionType.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.CloseCaseOpinions = txtCloseCaseOpinions.Text ;
                       entity.ApprovalOpinions = txtApprovalOpinions.Text ;
                       entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                        entity.ApprovalResults = Boolean.Parse(txtApprovalResults.Text);
                        entity.Approver_by = Int64.Parse(txtApprover_by.Text);
                        entity.Approver_at = DateTime.Parse(txtApprover_at.Text);
                        entity.PrintStatus = Int32.Parse(txtPrintStatus.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.RefBillID = Int64.Parse(txtRefBillID.Text);
                        entity.RefNO = txtRefNO.Text ;
                       entity.RefBizType = Int32.Parse(txtRefBizType.Text);
                                return entity;
}
        */

        
        private tb_PurOrder _EditEntity;
        public tb_PurOrder EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_PurOrder entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.PurOrderNo, txtPurOrderNo, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
          // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
          // DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v=>v.XXNAME, cmbDepartmentID);
          // DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v=>v.XXNAME, cmbProjectGroup_ID);
           DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.PayStatus, txtPayStatus, BindDataType4TextBox.Qty,false);
          // DataBindingHelper.BindData4Cmb<tb_PaymentMethod>(entity, k => k.Paytype_ID, v=>v.XXNAME, cmbPaytype_ID);
          // DataBindingHelper.BindData4Cmb<tb_FM_PayeeInfo>(entity, k => k.PayeeInfoID, v=>v.XXNAME, cmbPayeeInfoID);
          // DataBindingHelper.BindData4Cmb<tb_SaleOrder>(entity, k => k.SOrder_ID, v=>v.XXNAME, cmbSOrder_ID);
           DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.SOrderNo, txtSOrderNo, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_ProductionDemand>(entity, k => k.PDID, v=>v.XXNAME, cmbPDID);
           DataBindingHelper.BindData4DataTime<tb_PurOrder>(entity, t => t.PurDate, dtpPurDate,false);
           DataBindingHelper.BindData4DataTime<tb_PurOrder>(entity, t => t.PreDeliveryDate, dtpPreDeliveryDate,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.TotalQty, txtTotalQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.TotalUndeliveredQty, txtTotalUndeliveredQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.ForeignShipCost.ToString(), txtForeignShipCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.ShipCost.ToString(), txtShipCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.TotalTaxAmount.ToString(), txtTotalTaxAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.ActualAmount.ToString(), txtActualAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.TotalUntaxedAmount.ToString(), txtTotalUntaxedAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4DataTime<tb_PurOrder>(entity, t => t.Arrival_date, dtpArrival_date,false);
           DataBindingHelper.BindData4CheckBox<tb_PurOrder>(entity, t => t.IsIncludeTax, chkIsIncludeTax, false);
           DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.KeepAccountsType, txtKeepAccountsType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_PurOrder>(entity, t => t.PrePay, chkPrePay, false);
           DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.PrePayMoney.ToString(), txtPrePayMoney, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_PurOrder>(entity, t => t.IsCustomizedOrder, chkIsCustomizedOrder, false);
          // DataBindingHelper.BindData4Cmb<tb_FM_Account>(entity, k => k.Account_id, v=>v.XXNAME, cmbAccount_id);
          // DataBindingHelper.BindData4Cmb<tb_Currency>(entity, k => k.Currency_ID, v=>v.XXNAME, cmbCurrency_ID);
           DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.ExchangeRate.ToString(), txtExchangeRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.ForeignTotalAmount.ToString(), txtForeignTotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.ForeignDeposit.ToString(), txtForeignDeposit, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.Deposit.ToString(), txtDeposit, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.TaxDeductionType, txtTaxDeductionType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.CloseCaseOpinions, txtCloseCaseOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           //default  DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_PurOrder>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_PurOrder>(entity, t => t.Approver_at, dtpApprover_at,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_PurOrder>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4DataTime<tb_PurOrder>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_PurOrder>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.RefBillID, txtRefBillID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.RefNO, txtRefNO, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrder>(entity, t => t.RefBizType, txtRefBizType, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



