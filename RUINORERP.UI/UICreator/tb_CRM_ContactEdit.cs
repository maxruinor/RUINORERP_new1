
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:17
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
    /// 联系人表-爱好跟进数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_CRM_ContactEdit:UserControl
    {
     public tb_CRM_ContactEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_CRM_Contact UIToEntity()
        {
        tb_CRM_Contact entity = new tb_CRM_Contact();
                     entity.Customer_id = Int64.Parse(txtCustomer_id.Text);
                        entity.SocialTools = txtSocialTools.Text ;
                       entity.Contact_Name = txtContact_Name.Text ;
                       entity.Contact_Email = txtContact_Email.Text ;
                       entity.Contact_Phone = txtContact_Phone.Text ;
                       entity.Position = txtPosition.Text ;
                       entity.Preferences = txtPreferences.Text ;
                       entity.Address = txtAddress.Text ;
                       entity.Notes = txtNotes.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                                return entity;
}
        */

        
        private tb_CRM_Contact _EditEntity;
        public tb_CRM_Contact EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_CRM_Contact entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_CRM_Customer>(entity, k => k.Customer_id, v=>v.XXNAME, cmbCustomer_id);
           DataBindingHelper.BindData4TextBox<tb_CRM_Contact>(entity, t => t.SocialTools, txtSocialTools, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Contact>(entity, t => t.Contact_Name, txtContact_Name, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Contact>(entity, t => t.Contact_Email, txtContact_Email, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Contact>(entity, t => t.Contact_Phone, txtContact_Phone, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Contact>(entity, t => t.Position, txtPosition, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Contact>(entity, t => t.Preferences, txtPreferences, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Contact>(entity, t => t.Address, txtAddress, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Contact>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_CRM_Contact>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Contact>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_CRM_Contact>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Contact>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_CRM_Contact>(entity, t => t.isdeleted, chkisdeleted, false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



