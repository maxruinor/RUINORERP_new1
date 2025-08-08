
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:08
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
    /// 采购订单明细表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_PurOrderDetailEdit:UserControl
    {
     public tb_PurOrderDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_PurOrderDetail UIToEntity()
        {
        tb_PurOrderDetail entity = new tb_PurOrderDetail();
                     entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.PurOrder_ID = Int64.Parse(txtPurOrder_ID.Text);
                        entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.property = txtproperty.Text ;
                       entity.Quantity = Int32.Parse(txtQuantity.Text);
                        entity.UnitPrice = Decimal.Parse(txtUnitPrice.Text);
                        entity.Discount = Decimal.Parse(txtDiscount.Text);
                        entity.TransactionPrice = Decimal.Parse(txtTransactionPrice.Text);
                        entity.TaxRate = Decimal.Parse(txtTaxRate.Text);
                        entity.TaxAmount = Decimal.Parse(txtTaxAmount.Text);
                        entity.CustomizedCost = Decimal.Parse(txtCustomizedCost.Text);
                        entity.UntaxedCustomizedCost = Decimal.Parse(txtUntaxedCustomizedCost.Text);
                        entity.UntaxedUnitPrice = Decimal.Parse(txtUntaxedUnitPrice.Text);
                        entity.SubtotalAmount = Decimal.Parse(txtSubtotalAmount.Text);
                        entity.SubtotalUntaxedAmount = Decimal.Parse(txtSubtotalUntaxedAmount.Text);
                        entity.IsGift = Boolean.Parse(txtIsGift.Text);
                        entity.PreDeliveryDate = DateTime.Parse(txtPreDeliveryDate.Text);
                        entity.VendorModelCode = txtVendorModelCode.Text ;
                       entity.CustomertModel = txtCustomertModel.Text ;
                       entity.DeliveredQuantity = Int32.Parse(txtDeliveredQuantity.Text);
                        entity.IncludingTax = Boolean.Parse(txtIncludingTax.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.TotalReturnedQty = Int32.Parse(txtTotalReturnedQty.Text);
                        entity.UndeliveredQty = Int32.Parse(txtUndeliveredQty.Text);
                                return entity;
}
        */

        
        private tb_PurOrderDetail _EditEntity;
        public tb_PurOrderDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_PurOrderDetail entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
          // DataBindingHelper.BindData4Cmb<tb_PurOrder>(entity, k => k.PurOrder_ID, v=>v.XXNAME, cmbPurOrder_ID);
          // DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
           DataBindingHelper.BindData4TextBox<tb_PurOrderDetail>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderDetail>(entity, t => t.Quantity, txtQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderDetail>(entity, t => t.UnitPrice.ToString(), txtUnitPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderDetail>(entity, t => t.Discount.ToString(), txtDiscount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderDetail>(entity, t => t.TransactionPrice.ToString(), txtTransactionPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderDetail>(entity, t => t.TaxRate.ToString(), txtTaxRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderDetail>(entity, t => t.TaxAmount.ToString(), txtTaxAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderDetail>(entity, t => t.CustomizedCost.ToString(), txtCustomizedCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderDetail>(entity, t => t.UntaxedCustomizedCost.ToString(), txtUntaxedCustomizedCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderDetail>(entity, t => t.UntaxedUnitPrice.ToString(), txtUntaxedUnitPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderDetail>(entity, t => t.SubtotalAmount.ToString(), txtSubtotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderDetail>(entity, t => t.SubtotalUntaxedAmount.ToString(), txtSubtotalUntaxedAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_PurOrderDetail>(entity, t => t.IsGift, chkIsGift, false);
           DataBindingHelper.BindData4DataTime<tb_PurOrderDetail>(entity, t => t.PreDeliveryDate, dtpPreDeliveryDate,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderDetail>(entity, t => t.VendorModelCode, txtVendorModelCode, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderDetail>(entity, t => t.CustomertModel, txtCustomertModel, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderDetail>(entity, t => t.DeliveredQuantity, txtDeliveredQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_PurOrderDetail>(entity, t => t.IncludingTax, chkIncludingTax, false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderDetail>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderDetail>(entity, t => t.TotalReturnedQty, txtTotalReturnedQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderDetail>(entity, t => t.UndeliveredQty, txtUndeliveredQty, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



