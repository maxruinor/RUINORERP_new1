
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:11
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
    /// 批次表 在采购入库时和出库时保存批次ID数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_BatchNumberEdit:UserControl
    {
     public tb_BatchNumberEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_BatchNumber UIToEntity()
        {
        tb_BatchNumber entity = new tb_BatchNumber();
                     entity.BatchNO = txtBatchNO.Text ;
                       entity.采购单号 = txt采购单号.Text ;
                       entity.入库日期 = DateTime.Parse(txt入库日期.Text);
                        entity.供应商 = Int32.Parse(txt供应商.Text);
                        entity.采购单价 = Decimal.Parse(txt采购单价.Text);
                        entity.expiry_date = DateTime.Parse(txtexpiry_date.Text);
                        entity.production_date = DateTime.Parse(txtproduction_date.Text);
                        entity.sale_price = Decimal.Parse(txtsale_price.Text);
                        entity.quantity = Int32.Parse(txtquantity.Text);
                                return entity;
}
        */

        
        private tb_BatchNumber _EditEntity;
        public tb_BatchNumber EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_BatchNumber entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_BatchNumber>(entity, t => t.BatchNO, txtBatchNO, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_BatchNumber>(entity, t => t.采购单号, txt采购单号, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_BatchNumber>(entity, t => t.入库日期, dtp入库日期,false);
           DataBindingHelper.BindData4TextBox<tb_BatchNumber>(entity, t => t.供应商, txt供应商, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_BatchNumber>(entity, t => t.采购单价.ToString(), txt采购单价, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4DataTime<tb_BatchNumber>(entity, t => t.expiry_date, dtpexpiry_date,false);
           DataBindingHelper.BindData4DataTime<tb_BatchNumber>(entity, t => t.production_date, dtpproduction_date,false);
           DataBindingHelper.BindData4TextBox<tb_BatchNumber>(entity, t => t.sale_price.ToString(), txtsale_price, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_BatchNumber>(entity, t => t.quantity, txtquantity, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



