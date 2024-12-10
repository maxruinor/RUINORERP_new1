
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/10/2024 13:12:12
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
    /// 联系人表-爱好跟进数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_CRM_ContactQuery), "联系人表-爱好跟进数据查询", true)]
    public partial class tb_CRM_ContactQuery:UserControl
    {
     public tb_CRM_ContactQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_CRM_Contact => tb_CRM_Contact.Contact_id).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_CRM_Customer>(k => k.Customer_id, v=>v.XXNAME, cmbCustomer_id);
        }
        


    }
}


