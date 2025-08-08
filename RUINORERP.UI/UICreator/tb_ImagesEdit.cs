
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:37
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
    /// 图片表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_ImagesEdit:UserControl
    {
     public tb_ImagesEdit() {
     
                         InitializeComponent();
      
        
        
        

         }
/*
        
        tb_Images UIToEntity()
        {
        tb_Images entity = new tb_Images();
                     entity.Images = txtImages.Text ;
                       entity.Images_Path = txtImages_Path.Text ;
                               return entity;
}
        */

        
        private tb_Images _EditEntity;
        public tb_Images EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_Images entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_Images>(entity, t => t.Images, txtImages, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Images>(entity, t => t.Images_Path, txtImages_Path, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



