
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/20/2025 16:08:15
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
    /// 对账单明细数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_FM_StatementDetailQuery), "对账单明细数据查询", true)]
    public partial class tb_FM_StatementDetailQuery:UserControl
    {
     public tb_FM_StatementDetailQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_FM_StatementDetail => tb_FM_StatementDetail.StatementDetailId).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_Currency>(k => k.Currency_ID, v=>v.XXNAME, cmbCurrency_ID);
          // DataBindingHelper.InitDataToCmb<tb_FM_ReceivablePayable>(k => k.ARAPId, v=>v.XXNAME, cmbARAPId);
          // DataBindingHelper.InitDataToCmb<tb_FM_Statement>(k => k.StatementId, v=>v.XXNAME, cmbStatementId);
        }
        


    }
}


