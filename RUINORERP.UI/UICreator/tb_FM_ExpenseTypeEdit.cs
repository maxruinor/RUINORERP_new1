
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/20/2025 16:08:04
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
    /// 业务类型 报销，员工借支还款，运费数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_FM_ExpenseTypeEdit:UserControl
    {
     public tb_FM_ExpenseTypeEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        

         }
/*
        
        tb_FM_ExpenseType UIToEntity()
        {
        tb_FM_ExpenseType entity = new tb_FM_ExpenseType();
                     entity.subject_id = Int64.Parse(txtsubject_id.Text);
                        entity.Expense_name = txtExpense_name.Text ;
                       entity.EXPOrINC = Boolean.Parse(txtEXPOrINC.Text);
                        entity.ReceivePaymentType = Int32.Parse(txtReceivePaymentType.Text);
                        entity.Notes = txtNotes.Text ;
                               return entity;
}
        */

        
        private tb_FM_ExpenseType _EditEntity;
        public tb_FM_ExpenseType EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_FM_ExpenseType entity)
        {
        _EditEntity = entity;
                       subject_id主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_ExpenseType>(entity, t => t.subject_id, txtsubject_id, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ExpenseType>(entity, t => t.Expense_name, txtExpense_name, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_ExpenseType>(entity, t => t.EXPOrINC, chkEXPOrINC, false);
//有默认值
          subject_id主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_FM_ExpenseType>(entity, t => t.ReceivePaymentType, txtReceivePaymentType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_ExpenseType>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



