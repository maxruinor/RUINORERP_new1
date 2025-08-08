
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:25
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
    /// 单位换算表数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_Unit_ConversionQuery), "单位换算表数据查询", true)]
    public partial class tb_Unit_ConversionQuery:UserControl
    {
     public tb_Unit_ConversionQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_Unit_Conversion => tb_Unit_Conversion.UnitConversion_ID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          Target_unit_id主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_Unit>(k => k.Unit_ID, v=>v.XXNAME, cmbUnit_ID);
Source_unit_id主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_Unit>(k => k.Unit_ID, v=>v.XXNAME, cmbUnit_ID);
          Target_unit_id主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_Unit>(k => k.Unit_ID, v=>v.XXNAME, cmbUnit_ID);
Source_unit_id主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_Unit>(k => k.Unit_ID, v=>v.XXNAME, cmbUnit_ID);
        }
        


    }
}


