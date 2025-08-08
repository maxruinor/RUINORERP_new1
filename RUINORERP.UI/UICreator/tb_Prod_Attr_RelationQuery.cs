
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:52
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
    /// 产品主次及属性关系表数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_Prod_Attr_RelationQuery), "产品主次及属性关系表数据查询", true)]
    public partial class tb_Prod_Attr_RelationQuery:UserControl
    {
     public tb_Prod_Attr_RelationQuery() {
     
         
                
        //==============

            InitializeComponent();

       // RuleFor(tb_Prod_Attr_Relation => tb_Prod_Attr_Relation.RAR_ID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          // DataBindingHelper.InitDataToCmb<tb_Prod>(k => k.ProdBaseID, v=>v.XXNAME, cmbProdBaseID);
          // DataBindingHelper.InitDataToCmb<tb_ProdDetail>(k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
          // DataBindingHelper.InitDataToCmb<tb_ProdPropertyValue>(k => k.PropertyValueID, v=>v.XXNAME, cmbPropertyValueID);
          // DataBindingHelper.InitDataToCmb<tb_ProdProperty>(k => k.Property_ID, v=>v.XXNAME, cmbProperty_ID);
        }
        


    }
}


