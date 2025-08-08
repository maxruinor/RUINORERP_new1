
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:25
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
    /// 付款账号管理数据编辑
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
                        entity.ID = Int64.Parse(txtID.Text);
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
          // DataBindingHelper.BindData4Cmb<tb_FM_Subject>(entity, k => k.Subject_id, v=>v.XXNAME, cmbSubject_id);
          // DataBindingHelper.BindData4Cmb<tb_Company>(entity, k => k.ID, v=>v.XXNAME, cmbID);
          // DataBindingHelper.BindData4Cmb<tb_Currency>(entity, k => k.Currency_ID, v=>v.XXNAME, cmbCurrency_ID);
           DataBindingHelper.BindData4TextBox<tb_FM_Account>(entity, t => t.Account_name, txtAccount_name, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Account>(entity, t => t.Account_No, txtAccount_No, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Account>(entity, t => t.Account_type, txtAccount_type, BindDataType4TextBox.Qty,false);
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



