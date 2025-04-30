
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/30/2025 15:18:06
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
    /// 收款付款记录表-记录所有资金流动一批订单可分账户分批付 记录真实资金流动，用户需确保其 与银行流水一致数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_FM_PaymentRecordQuery), "收款付款记录表-记录所有资金流动一批订单可分账户分批付 记录真实资金流动，用户需确保其 与银行流水一致数据查询", true)]
    public partial class tb_FM_PaymentRecordQuery:UserControl
    {
     public tb_FM_PaymentRecordQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_FM_PaymentRecord => tb_FM_PaymentRecord.PaymentId).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_Currency>(k => k.Currency_ID, v=>v.XXNAME, cmbCurrency_ID);
          // DataBindingHelper.InitDataToCmb<tb_CustomerVendor>(k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
          // DataBindingHelper.InitDataToCmb<tb_Department>(k => k.DepartmentID, v=>v.XXNAME, cmbDepartmentID);
          // DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
          // DataBindingHelper.InitDataToCmb<tb_FM_PayeeInfo>(k => k.PayeeInfoID, v=>v.XXNAME, cmbPayeeInfoID);
          // DataBindingHelper.InitDataToCmb<tb_PaymentMethod>(k => k.Paytype_ID, v=>v.XXNAME, cmbPaytype_ID);
          // DataBindingHelper.InitDataToCmb<tb_ProjectGroup>(k => k.ProjectGroup_ID, v=>v.XXNAME, cmbProjectGroup_ID);
          // DataBindingHelper.InitDataToCmb<tb_FM_Account>(k => k.Account_id, v=>v.XXNAME, cmbAccount_id);
        }
        


    }
}


