
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/24/2025 14:14:54
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
    /// 项目组信息 用于业务分组小团队数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_ProjectGroupQuery), "项目组信息 用于业务分组小团队数据查询", true)]
    public partial class tb_ProjectGroupQuery:UserControl
    {
     public tb_ProjectGroupQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_ProjectGroup => tb_ProjectGroup.ProjectGroup_ID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_Department>(k => k.DepartmentID, v=>v.XXNAME, cmbDepartmentID);
        }
        


    }
}


