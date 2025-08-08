
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:54
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
    /// 产品转换单 A变成B出库,AB相近。可能只是换说明书或刷机  A  数量  加或减 。B数量增加或减少。数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_ProdConversionQuery), "产品转换单 A变成B出库,AB相近。可能只是换说明书或刷机  A  数量  加或减 。B数量增加或减少。数据查询", true)]
    public partial class tb_ProdConversionQuery:UserControl
    {
     public tb_ProdConversionQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_ProdConversion => tb_ProdConversion.ConversionID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
          // DataBindingHelper.InitDataToCmb<tb_Location>(k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
        }
        


    }
}


