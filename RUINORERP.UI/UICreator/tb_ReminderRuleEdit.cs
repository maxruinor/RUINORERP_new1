
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/04/2025 18:54:58
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
    /// 提醒规则数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_ReminderRuleEdit:UserControl
    {
     public tb_ReminderRuleEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_ReminderRule UIToEntity()
        {
        tb_ReminderRule entity = new tb_ReminderRule();
                     entity.RuleName = txtRuleName.Text ;
                       entity.RuleEngineType = Int32.Parse(txtRuleEngineType.Text);
                        entity.Description = txtDescription.Text ;
                       entity.ReminderBizType = Int32.Parse(txtReminderBizType.Text);
                        entity.CheckIntervalByMinutes = Int32.Parse(txtCheckIntervalByMinutes.Text);
                        entity.ReminderPriority = Int32.Parse(txtReminderPriority.Text);
                        entity.IsEnabled = Boolean.Parse(txtIsEnabled.Text);
                        entity.NotifyChannel = Int32.Parse(txtNotifyChannel.Text);
                        entity.EffectiveDate = DateTime.Parse(txtEffectiveDate.Text);
                        entity.ExpireDate = DateTime.Parse(txtExpireDate.Text);
                        entity.Condition = txtCondition.Text ;
                       entity.NotifyRecipientNames = txtNotifyRecipientNames.Text ;
                       entity.NotifyRecipients = txtNotifyRecipients.Text ;
                       entity.NotifyMessage = txtNotifyMessage.Text ;
                       entity.JsonConfig = txtJsonConfig.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                                return entity;
}
        */

        
        private tb_ReminderRule _EditEntity;
        public tb_ReminderRule EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_ReminderRule entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_ReminderRule>(entity, t => t.RuleName, txtRuleName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ReminderRule>(entity, t => t.RuleEngineType, txtRuleEngineType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ReminderRule>(entity, t => t.Description, txtDescription, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ReminderRule>(entity, t => t.ReminderBizType, txtReminderBizType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ReminderRule>(entity, t => t.CheckIntervalByMinutes, txtCheckIntervalByMinutes, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ReminderRule>(entity, t => t.ReminderPriority, txtReminderPriority, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_ReminderRule>(entity, t => t.IsEnabled, chkIsEnabled, false);
           DataBindingHelper.BindData4TextBox<tb_ReminderRule>(entity, t => t.NotifyChannel, txtNotifyChannel, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_ReminderRule>(entity, t => t.EffectiveDate, dtpEffectiveDate,false);
           DataBindingHelper.BindData4DataTime<tb_ReminderRule>(entity, t => t.ExpireDate, dtpExpireDate,false);
           DataBindingHelper.BindData4TextBox<tb_ReminderRule>(entity, t => t.Condition, txtCondition, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ReminderRule>(entity, t => t.NotifyRecipientNames, txtNotifyRecipientNames, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ReminderRule>(entity, t => t.NotifyRecipients, txtNotifyRecipients, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ReminderRule>(entity, t => t.NotifyMessage, txtNotifyMessage, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ReminderRule>(entity, t => t.JsonConfig, txtJsonConfig, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_ReminderRule>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_ReminderRule>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_ReminderRule>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_ReminderRule>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



