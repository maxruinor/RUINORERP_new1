
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/27/2024 18:04:30
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
    /// 返工退库数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_MRP_ReworkReturnEdit:UserControl
    {
     public tb_MRP_ReworkReturnEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_MRP_ReworkReturn UIToEntity()
        {
        tb_MRP_ReworkReturn entity = new tb_MRP_ReworkReturn();
                     entity.ReworkReturnNo = txtReworkReturnNo.Text ;
                       entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.MOID = Int64.Parse(txtMOID.Text);
                        entity.TotalQty = Int32.Parse(txtTotalQty.Text);
                        entity.TotalReworkFee = Decimal.Parse(txtTotalReworkFee.Text);
                        entity.TotalCost = Decimal.Parse(txtTotalCost.Text);
                        entity.ReturnDate = DateTime.Parse(txtReturnDate.Text);
                        entity.ExpectedReturnDate = DateTime.Parse(txtExpectedReturnDate.Text);
                        entity.ReasonForRework = txtReasonForRework.Text ;
                       entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.ApprovalOpinions = txtApprovalOpinions.Text ;
                       entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                        entity.ApprovalResults = Boolean.Parse(txtApprovalResults.Text);
                        entity.KeepAccountsType = Int32.Parse(txtKeepAccountsType.Text);
                        entity.ReceiptInvoiceClosed = Boolean.Parse(txtReceiptInvoiceClosed.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.Approver_by = Int64.Parse(txtApprover_by.Text);
                        entity.Approver_at = DateTime.Parse(txtApprover_at.Text);
                        entity.PrintStatus = Int32.Parse(txtPrintStatus.Text);
                        entity.GenerateVouchers = Boolean.Parse(txtGenerateVouchers.Text);
                        entity.VoucherID = Int64.Parse(txtVoucherID.Text);
                                return entity;
}
        */

        
        private tb_MRP_ReworkReturn _EditEntity;
        public tb_MRP_ReworkReturn EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_MRP_ReworkReturn entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_MRP_ReworkReturn>(entity, t => t.ReworkReturnNo, txtReworkReturnNo, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
          // DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v=>v.XXNAME, cmbDepartmentID);
          // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
          // DataBindingHelper.BindData4Cmb<tb_ManufacturingOrder>(entity, k => k.MOID, v=>v.XXNAME, cmbMOID);
           DataBindingHelper.BindData4TextBox<tb_MRP_ReworkReturn>(entity, t => t.TotalQty, txtTotalQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_MRP_ReworkReturn>(entity, t => t.TotalReworkFee.ToString(), txtTotalReworkFee, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_MRP_ReworkReturn>(entity, t => t.TotalCost.ToString(), txtTotalCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4DataTime<tb_MRP_ReworkReturn>(entity, t => t.ReturnDate, dtpReturnDate,false);
           DataBindingHelper.BindData4DataTime<tb_MRP_ReworkReturn>(entity, t => t.ExpectedReturnDate, dtpExpectedReturnDate,false);
           DataBindingHelper.BindData4TextBox<tb_MRP_ReworkReturn>(entity, t => t.ReasonForRework, txtReasonForRework, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_MRP_ReworkReturn>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4DataTime<tb_MRP_ReworkReturn>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_MRP_ReworkReturn>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_MRP_ReworkReturn>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_MRP_ReworkReturn>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_MRP_ReworkReturn>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_MRP_ReworkReturn>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           //default  DataBindingHelper.BindData4TextBox<tb_MRP_ReworkReturn>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_MRP_ReworkReturn>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4TextBox<tb_MRP_ReworkReturn>(entity, t => t.KeepAccountsType, txtKeepAccountsType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_MRP_ReworkReturn>(entity, t => t.ReceiptInvoiceClosed, chkReceiptInvoiceClosed, false);
           DataBindingHelper.BindData4TextBox<tb_MRP_ReworkReturn>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_MRP_ReworkReturn>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_MRP_ReworkReturn>(entity, t => t.Approver_at, dtpApprover_at,false);
           DataBindingHelper.BindData4TextBox<tb_MRP_ReworkReturn>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_MRP_ReworkReturn>(entity, t => t.GenerateVouchers, chkGenerateVouchers, false);
           DataBindingHelper.BindData4TextBox<tb_MRP_ReworkReturn>(entity, t => t.VoucherID, txtVoucherID, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



