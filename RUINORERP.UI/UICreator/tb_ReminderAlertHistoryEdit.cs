
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/12/2025 00:31:25
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
    /// 提醒信息是通过什么规则通知了什么内容给谁在什么时间。通知记录  暂时不处理数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_ReminderAlertHistoryEdit:UserControl
    {
     public tb_ReminderAlertHistoryEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        

         }
/*
        
        tb_ReminderAlertHistory UIToEntity()
        {
        tb_ReminderAlertHistory entity = new tb_ReminderAlertHistory();
                     entity.AlertId = Int64.Parse(txtAlertId.Text);
                        entity.User_ID = Int64.Parse(txtUser_ID.Text);
                        entity.IsRead = Boolean.Parse(txtIsRead.Text);
                        entity.Message = txtMessage.Text ;
                       entity.TriggerTime = DateTime.Parse(txtTriggerTime.Text);
                        entity.ReminderBizType = Int32.Parse(txtReminderBizType.Text);
                                return entity;
}
        */

        
        private tb_ReminderAlertHistory _EditEntity;
        public tb_ReminderAlertHistory EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_ReminderAlertHistory entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_ReminderAlert>(entity, k => k.AlertId, v=>v.XXNAME, cmbAlertId);
          // DataBindingHelper.BindData4Cmb<tb_UserInfo>(entity, k => k.User_ID, v=>v.XXNAME, cmbUser_ID);
           DataBindingHelper.BindData4CheckBox<tb_ReminderAlertHistory>(entity, t => t.IsRead, chkIsRead, false);
           DataBindingHelper.BindData4TextBox<tb_ReminderAlertHistory>(entity, t => t.Message, txtMessage, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_ReminderAlertHistory>(entity, t => t.TriggerTime, dtpTriggerTime,false);
           DataBindingHelper.BindData4TextBox<tb_ReminderAlertHistory>(entity, t => t.ReminderBizType, txtReminderBizType, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



