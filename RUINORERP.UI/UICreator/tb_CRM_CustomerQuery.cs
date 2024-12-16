
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/16/2024 18:39:01
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
    /// 目标客户-公海客户CRM系统中使用，给成交客户作外键引用数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_CRM_CustomerQuery), "目标客户-公海客户CRM系统中使用，给成交客户作外键引用数据查询", true)]
    public partial class tb_CRM_CustomerQuery:UserControl
    {
     public tb_CRM_CustomerQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_CRM_Customer => tb_CRM_Customer.Customer_id).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_CRM_Region>(k => k.Region_ID, v=>v.XXNAME, cmbRegion_ID);
          // DataBindingHelper.InitDataToCmb<tb_Department>(k => k.DepartmentID, v=>v.XXNAME, cmbDepartmentID);
          // DataBindingHelper.InitDataToCmb<tb_Cities>(k => k.CityID, v=>v.XXNAME, cmbCityID);
          // DataBindingHelper.InitDataToCmb<tb_CRM_Leads>(k => k.LeadID, v=>v.XXNAME, cmbLeadID);
          // DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
          // DataBindingHelper.InitDataToCmb<tb_Provinces>(k => k.ProvinceID, v=>v.XXNAME, cmbProvinceID);
        }
        


    }
}


