
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:11
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
    /// 开票资料表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_BillingInformationEdit:UserControl
    {
     public tb_BillingInformationEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_BillingInformation UIToEntity()
        {
        tb_BillingInformation entity = new tb_BillingInformation();
                     entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.Title = txtTitle.Text ;
                       entity.TaxNumber = txtTaxNumber.Text ;
                       entity.Address = txtAddress.Text ;
                       entity.PITEL = txtPITEL.Text ;
                       entity.BankAccount = txtBankAccount.Text ;
                       entity.BankName = txtBankName.Text ;
                       entity.Email = txtEmail.Text ;
                       entity.Notes = txtNotes.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                        entity.IsActive = Boolean.Parse(txtIsActive.Text);
                                return entity;
}
        */

        
        private tb_BillingInformation _EditEntity;
        public tb_BillingInformation EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_BillingInformation entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
           DataBindingHelper.BindData4TextBox<tb_BillingInformation>(entity, t => t.Title, txtTitle, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_BillingInformation>(entity, t => t.TaxNumber, txtTaxNumber, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_BillingInformation>(entity, t => t.Address, txtAddress, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_BillingInformation>(entity, t => t.PITEL, txtPITEL, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_BillingInformation>(entity, t => t.BankAccount, txtBankAccount, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_BillingInformation>(entity, t => t.BankName, txtBankName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_BillingInformation>(entity, t => t.Email, txtEmail, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_BillingInformation>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_BillingInformation>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_BillingInformation>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_BillingInformation>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_BillingInformation>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_BillingInformation>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4CheckBox<tb_BillingInformation>(entity, t => t.IsActive, chkIsActive, false);
//有默认值
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



