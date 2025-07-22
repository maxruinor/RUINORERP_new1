
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/22/2025 18:02:23
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
    /// 售后交付单数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_AS_AfterSaleDeliveryEdit:UserControl
    {
     public tb_AS_AfterSaleDeliveryEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_AS_AfterSaleDelivery UIToEntity()
        {
        tb_AS_AfterSaleDelivery entity = new tb_AS_AfterSaleDelivery();
                     entity.ASDeliveryNo = txtASDeliveryNo.Text ;
                       entity.ASApplyID = Int64.Parse(txtASApplyID.Text);
                        entity.ASApplyNo = txtASApplyNo.Text ;
                       entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.ProjectGroup_ID = Int64.Parse(txtProjectGroup_ID.Text);
                        entity.TotalDeliveryQty = Int32.Parse(txtTotalDeliveryQty.Text);
                        entity.DeliveryDate = DateTime.Parse(txtDeliveryDate.Text);
                        entity.ShippingAddress = txtShippingAddress.Text ;
                       entity.ShippingWay = txtShippingWay.Text ;
                       entity.TrackNo = txtTrackNo.Text ;
                       entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.ApprovalOpinions = txtApprovalOpinions.Text ;
                       entity.Approver_by = Int64.Parse(txtApprover_by.Text);
                        entity.Approver_at = DateTime.Parse(txtApprover_at.Text);
                        entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                        entity.ApprovalResults = Boolean.Parse(txtApprovalResults.Text);
                        entity.PrintStatus = Int32.Parse(txtPrintStatus.Text);
                        entity.Notes = txtNotes.Text ;
                               return entity;
}
        */

        
        private tb_AS_AfterSaleDelivery _EditEntity;
        public tb_AS_AfterSaleDelivery EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_AS_AfterSaleDelivery entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleDelivery>(entity, t => t.ASDeliveryNo, txtASDeliveryNo, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_AS_AfterSaleApply>(entity, k => k.ASApplyID, v=>v.XXNAME, cmbASApplyID);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleDelivery>(entity, t => t.ASApplyNo, txtASApplyNo, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
          // DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
          // DataBindingHelper.BindData4Cmb<tb_ProjectGroup>(entity, k => k.ProjectGroup_ID, v=>v.XXNAME, cmbProjectGroup_ID);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleDelivery>(entity, t => t.TotalDeliveryQty, txtTotalDeliveryQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_AS_AfterSaleDelivery>(entity, t => t.DeliveryDate, dtpDeliveryDate,false);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleDelivery>(entity, t => t.ShippingAddress, txtShippingAddress, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleDelivery>(entity, t => t.ShippingWay, txtShippingWay, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleDelivery>(entity, t => t.TrackNo, txtTrackNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_AS_AfterSaleDelivery>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4DataTime<tb_AS_AfterSaleDelivery>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleDelivery>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_AS_AfterSaleDelivery>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleDelivery>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleDelivery>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleDelivery>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleDelivery>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_AS_AfterSaleDelivery>(entity, t => t.Approver_at, dtpApprover_at,false);
           //default  DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleDelivery>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_AS_AfterSaleDelivery>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleDelivery>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_AS_AfterSaleDelivery>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



