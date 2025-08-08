
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:27
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
    /// 缴库明细统计数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class View_FinishedGoodsInvItemsEdit:UserControl
    {
     public View_FinishedGoodsInvItemsEdit() {
     
             
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        View_FinishedGoodsInvItems UIToEntity()
        {
        View_FinishedGoodsInvItems entity = new View_FinishedGoodsInvItems();
                     entity.DeliveryBillNo = txtDeliveryBillNo.Text ;
                       entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.DeliveryDate = DateTime.Parse(txtDeliveryDate.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.MONo = txtMONo.Text ;
                       entity.Unit_ID = Int64.Parse(txtUnit_ID.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.Rack_ID = Int64.Parse(txtRack_ID.Text);
                        entity.PayableQty = Int32.Parse(txtPayableQty.Text);
                        entity.Qty = Int32.Parse(txtQty.Text);
                        entity.UnitCost = Decimal.Parse(txtUnitCost.Text);
                        entity.UnpaidQty = Int32.Parse(txtUnpaidQty.Text);
                        entity.NetMachineHours = Decimal.Parse(txtNetMachineHours.Text);
                        entity.NetWorkingHours = Decimal.Parse(txtNetWorkingHours.Text);
                        entity.ApportionedCost = Decimal.Parse(txtApportionedCost.Text);
                        entity.ManuFee = Decimal.Parse(txtManuFee.Text);
                        entity.MaterialCost = Decimal.Parse(txtMaterialCost.Text);
                        entity.ProductionAllCost = Decimal.Parse(txtProductionAllCost.Text);
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
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.IsOutSourced = Boolean.Parse(txtIsOutSourced.Text);
                                return entity;
}
        */

        
        private View_FinishedGoodsInvItems _EditEntity;
        public View_FinishedGoodsInvItems EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(View_FinishedGoodsInvItems entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<View_FinishedGoodsInvItems>(entity, t => t.DeliveryBillNo, txtDeliveryBillNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_FinishedGoodsInvItems>(entity, t => t.Employee_ID, txtEmployee_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FinishedGoodsInvItems>(entity, t => t.DepartmentID, txtDepartmentID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FinishedGoodsInvItems>(entity, t => t.CustomerVendor_ID, txtCustomerVendor_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<View_FinishedGoodsInvItems>(entity, t => t.DeliveryDate, dtpDeliveryDate,false);
           DataBindingHelper.BindData4DataTime<View_FinishedGoodsInvItems>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<View_FinishedGoodsInvItems>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FinishedGoodsInvItems>(entity, t => t.MONo, txtMONo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_FinishedGoodsInvItems>(entity, t => t.Unit_ID, txtUnit_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FinishedGoodsInvItems>(entity, t => t.ProdDetailID, txtProdDetailID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FinishedGoodsInvItems>(entity, t => t.Location_ID, txtLocation_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FinishedGoodsInvItems>(entity, t => t.Rack_ID, txtRack_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FinishedGoodsInvItems>(entity, t => t.PayableQty, txtPayableQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FinishedGoodsInvItems>(entity, t => t.Qty, txtQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FinishedGoodsInvItems>(entity, t => t.UnitCost.ToString(), txtUnitCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_FinishedGoodsInvItems>(entity, t => t.UnpaidQty, txtUnpaidQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FinishedGoodsInvItems>(entity, t => t.NetMachineHours.ToString(), txtNetMachineHours, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_FinishedGoodsInvItems>(entity, t => t.NetWorkingHours.ToString(), txtNetWorkingHours, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_FinishedGoodsInvItems>(entity, t => t.ApportionedCost.ToString(), txtApportionedCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_FinishedGoodsInvItems>(entity, t => t.ManuFee.ToString(), txtManuFee, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_FinishedGoodsInvItems>(entity, t => t.MaterialCost.ToString(), txtMaterialCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_FinishedGoodsInvItems>(entity, t => t.ProductionAllCost.ToString(), txtProductionAllCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_FinishedGoodsInvItems>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_FinishedGoodsInvItems>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_FinishedGoodsInvItems>(entity, t => t.ProdBaseID, txtProdBaseID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FinishedGoodsInvItems>(entity, t => t.SKU, txtSKU, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_FinishedGoodsInvItems>(entity, t => t.CNName, txtCNName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_FinishedGoodsInvItems>(entity, t => t.Specifications, txtSpecifications, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_FinishedGoodsInvItems>(entity, t => t.Quantity, txtQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FinishedGoodsInvItems>(entity, t => t.prop, txtprop, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_FinishedGoodsInvItems>(entity, t => t.ProductNo, txtProductNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_FinishedGoodsInvItems>(entity, t => t.Model, txtModel, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_FinishedGoodsInvItems>(entity, t => t.Category_ID, txtCategory_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FinishedGoodsInvItems>(entity, t => t.Type_ID, txtType_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FinishedGoodsInvItems>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_FinishedGoodsInvItems>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<View_FinishedGoodsInvItems>(entity, t => t.IsOutSourced, chkIsOutSourced, false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



