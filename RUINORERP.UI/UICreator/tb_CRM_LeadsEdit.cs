
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/09/2024 12:15:46
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
    /// 线索机会-询盘数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_CRM_LeadsEdit:UserControl
    {
     public tb_CRM_LeadsEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_CRM_Leads UIToEntity()
        {
        tb_CRM_Leads entity = new tb_CRM_Leads();
                     entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.LeadsStatus = Int32.Parse(txtLeadsStatus.Text);
                        entity.SocialTools = txtSocialTools.Text ;
                       entity.CustomerName = txtCustomerName.Text ;
                       entity.CustomerTags = txtCustomerTags.Text ;
                       entity.GetCustomerSource = txtGetCustomerSource.Text ;
                       entity.InterestedProducts = txtInterestedProducts.Text ;
                       entity.Contact_Name = txtContact_Name.Text ;
                       entity.Contact_Phone = txtContact_Phone.Text ;
                       entity.Contact_Email = txtContact_Email.Text ;
                       entity.Position = txtPosition.Text ;
                       entity.SalePlatform = txtSalePlatform.Text ;
                       entity.Address = txtAddress.Text ;
                       entity.Website = txtWebsite.Text ;
                       entity.Notes = txtNotes.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                                return entity;
}
        */

        
        private tb_CRM_Leads _EditEntity;
        public tb_CRM_Leads EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_CRM_Leads entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
           DataBindingHelper.BindData4TextBox<tb_CRM_Leads>(entity, t => t.LeadsStatus, txtLeadsStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Leads>(entity, t => t.SocialTools, txtSocialTools, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Leads>(entity, t => t.CustomerName, txtCustomerName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Leads>(entity, t => t.CustomerTags, txtCustomerTags, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Leads>(entity, t => t.GetCustomerSource, txtGetCustomerSource, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Leads>(entity, t => t.InterestedProducts, txtInterestedProducts, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Leads>(entity, t => t.Contact_Name, txtContact_Name, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Leads>(entity, t => t.Contact_Phone, txtContact_Phone, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Leads>(entity, t => t.Contact_Email, txtContact_Email, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Leads>(entity, t => t.Position, txtPosition, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Leads>(entity, t => t.SalePlatform, txtSalePlatform, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Leads>(entity, t => t.Address, txtAddress, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Leads>(entity, t => t.Website, txtWebsite, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Leads>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_CRM_Leads>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Leads>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_CRM_Leads>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Leads>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CehckBox<tb_CRM_Leads>(entity, t => t.isdeleted, chkisdeleted, false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



