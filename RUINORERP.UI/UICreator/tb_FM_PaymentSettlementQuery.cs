﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/24/2025 20:27:03
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
    /// 记录收款 与应收的匹配，核销表 核销记录用于跟踪资金与债权债务的冲抵关系，确保财务数据可追溯。正常的收款，支付不需要保存核销记录数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_FM_PaymentSettlementQuery), "记录收款 与应收的匹配，核销表 核销记录用于跟踪资金与债权债务的冲抵关系，确保财务数据可追溯。正常的收款，支付不需要保存核销记录数据查询", true)]
    public partial class tb_FM_PaymentSettlementQuery:UserControl
    {
     public tb_FM_PaymentSettlementQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_FM_PaymentSettlement => tb_FM_PaymentSettlement.SettlementId).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_Currency>(k => k.Currency_ID, v=>v.XXNAME, cmbCurrency_ID);
ReversedSettlementID主外字段不一致。          // DataBindingHelper.InitDataToCmb<tb_FM_Account>(k => k.Account_id, v=>v.XXNAME, cmbAccount_id);
ReversedSettlementID主外字段不一致。          ReversedSettlementID主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_FM_PaymentSettlement>(k => k.SettlementId, v=>v.XXNAME, cmbSettlementId);
        }
        


    }
}


