
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:09
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
    /// 采购退回单数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_PurOrderReDetailEdit:UserControl
    {
     public tb_PurOrderReDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_PurOrderReDetail UIToEntity()
        {
        tb_PurOrderReDetail entity = new tb_PurOrderReDetail();
                     entity.PurRetrunID = Int64.Parse(txtPurRetrunID.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.property = txtproperty.Text ;
                       entity.Quantity = Int32.Parse(txtQuantity.Text);
                        entity.Discount = Decimal.Parse(txtDiscount.Text);
                        entity.TransactionPrice = Decimal.Parse(txtTransactionPrice.Text);
                        entity.TotalAmount = Decimal.Parse(txtTotalAmount.Text);
                        entity.Summary = txtSummary.Text ;
                       entity.CustomerType = txtCustomerType.Text ;
                       entity.commission = Decimal.Parse(txtcommission.Text);
                        entity.Notes = txtNotes.Text ;
                               return entity;
}
        */

        
        private tb_PurOrderReDetail _EditEntity;
        public tb_PurOrderReDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_PurOrderReDetail entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_PurOrderRe>(entity, k => k.PurRetrunID, v=>v.XXNAME, cmbPurRetrunID);
          // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
          // DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
           DataBindingHelper.BindData4TextBox<tb_PurOrderReDetail>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderReDetail>(entity, t => t.Quantity, txtQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderReDetail>(entity, t => t.Discount.ToString(), txtDiscount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderReDetail>(entity, t => t.TransactionPrice.ToString(), txtTransactionPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderReDetail>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderReDetail>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderReDetail>(entity, t => t.CustomerType, txtCustomerType, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderReDetail>(entity, t => t.commission.ToString(), txtcommission, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderReDetail>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



