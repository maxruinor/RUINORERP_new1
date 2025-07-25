
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/24/2025 20:27:19
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
    /// 应收应付明细数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_FM_ReceivablePayableDetailQuery), "应收应付明细数据查询", true)]
    public partial class tb_FM_ReceivablePayableDetailQuery:UserControl
    {
     public tb_FM_ReceivablePayableDetailQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_FM_ReceivablePayableDetail => tb_FM_ReceivablePayableDetail.ARAPDetailID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_ProdDetail>(k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
          // DataBindingHelper.InitDataToCmb<tb_Unit>(k => k.Unit_ID, v=>v.XXNAME, cmbUnit_ID);
          // DataBindingHelper.InitDataToCmb<tb_FM_ReceivablePayable>(k => k.ARAPId, v=>v.XXNAME, cmbARAPId);
        }
        


    }
}


