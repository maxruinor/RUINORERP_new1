
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:04
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
    /// 采购入库单数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_PurEntryDetailEdit:UserControl
    {
     public tb_PurEntryDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_PurEntryDetail UIToEntity()
        {
        tb_PurEntryDetail entity = new tb_PurEntryDetail();
                     entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.PurEntryID = Int64.Parse(txtPurEntryID.Text);
                        entity.property = txtproperty.Text ;
                       entity.Quantity = Int32.Parse(txtQuantity.Text);
                        entity.UnitPrice = Decimal.Parse(txtUnitPrice.Text);
                        entity.CustomizedCost = Decimal.Parse(txtCustomizedCost.Text);
                        entity.UntaxedCustomizedCost = Decimal.Parse(txtUntaxedCustomizedCost.Text);
                        entity.Discount = Decimal.Parse(txtDiscount.Text);
                        entity.IsGift = Boolean.Parse(txtIsGift.Text);
                        entity.TaxRate = Decimal.Parse(txtTaxRate.Text);
                        entity.TaxAmount = Decimal.Parse(txtTaxAmount.Text);
                        entity.UntaxedUnitPrice = Decimal.Parse(txtUntaxedUnitPrice.Text);
                        entity.TransactionPrice = Decimal.Parse(txtTransactionPrice.Text);
                        entity.SubtotalAmount = Decimal.Parse(txtSubtotalAmount.Text);
                        entity.SubtotalUntaxedAmount = Decimal.Parse(txtSubtotalUntaxedAmount.Text);
                        entity.VendorModelCode = txtVendorModelCode.Text ;
                       entity.CustomertModel = txtCustomertModel.Text ;
                       entity.Summary = txtSummary.Text ;
                       entity.IsIncludeTax = Boolean.Parse(txtIsIncludeTax.Text);
                        entity.ReturnedQty = Int32.Parse(txtReturnedQty.Text);
                        entity.Rack_ID = Int64.Parse(txtRack_ID.Text);
                        entity.PurOrder_ChildID = Int64.Parse(txtPurOrder_ChildID.Text);
                        entity.AllocatedFreightCost = Decimal.Parse(txtAllocatedFreightCost.Text);
                        entity.FreightAllocationRules = Int32.Parse(txtFreightAllocationRules.Text);
                                return entity;
}
        */

        
        private tb_PurEntryDetail _EditEntity;
        public tb_PurEntryDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_PurEntryDetail entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
          // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
          // DataBindingHelper.BindData4Cmb<tb_PurEntry>(entity, k => k.PurEntryID, v=>v.XXNAME, cmbPurEntryID);
           DataBindingHelper.BindData4TextBox<tb_PurEntryDetail>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryDetail>(entity, t => t.Quantity, txtQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryDetail>(entity, t => t.UnitPrice.ToString(), txtUnitPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryDetail>(entity, t => t.CustomizedCost.ToString(), txtCustomizedCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryDetail>(entity, t => t.UntaxedCustomizedCost.ToString(), txtUntaxedCustomizedCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryDetail>(entity, t => t.Discount.ToString(), txtDiscount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_PurEntryDetail>(entity, t => t.IsGift, chkIsGift, false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryDetail>(entity, t => t.TaxRate.ToString(), txtTaxRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryDetail>(entity, t => t.TaxAmount.ToString(), txtTaxAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryDetail>(entity, t => t.UntaxedUnitPrice.ToString(), txtUntaxedUnitPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryDetail>(entity, t => t.TransactionPrice.ToString(), txtTransactionPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryDetail>(entity, t => t.SubtotalAmount.ToString(), txtSubtotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryDetail>(entity, t => t.SubtotalUntaxedAmount.ToString(), txtSubtotalUntaxedAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryDetail>(entity, t => t.VendorModelCode, txtVendorModelCode, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryDetail>(entity, t => t.CustomertModel, txtCustomertModel, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryDetail>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_PurEntryDetail>(entity, t => t.IsIncludeTax, chkIsIncludeTax, false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryDetail>(entity, t => t.ReturnedQty, txtReturnedQty, BindDataType4TextBox.Qty,false);
          // DataBindingHelper.BindData4Cmb<tb_StorageRack>(entity, k => k.Rack_ID, v=>v.XXNAME, cmbRack_ID);
           DataBindingHelper.BindData4TextBox<tb_PurEntryDetail>(entity, t => t.PurOrder_ChildID, txtPurOrder_ChildID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryDetail>(entity, t => t.AllocatedFreightCost.ToString(), txtAllocatedFreightCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurEntryDetail>(entity, t => t.FreightAllocationRules, txtFreightAllocationRules, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



