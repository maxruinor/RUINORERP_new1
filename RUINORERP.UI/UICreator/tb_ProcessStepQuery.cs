
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
    /// 流程步骤数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_ProcessStepQuery), "流程步骤数据查询", true)]
    public partial class tb_ProcessStepQuery:UserControl
    {
     public tb_ProcessStepQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_ProcessStep => tb_ProcessStep.Step_Id).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_NextNodes>(k => k.NextNode_ID, v=>v.XXNAME, cmbNextNode_ID);
          // DataBindingHelper.InitDataToCmb<tb_Position>(k => k.Position_Id, v=>v.XXNAME, cmbPosition_Id);
          // DataBindingHelper.InitDataToCmb<tb_StepBody>(k => k.StepBodyld, v=>v.XXNAME, cmbStepBodyld);
        }
        


    }
}


