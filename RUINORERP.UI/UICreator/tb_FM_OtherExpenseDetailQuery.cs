
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:29
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
    /// 其它费用记录表，账户管理，财务系统中使用,像基础资料一样单表操作简单数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_FM_OtherExpenseDetailQuery), "其它费用记录表，账户管理，财务系统中使用,像基础资料一样单表操作简单数据查询", true)]
    public partial class tb_FM_OtherExpenseDetailQuery:UserControl
    {
     public tb_FM_OtherExpenseDetailQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_FM_OtherExpenseDetail => tb_FM_OtherExpenseDetail.ExpenseSubID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_CustomerVendor>(k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
          // DataBindingHelper.InitDataToCmb<tb_FM_OtherExpense>(k => k.ExpenseMainID, v=>v.XXNAME, cmbExpenseMainID);
          // DataBindingHelper.InitDataToCmb<tb_Department>(k => k.DepartmentID, v=>v.XXNAME, cmbDepartmentID);
          // DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
          // DataBindingHelper.InitDataToCmb<tb_ProjectGroup>(k => k.ProjectGroup_ID, v=>v.XXNAME, cmbProjectGroup_ID);
          // DataBindingHelper.InitDataToCmb<tb_FM_Account>(k => k.Account_id, v=>v.XXNAME, cmbAccount_id);
          // DataBindingHelper.InitDataToCmb<tb_FM_ExpenseType>(k => k.ExpenseType_id, v=>v.XXNAME, cmbExpenseType_id);
          // DataBindingHelper.InitDataToCmb<tb_FM_Subject>(k => k.Subject_id, v=>v.XXNAME, cmbSubject_id);
        }
        


    }
}


