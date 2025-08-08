
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
    /// 网店信息表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_OnlineStoreInfoEdit:UserControl
    {
     public tb_OnlineStoreInfoEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_OnlineStoreInfo UIToEntity()
        {
        tb_OnlineStoreInfo entity = new tb_OnlineStoreInfo();
                     entity.StoreCode = txtStoreCode.Text ;
                       entity.StoreName = txtStoreName.Text ;
                       entity.PlatformName = txtPlatformName.Text ;
                       entity.Contact = txtContact.Text ;
                       entity.Phone = txtPhone.Text ;
                       entity.Address = txtAddress.Text ;
                       entity.Website = txtWebsite.Text ;
                       entity.ResponsiblePerson = txtResponsiblePerson.Text ;
                       entity.Notes = txtNotes.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                                return entity;
}
        */

        
        private tb_OnlineStoreInfo _EditEntity;
        public tb_OnlineStoreInfo EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_OnlineStoreInfo entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_OnlineStoreInfo>(entity, t => t.StoreCode, txtStoreCode, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_OnlineStoreInfo>(entity, t => t.StoreName, txtStoreName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_OnlineStoreInfo>(entity, t => t.PlatformName, txtPlatformName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_OnlineStoreInfo>(entity, t => t.Contact, txtContact, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_OnlineStoreInfo>(entity, t => t.Phone, txtPhone, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_OnlineStoreInfo>(entity, t => t.Address, txtAddress, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_OnlineStoreInfo>(entity, t => t.Website, txtWebsite, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_OnlineStoreInfo>(entity, t => t.ResponsiblePerson, txtResponsiblePerson, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_OnlineStoreInfo>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_OnlineStoreInfo>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_OnlineStoreInfo>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_OnlineStoreInfo>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_OnlineStoreInfo>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



