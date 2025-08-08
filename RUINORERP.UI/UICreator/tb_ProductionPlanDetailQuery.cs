
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:02
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
    /// 生产计划明细数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_ProductionPlanDetailQuery), "生产计划明细数据查询", true)]
    public partial class tb_ProductionPlanDetailQuery:UserControl
    {
     public tb_ProductionPlanDetailQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_ProductionPlanDetail => tb_ProductionPlanDetail.PPCID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_ProdDetail>(k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
          // DataBindingHelper.InitDataToCmb<tb_ProductionPlan>(k => k.PPID, v=>v.XXNAME, cmbPPID);
          // DataBindingHelper.InitDataToCmb<tb_Location>(k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
          // DataBindingHelper.InitDataToCmb<tb_BOM_S>(k => k.BOM_ID, v=>v.XXNAME, cmbBOM_ID);
        }
        


    }
}


