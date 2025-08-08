
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:51
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
    /// 流程定义 http://www.phpheidong.com/blog/article/68471/a3129f742e5e396e3d1e/数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_ProcessDefinitionQuery), "流程定义 http://www.phpheidong.com/blog/article/68471/a3129f742e5e396e3d1e/数据查询", true)]
    public partial class tb_ProcessDefinitionQuery:UserControl
    {
     public tb_ProcessDefinitionQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_ProcessDefinition => tb_ProcessDefinition.ProcessDefinition_Id).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_ProcessStep>(k => k.Step_Id, v=>v.XXNAME, cmbStep_Id);
        }
        


    }
}


