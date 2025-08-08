
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:21
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
    /// 客户厂商认证文件表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_CustomerVendorFilesEdit:UserControl
    {
     public tb_CustomerVendorFilesEdit() {
     
                         InitializeComponent();
      
        
        
        
        

         }
/*
        
        tb_CustomerVendorFiles UIToEntity()
        {
        tb_CustomerVendorFiles entity = new tb_CustomerVendorFiles();
                     entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.FileName = txtFileName.Text ;
                       entity.FileType = txtFileType.Text ;
                               return entity;
}
        */

        
        private tb_CustomerVendorFiles _EditEntity;
        public tb_CustomerVendorFiles EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_CustomerVendorFiles entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
           DataBindingHelper.BindData4TextBox<tb_CustomerVendorFiles>(entity, t => t.FileName, txtFileName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CustomerVendorFiles>(entity, t => t.FileType, txtFileType, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



