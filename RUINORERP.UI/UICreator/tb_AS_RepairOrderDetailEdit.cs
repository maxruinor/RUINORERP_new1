
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/08/2025 19:05:31
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
    /// 维修工单明细数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_AS_RepairOrderDetailEdit:UserControl
    {
     public tb_AS_RepairOrderDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_AS_RepairOrderDetail UIToEntity()
        {
        tb_AS_RepairOrderDetail entity = new tb_AS_RepairOrderDetail();
                     entity.RepairOrderID = Int64.Parse(txtRepairOrderID.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.property = txtproperty.Text ;
                       entity.Quantity = Int32.Parse(txtQuantity.Text);
                        entity.Summary = txtSummary.Text ;
                       entity.RepairContent = txtRepairContent.Text ;
                       entity.MaterialCost = Decimal.Parse(txtMaterialCost.Text);
                        entity.LaborCost = Decimal.Parse(txtLaborCost.Text);
                        entity.SubtotalCost = Decimal.Parse(txtSubtotalCost.Text);
                        entity.TaxRate = Decimal.Parse(txtTaxRate.Text);
                        entity.SubtotalTaxAmount = Decimal.Parse(txtSubtotalTaxAmount.Text);
                        entity.SubtotalCostAmount = Decimal.Parse(txtSubtotalCostAmount.Text);
                        entity.TotalReturnedQty = Int32.Parse(txtTotalReturnedQty.Text);
                                return entity;
}
        */

        
        private tb_AS_RepairOrderDetail _EditEntity;
        public tb_AS_RepairOrderDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_AS_RepairOrderDetail entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_AS_RepairOrder>(entity, k => k.RepairOrderID, v=>v.XXNAME, cmbRepairOrderID);
          // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
          // DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrderDetail>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrderDetail>(entity, t => t.Quantity, txtQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrderDetail>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrderDetail>(entity, t => t.RepairContent, txtRepairContent, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrderDetail>(entity, t => t.MaterialCost.ToString(), txtMaterialCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrderDetail>(entity, t => t.LaborCost.ToString(), txtLaborCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrderDetail>(entity, t => t.SubtotalCost.ToString(), txtSubtotalCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrderDetail>(entity, t => t.TaxRate.ToString(), txtTaxRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrderDetail>(entity, t => t.SubtotalTaxAmount.ToString(), txtSubtotalTaxAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrderDetail>(entity, t => t.SubtotalCostAmount.ToString(), txtSubtotalCostAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrderDetail>(entity, t => t.TotalReturnedQty, txtTotalReturnedQty, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



