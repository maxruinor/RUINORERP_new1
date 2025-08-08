
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:17
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
    /// 先销售合同再订单,条款内容后面补充 注意一个合同可以多个发票一个发票也可以多个合同数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_SO_ContractQuery), "先销售合同再订单,条款内容后面补充 注意一个合同可以多个发票一个发票也可以多个合同数据查询", true)]
    public partial class tb_SO_ContractQuery:UserControl
    {
     public tb_SO_ContractQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_SO_Contract => tb_SO_Contract.SOContractID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_CustomerVendor>(k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
          // DataBindingHelper.InitDataToCmb<tb_BillingInformation>(k => k.BillingInfo_ID, v=>v.XXNAME, cmbBillingInfo_ID);
          // DataBindingHelper.InitDataToCmb<tb_ContractTemplate>(k => k.TemplateId, v=>v.XXNAME, cmbTemplateId);
          // DataBindingHelper.InitDataToCmb<tb_Company>(k => k.ID, v=>v.XXNAME, cmbID);
        }
        


    }
}


