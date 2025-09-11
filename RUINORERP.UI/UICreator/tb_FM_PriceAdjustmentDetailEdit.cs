
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/11/2025 15:24:56
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
    /// 价格调整单明细数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_FM_PriceAdjustmentDetailEdit:UserControl
    {
     public tb_FM_PriceAdjustmentDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_FM_PriceAdjustmentDetail UIToEntity()
        {
        tb_FM_PriceAdjustmentDetail entity = new tb_FM_PriceAdjustmentDetail();
                     entity.AdjustId = Int64.Parse(txtAdjustId.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.property = txtproperty.Text ;
                       entity.Specifications = txtSpecifications.Text ;
                       entity.Unit_ID = Int64.Parse(txtUnit_ID.Text);
                        entity.Original_UnitPrice_NoTax = Decimal.Parse(txtOriginal_UnitPrice_NoTax.Text);
                        entity.Correct_UnitPrice_NoTax = Decimal.Parse(txtCorrect_UnitPrice_NoTax.Text);
                        entity.Original_TaxRate = Decimal.Parse(txtOriginal_TaxRate.Text);
                        entity.Correct_TaxRate = Decimal.Parse(txtCorrect_TaxRate.Text);
                        entity.Original_UnitPrice_WithTax = Decimal.Parse(txtOriginal_UnitPrice_WithTax.Text);
                        entity.Correct_UnitPrice_WithTax = Decimal.Parse(txtCorrect_UnitPrice_WithTax.Text);
                        entity.UnitPrice_NoTax_Diff = Decimal.Parse(txtUnitPrice_NoTax_Diff.Text);
                        entity.UnitPrice_WithTax_Diff = Decimal.Parse(txtUnitPrice_WithTax_Diff.Text);
                        entity.Quantity = Decimal.Parse(txtQuantity.Text);
                        entity.Original_TaxAmount = Decimal.Parse(txtOriginal_TaxAmount.Text);
                        entity.Correct_TaxAmount = Decimal.Parse(txtCorrect_TaxAmount.Text);
                        entity.TaxAmount_Diff = Decimal.Parse(txtTaxAmount_Diff.Text);
                        entity.TotalAmount_Diff_NoTax = Decimal.Parse(txtTotalAmount_Diff_NoTax.Text);
                        entity.TotalAmount_Diff_WithTax = Decimal.Parse(txtTotalAmount_Diff_WithTax.Text);
                        entity.TotalAmount_Diff = Decimal.Parse(txtTotalAmount_Diff.Text);
                        entity.CustomerPartNo = txtCustomerPartNo.Text ;
                       entity.AdjustReason = txtAdjustReason.Text ;
                       entity.Summary = txtSummary.Text ;
                               return entity;
}
        */

        
        private tb_FM_PriceAdjustmentDetail _EditEntity;
        public tb_FM_PriceAdjustmentDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_FM_PriceAdjustmentDetail entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_FM_PriceAdjustment>(entity, k => k.AdjustId, v=>v.XXNAME, cmbAdjustId);
          // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
          // DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustmentDetail>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustmentDetail>(entity, t => t.Specifications, txtSpecifications, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_Unit>(entity, k => k.Unit_ID, v=>v.XXNAME, cmbUnit_ID);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustmentDetail>(entity, t => t.Original_UnitPrice_NoTax.ToString(), txtOriginal_UnitPrice_NoTax, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustmentDetail>(entity, t => t.Correct_UnitPrice_NoTax.ToString(), txtCorrect_UnitPrice_NoTax, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustmentDetail>(entity, t => t.Original_TaxRate.ToString(), txtOriginal_TaxRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustmentDetail>(entity, t => t.Correct_TaxRate.ToString(), txtCorrect_TaxRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustmentDetail>(entity, t => t.Original_UnitPrice_WithTax.ToString(), txtOriginal_UnitPrice_WithTax, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustmentDetail>(entity, t => t.Correct_UnitPrice_WithTax.ToString(), txtCorrect_UnitPrice_WithTax, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustmentDetail>(entity, t => t.UnitPrice_NoTax_Diff.ToString(), txtUnitPrice_NoTax_Diff, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustmentDetail>(entity, t => t.UnitPrice_WithTax_Diff.ToString(), txtUnitPrice_WithTax_Diff, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustmentDetail>(entity, t => t.Quantity.ToString(), txtQuantity, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustmentDetail>(entity, t => t.Original_TaxAmount.ToString(), txtOriginal_TaxAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustmentDetail>(entity, t => t.Correct_TaxAmount.ToString(), txtCorrect_TaxAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustmentDetail>(entity, t => t.TaxAmount_Diff.ToString(), txtTaxAmount_Diff, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustmentDetail>(entity, t => t.TotalAmount_Diff_NoTax.ToString(), txtTotalAmount_Diff_NoTax, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustmentDetail>(entity, t => t.TotalAmount_Diff_WithTax.ToString(), txtTotalAmount_Diff_WithTax, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustmentDetail>(entity, t => t.TotalAmount_Diff.ToString(), txtTotalAmount_Diff, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustmentDetail>(entity, t => t.CustomerPartNo, txtCustomerPartNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustmentDetail>(entity, t => t.AdjustReason, txtAdjustReason, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustmentDetail>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



