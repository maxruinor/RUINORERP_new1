
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 11:11:33
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
    /// 业务类型数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_BizTypeEdit:UserControl
    {
     public tb_BizTypeEdit() {
     
                         InitializeComponent();
      
        
        
        
        

         }
/*
        
        tb_BizType UIToEntity()
        {
        tb_BizType entity = new tb_BizType();
                     entity.TypeName = txtTypeName.Text ;
                       entity.TypeDesc = txtTypeDesc.Text ;
                       entity.Module = txtModule.Text ;
                               return entity;
}
        */

        
        private tb_BizType _EditEntity;
        public tb_BizType EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_BizType entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_BizType>(entity, t => t.TypeName, txtTypeName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_BizType>(entity, t => t.TypeDesc, txtTypeDesc, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_BizType>(entity, t => t.Module, txtModule, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



