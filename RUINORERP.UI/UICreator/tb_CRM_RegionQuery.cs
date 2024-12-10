
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/10/2024 13:12:15
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
    /// 销售分区表-大中华区数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_CRM_RegionQuery), "销售分区表-大中华区数据查询", true)]
    public partial class tb_CRM_RegionQuery:UserControl
    {
     public tb_CRM_RegionQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_CRM_Region => tb_CRM_Region.Region_ID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          Parent_region_id主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_CRM_Region>(k => k.Region_ID, v=>v.XXNAME, cmbRegion_ID);
        }
        


    }
}


