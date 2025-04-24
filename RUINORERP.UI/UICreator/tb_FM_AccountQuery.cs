
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/23/2025 23:00:49
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
    /// 付款账号管理数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_FM_AccountQuery), "付款账号管理数据查询", true)]
    public partial class tb_FM_AccountQuery:UserControl
    {
     public tb_FM_AccountQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_FM_Account => tb_FM_Account.Account_id).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_Department>(k => k.DepartmentID, v=>v.XXNAME, cmbDepartmentID);
          // DataBindingHelper.InitDataToCmb<tb_Currency>(k => k.Currency_ID, v=>v.XXNAME, cmbCurrency_ID);
          // DataBindingHelper.InitDataToCmb<tb_Company>(k => k.ID, v=>v.XXNAME, cmbID);
          // DataBindingHelper.InitDataToCmb<tb_FM_Subject>(k => k.Subject_id, v=>v.XXNAME, cmbSubject_id);
        }
        


    }
}


