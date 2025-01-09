
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/07/2025 21:48:21
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
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



