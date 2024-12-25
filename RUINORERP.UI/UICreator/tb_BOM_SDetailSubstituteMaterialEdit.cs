
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/25/2024 20:07:13
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
    /// 标准物料表BOM明细的替代材料表-使用优化级按库存量-成本-保质时间在配置来确定数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_BOM_SDetailSubstituteMaterialEdit:UserControl
    {
     public tb_BOM_SDetailSubstituteMaterialEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_BOM_SDetailSubstituteMaterial UIToEntity()
        {
        tb_BOM_SDetailSubstituteMaterial entity = new tb_BOM_SDetailSubstituteMaterial();
                     entity.SubID = Int64.Parse(txtSubID.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.SKU = txtSKU.Text ;
                       entity.property = txtproperty.Text ;
                       entity.Unit_ID = Int64.Parse(txtUnit_ID.Text);
                        entity.UnitConversion_ID = Int64.Parse(txtUnitConversion_ID.Text);
                        entity.UsedQty = Decimal.Parse(txtUsedQty.Text);
                        entity.Radix = Int32.Parse(txtRadix.Text);
                        entity.LossRate = Decimal.Parse(txtLossRate.Text);
                        entity.InstallPosition = txtInstallPosition.Text ;
                       entity.PositionNo = txtPositionNo.Text ;
                       entity.UnitlCost = Decimal.Parse(txtUnitlCost.Text);
                        entity.SubtotalUnitCost = Decimal.Parse(txtSubtotalUnitCost.Text);
                        entity.PositionDesc = txtPositionDesc.Text ;
                       entity.ManufacturingProcessID = Int64.Parse(txtManufacturingProcessID.Text);
                        entity.OutputRate = Decimal.Parse(txtOutputRate.Text);
                        entity.PriorityUseType = Int32.Parse(txtPriorityUseType.Text);
                        entity.Sort = Int32.Parse(txtSort.Text);
                        entity.Summary = txtSummary.Text ;
                               return entity;
}
        */

        
        private tb_BOM_SDetailSubstituteMaterial _EditEntity;
        public tb_BOM_SDetailSubstituteMaterial EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_BOM_SDetailSubstituteMaterial entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_BOM_SDetail>(entity, k => k.SubID, v=>v.XXNAME, cmbSubID);
          // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
           DataBindingHelper.BindData4TextBox<tb_BOM_SDetailSubstituteMaterial>(entity, t => t.SKU, txtSKU, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_SDetailSubstituteMaterial>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_Unit>(entity, k => k.Unit_ID, v=>v.XXNAME, cmbUnit_ID);
          // DataBindingHelper.BindData4Cmb<tb_Unit_Conversion>(entity, k => k.UnitConversion_ID, v=>v.XXNAME, cmbUnitConversion_ID);
           DataBindingHelper.BindData4TextBox<tb_BOM_SDetailSubstituteMaterial>(entity, t => t.UsedQty.ToString(), txtUsedQty, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_SDetailSubstituteMaterial>(entity, t => t.Radix, txtRadix, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_SDetailSubstituteMaterial>(entity, t => t.LossRate.ToString(), txtLossRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_SDetailSubstituteMaterial>(entity, t => t.InstallPosition, txtInstallPosition, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_SDetailSubstituteMaterial>(entity, t => t.PositionNo, txtPositionNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_SDetailSubstituteMaterial>(entity, t => t.UnitlCost.ToString(), txtUnitlCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_SDetailSubstituteMaterial>(entity, t => t.SubtotalUnitCost.ToString(), txtSubtotalUnitCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_SDetailSubstituteMaterial>(entity, t => t.PositionDesc, txtPositionDesc, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_SDetailSubstituteMaterial>(entity, t => t.ManufacturingProcessID, txtManufacturingProcessID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_SDetailSubstituteMaterial>(entity, t => t.OutputRate.ToString(), txtOutputRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_SDetailSubstituteMaterial>(entity, t => t.PriorityUseType, txtPriorityUseType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_SDetailSubstituteMaterial>(entity, t => t.Sort, txtSort, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_SDetailSubstituteMaterial>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



