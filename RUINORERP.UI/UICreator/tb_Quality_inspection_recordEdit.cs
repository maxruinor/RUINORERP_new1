
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:10
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
    /// 品质检验记录表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_Quality_inspection_recordEdit:UserControl
    {
     public tb_Quality_inspection_recordEdit() {
     
                         InitializeComponent();
      
        

         }
/*
        
        tb_Quality_inspection_record UIToEntity()
        {
        tb_Quality_inspection_record entity = new tb_Quality_inspection_record();
                             return entity;
}
        */

        
        private tb_Quality_inspection_record _EditEntity;
        public tb_Quality_inspection_record EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_Quality_inspection_record entity)
        {
        _EditEntity = entity;
             }




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



