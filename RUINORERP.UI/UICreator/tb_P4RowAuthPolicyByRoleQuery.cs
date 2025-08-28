
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/28/2025 15:02:28
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
    /// 行级权限规则-角色关联表数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_P4RowAuthPolicyByRoleQuery), "行级权限规则-角色关联表数据查询", true)]
    public partial class tb_P4RowAuthPolicyByRoleQuery:UserControl
    {
     public tb_P4RowAuthPolicyByRoleQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_P4RowAuthPolicyByRole => tb_P4RowAuthPolicyByRole.Policy_Role_RID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_RowAuthPolicy>(k => k.PolicyId, v=>v.XXNAME, cmbPolicyId);
          // DataBindingHelper.InitDataToCmb<tb_MenuInfo>(k => k.MenuID, v=>v.XXNAME, cmbMenuID);
          // DataBindingHelper.InitDataToCmb<tb_RoleInfo>(k => k.RoleID, v=>v.XXNAME, cmbRoleID);
        }
        


    }
}


