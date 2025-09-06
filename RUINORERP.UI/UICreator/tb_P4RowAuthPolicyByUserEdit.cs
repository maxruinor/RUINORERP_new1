
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/06/2025 15:41:57
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
    /// 行级权限规则-用户关联表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_P4RowAuthPolicyByUserEdit:UserControl
    {
     public tb_P4RowAuthPolicyByUserEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_P4RowAuthPolicyByUser UIToEntity()
        {
        tb_P4RowAuthPolicyByUser entity = new tb_P4RowAuthPolicyByUser();
                     entity.PolicyId = Int64.Parse(txtPolicyId.Text);
                        entity.MenuID = Int64.Parse(txtMenuID.Text);
                        entity.User_ID = Int64.Parse(txtUser_ID.Text);
                        entity.IsEnabled = Boolean.Parse(txtIsEnabled.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                                return entity;
}
        */

        
        private tb_P4RowAuthPolicyByUser _EditEntity;
        public tb_P4RowAuthPolicyByUser EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_P4RowAuthPolicyByUser entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_RowAuthPolicy>(entity, k => k.PolicyId, v=>v.XXNAME, cmbPolicyId);
          // DataBindingHelper.BindData4Cmb<tb_MenuInfo>(entity, k => k.MenuID, v=>v.XXNAME, cmbMenuID);
          // DataBindingHelper.BindData4Cmb<tb_UserInfo>(entity, k => k.User_ID, v=>v.XXNAME, cmbUser_ID);
           DataBindingHelper.BindData4CheckBox<tb_P4RowAuthPolicyByUser>(entity, t => t.IsEnabled, chkIsEnabled, false);
//有默认值
           DataBindingHelper.BindData4DataTime<tb_P4RowAuthPolicyByUser>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_P4RowAuthPolicyByUser>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_P4RowAuthPolicyByUser>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_P4RowAuthPolicyByUser>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



