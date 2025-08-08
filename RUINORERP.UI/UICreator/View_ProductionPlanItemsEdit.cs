
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:35
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
    /// 计划单明细统计数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class View_ProductionPlanItemsEdit:UserControl
    {
     public View_ProductionPlanItemsEdit() {
     
             
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        View_ProductionPlanItems UIToEntity()
        {
        View_ProductionPlanItems entity = new View_ProductionPlanItems();
                     entity.SaleOrderNo = txtSaleOrderNo.Text ;
                       entity.PPNo = txtPPNo.Text ;
                       entity.ProjectGroup_ID = Int64.Parse(txtProjectGroup_ID.Text);
                        entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.Priority = Int32.Parse(txtPriority.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.RequirementDate = DateTime.Parse(txtRequirementDate.Text);
                        entity.PlanDate = DateTime.Parse(txtPlanDate.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.Specifications = txtSpecifications.Text ;
                       entity.property = txtproperty.Text ;
                       entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.Quantity = Int32.Parse(txtQuantity.Text);
                        entity.BOM_ID = Int64.Parse(txtBOM_ID.Text);
                        entity.SKU = txtSKU.Text ;
                       entity.CNName = txtCNName.Text ;
                       entity.ProductNo = txtProductNo.Text ;
                       entity.Unit_ID = Int64.Parse(txtUnit_ID.Text);
                        entity.Model = txtModel.Text ;
                       entity.Category_ID = Int64.Parse(txtCategory_ID.Text);
                        entity.Type_ID = Int64.Parse(txtType_ID.Text);
                        entity.BarCode = txtBarCode.Text ;
                       entity.ShortCode = txtShortCode.Text ;
                       entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                        entity.IsAnalyzed = Boolean.Parse(txtIsAnalyzed.Text);
                        entity.AnalyzedQuantity = Int32.Parse(txtAnalyzedQuantity.Text);
                        entity.CompletedQuantity = Int32.Parse(txtCompletedQuantity.Text);
                        entity.Summary = txtSummary.Text ;
                               return entity;
}
        */

        
        private View_ProductionPlanItems _EditEntity;
        public View_ProductionPlanItems EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(View_ProductionPlanItems entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<View_ProductionPlanItems>(entity, t => t.SaleOrderNo, txtSaleOrderNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProductionPlanItems>(entity, t => t.PPNo, txtPPNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProductionPlanItems>(entity, t => t.ProjectGroup_ID, txtProjectGroup_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProductionPlanItems>(entity, t => t.DepartmentID, txtDepartmentID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProductionPlanItems>(entity, t => t.Priority, txtPriority, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProductionPlanItems>(entity, t => t.Employee_ID, txtEmployee_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<View_ProductionPlanItems>(entity, t => t.RequirementDate, dtpRequirementDate,false);
           DataBindingHelper.BindData4DataTime<View_ProductionPlanItems>(entity, t => t.PlanDate, dtpPlanDate,false);
           DataBindingHelper.BindData4TextBox<View_ProductionPlanItems>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<View_ProductionPlanItems>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<View_ProductionPlanItems>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProductionPlanItems>(entity, t => t.ProdDetailID, txtProdDetailID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProductionPlanItems>(entity, t => t.Specifications, txtSpecifications, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProductionPlanItems>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProductionPlanItems>(entity, t => t.Location_ID, txtLocation_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProductionPlanItems>(entity, t => t.Quantity, txtQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProductionPlanItems>(entity, t => t.BOM_ID, txtBOM_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProductionPlanItems>(entity, t => t.SKU, txtSKU, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProductionPlanItems>(entity, t => t.CNName, txtCNName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProductionPlanItems>(entity, t => t.ProductNo, txtProductNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProductionPlanItems>(entity, t => t.Unit_ID, txtUnit_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProductionPlanItems>(entity, t => t.Model, txtModel, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProductionPlanItems>(entity, t => t.Category_ID, txtCategory_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProductionPlanItems>(entity, t => t.Type_ID, txtType_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProductionPlanItems>(entity, t => t.BarCode, txtBarCode, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProductionPlanItems>(entity, t => t.ShortCode, txtShortCode, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<View_ProductionPlanItems>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4CheckBox<View_ProductionPlanItems>(entity, t => t.IsAnalyzed, chkIsAnalyzed, false);
           DataBindingHelper.BindData4TextBox<View_ProductionPlanItems>(entity, t => t.AnalyzedQuantity, txtAnalyzedQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProductionPlanItems>(entity, t => t.CompletedQuantity, txtCompletedQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProductionPlanItems>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



