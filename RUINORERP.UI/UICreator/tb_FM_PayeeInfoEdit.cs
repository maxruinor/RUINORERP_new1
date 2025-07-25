
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/24/2025 20:26:54
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
    /// 收款信息，供应商报销人的收款账号数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_FM_PayeeInfoEdit:UserControl
    {
     public tb_FM_PayeeInfoEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_FM_PayeeInfo UIToEntity()
        {
        tb_FM_PayeeInfo entity = new tb_FM_PayeeInfo();
                     entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.Account_type = Int32.Parse(txtAccount_type.Text);
                        entity.Account_name = txtAccount_name.Text ;
                       entity.Account_No = txtAccount_No.Text ;
                       entity.PaymentCodeImagePath = txtPaymentCodeImagePath.Text ;
                       entity.BelongingBank = txtBelongingBank.Text ;
                       entity.OpeningBank = txtOpeningBank.Text ;
                       entity.Notes = txtNotes.Text ;
                       entity.IsDefault = Boolean.Parse(txtIsDefault.Text);
                        entity.Is_enabled = Boolean.Parse(txtIs_enabled.Text);
                                return entity;
}
        */

        
        private tb_FM_PayeeInfo _EditEntity;
        public tb_FM_PayeeInfo EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_FM_PayeeInfo entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
          // DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
           DataBindingHelper.BindData4TextBox<tb_FM_PayeeInfo>(entity, t => t.Account_type, txtAccount_type, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PayeeInfo>(entity, t => t.Account_name, txtAccount_name, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PayeeInfo>(entity, t => t.Account_No, txtAccount_No, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PayeeInfo>(entity, t => t.PaymentCodeImagePath, txtPaymentCodeImagePath, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PayeeInfo>(entity, t => t.BelongingBank, txtBelongingBank, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PayeeInfo>(entity, t => t.OpeningBank, txtOpeningBank, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_PayeeInfo>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_PayeeInfo>(entity, t => t.IsDefault, chkIsDefault, false);
           DataBindingHelper.BindData4CheckBox<tb_FM_PayeeInfo>(entity, t => t.Is_enabled, chkIs_enabled, false);
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



