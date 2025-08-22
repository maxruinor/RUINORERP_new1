
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/20/2025 16:08:06
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
    /// 损益费用单数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_FM_ProfitLossDetailQuery), "损益费用单数据查询", true)]
    public partial class tb_FM_ProfitLossDetailQuery:UserControl
    {
     public tb_FM_ProfitLossDetailQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_FM_ProfitLossDetail => tb_FM_ProfitLossDetail.ProfitLossDetail_ID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_FM_ExpenseType>(k => k.ExpenseType_id, v=>v.XXNAME, cmbExpenseType_id);
          // DataBindingHelper.InitDataToCmb<tb_FM_ProfitLoss>(k => k.ProfitLossId, v=>v.XXNAME, cmbProfitLossId);
          // DataBindingHelper.InitDataToCmb<tb_ProdDetail>(k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
        }
        


    }
}


