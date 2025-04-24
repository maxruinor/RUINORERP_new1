
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
    /// 项目及成员关系表数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_ProjectGroupEmployeesQuery), "项目及成员关系表数据查询", true)]
    public partial class tb_ProjectGroupEmployeesQuery:UserControl
    {
     public tb_ProjectGroupEmployeesQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_ProjectGroupEmployees => tb_ProjectGroupEmployees.ProjectGroupEmpID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
          // DataBindingHelper.InitDataToCmb<tb_ProjectGroup>(k => k.ProjectGroup_ID, v=>v.XXNAME, cmbProjectGroup_ID);
        }
        


    }
}


