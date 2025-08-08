
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:37
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
    /// 库存表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_InventoryEdit:UserControl
    {
     public tb_InventoryEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_Inventory UIToEntity()
        {
        tb_Inventory entity = new tb_Inventory();
                     entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.Rack_ID = Int64.Parse(txtRack_ID.Text);
                        entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.Quantity = Int32.Parse(txtQuantity.Text);
                        entity.InitInventory = Int32.Parse(txtInitInventory.Text);
                        entity.Alert_Use = Int32.Parse(txtAlert_Use.Text);
                        entity.On_the_way_Qty = Int32.Parse(txtOn_the_way_Qty.Text);
                        entity.Sale_Qty = Int32.Parse(txtSale_Qty.Text);
                        entity.MakingQty = Int32.Parse(txtMakingQty.Text);
                        entity.NotOutQty = Int32.Parse(txtNotOutQty.Text);
                        entity.BatchNumber = Int32.Parse(txtBatchNumber.Text);
                        entity.Alert_Quantity = Int32.Parse(txtAlert_Quantity.Text);
                        entity.CostFIFO = Decimal.Parse(txtCostFIFO.Text);
                        entity.CostMonthlyWA = Decimal.Parse(txtCostMonthlyWA.Text);
                        entity.CostMovingWA = Decimal.Parse(txtCostMovingWA.Text);
                        entity.Inv_AdvCost = Decimal.Parse(txtInv_AdvCost.Text);
                        entity.Inv_Cost = Decimal.Parse(txtInv_Cost.Text);
                        entity.Inv_SubtotalCostMoney = Decimal.Parse(txtInv_SubtotalCostMoney.Text);
                        entity.LatestOutboundTime = DateTime.Parse(txtLatestOutboundTime.Text);
                        entity.LatestStorageTime = DateTime.Parse(txtLatestStorageTime.Text);
                        entity.LastInventoryDate = DateTime.Parse(txtLastInventoryDate.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                                return entity;
}
        */

        
        private tb_Inventory _EditEntity;
        public tb_Inventory EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_Inventory entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
          // DataBindingHelper.BindData4Cmb<tb_StorageRack>(entity, k => k.Rack_ID, v=>v.XXNAME, cmbRack_ID);
          // DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
           DataBindingHelper.BindData4TextBox<tb_Inventory>(entity, t => t.Quantity, txtQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_Inventory>(entity, t => t.InitInventory, txtInitInventory, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_Inventory>(entity, t => t.Alert_Use, txtAlert_Use, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_Inventory>(entity, t => t.On_the_way_Qty, txtOn_the_way_Qty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_Inventory>(entity, t => t.Sale_Qty, txtSale_Qty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_Inventory>(entity, t => t.MakingQty, txtMakingQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_Inventory>(entity, t => t.NotOutQty, txtNotOutQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_Inventory>(entity, t => t.BatchNumber, txtBatchNumber, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_Inventory>(entity, t => t.Alert_Quantity, txtAlert_Quantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_Inventory>(entity, t => t.CostFIFO.ToString(), txtCostFIFO, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_Inventory>(entity, t => t.CostMonthlyWA.ToString(), txtCostMonthlyWA, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_Inventory>(entity, t => t.CostMovingWA.ToString(), txtCostMovingWA, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_Inventory>(entity, t => t.Inv_AdvCost.ToString(), txtInv_AdvCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_Inventory>(entity, t => t.Inv_Cost.ToString(), txtInv_Cost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_Inventory>(entity, t => t.Inv_SubtotalCostMoney.ToString(), txtInv_SubtotalCostMoney, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4DataTime<tb_Inventory>(entity, t => t.LatestOutboundTime, dtpLatestOutboundTime,false);
           DataBindingHelper.BindData4DataTime<tb_Inventory>(entity, t => t.LatestStorageTime, dtpLatestStorageTime,false);
           DataBindingHelper.BindData4DataTime<tb_Inventory>(entity, t => t.LastInventoryDate, dtpLastInventoryDate,false);
           DataBindingHelper.BindData4TextBox<tb_Inventory>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_Inventory>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_Inventory>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_Inventory>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_Inventory>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



