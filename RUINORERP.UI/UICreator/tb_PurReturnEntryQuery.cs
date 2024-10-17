
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/16/2024 20:05:35
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
    /// 采购退货入库单数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_PurReturnEntryQuery), "采购退货入库单数据查询", true)]
    public partial class tb_PurReturnEntryQuery:UserControl
    {
     public tb_PurReturnEntryQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_PurReturnEntry => tb_PurReturnEntry.PurReEntry_ID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_PurEntryRe>(k => k.PurEntryRe_ID, v=>v.XXNAME, cmbPurEntryRe_ID);
          // DataBindingHelper.InitDataToCmb<tb_CustomerVendor>(k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
          // DataBindingHelper.InitDataToCmb<tb_PaymentMethod>(k => k.Paytype_ID, v=>v.XXNAME, cmbPaytype_ID);
          // DataBindingHelper.InitDataToCmb<tb_Department>(k => k.DepartmentID, v=>v.XXNAME, cmbDepartmentID);
          // DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
        }
        


    }
}


