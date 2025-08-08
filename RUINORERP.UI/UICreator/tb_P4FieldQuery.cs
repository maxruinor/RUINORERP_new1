
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
    /// 字段权限表数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_P4FieldQuery), "字段权限表数据查询", true)]
    public partial class tb_P4FieldQuery:UserControl
    {
     public tb_P4FieldQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_P4Field => tb_P4Field.P4Field_ID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_FieldInfo>(k => k.FieldInfo_ID, v=>v.XXNAME, cmbFieldInfo_ID);
          // DataBindingHelper.InitDataToCmb<tb_MenuInfo>(k => k.MenuID, v=>v.XXNAME, cmbMenuID);
          // DataBindingHelper.InitDataToCmb<tb_RoleInfo>(k => k.RoleID, v=>v.XXNAME, cmbRoleID);
        }
        


    }
}


