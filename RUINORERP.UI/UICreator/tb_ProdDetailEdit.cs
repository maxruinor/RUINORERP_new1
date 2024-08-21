
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/07/2024 19:06:32
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
    /// 产品详细表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_ProdDetailEdit:UserControl
    {
     public tb_ProdDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_ProdDetail UIToEntity()
        {
        tb_ProdDetail entity = new tb_ProdDetail();
                     entity.ProdBaseID = Int64.Parse(txtProdBaseID.Text);
                        entity.BOM_ID = Int64.Parse(txtBOM_ID.Text);
                        entity.SKU = txtSKU.Text ;
                       entity.BarCode = txtBarCode.Text ;
                       entity.ImagesPath = txtImagesPath.Text ;
                       entity.Images = Binary.Parse(txtImages.Text);
                        entity.Weight = Decimal.Parse(txtWeight.Text);
                        entity.Standard_Price = Decimal.Parse(txtStandard_Price.Text);
                        entity.Transfer_Price = Decimal.Parse(txtTransfer_Price.Text);
                        entity.Wholesale_Price = Decimal.Parse(txtWholesale_Price.Text);
                        entity.Market_Price = Decimal.Parse(txtMarket_Price.Text);
                        entity.Discount_Price = Decimal.Parse(txtDiscount_Price.Text);
                        entity.Image = Binary.Parse(txtImage.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.SalePublish = Boolean.Parse(txtSalePublish.Text);
                        entity.Is_enabled = Boolean.Parse(txtIs_enabled.Text);
                        entity.Is_available = Boolean.Parse(txtIs_available.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                                return entity;
}
        */

        
        private tb_ProdDetail _EditEntity;
        public tb_ProdDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_ProdDetail entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_Prod>(entity, k => k.ProdBaseID, v=>v.XXNAME, cmbProdBaseID);
          // DataBindingHelper.BindData4Cmb<tb_BOM_S>(entity, k => k.BOM_ID, v=>v.XXNAME, cmbBOM_ID);
           DataBindingHelper.BindData4TextBox<tb_ProdDetail>(entity, t => t.SKU, txtSKU, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProdDetail>(entity, t => t.BarCode, txtBarCode, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProdDetail>(entity, t => t.ImagesPath, txtImagesPath, BindDataType4TextBox.Text,false);
           //default  DataBindingHelper.BindData4TextBox<tb_ProdDetail>(entity, t => t.Images.ToString(), txtImages, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_ProdDetail>(entity, t => t.Weight.ToString(), txtWeight, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_ProdDetail>(entity, t => t.Standard_Price.ToString(), txtStandard_Price, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_ProdDetail>(entity, t => t.Transfer_Price.ToString(), txtTransfer_Price, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_ProdDetail>(entity, t => t.Wholesale_Price.ToString(), txtWholesale_Price, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_ProdDetail>(entity, t => t.Market_Price.ToString(), txtMarket_Price, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_ProdDetail>(entity, t => t.Discount_Price.ToString(), txtDiscount_Price, BindDataType4TextBox.Money,false);
           //default  DataBindingHelper.BindData4TextBox<tb_ProdDetail>(entity, t => t.Image.ToString(), txtImage, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_ProdDetail>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CehckBox<tb_ProdDetail>(entity, t => t.SalePublish, chkSalePublish, false);
//有默认值
           DataBindingHelper.BindData4CehckBox<tb_ProdDetail>(entity, t => t.Is_enabled, chkIs_enabled, false);
//有默认值
           DataBindingHelper.BindData4CehckBox<tb_ProdDetail>(entity, t => t.Is_available, chkIs_available, false);
//有默认值
           DataBindingHelper.BindData4DataTime<tb_ProdDetail>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_ProdDetail>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_ProdDetail>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_ProdDetail>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CehckBox<tb_ProdDetail>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4TextBox<tb_ProdDetail>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



