
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/19/2024 00:39:06
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
    /// 账户管理，财务系统中使用数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_FM_AccountEdit:UserControl
    {
     public tb_FM_AccountEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_FM_Account UIToEntity()
        {
        tb_FM_Account entity = new tb_FM_Account();
                     entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.Subject_id = Int64.Parse(txtSubject_id.Text);
                        entity.Currency_ID = Int64.Parse(txtCurrency_ID.Text);
                        entity.Account_name = txtAccount_name.Text ;
                       entity.Account_No = txtAccount_No.Text ;
                       entity.Account_type = Int32.Parse(txtAccount_type.Text);
                        entity.Bank = txtBank.Text ;
                       entity.OpeningBalance = Decimal.Parse(txtOpeningBalance.Text);
                        entity.CurrentBalance = Decimal.Parse(txtCurrentBalance.Text);
                                return entity;
}
        */

        
        private tb_FM_Account _EditEntity;
        public tb_FM_Account EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_FM_Account entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v=>v.XXNAME, cmbDepartmentID);
Subject_id主外字段不一致。          Subject_id主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_Account>(entity, t => t.Subject_id, txtSubject_id, BindDataType4TextBox.Qty,false);
          // DataBindingHelper.BindData4Cmb<tb_Currency>(entity, k => k.Currency_ID, v=>v.XXNAME, cmbCurrency_ID);
Subject_id主外字段不一致。           DataBindingHelper.BindData4TextBox<tb_FM_Account>(entity, t => t.Account_name, txtAccount_name, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Account>(entity, t => t.Account_No, txtAccount_No, BindDataType4TextBox.Text,false);
          Subject_id主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_Account>(entity, t => t.Account_type, txtAccount_type, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Account>(entity, t => t.Bank, txtBank, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Account>(entity, t => t.OpeningBalance.ToString(), txtOpeningBalance, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Account>(entity, t => t.CurrentBalance.ToString(), txtCurrentBalance, BindDataType4TextBox.Money,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



