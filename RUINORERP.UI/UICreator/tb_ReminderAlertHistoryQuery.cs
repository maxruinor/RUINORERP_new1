
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/12/2025 00:31:25
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
    /// 提醒信息是通过什么规则通知了什么内容给谁在什么时间。通知记录  暂时不处理数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_ReminderAlertHistoryQuery), "提醒信息是通过什么规则通知了什么内容给谁在什么时间。通知记录  暂时不处理数据查询", true)]
    public partial class tb_ReminderAlertHistoryQuery:UserControl
    {
     public tb_ReminderAlertHistoryQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_ReminderAlertHistory => tb_ReminderAlertHistory.HistoryId).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_ReminderAlert>(k => k.AlertId, v=>v.XXNAME, cmbAlertId);
          // DataBindingHelper.InitDataToCmb<tb_UserInfo>(k => k.User_ID, v=>v.XXNAME, cmbUser_ID);
        }
        


    }
}


