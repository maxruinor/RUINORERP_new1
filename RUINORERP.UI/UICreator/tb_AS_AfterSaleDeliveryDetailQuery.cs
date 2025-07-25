
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/24/2025 20:25:33
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
    /// 售后交付明细数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_AS_AfterSaleDeliveryDetailQuery), "售后交付明细数据查询", true)]
    public partial class tb_AS_AfterSaleDeliveryDetailQuery:UserControl
    {
     public tb_AS_AfterSaleDeliveryDetailQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_AS_AfterSaleDeliveryDetail => tb_AS_AfterSaleDeliveryDetail.ASDeliveryDetailID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_AS_AfterSaleDelivery>(k => k.ASDeliveryID, v=>v.XXNAME, cmbASDeliveryID);
          // DataBindingHelper.InitDataToCmb<tb_Location>(k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
          // DataBindingHelper.InitDataToCmb<tb_ProdDetail>(k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
        }
        


    }
}


