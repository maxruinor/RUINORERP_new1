
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/20/2025 18:12:11
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
    /// 记录收款 与应收的匹配，核销表-支持多对多、行项级核销 数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_FM_PaymentSettlementEdit:UserControl
    {
     public tb_FM_PaymentSettlementEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_FM_PaymentSettlement UIToEntity()
        {
        tb_FM_PaymentSettlement entity = new tb_FM_PaymentSettlement();
                     entity.PaymentId = Int64.Parse(txtPaymentId.Text);
                        entity.BizType = Int32.Parse(txtBizType.Text);
                        entity.SourceBilllID = Int64.Parse(txtSourceBilllID.Text);
                        entity.SourceBillDetailID = Int64.Parse(txtSourceBillDetailID.Text);
                        entity.SourceBillNO = txtSourceBillNO.Text ;
                       entity.ReceivePaymentType = Int64.Parse(txtReceivePaymentType.Text);
                        entity.Account_id = Int64.Parse(txtAccount_id.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.Currency_ID = Int64.Parse(txtCurrency_ID.Text);
                        entity.ExchangeRate = Decimal.Parse(txtExchangeRate.Text);
                        entity.SettledForeignAmount = Decimal.Parse(txtSettledForeignAmount.Text);
                        entity.SettledLocalAmount = Decimal.Parse(txtSettledLocalAmount.Text);
                        entity.PamountInWords = txtPamountInWords.Text ;
                       entity.SettleDate = DateTime.Parse(txtSettleDate.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.SettlementType = Int32.Parse(txtSettlementType.Text);
                        entity.EvidenceImagePath = txtEvidenceImagePath.Text ;
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

        
        private tb_FM_PaymentSettlement _EditEntity;
        public tb_FM_PaymentSettlement EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_FM_PaymentSettlement entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_FM_PaymentRecord>(entity, k => k.PaymentId, v=>v.XXNAME, cmbPaymentId);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.BizType, txtBizType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.SourceBilllID, txtSourceBilllID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.SourceBillDetailID, txtSourceBillDetailID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.SourceBillNO, txtSourceBillNO, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.ReceivePaymentType, txtReceivePaymentType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.Account_id, txtAccount_id, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.Employee_ID, txtEmployee_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.CustomerVendor_ID, txtCustomerVendor_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.Currency_ID, txtCurrency_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.ExchangeRate.ToString(), txtExchangeRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.SettledForeignAmount.ToString(), txtSettledForeignAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.SettledLocalAmount.ToString(), txtSettledLocalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.PamountInWords, txtPamountInWords, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_FM_PaymentSettlement>(entity, t => t.SettleDate, dtpSettleDate,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.SettlementType, txtSettlementType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.EvidenceImagePath, txtEvidenceImagePath, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_FM_PaymentSettlement>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_FM_PaymentSettlement>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_PaymentSettlement>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_FM_PaymentSettlement>(entity, t => t.Approver_at, dtpApprover_at,false);
           //default  DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_PaymentSettlement>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



