
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/16/2025 12:02:49
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
    /// 币别换算表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_CurrencyExchangeRateEdit:UserControl
    {
     public tb_CurrencyExchangeRateEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_CurrencyExchangeRate UIToEntity()
        {
        tb_CurrencyExchangeRate entity = new tb_CurrencyExchangeRate();
                     entity.ConversionName = txtConversionName.Text ;
                       entity.BaseCurrencyID = Int64.Parse(txtBaseCurrencyID.Text);
                        entity.TargetCurrencyID = Int64.Parse(txtTargetCurrencyID.Text);
                        entity.EffectiveDate = DateTime.Parse(txtEffectiveDate.Text);
                        entity.ExpirationDate = DateTime.Parse(txtExpirationDate.Text);
                        entity.DefaultExchRate = Decimal.Parse(txtDefaultExchRate.Text);
                        entity.ExecuteExchRate = Decimal.Parse(txtExecuteExchRate.Text);
                        entity.Is_enabled = Boolean.Parse(txtIs_enabled.Text);
                        entity.Is_available = Boolean.Parse(txtIs_available.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                                return entity;
}
        */

        
        private tb_CurrencyExchangeRate _EditEntity;
        public tb_CurrencyExchangeRate EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_CurrencyExchangeRate entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_CurrencyExchangeRate>(entity, t => t.ConversionName, txtConversionName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CurrencyExchangeRate>(entity, t => t.BaseCurrencyID, txtBaseCurrencyID, BindDataType4TextBox.Qty,false);
          DataBindingHelper.BindData4TextBox<tb_CurrencyExchangeRate>(entity, t => t.TargetCurrencyID, txtTargetCurrencyID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_CurrencyExchangeRate>(entity, t => t.EffectiveDate, dtpEffectiveDate,false);
           DataBindingHelper.BindData4DataTime<tb_CurrencyExchangeRate>(entity, t => t.ExpirationDate, dtpExpirationDate,false);
           DataBindingHelper.BindData4TextBox<tb_CurrencyExchangeRate>(entity, t => t.DefaultExchRate.ToString(), txtDefaultExchRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_CurrencyExchangeRate>(entity, t => t.ExecuteExchRate.ToString(), txtExecuteExchRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_CurrencyExchangeRate>(entity, t => t.Is_enabled, chkIs_enabled, false);
 
           DataBindingHelper.BindData4CheckBox<tb_CurrencyExchangeRate>(entity, t => t.Is_available, chkIs_available, false);
 
         
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



