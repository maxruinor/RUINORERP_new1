
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/29/2025 18:37:26
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
    /// 字段信息表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_FieldInfoEdit:UserControl
    {
     public tb_FieldInfoEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_FieldInfo UIToEntity()
        {
        tb_FieldInfo entity = new tb_FieldInfo();
                     entity.MenuID = Int64.Parse(txtMenuID.Text);
                        entity.EntityName = txtEntityName.Text ;
                       entity.FieldName = txtFieldName.Text ;
                       entity.FieldText = txtFieldText.Text ;
                       entity.ClassPath = txtClassPath.Text ;
                       entity.IsForm = Boolean.Parse(txtIsForm.Text);
                        entity.DefaultHide = Boolean.Parse(txtDefaultHide.Text);
                        entity.ReadOnly = Boolean.Parse(txtReadOnly.Text);
                        entity.IsEnabled = Boolean.Parse(txtIsEnabled.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.IsChild = Boolean.Parse(txtIsChild.Text);
                        entity.ChildEntityName = txtChildEntityName.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                                return entity;
}
        */

        
        private tb_FieldInfo _EditEntity;
        public tb_FieldInfo EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_FieldInfo entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_MenuInfo>(entity, k => k.MenuID, v=>v.XXNAME, cmbMenuID);
           DataBindingHelper.BindData4TextBox<tb_FieldInfo>(entity, t => t.EntityName, txtEntityName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FieldInfo>(entity, t => t.FieldName, txtFieldName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FieldInfo>(entity, t => t.FieldText, txtFieldText, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FieldInfo>(entity, t => t.ClassPath, txtClassPath, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_FieldInfo>(entity, t => t.IsForm, chkIsForm, false);
           DataBindingHelper.BindData4CheckBox<tb_FieldInfo>(entity, t => t.DefaultHide, chkDefaultHide, false);
           DataBindingHelper.BindData4CheckBox<tb_FieldInfo>(entity, t => t.ReadOnly, chkReadOnly, false);
           DataBindingHelper.BindData4CheckBox<tb_FieldInfo>(entity, t => t.IsEnabled, chkIsEnabled, false);
//有默认值
           DataBindingHelper.BindData4TextBox<tb_FieldInfo>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_FieldInfo>(entity, t => t.IsChild, chkIsChild, false);
           DataBindingHelper.BindData4TextBox<tb_FieldInfo>(entity, t => t.ChildEntityName, txtChildEntityName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_FieldInfo>(entity, t => t.Created_at, dtpCreated_at,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



