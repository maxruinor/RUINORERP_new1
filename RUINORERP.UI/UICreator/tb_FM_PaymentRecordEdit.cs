
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/24/2025 20:26:59
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
    /// 收付款记录表数据编辑
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
                       entity.ReceivePaymentType = Int32.Parse(txtReceivePaymentType.Text);
                        entity.Account_id = Int64.Parse(txtAccount_id.Text);
                        entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.PayeeInfoID = Int64.Parse(txtPayeeInfoID.Text);
                        entity.PayeeAccountNo = txtPayeeAccountNo.Text ;
                       entity.SourceBillNos = txtSourceBillNos.Text ;
                       entity.IsFromPlatform = Boolean.Parse(txtIsFromPlatform.Text);
                        entity.Currency_ID = Int64.Parse(txtCurrency_ID.Text);
                        entity.TotalForeignAmount = Decimal.Parse(txtTotalForeignAmount.Text);
                        entity.TotalLocalAmount = Decimal.Parse(txtTotalLocalAmount.Text);
                        entity.PaymentDate = DateTime.Parse(txtPaymentDate.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.Paytype_ID = Int64.Parse(txtPaytype_ID.Text);
                        entity.PaymentStatus = Int32.Parse(txtPaymentStatus.Text);
                        entity.PaymentImagePath = txtPaymentImagePath.Text ;
                       entity.ReferenceNo = txtReferenceNo.Text ;
                       entity.IsReversed = Boolean.Parse(txtIsReversed.Text);
                        entity.ReversedOriginalId = Int64.Parse(txtReversedOriginalId.Text);
                        entity.ReversedOriginalNo = txtReversedOriginalNo.Text ;
                       entity.ReversedByPaymentId = Int64.Parse(txtReversedByPaymentId.Text);
                        entity.ReversedByPaymentNo = txtReversedByPaymentNo.Text ;
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
          ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.ReceivePaymentType, txtReceivePaymentType, BindDataType4TextBox.Qty,false);
          // DataBindingHelper.BindData4Cmb<tb_FM_Account>(entity, k => k.Account_id, v=>v.XXNAME, cmbAccount_id);
ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。          // DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。          // DataBindingHelper.BindData4Cmb<tb_FM_PayeeInfo>(entity, k => k.PayeeInfoID, v=>v.XXNAME, cmbPayeeInfoID);
ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。           DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.PayeeAccountNo, txtPayeeAccountNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.SourceBillNos, txtSourceBillNos, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_PaymentRecord>(entity, t => t.IsFromPlatform, chkIsFromPlatform, false);
          // DataBindingHelper.BindData4Cmb<tb_Currency>(entity, k => k.Currency_ID, v=>v.XXNAME, cmbCurrency_ID);
ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。           DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.TotalForeignAmount.ToString(), txtTotalForeignAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.TotalLocalAmount.ToString(), txtTotalLocalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4DataTime<tb_FM_PaymentRecord>(entity, t => t.PaymentDate, dtpPaymentDate,false);
          // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。          // DataBindingHelper.BindData4Cmb<tb_PaymentMethod>(entity, k => k.Paytype_ID, v=>v.XXNAME, cmbPaytype_ID);
ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。          ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.PaymentStatus, txtPaymentStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.PaymentImagePath, txtPaymentImagePath, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.ReferenceNo, txtReferenceNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_PaymentRecord>(entity, t => t.IsReversed, chkIsReversed, false);
          ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.ReversedOriginalId, txtReversedOriginalId, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.ReversedOriginalNo, txtReversedOriginalNo, BindDataType4TextBox.Text,false);
          ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.ReversedByPaymentId, txtReversedByPaymentId, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.ReversedByPaymentNo, txtReversedByPaymentNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.Remark, txtRemark, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_FM_PaymentRecord>(entity, t => t.Created_at, dtpCreated_at,false);
          ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_FM_PaymentRecord>(entity, t => t.Modified_at, dtpModified_at,false);
          ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_PaymentRecord>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
          ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_FM_PaymentRecord>(entity, t => t.Approver_at, dtpApprover_at,false);
           //default  DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_PaymentRecord>(entity, t => t.ApprovalResults, chkApprovalResults, false);
          ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecord>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



