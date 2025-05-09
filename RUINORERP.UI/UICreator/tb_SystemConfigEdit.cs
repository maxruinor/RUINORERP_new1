
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/08/2025 09:57:44
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
    /// 系统配置表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_SystemConfigEdit:UserControl
    {
     public tb_SystemConfigEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_SystemConfig UIToEntity()
        {
        tb_SystemConfig entity = new tb_SystemConfig();
                     entity.QtyDataPrecision = Int32.Parse(txtQtyDataPrecision.Text);
                        entity.TaxRateDataPrecision = Int32.Parse(txtTaxRateDataPrecision.Text);
                        entity.MoneyDataPrecision = Int32.Parse(txtMoneyDataPrecision.Text);
                        entity.CheckNegativeInventory = Boolean.Parse(txtCheckNegativeInventory.Text);
                        entity.CostCalculationMethod = Int32.Parse(txtCostCalculationMethod.Text);
                        entity.ShowDebugInfo = Boolean.Parse(txtShowDebugInfo.Text);
                        entity.OwnershipControl = Boolean.Parse(txtOwnershipControl.Text);
                        entity.SaleBizLimited = Boolean.Parse(txtSaleBizLimited.Text);
                        entity.DepartBizLimited = Boolean.Parse(txtDepartBizLimited.Text);
                        entity.PurchsaeBizLimited = Boolean.Parse(txtPurchsaeBizLimited.Text);
                        entity.CurrencyDataPrecisionAutoAddZero = Boolean.Parse(txtCurrencyDataPrecisionAutoAddZero.Text);
                        entity.UseBarCode = Boolean.Parse(txtUseBarCode.Text);
                        entity.QueryPageLayoutCustomize = Boolean.Parse(txtQueryPageLayoutCustomize.Text);
                        entity.AutoApprovedSaleOrderAmount = Decimal.Parse(txtAutoApprovedSaleOrderAmount.Text);
                        entity.AutoApprovedPurOrderAmount = Decimal.Parse(txtAutoApprovedPurOrderAmount.Text);
                        entity.QueryGridColCustomize = Boolean.Parse(txtQueryGridColCustomize.Text);
                        entity.BillGridColCustomize = Boolean.Parse(txtBillGridColCustomize.Text);
                        entity.IsDebug = Boolean.Parse(txtIsDebug.Text);
                        entity.EnableVoucherModule = Boolean.Parse(txtEnableVoucherModule.Text);
                        entity.EnableContractModule = Boolean.Parse(txtEnableContractModule.Text);
                        entity.EnableInvoiceModule = Boolean.Parse(txtEnableInvoiceModule.Text);
                        entity.EnableMultiCurrency = Boolean.Parse(txtEnableMultiCurrency.Text);
                        entity.EnableFinancialModule = Boolean.Parse(txtEnableFinancialModule.Text);
                                return entity;
}
        */

        
        private tb_SystemConfig _EditEntity;
        public tb_SystemConfig EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_SystemConfig entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_SystemConfig>(entity, t => t.QtyDataPrecision, txtQtyDataPrecision, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SystemConfig>(entity, t => t.TaxRateDataPrecision, txtTaxRateDataPrecision, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SystemConfig>(entity, t => t.MoneyDataPrecision, txtMoneyDataPrecision, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_SystemConfig>(entity, t => t.CheckNegativeInventory, chkCheckNegativeInventory, false);
//有默认值
           DataBindingHelper.BindData4TextBox<tb_SystemConfig>(entity, t => t.CostCalculationMethod, txtCostCalculationMethod, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_SystemConfig>(entity, t => t.ShowDebugInfo, chkShowDebugInfo, false);
           DataBindingHelper.BindData4CheckBox<tb_SystemConfig>(entity, t => t.OwnershipControl, chkOwnershipControl, false);
//有默认值
           DataBindingHelper.BindData4CheckBox<tb_SystemConfig>(entity, t => t.SaleBizLimited, chkSaleBizLimited, false);
           DataBindingHelper.BindData4CheckBox<tb_SystemConfig>(entity, t => t.DepartBizLimited, chkDepartBizLimited, false);
           DataBindingHelper.BindData4CheckBox<tb_SystemConfig>(entity, t => t.PurchsaeBizLimited, chkPurchsaeBizLimited, false);
           DataBindingHelper.BindData4CheckBox<tb_SystemConfig>(entity, t => t.CurrencyDataPrecisionAutoAddZero, chkCurrencyDataPrecisionAutoAddZero, false);
//有默认值
           DataBindingHelper.BindData4CheckBox<tb_SystemConfig>(entity, t => t.UseBarCode, chkUseBarCode, false);
           DataBindingHelper.BindData4CheckBox<tb_SystemConfig>(entity, t => t.QueryPageLayoutCustomize, chkQueryPageLayoutCustomize, false);
           DataBindingHelper.BindData4TextBox<tb_SystemConfig>(entity, t => t.AutoApprovedSaleOrderAmount.ToString(), txtAutoApprovedSaleOrderAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SystemConfig>(entity, t => t.AutoApprovedPurOrderAmount.ToString(), txtAutoApprovedPurOrderAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_SystemConfig>(entity, t => t.QueryGridColCustomize, chkQueryGridColCustomize, false);
           DataBindingHelper.BindData4CheckBox<tb_SystemConfig>(entity, t => t.BillGridColCustomize, chkBillGridColCustomize, false);
           DataBindingHelper.BindData4CheckBox<tb_SystemConfig>(entity, t => t.IsDebug, chkIsDebug, false);
           DataBindingHelper.BindData4CheckBox<tb_SystemConfig>(entity, t => t.EnableVoucherModule, chkEnableVoucherModule, false);
//有默认值
           DataBindingHelper.BindData4CheckBox<tb_SystemConfig>(entity, t => t.EnableContractModule, chkEnableContractModule, false);
//有默认值
           DataBindingHelper.BindData4CheckBox<tb_SystemConfig>(entity, t => t.EnableInvoiceModule, chkEnableInvoiceModule, false);
           DataBindingHelper.BindData4CheckBox<tb_SystemConfig>(entity, t => t.EnableMultiCurrency, chkEnableMultiCurrency, false);
//有默认值
           DataBindingHelper.BindData4CheckBox<tb_SystemConfig>(entity, t => t.EnableFinancialModule, chkEnableFinancialModule, false);
//有默认值
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



