
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/13/2025 17:09:42
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
    /// 库存快照表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_InventorySnapshotEdit:UserControl
    {
     public tb_InventorySnapshotEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_InventorySnapshot UIToEntity()
        {
        tb_InventorySnapshot entity = new tb_InventorySnapshot();
                     entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.Quantity = Int32.Parse(txtQuantity.Text);
                        entity.InitInventory = Int32.Parse(txtInitInventory.Text);
                        entity.Rack_ID = Int64.Parse(txtRack_ID.Text);
                        entity.On_the_way_Qty = Int32.Parse(txtOn_the_way_Qty.Text);
                        entity.Sale_Qty = Int32.Parse(txtSale_Qty.Text);
                        entity.MakingQty = Int32.Parse(txtMakingQty.Text);
                        entity.NotOutQty = Int32.Parse(txtNotOutQty.Text);
                        entity.CostFIFO = Decimal.Parse(txtCostFIFO.Text);
                        entity.CostMonthlyWA = Decimal.Parse(txtCostMonthlyWA.Text);
                        entity.CostMovingWA = Decimal.Parse(txtCostMovingWA.Text);
                        entity.Inv_AdvCost = Decimal.Parse(txtInv_AdvCost.Text);
                        entity.Inv_Cost = Decimal.Parse(txtInv_Cost.Text);
                        entity.Inv_SubtotalCostMoney = Decimal.Parse(txtInv_SubtotalCostMoney.Text);
                        entity.LatestOutboundTime = DateTime.Parse(txtLatestOutboundTime.Text);
                        entity.LatestStorageTime = DateTime.Parse(txtLatestStorageTime.Text);
                        entity.LastInventoryDate = DateTime.Parse(txtLastInventoryDate.Text);
                        entity.SnapshotTime = DateTime.Parse(txtSnapshotTime.Text);
                        entity.Notes = txtNotes.Text ;
                               return entity;
}
        */

        
        private tb_InventorySnapshot _EditEntity;
        public tb_InventorySnapshot EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_InventorySnapshot entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_InventorySnapshot>(entity, t => t.ProdDetailID, txtProdDetailID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_InventorySnapshot>(entity, t => t.Location_ID, txtLocation_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_InventorySnapshot>(entity, t => t.Quantity, txtQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_InventorySnapshot>(entity, t => t.InitInventory, txtInitInventory, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_InventorySnapshot>(entity, t => t.Rack_ID, txtRack_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_InventorySnapshot>(entity, t => t.On_the_way_Qty, txtOn_the_way_Qty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_InventorySnapshot>(entity, t => t.Sale_Qty, txtSale_Qty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_InventorySnapshot>(entity, t => t.MakingQty, txtMakingQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_InventorySnapshot>(entity, t => t.NotOutQty, txtNotOutQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_InventorySnapshot>(entity, t => t.CostFIFO.ToString(), txtCostFIFO, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_InventorySnapshot>(entity, t => t.CostMonthlyWA.ToString(), txtCostMonthlyWA, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_InventorySnapshot>(entity, t => t.CostMovingWA.ToString(), txtCostMovingWA, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_InventorySnapshot>(entity, t => t.Inv_AdvCost.ToString(), txtInv_AdvCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_InventorySnapshot>(entity, t => t.Inv_Cost.ToString(), txtInv_Cost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_InventorySnapshot>(entity, t => t.Inv_SubtotalCostMoney.ToString(), txtInv_SubtotalCostMoney, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4DataTime<tb_InventorySnapshot>(entity, t => t.LatestOutboundTime, dtpLatestOutboundTime,false);
           DataBindingHelper.BindData4DataTime<tb_InventorySnapshot>(entity, t => t.LatestStorageTime, dtpLatestStorageTime,false);
           DataBindingHelper.BindData4DataTime<tb_InventorySnapshot>(entity, t => t.LastInventoryDate, dtpLastInventoryDate,false);
           DataBindingHelper.BindData4DataTime<tb_InventorySnapshot>(entity, t => t.SnapshotTime, dtpSnapshotTime,false);
           DataBindingHelper.BindData4TextBox<tb_InventorySnapshot>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



