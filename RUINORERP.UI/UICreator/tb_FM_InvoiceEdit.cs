
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:27
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
    /// 发票数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_FM_InvoiceEdit:UserControl
    {
     public tb_FM_InvoiceEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_FM_Invoice UIToEntity()
        {
        tb_FM_Invoice entity = new tb_FM_Invoice();
                     entity.InvoiceNo = txtInvoiceNo.Text ;
                       entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.BillingInfo_ID = Int64.Parse(txtBillingInfo_ID.Text);
                        entity.InvoiceDate = DateTime.Parse(txtInvoiceDate.Text);
                        entity.TaxAmount = Decimal.Parse(txtTaxAmount.Text);
                        entity.TotalWithTax = Decimal.Parse(txtTotalWithTax.Text);
                        entity.TotalAmount = Decimal.Parse(txtTotalAmount.Text);
                        entity.IsRed = Boolean.Parse(txtIsRed.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.ReceivePaymentType = Int32.Parse(txtReceivePaymentType.Text);
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
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.PrintStatus = Int32.Parse(txtPrintStatus.Text);
                                return entity;
}
        */

        
        private tb_FM_Invoice _EditEntity;
        public tb_FM_Invoice EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_FM_Invoice entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_FM_Invoice>(entity, t => t.InvoiceNo, txtInvoiceNo, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
          // DataBindingHelper.BindData4Cmb<tb_BillingInformation>(entity, k => k.BillingInfo_ID, v=>v.XXNAME, cmbBillingInfo_ID);
           DataBindingHelper.BindData4DataTime<tb_FM_Invoice>(entity, t => t.InvoiceDate, dtpInvoiceDate,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Invoice>(entity, t => t.TaxAmount.ToString(), txtTaxAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Invoice>(entity, t => t.TotalWithTax.ToString(), txtTotalWithTax, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Invoice>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_Invoice>(entity, t => t.IsRed, chkIsRed, false);
           DataBindingHelper.BindData4TextBox<tb_FM_Invoice>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Invoice>(entity, t => t.ReceivePaymentType, txtReceivePaymentType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_FM_Invoice>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Invoice>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_FM_Invoice>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Invoice>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_Invoice>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4TextBox<tb_FM_Invoice>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Invoice>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_FM_Invoice>(entity, t => t.Approver_at, dtpApprover_at,false);
           //default  DataBindingHelper.BindData4TextBox<tb_FM_Invoice>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_Invoice>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4TextBox<tb_FM_Invoice>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Invoice>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



