
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:30
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
    /// 退料统计数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class View_MaterialReturnItemsEdit:UserControl
    {
     public View_MaterialReturnItemsEdit() {
     
             
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        View_MaterialReturnItems UIToEntity()
        {
        View_MaterialReturnItems entity = new View_MaterialReturnItems();
                     entity.BillNo = txtBillNo.Text ;
                       entity.BillType = Int32.Parse(txtBillType.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.Outgoing = Boolean.Parse(txtOutgoing.Text);
                        entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.ReturnDate = DateTime.Parse(txtReturnDate.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.MaterialRequisitionNO = txtMaterialRequisitionNO.Text ;
                       entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.property = txtproperty.Text ;
                       entity.Quantity = Int32.Parse(txtQuantity.Text);
                        entity.Cost = Decimal.Parse(txtCost.Text);
                        entity.Price = Decimal.Parse(txtPrice.Text);
                        entity.SKU = txtSKU.Text ;
                       entity.CNName = txtCNName.Text ;
                       entity.Specifications = txtSpecifications.Text ;
                       entity.ProductNo = txtProductNo.Text ;
                       entity.Unit_ID = Int64.Parse(txtUnit_ID.Text);
                        entity.Model = txtModel.Text ;
                       entity.Category_ID = Int64.Parse(txtCategory_ID.Text);
                        entity.Type_ID = Int64.Parse(txtType_ID.Text);
                                return entity;
}
        */

        
        private View_MaterialReturnItems _EditEntity;
        public View_MaterialReturnItems EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(View_MaterialReturnItems entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<View_MaterialReturnItems>(entity, t => t.BillNo, txtBillNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MaterialReturnItems>(entity, t => t.BillType, txtBillType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MaterialReturnItems>(entity, t => t.Employee_ID, txtEmployee_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MaterialReturnItems>(entity, t => t.DepartmentID, txtDepartmentID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<View_MaterialReturnItems>(entity, t => t.Outgoing, chkOutgoing, false);
           DataBindingHelper.BindData4TextBox<View_MaterialReturnItems>(entity, t => t.CustomerVendor_ID, txtCustomerVendor_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<View_MaterialReturnItems>(entity, t => t.ReturnDate, dtpReturnDate,false);
           DataBindingHelper.BindData4DataTime<View_MaterialReturnItems>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<View_MaterialReturnItems>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MaterialReturnItems>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MaterialReturnItems>(entity, t => t.MaterialRequisitionNO, txtMaterialRequisitionNO, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<View_MaterialReturnItems>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4TextBox<View_MaterialReturnItems>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MaterialReturnItems>(entity, t => t.ProdDetailID, txtProdDetailID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MaterialReturnItems>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MaterialReturnItems>(entity, t => t.Quantity, txtQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MaterialReturnItems>(entity, t => t.Cost.ToString(), txtCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_MaterialReturnItems>(entity, t => t.Price.ToString(), txtPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_MaterialReturnItems>(entity, t => t.SKU, txtSKU, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MaterialReturnItems>(entity, t => t.CNName, txtCNName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MaterialReturnItems>(entity, t => t.Specifications, txtSpecifications, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MaterialReturnItems>(entity, t => t.ProductNo, txtProductNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MaterialReturnItems>(entity, t => t.Unit_ID, txtUnit_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MaterialReturnItems>(entity, t => t.Model, txtModel, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MaterialReturnItems>(entity, t => t.Category_ID, txtCategory_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MaterialReturnItems>(entity, t => t.Type_ID, txtType_ID, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



