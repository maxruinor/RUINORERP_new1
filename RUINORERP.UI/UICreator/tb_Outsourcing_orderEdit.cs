
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:46
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
    /// 外发加工订单表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_Outsourcing_orderEdit:UserControl
    {
     public tb_Outsourcing_orderEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        

         }
/*
        
        tb_Outsourcing_order UIToEntity()
        {
        tb_Outsourcing_order entity = new tb_Outsourcing_order();
                     entity.Quantity = Int32.Parse(txtQuantity.Text);
                        entity.Unit_price = Decimal.Parse(txtUnit_price.Text);
                        entity.Total_amount = Decimal.Parse(txtTotal_amount.Text);
                        entity.Status = Int32.Parse(txtStatus.Text);
                        entity.Order_date = DateTime.Parse(txtOrder_date.Text);
                        entity.Delivery_date = DateTime.Parse(txtDelivery_date.Text);
                                return entity;
}
        */

        
        private tb_Outsourcing_order _EditEntity;
        public tb_Outsourcing_order EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_Outsourcing_order entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_Outsourcing_order>(entity, t => t.Quantity, txtQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_Outsourcing_order>(entity, t => t.Unit_price.ToString(), txtUnit_price, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_Outsourcing_order>(entity, t => t.Total_amount.ToString(), txtTotal_amount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_Outsourcing_order>(entity, t => t.Status, txtStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_Outsourcing_order>(entity, t => t.Order_date, dtpOrder_date,false);
           DataBindingHelper.BindData4DataTime<tb_Outsourcing_order>(entity, t => t.Delivery_date, dtpDelivery_date,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



