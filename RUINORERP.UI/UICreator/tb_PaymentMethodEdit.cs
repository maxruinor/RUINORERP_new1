
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:48
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
    /// 付款方式 交易方式，后面扩展有关账期 账龄分析的字段数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_PaymentMethodEdit:UserControl
    {
     public tb_PaymentMethodEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        

         }
/*
        
        tb_PaymentMethod UIToEntity()
        {
        tb_PaymentMethod entity = new tb_PaymentMethod();
                     entity.Paytype_Name = txtPaytype_Name.Text ;
                       entity.Desc = txtDesc.Text ;
                       entity.Sort = Int32.Parse(txtSort.Text);
                        entity.Cash = Boolean.Parse(txtCash.Text);
                                return entity;
}
        */

        
        private tb_PaymentMethod _EditEntity;
        public tb_PaymentMethod EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_PaymentMethod entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_PaymentMethod>(entity, t => t.Paytype_Name, txtPaytype_Name, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PaymentMethod>(entity, t => t.Desc, txtDesc, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PaymentMethod>(entity, t => t.Sort, txtSort, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_PaymentMethod>(entity, t => t.Cash, chkCash, false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



