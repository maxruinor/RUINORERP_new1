
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:24
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
    /// 成品入库单明细数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_FinishedGoodsInvDetailEdit:UserControl
    {
     public tb_FinishedGoodsInvDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_FinishedGoodsInvDetail UIToEntity()
        {
        tb_FinishedGoodsInvDetail entity = new tb_FinishedGoodsInvDetail();
                     entity.FG_ID = Int64.Parse(txtFG_ID.Text);
                        entity.Unit_ID = Int64.Parse(txtUnit_ID.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.Rack_ID = Int64.Parse(txtRack_ID.Text);
                        entity.PayableQty = Int32.Parse(txtPayableQty.Text);
                        entity.Qty = Int32.Parse(txtQty.Text);
                        entity.UnitCost = Decimal.Parse(txtUnitCost.Text);
                        entity.UnpaidQty = Int32.Parse(txtUnpaidQty.Text);
                        entity.NetMachineHours = Decimal.Parse(txtNetMachineHours.Text);
                        entity.NetWorkingHours = Decimal.Parse(txtNetWorkingHours.Text);
                        entity.ApportionedCost = Decimal.Parse(txtApportionedCost.Text);
                        entity.ManuFee = Decimal.Parse(txtManuFee.Text);
                        entity.MaterialCost = Decimal.Parse(txtMaterialCost.Text);
                        entity.ProductionAllCost = Decimal.Parse(txtProductionAllCost.Text);
                        entity.Summary = txtSummary.Text ;
                       entity.property = txtproperty.Text ;
                               return entity;
}
        */

        
        private tb_FinishedGoodsInvDetail _EditEntity;
        public tb_FinishedGoodsInvDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_FinishedGoodsInvDetail entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_FinishedGoodsInv>(entity, k => k.FG_ID, v=>v.XXNAME, cmbFG_ID);
          // DataBindingHelper.BindData4Cmb<tb_Unit>(entity, k => k.Unit_ID, v=>v.XXNAME, cmbUnit_ID);
          // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
          // DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
          // DataBindingHelper.BindData4Cmb<tb_StorageRack>(entity, k => k.Rack_ID, v=>v.XXNAME, cmbRack_ID);
           DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInvDetail>(entity, t => t.PayableQty, txtPayableQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInvDetail>(entity, t => t.Qty, txtQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInvDetail>(entity, t => t.UnitCost.ToString(), txtUnitCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInvDetail>(entity, t => t.UnpaidQty, txtUnpaidQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInvDetail>(entity, t => t.NetMachineHours.ToString(), txtNetMachineHours, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInvDetail>(entity, t => t.NetWorkingHours.ToString(), txtNetWorkingHours, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInvDetail>(entity, t => t.ApportionedCost.ToString(), txtApportionedCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInvDetail>(entity, t => t.ManuFee.ToString(), txtManuFee, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInvDetail>(entity, t => t.MaterialCost.ToString(), txtMaterialCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInvDetail>(entity, t => t.ProductionAllCost.ToString(), txtProductionAllCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInvDetail>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInvDetail>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



