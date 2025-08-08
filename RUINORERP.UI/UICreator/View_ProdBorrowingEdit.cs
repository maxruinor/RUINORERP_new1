
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:32
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
    /// 借出单统计数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class View_ProdBorrowingEdit:UserControl
    {
     public View_ProdBorrowingEdit() {
     
             
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        View_ProdBorrowing UIToEntity()
        {
        View_ProdBorrowing entity = new View_ProdBorrowing();
                     entity.BorrowID = Int64.Parse(txtBorrowID.Text);
                        entity.BorrowNo = txtBorrowNo.Text ;
                       entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.Out_date = DateTime.Parse(txtOut_date.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.DueDate = DateTime.Parse(txtDueDate.Text);
                        entity.Reason = txtReason.Text ;
                       entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.CloseCaseOpinions = txtCloseCaseOpinions.Text ;
                       entity.ApprovalOpinions = txtApprovalOpinions.Text ;
                       entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.SKU = txtSKU.Text ;
                       entity.Specifications = txtSpecifications.Text ;
                       entity.CNName = txtCNName.Text ;
                       entity.Model = txtModel.Text ;
                       entity.Type_ID = Int64.Parse(txtType_ID.Text);
                        entity.property = txtproperty.Text ;
                       entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.Qty = Int32.Parse(txtQty.Text);
                        entity.ReQty = Int32.Parse(txtReQty.Text);
                        entity.Price = Decimal.Parse(txtPrice.Text);
                        entity.SubtotalPirceAmount = Decimal.Parse(txtSubtotalPirceAmount.Text);
                        entity.Cost = Decimal.Parse(txtCost.Text);
                        entity.SubtotalCostAmount = Decimal.Parse(txtSubtotalCostAmount.Text);
                        entity.Summary = txtSummary.Text ;
                               return entity;
}
        */

        
        private View_ProdBorrowing _EditEntity;
        public View_ProdBorrowing EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(View_ProdBorrowing entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<View_ProdBorrowing>(entity, t => t.BorrowID, txtBorrowID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdBorrowing>(entity, t => t.BorrowNo, txtBorrowNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdBorrowing>(entity, t => t.CustomerVendor_ID, txtCustomerVendor_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdBorrowing>(entity, t => t.Employee_ID, txtEmployee_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<View_ProdBorrowing>(entity, t => t.Out_date, dtpOut_date,false);
           DataBindingHelper.BindData4TextBox<View_ProdBorrowing>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<View_ProdBorrowing>(entity, t => t.DueDate, dtpDueDate,false);
           DataBindingHelper.BindData4TextBox<View_ProdBorrowing>(entity, t => t.Reason, txtReason, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdBorrowing>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdBorrowing>(entity, t => t.CloseCaseOpinions, txtCloseCaseOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdBorrowing>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdBorrowing>(entity, t => t.ProdDetailID, txtProdDetailID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdBorrowing>(entity, t => t.SKU, txtSKU, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdBorrowing>(entity, t => t.Specifications, txtSpecifications, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdBorrowing>(entity, t => t.CNName, txtCNName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdBorrowing>(entity, t => t.Model, txtModel, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdBorrowing>(entity, t => t.Type_ID, txtType_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdBorrowing>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdBorrowing>(entity, t => t.Location_ID, txtLocation_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdBorrowing>(entity, t => t.Qty, txtQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdBorrowing>(entity, t => t.ReQty, txtReQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdBorrowing>(entity, t => t.Price.ToString(), txtPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_ProdBorrowing>(entity, t => t.SubtotalPirceAmount.ToString(), txtSubtotalPirceAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_ProdBorrowing>(entity, t => t.Cost.ToString(), txtCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_ProdBorrowing>(entity, t => t.SubtotalCostAmount.ToString(), txtSubtotalCostAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_ProdBorrowing>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



