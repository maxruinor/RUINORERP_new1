
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:18
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
    /// 步骤变量数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_StepBodyParaEdit:UserControl
    {
     public tb_StepBodyParaEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        

         }
/*
        
        tb_StepBodyPara UIToEntity()
        {
        tb_StepBodyPara entity = new tb_StepBodyPara();
                     entity.Key = txtKey.Text ;
                       entity.Name = txtName.Text ;
                       entity.DisplayName = txtDisplayName.Text ;
                       entity.Value = txtValue.Text ;
                       entity.StepBodyParaType = txtStepBodyParaType.Text ;
                               return entity;
}
        */

        
        private tb_StepBodyPara _EditEntity;
        public tb_StepBodyPara EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_StepBodyPara entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_StepBodyPara>(entity, t => t.Key, txtKey, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_StepBodyPara>(entity, t => t.Name, txtName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_StepBodyPara>(entity, t => t.DisplayName, txtDisplayName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_StepBodyPara>(entity, t => t.Value, txtValue, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_StepBodyPara>(entity, t => t.StepBodyParaType, txtStepBodyParaType, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



