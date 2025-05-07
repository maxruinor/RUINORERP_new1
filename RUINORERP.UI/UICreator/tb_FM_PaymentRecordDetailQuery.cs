
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/07/2025 15:37:42
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
    /// 收款单明细表：记录收款分配到应收单的明细数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_FM_PaymentRecordDetailQuery), "收款单明细表：记录收款分配到应收单的明细数据查询", true)]
    public partial class tb_FM_PaymentRecordDetailQuery:UserControl
    {
     public tb_FM_PaymentRecordDetailQuery() {
     
         
        
    
                
        //==============

            InitializeComponent();

       // RuleFor(tb_FM_PaymentRecordDetail => tb_FM_PaymentRecordDetail.PaymentDetailId).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_FM_PaymentRecord>(k => k.PaymentId, v=>v.XXNAME, cmbPaymentId);
        }
        


    }
}


