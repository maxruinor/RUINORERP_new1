
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
    /// 产品属性类型EVA数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_ProdPropertyTypeEdit:UserControl
    {
     public tb_ProdPropertyTypeEdit() {
     
                         InitializeComponent();
      
        
        
        

         }
/*
        
        tb_ProdPropertyType UIToEntity()
        {
        tb_ProdPropertyType entity = new tb_ProdPropertyType();
                     entity.PropertyTypeName = txtPropertyTypeName.Text ;
                       entity.PropertyTypeDesc = txtPropertyTypeDesc.Text ;
                               return entity;
}
        */

        
        private tb_ProdPropertyType _EditEntity;
        public tb_ProdPropertyType EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_ProdPropertyType entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_ProdPropertyType>(entity, t => t.PropertyTypeName, txtPropertyTypeName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProdPropertyType>(entity, t => t.PropertyTypeDesc, txtPropertyTypeDesc, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



