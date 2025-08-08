
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:51
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
    /// 流程定义 http://www.phpheidong.com/blog/article/68471/a3129f742e5e396e3d1e/数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_ProcessDefinitionEdit:UserControl
    {
     public tb_ProcessDefinitionEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        

         }
/*
        
        tb_ProcessDefinition UIToEntity()
        {
        tb_ProcessDefinition entity = new tb_ProcessDefinition();
                     entity.Step_Id = Int64.Parse(txtStep_Id.Text);
                        entity.Version = txtVersion.Text ;
                       entity.Title = txtTitle.Text ;
                       entity.Color = txtColor.Text ;
                       entity.Icon = txtIcon.Text ;
                       entity.Description = txtDescription.Text ;
                       entity.Notes = txtNotes.Text ;
                               return entity;
}
        */

        
        private tb_ProcessDefinition _EditEntity;
        public tb_ProcessDefinition EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_ProcessDefinition entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_ProcessStep>(entity, k => k.Step_Id, v=>v.XXNAME, cmbStep_Id);
           DataBindingHelper.BindData4TextBox<tb_ProcessDefinition>(entity, t => t.Version, txtVersion, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProcessDefinition>(entity, t => t.Title, txtTitle, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProcessDefinition>(entity, t => t.Color, txtColor, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProcessDefinition>(entity, t => t.Icon, txtIcon, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProcessDefinition>(entity, t => t.Description, txtDescription, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProcessDefinition>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



