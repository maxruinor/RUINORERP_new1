
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:15
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
    /// 请购单，可能来自销售订单,也可以来自其它日常需求也可能来自生产需求也可以直接录数据，是一个纯业务性的数据表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_BuyingRequisitionEdit:UserControl
    {
     public tb_BuyingRequisitionEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_BuyingRequisition UIToEntity()
        {
        tb_BuyingRequisition entity = new tb_BuyingRequisition();
                     entity.PuRequisitionNo = txtPuRequisitionNo.Text ;
                       entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.RefBillID = Int64.Parse(txtRefBillID.Text);
                        entity.RefBillNO = txtRefBillNO.Text ;
                       entity.RefBizType = Int32.Parse(txtRefBizType.Text);
                        entity.ApplicationDate = DateTime.Parse(txtApplicationDate.Text);
                        entity.RequirementDate = DateTime.Parse(txtRequirementDate.Text);
                        entity.TotalQty = Int32.Parse(txtTotalQty.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.Purpose = txtPurpose.Text ;
                       entity.ApprovalOpinions = txtApprovalOpinions.Text ;
                       entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                        entity.ApprovalResults = Boolean.Parse(txtApprovalResults.Text);
                        entity.Approver_by = Int64.Parse(txtApprover_by.Text);
                        entity.Approver_at = DateTime.Parse(txtApprover_at.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.PrintStatus = Int32.Parse(txtPrintStatus.Text);
                        entity.CloseCaseOpinions = txtCloseCaseOpinions.Text ;
                               return entity;
}
        */

        
        private tb_BuyingRequisition _EditEntity;
        public tb_BuyingRequisition EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_BuyingRequisition entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_BuyingRequisition>(entity, t => t.PuRequisitionNo, txtPuRequisitionNo, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
          // DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
          // DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v=>v.XXNAME, cmbDepartmentID);
           DataBindingHelper.BindData4TextBox<tb_BuyingRequisition>(entity, t => t.RefBillID, txtRefBillID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_BuyingRequisition>(entity, t => t.RefBillNO, txtRefBillNO, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_BuyingRequisition>(entity, t => t.RefBizType, txtRefBizType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_BuyingRequisition>(entity, t => t.ApplicationDate, dtpApplicationDate,false);
           DataBindingHelper.BindData4DataTime<tb_BuyingRequisition>(entity, t => t.RequirementDate, dtpRequirementDate,false);
           DataBindingHelper.BindData4TextBox<tb_BuyingRequisition>(entity, t => t.TotalQty, txtTotalQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_BuyingRequisition>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_BuyingRequisition>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_BuyingRequisition>(entity, t => t.Purpose, txtPurpose, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_BuyingRequisition>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           //default  DataBindingHelper.BindData4TextBox<tb_BuyingRequisition>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_BuyingRequisition>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4TextBox<tb_BuyingRequisition>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_BuyingRequisition>(entity, t => t.Approver_at, dtpApprover_at,false);
           DataBindingHelper.BindData4CheckBox<tb_BuyingRequisition>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4DataTime<tb_BuyingRequisition>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_BuyingRequisition>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_BuyingRequisition>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_BuyingRequisition>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_BuyingRequisition>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_BuyingRequisition>(entity, t => t.CloseCaseOpinions, txtCloseCaseOpinions, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



