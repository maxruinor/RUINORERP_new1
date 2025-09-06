
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/06/2025 15:41:52
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
    /// 应收应付明细数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_FM_ReceivablePayableDetailEdit:UserControl
    {
     public tb_FM_ReceivablePayableDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_FM_ReceivablePayableDetail UIToEntity()
        {
        tb_FM_ReceivablePayableDetail entity = new tb_FM_ReceivablePayableDetail();
                     entity.ARAPId = Int64.Parse(txtARAPId.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.property = txtproperty.Text ;
                       entity.ExpenseType_id = Int64.Parse(txtExpenseType_id.Text);
                        entity.ExpenseDescription = txtExpenseDescription.Text ;
                       entity.IncludeTax = Boolean.Parse(txtIncludeTax.Text);
                        entity.ExchangeRate = Decimal.Parse(txtExchangeRate.Text);
                        entity.UnitPrice = Decimal.Parse(txtUnitPrice.Text);
                        entity.Quantity = Decimal.Parse(txtQuantity.Text);
                        entity.CustomerPartNo = txtCustomerPartNo.Text ;
                       entity.Description = txtDescription.Text ;
                       entity.TaxRate = Decimal.Parse(txtTaxRate.Text);
                        entity.TaxLocalAmount = Decimal.Parse(txtTaxLocalAmount.Text);
                        entity.LocalPayableAmount = Decimal.Parse(txtLocalPayableAmount.Text);
                        entity.Summary = txtSummary.Text ;
                               return entity;
}
        */

        
        private tb_FM_ReceivablePayableDetail _EditEntity;
        public tb_FM_ReceivablePayableDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_FM_ReceivablePayableDetail entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_FM_ReceivablePayable>(entity, k => k.ARAPId, v=>v.XXNAME, cmbARAPId);
          // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
           DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayableDetail>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayableDetail>(entity, t => t.ExpenseType_id, txtExpenseType_id, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayableDetail>(entity, t => t.ExpenseDescription, txtExpenseDescription, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_ReceivablePayableDetail>(entity, t => t.IncludeTax, chkIncludeTax, false);
           DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayableDetail>(entity, t => t.ExchangeRate.ToString(), txtExchangeRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayableDetail>(entity, t => t.UnitPrice.ToString(), txtUnitPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayableDetail>(entity, t => t.Quantity.ToString(), txtQuantity, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayableDetail>(entity, t => t.CustomerPartNo, txtCustomerPartNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayableDetail>(entity, t => t.Description, txtDescription, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayableDetail>(entity, t => t.TaxRate.ToString(), txtTaxRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayableDetail>(entity, t => t.TaxLocalAmount.ToString(), txtTaxLocalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayableDetail>(entity, t => t.LocalPayableAmount.ToString(), txtLocalPayableAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ReceivablePayableDetail>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



