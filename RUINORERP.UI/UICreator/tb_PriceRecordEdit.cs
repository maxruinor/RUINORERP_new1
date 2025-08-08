
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:50
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
    /// 价格记录表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_PriceRecordEdit:UserControl
    {
     public tb_PriceRecordEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        

         }
/*
        
        tb_PriceRecord UIToEntity()
        {
        tb_PriceRecord entity = new tb_PriceRecord();
                     entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.PurDate = DateTime.Parse(txtPurDate.Text);
                        entity.SaleDate = DateTime.Parse(txtSaleDate.Text);
                        entity.PurPrice = Decimal.Parse(txtPurPrice.Text);
                        entity.SalePrice = Decimal.Parse(txtSalePrice.Text);
                                return entity;
}
        */

        
        private tb_PriceRecord _EditEntity;
        public tb_PriceRecord EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_PriceRecord entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
          // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
           DataBindingHelper.BindData4DataTime<tb_PriceRecord>(entity, t => t.PurDate, dtpPurDate,false);
           DataBindingHelper.BindData4DataTime<tb_PriceRecord>(entity, t => t.SaleDate, dtpSaleDate,false);
           DataBindingHelper.BindData4TextBox<tb_PriceRecord>(entity, t => t.PurPrice.ToString(), txtPurPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PriceRecord>(entity, t => t.SalePrice.ToString(), txtSalePrice, BindDataType4TextBox.Money,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



