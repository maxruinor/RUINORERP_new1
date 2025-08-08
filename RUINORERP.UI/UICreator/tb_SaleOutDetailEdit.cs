
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:14
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
    /// 销售出库明细数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_SaleOutDetailEdit:UserControl
    {
     public tb_SaleOutDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_SaleOutDetail UIToEntity()
        {
        tb_SaleOutDetail entity = new tb_SaleOutDetail();
                     entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.property = txtproperty.Text ;
                       entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.Rack_ID = Int64.Parse(txtRack_ID.Text);
                        entity.SaleOut_MainID = Int64.Parse(txtSaleOut_MainID.Text);
                        entity.Quantity = Int32.Parse(txtQuantity.Text);
                        entity.UnitPrice = Decimal.Parse(txtUnitPrice.Text);
                        entity.Discount = Decimal.Parse(txtDiscount.Text);
                        entity.TransactionPrice = Decimal.Parse(txtTransactionPrice.Text);
                        entity.SubtotalTransAmount = Decimal.Parse(txtSubtotalTransAmount.Text);
                        entity.Summary = txtSummary.Text ;
                       entity.CustomerPartNo = txtCustomerPartNo.Text ;
                       entity.CustomizedCost = Decimal.Parse(txtCustomizedCost.Text);
                        entity.Cost = Decimal.Parse(txtCost.Text);
                        entity.SubtotalCostAmount = Decimal.Parse(txtSubtotalCostAmount.Text);
                        entity.TaxRate = Decimal.Parse(txtTaxRate.Text);
                        entity.TotalReturnedQty = Int32.Parse(txtTotalReturnedQty.Text);
                        entity.Gift = Boolean.Parse(txtGift.Text);
                        entity.IncludingTax = Boolean.Parse(txtIncludingTax.Text);
                        entity.SubtotalUntaxedAmount = Decimal.Parse(txtSubtotalUntaxedAmount.Text);
                        entity.UnitCommissionAmount = Decimal.Parse(txtUnitCommissionAmount.Text);
                        entity.CommissionAmount = Decimal.Parse(txtCommissionAmount.Text);
                        entity.SubtotalTaxAmount = Decimal.Parse(txtSubtotalTaxAmount.Text);
                        entity.SaleFlagCode = txtSaleFlagCode.Text ;
                       entity.SaleOrderDetail_ID = Int64.Parse(txtSaleOrderDetail_ID.Text);
                        entity.AllocatedFreightIncome = Decimal.Parse(txtAllocatedFreightIncome.Text);
                        entity.AllocatedFreightCost = Decimal.Parse(txtAllocatedFreightCost.Text);
                        entity.FreightAllocationRules = Int32.Parse(txtFreightAllocationRules.Text);
                                return entity;
}
        */

        
        private tb_SaleOutDetail _EditEntity;
        public tb_SaleOutDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_SaleOutDetail entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
           DataBindingHelper.BindData4TextBox<tb_SaleOutDetail>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
          // DataBindingHelper.BindData4Cmb<tb_StorageRack>(entity, k => k.Rack_ID, v=>v.XXNAME, cmbRack_ID);
          // DataBindingHelper.BindData4Cmb<tb_SaleOut>(entity, k => k.SaleOut_MainID, v=>v.XXNAME, cmbSaleOut_MainID);
           DataBindingHelper.BindData4TextBox<tb_SaleOutDetail>(entity, t => t.Quantity, txtQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutDetail>(entity, t => t.UnitPrice.ToString(), txtUnitPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutDetail>(entity, t => t.Discount.ToString(), txtDiscount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutDetail>(entity, t => t.TransactionPrice.ToString(), txtTransactionPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutDetail>(entity, t => t.SubtotalTransAmount.ToString(), txtSubtotalTransAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutDetail>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutDetail>(entity, t => t.CustomerPartNo, txtCustomerPartNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutDetail>(entity, t => t.CustomizedCost.ToString(), txtCustomizedCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutDetail>(entity, t => t.Cost.ToString(), txtCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutDetail>(entity, t => t.SubtotalCostAmount.ToString(), txtSubtotalCostAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutDetail>(entity, t => t.TaxRate.ToString(), txtTaxRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutDetail>(entity, t => t.TotalReturnedQty, txtTotalReturnedQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_SaleOutDetail>(entity, t => t.Gift, chkGift, false);
           DataBindingHelper.BindData4CheckBox<tb_SaleOutDetail>(entity, t => t.IncludingTax, chkIncludingTax, false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutDetail>(entity, t => t.SubtotalUntaxedAmount.ToString(), txtSubtotalUntaxedAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutDetail>(entity, t => t.UnitCommissionAmount.ToString(), txtUnitCommissionAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutDetail>(entity, t => t.CommissionAmount.ToString(), txtCommissionAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutDetail>(entity, t => t.SubtotalTaxAmount.ToString(), txtSubtotalTaxAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutDetail>(entity, t => t.SaleFlagCode, txtSaleFlagCode, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutDetail>(entity, t => t.SaleOrderDetail_ID, txtSaleOrderDetail_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutDetail>(entity, t => t.AllocatedFreightIncome.ToString(), txtAllocatedFreightIncome, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutDetail>(entity, t => t.AllocatedFreightCost.ToString(), txtAllocatedFreightCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_SaleOutDetail>(entity, t => t.FreightAllocationRules, txtFreightAllocationRules, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



