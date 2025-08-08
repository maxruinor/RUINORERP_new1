
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
    /// 合同明细数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_SO_ContractDetailQuery), "合同明细数据查询", true)]
    public partial class tb_SO_ContractDetailQuery:UserControl
    {
     public tb_SO_ContractDetailQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_SO_ContractDetail => tb_SO_ContractDetail.SOContractSub_ID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_SO_Contract>(k => k.SOContractID, v=>v.XXNAME, cmbSOContractID);
        }
        


    }
}


