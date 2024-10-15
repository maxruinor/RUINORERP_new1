
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/15/2024 18:45:34
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
    /// 调拨单-两个仓库之间的库存转移数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_StockTransferQuery), "调拨单-两个仓库之间的库存转移数据查询", true)]
    public partial class tb_StockTransferQuery:UserControl
    {
     public tb_StockTransferQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_StockTransfer => tb_StockTransfer.StockTransferID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。          Location_ID_from主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_Location>(k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
Location_ID_to主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_Location>(k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
          Location_ID_from主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_Location>(k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
Location_ID_to主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_Location>(k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
        }
        


    }
}


