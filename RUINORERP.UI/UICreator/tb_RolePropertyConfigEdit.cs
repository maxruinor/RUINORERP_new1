
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:11
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
    /// 角色属性配置不同角色权限功能等不一样数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_RolePropertyConfigEdit:UserControl
    {
     public tb_RolePropertyConfigEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_RolePropertyConfig UIToEntity()
        {
        tb_RolePropertyConfig entity = new tb_RolePropertyConfig();
                     entity.RolePropertyName = txtRolePropertyName.Text ;
                       entity.QtyDataPrecision = Int32.Parse(txtQtyDataPrecision.Text);
                        entity.TaxRateDataPrecision = Int32.Parse(txtTaxRateDataPrecision.Text);
                        entity.MoneyDataPrecision = Int32.Parse(txtMoneyDataPrecision.Text);
                        entity.CurrencyDataPrecisionAutoAddZero = Boolean.Parse(txtCurrencyDataPrecisionAutoAddZero.Text);
                        entity.CostCalculationMethod = Int32.Parse(txtCostCalculationMethod.Text);
                        entity.ShowDebugInfo = Boolean.Parse(txtShowDebugInfo.Text);
                        entity.OwnershipControl = Boolean.Parse(txtOwnershipControl.Text);
                        entity.SaleBizLimited = Boolean.Parse(txtSaleBizLimited.Text);
                        entity.DepartBizLimited = Boolean.Parse(txtDepartBizLimited.Text);
                        entity.PurchsaeBizLimited = Boolean.Parse(txtPurchsaeBizLimited.Text);
                        entity.QueryPageLayoutCustomize = Boolean.Parse(txtQueryPageLayoutCustomize.Text);
                        entity.QueryGridColCustomize = Boolean.Parse(txtQueryGridColCustomize.Text);
                        entity.BillGridColCustomize = Boolean.Parse(txtBillGridColCustomize.Text);
                        entity.ExclusiveLimited = Boolean.Parse(txtExclusiveLimited.Text);
                        entity.DataBoardUnits = txtDataBoardUnits.Text ;
                               return entity;
}
        */

        
        private tb_RolePropertyConfig _EditEntity;
        public tb_RolePropertyConfig EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_RolePropertyConfig entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_RolePropertyConfig>(entity, t => t.RolePropertyName, txtRolePropertyName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_RolePropertyConfig>(entity, t => t.QtyDataPrecision, txtQtyDataPrecision, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_RolePropertyConfig>(entity, t => t.TaxRateDataPrecision, txtTaxRateDataPrecision, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_RolePropertyConfig>(entity, t => t.MoneyDataPrecision, txtMoneyDataPrecision, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_RolePropertyConfig>(entity, t => t.CurrencyDataPrecisionAutoAddZero, chkCurrencyDataPrecisionAutoAddZero, false);
//有默认值
           DataBindingHelper.BindData4TextBox<tb_RolePropertyConfig>(entity, t => t.CostCalculationMethod, txtCostCalculationMethod, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_RolePropertyConfig>(entity, t => t.ShowDebugInfo, chkShowDebugInfo, false);
           DataBindingHelper.BindData4CheckBox<tb_RolePropertyConfig>(entity, t => t.OwnershipControl, chkOwnershipControl, false);
           DataBindingHelper.BindData4CheckBox<tb_RolePropertyConfig>(entity, t => t.SaleBizLimited, chkSaleBizLimited, false);
           DataBindingHelper.BindData4CheckBox<tb_RolePropertyConfig>(entity, t => t.DepartBizLimited, chkDepartBizLimited, false);
           DataBindingHelper.BindData4CheckBox<tb_RolePropertyConfig>(entity, t => t.PurchsaeBizLimited, chkPurchsaeBizLimited, false);
           DataBindingHelper.BindData4CheckBox<tb_RolePropertyConfig>(entity, t => t.QueryPageLayoutCustomize, chkQueryPageLayoutCustomize, false);
           DataBindingHelper.BindData4CheckBox<tb_RolePropertyConfig>(entity, t => t.QueryGridColCustomize, chkQueryGridColCustomize, false);
           DataBindingHelper.BindData4CheckBox<tb_RolePropertyConfig>(entity, t => t.BillGridColCustomize, chkBillGridColCustomize, false);
           DataBindingHelper.BindData4CheckBox<tb_RolePropertyConfig>(entity, t => t.ExclusiveLimited, chkExclusiveLimited, false);
           DataBindingHelper.BindData4TextBox<tb_RolePropertyConfig>(entity, t => t.DataBoardUnits, txtDataBoardUnits, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



