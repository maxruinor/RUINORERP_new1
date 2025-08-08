
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:37
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
    /// 存货预警特性表数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_Inv_Alert_AttributeQuery), "存货预警特性表数据查询", true)]
    public partial class tb_Inv_Alert_AttributeQuery:UserControl
    {
     public tb_Inv_Alert_AttributeQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_Inv_Alert_Attribute => tb_Inv_Alert_Attribute.Inv_Alert_ID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_Inventory>(k => k.Inventory_ID, v=>v.XXNAME, cmbInventory_ID);
        }
        


    }
}


