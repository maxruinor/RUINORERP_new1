
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:54
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
    /// 产品类别表 与行业相关的产品分类数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_ProdCategoriesEdit:UserControl
    {
     public tb_ProdCategoriesEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_ProdCategories UIToEntity()
        {
        tb_ProdCategories entity = new tb_ProdCategories();
                     entity.Category_name = txtCategory_name.Text ;
                       entity.CategoryCode = txtCategoryCode.Text ;
                       entity.Is_enabled = Boolean.Parse(txtIs_enabled.Text);
                        entity.CategoryLevel = Int32.Parse(txtCategoryLevel.Text);
                        entity.Sort = Int32.Parse(txtSort.Text);
                        entity.Parent_id = Int64.Parse(txtParent_id.Text);
                        entity.Images = Binary.Parse(txtImages.Text);
                        entity.Notes = txtNotes.Text ;
                               return entity;
}
        */

        
        private tb_ProdCategories _EditEntity;
        public tb_ProdCategories EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_ProdCategories entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_ProdCategories>(entity, t => t.Category_name, txtCategory_name, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProdCategories>(entity, t => t.CategoryCode, txtCategoryCode, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_ProdCategories>(entity, t => t.Is_enabled, chkIs_enabled, false);
//有默认值
          Parent_id主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_ProdCategories>(entity, t => t.CategoryLevel, txtCategoryLevel, BindDataType4TextBox.Qty,false);
          Parent_id主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_ProdCategories>(entity, t => t.Sort, txtSort, BindDataType4TextBox.Qty,false);
          Parent_id主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_ProdCategories>(entity, t => t.Parent_id, txtParent_id, BindDataType4TextBox.Qty,false);
           //default  DataBindingHelper.BindData4TextBox<tb_ProdCategories>(entity, t => t.Images.ToString(), txtImages, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_ProdCategories>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



