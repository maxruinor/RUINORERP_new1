
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:29
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
    /// 制令单明细统计数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class View_ManufacturingOrderItemsEdit:UserControl
    {
     public View_ManufacturingOrderItemsEdit() {
     
             
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        View_ManufacturingOrderItems UIToEntity()
        {
        View_ManufacturingOrderItems entity = new View_ManufacturingOrderItems();
                     entity.MONO = txtMONO.Text ;
                       entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.CustomerVendor_ID_Out = Int64.Parse(txtCustomerVendor_ID_Out.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.ManufacturingQty = Int32.Parse(txtManufacturingQty.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.IsOutSourced = Boolean.Parse(txtIsOutSourced.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.ShouldSendQty = Decimal.Parse(txtShouldSendQty.Text);
                        entity.ActualSentQty = Decimal.Parse(txtActualSentQty.Text);
                        entity.UnitCost = Decimal.Parse(txtUnitCost.Text);
                        entity.SubtotalUnitCost = Decimal.Parse(txtSubtotalUnitCost.Text);
                        entity.BOM_ID = Int64.Parse(txtBOM_ID.Text);
                        entity.BOM_NO = txtBOM_NO.Text ;
                       entity.Summary = txtSummary.Text ;
                       entity.property = txtproperty.Text ;
                       entity.ProdBaseID = Int64.Parse(txtProdBaseID.Text);
                        entity.SKU = txtSKU.Text ;
                       entity.CNName = txtCNName.Text ;
                       entity.Specifications = txtSpecifications.Text ;
                       entity.Quantity = Int32.Parse(txtQuantity.Text);
                        entity.prop = txtprop.Text ;
                       entity.ProductNo = txtProductNo.Text ;
                       entity.Model = txtModel.Text ;
                       entity.Category_ID = Int64.Parse(txtCategory_ID.Text);
                        entity.Type_ID = Int64.Parse(txtType_ID.Text);
                                return entity;
}
        */

        
        private View_ManufacturingOrderItems _EditEntity;
        public View_ManufacturingOrderItems EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(View_ManufacturingOrderItems entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<View_ManufacturingOrderItems>(entity, t => t.MONO, txtMONO, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ManufacturingOrderItems>(entity, t => t.Employee_ID, txtEmployee_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ManufacturingOrderItems>(entity, t => t.DepartmentID, txtDepartmentID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ManufacturingOrderItems>(entity, t => t.CustomerVendor_ID, txtCustomerVendor_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ManufacturingOrderItems>(entity, t => t.CustomerVendor_ID_Out, txtCustomerVendor_ID_Out, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<View_ManufacturingOrderItems>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<View_ManufacturingOrderItems>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ManufacturingOrderItems>(entity, t => t.ManufacturingQty, txtManufacturingQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ManufacturingOrderItems>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ManufacturingOrderItems>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<View_ManufacturingOrderItems>(entity, t => t.IsOutSourced, chkIsOutSourced, false);
           DataBindingHelper.BindData4TextBox<View_ManufacturingOrderItems>(entity, t => t.ProdDetailID, txtProdDetailID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ManufacturingOrderItems>(entity, t => t.Location_ID, txtLocation_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ManufacturingOrderItems>(entity, t => t.ShouldSendQty.ToString(), txtShouldSendQty, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_ManufacturingOrderItems>(entity, t => t.ActualSentQty.ToString(), txtActualSentQty, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_ManufacturingOrderItems>(entity, t => t.UnitCost.ToString(), txtUnitCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_ManufacturingOrderItems>(entity, t => t.SubtotalUnitCost.ToString(), txtSubtotalUnitCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_ManufacturingOrderItems>(entity, t => t.BOM_ID, txtBOM_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ManufacturingOrderItems>(entity, t => t.BOM_NO, txtBOM_NO, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ManufacturingOrderItems>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ManufacturingOrderItems>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ManufacturingOrderItems>(entity, t => t.ProdBaseID, txtProdBaseID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ManufacturingOrderItems>(entity, t => t.SKU, txtSKU, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ManufacturingOrderItems>(entity, t => t.CNName, txtCNName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ManufacturingOrderItems>(entity, t => t.Specifications, txtSpecifications, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ManufacturingOrderItems>(entity, t => t.Quantity, txtQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ManufacturingOrderItems>(entity, t => t.prop, txtprop, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ManufacturingOrderItems>(entity, t => t.ProductNo, txtProductNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ManufacturingOrderItems>(entity, t => t.Model, txtModel, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ManufacturingOrderItems>(entity, t => t.Category_ID, txtCategory_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ManufacturingOrderItems>(entity, t => t.Type_ID, txtType_ID, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



