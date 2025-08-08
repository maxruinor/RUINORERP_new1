
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:40
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
    /// 制令单的原料明细表 明细对应的是一个树，结构同BOM，先把BOM搞好再来实现这里的细节数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_ManufacturingOrderDetailQuery), "制令单的原料明细表 明细对应的是一个树，结构同BOM，先把BOM搞好再来实现这里的细节数据查询", true)]
    public partial class tb_ManufacturingOrderDetailQuery:UserControl
    {
     public tb_ManufacturingOrderDetailQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_ManufacturingOrderDetail => tb_ManufacturingOrderDetail.MOCID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_Location>(k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
          // DataBindingHelper.InitDataToCmb<tb_ProdDetail>(k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
          // DataBindingHelper.InitDataToCmb<tb_ManufacturingOrder>(k => k.MOID, v=>v.XXNAME, cmbMOID);
        }
        


    }
}


