
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:22
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
    /// 业务编号规则数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_sys_BillNoRuleEdit:UserControl
    {
     public tb_sys_BillNoRuleEdit() {
     
                         InitializeComponent();
      
        
        
                    InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_sys_BillNoRule UIToEntity()
        {
        tb_sys_BillNoRule entity = new tb_sys_BillNoRule();
                     entity.RuleName = txtRuleName.Text ;
                       entity.Prefix = txtPrefix.Text ;
                       entity.DateFormat = Int32.Parse(txtDateFormat.Text);
                        entity.SequenceLength = Int32.Parse(txtSequenceLength.Text);
                        entity.UseCheckDigit = Boolean.Parse(txtUseCheckDigit.Text);
                        entity.RedisKeyPattern = txtRedisKeyPattern.Text ;
                       entity.ResetMode = Int32.Parse(txtResetMode.Text);
                        entity.IsActive = Boolean.Parse(txtIsActive.Text);
                        entity.Description = txtDescription.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                                return entity;
}
        */

        
        private tb_sys_BillNoRule _EditEntity;
        public tb_sys_BillNoRule EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_sys_BillNoRule entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_sys_BillNoRule>(entity, t => t.RuleName, txtRuleName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_sys_BillNoRule>(entity, t => t.Prefix, txtPrefix, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_sys_BillNoRule>(entity, t => t.DateFormat, txtDateFormat, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_sys_BillNoRule>(entity, t => t.SequenceLength, txtSequenceLength, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_sys_BillNoRule>(entity, t => t.UseCheckDigit, chkUseCheckDigit, false);
           DataBindingHelper.BindData4TextBox<tb_sys_BillNoRule>(entity, t => t.RedisKeyPattern, txtRedisKeyPattern, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_sys_BillNoRule>(entity, t => t.ResetMode, txtResetMode, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_sys_BillNoRule>(entity, t => t.IsActive, chkIsActive, false);
//有默认值
           DataBindingHelper.BindData4TextBox<tb_sys_BillNoRule>(entity, t => t.Description, txtDescription, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_sys_BillNoRule>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_sys_BillNoRule>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_sys_BillNoRule>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_sys_BillNoRule>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



