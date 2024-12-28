
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/27/2024 18:30:50
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
    /// 采购入库退回单明细数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_MRP_ReworkReturnDetailEdit:UserControl
    {
     public tb_MRP_ReworkReturnDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_MRP_ReworkReturnDetail UIToEntity()
        {
        tb_MRP_ReworkReturnDetail entity = new tb_MRP_ReworkReturnDetail();
                     entity.ReworkReturnID = Int64.Parse(txtReworkReturnID.Text);
                        entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.property = txtproperty.Text ;
                       entity.Quantity = Int32.Parse(txtQuantity.Text);
                        entity.DeliveredQuantity = Int32.Parse(txtDeliveredQuantity.Text);
                        entity.ReworkFee = Decimal.Parse(txtReworkFee.Text);
                        entity.SubtotalReworkFee = Decimal.Parse(txtSubtotalReworkFee.Text);
                        entity.UnitCost = Decimal.Parse(txtUnitCost.Text);
                        entity.SubtotalCostAmount = Decimal.Parse(txtSubtotalCostAmount.Text);
                        entity.CustomertModel = txtCustomertModel.Text ;
                       entity.Summary = txtSummary.Text ;
                               return entity;
}
        */

        
        private tb_MRP_ReworkReturnDetail _EditEntity;
        public tb_MRP_ReworkReturnDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_MRP_ReworkReturnDetail entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_MRP_ReworkReturn>(entity, k => k.ReworkReturnID, v=>v.XXNAME, cmbReworkReturnID);
          // DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
          // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
           DataBindingHelper.BindData4TextBox<tb_MRP_ReworkReturnDetail>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_MRP_ReworkReturnDetail>(entity, t => t.Quantity, txtQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_MRP_ReworkReturnDetail>(entity, t => t.DeliveredQuantity, txtDeliveredQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_MRP_ReworkReturnDetail>(entity, t => t.ReworkFee.ToString(), txtReworkFee, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_MRP_ReworkReturnDetail>(entity, t => t.SubtotalReworkFee.ToString(), txtSubtotalReworkFee, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_MRP_ReworkReturnDetail>(entity, t => t.UnitCost.ToString(), txtUnitCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_MRP_ReworkReturnDetail>(entity, t => t.SubtotalCostAmount.ToString(), txtSubtotalCostAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_MRP_ReworkReturnDetail>(entity, t => t.CustomertModel, txtCustomertModel, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_MRP_ReworkReturnDetail>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



