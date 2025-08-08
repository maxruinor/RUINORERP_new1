
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:50
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
    /// 位置信息数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_PositionEdit:UserControl
    {
     public tb_PositionEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        

         }
/*
        
        tb_Position UIToEntity()
        {
        tb_Position entity = new tb_Position();
                     entity.Left = txtLeft.Text ;
                       entity.Right = txtRight.Text ;
                       entity.Bottom = txtBottom.Text ;
                       entity.Top = txtTop.Text ;
                               return entity;
}
        */

        
        private tb_Position _EditEntity;
        public tb_Position EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_Position entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_Position>(entity, t => t.Left, txtLeft, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Position>(entity, t => t.Right, txtRight, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Position>(entity, t => t.Bottom, txtBottom, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Position>(entity, t => t.Top, txtTop, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



