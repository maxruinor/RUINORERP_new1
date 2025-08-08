
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:10
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
    /// 品质检验记录表数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_Quality_inspection_recordQuery), "品质检验记录表数据查询", true)]
    public partial class tb_Quality_inspection_recordQuery:UserControl
    {
     public tb_Quality_inspection_recordQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_Quality_inspection_record => tb_Quality_inspection_record.id).NotEmpty();
       
       
       //===============
       
          

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
        }
        


    }
}


