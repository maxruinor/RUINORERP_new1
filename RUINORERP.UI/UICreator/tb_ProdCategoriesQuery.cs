
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:54
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
    /// 产品类别表 与行业相关的产品分类数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_ProdCategoriesQuery), "产品类别表 与行业相关的产品分类数据查询", true)]
    public partial class tb_ProdCategoriesQuery:UserControl
    {
     public tb_ProdCategoriesQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_ProdCategories => tb_ProdCategories.Category_ID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          Parent_id主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_ProdCategories>(k => k.Category_ID, v=>v.XXNAME, cmbCategory_ID);
        }
        


    }
}


