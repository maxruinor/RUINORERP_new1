
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/22/2025 18:02:17
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
    /// 审核流程明细表数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_ApprovalProcessDetailQuery), "审核流程明细表数据查询", true)]
    public partial class tb_ApprovalProcessDetailQuery:UserControl
    {
     public tb_ApprovalProcessDetailQuery() {
     
         
        
    
                
        //==============

            InitializeComponent();

       // RuleFor(tb_ApprovalProcessDetail => tb_ApprovalProcessDetail.ApprovalCID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_Approval>(k => k.ApprovalID, v=>v.XXNAME, cmbApprovalID);
        }
        


    }
}


