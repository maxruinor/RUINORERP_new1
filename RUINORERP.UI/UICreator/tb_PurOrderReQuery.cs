
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:09
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
    /// 采购退回单 当采购发生退回时，您可以将它记录在本系统的采购退回作业中。在实际业务中虽然发生了采购但实际货物却还未入库，采购退回作业可退回订金、退回数量处理。采购退回单可以由采购订单转入，也可以手动录入新增单据,一般没有金额变化的，可以直接作废采购单。有订单等才需要做退回数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_PurOrderReQuery), "采购退回单 当采购发生退回时，您可以将它记录在本系统的采购退回作业中。在实际业务中虽然发生了采购但实际货物却还未入库，采购退回作业可退回订金、退回数量处理。采购退回单可以由采购订单转入，也可以手动录入新增单据,一般没有金额变化的，可以直接作废采购单。有订单等才需要做退回数据查询", true)]
    public partial class tb_PurOrderReQuery:UserControl
    {
     public tb_PurOrderReQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_PurOrderRe => tb_PurOrderRe.PurRetrunID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_PurOrder>(k => k.PurOrder_ID, v=>v.XXNAME, cmbPurOrder_ID);
          // DataBindingHelper.InitDataToCmb<tb_CustomerVendor>(k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
        }
        


    }
}


