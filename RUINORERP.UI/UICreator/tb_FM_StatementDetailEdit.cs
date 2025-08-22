
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/20/2025 16:08:14
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
    /// 对账单明细数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_FM_StatementDetailEdit:UserControl
    {
     public tb_FM_StatementDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_FM_StatementDetail UIToEntity()
        {
        tb_FM_StatementDetail entity = new tb_FM_StatementDetail();
                     entity.StatementId = Int64.Parse(txtStatementId.Text);
                        entity.ARAPId = Int64.Parse(txtARAPId.Text);
                        entity.ReceivePaymentType = Int32.Parse(txtReceivePaymentType.Text);
                        entity.Currency_ID = Int64.Parse(txtCurrency_ID.Text);
                        entity.ExchangeRate = Decimal.Parse(txtExchangeRate.Text);
                        entity.IncludedLocalAmount = Decimal.Parse(txtIncludedLocalAmount.Text);
                        entity.IncludedForeignAmount = Decimal.Parse(txtIncludedForeignAmount.Text);
                        entity.WrittenOffLocalAmount = Decimal.Parse(txtWrittenOffLocalAmount.Text);
                        entity.WrittenOffForeignAmount = Decimal.Parse(txtWrittenOffForeignAmount.Text);
                        entity.RemainingLocalAmount = Decimal.Parse(txtRemainingLocalAmount.Text);
                        entity.RemainingForeignAmount = Decimal.Parse(txtRemainingForeignAmount.Text);
                        entity.ARAPWriteOffStatus = Int32.Parse(txtARAPWriteOffStatus.Text);
                        entity.Summary = txtSummary.Text ;
                               return entity;
}
        */

        
        private tb_FM_StatementDetail _EditEntity;
        public tb_FM_StatementDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_FM_StatementDetail entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_FM_Statement>(entity, k => k.StatementId, v=>v.XXNAME, cmbStatementId);
          // DataBindingHelper.BindData4Cmb<tb_FM_ReceivablePayable>(entity, k => k.ARAPId, v=>v.XXNAME, cmbARAPId);
           DataBindingHelper.BindData4TextBox<tb_FM_StatementDetail>(entity, t => t.ReceivePaymentType, txtReceivePaymentType, BindDataType4TextBox.Qty,false);
          // DataBindingHelper.BindData4Cmb<tb_Currency>(entity, k => k.Currency_ID, v=>v.XXNAME, cmbCurrency_ID);
           DataBindingHelper.BindData4TextBox<tb_FM_StatementDetail>(entity, t => t.ExchangeRate.ToString(), txtExchangeRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_StatementDetail>(entity, t => t.IncludedLocalAmount.ToString(), txtIncludedLocalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_StatementDetail>(entity, t => t.IncludedForeignAmount.ToString(), txtIncludedForeignAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_StatementDetail>(entity, t => t.WrittenOffLocalAmount.ToString(), txtWrittenOffLocalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_StatementDetail>(entity, t => t.WrittenOffForeignAmount.ToString(), txtWrittenOffForeignAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_StatementDetail>(entity, t => t.RemainingLocalAmount.ToString(), txtRemainingLocalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_StatementDetail>(entity, t => t.RemainingForeignAmount.ToString(), txtRemainingForeignAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_StatementDetail>(entity, t => t.ARAPWriteOffStatus, txtARAPWriteOffStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_StatementDetail>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



