
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/12/2025 00:31:22
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
    /// 提醒内容数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_ReminderAlertQuery), "提醒内容数据查询", true)]
    public partial class tb_ReminderAlertQuery:UserControl
    {
     public tb_ReminderAlertQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_ReminderAlert => tb_ReminderAlert.AlertId).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_ReminderRule>(k => k.RuleId, v=>v.XXNAME, cmbRuleId);
        }
        


    }
}


