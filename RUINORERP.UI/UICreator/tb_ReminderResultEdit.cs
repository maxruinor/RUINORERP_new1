
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
    /// 用户接收提醒内容数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_ReminderResultEdit:UserControl
    {
     public tb_ReminderResultEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        

         }
/*
        
        tb_ReminderResult UIToEntity()
        {
        tb_ReminderResult entity = new tb_ReminderResult();
                     entity.RuleId = Int64.Parse(txtRuleId.Text);
                        entity.ReminderBizType = Int32.Parse(txtReminderBizType.Text);
                        entity.TriggerTime = DateTime.Parse(txtTriggerTime.Text);
                        entity.Message = txtMessage.Text ;
                       entity.IsRead = Boolean.Parse(txtIsRead.Text);
                        entity.ReadTime = DateTime.Parse(txtReadTime.Text);
                        entity.JsonResult = txtJsonResult.Text ;
                               return entity;
}
        */

        
        private tb_ReminderResult _EditEntity;
        public tb_ReminderResult EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_ReminderResult entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_ReminderRule>(entity, k => k.RuleId, v=>v.XXNAME, cmbRuleId);
           DataBindingHelper.BindData4TextBox<tb_ReminderResult>(entity, t => t.ReminderBizType, txtReminderBizType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_ReminderResult>(entity, t => t.TriggerTime, dtpTriggerTime,false);
           DataBindingHelper.BindData4TextBox<tb_ReminderResult>(entity, t => t.Message, txtMessage, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_ReminderResult>(entity, t => t.IsRead, chkIsRead, false);
           DataBindingHelper.BindData4DataTime<tb_ReminderResult>(entity, t => t.ReadTime, dtpReadTime,false);
           DataBindingHelper.BindData4TextBox<tb_ReminderResult>(entity, t => t.JsonResult, txtJsonResult, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



