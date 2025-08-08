
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:22
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
    /// 业务编号规则数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_sys_BillNoRuleQuery), "业务编号规则数据查询", true)]
    public partial class tb_sys_BillNoRuleQuery:UserControl
    {
     public tb_sys_BillNoRuleQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_sys_BillNoRule => tb_sys_BillNoRule.BillNoRuleID).NotEmpty();
       
       
       //===============
       
          
    
        
    
                
        //==============

            InitializeComponent();

       // RuleFor(tb_sys_BillNoRule => tb_sys_BillNoRule.BizType).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
        }
        


    }
}


