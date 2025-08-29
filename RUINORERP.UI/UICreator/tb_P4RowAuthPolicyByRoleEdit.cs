
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/29/2025 20:39:06
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
    /// 行级权限规则-角色关联表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_P4RowAuthPolicyByRoleEdit:UserControl
    {
     public tb_P4RowAuthPolicyByRoleEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_P4RowAuthPolicyByRole UIToEntity()
        {
        tb_P4RowAuthPolicyByRole entity = new tb_P4RowAuthPolicyByRole();
                     entity.PolicyId = Int64.Parse(txtPolicyId.Text);
                        entity.RoleID = Int64.Parse(txtRoleID.Text);
                        entity.MenuID = Int64.Parse(txtMenuID.Text);
                        entity.IsEnabled = Boolean.Parse(txtIsEnabled.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                                return entity;
}
        */

        
        private tb_P4RowAuthPolicyByRole _EditEntity;
        public tb_P4RowAuthPolicyByRole EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_P4RowAuthPolicyByRole entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_RowAuthPolicy>(entity, k => k.PolicyId, v=>v.XXNAME, cmbPolicyId);
          // DataBindingHelper.BindData4Cmb<tb_RoleInfo>(entity, k => k.RoleID, v=>v.XXNAME, cmbRoleID);
          // DataBindingHelper.BindData4Cmb<tb_MenuInfo>(entity, k => k.MenuID, v=>v.XXNAME, cmbMenuID);
           DataBindingHelper.BindData4CheckBox<tb_P4RowAuthPolicyByRole>(entity, t => t.IsEnabled, chkIsEnabled, false);
//有默认值
           DataBindingHelper.BindData4DataTime<tb_P4RowAuthPolicyByRole>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_P4RowAuthPolicyByRole>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_P4RowAuthPolicyByRole>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_P4RowAuthPolicyByRole>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



