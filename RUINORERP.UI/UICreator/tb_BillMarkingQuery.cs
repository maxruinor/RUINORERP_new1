
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:11
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
    /// 单据标识 保存在主单中一个字段，作用于各种单明细的搜索过滤 有必要吗？数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_BillMarkingQuery), "单据标识 保存在主单中一个字段，作用于各种单明细的搜索过滤 有必要吗？数据查询", true)]
    public partial class tb_BillMarkingQuery:UserControl
    {
     public tb_BillMarkingQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_BillMarking => tb_BillMarking.BMType_ID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
        }
        


    }
}


