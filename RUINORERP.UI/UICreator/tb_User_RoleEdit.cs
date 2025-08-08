
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:25
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
    /// 用户角色关系表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_User_RoleEdit:UserControl
    {
     public tb_User_RoleEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        

         }
/*
        
        tb_User_Role UIToEntity()
        {
        tb_User_Role entity = new tb_User_Role();
                     entity.User_ID = Int64.Parse(txtUser_ID.Text);
                        entity.RoleID = Int64.Parse(txtRoleID.Text);
                        entity.Authorized = Boolean.Parse(txtAuthorized.Text);
                        entity.DefaultRole = Boolean.Parse(txtDefaultRole.Text);
                                return entity;
}
        */

        
        private tb_User_Role _EditEntity;
        public tb_User_Role EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_User_Role entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_UserInfo>(entity, k => k.User_ID, v=>v.XXNAME, cmbUser_ID);
          // DataBindingHelper.BindData4Cmb<tb_RoleInfo>(entity, k => k.RoleID, v=>v.XXNAME, cmbRoleID);
           DataBindingHelper.BindData4CheckBox<tb_User_Role>(entity, t => t.Authorized, chkAuthorized, false);
           DataBindingHelper.BindData4CheckBox<tb_User_Role>(entity, t => t.DefaultRole, chkDefaultRole, false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



