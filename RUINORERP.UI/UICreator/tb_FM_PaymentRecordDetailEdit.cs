
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/01/2025 12:16:52
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
    /// 收付款记录明细表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_FM_PaymentRecordDetailEdit:UserControl
    {
     public tb_FM_PaymentRecordDetailEdit() {
     
             
                    InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_FM_PaymentRecordDetail UIToEntity()
        {
        tb_FM_PaymentRecordDetail entity = new tb_FM_PaymentRecordDetail();
                     entity.PaymentId = Int64.Parse(txtPaymentId.Text);
                        entity.SourceBizType = Int32.Parse(txtSourceBizType.Text);
                        entity.SourceBilllId = Int64.Parse(txtSourceBilllId.Text);
                        entity.SourceBillNo = txtSourceBillNo.Text ;
                       entity.IsFromPlatform = Boolean.Parse(txtIsFromPlatform.Text);
                        entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.ProjectGroup_ID = Int64.Parse(txtProjectGroup_ID.Text);
                        entity.Currency_ID = Int64.Parse(txtCurrency_ID.Text);
                        entity.ExchangeRate = Decimal.Parse(txtExchangeRate.Text);
                        entity.ForeignAmount = Decimal.Parse(txtForeignAmount.Text);
                        entity.LocalAmount = Decimal.Parse(txtLocalAmount.Text);
                        entity.Summary = txtSummary.Text ;
                               return entity;
}
        */

        
        private tb_FM_PaymentRecordDetail _EditEntity;
        public tb_FM_PaymentRecordDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_FM_PaymentRecordDetail entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_FM_PaymentRecord>(entity, k => k.PaymentId, v=>v.XXNAME, cmbPaymentId);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecordDetail>(entity, t => t.SourceBizType, txtSourceBizType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecordDetail>(entity, t => t.SourceBilllId, txtSourceBilllId, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecordDetail>(entity, t => t.SourceBillNo, txtSourceBillNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_PaymentRecordDetail>(entity, t => t.IsFromPlatform, chkIsFromPlatform, false);
          // DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v=>v.XXNAME, cmbDepartmentID);
          // DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v=>v.XXNAME, cmbProjectGroup_ID);
          // DataBindingHelper.BindData4Cmb<tb_Currency>(entity, k => k.Currency_ID, v=>v.XXNAME, cmbCurrency_ID);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecordDetail>(entity, t => t.ExchangeRate.ToString(), txtExchangeRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecordDetail>(entity, t => t.ForeignAmount.ToString(), txtForeignAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecordDetail>(entity, t => t.LocalAmount.ToString(), txtLocalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PaymentRecordDetail>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



