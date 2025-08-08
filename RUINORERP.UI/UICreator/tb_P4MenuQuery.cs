
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:47
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
    /// 菜单权限表数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_P4MenuQuery), "菜单权限表数据查询", true)]
    public partial class tb_P4MenuQuery:UserControl
    {
     public tb_P4MenuQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_P4Menu => tb_P4Menu.P4Menu_ID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_MenuInfo>(k => k.MenuID, v=>v.XXNAME, cmbMenuID);
          // DataBindingHelper.InitDataToCmb<tb_ModuleDefinition>(k => k.ModuleID, v=>v.XXNAME, cmbModuleID);
          // DataBindingHelper.InitDataToCmb<tb_RoleInfo>(k => k.RoleID, v=>v.XXNAME, cmbRoleID);
        }
        


    }
}


