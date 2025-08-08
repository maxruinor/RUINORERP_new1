
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:58
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
    /// 归还单，如果部分无法归还，则强制结案借出单。生成一个财务数据做记录。数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_ProdReturningQuery), "归还单，如果部分无法归还，则强制结案借出单。生成一个财务数据做记录。数据查询", true)]
    public partial class tb_ProdReturningQuery:UserControl
    {
     public tb_ProdReturningQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_ProdReturning => tb_ProdReturning.ReturnID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_CustomerVendor>(k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
          // DataBindingHelper.InitDataToCmb<tb_Employee>(k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
          // DataBindingHelper.InitDataToCmb<tb_ProdBorrowing>(k => k.BorrowID, v=>v.XXNAME, cmbBorrowID);
        }
        


    }
}


