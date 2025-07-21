
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/19/2025 17:12:45
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
    /// 维修物料明细表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_AS_RepairOrderMaterialDetailEdit:UserControl
    {
     public tb_AS_RepairOrderMaterialDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_AS_RepairOrderMaterialDetail UIToEntity()
        {
        tb_AS_RepairOrderMaterialDetail entity = new tb_AS_RepairOrderMaterialDetail();
                     entity.RepairOrderID = Int64.Parse(txtRepairOrderID.Text);
                        entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.property = txtproperty.Text ;
                       entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.ShouldSendQty = Decimal.Parse(txtShouldSendQty.Text);
                        entity.ActualSentQty = Decimal.Parse(txtActualSentQty.Text);
                        entity.UnitPrice = Decimal.Parse(txtUnitPrice.Text);
                        entity.SubtotalTransAmount = Decimal.Parse(txtSubtotalTransAmount.Text);
                        entity.TaxRate = Decimal.Parse(txtTaxRate.Text);
                        entity.SubtotalTaxAmount = Decimal.Parse(txtSubtotalTaxAmount.Text);
                        entity.SubtotalUntaxedAmount = Decimal.Parse(txtSubtotalUntaxedAmount.Text);
                        entity.Summary = txtSummary.Text ;
                       entity.Cost = Decimal.Parse(txtCost.Text);
                        entity.SubtotalCost = Decimal.Parse(txtSubtotalCost.Text);
                        entity.Gift = Boolean.Parse(txtGift.Text);
                        entity.IsCritical = Boolean.Parse(txtIsCritical.Text);
                                return entity;
}
        */

        
        private tb_AS_RepairOrderMaterialDetail _EditEntity;
        public tb_AS_RepairOrderMaterialDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_AS_RepairOrderMaterialDetail entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_AS_RepairOrder>(entity, k => k.RepairOrderID, v=>v.XXNAME, cmbRepairOrderID);
          // DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrderMaterialDetail>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrderMaterialDetail>(entity, t => t.ShouldSendQty.ToString(), txtShouldSendQty, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrderMaterialDetail>(entity, t => t.ActualSentQty.ToString(), txtActualSentQty, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrderMaterialDetail>(entity, t => t.UnitPrice.ToString(), txtUnitPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrderMaterialDetail>(entity, t => t.SubtotalTransAmount.ToString(), txtSubtotalTransAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrderMaterialDetail>(entity, t => t.TaxRate.ToString(), txtTaxRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrderMaterialDetail>(entity, t => t.SubtotalTaxAmount.ToString(), txtSubtotalTaxAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrderMaterialDetail>(entity, t => t.SubtotalUntaxedAmount.ToString(), txtSubtotalUntaxedAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrderMaterialDetail>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrderMaterialDetail>(entity, t => t.Cost.ToString(), txtCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairOrderMaterialDetail>(entity, t => t.SubtotalCost.ToString(), txtSubtotalCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_AS_RepairOrderMaterialDetail>(entity, t => t.Gift, chkGift, false);
           DataBindingHelper.BindData4CheckBox<tb_AS_RepairOrderMaterialDetail>(entity, t => t.IsCritical, chkIsCritical, false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



