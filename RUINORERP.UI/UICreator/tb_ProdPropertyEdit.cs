
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:57
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
    /// 产品属性表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_ProdPropertyEdit:UserControl
    {
     public tb_ProdPropertyEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_ProdProperty UIToEntity()
        {
        tb_ProdProperty entity = new tb_ProdProperty();
                     entity.PropertyName = txtPropertyName.Text ;
                       entity.PropertyDesc = txtPropertyDesc.Text ;
                       entity.SortOrder = Int32.Parse(txtSortOrder.Text);
                        entity.InputType = txtInputType.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                                return entity;
}
        */

        
        private tb_ProdProperty _EditEntity;
        public tb_ProdProperty EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_ProdProperty entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_ProdProperty>(entity, t => t.PropertyName, txtPropertyName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProdProperty>(entity, t => t.PropertyDesc, txtPropertyDesc, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProdProperty>(entity, t => t.SortOrder, txtSortOrder, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ProdProperty>(entity, t => t.InputType, txtInputType, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_ProdProperty>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_ProdProperty>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_ProdProperty>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_ProdProperty>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_ProdProperty>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4TextBox<tb_ProdProperty>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



