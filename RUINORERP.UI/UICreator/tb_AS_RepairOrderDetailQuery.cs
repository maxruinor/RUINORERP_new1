﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/24/2025 20:25:42
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
    /// 维修工单明细数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_AS_RepairOrderDetailQuery), "维修工单明细数据查询", true)]
    public partial class tb_AS_RepairOrderDetailQuery:UserControl
    {
     public tb_AS_RepairOrderDetailQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_AS_RepairOrderDetail => tb_AS_RepairOrderDetail.RepairOrderDetailID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_Location>(k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
          // DataBindingHelper.InitDataToCmb<tb_ProdDetail>(k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
          // DataBindingHelper.InitDataToCmb<tb_AS_RepairOrder>(k => k.RepairOrderID, v=>v.XXNAME, cmbRepairOrderID);
        }
        


    }
}


