
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/29/2025 11:22:25
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
    /// 记录收款 与应收的匹配，核销表数据编辑
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
                       entity.BizType = Int32.Parse(txtBizType.Text);
                        entity.SourceBillID = Int64.Parse(txtSourceBillID.Text);
                        entity.SourceBillNO = txtSourceBillNO.Text ;
                       entity.SourceBizType = Int32.Parse(txtSourceBizType.Text);
                        entity.SourceCurrencyID = Int64.Parse(txtSourceCurrencyID.Text);
                        entity.ExchangeRate = Decimal.Parse(txtExchangeRate.Text);
                        entity.TargetBizType = Int32.Parse(txtTargetBizType.Text);
                        entity.TargetBillID = Int64.Parse(txtTargetBillID.Text);
                        entity.TargetBillNO = txtTargetBillNO.Text ;
                       entity.TargetCurrencyID = Int64.Parse(txtTargetCurrencyID.Text);
                        entity.ReceivePaymentType = Int64.Parse(txtReceivePaymentType.Text);
                        entity.Account_id = Int64.Parse(txtAccount_id.Text);
                        entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.SettledForeignAmount = Decimal.Parse(txtSettledForeignAmount.Text);
                        entity.SettledLocalAmount = Decimal.Parse(txtSettledLocalAmount.Text);
                        entity.IsAutoSettlement = Boolean.Parse(txtIsAutoSettlement.Text);
                        entity.IsReversed = Boolean.Parse(txtIsReversed.Text);
                        entity.ReversedSettlementID = Int64.Parse(txtReversedSettlementID.Text);
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
          ReversedSettlementID主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.BizType, txtBizType, BindDataType4TextBox.Qty,false);
          ReversedSettlementID主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.SourceBillID, txtSourceBillID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.SourceBillNO, txtSourceBillNO, BindDataType4TextBox.Text,false);
          ReversedSettlementID主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.SourceBizType, txtSourceBizType, BindDataType4TextBox.Qty,false);
          ReversedSettlementID主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.SourceCurrencyID, txtSourceCurrencyID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.ExchangeRate.ToString(), txtExchangeRate, BindDataType4TextBox.Money,false);
          ReversedSettlementID主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.TargetBizType, txtTargetBizType, BindDataType4TextBox.Qty,false);
          ReversedSettlementID主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.TargetBillID, txtTargetBillID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.TargetBillNO, txtTargetBillNO, BindDataType4TextBox.Text,false);
          ReversedSettlementID主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.TargetCurrencyID, txtTargetCurrencyID, BindDataType4TextBox.Qty,false);
          ReversedSettlementID主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.ReceivePaymentType, txtReceivePaymentType, BindDataType4TextBox.Qty,false);
          ReversedSettlementID主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.Account_id, txtAccount_id, BindDataType4TextBox.Qty,false);
          ReversedSettlementID主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.CustomerVendor_ID, txtCustomerVendor_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.SettledForeignAmount.ToString(), txtSettledForeignAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.SettledLocalAmount.ToString(), txtSettledLocalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_PaymentSettlement>(entity, t => t.IsAutoSettlement, chkIsAutoSettlement, false);
           DataBindingHelper.BindData4CheckBox<tb_FM_PaymentSettlement>(entity, t => t.IsReversed, chkIsReversed, false);
          ReversedSettlementID主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_PaymentSettlement>(entity, t => t.ReversedSettlementID, txtReversedSettlementID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_FM_PaymentSettlement>(entity, t => t.SettleDate, dtpSettleDate,false);
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



