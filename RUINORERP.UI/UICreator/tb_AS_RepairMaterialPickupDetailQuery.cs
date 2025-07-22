
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/22/2025 18:02:28
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
    /// 维修领料单明细数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_AS_RepairMaterialPickupDetailQuery), "维修领料单明细数据查询", true)]
    public partial class tb_AS_RepairMaterialPickupDetailQuery:UserControl
    {
     public tb_AS_RepairMaterialPickupDetailQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_AS_RepairMaterialPickupDetail => tb_AS_RepairMaterialPickupDetail.RMPDetailID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_AS_RepairMaterialPickup>(k => k.RMRID, v=>v.XXNAME, cmbRMRID);
          // DataBindingHelper.InitDataToCmb<tb_ProdDetail>(k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
          // DataBindingHelper.InitDataToCmb<tb_Location>(k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
        }
        


    }
}


