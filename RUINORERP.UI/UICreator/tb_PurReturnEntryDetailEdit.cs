
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/16/2024 20:05:38
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
    /// 采购退货入库单明细数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_PurReturnEntryDetailEdit:UserControl
    {
     public tb_PurReturnEntryDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_PurReturnEntryDetail UIToEntity()
        {
        tb_PurReturnEntryDetail entity = new tb_PurReturnEntryDetail();
                     entity.PurReEntry_ID = Int64.Parse(txtPurReEntry_ID.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.CustomertModel = txtCustomertModel.Text ;
                       entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.Rack_ID = Int64.Parse(txtRack_ID.Text);
                        entity.property = txtproperty.Text ;
                       entity.Quantity = Int32.Parse(txtQuantity.Text);
                        entity.UnitPrice = Decimal.Parse(txtUnitPrice.Text);
                        entity.Discount = Decimal.Parse(txtDiscount.Text);
                        entity.TransactionPrice = Decimal.Parse(txtTransactionPrice.Text);
                        entity.IsGift = Boolean.Parse(txtIsGift.Text);
                        entity.TaxRate = Decimal.Parse(txtTaxRate.Text);
                        entity.TaxAmount = Decimal.Parse(txtTaxAmount.Text);
                        entity.SubtotalTrPriceAmount = Decimal.Parse(txtSubtotalTrPriceAmount.Text);
                        entity.Summary = txtSummary.Text ;
                       entity.DiscountAmount = Decimal.Parse(txtDiscountAmount.Text);
                        entity.IsIncludeTax = Boolean.Parse(txtIsIncludeTax.Text);
                                return entity;
}
        */

        
        private tb_PurReturnEntryDetail _EditEntity;
        public tb_PurReturnEntryDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_PurReturnEntryDetail entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_PurReturnEntry>(entity, k => k.PurReEntry_ID, v=>v.XXNAME, cmbPurReEntry_ID);
          // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
           DataBindingHelper.BindData4TextBox<tb_PurReturnEntryDetail>(entity, t => t.CustomertModel, txtCustomertModel, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
          // DataBindingHelper.BindData4Cmb<tb_StorageRack>(entity, k => k.Rack_ID, v=>v.XXNAME, cmbRack_ID);
           DataBindingHelper.BindData4TextBox<tb_PurReturnEntryDetail>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PurReturnEntryDetail>(entity, t => t.Quantity, txtQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_PurReturnEntryDetail>(entity, t => t.UnitPrice.ToString(), txtUnitPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurReturnEntryDetail>(entity, t => t.Discount.ToString(), txtDiscount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurReturnEntryDetail>(entity, t => t.TransactionPrice.ToString(), txtTransactionPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CehckBox<tb_PurReturnEntryDetail>(entity, t => t.IsGift, chkIsGift, false);
           DataBindingHelper.BindData4TextBox<tb_PurReturnEntryDetail>(entity, t => t.TaxRate.ToString(), txtTaxRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurReturnEntryDetail>(entity, t => t.TaxAmount.ToString(), txtTaxAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurReturnEntryDetail>(entity, t => t.SubtotalTrPriceAmount.ToString(), txtSubtotalTrPriceAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurReturnEntryDetail>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PurReturnEntryDetail>(entity, t => t.DiscountAmount.ToString(), txtDiscountAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CehckBox<tb_PurReturnEntryDetail>(entity, t => t.IsIncludeTax, chkIsIncludeTax, false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



