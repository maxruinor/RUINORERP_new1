
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/24/2025 17:35:21
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
    /// 货物类型  成品  半成品  包装材料 下脚料这种内容数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_ProductTypeQuery), "货物类型  成品  半成品  包装材料 下脚料这种内容数据查询", true)]
    public partial class tb_ProductTypeQuery:UserControl
    {
     public tb_ProductTypeQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_ProductType => tb_ProductType.Type_ID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
        }
        


    }
}


