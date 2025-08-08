
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:07
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
    /// 采购订单，可能来自销售订单也可能来自生产需求也可以直接录数据数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_PurOrderQuery), "采购订单，可能来自销售订单也可能来自生产需求也可以直接录数据数据查询", true)]
    public partial class tb_PurOrderQuery:UserControl
    {
     public tb_PurOrderQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_PurOrder => tb_PurOrder.PurOrder_ID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_CustomerVendor>(k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
          // DataBindingHelper.InitDataToCmb<tb_PaymentMethod>(k => k.Paytype_ID, v=>v.XXNAME, cmbPaytype_ID);
          // DataBindingHelper.InitDataToCmb<tb_SaleOrder>(k => k.SOrder_ID, v=>v.XXNAME, cmbSOrder_ID);
          // DataBindingHelper.InitDataToCmb<tb_Currency>(k => k.Currency_ID, v=>v.XXNAME, cmbCurrency_ID);
          // DataBindingHelper.InitDataToCmb<tb_FM_Account>(k => k.Account_id, v=>v.XXNAME, cmbAccount_id);
          // DataBindingHelper.InitDataToCmb<tb_FM_PayeeInfo>(k => k.PayeeInfoID, v=>v.XXNAME, cmbPayeeInfoID);
          // DataBindingHelper.InitDataToCmb<tb_ProductionDemand>(k => k.PDID, v=>v.XXNAME, cmbPDID);
          // DataBindingHelper.InitDataToCmb<tb_Department>(k => k.DepartmentID, v=>v.XXNAME, cmbDepartmentID);
          // DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
          // DataBindingHelper.InitDataToCmb<tb_ProjectGroup>(k => k.ProjectGroup_ID, v=>v.XXNAME, cmbProjectGroup_ID);
        }
        


    }
}


