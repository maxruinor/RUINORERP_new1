
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 09:38:35
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
    /// 系统全局动态配置表 行转列数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_SysGlobalDynamicConfigEdit:UserControl
    {
     public tb_SysGlobalDynamicConfigEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_SysGlobalDynamicConfig UIToEntity()
        {
        tb_SysGlobalDynamicConfig entity = new tb_SysGlobalDynamicConfig();
                     entity.ConfigKey = txtConfigKey.Text ;
                       entity.ConfigValue = txtConfigValue.Text ;
                       entity.Description = Boolean.Parse(txtDescription.Text);
                        entity.ValueType = txtValueType.Text ;
                       entity.ConfigType = txtConfigType.Text ;
                       entity.IsActive = Boolean.Parse(txtIsActive.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                                return entity;
}
        */

        
        private tb_SysGlobalDynamicConfig _EditEntity;
        public tb_SysGlobalDynamicConfig EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_SysGlobalDynamicConfig entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_SysGlobalDynamicConfig>(entity, t => t.ConfigKey, txtConfigKey, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_SysGlobalDynamicConfig>(entity, t => t.ConfigValue, txtConfigValue, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CehckBox<tb_SysGlobalDynamicConfig>(entity, t => t.Description, chkDescription, false);
           DataBindingHelper.BindData4TextBox<tb_SysGlobalDynamicConfig>(entity, t => t.ValueType, txtValueType, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_SysGlobalDynamicConfig>(entity, t => t.ConfigType, txtConfigType, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CehckBox<tb_SysGlobalDynamicConfig>(entity, t => t.IsActive, chkIsActive, false);
//有默认值
           DataBindingHelper.BindData4DataTime<tb_SysGlobalDynamicConfig>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_SysGlobalDynamicConfig>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_SysGlobalDynamicConfig>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_SysGlobalDynamicConfig>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



