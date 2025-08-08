
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:12
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
    /// 薪资发放表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_SalaryPaymentEdit:UserControl
    {
     public tb_SalaryPaymentEdit() {
     
                         InitializeComponent();
      
        
        
        

         }
/*
        
        tb_SalaryPayment UIToEntity()
        {
        tb_SalaryPayment entity = new tb_SalaryPayment();
                     entity.salary_month = DateTime.Parse(txtsalary_month.Text);
                        entity.amount = Decimal.Parse(txtamount.Text);
                                return entity;
}
        */

        
        private tb_SalaryPayment _EditEntity;
        public tb_SalaryPayment EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_SalaryPayment entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4DataTime<tb_SalaryPayment>(entity, t => t.salary_month, dtpsalary_month,false);
           DataBindingHelper.BindData4TextBox<tb_SalaryPayment>(entity, t => t.amount.ToString(), txtamount, BindDataType4TextBox.Money,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



