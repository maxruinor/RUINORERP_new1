
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
    /// 流程图定义数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_FlowchartDefinitionEdit:UserControl
    {
     public tb_FlowchartDefinitionEdit() {
     
                         InitializeComponent();
      
        
        
        
        

         }
/*
        
        tb_FlowchartDefinition UIToEntity()
        {
        tb_FlowchartDefinition entity = new tb_FlowchartDefinition();
                     entity.ModuleID = Int64.Parse(txtModuleID.Text);
                        entity.FlowchartNo = txtFlowchartNo.Text ;
                       entity.FlowchartName = txtFlowchartName.Text ;
                               return entity;
}
        */

        
        private tb_FlowchartDefinition _EditEntity;
        public tb_FlowchartDefinition EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_FlowchartDefinition entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_ModuleDefinition>(entity, k => k.ModuleID, v=>v.XXNAME, cmbModuleID);
           DataBindingHelper.BindData4TextBox<tb_FlowchartDefinition>(entity, t => t.FlowchartNo, txtFlowchartNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FlowchartDefinition>(entity, t => t.FlowchartName, txtFlowchartName, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



