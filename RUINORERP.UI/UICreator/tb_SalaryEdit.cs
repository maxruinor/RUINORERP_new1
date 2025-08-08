
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
    /// 工资表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_SalaryEdit:UserControl
    {
     public tb_SalaryEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        

         }
/*
        
        tb_Salary UIToEntity()
        {
        tb_Salary entity = new tb_Salary();
                     entity.SalaryDate = DateTime.Parse(txtSalaryDate.Text);
                        entity.BaseSalary = Decimal.Parse(txtBaseSalary.Text);
                        entity.Bonus = Decimal.Parse(txtBonus.Text);
                        entity.Deduction = Decimal.Parse(txtDeduction.Text);
                        entity.ActualSalary = Decimal.Parse(txtActualSalary.Text);
                                return entity;
}
        */

        
        private tb_Salary _EditEntity;
        public tb_Salary EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_Salary entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4DataTime<tb_Salary>(entity, t => t.SalaryDate, dtpSalaryDate,false);
           DataBindingHelper.BindData4TextBox<tb_Salary>(entity, t => t.BaseSalary.ToString(), txtBaseSalary, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_Salary>(entity, t => t.Bonus.ToString(), txtBonus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_Salary>(entity, t => t.Deduction.ToString(), txtDeduction, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_Salary>(entity, t => t.ActualSalary.ToString(), txtActualSalary, BindDataType4TextBox.Money,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



