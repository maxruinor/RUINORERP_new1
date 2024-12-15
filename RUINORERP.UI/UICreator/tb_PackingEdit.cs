
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/14/2024 16:49:14
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
    /// 包装规格表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_PackingEdit:UserControl
    {
     public tb_PackingEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_Packing UIToEntity()
        {
        tb_Packing entity = new tb_Packing();
                     entity.PackagingName = txtPackagingName.Text ;
                       entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.BundleID = Int64.Parse(txtBundleID.Text);
                        entity.ProdBaseID = Int64.Parse(txtProdBaseID.Text);
                        entity.Unit_ID = Int64.Parse(txtUnit_ID.Text);
                        entity.SKU = txtSKU.Text ;
                       entity.property = txtproperty.Text ;
                       entity.PackImage = Binary.Parse(txtPackImage.Text);
                        entity.BoxMaterial = txtBoxMaterial.Text ;
                       entity.Length = Decimal.Parse(txtLength.Text);
                        entity.Width = Decimal.Parse(txtWidth.Text);
                        entity.Height = Decimal.Parse(txtHeight.Text);
                        entity.Volume = Decimal.Parse(txtVolume.Text);
                        entity.NetWeight = Decimal.Parse(txtNetWeight.Text);
                        entity.GrossWeight = Decimal.Parse(txtGrossWeight.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.Is_enabled = Boolean.Parse(txtIs_enabled.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                                return entity;
}
        */

        
        private tb_Packing _EditEntity;
        public tb_Packing EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_Packing entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_Packing>(entity, t => t.PackagingName, txtPackagingName, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
          // DataBindingHelper.BindData4Cmb<tb_ProdBundle>(entity, k => k.BundleID, v=>v.XXNAME, cmbBundleID);
          // DataBindingHelper.BindData4Cmb<tb_Prod>(entity, k => k.ProdBaseID, v=>v.XXNAME, cmbProdBaseID);
          // DataBindingHelper.BindData4Cmb<tb_Unit>(entity, k => k.Unit_ID, v=>v.XXNAME, cmbUnit_ID);
           DataBindingHelper.BindData4TextBox<tb_Packing>(entity, t => t.SKU, txtSKU, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Packing>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           //default  DataBindingHelper.BindData4TextBox<tb_Packing>(entity, t => t.PackImage.ToString(), txtPackImage, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_Packing>(entity, t => t.BoxMaterial, txtBoxMaterial, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Packing>(entity, t => t.Length.ToString(), txtLength, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_Packing>(entity, t => t.Width.ToString(), txtWidth, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_Packing>(entity, t => t.Height.ToString(), txtHeight, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_Packing>(entity, t => t.Volume.ToString(), txtVolume, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_Packing>(entity, t => t.NetWeight.ToString(), txtNetWeight, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_Packing>(entity, t => t.GrossWeight.ToString(), txtGrossWeight, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_Packing>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_Packing>(entity, t => t.Is_enabled, chkIs_enabled, false);
//有默认值
           DataBindingHelper.BindData4CheckBox<tb_Packing>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4DataTime<tb_Packing>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_Packing>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_Packing>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_Packing>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



