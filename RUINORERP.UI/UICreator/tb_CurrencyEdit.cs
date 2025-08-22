
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/20/2025 16:07:58
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
    /// 币别资料表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_CurrencyEdit:UserControl
    {
     public tb_CurrencyEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_Currency UIToEntity()
        {
        tb_Currency entity = new tb_Currency();
                     entity.Country = txtCountry.Text ;
                       entity.CurrencyCode = txtCurrencyCode.Text ;
                       entity.CurrencyName = txtCurrencyName.Text ;
                       entity.CurrencySymbol = txtCurrencySymbol.Text ;
                       entity.Is_BaseCurrency = Boolean.Parse(txtIs_BaseCurrency.Text);
                        entity.Is_enabled = Boolean.Parse(txtIs_enabled.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                                return entity;
}
        */

        
        private tb_Currency _EditEntity;
        public tb_Currency EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_Currency entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_Currency>(entity, t => t.Country, txtCountry, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Currency>(entity, t => t.CurrencyCode, txtCurrencyCode, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Currency>(entity, t => t.CurrencyName, txtCurrencyName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Currency>(entity, t => t.CurrencySymbol, txtCurrencySymbol, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_Currency>(entity, t => t.Is_BaseCurrency, chkIs_BaseCurrency, false);
           DataBindingHelper.BindData4CheckBox<tb_Currency>(entity, t => t.Is_enabled, chkIs_enabled, false);
//有默认值
           DataBindingHelper.BindData4DataTime<tb_Currency>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_Currency>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_Currency>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_Currency>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



