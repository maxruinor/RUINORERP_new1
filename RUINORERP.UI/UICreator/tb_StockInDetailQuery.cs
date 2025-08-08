
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:19
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
    /// 入库单明细数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_StockInDetailQuery), "入库单明细数据查询", true)]
    public partial class tb_StockInDetailQuery:UserControl
    {
     public tb_StockInDetailQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_StockInDetail => tb_StockInDetail.Sub_ID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_StockIn>(k => k.MainID, v=>v.XXNAME, cmbMainID);
          // DataBindingHelper.InitDataToCmb<tb_StorageRack>(k => k.Rack_ID, v=>v.XXNAME, cmbRack_ID);
          // DataBindingHelper.InitDataToCmb<tb_ProdDetail>(k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
          // DataBindingHelper.InitDataToCmb<tb_Location>(k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
        }
        


    }
}


