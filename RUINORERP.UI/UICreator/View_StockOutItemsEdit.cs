
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:39
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
    /// 其它出库统计数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class View_StockOutItemsEdit:UserControl
    {
     public View_StockOutItemsEdit() {
     
             
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        View_StockOutItems UIToEntity()
        {
        View_StockOutItems entity = new View_StockOutItems();
                     entity.Type_ID = Int64.Parse(txtType_ID.Text);
                        entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.BillNo = txtBillNo.Text ;
                       entity.Bill_Date = DateTime.Parse(txtBill_Date.Text);
                        entity.Out_date = DateTime.Parse(txtOut_date.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.ApprovalResults = Boolean.Parse(txtApprovalResults.Text);
                        entity.RefNO = txtRefNO.Text ;
                       entity.RefBizType = Int32.Parse(txtRefBizType.Text);
                        entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.Rack_ID = Int64.Parse(txtRack_ID.Text);
                        entity.Qty = Int32.Parse(txtQty.Text);
                        entity.Price = Decimal.Parse(txtPrice.Text);
                        entity.Cost = Decimal.Parse(txtCost.Text);
                        entity.Summary = txtSummary.Text ;
                       entity.SubtotalCostAmount = Decimal.Parse(txtSubtotalCostAmount.Text);
                        entity.SubtotalPirceAmount = Decimal.Parse(txtSubtotalPirceAmount.Text);
                        entity.property = txtproperty.Text ;
                       entity.SKU = txtSKU.Text ;
                       entity.CNName = txtCNName.Text ;
                       entity.Specifications = txtSpecifications.Text ;
                       entity.ProductNo = txtProductNo.Text ;
                       entity.Model = txtModel.Text ;
                       entity.Category_ID = Int64.Parse(txtCategory_ID.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.ApprovalOpinions = txtApprovalOpinions.Text ;
                       entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                        entity.RefBillID = Int64.Parse(txtRefBillID.Text);
                        entity.Unit_ID = Int64.Parse(txtUnit_ID.Text);
                                return entity;
}
        */

        
        private View_StockOutItems _EditEntity;
        public View_StockOutItems EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(View_StockOutItems entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<View_StockOutItems>(entity, t => t.Type_ID, txtType_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_StockOutItems>(entity, t => t.CustomerVendor_ID, txtCustomerVendor_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_StockOutItems>(entity, t => t.Employee_ID, txtEmployee_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_StockOutItems>(entity, t => t.BillNo, txtBillNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<View_StockOutItems>(entity, t => t.Bill_Date, dtpBill_Date,false);
           DataBindingHelper.BindData4DataTime<View_StockOutItems>(entity, t => t.Out_date, dtpOut_date,false);
           DataBindingHelper.BindData4DataTime<View_StockOutItems>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<View_StockOutItems>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_StockOutItems>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<View_StockOutItems>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4TextBox<View_StockOutItems>(entity, t => t.RefNO, txtRefNO, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_StockOutItems>(entity, t => t.RefBizType, txtRefBizType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_StockOutItems>(entity, t => t.Location_ID, txtLocation_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_StockOutItems>(entity, t => t.ProdDetailID, txtProdDetailID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_StockOutItems>(entity, t => t.Rack_ID, txtRack_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_StockOutItems>(entity, t => t.Qty, txtQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_StockOutItems>(entity, t => t.Price.ToString(), txtPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_StockOutItems>(entity, t => t.Cost.ToString(), txtCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_StockOutItems>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_StockOutItems>(entity, t => t.SubtotalCostAmount.ToString(), txtSubtotalCostAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_StockOutItems>(entity, t => t.SubtotalPirceAmount.ToString(), txtSubtotalPirceAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_StockOutItems>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_StockOutItems>(entity, t => t.SKU, txtSKU, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_StockOutItems>(entity, t => t.CNName, txtCNName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_StockOutItems>(entity, t => t.Specifications, txtSpecifications, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_StockOutItems>(entity, t => t.ProductNo, txtProductNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_StockOutItems>(entity, t => t.Model, txtModel, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_StockOutItems>(entity, t => t.Category_ID, txtCategory_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_StockOutItems>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_StockOutItems>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           //default  DataBindingHelper.BindData4TextBox<View_StockOutItems>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_StockOutItems>(entity, t => t.RefBillID, txtRefBillID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_StockOutItems>(entity, t => t.Unit_ID, txtUnit_ID, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



