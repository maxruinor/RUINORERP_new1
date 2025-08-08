
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:17
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
    /// 步骤定义数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_StepBodyEdit:UserControl
    {
     public tb_StepBodyEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        

         }
/*
        
        tb_StepBody UIToEntity()
        {
        tb_StepBody entity = new tb_StepBody();
                     entity.Para_Id = Int64.Parse(txtPara_Id.Text);
                        entity.Name = txtName.Text ;
                       entity.DisplayName = txtDisplayName.Text ;
                       entity.TypeFullName = txtTypeFullName.Text ;
                       entity.AssemblyFullName = txtAssemblyFullName.Text ;
                               return entity;
}
        */

        
        private tb_StepBody _EditEntity;
        public tb_StepBody EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_StepBody entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_StepBodyPara>(entity, k => k.Para_Id, v=>v.XXNAME, cmbPara_Id);
           DataBindingHelper.BindData4TextBox<tb_StepBody>(entity, t => t.Name, txtName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_StepBody>(entity, t => t.DisplayName, txtDisplayName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_StepBody>(entity, t => t.TypeFullName, txtTypeFullName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_StepBody>(entity, t => t.AssemblyFullName, txtAssemblyFullName, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



