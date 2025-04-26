
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/25/2025 10:38:50
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
    /// 客户厂商表 开票资料这种与财务有关另外开表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_CustomerVendorEdit:UserControl
    {
     public tb_CustomerVendorEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_CustomerVendor UIToEntity()
        {
        tb_CustomerVendor entity = new tb_CustomerVendor();
                     entity.CVCode = txtCVCode.Text ;
                       entity.CVName = txtCVName.Text ;
                       entity.ShortName = txtShortName.Text ;
                       entity.Type_ID = Int64.Parse(txtType_ID.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.IsExclusive = Boolean.Parse(txtIsExclusive.Text);
                        entity.Paytype_ID = Int64.Parse(txtPaytype_ID.Text);
                        entity.Customer_id = Int64.Parse(txtCustomer_id.Text);
                        entity.Area = txtArea.Text ;
                       entity.Contact = txtContact.Text ;
                       entity.Phone = txtPhone.Text ;
                       entity.Address = txtAddress.Text ;
                       entity.Website = txtWebsite.Text ;
                       entity.CreditLimit = Decimal.Parse(txtCreditLimit.Text);
                        entity.CreditDays = Int32.Parse(txtCreditDays.Text);
                        entity.IsCustomer = Boolean.Parse(txtIsCustomer.Text);
                        entity.IsVendor = Boolean.Parse(txtIsVendor.Text);
                        entity.IsOther = Boolean.Parse(txtIsOther.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.Is_enabled = Boolean.Parse(txtIs_enabled.Text);
                        entity.Is_available = Boolean.Parse(txtIs_available.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                                return entity;
}
        */

        
        private tb_CustomerVendor _EditEntity;
        public tb_CustomerVendor EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_CustomerVendor entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_CustomerVendor>(entity, t => t.CVCode, txtCVCode, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CustomerVendor>(entity, t => t.CVName, txtCVName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CustomerVendor>(entity, t => t.ShortName, txtShortName, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_CustomerVendorType>(entity, k => k.Type_ID, v=>v.XXNAME, cmbType_ID);
          // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
           DataBindingHelper.BindData4CheckBox<tb_CustomerVendor>(entity, t => t.IsExclusive, chkIsExclusive, false);
//有默认值
          // DataBindingHelper.BindData4Cmb<tb_PaymentMethod>(entity, k => k.Paytype_ID, v=>v.XXNAME, cmbPaytype_ID);
           DataBindingHelper.BindData4TextBox<tb_CustomerVendor>(entity, t => t.Customer_id, txtCustomer_id, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_CustomerVendor>(entity, t => t.Area, txtArea, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CustomerVendor>(entity, t => t.Contact, txtContact, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CustomerVendor>(entity, t => t.Phone, txtPhone, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CustomerVendor>(entity, t => t.Address, txtAddress, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CustomerVendor>(entity, t => t.Website, txtWebsite, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CustomerVendor>(entity, t => t.CreditLimit.ToString(), txtCreditLimit, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_CustomerVendor>(entity, t => t.CreditDays, txtCreditDays, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_CustomerVendor>(entity, t => t.IsCustomer, chkIsCustomer, false);
           DataBindingHelper.BindData4CheckBox<tb_CustomerVendor>(entity, t => t.IsVendor, chkIsVendor, false);
           DataBindingHelper.BindData4CheckBox<tb_CustomerVendor>(entity, t => t.IsOther, chkIsOther, false);
           DataBindingHelper.BindData4TextBox<tb_CustomerVendor>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_CustomerVendor>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_CustomerVendor>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_CustomerVendor>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_CustomerVendor>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_CustomerVendor>(entity, t => t.Is_enabled, chkIs_enabled, false);
//有默认值
           DataBindingHelper.BindData4CheckBox<tb_CustomerVendor>(entity, t => t.isdeleted, chkisdeleted, false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



