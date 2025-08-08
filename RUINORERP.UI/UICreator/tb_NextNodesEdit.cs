
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:45
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
    /// 流程步骤 转移条件集合数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_NextNodesEdit:UserControl
    {
     public tb_NextNodesEdit() {
     
                         InitializeComponent();
      
        
        
        

         }
/*
        
        tb_NextNodes UIToEntity()
        {
        tb_NextNodes entity = new tb_NextNodes();
                     entity.ConNodeConditions_Id = Int64.Parse(txtConNodeConditions_Id.Text);
                        entity.NexNodeName = txtNexNodeName.Text ;
                               return entity;
}
        */

        
        private tb_NextNodes _EditEntity;
        public tb_NextNodes EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_NextNodes entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_ConNodeConditions>(entity, k => k.ConNodeConditions_Id, v=>v.XXNAME, cmbConNodeConditions_Id);
           DataBindingHelper.BindData4TextBox<tb_NextNodes>(entity, t => t.NexNodeName, txtNexNodeName, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



