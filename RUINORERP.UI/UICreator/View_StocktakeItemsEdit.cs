
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:40
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
    /// 盘点明细统计数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class View_StocktakeItemsEdit:UserControl
    {
     public View_StocktakeItemsEdit() {
     
             
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        View_StocktakeItems UIToEntity()
        {
        View_StocktakeItems entity = new View_StocktakeItems();
                     entity.CheckNo = txtCheckNo.Text ;
                       entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.CheckMode = Int32.Parse(txtCheckMode.Text);
                        entity.Adjust_Type = Int32.Parse(txtAdjust_Type.Text);
                        entity.CheckResult = Int32.Parse(txtCheckResult.Text);
                        entity.Check_date = DateTime.Parse(txtCheck_date.Text);
                        entity.CarryingDate = DateTime.Parse(txtCarryingDate.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.ApprovalOpinions = txtApprovalOpinions.Text ;
                       entity.ApprovalResults = Boolean.Parse(txtApprovalResults.Text);
                        entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                        entity.SKU = txtSKU.Text ;
                       entity.CNName = txtCNName.Text ;
                       entity.Specifications = txtSpecifications.Text ;
                       entity.ProductNo = txtProductNo.Text ;
                       entity.Model = txtModel.Text ;
                       entity.Category_ID = Int64.Parse(txtCategory_ID.Text);
                        entity.Type_ID = Int64.Parse(txtType_ID.Text);
                        entity.Unit_ID = Int64.Parse(txtUnit_ID.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.Rack_ID = Int64.Parse(txtRack_ID.Text);
                        entity.Cost = Decimal.Parse(txtCost.Text);
                        entity.CarryinglQty = Int32.Parse(txtCarryinglQty.Text);
                        entity.CarryingSubtotalAmount = Decimal.Parse(txtCarryingSubtotalAmount.Text);
                        entity.DiffQty = Int32.Parse(txtDiffQty.Text);
                        entity.DiffSubtotalAmount = Decimal.Parse(txtDiffSubtotalAmount.Text);
                        entity.CheckQty = Int32.Parse(txtCheckQty.Text);
                        entity.CheckSubtotalAmount = Decimal.Parse(txtCheckSubtotalAmount.Text);
                        entity.property = txtproperty.Text ;
                               return entity;
}
        */

        
        private View_StocktakeItems _EditEntity;
        public View_StocktakeItems EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(View_StocktakeItems entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<View_StocktakeItems>(entity, t => t.CheckNo, txtCheckNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_StocktakeItems>(entity, t => t.Location_ID, txtLocation_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_StocktakeItems>(entity, t => t.Employee_ID, txtEmployee_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_StocktakeItems>(entity, t => t.CheckMode, txtCheckMode, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_StocktakeItems>(entity, t => t.Adjust_Type, txtAdjust_Type, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_StocktakeItems>(entity, t => t.CheckResult, txtCheckResult, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<View_StocktakeItems>(entity, t => t.Check_date, dtpCheck_date,false);
           DataBindingHelper.BindData4DataTime<View_StocktakeItems>(entity, t => t.CarryingDate, dtpCarryingDate,false);
           DataBindingHelper.BindData4TextBox<View_StocktakeItems>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<View_StocktakeItems>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<View_StocktakeItems>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_StocktakeItems>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_StocktakeItems>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<View_StocktakeItems>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           //default  DataBindingHelper.BindData4TextBox<View_StocktakeItems>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_StocktakeItems>(entity, t => t.SKU, txtSKU, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_StocktakeItems>(entity, t => t.CNName, txtCNName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_StocktakeItems>(entity, t => t.Specifications, txtSpecifications, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_StocktakeItems>(entity, t => t.ProductNo, txtProductNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_StocktakeItems>(entity, t => t.Model, txtModel, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_StocktakeItems>(entity, t => t.Category_ID, txtCategory_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_StocktakeItems>(entity, t => t.Type_ID, txtType_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_StocktakeItems>(entity, t => t.Unit_ID, txtUnit_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_StocktakeItems>(entity, t => t.ProdDetailID, txtProdDetailID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_StocktakeItems>(entity, t => t.Rack_ID, txtRack_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_StocktakeItems>(entity, t => t.Cost.ToString(), txtCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_StocktakeItems>(entity, t => t.CarryinglQty, txtCarryinglQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_StocktakeItems>(entity, t => t.CarryingSubtotalAmount.ToString(), txtCarryingSubtotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_StocktakeItems>(entity, t => t.DiffQty, txtDiffQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_StocktakeItems>(entity, t => t.DiffSubtotalAmount.ToString(), txtDiffSubtotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_StocktakeItems>(entity, t => t.CheckQty, txtCheckQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_StocktakeItems>(entity, t => t.CheckSubtotalAmount.ToString(), txtCheckSubtotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_StocktakeItems>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



