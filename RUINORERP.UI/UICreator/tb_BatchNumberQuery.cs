
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 11:11:33
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
    /// 批次表 在采购入库时和出库时保存批次ID数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_BatchNumberQuery), "批次表 在采购入库时和出库时保存批次ID数据查询", true)]
    public partial class tb_BatchNumberQuery:UserControl
    {
     public tb_BatchNumberQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_BatchNumber => tb_BatchNumber.Batch_ID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
        }
        


    }
}


