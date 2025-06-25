
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/24/2025 18:44:35
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
    /// 价格调整单明细数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_FM_PriceAdjustmentDetailQuery), "价格调整单明细数据查询", true)]
    public partial class tb_FM_PriceAdjustmentDetailQuery:UserControl
    {
     public tb_FM_PriceAdjustmentDetailQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_FM_PriceAdjustmentDetail => tb_FM_PriceAdjustmentDetail.AdjustDetailID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_ProdDetail>(k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
          // DataBindingHelper.InitDataToCmb<tb_Unit>(k => k.Unit_ID, v=>v.XXNAME, cmbUnit_ID);
          // DataBindingHelper.InitDataToCmb<tb_FM_PriceAdjustment>(k => k.AdjustId, v=>v.XXNAME, cmbAdjustId);
        }
        


    }
}


