
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:11
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
    /// 用户接收提醒内容数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_ReminderResultQuery), "用户接收提醒内容数据查询", true)]
    public partial class tb_ReminderResultQuery:UserControl
    {
     public tb_ReminderResultQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_ReminderResult => tb_ReminderResult.ResultId).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_ReminderRule>(k => k.RuleId, v=>v.XXNAME, cmbRuleId);
        }
        


    }
}


