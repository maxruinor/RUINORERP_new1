
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:46
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
    /// 订单装箱单，针对特别订单详细处理 也用主子表来做。暂时不搞数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_OrderPackingQuery), "订单装箱单，针对特别订单详细处理 也用主子表来做。暂时不搞数据查询", true)]
    public partial class tb_OrderPackingQuery:UserControl
    {
     public tb_OrderPackingQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_OrderPacking => tb_OrderPacking.OrderPackaging_ID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_SaleOrder>(k => k.SOrder_ID, v=>v.XXNAME, cmbSOrder_ID);
        }
        


    }
}


