
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
    /// 生产领料统计数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class View_MaterialRequisitionItemsEdit:UserControl
    {
     public View_MaterialRequisitionItemsEdit() {
     
             
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        View_MaterialRequisitionItems UIToEntity()
        {
        View_MaterialRequisitionItems entity = new View_MaterialRequisitionItems();
                     entity.MaterialRequisitionNO = txtMaterialRequisitionNO.Text ;
                       entity.MONO = txtMONO.Text ;
                       entity.DeliveryDate = DateTime.Parse(txtDeliveryDate.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.ProjectGroup_ID = Int64.Parse(txtProjectGroup_ID.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.ApprovalOpinions = txtApprovalOpinions.Text ;
                       entity.Approver_by = Int64.Parse(txtApprover_by.Text);
                        entity.Approver_at = DateTime.Parse(txtApprover_at.Text);
                        entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                        entity.ApprovalResults = Boolean.Parse(txtApprovalResults.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
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
                        entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.SKU = txtSKU.Text ;
                       entity.ProdBaseID = Int64.Parse(txtProdBaseID.Text);
                        entity.CNName = txtCNName.Text ;
                       entity.Specifications = txtSpecifications.Text ;
                       entity.ProductNo = txtProductNo.Text ;
                       entity.Unit_ID = Int64.Parse(txtUnit_ID.Text);
                        entity.Model = txtModel.Text ;
                       entity.Category_ID = Int64.Parse(txtCategory_ID.Text);
                        entity.Type_ID = Int64.Parse(txtType_ID.Text);
                        entity.ShortCode = txtShortCode.Text ;
                       entity.BarCode = txtBarCode.Text ;
                               return entity;
}
        */

        
        private View_MaterialRequisitionItems _EditEntity;
        public View_MaterialRequisitionItems EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(View_MaterialRequisitionItems entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<View_MaterialRequisitionItems>(entity, t => t.MaterialRequisitionNO, txtMaterialRequisitionNO, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MaterialRequisitionItems>(entity, t => t.MONO, txtMONO, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<View_MaterialRequisitionItems>(entity, t => t.DeliveryDate, dtpDeliveryDate,false);
           DataBindingHelper.BindData4TextBox<View_MaterialRequisitionItems>(entity, t => t.Employee_ID, txtEmployee_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MaterialRequisitionItems>(entity, t => t.DepartmentID, txtDepartmentID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MaterialRequisitionItems>(entity, t => t.CustomerVendor_ID, txtCustomerVendor_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MaterialRequisitionItems>(entity, t => t.ProjectGroup_ID, txtProjectGroup_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<View_MaterialRequisitionItems>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<View_MaterialRequisitionItems>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MaterialRequisitionItems>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MaterialRequisitionItems>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MaterialRequisitionItems>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<View_MaterialRequisitionItems>(entity, t => t.Approver_at, dtpApprover_at,false);
           //default  DataBindingHelper.BindData4TextBox<View_MaterialRequisitionItems>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<View_MaterialRequisitionItems>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4TextBox<View_MaterialRequisitionItems>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MaterialRequisitionItems>(entity, t => t.ProdDetailID, txtProdDetailID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MaterialRequisitionItems>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MaterialRequisitionItems>(entity, t => t.ShouldSendQty, txtShouldSendQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MaterialRequisitionItems>(entity, t => t.ActualSentQty, txtActualSentQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MaterialRequisitionItems>(entity, t => t.CanQuantity, txtCanQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MaterialRequisitionItems>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MaterialRequisitionItems>(entity, t => t.CustomerPartNo, txtCustomerPartNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MaterialRequisitionItems>(entity, t => t.Cost.ToString(), txtCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_MaterialRequisitionItems>(entity, t => t.Price.ToString(), txtPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_MaterialRequisitionItems>(entity, t => t.SubtotalPrice.ToString(), txtSubtotalPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_MaterialRequisitionItems>(entity, t => t.SubtotalCost.ToString(), txtSubtotalCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_MaterialRequisitionItems>(entity, t => t.ReturnQty, txtReturnQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MaterialRequisitionItems>(entity, t => t.Location_ID, txtLocation_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MaterialRequisitionItems>(entity, t => t.SKU, txtSKU, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MaterialRequisitionItems>(entity, t => t.ProdBaseID, txtProdBaseID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MaterialRequisitionItems>(entity, t => t.CNName, txtCNName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MaterialRequisitionItems>(entity, t => t.Specifications, txtSpecifications, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MaterialRequisitionItems>(entity, t => t.ProductNo, txtProductNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MaterialRequisitionItems>(entity, t => t.Unit_ID, txtUnit_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MaterialRequisitionItems>(entity, t => t.Model, txtModel, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MaterialRequisitionItems>(entity, t => t.Category_ID, txtCategory_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MaterialRequisitionItems>(entity, t => t.Type_ID, txtType_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_MaterialRequisitionItems>(entity, t => t.ShortCode, txtShortCode, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_MaterialRequisitionItems>(entity, t => t.BarCode, txtBarCode, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



