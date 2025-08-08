
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:38
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
    /// 库位类别数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_LocationTypeEdit:UserControl
    {
     public tb_LocationTypeEdit() {
     
                         InitializeComponent();
      
        
        
        

         }
/*
        
        tb_LocationType UIToEntity()
        {
        tb_LocationType entity = new tb_LocationType();
                     entity.TypeName = txtTypeName.Text ;
                       entity.Desc = txtDesc.Text ;
                               return entity;
}
        */

        
        private tb_LocationType _EditEntity;
        public tb_LocationType EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_LocationType entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_LocationType>(entity, t => t.TypeName, txtTypeName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_LocationType>(entity, t => t.Desc, txtDesc, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



