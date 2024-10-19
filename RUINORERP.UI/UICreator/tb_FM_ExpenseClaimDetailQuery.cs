
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/19/2024 00:34:10
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
    /// 费用报销单明细数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_FM_ExpenseClaimDetailQuery), "费用报销单明细数据查询", true)]
    public partial class tb_FM_ExpenseClaimDetailQuery:UserControl
    {
     public tb_FM_ExpenseClaimDetailQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_FM_ExpenseClaimDetail => tb_FM_ExpenseClaimDetail.ClaimSubID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          Account_id主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_FM_Account>(k => k.account_id, v=>v.XXNAME, cmbaccount_id);
Subject_id主外字段不一致。          Account_id主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_FM_ExpenseType>(k => k.ExpenseType_id, v=>v.XXNAME, cmbExpenseType_id);
Subject_id主外字段不一致。          Account_id主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_ProjectGroup>(k => k.ProjectGroup_ID, v=>v.XXNAME, cmbProjectGroup_ID);
Subject_id主外字段不一致。          Account_id主外字段不一致。Subject_id主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_FM_Subject>(k => k.subject_id, v=>v.XXNAME, cmbsubject_id);
          Account_id主外字段不一致。Subject_id主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_Department>(k => k.DepartmentID, v=>v.XXNAME, cmbDepartmentID);
          Account_id主外字段不一致。Subject_id主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
          Account_id主外字段不一致。Subject_id主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_FM_ExpenseClaim>(k => k.ClaimMainID, v=>v.XXNAME, cmbClaimMainID);
        }
        


    }
}


