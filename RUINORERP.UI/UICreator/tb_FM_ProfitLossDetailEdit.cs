
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/20/2025 16:08:06
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
    /// 损益费用单数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_FM_ProfitLossDetailEdit:UserControl
    {
     public tb_FM_ProfitLossDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_FM_ProfitLossDetail UIToEntity()
        {
        tb_FM_ProfitLossDetail entity = new tb_FM_ProfitLossDetail();
                     entity.ProfitLossId = Int64.Parse(txtProfitLossId.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.property = txtproperty.Text ;
                       entity.ExpenseType_id = Int64.Parse(txtExpenseType_id.Text);
                        entity.ExpenseDescription = txtExpenseDescription.Text ;
                       entity.UnitPrice = Decimal.Parse(txtUnitPrice.Text);
                        entity.Quantity = Decimal.Parse(txtQuantity.Text);
                        entity.SubtotalAmont = Decimal.Parse(txtSubtotalAmont.Text);
                        entity.UntaxedSubtotalAmont = Decimal.Parse(txtUntaxedSubtotalAmont.Text);
                        entity.TaxRate = Decimal.Parse(txtTaxRate.Text);
                        entity.TaxSubtotalAmont = Decimal.Parse(txtTaxSubtotalAmont.Text);
                        entity.Summary = txtSummary.Text ;
                               return entity;
}
        */

        
        private tb_FM_ProfitLossDetail _EditEntity;
        public tb_FM_ProfitLossDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_FM_ProfitLossDetail entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_FM_ProfitLoss>(entity, k => k.ProfitLossId, v=>v.XXNAME, cmbProfitLossId);
          // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
           DataBindingHelper.BindData4TextBox<tb_FM_ProfitLossDetail>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_FM_ExpenseType>(entity, k => k.ExpenseType_id, v=>v.XXNAME, cmbExpenseType_id);
           DataBindingHelper.BindData4TextBox<tb_FM_ProfitLossDetail>(entity, t => t.ExpenseDescription, txtExpenseDescription, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ProfitLossDetail>(entity, t => t.UnitPrice.ToString(), txtUnitPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ProfitLossDetail>(entity, t => t.Quantity.ToString(), txtQuantity, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ProfitLossDetail>(entity, t => t.SubtotalAmont.ToString(), txtSubtotalAmont, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ProfitLossDetail>(entity, t => t.UntaxedSubtotalAmont.ToString(), txtUntaxedSubtotalAmont, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ProfitLossDetail>(entity, t => t.TaxRate.ToString(), txtTaxRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ProfitLossDetail>(entity, t => t.TaxSubtotalAmont.ToString(), txtTaxSubtotalAmont, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ProfitLossDetail>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



