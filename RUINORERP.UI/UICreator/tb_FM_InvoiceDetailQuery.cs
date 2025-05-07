
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/07/2025 14:22:22
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
    /// 发票明细数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_FM_InvoiceDetailQuery), "发票明细数据查询", true)]
    public partial class tb_FM_InvoiceDetailQuery:UserControl
    {
     public tb_FM_InvoiceDetailQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_FM_InvoiceDetail => tb_FM_InvoiceDetail.InvoiceDetailID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_FM_Invoice>(k => k.InvoiceId, v=>v.XXNAME, cmbInvoiceId);
        }
        


    }
}


