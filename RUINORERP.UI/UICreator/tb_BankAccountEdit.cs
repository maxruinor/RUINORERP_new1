
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 11:11:33
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
    /// 银行账号信息表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_BankAccountEdit:UserControl
    {
     public tb_BankAccountEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        

         }
/*
        
        tb_BankAccount UIToEntity()
        {
        tb_BankAccount entity = new tb_BankAccount();
                     entity.Account_Name = txtAccount_Name.Text ;
                       entity.Account_No = txtAccount_No.Text ;
                       entity.OpeningBank = txtOpeningBank.Text ;
                       entity.Is_enabled = Boolean.Parse(txtIs_enabled.Text);
                        entity.Notes = txtNotes.Text ;
                               return entity;
}
        */

        
        private tb_BankAccount _EditEntity;
        public tb_BankAccount EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_BankAccount entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_BankAccount>(entity, t => t.Account_Name, txtAccount_Name, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_BankAccount>(entity, t => t.Account_No, txtAccount_No, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_BankAccount>(entity, t => t.OpeningBank, txtOpeningBank, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CehckBox<tb_BankAccount>(entity, t => t.Is_enabled, chkIs_enabled, false);
//有默认值
           DataBindingHelper.BindData4TextBox<tb_BankAccount>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



