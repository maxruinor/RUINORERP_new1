﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/20/2025 22:58:08
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
    /// UI表格设置数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_UIGridSettingQuery), "UI表格设置数据查询", true)]
    public partial class tb_UIGridSettingQuery:UserControl
    {
     public tb_UIGridSettingQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_UIGridSetting => tb_UIGridSetting.UIGID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_UIMenuPersonalization>(k => k.UIMenuPID, v=>v.XXNAME, cmbUIMenuPID);
        }
        


    }
}


