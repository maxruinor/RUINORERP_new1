﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/07/2024 19:06:32
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
    /// 产品详细表数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_ProdDetailQuery), "产品详细表数据查询", true)]
    public partial class tb_ProdDetailQuery:UserControl
    {
     public tb_ProdDetailQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_ProdDetail => tb_ProdDetail.ProdDetailID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_BOM_S>(k => k.BOM_ID, v=>v.XXNAME, cmbBOM_ID);
          // DataBindingHelper.InitDataToCmb<tb_Prod>(k => k.ProdBaseID, v=>v.XXNAME, cmbProdBaseID);
        }
        


    }
}


