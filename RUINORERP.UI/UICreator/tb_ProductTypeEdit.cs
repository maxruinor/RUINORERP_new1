
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/24/2025 17:35:20
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
    /// 货物类型  成品  半成品  包装材料 下脚料这种内容数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_ProductTypeEdit:UserControl
    {
     public tb_ProductTypeEdit() {
     
                         InitializeComponent();
      
        
        
        
        

         }
/*
        
        tb_ProductType UIToEntity()
        {
        tb_ProductType entity = new tb_ProductType();
                     entity.TypeName = txtTypeName.Text ;
                       entity.TypeDesc = txtTypeDesc.Text ;
                       entity.ForSale = Boolean.Parse(txtForSale.Text);
                                return entity;
}
        */

        
        private tb_ProductType _EditEntity;
        public tb_ProductType EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_ProductType entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_ProductType>(entity, t => t.TypeName, txtTypeName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProductType>(entity, t => t.TypeDesc, txtTypeDesc, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_ProductType>(entity, t => t.ForSale, chkForSale, false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



