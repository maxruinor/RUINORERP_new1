﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/20/2024 20:30:05
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
    /// 拆分单明细数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_ProdSplitDetailQuery), "拆分单明细数据查询", true)]
    public partial class tb_ProdSplitDetailQuery:UserControl
    {
     public tb_ProdSplitDetailQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_ProdSplitDetail => tb_ProdSplitDetail.SplitSub_ID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_Location>(k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
          // DataBindingHelper.InitDataToCmb<tb_ProdDetail>(k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
          // DataBindingHelper.InitDataToCmb<tb_ProdSplit>(k => k.SplitID, v=>v.XXNAME, cmbSplitID);
        }
        


    }
}


