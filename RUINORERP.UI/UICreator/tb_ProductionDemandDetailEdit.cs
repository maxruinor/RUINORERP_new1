
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:00
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
    /// 生产需求分析表明细数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_ProductionDemandDetailEdit:UserControl
    {
     public tb_ProductionDemandDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_ProductionDemandDetail UIToEntity()
        {
        tb_ProductionDemandDetail entity = new tb_ProductionDemandDetail();
                     entity.PDID = Int64.Parse(txtPDID.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.property = txtproperty.Text ;
                       entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.BOM_ID = Int64.Parse(txtBOM_ID.Text);
                        entity.ID = Int64.Parse(txtID.Text);
                        entity.ParentId = Int64.Parse(txtParentId.Text);
                        entity.NetRequirement = Int32.Parse(txtNetRequirement.Text);
                        entity.GrossRequirement = Int32.Parse(txtGrossRequirement.Text);
                        entity.NeedQuantity = Int32.Parse(txtNeedQuantity.Text);
                        entity.MissingQuantity = Int32.Parse(txtMissingQuantity.Text);
                        entity.RequirementDate = DateTime.Parse(txtRequirementDate.Text);
                        entity.BookInventory = Int32.Parse(txtBookInventory.Text);
                        entity.AvailableStock = Int32.Parse(txtAvailableStock.Text);
                        entity.InTransitInventory = Int32.Parse(txtInTransitInventory.Text);
                        entity.MakeProcessInventory = Int32.Parse(txtMakeProcessInventory.Text);
                        entity.Sale_Qty = Int32.Parse(txtSale_Qty.Text);
                        entity.NotOutQty = Int32.Parse(txtNotOutQty.Text);
                        entity.Summary = txtSummary.Text ;
                               return entity;
}
        */

        
        private tb_ProductionDemandDetail _EditEntity;
        public tb_ProductionDemandDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_ProductionDemandDetail entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_ProductionDemand>(entity, k => k.PDID, v=>v.XXNAME, cmbPDID);
          // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
           DataBindingHelper.BindData4TextBox<tb_ProductionDemandDetail>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
          // DataBindingHelper.BindData4Cmb<tb_BOM_S>(entity, k => k.BOM_ID, v=>v.XXNAME, cmbBOM_ID);
           DataBindingHelper.BindData4TextBox<tb_ProductionDemandDetail>(entity, t => t.ID, txtID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ProductionDemandDetail>(entity, t => t.ParentId, txtParentId, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ProductionDemandDetail>(entity, t => t.NetRequirement, txtNetRequirement, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ProductionDemandDetail>(entity, t => t.GrossRequirement, txtGrossRequirement, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ProductionDemandDetail>(entity, t => t.NeedQuantity, txtNeedQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ProductionDemandDetail>(entity, t => t.MissingQuantity, txtMissingQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_ProductionDemandDetail>(entity, t => t.RequirementDate, dtpRequirementDate,false);
           DataBindingHelper.BindData4TextBox<tb_ProductionDemandDetail>(entity, t => t.BookInventory, txtBookInventory, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ProductionDemandDetail>(entity, t => t.AvailableStock, txtAvailableStock, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ProductionDemandDetail>(entity, t => t.InTransitInventory, txtInTransitInventory, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ProductionDemandDetail>(entity, t => t.MakeProcessInventory, txtMakeProcessInventory, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ProductionDemandDetail>(entity, t => t.Sale_Qty, txtSale_Qty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ProductionDemandDetail>(entity, t => t.NotOutQty, txtNotOutQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ProductionDemandDetail>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



