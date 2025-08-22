
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/20/2025 16:08:01
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
    /// 部门表是否分层数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_DepartmentQuery), "部门表是否分层数据查询", true)]
    public partial class tb_DepartmentQuery:UserControl
    {
     public tb_DepartmentQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_Department => tb_Department.DepartmentID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_Company>(k => k.ID, v=>v.XXNAME, cmbID);
        }
        


    }
}


