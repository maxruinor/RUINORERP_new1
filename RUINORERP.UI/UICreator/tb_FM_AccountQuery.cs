
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/19/2024 00:39:07
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
    /// 账户管理，财务系统中使用数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_FM_AccountQuery), "账户管理，财务系统中使用数据查询", true)]
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
Subject_id主外字段不一致。          // DataBindingHelper.InitDataToCmb<tb_Currency>(k => k.Currency_ID, v=>v.XXNAME, cmbCurrency_ID);
Subject_id主外字段不一致。          Subject_id主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_FM_Subject>(k => k.subject_id, v=>v.XXNAME, cmbsubject_id);
        }
        


    }
}


