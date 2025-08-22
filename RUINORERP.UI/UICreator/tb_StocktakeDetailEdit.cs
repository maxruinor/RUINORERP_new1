
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/22/2025 21:05:41
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
    /// 盘点明细表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_StocktakeDetailEdit:UserControl
    {
     public tb_StocktakeDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_StocktakeDetail UIToEntity()
        {
        tb_StocktakeDetail entity = new tb_StocktakeDetail();
                     entity.MainID = Int64.Parse(txtMainID.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.Rack_ID = Int64.Parse(txtRack_ID.Text);
                        entity.CarryinglQty = Int32.Parse(txtCarryinglQty.Text);
                        entity.DiffQty = Int32.Parse(txtDiffQty.Text);
                        entity.CheckQty = Int32.Parse(txtCheckQty.Text);
                        entity.Cost = Decimal.Parse(txtCost.Text);
                        entity.TaxRate = Decimal.Parse(txtTaxRate.Text);
                        entity.UntaxedCost = Decimal.Parse(txtUntaxedCost.Text);
                        entity.CarryingSubtotalAmount = Decimal.Parse(txtCarryingSubtotalAmount.Text);
                        entity.DiffSubtotalAmount = Decimal.Parse(txtDiffSubtotalAmount.Text);
                        entity.CheckSubtotalAmount = Decimal.Parse(txtCheckSubtotalAmount.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.property = txtproperty.Text ;
                               return entity;
}
        */

        
        private tb_StocktakeDetail _EditEntity;
        public tb_StocktakeDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_StocktakeDetail entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_Stocktake>(entity, k => k.MainID, v=>v.XXNAME, cmbMainID);
          // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
          // DataBindingHelper.BindData4Cmb<tb_StorageRack>(entity, k => k.Rack_ID, v=>v.XXNAME, cmbRack_ID);
           DataBindingHelper.BindData4TextBox<tb_StocktakeDetail>(entity, t => t.CarryinglQty, txtCarryinglQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_StocktakeDetail>(entity, t => t.DiffQty, txtDiffQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_StocktakeDetail>(entity, t => t.CheckQty, txtCheckQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_StocktakeDetail>(entity, t => t.Cost.ToString(), txtCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_StocktakeDetail>(entity, t => t.TaxRate.ToString(), txtTaxRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_StocktakeDetail>(entity, t => t.UntaxedCost.ToString(), txtUntaxedCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_StocktakeDetail>(entity, t => t.CarryingSubtotalAmount.ToString(), txtCarryingSubtotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_StocktakeDetail>(entity, t => t.DiffSubtotalAmount.ToString(), txtDiffSubtotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_StocktakeDetail>(entity, t => t.CheckSubtotalAmount.ToString(), txtCheckSubtotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_StocktakeDetail>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_StocktakeDetail>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



