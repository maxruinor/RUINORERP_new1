
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/24/2025 18:44:30
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
    /// 价格调整单数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_FM_PriceAdjustmentEdit:UserControl
    {
     public tb_FM_PriceAdjustmentEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_FM_PriceAdjustment UIToEntity()
        {
        tb_FM_PriceAdjustment entity = new tb_FM_PriceAdjustment();
                     entity.AdjustNo = txtAdjustNo.Text ;
                       entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.ReceivePaymentType = Int32.Parse(txtReceivePaymentType.Text);
                        entity.SourceBizType = Int32.Parse(txtSourceBizType.Text);
                        entity.SourceBillId = Int64.Parse(txtSourceBillId.Text);
                        entity.SourceBillNo = txtSourceBillNo.Text ;
                       entity.PayStatus = Int32.Parse(txtPayStatus.Text);
                        entity.Paytype_ID = Int64.Parse(txtPaytype_ID.Text);
                        entity.Currency_ID = Int64.Parse(txtCurrency_ID.Text);
                        entity.AdjustReason = txtAdjustReason.Text ;
                       entity.ExchangeRate = Decimal.Parse(txtExchangeRate.Text);
                        entity.AdjustDate = DateTime.Parse(txtAdjustDate.Text);
                        entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.ProjectGroup_ID = Int64.Parse(txtProjectGroup_ID.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.InvoiceId = Int64.Parse(txtInvoiceId.Text);
                        entity.Invoiced = Boolean.Parse(txtInvoiced.Text);
                        entity.TotalForeignDiffAmount = Decimal.Parse(txtTotalForeignDiffAmount.Text);
                        entity.TotalLocalDiffAmount = Decimal.Parse(txtTotalLocalDiffAmount.Text);
                        entity.IsIncludeTax = Boolean.Parse(txtIsIncludeTax.Text);
                        entity.TaxTotalDiffLocalAmount = Decimal.Parse(txtTaxTotalDiffLocalAmount.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.Remark = txtRemark.Text ;
                       entity.ApprovalOpinions = txtApprovalOpinions.Text ;
                       entity.Approver_by = Int64.Parse(txtApprover_by.Text);
                        entity.Approver_at = DateTime.Parse(txtApprover_at.Text);
                        entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                        entity.ApprovalResults = Boolean.Parse(txtApprovalResults.Text);
                        entity.PrintStatus = Int32.Parse(txtPrintStatus.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                                return entity;
}
        */

        
        private tb_FM_PriceAdjustment _EditEntity;
        public tb_FM_PriceAdjustment EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_FM_PriceAdjustment entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustment>(entity, t => t.AdjustNo, txtAdjustNo, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustment>(entity, t => t.ReceivePaymentType, txtReceivePaymentType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustment>(entity, t => t.SourceBizType, txtSourceBizType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustment>(entity, t => t.SourceBillId, txtSourceBillId, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustment>(entity, t => t.SourceBillNo, txtSourceBillNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustment>(entity, t => t.PayStatus, txtPayStatus, BindDataType4TextBox.Qty,false);
          // DataBindingHelper.BindData4Cmb<tb_PaymentMethod>(entity, k => k.Paytype_ID, v=>v.XXNAME, cmbPaytype_ID);
          // DataBindingHelper.BindData4Cmb<tb_Currency>(entity, k => k.Currency_ID, v=>v.XXNAME, cmbCurrency_ID);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustment>(entity, t => t.AdjustReason, txtAdjustReason, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustment>(entity, t => t.ExchangeRate.ToString(), txtExchangeRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4DataTime<tb_FM_PriceAdjustment>(entity, t => t.AdjustDate, dtpAdjustDate,false);
          // DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v=>v.XXNAME, cmbDepartmentID);
          // DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v=>v.XXNAME, cmbProjectGroup_ID);
          // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustment>(entity, t => t.InvoiceId, txtInvoiceId, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_PriceAdjustment>(entity, t => t.Invoiced, chkInvoiced, false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustment>(entity, t => t.TotalForeignDiffAmount.ToString(), txtTotalForeignDiffAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustment>(entity, t => t.TotalLocalDiffAmount.ToString(), txtTotalLocalDiffAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_PriceAdjustment>(entity, t => t.IsIncludeTax, chkIsIncludeTax, false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustment>(entity, t => t.TaxTotalDiffLocalAmount.ToString(), txtTaxTotalDiffLocalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustment>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustment>(entity, t => t.Remark, txtRemark, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustment>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustment>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_FM_PriceAdjustment>(entity, t => t.Approver_at, dtpApprover_at,false);
           //default  DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustment>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_PriceAdjustment>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustment>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_FM_PriceAdjustment>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustment>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_FM_PriceAdjustment>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustment>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_PriceAdjustment>(entity, t => t.isdeleted, chkisdeleted, false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



