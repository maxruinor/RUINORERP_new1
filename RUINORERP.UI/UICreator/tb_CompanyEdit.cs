
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:16
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
    /// 系统使用者公司数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_CompanyEdit:UserControl
    {
     public tb_CompanyEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_Company UIToEntity()
        {
        tb_Company entity = new tb_Company();
                     entity.CompanyCode = txtCompanyCode.Text ;
                       entity.CNName = txtCNName.Text ;
                       entity.ENName = txtENName.Text ;
                       entity.ShortName = txtShortName.Text ;
                       entity.LegalPersonName = txtLegalPersonName.Text ;
                       entity.UnifiedSocialCreditIdentifier = txtUnifiedSocialCreditIdentifier.Text ;
                       entity.Contact = txtContact.Text ;
                       entity.Phone = txtPhone.Text ;
                       entity.Fax = txtFax.Text ;
                       entity.Address = txtAddress.Text ;
                       entity.ENAddress = txtENAddress.Text ;
                       entity.Website = txtWebsite.Text ;
                       entity.Email = txtEmail.Text ;
                       entity.InvoiceTitle = txtInvoiceTitle.Text ;
                       entity.InvoiceTaxNumber = txtInvoiceTaxNumber.Text ;
                       entity.InvoiceAddress = txtInvoiceAddress.Text ;
                       entity.InvoiceTEL = txtInvoiceTEL.Text ;
                       entity.InvoiceBankAccount = txtInvoiceBankAccount.Text ;
                       entity.InvoiceBankName = txtInvoiceBankName.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.Notes = txtNotes.Text ;
                               return entity;
}
        */

        
        private tb_Company _EditEntity;
        public tb_Company EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_Company entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.CompanyCode, txtCompanyCode, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.CNName, txtCNName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.ENName, txtENName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.ShortName, txtShortName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.LegalPersonName, txtLegalPersonName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.UnifiedSocialCreditIdentifier, txtUnifiedSocialCreditIdentifier, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.Contact, txtContact, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.Phone, txtPhone, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.Fax, txtFax, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.Address, txtAddress, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.ENAddress, txtENAddress, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.Website, txtWebsite, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.Email, txtEmail, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.InvoiceTitle, txtInvoiceTitle, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.InvoiceTaxNumber, txtInvoiceTaxNumber, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.InvoiceAddress, txtInvoiceAddress, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.InvoiceTEL, txtInvoiceTEL, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.InvoiceBankAccount, txtInvoiceBankAccount, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.InvoiceBankName, txtInvoiceBankName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_Company>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_Company>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_Company>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



