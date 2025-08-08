
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:37
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
    /// 采购订单统计数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class View_PurOrderItemsEdit:UserControl
    {
     public View_PurOrderItemsEdit() {
     
             
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        View_PurOrderItems UIToEntity()
        {
        View_PurOrderItems entity = new View_PurOrderItems();
                     entity.PurOrder_ID = Int64.Parse(txtPurOrder_ID.Text);
                        entity.PurOrderNo = txtPurOrderNo.Text ;
                       entity.PDID = Int64.Parse(txtPDID.Text);
                        entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.Paytype_ID = Int64.Parse(txtPaytype_ID.Text);
                        entity.SOrder_ID = Int64.Parse(txtSOrder_ID.Text);
                        entity.PurDate = DateTime.Parse(txtPurDate.Text);
                        entity.IsIncludeTax = Boolean.Parse(txtIsIncludeTax.Text);
                        entity.ShipCost = Decimal.Parse(txtShipCost.Text);
                        entity.OrderPreDeliveryDate = DateTime.Parse(txtOrderPreDeliveryDate.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.Arrival_date = DateTime.Parse(txtArrival_date.Text);
                        entity.Deposit = Decimal.Parse(txtDeposit.Text);
                        entity.TaxDeductionType = Int32.Parse(txtTaxDeductionType.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.SKU = txtSKU.Text ;
                       entity.Specifications = txtSpecifications.Text ;
                       entity.CNName = txtCNName.Text ;
                       entity.Model = txtModel.Text ;
                       entity.Type_ID = Int64.Parse(txtType_ID.Text);
                        entity.property = txtproperty.Text ;
                       entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.Quantity = Int32.Parse(txtQuantity.Text);
                        entity.UnitPrice = Decimal.Parse(txtUnitPrice.Text);
                        entity.TaxRate = Decimal.Parse(txtTaxRate.Text);
                        entity.TaxAmount = Decimal.Parse(txtTaxAmount.Text);
                        entity.SubtotalAmount = Decimal.Parse(txtSubtotalAmount.Text);
                        entity.IsGift = Boolean.Parse(txtIsGift.Text);
                        entity.ItemPreDeliveryDate = DateTime.Parse(txtItemPreDeliveryDate.Text);
                        entity.CustomertModel = txtCustomertModel.Text ;
                       entity.DeliveredQuantity = Int32.Parse(txtDeliveredQuantity.Text);
                        entity.Summary = txtSummary.Text ;
                               return entity;
}
        */

        
        private View_PurOrderItems _EditEntity;
        public View_PurOrderItems EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(View_PurOrderItems entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<View_PurOrderItems>(entity, t => t.PurOrder_ID, txtPurOrder_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_PurOrderItems>(entity, t => t.PurOrderNo, txtPurOrderNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_PurOrderItems>(entity, t => t.PDID, txtPDID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_PurOrderItems>(entity, t => t.CustomerVendor_ID, txtCustomerVendor_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_PurOrderItems>(entity, t => t.Employee_ID, txtEmployee_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_PurOrderItems>(entity, t => t.DepartmentID, txtDepartmentID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_PurOrderItems>(entity, t => t.Paytype_ID, txtPaytype_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_PurOrderItems>(entity, t => t.SOrder_ID, txtSOrder_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<View_PurOrderItems>(entity, t => t.PurDate, dtpPurDate,false);
           DataBindingHelper.BindData4CheckBox<View_PurOrderItems>(entity, t => t.IsIncludeTax, chkIsIncludeTax, false);
           DataBindingHelper.BindData4TextBox<View_PurOrderItems>(entity, t => t.ShipCost.ToString(), txtShipCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4DataTime<View_PurOrderItems>(entity, t => t.OrderPreDeliveryDate, dtpOrderPreDeliveryDate,false);
           DataBindingHelper.BindData4TextBox<View_PurOrderItems>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<View_PurOrderItems>(entity, t => t.Arrival_date, dtpArrival_date,false);
           DataBindingHelper.BindData4TextBox<View_PurOrderItems>(entity, t => t.Deposit.ToString(), txtDeposit, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_PurOrderItems>(entity, t => t.TaxDeductionType, txtTaxDeductionType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_PurOrderItems>(entity, t => t.ProdDetailID, txtProdDetailID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_PurOrderItems>(entity, t => t.SKU, txtSKU, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_PurOrderItems>(entity, t => t.Specifications, txtSpecifications, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_PurOrderItems>(entity, t => t.CNName, txtCNName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_PurOrderItems>(entity, t => t.Model, txtModel, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_PurOrderItems>(entity, t => t.Type_ID, txtType_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_PurOrderItems>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_PurOrderItems>(entity, t => t.Location_ID, txtLocation_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_PurOrderItems>(entity, t => t.Quantity, txtQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_PurOrderItems>(entity, t => t.UnitPrice.ToString(), txtUnitPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_PurOrderItems>(entity, t => t.TaxRate.ToString(), txtTaxRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_PurOrderItems>(entity, t => t.TaxAmount.ToString(), txtTaxAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_PurOrderItems>(entity, t => t.SubtotalAmount.ToString(), txtSubtotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<View_PurOrderItems>(entity, t => t.IsGift, chkIsGift, false);
           DataBindingHelper.BindData4DataTime<View_PurOrderItems>(entity, t => t.ItemPreDeliveryDate, dtpItemPreDeliveryDate,false);
           DataBindingHelper.BindData4TextBox<View_PurOrderItems>(entity, t => t.CustomertModel, txtCustomertModel, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_PurOrderItems>(entity, t => t.DeliveredQuantity, txtDeliveredQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_PurOrderItems>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



