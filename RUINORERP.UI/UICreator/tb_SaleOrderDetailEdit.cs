
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:13
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
    /// 销售订单明细数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_SaleOrderDetailEdit:UserControl
    {
     public tb_SaleOrderDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_SaleOrderDetail UIToEntity()
        {
        tb_SaleOrderDetail entity = new tb_SaleOrderDetail();
                     entity.SOrder_ID = Int64.Parse(txtSOrder_ID.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.property = txtproperty.Text ;
                       entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.UnitPrice = Decimal.Parse(txtUnitPrice.Text);
                        entity.Quantity = Int32.Parse(txtQuantity.Text);
                        entity.Discount = Decimal.Parse(txtDiscount.Text);
                        entity.TransactionPrice = Decimal.Parse(txtTransactionPrice.Text);
                        entity.SubtotalTransAmount = Decimal.Parse(txtSubtotalTransAmount.Text);
                        entity.CustomizedCost = Decimal.Parse(txtCustomizedCost.Text);
                        entity.Cost = Decimal.Parse(txtCost.Text);
                        entity.SubtotalCostAmount = Decimal.Parse(txtSubtotalCostAmount.Text);
                        entity.TotalDeliveredQty = Int32.Parse(txtTotalDeliveredQty.Text);
                        entity.UnitCommissionAmount = Decimal.Parse(txtUnitCommissionAmount.Text);
                        entity.CommissionAmount = Decimal.Parse(txtCommissionAmount.Text);
                        entity.TaxRate = Decimal.Parse(txtTaxRate.Text);
                        entity.SubtotalTaxAmount = Decimal.Parse(txtSubtotalTaxAmount.Text);
                        entity.SubtotalUntaxedAmount = Decimal.Parse(txtSubtotalUntaxedAmount.Text);
                        entity.Summary = txtSummary.Text ;
                       entity.CustomerPartNo = txtCustomerPartNo.Text ;
                       entity.Gift = Boolean.Parse(txtGift.Text);
                        entity.TotalReturnedQty = Int32.Parse(txtTotalReturnedQty.Text);
                        entity.SaleFlagCode = txtSaleFlagCode.Text ;
                               return entity;
}
        */

        
        private tb_SaleOrderDetail _EditEntity;
        public tb_SaleOrderDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_SaleOrderDetail entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_SaleOrder>(entity, k => k.SOrder_ID, v=>v.XXNAME, cmbSOrder_ID);
          // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
           DataBindingHelper.BindData4TextBox<tb_SaleOrderDetail>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
           DataBindingHelper.BindData4TextBox<tb_SaleOrderDetail>(entity, t => t.UnitPrice.ToString(), txtUnitPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrderDetail>(entity, t => t.Quantity, txtQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrderDetail>(entity, t => t.Discount.ToString(), txtDiscount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrderDetail>(entity, t => t.TransactionPrice.ToString(), txtTransactionPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrderDetail>(entity, t => t.SubtotalTransAmount.ToString(), txtSubtotalTransAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrderDetail>(entity, t => t.CustomizedCost.ToString(), txtCustomizedCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrderDetail>(entity, t => t.Cost.ToString(), txtCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrderDetail>(entity, t => t.SubtotalCostAmount.ToString(), txtSubtotalCostAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrderDetail>(entity, t => t.TotalDeliveredQty, txtTotalDeliveredQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrderDetail>(entity, t => t.UnitCommissionAmount.ToString(), txtUnitCommissionAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrderDetail>(entity, t => t.CommissionAmount.ToString(), txtCommissionAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrderDetail>(entity, t => t.TaxRate.ToString(), txtTaxRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrderDetail>(entity, t => t.SubtotalTaxAmount.ToString(), txtSubtotalTaxAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrderDetail>(entity, t => t.SubtotalUntaxedAmount.ToString(), txtSubtotalUntaxedAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrderDetail>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrderDetail>(entity, t => t.CustomerPartNo, txtCustomerPartNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_SaleOrderDetail>(entity, t => t.Gift, chkGift, false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrderDetail>(entity, t => t.TotalReturnedQty, txtTotalReturnedQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOrderDetail>(entity, t => t.SaleFlagCode, txtSaleFlagCode, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



