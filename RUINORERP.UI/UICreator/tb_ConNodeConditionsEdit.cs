
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:16
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
    /// 流程步骤 为转移条件集合，Field为条件左参数，Operator为操作操作符如果值类型为String则表达式只能为==或者!=，Value为表达式值数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_ConNodeConditionsEdit:UserControl
    {
     public tb_ConNodeConditionsEdit() {
     
                         InitializeComponent();
      
        
        
        
        

         }
/*
        
        tb_ConNodeConditions UIToEntity()
        {
        tb_ConNodeConditions entity = new tb_ConNodeConditions();
                     entity.Field = txtField.Text ;
                       entity.Operator = txtOperator.Text ;
                       entity.Value = txtValue.Text ;
                               return entity;
}
        */

        
        private tb_ConNodeConditions _EditEntity;
        public tb_ConNodeConditions EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_ConNodeConditions entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_ConNodeConditions>(entity, t => t.Field, txtField, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ConNodeConditions>(entity, t => t.Operator, txtOperator, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ConNodeConditions>(entity, t => t.Value, txtValue, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



