
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/13/2025 22:52:39
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
    /// 往来单位类型,如级别，电商，大客户，亚马逊等数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_CustomerVendorTypeEdit:UserControl
    {
     public tb_CustomerVendorTypeEdit() {
     
                         InitializeComponent();
      
        
        
        
        

         }
/*
        
        tb_CustomerVendorType UIToEntity()
        {
        tb_CustomerVendorType entity = new tb_CustomerVendorType();
                     entity.TypeName = txtTypeName.Text ;
                       entity.Desc = txtDesc.Text ;
                       entity.BusinessPartnerType = Int32.Parse(txtBusinessPartnerType.Text);
                                return entity;
}
        */

        
        private tb_CustomerVendorType _EditEntity;
        public tb_CustomerVendorType EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_CustomerVendorType entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_CustomerVendorType>(entity, t => t.TypeName, txtTypeName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CustomerVendorType>(entity, t => t.Desc, txtDesc, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CustomerVendorType>(entity, t => t.BusinessPartnerType, txtBusinessPartnerType, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



