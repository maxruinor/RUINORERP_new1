
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 11:11:33
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
    /// 单据标识 保存在主单中一个字段，作用于各种单明细的搜索过滤 有必要吗？数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_BillMarkingEdit:UserControl
    {
     public tb_BillMarkingEdit() {
     
                         InitializeComponent();
      
        
        
        

         }
/*
        
        tb_BillMarking UIToEntity()
        {
        tb_BillMarking entity = new tb_BillMarking();
                     entity.TypeName = txtTypeName.Text ;
                       entity.Desc = txtDesc.Text ;
                               return entity;
}
        */

        
        private tb_BillMarking _EditEntity;
        public tb_BillMarking EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_BillMarking entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_BillMarking>(entity, t => t.TypeName, txtTypeName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_BillMarking>(entity, t => t.Desc, txtDesc, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



