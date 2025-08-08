
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:19
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
    /// 跟进记录表数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_CRM_FollowUpRecordsQuery), "跟进记录表数据查询", true)]
    public partial class tb_CRM_FollowUpRecordsQuery:UserControl
    {
     public tb_CRM_FollowUpRecordsQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_CRM_FollowUpRecords => tb_CRM_FollowUpRecords.RecordID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_CRM_FollowUpPlans>(k => k.PlanID, v=>v.XXNAME, cmbPlanID);
          // DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
          // DataBindingHelper.InitDataToCmb<tb_CRM_Customer>(k => k.Customer_id, v=>v.XXNAME, cmbCustomer_id);
          // DataBindingHelper.InitDataToCmb<tb_CRM_Leads>(k => k.LeadID, v=>v.XXNAME, cmbLeadID);
        }
        


    }
}


