
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/13/2025 22:52:39
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
    /// 往来单位类型,如级别，电商，大客户，亚马逊等数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_CustomerVendorTypeQuery), "往来单位类型,如级别，电商，大客户，亚马逊等数据查询", true)]
    public partial class tb_CustomerVendorTypeQuery:UserControl
    {
     public tb_CustomerVendorTypeQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_CustomerVendorType => tb_CustomerVendorType.Type_ID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
        }
        


    }
}


