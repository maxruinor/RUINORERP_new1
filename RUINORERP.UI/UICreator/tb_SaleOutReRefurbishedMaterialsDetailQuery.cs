
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:16
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
    /// 销售退货翻新物料明细表数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_SaleOutReRefurbishedMaterialsDetailQuery), "销售退货翻新物料明细表数据查询", true)]
    public partial class tb_SaleOutReRefurbishedMaterialsDetailQuery:UserControl
    {
     public tb_SaleOutReRefurbishedMaterialsDetailQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_SaleOutReRefurbishedMaterialsDetail => tb_SaleOutReRefurbishedMaterialsDetail.SOutReturnDetail_ID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_Location>(k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
          // DataBindingHelper.InitDataToCmb<tb_SaleOutRe>(k => k.SaleOutRe_ID, v=>v.XXNAME, cmbSaleOutRe_ID);
          // DataBindingHelper.InitDataToCmb<tb_ProdDetail>(k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
        }
        


    }
}


