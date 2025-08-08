
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:41
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
    /// 领料单(包括生产和托工)数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_MaterialRequisitionEdit:UserControl
    {
     public tb_MaterialRequisitionEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_MaterialRequisition UIToEntity()
        {
        tb_MaterialRequisition entity = new tb_MaterialRequisition();
                     entity.MaterialRequisitionNO = txtMaterialRequisitionNO.Text ;
                       entity.MONO = txtMONO.Text ;
                       entity.DeliveryDate = DateTime.Parse(txtDeliveryDate.Text);
                        entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.MOID = Int64.Parse(txtMOID.Text);
                        entity.ProjectGroup_ID = Int64.Parse(txtProjectGroup_ID.Text);
                        entity.ShippingAddress = txtShippingAddress.Text ;
                       entity.shippingWay = txtshippingWay.Text ;
                       entity.TotalPrice = Decimal.Parse(txtTotalPrice.Text);
                        entity.TotalCost = Decimal.Parse(txtTotalCost.Text);
                        entity.ExpectedQuantity = Int32.Parse(txtExpectedQuantity.Text);
                        entity.TotalSendQty = Int32.Parse(txtTotalSendQty.Text);
                        entity.TotalReQty = Int32.Parse(txtTotalReQty.Text);
                        entity.TrackNo = txtTrackNo.Text ;
                       entity.ShipCost = Decimal.Parse(txtShipCost.Text);
                        entity.ReApply = Boolean.Parse(txtReApply.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.ApprovalOpinions = txtApprovalOpinions.Text ;
                       entity.Approver_by = Int64.Parse(txtApprover_by.Text);
                        entity.Approver_at = DateTime.Parse(txtApprover_at.Text);
                        entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                        entity.ApprovalResults = Boolean.Parse(txtApprovalResults.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.GeneEvidence = Boolean.Parse(txtGeneEvidence.Text);
                        entity.Outgoing = Boolean.Parse(txtOutgoing.Text);
                        entity.PrintStatus = Int32.Parse(txtPrintStatus.Text);
                                return entity;
}
        */

        
        private tb_MaterialRequisition _EditEntity;
        public tb_MaterialRequisition EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_MaterialRequisition entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, t => t.MaterialRequisitionNO, txtMaterialRequisitionNO, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, t => t.MONO, txtMONO, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_MaterialRequisition>(entity, t => t.DeliveryDate, dtpDeliveryDate,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, t => t.Location_ID, txtLocation_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, t => t.Employee_ID, txtEmployee_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, t => t.DepartmentID, txtDepartmentID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, t => t.CustomerVendor_ID, txtCustomerVendor_ID, BindDataType4TextBox.Qty,false);
          // DataBindingHelper.BindData4Cmb<tb_ManufacturingOrder>(entity, k => k.MOID, v=>v.XXNAME, cmbMOID);
           DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, t => t.ProjectGroup_ID, txtProjectGroup_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, t => t.ShippingAddress, txtShippingAddress, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, t => t.shippingWay, txtshippingWay, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, t => t.TotalPrice.ToString(), txtTotalPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, t => t.TotalCost.ToString(), txtTotalCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, t => t.ExpectedQuantity, txtExpectedQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, t => t.TotalSendQty, txtTotalSendQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, t => t.TotalReQty, txtTotalReQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, t => t.TrackNo, txtTrackNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, t => t.ShipCost.ToString(), txtShipCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_MaterialRequisition>(entity, t => t.ReApply, chkReApply, false);
           DataBindingHelper.BindData4CheckBox<tb_MaterialRequisition>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4DataTime<tb_MaterialRequisition>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_MaterialRequisition>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_MaterialRequisition>(entity, t => t.Approver_at, dtpApprover_at,false);
           //default  DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_MaterialRequisition>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_MaterialRequisition>(entity, t => t.GeneEvidence, chkGeneEvidence, false);
           DataBindingHelper.BindData4CheckBox<tb_MaterialRequisition>(entity, t => t.Outgoing, chkOutgoing, false);
           DataBindingHelper.BindData4TextBox<tb_MaterialRequisition>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



