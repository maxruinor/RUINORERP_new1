
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/29/2025 15:33:02
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
    /// 审计日志表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_AuditLogsEdit:UserControl
    {
     public tb_AuditLogsEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_AuditLogs UIToEntity()
        {
        tb_AuditLogs entity = new tb_AuditLogs();
                     entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.UserName = txtUserName.Text ;
                       entity.ActionTime = DateTime.Parse(txtActionTime.Text);
                        entity.ActionType = txtActionType.Text ;
                       entity.ObjectType = Int32.Parse(txtObjectType.Text);
                        entity.ObjectId = Int64.Parse(txtObjectId.Text);
                        entity.ObjectNo = txtObjectNo.Text ;
                       entity.OldState = txtOldState.Text ;
                       entity.NewState = txtNewState.Text ;
                       entity.Notes = txtNotes.Text ;
                       entity.DataContent = txtDataContent.Text ;
                               return entity;
}
        */

        
        private tb_AuditLogs _EditEntity;
        public tb_AuditLogs EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_AuditLogs entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
           DataBindingHelper.BindData4TextBox<tb_AuditLogs>(entity, t => t.UserName, txtUserName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_AuditLogs>(entity, t => t.ActionTime, dtpActionTime,false);
           DataBindingHelper.BindData4TextBox<tb_AuditLogs>(entity, t => t.ActionType, txtActionType, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_AuditLogs>(entity, t => t.ObjectType, txtObjectType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_AuditLogs>(entity, t => t.ObjectId, txtObjectId, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_AuditLogs>(entity, t => t.ObjectNo, txtObjectNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_AuditLogs>(entity, t => t.OldState, txtOldState, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_AuditLogs>(entity, t => t.NewState, txtNewState, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_AuditLogs>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_AuditLogs>(entity, t => t.DataContent, txtDataContent, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



