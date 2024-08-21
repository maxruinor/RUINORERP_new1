
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/22/2024 14:01:31
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
    /// 出入库类型  非生产领料/退料  借出，归还  报损报溢？单独处理？数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_OutInStockTypeQuery), "出入库类型  非生产领料/退料  借出，归还  报损报溢？单独处理？数据查询", true)]
    public partial class tb_OutInStockTypeQuery:UserControl
    {
     public tb_OutInStockTypeQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_OutInStockType => tb_OutInStockType.Type_ID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
        }
        


    }
}


