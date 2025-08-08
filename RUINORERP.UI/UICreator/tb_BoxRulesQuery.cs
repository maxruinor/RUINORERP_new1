
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:14
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
    /// 箱规表数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_BoxRulesQuery), "箱规表数据查询", true)]
    public partial class tb_BoxRulesQuery:UserControl
    {
     public tb_BoxRulesQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_BoxRules => tb_BoxRules.BoxRules_ID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_CartoonBox>(k => k.CartonID, v=>v.XXNAME, cmbCartonID);
          // DataBindingHelper.InitDataToCmb<tb_Packing>(k => k.Pack_ID, v=>v.XXNAME, cmbPack_ID);
        }
        


    }
}


