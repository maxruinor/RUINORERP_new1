
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:16
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
    /// 销售退货翻新物料明细表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_SaleOutReRefurbishedMaterialsDetailEdit:UserControl
    {
     public tb_SaleOutReRefurbishedMaterialsDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_SaleOutReRefurbishedMaterialsDetail UIToEntity()
        {
        tb_SaleOutReRefurbishedMaterialsDetail entity = new tb_SaleOutReRefurbishedMaterialsDetail();
                     entity.SaleOutRe_ID = Int64.Parse(txtSaleOutRe_ID.Text);
                        entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.Rack_ID = Int64.Parse(txtRack_ID.Text);
                        entity.property = txtproperty.Text ;
                       entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.Quantity = Int32.Parse(txtQuantity.Text);
                        entity.TransactionPrice = Decimal.Parse(txtTransactionPrice.Text);
                        entity.SubtotalTransAmount = Decimal.Parse(txtSubtotalTransAmount.Text);
                        entity.Summary = txtSummary.Text ;
                       entity.CustomerPartNo = txtCustomerPartNo.Text ;
                       entity.Cost = Decimal.Parse(txtCost.Text);
                        entity.SubtotalCostAmount = Decimal.Parse(txtSubtotalCostAmount.Text);
                        entity.TaxRate = Decimal.Parse(txtTaxRate.Text);
                        entity.SubtotalTaxAmount = Decimal.Parse(txtSubtotalTaxAmount.Text);
                        entity.SubtotalUntaxedAmount = Decimal.Parse(txtSubtotalUntaxedAmount.Text);
                        entity.Gift = Boolean.Parse(txtGift.Text);
                                return entity;
}
        */

        
        private tb_SaleOutReRefurbishedMaterialsDetail _EditEntity;
        public tb_SaleOutReRefurbishedMaterialsDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_SaleOutReRefurbishedMaterialsDetail entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_SaleOutRe>(entity, k => k.SaleOutRe_ID, v=>v.XXNAME, cmbSaleOutRe_ID);
          // DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
           DataBindingHelper.BindData4TextBox<tb_SaleOutReRefurbishedMaterialsDetail>(entity, t => t.Rack_ID, txtRack_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutReRefurbishedMaterialsDetail>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
           DataBindingHelper.BindData4TextBox<tb_SaleOutReRefurbishedMaterialsDetail>(entity, t => t.Quantity, txtQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutReRefurbishedMaterialsDetail>(entity, t => t.TransactionPrice.ToString(), txtTransactionPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutReRefurbishedMaterialsDetail>(entity, t => t.SubtotalTransAmount.ToString(), txtSubtotalTransAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutReRefurbishedMaterialsDetail>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutReRefurbishedMaterialsDetail>(entity, t => t.CustomerPartNo, txtCustomerPartNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutReRefurbishedMaterialsDetail>(entity, t => t.Cost.ToString(), txtCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutReRefurbishedMaterialsDetail>(entity, t => t.SubtotalCostAmount.ToString(), txtSubtotalCostAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutReRefurbishedMaterialsDetail>(entity, t => t.TaxRate.ToString(), txtTaxRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutReRefurbishedMaterialsDetail>(entity, t => t.SubtotalTaxAmount.ToString(), txtSubtotalTaxAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutReRefurbishedMaterialsDetail>(entity, t => t.SubtotalUntaxedAmount.ToString(), txtSubtotalUntaxedAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_SaleOutReRefurbishedMaterialsDetail>(entity, t => t.Gift, chkGift, false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



