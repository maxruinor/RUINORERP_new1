
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/31/2024 20:20:00
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
    /// 销售订单数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_SaleOrderEdit:UserControl
    {
     public tb_SaleOrderEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_SaleOrder UIToEntity()
        {
        tb_SaleOrder entity = new tb_SaleOrder();
                     entity.SOrderNo = txtSOrderNo.Text ;
                       entity.PayStatus = Int32.Parse(txtPayStatus.Text);
                        entity.Paytype_ID = Int64.Parse(txtPaytype_ID.Text);
                        entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.ProjectGroup_ID = Int64.Parse(txtProjectGroup_ID.Text);
                        entity.ShipCost = Decimal.Parse(txtShipCost.Text);
                        entity.TotalQty = Int32.Parse(txtTotalQty.Text);
                        entity.TotalCost = Decimal.Parse(txtTotalCost.Text);
                        entity.TotalAmount = Decimal.Parse(txtTotalAmount.Text);
                        entity.TotalTaxAmount = Decimal.Parse(txtTotalTaxAmount.Text);
                        entity.PreDeliveryDate = DateTime.Parse(txtPreDeliveryDate.Text);
                        entity.SaleDate = DateTime.Parse(txtSaleDate.Text);
                        entity.DeliveryDate = DateTime.Parse(txtDeliveryDate.Text);
                        entity.ShippingAddress = txtShippingAddress.Text ;
                       entity.ShippingWay = txtShippingWay.Text ;
                       entity.TrackNo = txtTrackNo.Text ;
                       entity.CollectedMoney = Decimal.Parse(txtCollectedMoney.Text);
                        entity.PrePayMoney = Decimal.Parse(txtPrePayMoney.Text);
                        entity.Deposit = Decimal.Parse(txtDeposit.Text);
                        entity.TaxRate = Decimal.Parse(txtTaxRate.Text);
                        entity.DeliveryDateConfirm = Boolean.Parse(txtDeliveryDateConfirm.Text);
                        entity.TotalUntaxedAmount = Decimal.Parse(txtTotalUntaxedAmount.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.CloseCaseOpinions = txtCloseCaseOpinions.Text ;
                       entity.Notes = txtNotes.Text ;
                       entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                        entity.ApprovalOpinions = txtApprovalOpinions.Text ;
                       entity.Approver_by = Int64.Parse(txtApprover_by.Text);
                        entity.Approver_at = DateTime.Parse(txtApprover_at.Text);
                        entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                        entity.ApprovalResults = Boolean.Parse(txtApprovalResults.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.KeepAccountsType = Int32.Parse(txtKeepAccountsType.Text);
                        entity.TaxDeductionType = Int32.Parse(txtTaxDeductionType.Text);
                        entity.OrderPriority = Int32.Parse(txtOrderPriority.Text);
                        entity.PlatformOrderNo = txtPlatformOrderNo.Text ;
                       entity.PrintStatus = Int32.Parse(txtPrintStatus.Text);
                        entity.IsNotFromPlatform = Boolean.Parse(txtIsNotFromPlatform.Text);
                        entity.RefBillID = Int64.Parse(txtRefBillID.Text);
                        entity.RefNO = txtRefNO.Text ;
                       entity.RefBizType = Int32.Parse(txtRefBizType.Text);
                                return entity;
}
        */

        
        private tb_SaleOrder _EditEntity;
        public tb_SaleOrder EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_SaleOrder entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.SOrderNo, txtSOrderNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.PayStatus, txtPayStatus, BindDataType4TextBox.Qty,false);
          // DataBindingHelper.BindData4Cmb<tb_PaymentMethod>(entity, k => k.Paytype_ID, v=>v.XXNAME, cmbPaytype_ID);
          // DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
          // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
          // DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v=>v.XXNAME, cmbProjectGroup_ID);
           DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.ShipCost.ToString(), txtShipCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.TotalQty, txtTotalQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.TotalCost.ToString(), txtTotalCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.TotalTaxAmount.ToString(), txtTotalTaxAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4DataTime<tb_SaleOrder>(entity, t => t.PreDeliveryDate, dtpPreDeliveryDate,false);
           DataBindingHelper.BindData4DataTime<tb_SaleOrder>(entity, t => t.SaleDate, dtpSaleDate,false);
           DataBindingHelper.BindData4DataTime<tb_SaleOrder>(entity, t => t.DeliveryDate, dtpDeliveryDate,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.ShippingAddress, txtShippingAddress, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.ShippingWay, txtShippingWay, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.TrackNo, txtTrackNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.CollectedMoney.ToString(), txtCollectedMoney, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.PrePayMoney.ToString(), txtPrePayMoney, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.Deposit.ToString(), txtDeposit, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.TaxRate.ToString(), txtTaxRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CehckBox<tb_SaleOrder>(entity, t => t.DeliveryDateConfirm, chkDeliveryDateConfirm, false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.TotalUntaxedAmount.ToString(), txtTotalUntaxedAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4DataTime<tb_SaleOrder>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_SaleOrder>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.CloseCaseOpinions, txtCloseCaseOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CehckBox<tb_SaleOrder>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_SaleOrder>(entity, t => t.Approver_at, dtpApprover_at,false);
           //default  DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CehckBox<tb_SaleOrder>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.KeepAccountsType, txtKeepAccountsType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.TaxDeductionType, txtTaxDeductionType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.OrderPriority, txtOrderPriority, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.PlatformOrderNo, txtPlatformOrderNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CehckBox<tb_SaleOrder>(entity, t => t.IsNotFromPlatform, chkIsNotFromPlatform, false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.RefBillID, txtRefBillID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.RefNO, txtRefNO, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrder>(entity, t => t.RefBizType, txtRefBizType, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



