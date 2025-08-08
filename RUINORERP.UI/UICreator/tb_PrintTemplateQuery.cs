
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:50
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
    /// 打印模板数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_PrintTemplateQuery), "打印模板数据查询", true)]
    public partial class tb_PrintTemplateQuery:UserControl
    {
     public tb_PrintTemplateQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_PrintTemplate => tb_PrintTemplate.ID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_PrintConfig>(k => k.PrintConfigID, v=>v.XXNAME, cmbPrintConfigID);
        }
        


    }
}


