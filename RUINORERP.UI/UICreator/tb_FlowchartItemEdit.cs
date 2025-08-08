
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
    /// 流程图子项数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_FlowchartItemEdit:UserControl
    {
     public tb_FlowchartItemEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        

         }
/*
        
        tb_FlowchartItem UIToEntity()
        {
        tb_FlowchartItem entity = new tb_FlowchartItem();
                     entity.IconFile_Path = txtIconFile_Path.Text ;
                       entity.Title = txtTitle.Text ;
                       entity.SizeString = txtSizeString.Text ;
                       entity.PointToString = txtPointToString.Text ;
                               return entity;
}
        */

        
        private tb_FlowchartItem _EditEntity;
        public tb_FlowchartItem EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_FlowchartItem entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_FlowchartItem>(entity, t => t.IconFile_Path, txtIconFile_Path, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FlowchartItem>(entity, t => t.Title, txtTitle, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FlowchartItem>(entity, t => t.SizeString, txtSizeString, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FlowchartItem>(entity, t => t.PointToString, txtPointToString, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



