
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/01/2025 12:16:47
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
    /// 收付款记录表数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_FM_PaymentRecordQuery), "收付款记录表数据查询", true)]
    public partial class tb_FM_PaymentRecordQuery:UserControl
    {
     public tb_FM_PaymentRecordQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_FM_PaymentRecord => tb_FM_PaymentRecord.PaymentId).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_Currency>(k => k.Currency_ID, v=>v.XXNAME, cmbCurrency_ID);
Reimburser主外字段不一致。ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。          // DataBindingHelper.InitDataToCmb<tb_CustomerVendor>(k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
Reimburser主外字段不一致。ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。          Reimburser主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。          Reimburser主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_FM_PayeeInfo>(k => k.PayeeInfoID, v=>v.XXNAME, cmbPayeeInfoID);
ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。          Reimburser主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_PaymentMethod>(k => k.Paytype_ID, v=>v.XXNAME, cmbPaytype_ID);
ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。          Reimburser主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_FM_Account>(k => k.Account_id, v=>v.XXNAME, cmbAccount_id);
ReversedByPaymentId主外字段不一致。ReversedOriginalId主外字段不一致。          Reimburser主外字段不一致。ReversedByPaymentId主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_FM_PaymentRecord>(k => k.PaymentId, v=>v.XXNAME, cmbPaymentId);
ReversedOriginalId主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_FM_PaymentRecord>(k => k.PaymentId, v=>v.XXNAME, cmbPaymentId);
          Reimburser主外字段不一致。ReversedByPaymentId主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_FM_PaymentRecord>(k => k.PaymentId, v=>v.XXNAME, cmbPaymentId);
ReversedOriginalId主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_FM_PaymentRecord>(k => k.PaymentId, v=>v.XXNAME, cmbPaymentId);
        }
        


    }
}


