
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/20/2025 22:58:11
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
    /// UI录入数据预设值表数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_UIInputDataFieldQuery), "UI录入数据预设值表数据查询", true)]
    public partial class tb_UIInputDataFieldQuery:UserControl
    {
     public tb_UIInputDataFieldQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_UIInputDataField => tb_UIInputDataField.PresetValueID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_UIMenuPersonalization>(k => k.UIMenuPID, v=>v.XXNAME, cmbUIMenuPID);
        }
        


    }
}


