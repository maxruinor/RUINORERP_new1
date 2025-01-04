
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/30/2024 18:08:37
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
    /// 系统注册信息数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_sys_RegistrationInfoEdit:UserControl
    {
     public tb_sys_RegistrationInfoEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_sys_RegistrationInfo UIToEntity()
        {
        tb_sys_RegistrationInfo entity = new tb_sys_RegistrationInfo();
                     entity.CompanyName = txtCompanyName.Text ;
                       entity.ContactName = txtContactName.Text ;
                       entity.PhoneNumber = txtPhoneNumber.Text ;
                       entity.MachineCode = txtMachineCode.Text ;
                       entity.RegistrationCode = txtRegistrationCode.Text ;
                       entity.ConcurrentUsers = Int32.Parse(txtConcurrentUsers.Text);
                        entity.ExpirationDate = DateTime.Parse(txtExpirationDate.Text);
                        entity.ProductVersion = txtProductVersion.Text ;
                       entity.LicenseType = txtLicenseType.Text ;
                       entity.PurchaseDate = DateTime.Parse(txtPurchaseDate.Text);
                        entity.RegistrationDate = DateTime.Parse(txtRegistrationDate.Text);
                        entity.IsRegistered = Boolean.Parse(txtIsRegistered.Text);
                        entity.Remarks = txtRemarks.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                                return entity;
}
        */

        
        private tb_sys_RegistrationInfo _EditEntity;
        public tb_sys_RegistrationInfo EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_sys_RegistrationInfo entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_sys_RegistrationInfo>(entity, t => t.CompanyName, txtCompanyName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_sys_RegistrationInfo>(entity, t => t.ContactName, txtContactName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_sys_RegistrationInfo>(entity, t => t.PhoneNumber, txtPhoneNumber, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_sys_RegistrationInfo>(entity, t => t.MachineCode, txtMachineCode, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_sys_RegistrationInfo>(entity, t => t.RegistrationCode, txtRegistrationCode, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_sys_RegistrationInfo>(entity, t => t.ConcurrentUsers, txtConcurrentUsers, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_sys_RegistrationInfo>(entity, t => t.ExpirationDate, dtpExpirationDate,false);
           DataBindingHelper.BindData4TextBox<tb_sys_RegistrationInfo>(entity, t => t.ProductVersion, txtProductVersion, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_sys_RegistrationInfo>(entity, t => t.LicenseType, txtLicenseType, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_sys_RegistrationInfo>(entity, t => t.PurchaseDate, dtpPurchaseDate,false);
           DataBindingHelper.BindData4DataTime<tb_sys_RegistrationInfo>(entity, t => t.RegistrationDate, dtpRegistrationDate,false);
           DataBindingHelper.BindData4CheckBox<tb_sys_RegistrationInfo>(entity, t => t.IsRegistered, chkIsRegistered, false);
           DataBindingHelper.BindData4TextBox<tb_sys_RegistrationInfo>(entity, t => t.Remarks, txtRemarks, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_sys_RegistrationInfo>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_sys_RegistrationInfo>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_sys_RegistrationInfo>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_sys_RegistrationInfo>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



