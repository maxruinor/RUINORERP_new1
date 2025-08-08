
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
    /// 调拨明细统计数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class View_StockTransferItemsEdit:UserControl
    {
     public View_StockTransferItemsEdit() {
     
             
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        View_StockTransferItems UIToEntity()
        {
        View_StockTransferItems entity = new View_StockTransferItems();
                     entity.StockTransferNo = txtStockTransferNo.Text ;
                       entity.Location_ID_from = Int64.Parse(txtLocation_ID_from.Text);
                        entity.Location_ID_to = Int64.Parse(txtLocation_ID_to.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.Transfer_date = DateTime.Parse(txtTransfer_date.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.ApprovalOpinions = txtApprovalOpinions.Text ;
                       entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                        entity.ApprovalResults = Boolean.Parse(txtApprovalResults.Text);
                        entity.SKU = txtSKU.Text ;
                       entity.CNName = txtCNName.Text ;
                       entity.Specifications = txtSpecifications.Text ;
                       entity.ProductNo = txtProductNo.Text ;
                       entity.Model = txtModel.Text ;
                       entity.Category_ID = Int64.Parse(txtCategory_ID.Text);
                        entity.Unit_ID = Int64.Parse(txtUnit_ID.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.property = txtproperty.Text ;
                       entity.Qty = Int32.Parse(txtQty.Text);
                        entity.TransPrice = Decimal.Parse(txtTransPrice.Text);
                        entity.Cost = Decimal.Parse(txtCost.Text);
                        entity.Summary = txtSummary.Text ;
                       entity.SubtotalTransferPirceAmount = Decimal.Parse(txtSubtotalTransferPirceAmount.Text);
                        entity.SubtotalCostAmount = Decimal.Parse(txtSubtotalCostAmount.Text);
                                return entity;
}
        */

        
        private View_StockTransferItems _EditEntity;
        public View_StockTransferItems EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(View_StockTransferItems entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<View_StockTransferItems>(entity, t => t.StockTransferNo, txtStockTransferNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_StockTransferItems>(entity, t => t.Location_ID_from, txtLocation_ID_from, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_StockTransferItems>(entity, t => t.Location_ID_to, txtLocation_ID_to, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_StockTransferItems>(entity, t => t.Employee_ID, txtEmployee_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<View_StockTransferItems>(entity, t => t.Transfer_date, dtpTransfer_date,false);
           DataBindingHelper.BindData4DataTime<View_StockTransferItems>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<View_StockTransferItems>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_StockTransferItems>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_StockTransferItems>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_StockTransferItems>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           //default  DataBindingHelper.BindData4TextBox<View_StockTransferItems>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<View_StockTransferItems>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4TextBox<View_StockTransferItems>(entity, t => t.SKU, txtSKU, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_StockTransferItems>(entity, t => t.CNName, txtCNName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_StockTransferItems>(entity, t => t.Specifications, txtSpecifications, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_StockTransferItems>(entity, t => t.ProductNo, txtProductNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_StockTransferItems>(entity, t => t.Model, txtModel, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_StockTransferItems>(entity, t => t.Category_ID, txtCategory_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_StockTransferItems>(entity, t => t.Unit_ID, txtUnit_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_StockTransferItems>(entity, t => t.ProdDetailID, txtProdDetailID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_StockTransferItems>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_StockTransferItems>(entity, t => t.Qty, txtQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_StockTransferItems>(entity, t => t.TransPrice.ToString(), txtTransPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_StockTransferItems>(entity, t => t.Cost.ToString(), txtCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_StockTransferItems>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_StockTransferItems>(entity, t => t.SubtotalTransferPirceAmount.ToString(), txtSubtotalTransferPirceAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_StockTransferItems>(entity, t => t.SubtotalCostAmount.ToString(), txtSubtotalCostAmount, BindDataType4TextBox.Money,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



