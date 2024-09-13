
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 11:11:36
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
    /// BOM配置历史 数据保存在BOM中 只是多份一样，细微区别用版本号标识数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_BOMConfigHistoryQuery), "BOM配置历史 数据保存在BOM中 只是多份一样，细微区别用版本号标识数据查询", true)]
    public partial class tb_BOMConfigHistoryQuery:UserControl
    {
     public tb_BOMConfigHistoryQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_BOMConfigHistory => tb_BOMConfigHistory.BOM_S_VERID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
        }
        


    }
}


