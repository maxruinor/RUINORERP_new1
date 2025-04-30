
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/30/2025 15:18:05
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
    /// 收款付款记录表-记录所有资金流动一批订单可分账户分批付 记录真实资金流动，用户需确保其 与银行流水一致数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_FM_PaymentRecordEdit:UserControl
    {
     public tb_FM_PaymentRecordEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_FM_PaymentRecord UIToEntity()
        {
        tb_FM_PaymentRecord entity = new tb_FM_PaymentRecord();
                     entity.PaymentNo = txtPaymentNo.Text ;
                       entity.BizType = Int32.Parse(txtBizType.Text);
                        entity.SourceBilllID = Int64.Parse(txtSourceBilllID.Text);
                        entity.SourceBillNO = txtSourceBillNO.Text ;
                       entity.ReceivePaymentType = Int32.Parse(txtReceivePaymentType.Text);
                        entity.Account_id = Int64.Parse(txtAccount_id.Text);
                        entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.PayeeInfoID = Int64.Parse(txtPayeeInfoID.Text);
                        entity.PayeeAccountNo = txtPayeeAccountNo.Text ;
                       entity.Currency_ID = Int64.Parse(txtCurrency_ID.Text);
                        entity.ExchangeRate = Decimal.Parse(txtExchangeRate.Text);
                        entity.ForeignPaidAmount = Decimal.Parse(txtForeignPaidAmount.Text);
                        entity.LocalPaidAmount = Decimal.Parse(txtLocalPaidAmount.Text);
                        entity.PaymentDate = DateTime.Parse(txtPaymentDate.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.ProjectGroup_ID = Int64.Parse(txtProjectGroup_ID.Text);
                        entity.Paytype_ID = Int64.Parse(txtPaytype_ID.Text);
                        entity.PaymentStatus = Int64.Parse(txtPaymentStatus.Text);
                        entity.PaymentImagePath = txtPaymentImagePath.Text ;
                       entity.ReferenceNo = txtReferenceNo.Text ;
                       entity.IsReversed = Boolean.Parse(txtIsReversed.Text);
                        entity.ReversedPaymentId = Int64.Parse(txtReversedPaymentId.Text);
                        entity.ReversedPaymentNo = txtReversedPaymentNo.Text ;
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

        
        private tb_FM_PaymentRecord _EditEntity;
        public tb_FM_PaymentRecord EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_FM_PaymentRecord entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.PaymentNo, txtPaymentNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.BizType, txtBizType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.SourceBilllID, txtSourceBilllID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.SourceBillNO, txtSourceBillNO, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.ReceivePaymentType, txtReceivePaymentType, BindDataType4TextBox.Qty,false);
          // DataBindingHelper.BindData4Cmb<tb_FM_Account>(entity, k => k.Account_id, v=>v.XXNAME, cmbAccount_id);
          // DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
          // DataBindingHelper.BindData4Cmb<tb_FM_PayeeInfo>(entity, k => k.PayeeInfoID, v=>v.XXNAME, cmbPayeeInfoID);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.PayeeAccountNo, txtPayeeAccountNo, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_Currency>(entity, k => k.Currency_ID, v=>v.XXNAME, cmbCurrency_ID);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.ExchangeRate.ToString(), txtExchangeRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.ForeignPaidAmount.ToString(), txtForeignPaidAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.LocalPaidAmount.ToString(), txtLocalPaidAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4DataTime<tb_FM_PaymentRecord>(entity, t => t.PaymentDate, dtpPaymentDate,false);
          // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
          // DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v=>v.XXNAME, cmbDepartmentID);
          // DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v=>v.XXNAME, cmbProjectGroup_ID);
          // DataBindingHelper.BindData4Cmb<tb_PaymentMethod>(entity, k => k.Paytype_ID, v=>v.XXNAME, cmbPaytype_ID);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.PaymentStatus, txtPaymentStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.PaymentImagePath, txtPaymentImagePath, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.ReferenceNo, txtReferenceNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_PaymentRecord>(entity, t => t.IsReversed, chkIsReversed, false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.ReversedPaymentId, txtReversedPaymentId, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.ReversedPaymentNo, txtReversedPaymentNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.Remark, txtRemark, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_FM_PaymentRecord>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_FM_PaymentRecord>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_PaymentRecord>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_FM_PaymentRecord>(entity, t => t.Approver_at, dtpApprover_at,false);
           //default  DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_PaymentRecord>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



