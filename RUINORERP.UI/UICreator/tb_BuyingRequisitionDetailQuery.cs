
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:15
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
    /// 请购单明细表数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_BuyingRequisitionDetailQuery), "请购单明细表数据查询", true)]
    public partial class tb_BuyingRequisitionDetailQuery:UserControl
    {
     public tb_BuyingRequisitionDetailQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_BuyingRequisitionDetail => tb_BuyingRequisitionDetail.PuRequisition_ChildID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_ProdDetail>(k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
          // DataBindingHelper.InitDataToCmb<tb_BuyingRequisition>(k => k.PuRequisition_ID, v=>v.XXNAME, cmbPuRequisition_ID);
        }
        


    }
}


