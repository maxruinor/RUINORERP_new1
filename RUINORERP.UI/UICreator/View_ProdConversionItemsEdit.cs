
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
    /// 转换明细统计数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class View_ProdConversionItemsEdit:UserControl
    {
     public View_ProdConversionItemsEdit() {
     
             
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        View_ProdConversionItems UIToEntity()
        {
        View_ProdConversionItems entity = new View_ProdConversionItems();
                     entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.ConversionNo = txtConversionNo.Text ;
                       entity.ConversionDate = DateTime.Parse(txtConversionDate.Text);
                        entity.Reason = txtReason.Text ;
                       entity.Notes = txtNotes.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.ApprovalOpinions = txtApprovalOpinions.Text ;
                       entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                        entity.ApprovalResults = Boolean.Parse(txtApprovalResults.Text);
                        entity.ProdDetailID_from = Int64.Parse(txtProdDetailID_from.Text);
                        entity.BarCode_from = txtBarCode_from.Text ;
                       entity.SKU_from = txtSKU_from.Text ;
                       entity.Type_ID_from = Int64.Parse(txtType_ID_from.Text);
                        entity.CNName_from = txtCNName_from.Text ;
                       entity.Model_from = txtModel_from.Text ;
                       entity.Specifications_from = txtSpecifications_from.Text ;
                       entity.property_from = txtproperty_from.Text ;
                       entity.ConversionQty = Int32.Parse(txtConversionQty.Text);
                        entity.ProdDetailID_to = Int64.Parse(txtProdDetailID_to.Text);
                        entity.BarCode_to = txtBarCode_to.Text ;
                       entity.SKU_to = txtSKU_to.Text ;
                       entity.Type_ID_to = Int64.Parse(txtType_ID_to.Text);
                        entity.CNName_to = txtCNName_to.Text ;
                       entity.Model_to = txtModel_to.Text ;
                       entity.Specifications_to = txtSpecifications_to.Text ;
                       entity.property_to = txtproperty_to.Text ;
                       entity.Summary = txtSummary.Text ;
                               return entity;
}
        */

        
        private View_ProdConversionItems _EditEntity;
        public View_ProdConversionItems EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(View_ProdConversionItems entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<View_ProdConversionItems>(entity, t => t.Employee_ID, txtEmployee_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdConversionItems>(entity, t => t.Location_ID, txtLocation_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdConversionItems>(entity, t => t.ConversionNo, txtConversionNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<View_ProdConversionItems>(entity, t => t.ConversionDate, dtpConversionDate,false);
           DataBindingHelper.BindData4TextBox<View_ProdConversionItems>(entity, t => t.Reason, txtReason, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdConversionItems>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<View_ProdConversionItems>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<View_ProdConversionItems>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdConversionItems>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdConversionItems>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           //default  DataBindingHelper.BindData4TextBox<View_ProdConversionItems>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<View_ProdConversionItems>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4TextBox<View_ProdConversionItems>(entity, t => t.ProdDetailID_from, txtProdDetailID_from, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdConversionItems>(entity, t => t.BarCode_from, txtBarCode_from, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdConversionItems>(entity, t => t.SKU_from, txtSKU_from, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdConversionItems>(entity, t => t.Type_ID_from, txtType_ID_from, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdConversionItems>(entity, t => t.CNName_from, txtCNName_from, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdConversionItems>(entity, t => t.Model_from, txtModel_from, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdConversionItems>(entity, t => t.Specifications_from, txtSpecifications_from, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdConversionItems>(entity, t => t.property_from, txtproperty_from, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdConversionItems>(entity, t => t.ConversionQty, txtConversionQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdConversionItems>(entity, t => t.ProdDetailID_to, txtProdDetailID_to, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdConversionItems>(entity, t => t.BarCode_to, txtBarCode_to, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdConversionItems>(entity, t => t.SKU_to, txtSKU_to, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdConversionItems>(entity, t => t.Type_ID_to, txtType_ID_to, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdConversionItems>(entity, t => t.CNName_to, txtCNName_to, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdConversionItems>(entity, t => t.Model_to, txtModel_to, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdConversionItems>(entity, t => t.Specifications_to, txtSpecifications_to, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdConversionItems>(entity, t => t.property_to, txtproperty_to, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdConversionItems>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



