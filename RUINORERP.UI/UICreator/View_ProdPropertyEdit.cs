
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:34
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
    /// 数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class View_ProdPropertyEdit:UserControl
    {
     public View_ProdPropertyEdit() {
     
             
        
        
        
        
        

         }
/*
        
        View_ProdProperty UIToEntity()
        {
        View_ProdProperty entity = new View_ProdProperty();
                     entity.ProdBaseID = Int64.Parse(txtProdBaseID.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.Property_ID = Int64.Parse(txtProperty_ID.Text);
                        entity.PropertyName = txtPropertyName.Text ;
                       entity.PropertyValueID = Int64.Parse(txtPropertyValueID.Text);
                        entity.PropertyValueName = txtPropertyValueName.Text ;
                               return entity;
}
        */

        
        private View_ProdProperty _EditEntity;
        public View_ProdProperty EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(View_ProdProperty entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<View_ProdProperty>(entity, t => t.ProdBaseID, txtProdBaseID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdProperty>(entity, t => t.ProdDetailID, txtProdDetailID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdProperty>(entity, t => t.Property_ID, txtProperty_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdProperty>(entity, t => t.PropertyName, txtPropertyName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdProperty>(entity, t => t.PropertyValueID, txtPropertyValueID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdProperty>(entity, t => t.PropertyValueName, txtPropertyValueName, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



