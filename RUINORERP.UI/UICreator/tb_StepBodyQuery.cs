
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:18
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
    /// 步骤定义数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_StepBodyQuery), "步骤定义数据查询", true)]
    public partial class tb_StepBodyQuery:UserControl
    {
     public tb_StepBodyQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_StepBody => tb_StepBody.StepBodyld).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_StepBodyPara>(k => k.Para_Id, v=>v.XXNAME, cmbPara_Id);
        }
        


    }
}


