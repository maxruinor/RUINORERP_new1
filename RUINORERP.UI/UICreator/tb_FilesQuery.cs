
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:23
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
    /// 文档表数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_FilesQuery), "文档表数据查询", true)]
    public partial class tb_FilesQuery:UserControl
    {
     public tb_FilesQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_Files => tb_Files.Doc_ID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
        }
        


    }
}


