
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/07/2025 14:22:22
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
    /// 发票明细数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_FM_InvoiceDetailEdit:UserControl
    {
     public tb_FM_InvoiceDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_FM_InvoiceDetail UIToEntity()
        {
        tb_FM_InvoiceDetail entity = new tb_FM_InvoiceDetail();
                     entity.InvoiceId = Int64.Parse(txtInvoiceId.Text);
                        entity.SourceBizType = Int32.Parse(txtSourceBizType.Text);
                        entity.SourceBillId = Int64.Parse(txtSourceBillId.Text);
                        entity.SourceBillNo = txtSourceBillNo.Text ;
                       entity.ItemType = Int32.Parse(txtItemType.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.property = txtproperty.Text ;
                       entity.Specifications = txtSpecifications.Text ;
                       entity.Unit_ID = Int64.Parse(txtUnit_ID.Text);
                        entity.IsIncludeTax = Boolean.Parse(txtIsIncludeTax.Text);
                        entity.UnitPrice = Decimal.Parse(txtUnitPrice.Text);
                        entity.Quantity = Decimal.Parse(txtQuantity.Text);
                        entity.TaxRate = Decimal.Parse(txtTaxRate.Text);
                        entity.TaxSubtotalAmount = Decimal.Parse(txtTaxSubtotalAmount.Text);
                        entity.SubtotalAmount = Decimal.Parse(txtSubtotalAmount.Text);
                        entity.Summary = txtSummary.Text ;
                               return entity;
}
        */

        
        private tb_FM_InvoiceDetail _EditEntity;
        public tb_FM_InvoiceDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_FM_InvoiceDetail entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_FM_Invoice>(entity, k => k.InvoiceId, v=>v.XXNAME, cmbInvoiceId);
           DataBindingHelper.BindData4TextBox<tb_FM_InvoiceDetail>(entity, t => t.SourceBizType, txtSourceBizType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_InvoiceDetail>(entity, t => t.SourceBillId, txtSourceBillId, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_InvoiceDetail>(entity, t => t.SourceBillNo, txtSourceBillNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_InvoiceDetail>(entity, t => t.ItemType, txtItemType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_InvoiceDetail>(entity, t => t.ProdDetailID, txtProdDetailID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_InvoiceDetail>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_InvoiceDetail>(entity, t => t.Specifications, txtSpecifications, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_InvoiceDetail>(entity, t => t.Unit_ID, txtUnit_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_InvoiceDetail>(entity, t => t.IsIncludeTax, chkIsIncludeTax, false);
           DataBindingHelper.BindData4TextBox<tb_FM_InvoiceDetail>(entity, t => t.UnitPrice.ToString(), txtUnitPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_InvoiceDetail>(entity, t => t.Quantity.ToString(), txtQuantity, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_InvoiceDetail>(entity, t => t.TaxRate.ToString(), txtTaxRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_InvoiceDetail>(entity, t => t.TaxSubtotalAmount.ToString(), txtTaxSubtotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_InvoiceDetail>(entity, t => t.SubtotalAmount.ToString(), txtSubtotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_InvoiceDetail>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



