
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:08
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
    /// 采购退回单 当采购发生退回时，您可以将它记录在本系统的采购退回作业中。在实际业务中虽然发生了采购但实际货物却还未入库，采购退回作业可退回订金、退回数量处理。采购退回单可以由采购订单转入，也可以手动录入新增单据,一般没有金额变化的，可以直接作废采购单。有订单等才需要做退回数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_PurOrderReEdit:UserControl
    {
     public tb_PurOrderReEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_PurOrderRe UIToEntity()
        {
        tb_PurOrderRe entity = new tb_PurOrderRe();
                     entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.PurOrder_ID = Int64.Parse(txtPurOrder_ID.Text);
                        entity.PurOrderNo = txtPurOrderNo.Text ;
                       entity.ReturnDate = DateTime.Parse(txtReturnDate.Text);
                        entity.PurReturnNo = txtPurReturnNo.Text ;
                       entity.GetPayment = Decimal.Parse(txtGetPayment.Text);
                        entity.TotalTaxAmount = Decimal.Parse(txtTotalTaxAmount.Text);
                        entity.TotalAmount = Decimal.Parse(txtTotalAmount.Text);
                        entity.DeliveryDate = DateTime.Parse(txtDeliveryDate.Text);
                        entity.ReturnAddress = txtReturnAddress.Text ;
                       entity.ShippingWay = txtShippingWay.Text ;
                       entity.TrackNo = txtTrackNo.Text ;
                       entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.ApprovalOpinions = txtApprovalOpinions.Text ;
                       entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                        entity.ApprovalResults = Boolean.Parse(txtApprovalResults.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.Approver_by = Int64.Parse(txtApprover_by.Text);
                        entity.Approver_at = DateTime.Parse(txtApprover_at.Text);
                        entity.PrintStatus = Int32.Parse(txtPrintStatus.Text);
                        entity.GenerateVouchers = Boolean.Parse(txtGenerateVouchers.Text);
                        entity.TotalQty = Decimal.Parse(txtTotalQty.Text);
                                return entity;
}
        */

        
        private tb_PurOrderRe _EditEntity;
        public tb_PurOrderRe EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_PurOrderRe entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
          // DataBindingHelper.BindData4Cmb<tb_PurOrder>(entity, k => k.PurOrder_ID, v=>v.XXNAME, cmbPurOrder_ID);
           DataBindingHelper.BindData4TextBox<tb_PurOrderRe>(entity, t => t.PurOrderNo, txtPurOrderNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_PurOrderRe>(entity, t => t.ReturnDate, dtpReturnDate,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderRe>(entity, t => t.PurReturnNo, txtPurReturnNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderRe>(entity, t => t.GetPayment.ToString(), txtGetPayment, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderRe>(entity, t => t.TotalTaxAmount.ToString(), txtTotalTaxAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderRe>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4DataTime<tb_PurOrderRe>(entity, t => t.DeliveryDate, dtpDeliveryDate,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderRe>(entity, t => t.ReturnAddress, txtReturnAddress, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderRe>(entity, t => t.ShippingWay, txtShippingWay, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderRe>(entity, t => t.TrackNo, txtTrackNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_PurOrderRe>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4DataTime<tb_PurOrderRe>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderRe>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_PurOrderRe>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderRe>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderRe>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderRe>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           //default  DataBindingHelper.BindData4TextBox<tb_PurOrderRe>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_PurOrderRe>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderRe>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderRe>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_PurOrderRe>(entity, t => t.Approver_at, dtpApprover_at,false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderRe>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_PurOrderRe>(entity, t => t.GenerateVouchers, chkGenerateVouchers, false);
           DataBindingHelper.BindData4TextBox<tb_PurOrderRe>(entity, t => t.TotalQty.ToString(), txtTotalQty, BindDataType4TextBox.Money,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



