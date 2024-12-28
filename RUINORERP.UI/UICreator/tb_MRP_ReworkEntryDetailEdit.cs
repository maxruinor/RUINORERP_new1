
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/27/2024 18:30:48
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
    /// 返工入库单明细数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_MRP_ReworkEntryDetailEdit:UserControl
    {
     public tb_MRP_ReworkEntryDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_MRP_ReworkEntryDetail UIToEntity()
        {
        tb_MRP_ReworkEntryDetail entity = new tb_MRP_ReworkEntryDetail();
                     entity.ReworkEntryID = Int64.Parse(txtReworkEntryID.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.CustomertModel = txtCustomertModel.Text ;
                       entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.Rack_ID = Int64.Parse(txtRack_ID.Text);
                        entity.property = txtproperty.Text ;
                       entity.Quantity = Int32.Parse(txtQuantity.Text);
                        entity.ReworkFee = Decimal.Parse(txtReworkFee.Text);
                        entity.SubtotalReworkFee = Decimal.Parse(txtSubtotalReworkFee.Text);
                        entity.UnitCost = Decimal.Parse(txtUnitCost.Text);
                        entity.SubtotalCostAmount = Decimal.Parse(txtSubtotalCostAmount.Text);
                        entity.Summary = txtSummary.Text ;
                               return entity;
}
        */

        
        private tb_MRP_ReworkEntryDetail _EditEntity;
        public tb_MRP_ReworkEntryDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_MRP_ReworkEntryDetail entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_MRP_ReworkEntry>(entity, k => k.ReworkEntryID, v=>v.XXNAME, cmbReworkEntryID);
          // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
           DataBindingHelper.BindData4TextBox<tb_MRP_ReworkEntryDetail>(entity, t => t.CustomertModel, txtCustomertModel, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
          // DataBindingHelper.BindData4Cmb<tb_StorageRack>(entity, k => k.Rack_ID, v=>v.XXNAME, cmbRack_ID);
           DataBindingHelper.BindData4TextBox<tb_MRP_ReworkEntryDetail>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_MRP_ReworkEntryDetail>(entity, t => t.Quantity, txtQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_MRP_ReworkEntryDetail>(entity, t => t.ReworkFee.ToString(), txtReworkFee, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_MRP_ReworkEntryDetail>(entity, t => t.SubtotalReworkFee.ToString(), txtSubtotalReworkFee, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_MRP_ReworkEntryDetail>(entity, t => t.UnitCost.ToString(), txtUnitCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_MRP_ReworkEntryDetail>(entity, t => t.SubtotalCostAmount.ToString(), txtSubtotalCostAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_MRP_ReworkEntryDetail>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



