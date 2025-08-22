
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/20/2025 16:08:16
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
    /// 项目组信息 用于业务分组小团队数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_ProjectGroupEdit:UserControl
    {
     public tb_ProjectGroupEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_ProjectGroup UIToEntity()
        {
        tb_ProjectGroup entity = new tb_ProjectGroup();
                     entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.ProjectGroupCode = txtProjectGroupCode.Text ;
                       entity.ProjectGroupName = txtProjectGroupName.Text ;
                       entity.ResponsiblePerson = txtResponsiblePerson.Text ;
                       entity.Phone = txtPhone.Text ;
                       entity.Notes = txtNotes.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.StartDate = DateTime.Parse(txtStartDate.Text);
                        entity.Is_enabled = Boolean.Parse(txtIs_enabled.Text);
                        entity.EndDate = DateTime.Parse(txtEndDate.Text);
                                return entity;
}
        */

        
        private tb_ProjectGroup _EditEntity;
        public tb_ProjectGroup EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_ProjectGroup entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v=>v.XXNAME, cmbDepartmentID);
           DataBindingHelper.BindData4TextBox<tb_ProjectGroup>(entity, t => t.ProjectGroupCode, txtProjectGroupCode, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProjectGroup>(entity, t => t.ProjectGroupName, txtProjectGroupName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProjectGroup>(entity, t => t.ResponsiblePerson, txtResponsiblePerson, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProjectGroup>(entity, t => t.Phone, txtPhone, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProjectGroup>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_ProjectGroup>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_ProjectGroup>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_ProjectGroup>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_ProjectGroup>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_ProjectGroup>(entity, t => t.StartDate, dtpStartDate,false);
           DataBindingHelper.BindData4CheckBox<tb_ProjectGroup>(entity, t => t.Is_enabled, chkIs_enabled, false);
//有默认值
           DataBindingHelper.BindData4DataTime<tb_ProjectGroup>(entity, t => t.EndDate, dtpEndDate,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



