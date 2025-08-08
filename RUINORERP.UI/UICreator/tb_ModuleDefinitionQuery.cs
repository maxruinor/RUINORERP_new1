
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:43
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
    /// 功能模块定义（仅限部分已经硬码并体现于菜单表中）数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_ModuleDefinitionQuery), "功能模块定义（仅限部分已经硬码并体现于菜单表中）数据查询", true)]
    public partial class tb_ModuleDefinitionQuery:UserControl
    {
     public tb_ModuleDefinitionQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_ModuleDefinition => tb_ModuleDefinition.ModuleID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
        }
        


    }
}


