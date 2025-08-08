
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:41
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
    /// 领料单明细数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_MaterialRequisitionDetailEdit:UserControl
    {
     public tb_MaterialRequisitionDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_MaterialRequisitionDetail UIToEntity()
        {
        tb_MaterialRequisitionDetail entity = new tb_MaterialRequisitionDetail();
                     entity.MR_ID = Int64.Parse(txtMR_ID.Text);
                        entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.property = txtproperty.Text ;
                       entity.ShouldSendQty = Int32.Parse(txtShouldSendQty.Text);
                        entity.ActualSentQty = Int32.Parse(txtActualSentQty.Text);
                        entity.CanQuantity = Int32.Parse(txtCanQuantity.Text);
                        entity.Summary = txtSummary.Text ;
                       entity.CustomerPartNo = txtCustomerPartNo.Text ;
                       entity.Cost = Decimal.Parse(txtCost.Text);
                        entity.Price = Decimal.Parse(txtPrice.Text);
                        entity.SubtotalPrice = Decimal.Parse(txtSubtotalPrice.Text);
                        entity.SubtotalCost = Decimal.Parse(txtSubtotalCost.Text);
                        entity.ReturnQty = Int32.Parse(txtReturnQty.Text);
                        entity.ManufacturingOrderDetailRowID = Int64.Parse(txtManufacturingOrderDetailRowID.Text);
                                return entity;
}
        */

        
        private tb_MaterialRequisitionDetail _EditEntity;
        public tb_MaterialRequisitionDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_MaterialRequisitionDetail entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_MaterialRequisition>(entity, k => k.MR_ID, v=>v.XXNAME, cmbMR_ID);
          // DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
          // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
           DataBindingHelper.BindData4TextBox<tb_MaterialRequisitionDetail>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialRequisitionDetail>(entity, t => t.ShouldSendQty, txtShouldSendQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialRequisitionDetail>(entity, t => t.ActualSentQty, txtActualSentQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialRequisitionDetail>(entity, t => t.CanQuantity, txtCanQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialRequisitionDetail>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialRequisitionDetail>(entity, t => t.CustomerPartNo, txtCustomerPartNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialRequisitionDetail>(entity, t => t.Cost.ToString(), txtCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialRequisitionDetail>(entity, t => t.Price.ToString(), txtPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialRequisitionDetail>(entity, t => t.SubtotalPrice.ToString(), txtSubtotalPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialRequisitionDetail>(entity, t => t.SubtotalCost.ToString(), txtSubtotalCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialRequisitionDetail>(entity, t => t.ReturnQty, txtReturnQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialRequisitionDetail>(entity, t => t.ManufacturingOrderDetailRowID, txtManufacturingOrderDetailRowID, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



