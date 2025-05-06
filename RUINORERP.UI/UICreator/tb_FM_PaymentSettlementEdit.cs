
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/06/2025 10:30:38
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
    /// 记录收款 与应收的匹配，核销表 核销记录用于跟踪资金与债权债务的冲抵关系，确保财务数据可追溯。正常的收款，支付不需要保存核销记录数据编辑
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
                     entity.SettlementNo = txtSettlementNo.Text ;
                       entity.SourceBizType = Int32.Parse(txtSourceBizType.Text);
                        entity.SourceBillID = Int64.Parse(txtSourceBillID.Text);
                        entity.SourceBillNO = txtSourceBillNO.Text ;
                       entity.ExchangeRate = Decimal.Parse(txtExchangeRate.Text);
                        entity.TargetBizType = Int32.Parse(txtTargetBizType.Text);
                        entity.TargetBillID = Int64.Parse(txtTargetBillID.Text);
                        entity.TargetBillNO = txtTargetBillNO.Text ;
                       entity.ReceivePaymentType = Int32.Parse(txtReceivePaymentType.Text);
                        entity.Account_id = Int64.Parse(txtAccount_id.Text);
                        entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.SettledForeignAmount = Decimal.Parse(txtSettledForeignAmount.Text);
                        entity.SettledLocalAmount = Decimal.Parse(txtSettledLocalAmount.Text);
                        entity.IsAutoSettlement = Boolean.Parse(txtIsAutoSettlement.Text);
                        entity.IsReversed = Boolean.Parse(txtIsReversed.Text);
                        entity.ReversedSettlementID = Int64.Parse(txtReversedSettlementID.Text);
                        entity.Currency_ID = Int64.Parse(txtCurrency_ID.Text);
                        entity.SettleDate = DateTime.Parse(txtSettleDate.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.SettlementType = Int32.Parse(txtSettlementType.Text);
                        entity.EvidenceImagePath = txtEvidenceImagePath.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                                return entity;
}
        */

        
        private tb_FM_PaymentSettlement _EditEntity;
        public tb_FM_PaymentSettlement EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_FM_PaymentSettlement entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.SettlementNo, txtSettlementNo, BindDataType4TextBox.Text,false);
          ReversedSettlementID主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.SourceBizType, txtSourceBizType, BindDataType4TextBox.Qty,false);
          ReversedSettlementID主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.SourceBillID, txtSourceBillID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.SourceBillNO, txtSourceBillNO, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.ExchangeRate.ToString(), txtExchangeRate, BindDataType4TextBox.Money,false);
          ReversedSettlementID主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.TargetBizType, txtTargetBizType, BindDataType4TextBox.Qty,false);
          ReversedSettlementID主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.TargetBillID, txtTargetBillID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.TargetBillNO, txtTargetBillNO, BindDataType4TextBox.Text,false);
          ReversedSettlementID主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.ReceivePaymentType, txtReceivePaymentType, BindDataType4TextBox.Qty,false);
          // DataBindingHelper.BindData4Cmb<tb_FM_Account>(entity, k => k.Account_id, v=>v.XXNAME, cmbAccount_id);
ReversedSettlementID主外字段不一致。          ReversedSettlementID主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.CustomerVendor_ID, txtCustomerVendor_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.SettledForeignAmount.ToString(), txtSettledForeignAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.SettledLocalAmount.ToString(), txtSettledLocalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_PaymentSettlement>(entity, t => t.IsAutoSettlement, chkIsAutoSettlement, false);
           DataBindingHelper.BindData4CheckBox<tb_FM_PaymentSettlement>(entity, t => t.IsReversed, chkIsReversed, false);
          ReversedSettlementID主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.ReversedSettlementID, txtReversedSettlementID, BindDataType4TextBox.Qty,false);
          // DataBindingHelper.BindData4Cmb<tb_Currency>(entity, k => k.Currency_ID, v=>v.XXNAME, cmbCurrency_ID);
ReversedSettlementID主外字段不一致。           DataBindingHelper.BindData4DataTime<tb_FM_PaymentSettlement>(entity, t => t.SettleDate, dtpSettleDate,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
          ReversedSettlementID主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.SettlementType, txtSettlementType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.EvidenceImagePath, txtEvidenceImagePath, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_FM_PaymentSettlement>(entity, t => t.Created_at, dtpCreated_at,false);
          ReversedSettlementID主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



