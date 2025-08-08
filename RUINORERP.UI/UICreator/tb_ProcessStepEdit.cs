
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:51
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
    /// 流程步骤数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_ProcessStepEdit:UserControl
    {
     public tb_ProcessStepEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_ProcessStep UIToEntity()
        {
        tb_ProcessStep entity = new tb_ProcessStep();
                     entity.StepBodyld = Int64.Parse(txtStepBodyld.Text);
                        entity.Position_Id = Int64.Parse(txtPosition_Id.Text);
                        entity.NextNode_ID = Int64.Parse(txtNextNode_ID.Text);
                        entity.Version = txtVersion.Text ;
                       entity.Name = txtName.Text ;
                       entity.DisplayName = txtDisplayName.Text ;
                       entity.StepNodeType = txtStepNodeType.Text ;
                       entity.Description = txtDescription.Text ;
                       entity.Notes = txtNotes.Text ;
                               return entity;
}
        */

        
        private tb_ProcessStep _EditEntity;
        public tb_ProcessStep EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_ProcessStep entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_StepBody>(entity, k => k.StepBodyld, v=>v.XXNAME, cmbStepBodyld);
          // DataBindingHelper.BindData4Cmb<tb_Position>(entity, k => k.Position_Id, v=>v.XXNAME, cmbPosition_Id);
          // DataBindingHelper.BindData4Cmb<tb_NextNodes>(entity, k => k.NextNode_ID, v=>v.XXNAME, cmbNextNode_ID);
           DataBindingHelper.BindData4TextBox<tb_ProcessStep>(entity, t => t.Version, txtVersion, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProcessStep>(entity, t => t.Name, txtName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProcessStep>(entity, t => t.DisplayName, txtDisplayName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProcessStep>(entity, t => t.StepNodeType, txtStepNodeType, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProcessStep>(entity, t => t.Description, txtDescription, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProcessStep>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



