
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:43
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
    /// 功能模块定义（仅限部分已经硬码并体现于菜单表中）数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_ModuleDefinitionEdit:UserControl
    {
     public tb_ModuleDefinitionEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        

         }
/*
        
        tb_ModuleDefinition UIToEntity()
        {
        tb_ModuleDefinition entity = new tb_ModuleDefinition();
                     entity.ModuleNo = txtModuleNo.Text ;
                       entity.ModuleName = txtModuleName.Text ;
                       entity.Visible = Boolean.Parse(txtVisible.Text);
                        entity.Available = Boolean.Parse(txtAvailable.Text);
                        entity.IconFile_Path = txtIconFile_Path.Text ;
                               return entity;
}
        */

        
        private tb_ModuleDefinition _EditEntity;
        public tb_ModuleDefinition EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_ModuleDefinition entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_ModuleDefinition>(entity, t => t.ModuleNo, txtModuleNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ModuleDefinition>(entity, t => t.ModuleName, txtModuleName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_ModuleDefinition>(entity, t => t.Visible, chkVisible, false);
           DataBindingHelper.BindData4CheckBox<tb_ModuleDefinition>(entity, t => t.Available, chkAvailable, false);
           DataBindingHelper.BindData4TextBox<tb_ModuleDefinition>(entity, t => t.IconFile_Path, txtIconFile_Path, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



