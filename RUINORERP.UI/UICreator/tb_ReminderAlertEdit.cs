
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/12/2025 00:31:22
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
    /// 提醒内容数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_ReminderAlertEdit:UserControl
    {
     public tb_ReminderAlertEdit() {
     
                         InitializeComponent();
      
        
        
        
        

         }
/*
        
        tb_ReminderAlert UIToEntity()
        {
        tb_ReminderAlert entity = new tb_ReminderAlert();
                     entity.RuleId = Int64.Parse(txtRuleId.Text);
                        entity.AlertTime = DateTime.Parse(txtAlertTime.Text);
                        entity.Message = txtMessage.Text ;
                               return entity;
}
        */

        
        private tb_ReminderAlert _EditEntity;
        public tb_ReminderAlert EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_ReminderAlert entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_ReminderRule>(entity, k => k.RuleId, v=>v.XXNAME, cmbRuleId);
           DataBindingHelper.BindData4DataTime<tb_ReminderAlert>(entity, t => t.AlertTime, dtpAlertTime,false);
           DataBindingHelper.BindData4TextBox<tb_ReminderAlert>(entity, t => t.Message, txtMessage, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



