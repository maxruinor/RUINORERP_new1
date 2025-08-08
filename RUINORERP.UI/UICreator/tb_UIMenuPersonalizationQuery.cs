
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:24
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
    /// 用户角色菜单个性化设置表一个角色用户菜单 三个字段为联合主键 就一行数据数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_UIMenuPersonalizationQuery), "用户角色菜单个性化设置表一个角色用户菜单 三个字段为联合主键 就一行数据数据查询", true)]
    public partial class tb_UIMenuPersonalizationQuery:UserControl
    {
     public tb_UIMenuPersonalizationQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_UIMenuPersonalization => tb_UIMenuPersonalization.UIMenuPID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_MenuInfo>(k => k.MenuID, v=>v.XXNAME, cmbMenuID);
          // DataBindingHelper.InitDataToCmb<tb_UserPersonalized>(k => k.UserPersonalizedID, v=>v.XXNAME, cmbUserPersonalizedID);
        }
        


    }
}


