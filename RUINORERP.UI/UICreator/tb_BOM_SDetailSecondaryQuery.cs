
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
    /// 标准物料表次级产出明细数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_BOM_SDetailSecondaryQuery), "标准物料表次级产出明细数据查询", true)]
    public partial class tb_BOM_SDetailSecondaryQuery:UserControl
    {
     public tb_BOM_SDetailSecondaryQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_BOM_SDetailSecondary => tb_BOM_SDetailSecondary.SecID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_BOM_S>(k => k.BOM_ID, v=>v.XXNAME, cmbBOM_ID);
          // DataBindingHelper.InitDataToCmb<tb_ProdDetail>(k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
        }
        


    }
}


