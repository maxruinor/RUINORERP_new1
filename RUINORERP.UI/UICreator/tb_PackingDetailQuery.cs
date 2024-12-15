
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/14/2024 16:49:17
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
    /// 包装清单数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_PackingDetailQuery), "包装清单数据查询", true)]
    public partial class tb_PackingDetailQuery:UserControl
    {
     public tb_PackingDetailQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_PackingDetail => tb_PackingDetail.PackDetail_ID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_ProdDetail>(k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
          // DataBindingHelper.InitDataToCmb<tb_Packing>(k => k.Pack_ID, v=>v.XXNAME, cmbPack_ID);
        }
        


    }
}


