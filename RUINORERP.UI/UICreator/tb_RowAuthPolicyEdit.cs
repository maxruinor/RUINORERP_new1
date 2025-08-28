
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/28/2025 15:02:31
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
    /// 行级权限规则数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_RowAuthPolicyEdit:UserControl
    {
     public tb_RowAuthPolicyEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_RowAuthPolicy UIToEntity()
        {
        tb_RowAuthPolicy entity = new tb_RowAuthPolicy();
                     entity.PolicyName = txtPolicyName.Text ;
                       entity.TargetTable = txtTargetTable.Text ;
                       entity.TargetEntity = txtTargetEntity.Text ;
                       entity.IsJoinRequired = Boolean.Parse(txtIsJoinRequired.Text);
                        entity.JoinTable = txtJoinTable.Text ;
                       entity.JoinType = txtJoinType.Text ;
                       entity.JoinOnClause = txtJoinOnClause.Text ;
                       entity.FilterClause = txtFilterClause.Text ;
                       entity.EntityType = txtEntityType.Text ;
                       entity.IsEnabled = Boolean.Parse(txtIsEnabled.Text);
                        entity.PolicyDescription = txtPolicyDescription.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                                return entity;
}
        */

        
        private tb_RowAuthPolicy _EditEntity;
        public tb_RowAuthPolicy EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_RowAuthPolicy entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_RowAuthPolicy>(entity, t => t.PolicyName, txtPolicyName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_RowAuthPolicy>(entity, t => t.TargetTable, txtTargetTable, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_RowAuthPolicy>(entity, t => t.TargetEntity, txtTargetEntity, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_RowAuthPolicy>(entity, t => t.IsJoinRequired, chkIsJoinRequired, false);
           DataBindingHelper.BindData4TextBox<tb_RowAuthPolicy>(entity, t => t.JoinTable, txtJoinTable, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_RowAuthPolicy>(entity, t => t.JoinType, txtJoinType, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_RowAuthPolicy>(entity, t => t.JoinOnClause, txtJoinOnClause, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_RowAuthPolicy>(entity, t => t.FilterClause, txtFilterClause, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_RowAuthPolicy>(entity, t => t.EntityType, txtEntityType, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_RowAuthPolicy>(entity, t => t.IsEnabled, chkIsEnabled, false);
//有默认值
           DataBindingHelper.BindData4TextBox<tb_RowAuthPolicy>(entity, t => t.PolicyDescription, txtPolicyDescription, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_RowAuthPolicy>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_RowAuthPolicy>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_RowAuthPolicy>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_RowAuthPolicy>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



