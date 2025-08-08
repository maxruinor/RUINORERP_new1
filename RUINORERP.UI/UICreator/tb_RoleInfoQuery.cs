
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:11
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
    /// 角色表数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_RoleInfoQuery), "角色表数据查询", true)]
    public partial class tb_RoleInfoQuery:UserControl
    {
     public tb_RoleInfoQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_RoleInfo => tb_RoleInfo.RoleID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_RolePropertyConfig>(k => k.RolePropertyID, v=>v.XXNAME, cmbRolePropertyID);
        }
        


    }
}


