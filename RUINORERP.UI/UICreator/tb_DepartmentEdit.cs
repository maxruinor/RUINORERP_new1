
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:21
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
    /// 部门表是否分层数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_DepartmentEdit:UserControl
    {
     public tb_DepartmentEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_Department UIToEntity()
        {
        tb_Department entity = new tb_Department();
                     entity.ID = Int64.Parse(txtID.Text);
                        entity.DepartmentCode = txtDepartmentCode.Text ;
                       entity.DepartmentName = txtDepartmentName.Text ;
                       entity.TEL = txtTEL.Text ;
                       entity.Notes = txtNotes.Text ;
                       entity.Director = txtDirector.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                                return entity;
}
        */

        
        private tb_Department _EditEntity;
        public tb_Department EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_Department entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_Company>(entity, k => k.ID, v=>v.XXNAME, cmbID);
           DataBindingHelper.BindData4TextBox<tb_Department>(entity, t => t.DepartmentCode, txtDepartmentCode, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Department>(entity, t => t.DepartmentName, txtDepartmentName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Department>(entity, t => t.TEL, txtTEL, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Department>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Department>(entity, t => t.Director, txtDirector, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_Department>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_Department>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_Department>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_Department>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_Department>(entity, t => t.isdeleted, chkisdeleted, false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



