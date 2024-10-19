
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/19/2024 00:34:12
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
Account_id主外字段不一致。Subject_id主外字段不一致。          // DataBindingHelper.InitDataToCmb<tb_FM_OtherExpense>(k => k.ExpenseMainID, v=>v.XXNAME, cmbExpenseMainID);
Account_id主外字段不一致。Subject_id主外字段不一致。          // DataBindingHelper.InitDataToCmb<tb_Department>(k => k.DepartmentID, v=>v.XXNAME, cmbDepartmentID);
Account_id主外字段不一致。Subject_id主外字段不一致。          // DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
Account_id主外字段不一致。Subject_id主外字段不一致。          // DataBindingHelper.InitDataToCmb<tb_ProjectGroup>(k => k.ProjectGroup_ID, v=>v.XXNAME, cmbProjectGroup_ID);
Account_id主外字段不一致。Subject_id主外字段不一致。          Account_id主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_FM_Account>(k => k.account_id, v=>v.XXNAME, cmbaccount_id);
Subject_id主外字段不一致。          Account_id主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_FM_ExpenseType>(k => k.ExpenseType_id, v=>v.XXNAME, cmbExpenseType_id);
Subject_id主外字段不一致。          Account_id主外字段不一致。Subject_id主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_FM_Subject>(k => k.subject_id, v=>v.XXNAME, cmbsubject_id);
        }
        


    }
}


