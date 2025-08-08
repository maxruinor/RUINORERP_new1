
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:23
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
    /// 成品入库单 要进一步完善数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_FinishedGoodsInvEdit:UserControl
    {
     public tb_FinishedGoodsInvEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_FinishedGoodsInv UIToEntity()
        {
        tb_FinishedGoodsInv entity = new tb_FinishedGoodsInv();
                     entity.DeliveryBillNo = txtDeliveryBillNo.Text ;
                       entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.DeliveryDate = DateTime.Parse(txtDeliveryDate.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.IsOutSourced = Boolean.Parse(txtIsOutSourced.Text);
                        entity.ShippingWay = txtShippingWay.Text ;
                       entity.TrackNo = txtTrackNo.Text ;
                       entity.MONo = txtMONo.Text ;
                       entity.MOID = Int64.Parse(txtMOID.Text);
                        entity.TotalQty = Int32.Parse(txtTotalQty.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.ApprovalOpinions = txtApprovalOpinions.Text ;
                       entity.Approver_by = Int64.Parse(txtApprover_by.Text);
                        entity.Approver_at = DateTime.Parse(txtApprover_at.Text);
                        entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                        entity.ApprovalResults = Boolean.Parse(txtApprovalResults.Text);
                        entity.GeneEvidence = Boolean.Parse(txtGeneEvidence.Text);
                        entity.TotalNetMachineHours = Decimal.Parse(txtTotalNetMachineHours.Text);
                        entity.TotalNetWorkingHours = Decimal.Parse(txtTotalNetWorkingHours.Text);
                        entity.TotalApportionedCost = Decimal.Parse(txtTotalApportionedCost.Text);
                        entity.TotalManuFee = Decimal.Parse(txtTotalManuFee.Text);
                        entity.TotalProductionCost = Decimal.Parse(txtTotalProductionCost.Text);
                        entity.TotalMaterialCost = Decimal.Parse(txtTotalMaterialCost.Text);
                        entity.PrintStatus = Int32.Parse(txtPrintStatus.Text);
                                return entity;
}
        */

        
        private tb_FinishedGoodsInv _EditEntity;
        public tb_FinishedGoodsInv EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_FinishedGoodsInv entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInv>(entity, t => t.DeliveryBillNo, txtDeliveryBillNo, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
          // DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v=>v.XXNAME, cmbDepartmentID);
          // DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
           DataBindingHelper.BindData4DataTime<tb_FinishedGoodsInv>(entity, t => t.DeliveryDate, dtpDeliveryDate,false);
           DataBindingHelper.BindData4DataTime<tb_FinishedGoodsInv>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInv>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_FinishedGoodsInv>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInv>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInv>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_FinishedGoodsInv>(entity, t => t.IsOutSourced, chkIsOutSourced, false);
           DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInv>(entity, t => t.ShippingWay, txtShippingWay, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInv>(entity, t => t.TrackNo, txtTrackNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInv>(entity, t => t.MONo, txtMONo, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_ManufacturingOrder>(entity, k => k.MOID, v=>v.XXNAME, cmbMOID);
           DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInv>(entity, t => t.TotalQty, txtTotalQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_FinishedGoodsInv>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInv>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInv>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInv>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_FinishedGoodsInv>(entity, t => t.Approver_at, dtpApprover_at,false);
           //default  DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInv>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_FinishedGoodsInv>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4CheckBox<tb_FinishedGoodsInv>(entity, t => t.GeneEvidence, chkGeneEvidence, false);
           DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInv>(entity, t => t.TotalNetMachineHours.ToString(), txtTotalNetMachineHours, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInv>(entity, t => t.TotalNetWorkingHours.ToString(), txtTotalNetWorkingHours, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInv>(entity, t => t.TotalApportionedCost.ToString(), txtTotalApportionedCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInv>(entity, t => t.TotalManuFee.ToString(), txtTotalManuFee, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInv>(entity, t => t.TotalProductionCost.ToString(), txtTotalProductionCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInv>(entity, t => t.TotalMaterialCost.ToString(), txtTotalMaterialCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FinishedGoodsInv>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



