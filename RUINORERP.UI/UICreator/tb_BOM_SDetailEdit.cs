
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/27/2024 11:23:48
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
    /// 标准物料表BOM明细-要适当冗余数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_BOM_SDetailEdit:UserControl
    {
     public tb_BOM_SDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_BOM_SDetail UIToEntity()
        {
        tb_BOM_SDetail entity = new tb_BOM_SDetail();
                     entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.IsKeyMaterial = Boolean.Parse(txtIsKeyMaterial.Text);
                        entity.SKU = txtSKU.Text ;
                       entity.BOM_ID = Int64.Parse(txtBOM_ID.Text);
                        entity.Summary = txtSummary.Text ;
                       entity.Unit_ID = Int64.Parse(txtUnit_ID.Text);
                        entity.UnitConversion_ID = Int64.Parse(txtUnitConversion_ID.Text);
                        entity.UsedQty = Decimal.Parse(txtUsedQty.Text);
                        entity.Radix = Int32.Parse(txtRadix.Text);
                        entity.LossRate = Decimal.Parse(txtLossRate.Text);
                        entity.InstallPosition = txtInstallPosition.Text ;
                       entity.PositionNo = txtPositionNo.Text ;
                       entity.UnitCost = Decimal.Parse(txtUnitCost.Text);
                        entity.SubtotalUnitCost = Decimal.Parse(txtSubtotalUnitCost.Text);
                        entity.PositionDesc = txtPositionDesc.Text ;
                       entity.ManufacturingProcessID = Int64.Parse(txtManufacturingProcessID.Text);
                        entity.Child_BOM_Node_ID = Int64.Parse(txtChild_BOM_Node_ID.Text);
                        entity.Substitute = Int64.Parse(txtSubstitute.Text);
                        entity.OutputRate = Decimal.Parse(txtOutputRate.Text);
                        entity.Sort = Int32.Parse(txtSort.Text);
                                return entity;
}
        */

        
        private tb_BOM_SDetail _EditEntity;
        public tb_BOM_SDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_BOM_SDetail entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
           DataBindingHelper.BindData4CheckBox<tb_BOM_SDetail>(entity, t => t.IsKeyMaterial, chkIsKeyMaterial, false);
           DataBindingHelper.BindData4TextBox<tb_BOM_SDetail>(entity, t => t.SKU, txtSKU, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_BOM_S>(entity, k => k.BOM_ID, v=>v.XXNAME, cmbBOM_ID);
           DataBindingHelper.BindData4TextBox<tb_BOM_SDetail>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_Unit>(entity, k => k.Unit_ID, v=>v.XXNAME, cmbUnit_ID);
          // DataBindingHelper.BindData4Cmb<tb_Unit_Conversion>(entity, k => k.UnitConversion_ID, v=>v.XXNAME, cmbUnitConversion_ID);
           DataBindingHelper.BindData4TextBox<tb_BOM_SDetail>(entity, t => t.UsedQty.ToString(), txtUsedQty, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_SDetail>(entity, t => t.Radix, txtRadix, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_SDetail>(entity, t => t.LossRate.ToString(), txtLossRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_SDetail>(entity, t => t.InstallPosition, txtInstallPosition, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_SDetail>(entity, t => t.PositionNo, txtPositionNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_SDetail>(entity, t => t.UnitCost.ToString(), txtUnitCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_SDetail>(entity, t => t.SubtotalUnitCost.ToString(), txtSubtotalUnitCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_SDetail>(entity, t => t.PositionDesc, txtPositionDesc, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_SDetail>(entity, t => t.ManufacturingProcessID, txtManufacturingProcessID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_SDetail>(entity, t => t.Child_BOM_Node_ID, txtChild_BOM_Node_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_SDetail>(entity, t => t.Substitute, txtSubstitute, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_SDetail>(entity, t => t.OutputRate.ToString(), txtOutputRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_SDetail>(entity, t => t.Sort, txtSort, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



