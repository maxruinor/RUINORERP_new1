﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/23/2025 23:27:32
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
    /// 收付款方式与账号映射配置表-销售订单收款时付款方式即可指定到收到哪个账号下面数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_PayMethodAccountMapperQuery), "收付款方式与账号映射配置表-销售订单收款时付款方式即可指定到收到哪个账号下面数据查询", true)]
    public partial class tb_PayMethodAccountMapperQuery:UserControl
    {
     public tb_PayMethodAccountMapperQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_PayMethodAccountMapper => tb_PayMethodAccountMapper.PAMID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_FM_Account>(k => k.Account_id, v=>v.XXNAME, cmbAccount_id);
          // DataBindingHelper.InitDataToCmb<tb_PaymentMethod>(k => k.Paytype_ID, v=>v.XXNAME, cmbPaytype_ID);
        }
        


    }
}


