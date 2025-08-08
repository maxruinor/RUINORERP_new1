
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:25
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
    /// 流程图线数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_FlowchartLineEdit:UserControl
    {
     public tb_FlowchartLineEdit() {
     
                         InitializeComponent();
      
        
        
        

         }
/*
        
        tb_FlowchartLine UIToEntity()
        {
        tb_FlowchartLine entity = new tb_FlowchartLine();
                     entity.PointToString1 = txtPointToString1.Text ;
                       entity.PointToString2 = txtPointToString2.Text ;
                               return entity;
}
        */

        
        private tb_FlowchartLine _EditEntity;
        public tb_FlowchartLine EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_FlowchartLine entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_FlowchartLine>(entity, t => t.PointToString1, txtPointToString1, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FlowchartLine>(entity, t => t.PointToString2, txtPointToString2, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



