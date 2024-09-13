
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 11:11:37
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
    /// 字段信息表数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_ButtonInfoQuery), "字段信息表数据查询", true)]
    public partial class tb_ButtonInfoQuery:UserControl
    {
     public tb_ButtonInfoQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_ButtonInfo => tb_ButtonInfo.ButtonInfo_ID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_MenuInfo>(k => k.MenuID, v=>v.XXNAME, cmbMenuID);
        }
        


    }
}


