
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:19
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
    /// 出库单明细数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_StockOutDetailEdit:UserControl
    {
     public tb_StockOutDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_StockOutDetail UIToEntity()
        {
        tb_StockOutDetail entity = new tb_StockOutDetail();
                     entity.MainID = Int64.Parse(txtMainID.Text);
                        entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.Rack_ID = Int64.Parse(txtRack_ID.Text);
                        entity.Qty = Int32.Parse(txtQty.Text);
                        entity.Price = Decimal.Parse(txtPrice.Text);
                        entity.Cost = Decimal.Parse(txtCost.Text);
                        entity.Summary = txtSummary.Text ;
                       entity.SubtotalCostAmount = Decimal.Parse(txtSubtotalCostAmount.Text);
                        entity.SubtotalPirceAmount = Decimal.Parse(txtSubtotalPirceAmount.Text);
                        entity.property = txtproperty.Text ;
                               return entity;
}
        */

        
        private tb_StockOutDetail _EditEntity;
        public tb_StockOutDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_StockOutDetail entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_StockOut>(entity, k => k.MainID, v=>v.XXNAME, cmbMainID);
          // DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
          // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
          // DataBindingHelper.BindData4Cmb<tb_StorageRack>(entity, k => k.Rack_ID, v=>v.XXNAME, cmbRack_ID);
           DataBindingHelper.BindData4TextBox<tb_StockOutDetail>(entity, t => t.Qty, txtQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_StockOutDetail>(entity, t => t.Price.ToString(), txtPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_StockOutDetail>(entity, t => t.Cost.ToString(), txtCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_StockOutDetail>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_StockOutDetail>(entity, t => t.SubtotalCostAmount.ToString(), txtSubtotalCostAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_StockOutDetail>(entity, t => t.SubtotalPirceAmount.ToString(), txtSubtotalPirceAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_StockOutDetail>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



