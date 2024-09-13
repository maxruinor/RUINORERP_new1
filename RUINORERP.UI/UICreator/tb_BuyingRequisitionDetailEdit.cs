
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 11:11:38
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
    /// 请购单明细表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_BuyingRequisitionDetailEdit:UserControl
    {
     public tb_BuyingRequisitionDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_BuyingRequisitionDetail UIToEntity()
        {
        tb_BuyingRequisitionDetail entity = new tb_BuyingRequisitionDetail();
                     entity.PuRequisition_ID = Int64.Parse(txtPuRequisition_ID.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.RequirementDate = DateTime.Parse(txtRequirementDate.Text);
                        entity.property = txtproperty.Text ;
                       entity.ActualRequiredQty = Int32.Parse(txtActualRequiredQty.Text);
                        entity.Quantity = Int32.Parse(txtQuantity.Text);
                        entity.EstimatedPrice = Decimal.Parse(txtEstimatedPrice.Text);
                        entity.DeliveredQuantity = Int32.Parse(txtDeliveredQuantity.Text);
                        entity.Purpose = txtPurpose.Text ;
                       entity.Notes = txtNotes.Text ;
                       entity.Purchased = Boolean.Parse(txtPurchased.Text);
                                return entity;
}
        */

        
        private tb_BuyingRequisitionDetail _EditEntity;
        public tb_BuyingRequisitionDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_BuyingRequisitionDetail entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_BuyingRequisition>(entity, k => k.PuRequisition_ID, v=>v.XXNAME, cmbPuRequisition_ID);
          // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
           DataBindingHelper.BindData4DataTime<tb_BuyingRequisitionDetail>(entity, t => t.RequirementDate, dtpRequirementDate,false);
           DataBindingHelper.BindData4TextBox<tb_BuyingRequisitionDetail>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_BuyingRequisitionDetail>(entity, t => t.ActualRequiredQty, txtActualRequiredQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_BuyingRequisitionDetail>(entity, t => t.Quantity, txtQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_BuyingRequisitionDetail>(entity, t => t.EstimatedPrice.ToString(), txtEstimatedPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_BuyingRequisitionDetail>(entity, t => t.DeliveredQuantity, txtDeliveredQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_BuyingRequisitionDetail>(entity, t => t.Purpose, txtPurpose, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_BuyingRequisitionDetail>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CehckBox<tb_BuyingRequisitionDetail>(entity, t => t.Purchased, chkPurchased, false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



