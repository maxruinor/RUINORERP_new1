
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/15/2024 18:45:37
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
    /// 调拨单明细数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_StockTransferDetailEdit:UserControl
    {
     public tb_StockTransferDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_StockTransferDetail UIToEntity()
        {
        tb_StockTransferDetail entity = new tb_StockTransferDetail();
                     entity.StockTransferID = Int64.Parse(txtStockTransferID.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.property = txtproperty.Text ;
                       entity.Qty = Int32.Parse(txtQty.Text);
                        entity.TransPrice = Decimal.Parse(txtTransPrice.Text);
                        entity.Cost = Decimal.Parse(txtCost.Text);
                        entity.Summary = txtSummary.Text ;
                       entity.SubtotalCostAmount = Decimal.Parse(txtSubtotalCostAmount.Text);
                        entity.SubtotalTransferPirceAmount = Decimal.Parse(txtSubtotalTransferPirceAmount.Text);
                                return entity;
}
        */

        
        private tb_StockTransferDetail _EditEntity;
        public tb_StockTransferDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_StockTransferDetail entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_StockTransfer>(entity, k => k.StockTransferID, v=>v.XXNAME, cmbStockTransferID);
          // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
           DataBindingHelper.BindData4TextBox<tb_StockTransferDetail>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_StockTransferDetail>(entity, t => t.Qty, txtQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_StockTransferDetail>(entity, t => t.TransPrice.ToString(), txtTransPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_StockTransferDetail>(entity, t => t.Cost.ToString(), txtCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_StockTransferDetail>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_StockTransferDetail>(entity, t => t.SubtotalCostAmount.ToString(), txtSubtotalCostAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_StockTransferDetail>(entity, t => t.SubtotalTransferPirceAmount.ToString(), txtSubtotalTransferPirceAmount, BindDataType4TextBox.Money,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



