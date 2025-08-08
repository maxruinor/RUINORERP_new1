
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:13
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
    /// 标准物料表BOM明细的替代材料表-使用优化级按库存量-成本-保质时间在配置来确定数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_BOM_SDetailSubstituteMaterialQuery), "标准物料表BOM明细的替代材料表-使用优化级按库存量-成本-保质时间在配置来确定数据查询", true)]
    public partial class tb_BOM_SDetailSubstituteMaterialQuery:UserControl
    {
     public tb_BOM_SDetailSubstituteMaterialQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_BOM_SDetailSubstituteMaterial => tb_BOM_SDetailSubstituteMaterial.SubstituteMaterialID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_BOM_SDetail>(k => k.SubID, v=>v.XXNAME, cmbSubID);
          // DataBindingHelper.InitDataToCmb<tb_Unit>(k => k.Unit_ID, v=>v.XXNAME, cmbUnit_ID);
          // DataBindingHelper.InitDataToCmb<tb_Unit_Conversion>(k => k.UnitConversion_ID, v=>v.XXNAME, cmbUnitConversion_ID);
          // DataBindingHelper.InitDataToCmb<tb_ProdDetail>(k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
        }
        


    }
}


