
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:22
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
    /// 文档表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_DocumentsEdit:UserControl
    {
     public tb_DocumentsEdit() {
     
                         InitializeComponent();
      
        
        

         }
/*
        
        tb_Documents UIToEntity()
        {
        tb_Documents entity = new tb_Documents();
                     entity.Files_Path = txtFiles_Path.Text ;
                               return entity;
}
        */

        
        private tb_Documents _EditEntity;
        public tb_Documents EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_Documents entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_Documents>(entity, t => t.Files_Path, txtFiles_Path, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



