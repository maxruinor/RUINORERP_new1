
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/27/2024 18:30:50
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
    /// 采购入库退回单明细数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_MRP_ReworkReturnDetailQuery), "采购入库退回单明细数据查询", true)]
    public partial class tb_MRP_ReworkReturnDetailQuery:UserControl
    {
     public tb_MRP_ReworkReturnDetailQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_MRP_ReworkReturnDetail => tb_MRP_ReworkReturnDetail.ReworkReturnCID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_MRP_ReworkReturn>(k => k.ReworkReturnID, v=>v.XXNAME, cmbReworkReturnID);
          // DataBindingHelper.InitDataToCmb<tb_Location>(k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
          // DataBindingHelper.InitDataToCmb<tb_ProdDetail>(k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
        }
        


    }
}


