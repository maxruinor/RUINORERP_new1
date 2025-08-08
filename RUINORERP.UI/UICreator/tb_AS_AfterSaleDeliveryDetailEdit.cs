
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:07
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
    /// 售后交付明细数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_AS_AfterSaleDeliveryDetailEdit:UserControl
    {
     public tb_AS_AfterSaleDeliveryDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_AS_AfterSaleDeliveryDetail UIToEntity()
        {
        tb_AS_AfterSaleDeliveryDetail entity = new tb_AS_AfterSaleDeliveryDetail();
                     entity.ASDeliveryID = Int64.Parse(txtASDeliveryID.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.property = txtproperty.Text ;
                       entity.Quantity = Int32.Parse(txtQuantity.Text);
                        entity.SaleFlagCode = txtSaleFlagCode.Text ;
                       entity.Summary = txtSummary.Text ;
                       entity.CustomerPartNo = txtCustomerPartNo.Text ;
                       entity.ASApplyDetailID = Int64.Parse(txtASApplyDetailID.Text);
                                return entity;
}
        */

        
        private tb_AS_AfterSaleDeliveryDetail _EditEntity;
        public tb_AS_AfterSaleDeliveryDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_AS_AfterSaleDeliveryDetail entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_AS_AfterSaleDelivery>(entity, k => k.ASDeliveryID, v=>v.XXNAME, cmbASDeliveryID);
          // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
          // DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleDeliveryDetail>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleDeliveryDetail>(entity, t => t.Quantity, txtQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleDeliveryDetail>(entity, t => t.SaleFlagCode, txtSaleFlagCode, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleDeliveryDetail>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleDeliveryDetail>(entity, t => t.CustomerPartNo, txtCustomerPartNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleDeliveryDetail>(entity, t => t.ASApplyDetailID, txtASApplyDetailID, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



