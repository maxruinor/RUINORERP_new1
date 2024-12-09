
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/09/2024 12:02:39
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
    /// 城市表数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_CitiesQuery), "城市表数据查询", true)]
    public partial class tb_CitiesQuery:UserControl
    {
     public tb_CitiesQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_Cities => tb_Cities.CityID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_Provinces>(k => k.ProvinceID, v=>v.XXNAME, cmbProvinceID);
        }
        


    }
}


