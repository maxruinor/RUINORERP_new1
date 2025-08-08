
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:40
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
    /// 制令单的原料明细表 明细对应的是一个树，结构同BOM，先把BOM搞好再来实现这里的细节数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_ManufacturingOrderDetailEdit:UserControl
    {
     public tb_ManufacturingOrderDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_ManufacturingOrderDetail UIToEntity()
        {
        tb_ManufacturingOrderDetail entity = new tb_ManufacturingOrderDetail();
                     entity.MOID = Int64.Parse(txtMOID.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.IsKeyMaterial = Boolean.Parse(txtIsKeyMaterial.Text);
                        entity.property = txtproperty.Text ;
                       entity.ID = Int64.Parse(txtID.Text);
                        entity.ParentId = Int64.Parse(txtParentId.Text);
                        entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.BOM_NO = txtBOM_NO.Text ;
                       entity.ShouldSendQty = Decimal.Parse(txtShouldSendQty.Text);
                        entity.ActualSentQty = Decimal.Parse(txtActualSentQty.Text);
                        entity.OverSentQty = Decimal.Parse(txtOverSentQty.Text);
                        entity.WastageQty = Decimal.Parse(txtWastageQty.Text);
                        entity.CurrentIinventory = Decimal.Parse(txtCurrentIinventory.Text);
                        entity.UnitCost = Decimal.Parse(txtUnitCost.Text);
                        entity.SubtotalUnitCost = Decimal.Parse(txtSubtotalUnitCost.Text);
                        entity.BOM_ID = Int64.Parse(txtBOM_ID.Text);
                        entity.IsExternalProduce = Boolean.Parse(txtIsExternalProduce.Text);
                        entity.Summary = txtSummary.Text ;
                       entity.AssemblyPosition = txtAssemblyPosition.Text ;
                       entity.AlternativeProducts = txtAlternativeProducts.Text ;
                       entity.Prelevel_BOM_Desc = txtPrelevel_BOM_Desc.Text ;
                       entity.Prelevel_BOM_ID = Int64.Parse(txtPrelevel_BOM_ID.Text);
                                return entity;
}
        */

        
        private tb_ManufacturingOrderDetail _EditEntity;
        public tb_ManufacturingOrderDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_ManufacturingOrderDetail entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_ManufacturingOrder>(entity, k => k.MOID, v=>v.XXNAME, cmbMOID);
          // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
           DataBindingHelper.BindData4CheckBox<tb_ManufacturingOrderDetail>(entity, t => t.IsKeyMaterial, chkIsKeyMaterial, false);
           DataBindingHelper.BindData4TextBox<tb_ManufacturingOrderDetail>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ManufacturingOrderDetail>(entity, t => t.ID, txtID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ManufacturingOrderDetail>(entity, t => t.ParentId, txtParentId, BindDataType4TextBox.Qty,false);
          // DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
           DataBindingHelper.BindData4TextBox<tb_ManufacturingOrderDetail>(entity, t => t.BOM_NO, txtBOM_NO, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ManufacturingOrderDetail>(entity, t => t.ShouldSendQty.ToString(), txtShouldSendQty, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_ManufacturingOrderDetail>(entity, t => t.ActualSentQty.ToString(), txtActualSentQty, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_ManufacturingOrderDetail>(entity, t => t.OverSentQty.ToString(), txtOverSentQty, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_ManufacturingOrderDetail>(entity, t => t.WastageQty.ToString(), txtWastageQty, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_ManufacturingOrderDetail>(entity, t => t.CurrentIinventory.ToString(), txtCurrentIinventory, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_ManufacturingOrderDetail>(entity, t => t.UnitCost.ToString(), txtUnitCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_ManufacturingOrderDetail>(entity, t => t.SubtotalUnitCost.ToString(), txtSubtotalUnitCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_ManufacturingOrderDetail>(entity, t => t.BOM_ID, txtBOM_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_ManufacturingOrderDetail>(entity, t => t.IsExternalProduce, chkIsExternalProduce, false);
           DataBindingHelper.BindData4TextBox<tb_ManufacturingOrderDetail>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ManufacturingOrderDetail>(entity, t => t.AssemblyPosition, txtAssemblyPosition, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ManufacturingOrderDetail>(entity, t => t.AlternativeProducts, txtAlternativeProducts, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ManufacturingOrderDetail>(entity, t => t.Prelevel_BOM_Desc, txtPrelevel_BOM_Desc, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ManufacturingOrderDetail>(entity, t => t.Prelevel_BOM_ID, txtPrelevel_BOM_ID, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



