
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/09/2024 12:02:43
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
    /// 跟进计划表数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_CRM_FollowUpPlansQuery), "跟进计划表数据查询", true)]
    public partial class tb_CRM_FollowUpPlansQuery:UserControl
    {
     public tb_CRM_FollowUpPlansQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_CRM_FollowUpPlans => tb_CRM_FollowUpPlans.PlanID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
          // DataBindingHelper.InitDataToCmb<tb_CRM_Customer>(k => k.Customer_id, v=>v.XXNAME, cmbCustomer_id);
        }
        


    }
}


