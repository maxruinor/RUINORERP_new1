
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/10/2025 15:31:57
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
    /// 用户表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_UserInfoEdit:UserControl
    {
     public tb_UserInfoEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_UserInfo UIToEntity()
        {
        tb_UserInfo entity = new tb_UserInfo();
                     entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.UserName = txtUserName.Text ;
                       entity.Password = txtPassword.Text ;
                       entity.is_enabled = Boolean.Parse(txtis_enabled.Text);
                        entity.is_available = Boolean.Parse(txtis_available.Text);
                        entity.IsSuperUser = Boolean.Parse(txtIsSuperUser.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.Lastlogin_at = DateTime.Parse(txtLastlogin_at.Text);
                        entity.Lastlogout_at = DateTime.Parse(txtLastlogout_at.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                                return entity;
}
        */

        
        private tb_UserInfo _EditEntity;
        public tb_UserInfo EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_UserInfo entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
           DataBindingHelper.BindData4TextBox<tb_UserInfo>(entity, t => t.UserName, txtUserName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_UserInfo>(entity, t => t.Password, txtPassword, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_UserInfo>(entity, t => t.is_enabled, chkis_enabled, false);
//有默认值
           DataBindingHelper.BindData4CheckBox<tb_UserInfo>(entity, t => t.is_available, chkis_available, false);
//有默认值
           DataBindingHelper.BindData4CheckBox<tb_UserInfo>(entity, t => t.IsSuperUser, chkIsSuperUser, false);
           DataBindingHelper.BindData4TextBox<tb_UserInfo>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_UserInfo>(entity, t => t.Lastlogin_at, dtpLastlogin_at,false);
           DataBindingHelper.BindData4DataTime<tb_UserInfo>(entity, t => t.Lastlogout_at, dtpLastlogout_at,false);
           DataBindingHelper.BindData4DataTime<tb_UserInfo>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_UserInfo>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_UserInfo>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_UserInfo>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



