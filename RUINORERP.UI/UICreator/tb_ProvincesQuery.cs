
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:03
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
    /// 省份表数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_ProvincesQuery), "省份表数据查询", true)]
    public partial class tb_ProvincesQuery:UserControl
    {
     public tb_ProvincesQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_Provinces => tb_Provinces.ProvinceID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_CRM_Region>(k => k.Region_ID, v=>v.XXNAME, cmbRegion_ID);
        }
        


    }
}


