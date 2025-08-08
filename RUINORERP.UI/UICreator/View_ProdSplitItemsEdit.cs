
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
    /// 分割明细统计-只管明细产品的数据主表的用查询数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class View_ProdSplitItemsEdit:UserControl
    {
     public View_ProdSplitItemsEdit() {
     
             
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        View_ProdSplitItems UIToEntity()
        {
        View_ProdSplitItems entity = new View_ProdSplitItems();
                     entity.CNName = txtCNName.Text ;
                       entity.Specifications = txtSpecifications.Text ;
                       entity.ProductNo = txtProductNo.Text ;
                       entity.Model = txtModel.Text ;
                       entity.Category_ID = Int64.Parse(txtCategory_ID.Text);
                        entity.Unit_ID = Int64.Parse(txtUnit_ID.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.SplitNo = txtSplitNo.Text ;
                       entity.SplitDate = DateTime.Parse(txtSplitDate.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.ApprovalResults = Boolean.Parse(txtApprovalResults.Text);
                        entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Qty = Int32.Parse(txtQty.Text);
                        entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.property = txtproperty.Text ;
                       entity.Summary = txtSummary.Text ;
                               return entity;
}
        */

        
        private View_ProdSplitItems _EditEntity;
        public View_ProdSplitItems EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(View_ProdSplitItems entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<View_ProdSplitItems>(entity, t => t.CNName, txtCNName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdSplitItems>(entity, t => t.Specifications, txtSpecifications, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdSplitItems>(entity, t => t.ProductNo, txtProductNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdSplitItems>(entity, t => t.Model, txtModel, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdSplitItems>(entity, t => t.Category_ID, txtCategory_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdSplitItems>(entity, t => t.Unit_ID, txtUnit_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdSplitItems>(entity, t => t.Employee_ID, txtEmployee_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdSplitItems>(entity, t => t.SplitNo, txtSplitNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<View_ProdSplitItems>(entity, t => t.SplitDate, dtpSplitDate,false);
           DataBindingHelper.BindData4TextBox<View_ProdSplitItems>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<View_ProdSplitItems>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           //default  DataBindingHelper.BindData4TextBox<View_ProdSplitItems>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4DataTime<View_ProdSplitItems>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<View_ProdSplitItems>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdSplitItems>(entity, t => t.Qty, txtQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdSplitItems>(entity, t => t.Location_ID, txtLocation_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdSplitItems>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdSplitItems>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



