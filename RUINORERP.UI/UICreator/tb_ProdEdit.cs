
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/07/2024 19:06:28
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
    /// 货品基本信息表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_ProdEdit:UserControl
    {
     public tb_ProdEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_Prod UIToEntity()
        {
        tb_Prod entity = new tb_Prod();
                     entity.ProductNo = txtProductNo.Text ;
                       entity.CNName = txtCNName.Text ;
                       entity.ImagesPath = txtImagesPath.Text ;
                       entity.Images = Binary.Parse(txtImages.Text);
                        entity.ENName = txtENName.Text ;
                       entity.Model = txtModel.Text ;
                       entity.ShortCode = txtShortCode.Text ;
                       entity.Specifications = txtSpecifications.Text ;
                       entity.SourceType = Int32.Parse(txtSourceType.Text);
                        entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.PropertyType = Int32.Parse(txtPropertyType.Text);
                        entity.Unit_ID = Int64.Parse(txtUnit_ID.Text);
                        entity.Category_ID = Int64.Parse(txtCategory_ID.Text);
                        entity.Type_ID = Int64.Parse(txtType_ID.Text);
                        entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.Rack_ID = Int64.Parse(txtRack_ID.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.Brand = txtBrand.Text ;
                       entity.ProductENDesc = txtProductENDesc.Text ;
                       entity.ProductCNDesc = txtProductCNDesc.Text ;
                       entity.TaxRate = Decimal.Parse(txtTaxRate.Text);
                        entity.CustomsCode = txtCustomsCode.Text ;
                       entity.Tag = txtTag.Text ;
                       entity.SalePublish = Boolean.Parse(txtSalePublish.Text);
                        entity.Is_enabled = Boolean.Parse(txtIs_enabled.Text);
                        entity.Is_available = Boolean.Parse(txtIs_available.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                                return entity;
}
        */

        
        private tb_Prod _EditEntity;
        public tb_Prod EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_Prod entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.ProductNo, txtProductNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.CNName, txtCNName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.ImagesPath, txtImagesPath, BindDataType4TextBox.Text,false);
           //default  DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.Images.ToString(), txtImages, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.ENName, txtENName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.Model, txtModel, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.ShortCode, txtShortCode, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.Specifications, txtSpecifications, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.SourceType, txtSourceType, BindDataType4TextBox.Qty,false);
          // DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v=>v.XXNAME, cmbDepartmentID);
           DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.PropertyType, txtPropertyType, BindDataType4TextBox.Qty,false);
          // DataBindingHelper.BindData4Cmb<tb_Unit>(entity, k => k.Unit_ID, v=>v.XXNAME, cmbUnit_ID);
          // DataBindingHelper.BindData4Cmb<tb_ProdCategories>(entity, k => k.Category_ID, v=>v.XXNAME, cmbCategory_ID);
          // DataBindingHelper.BindData4Cmb<tb_ProductType>(entity, k => k.Type_ID, v=>v.XXNAME, cmbType_ID);
          // DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
          // DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
          // DataBindingHelper.BindData4Cmb<tb_StorageRack>(entity, k => k.Rack_ID, v=>v.XXNAME, cmbRack_ID);
          // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
           DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.Brand, txtBrand, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.ProductENDesc, txtProductENDesc, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.ProductCNDesc, txtProductCNDesc, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.TaxRate.ToString(), txtTaxRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.CustomsCode, txtCustomsCode, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.Tag, txtTag, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CehckBox<tb_Prod>(entity, t => t.SalePublish, chkSalePublish, false);
           DataBindingHelper.BindData4CehckBox<tb_Prod>(entity, t => t.Is_enabled, chkIs_enabled, false);
//有默认值
           DataBindingHelper.BindData4CehckBox<tb_Prod>(entity, t => t.Is_available, chkIs_available, false);
//有默认值
           DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_Prod>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_Prod>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CehckBox<tb_Prod>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4TextBox<tb_Prod>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



