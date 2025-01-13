
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/13/2025 18:30:18
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
    /// UI查询条件设置数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_UIQueryConditionEdit:UserControl
    {
     public tb_UIQueryConditionEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_UIQueryCondition UIToEntity()
        {
        tb_UIQueryCondition entity = new tb_UIQueryCondition();
                     entity.UIMenuPID = Int64.Parse(txtUIMenuPID.Text);
                        entity.Caption = txtCaption.Text ;
                       entity.FieldName = txtFieldName.Text ;
                       entity.ValueType = txtValueType.Text ;
                       entity.Sort = Int32.Parse(txtSort.Text);
                        entity.IsVisble = Boolean.Parse(txtIsVisble.Text);
                        entity.Default1 = txtDefault1.Text ;
                       entity.Default2 = txtDefault2.Text ;
                       entity.EnableDefault1 = Boolean.Parse(txtEnableDefault1.Text);
                        entity.EnableDefault2 = Boolean.Parse(txtEnableDefault2.Text);
                        entity.Focused = Boolean.Parse(txtFocused.Text);
                        entity.DiffDays1 = Int32.Parse(txtDiffDays1.Text);
                        entity.DiffDays2 = Int32.Parse(txtDiffDays2.Text);
                                return entity;
}
        */

        
        private tb_UIQueryCondition _EditEntity;
        public tb_UIQueryCondition EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_UIQueryCondition entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_UIMenuPersonalization>(entity, k => k.UIMenuPID, v=>v.XXNAME, cmbUIMenuPID);
           DataBindingHelper.BindData4TextBox<tb_UIQueryCondition>(entity, t => t.Caption, txtCaption, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_UIQueryCondition>(entity, t => t.FieldName, txtFieldName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_UIQueryCondition>(entity, t => t.ValueType, txtValueType, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_UIQueryCondition>(entity, t => t.Sort, txtSort, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_UIQueryCondition>(entity, t => t.IsVisble, chkIsVisble, false);
           DataBindingHelper.BindData4TextBox<tb_UIQueryCondition>(entity, t => t.Default1, txtDefault1, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_UIQueryCondition>(entity, t => t.Default2, txtDefault2, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_UIQueryCondition>(entity, t => t.EnableDefault1, chkEnableDefault1, false);
           DataBindingHelper.BindData4CheckBox<tb_UIQueryCondition>(entity, t => t.EnableDefault2, chkEnableDefault2, false);
           DataBindingHelper.BindData4CheckBox<tb_UIQueryCondition>(entity, t => t.Focused, chkFocused, false);
           DataBindingHelper.BindData4TextBox<tb_UIQueryCondition>(entity, t => t.DiffDays1, txtDiffDays1, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_UIQueryCondition>(entity, t => t.DiffDays2, txtDiffDays2, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



