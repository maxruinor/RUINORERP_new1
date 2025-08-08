
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
    /// 客户厂商认证文件表数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_CustomerVendorFilesQuery), "客户厂商认证文件表数据查询", true)]
    public partial class tb_CustomerVendorFilesQuery:UserControl
    {
     public tb_CustomerVendorFilesQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_CustomerVendorFiles => tb_CustomerVendorFiles.File_ID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_CustomerVendor>(k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
        }
        


    }
}


