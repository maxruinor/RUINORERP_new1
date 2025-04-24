
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/24/2025 14:14:54
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
    /// 项目及成员关系表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_ProjectGroupEmployeesEdit:UserControl
    {
     public tb_ProjectGroupEmployeesEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        

         }
/*
        
        tb_ProjectGroupEmployees UIToEntity()
        {
        tb_ProjectGroupEmployees entity = new tb_ProjectGroupEmployees();
                     entity.ProjectGroup_ID = Int64.Parse(txtProjectGroup_ID.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.Assigned = Boolean.Parse(txtAssigned.Text);
                        entity.DefaultGroup = Boolean.Parse(txtDefaultGroup.Text);
                                return entity;
}
        */

        
        private tb_ProjectGroupEmployees _EditEntity;
        public tb_ProjectGroupEmployees EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_ProjectGroupEmployees entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v=>v.XXNAME, cmbProjectGroup_ID);
          // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
           DataBindingHelper.BindData4CheckBox<tb_ProjectGroupEmployees>(entity, t => t.Assigned, chkAssigned, false);
           DataBindingHelper.BindData4CheckBox<tb_ProjectGroupEmployees>(entity, t => t.DefaultGroup, chkDefaultGroup, false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



