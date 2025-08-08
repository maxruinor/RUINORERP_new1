
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:11
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
    /// 角色表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_RoleInfoEdit:UserControl
    {
     public tb_RoleInfoEdit() {
     
                         InitializeComponent();
      
        
        
        
        

         }
/*
        
        tb_RoleInfo UIToEntity()
        {
        tb_RoleInfo entity = new tb_RoleInfo();
                     entity.RoleName = txtRoleName.Text ;
                       entity.Desc = txtDesc.Text ;
                       entity.RolePropertyID = Int64.Parse(txtRolePropertyID.Text);
                                return entity;
}
        */

        
        private tb_RoleInfo _EditEntity;
        public tb_RoleInfo EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_RoleInfo entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_RoleInfo>(entity, t => t.RoleName, txtRoleName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_RoleInfo>(entity, t => t.Desc, txtDesc, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_RolePropertyConfig>(entity, k => k.RolePropertyID, v=>v.XXNAME, cmbRolePropertyID);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



