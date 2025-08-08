
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:21
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
    /// 客户厂商表 开票资料这种与财务有关另外开表数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_CustomerVendorQuery), "客户厂商表 开票资料这种与财务有关另外开表数据查询", true)]
    public partial class tb_CustomerVendorQuery:UserControl
    {
     public tb_CustomerVendorQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_CustomerVendor => tb_CustomerVendor.CustomerVendor_ID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
          // DataBindingHelper.InitDataToCmb<tb_CustomerVendorType>(k => k.Type_ID, v=>v.XXNAME, cmbType_ID);
          // DataBindingHelper.InitDataToCmb<tb_PaymentMethod>(k => k.Paytype_ID, v=>v.XXNAME, cmbPaytype_ID);
        }
        


    }
}


