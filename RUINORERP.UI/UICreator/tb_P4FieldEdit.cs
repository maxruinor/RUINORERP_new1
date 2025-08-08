
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:47
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
    /// 字段权限表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_P4FieldEdit:UserControl
    {
     public tb_P4FieldEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_P4Field UIToEntity()
        {
        tb_P4Field entity = new tb_P4Field();
                     entity.FieldInfo_ID = Int64.Parse(txtFieldInfo_ID.Text);
                        entity.RoleID = Int64.Parse(txtRoleID.Text);
                        entity.MenuID = Int64.Parse(txtMenuID.Text);
                        entity.IsVisble = Boolean.Parse(txtIsVisble.Text);
                        entity.IsChild = Boolean.Parse(txtIsChild.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                                return entity;
}
        */

        
        private tb_P4Field _EditEntity;
        public tb_P4Field EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_P4Field entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_FieldInfo>(entity, k => k.FieldInfo_ID, v=>v.XXNAME, cmbFieldInfo_ID);
          // DataBindingHelper.BindData4Cmb<tb_RoleInfo>(entity, k => k.RoleID, v=>v.XXNAME, cmbRoleID);
          // DataBindingHelper.BindData4Cmb<tb_MenuInfo>(entity, k => k.MenuID, v=>v.XXNAME, cmbMenuID);
           DataBindingHelper.BindData4CheckBox<tb_P4Field>(entity, t => t.IsVisble, chkIsVisble, false);
           DataBindingHelper.BindData4CheckBox<tb_P4Field>(entity, t => t.IsChild, chkIsChild, false);
           DataBindingHelper.BindData4DataTime<tb_P4Field>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_P4Field>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_P4Field>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_P4Field>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



