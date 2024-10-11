
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/10/2024 14:15:53
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
    /// 产品转换单明细数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_ProdConversionDetailEdit:UserControl
    {
     public tb_ProdConversionDetailEdit() {
     
             
                    InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_ProdConversionDetail UIToEntity()
        {
        tb_ProdConversionDetail entity = new tb_ProdConversionDetail();
                     entity.ConversionID = Int64.Parse(txtConversionID.Text);
                        entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.ProdDetailID_from = Int64.Parse(txtProdDetailID_from.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.SKU_from = txtSKU_from.Text ;
                       entity.Type_ID_from = Int64.Parse(txtType_ID_from.Text);
                        entity.CNName_from = txtCNName_from.Text ;
                       entity.Model_from = txtModel_from.Text ;
                       entity.Specifications_from = txtSpecifications_from.Text ;
                       entity.property_from = txtproperty_from.Text ;
                       entity.ConversionQty = Int32.Parse(txtConversionQty.Text);
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

        
        private tb_ProdConversionDetail _EditEntity;
        public tb_ProdConversionDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_ProdConversionDetail entity)
        {
        _EditEntity = entity;
                       ProdDetailID_from主外字段不一致。// DataBindingHelper.BindData4Cmb<tb_ProdConversion>(entity, k => k.ConversionID, v=>v.XXNAME, cmbConversionID);
Type_ID_from主外字段不一致。Type_ID_to主外字段不一致。          ProdDetailID_from主外字段不一致。// DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
Type_ID_from主外字段不一致。Type_ID_to主外字段不一致。          ProdDetailID_from主外字段不一致。Type_ID_from主外字段不一致。Type_ID_to主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_ProdConversionDetail>(entity, t => t.ProdDetailID_from, txtProdDetailID_from, BindDataType4TextBox.Qty,false);
          ProdDetailID_from主外字段不一致。// DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
// DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
Type_ID_from主外字段不一致。Type_ID_to主外字段不一致。           DataBindingHelper.BindData4TextBox<tb_ProdConversionDetail>(entity, t => t.SKU_from, txtSKU_from, BindDataType4TextBox.Text,false);
          ProdDetailID_from主外字段不一致。Type_ID_from主外字段不一致。Type_ID_to主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_ProdConversionDetail>(entity, t => t.Type_ID_from, txtType_ID_from, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ProdConversionDetail>(entity, t => t.CNName_from, txtCNName_from, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProdConversionDetail>(entity, t => t.Model_from, txtModel_from, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProdConversionDetail>(entity, t => t.Specifications_from, txtSpecifications_from, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProdConversionDetail>(entity, t => t.property_from, txtproperty_from, BindDataType4TextBox.Text,false);
          ProdDetailID_from主外字段不一致。Type_ID_from主外字段不一致。Type_ID_to主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_ProdConversionDetail>(entity, t => t.ConversionQty, txtConversionQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ProdConversionDetail>(entity, t => t.SKU_to, txtSKU_to, BindDataType4TextBox.Text,false);
          ProdDetailID_from主外字段不一致。Type_ID_from主外字段不一致。Type_ID_to主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_ProdConversionDetail>(entity, t => t.Type_ID_to, txtType_ID_to, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ProdConversionDetail>(entity, t => t.CNName_to, txtCNName_to, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProdConversionDetail>(entity, t => t.Model_to, txtModel_to, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProdConversionDetail>(entity, t => t.Specifications_to, txtSpecifications_to, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProdConversionDetail>(entity, t => t.property_to, txtproperty_to, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProdConversionDetail>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



