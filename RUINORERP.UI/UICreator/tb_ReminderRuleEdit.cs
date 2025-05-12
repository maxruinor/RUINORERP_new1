
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
    /// 库存策略通过这里设置的条件查询出一个库存集合提醒给用户数据编辑
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
                       entity.Description = txtDescription.Text ;
                       entity.ReminderBizType = Int32.Parse(txtReminderBizType.Text);
                        entity.Priority = Int32.Parse(txtPriority.Text);
                        entity.IsEnabled = Boolean.Parse(txtIsEnabled.Text);
                        entity.NotifyChannels = txtNotifyChannels.Text ;
                       entity.EffectiveDate = DateTime.Parse(txtEffectiveDate.Text);
                        entity.ExpireDate = DateTime.Parse(txtExpireDate.Text);
                        entity.Condition = txtCondition.Text ;
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
           DataBindingHelper.BindData4TextBox<tb_ReminderRule>(entity, t => t.Description, txtDescription, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ReminderRule>(entity, t => t.ReminderBizType, txtReminderBizType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ReminderRule>(entity, t => t.Priority, txtPriority, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_ReminderRule>(entity, t => t.IsEnabled, chkIsEnabled, false);
           DataBindingHelper.BindData4TextBox<tb_ReminderRule>(entity, t => t.NotifyChannels, txtNotifyChannels, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_ReminderRule>(entity, t => t.EffectiveDate, dtpEffectiveDate,false);
           DataBindingHelper.BindData4DataTime<tb_ReminderRule>(entity, t => t.ExpireDate, dtpExpireDate,false);
           DataBindingHelper.BindData4TextBox<tb_ReminderRule>(entity, t => t.Condition, txtCondition, BindDataType4TextBox.Text,false);
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



