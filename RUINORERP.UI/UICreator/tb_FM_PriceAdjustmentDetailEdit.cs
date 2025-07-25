
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/24/2025 20:27:12
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
                        entity.property = txtproperty.Text ;
                       entity.Specifications = txtSpecifications.Text ;
                       entity.Unit_ID = Int64.Parse(txtUnit_ID.Text);
                        entity.ExchangeRate = Decimal.Parse(txtExchangeRate.Text);
                        entity.OriginalUnitPrice = Decimal.Parse(txtOriginalUnitPrice.Text);
                        entity.AdjustedUnitPrice = Decimal.Parse(txtAdjustedUnitPrice.Text);
                        entity.DiffUnitPrice = Decimal.Parse(txtDiffUnitPrice.Text);
                        entity.Quantity = Decimal.Parse(txtQuantity.Text);
                        entity.CustomerPartNo = txtCustomerPartNo.Text ;
                       entity.SubtotalDiffLocalAmount = Decimal.Parse(txtSubtotalDiffLocalAmount.Text);
                        entity.Description = txtDescription.Text ;
                       entity.TaxRate = Decimal.Parse(txtTaxRate.Text);
                        entity.TaxDiffLocalAmount = Decimal.Parse(txtTaxDiffLocalAmount.Text);
                        entity.TaxSubtotalDiffLocalAmount = Decimal.Parse(txtTaxSubtotalDiffLocalAmount.Text);
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
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustmentDetail>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustmentDetail>(entity, t => t.Specifications, txtSpecifications, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_Unit>(entity, k => k.Unit_ID, v=>v.XXNAME, cmbUnit_ID);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustmentDetail>(entity, t => t.ExchangeRate.ToString(), txtExchangeRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustmentDetail>(entity, t => t.OriginalUnitPrice.ToString(), txtOriginalUnitPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustmentDetail>(entity, t => t.AdjustedUnitPrice.ToString(), txtAdjustedUnitPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustmentDetail>(entity, t => t.DiffUnitPrice.ToString(), txtDiffUnitPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustmentDetail>(entity, t => t.Quantity.ToString(), txtQuantity, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustmentDetail>(entity, t => t.CustomerPartNo, txtCustomerPartNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustmentDetail>(entity, t => t.SubtotalDiffLocalAmount.ToString(), txtSubtotalDiffLocalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustmentDetail>(entity, t => t.Description, txtDescription, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustmentDetail>(entity, t => t.TaxRate.ToString(), txtTaxRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustmentDetail>(entity, t => t.TaxDiffLocalAmount.ToString(), txtTaxDiffLocalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PriceAdjustmentDetail>(entity, t => t.TaxSubtotalDiffLocalAmount.ToString(), txtTaxSubtotalDiffLocalAmount, BindDataType4TextBox.Money,false);
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



