
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:55
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
    /// 产品转换单明细数据查询
    /// </summary>
     [MenuAttribute(typeof(tb_ProdConversionDetailQuery), "产品转换单明细数据查询", true)]
    public partial class tb_ProdConversionDetailQuery:UserControl
    {
     public tb_ProdConversionDetailQuery() {
     
         
        
    
                
        //==============

            InitializeComponent();

       // RuleFor(tb_ProdConversionDetail => tb_ProdConversionDetail.ConversionSub_ID).NotEmpty();
       
       
       //===============
       
          
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        
    
        

  
   
   
  }
    

        

        public void LoadDroplistData()
        {
          ProdDetailID_from主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_ProdDetail>(k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
ProdDetailID_to主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_ProdDetail>(k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
Type_ID_from主外字段不一致。Type_ID_to主外字段不一致。          ProdDetailID_from主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_ProdConversion>(k => k.ConversionID, v=>v.XXNAME, cmbConversionID);
ProdDetailID_to主外字段不一致。Type_ID_from主外字段不一致。Type_ID_to主外字段不一致。          ProdDetailID_from主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_ProdDetail>(k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
ProdDetailID_to主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_ProdDetail>(k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
Type_ID_from主外字段不一致。Type_ID_to主外字段不一致。          ProdDetailID_from主外字段不一致。ProdDetailID_to主外字段不一致。Type_ID_from主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_ProductType>(k => k.Type_ID, v=>v.XXNAME, cmbType_ID);
Type_ID_to主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_ProductType>(k => k.Type_ID, v=>v.XXNAME, cmbType_ID);
          ProdDetailID_from主外字段不一致。ProdDetailID_to主外字段不一致。Type_ID_from主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_ProductType>(k => k.Type_ID, v=>v.XXNAME, cmbType_ID);
Type_ID_to主外字段不一致。// DataBindingHelper.InitDataToCmb<tb_ProductType>(k => k.Type_ID, v=>v.XXNAME, cmbType_ID);
        }
        


    }
}


