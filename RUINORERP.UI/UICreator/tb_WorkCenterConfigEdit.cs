
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:25
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
    /// 工作台配置表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_WorkCenterConfigEdit:UserControl
    {
     public tb_WorkCenterConfigEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        

         }
/*
        
        tb_WorkCenterConfig UIToEntity()
        {
        tb_WorkCenterConfig entity = new tb_WorkCenterConfig();
                     entity.RoleID = Int64.Parse(txtRoleID.Text);
                        entity.User_ID = Int64.Parse(txtUser_ID.Text);
                        entity.Operable = Boolean.Parse(txtOperable.Text);
                        entity.OnlyDisplay = Boolean.Parse(txtOnlyDisplay.Text);
                        entity.ToDoList = txtToDoList.Text ;
                       entity.FrequentlyMenus = txtFrequentlyMenus.Text ;
                       entity.DataOverview = txtDataOverview.Text ;
                               return entity;
}
        */

        
        private tb_WorkCenterConfig _EditEntity;
        public tb_WorkCenterConfig EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_WorkCenterConfig entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_WorkCenterConfig>(entity, t => t.RoleID, txtRoleID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_WorkCenterConfig>(entity, t => t.User_ID, txtUser_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_WorkCenterConfig>(entity, t => t.Operable, chkOperable, false);
           DataBindingHelper.BindData4CheckBox<tb_WorkCenterConfig>(entity, t => t.OnlyDisplay, chkOnlyDisplay, false);
           DataBindingHelper.BindData4TextBox<tb_WorkCenterConfig>(entity, t => t.ToDoList, txtToDoList, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_WorkCenterConfig>(entity, t => t.FrequentlyMenus, txtFrequentlyMenus, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_WorkCenterConfig>(entity, t => t.DataOverview, txtDataOverview, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



