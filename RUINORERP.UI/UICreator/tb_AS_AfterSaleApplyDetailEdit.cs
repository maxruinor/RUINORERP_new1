
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/09/2025 13:49:58
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
    /// 售后申请单明细数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_AS_AfterSaleApplyDetailEdit:UserControl
    {
     public tb_AS_AfterSaleApplyDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_AS_AfterSaleApplyDetail UIToEntity()
        {
        tb_AS_AfterSaleApplyDetail entity = new tb_AS_AfterSaleApplyDetail();
                     entity.ASApplyID = Int64.Parse(txtASApplyID.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.property = txtproperty.Text ;
                       entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.FaultDescription = txtFaultDescription.Text ;
                       entity.InitialQuantity = Int32.Parse(txtInitialQuantity.Text);
                        entity.ConfirmedQuantity = Int32.Parse(txtConfirmedQuantity.Text);
                        entity.CustomerPartNo = txtCustomerPartNo.Text ;
                       entity.DeliveredQty = Int32.Parse(txtDeliveredQty.Text);
                        entity.Summary = txtSummary.Text ;
                               return entity;
}
        */

        
        private tb_AS_AfterSaleApplyDetail _EditEntity;
        public tb_AS_AfterSaleApplyDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_AS_AfterSaleApplyDetail entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_AS_AfterSaleApply>(entity, k => k.ASApplyID, v=>v.XXNAME, cmbASApplyID);
          // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApplyDetail>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApplyDetail>(entity, t => t.FaultDescription, txtFaultDescription, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApplyDetail>(entity, t => t.InitialQuantity, txtInitialQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApplyDetail>(entity, t => t.ConfirmedQuantity, txtConfirmedQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApplyDetail>(entity, t => t.CustomerPartNo, txtCustomerPartNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApplyDetail>(entity, t => t.DeliveredQty, txtDeliveredQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleApplyDetail>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



