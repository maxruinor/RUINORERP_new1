
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:14
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
    /// 字段信息表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_ButtonInfoEdit:UserControl
    {
     public tb_ButtonInfoEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_ButtonInfo UIToEntity()
        {
        tb_ButtonInfo entity = new tb_ButtonInfo();
                     entity.MenuID = Int64.Parse(txtMenuID.Text);
                        entity.BtnName = txtBtnName.Text ;
                       entity.BtnText = txtBtnText.Text ;
                       entity.HotKey = txtHotKey.Text ;
                       entity.ButtonType = txtButtonType.Text ;
                       entity.FormName = txtFormName.Text ;
                       entity.ClassPath = txtClassPath.Text ;
                       entity.IsForm = Boolean.Parse(txtIsForm.Text);
                        entity.IsEnabled = Boolean.Parse(txtIsEnabled.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                                return entity;
}
        */

        
        private tb_ButtonInfo _EditEntity;
        public tb_ButtonInfo EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_ButtonInfo entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_MenuInfo>(entity, k => k.MenuID, v=>v.XXNAME, cmbMenuID);
           DataBindingHelper.BindData4TextBox<tb_ButtonInfo>(entity, t => t.BtnName, txtBtnName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ButtonInfo>(entity, t => t.BtnText, txtBtnText, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ButtonInfo>(entity, t => t.HotKey, txtHotKey, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ButtonInfo>(entity, t => t.ButtonType, txtButtonType, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ButtonInfo>(entity, t => t.FormName, txtFormName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ButtonInfo>(entity, t => t.ClassPath, txtClassPath, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_ButtonInfo>(entity, t => t.IsForm, chkIsForm, false);
           DataBindingHelper.BindData4CheckBox<tb_ButtonInfo>(entity, t => t.IsEnabled, chkIsEnabled, false);
//有默认值
           DataBindingHelper.BindData4TextBox<tb_ButtonInfo>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_ButtonInfo>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_ButtonInfo>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_ButtonInfo>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_ButtonInfo>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



